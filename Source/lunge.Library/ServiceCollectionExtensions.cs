using System;
using System.ComponentModel.Composition.Hosting;
using System.ComponentModel.Composition.Primitives;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Microsoft.Extensions.DependencyInjection;

namespace lunge.Library
{
    public static class ServiceCollectionExtensions
    {
        public static void AddGameConfiguration(this IServiceCollection services)
        {
            // your base DI def here
            Console.WriteLine("Adding game DI!");
        }

        public static void AddPlugins(this IServiceCollection services)
        {
            var path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

            var dirCat = new DirectoryCatalog(path);
            var importDef = BuildImportDefinition();

            try
            {
                using var aggregateCatalog = new AggregateCatalog();
                aggregateCatalog.Catalogs.Add(dirCat);

                using var compositionContainer = new CompositionContainer(aggregateCatalog);
                var exports = compositionContainer.GetExports(importDef);

                foreach (var module in exports
                    .Select(x => x.Value as IStartupProvider)
                    .Where(x => x != null))
                {
                    module.ConfigureServices(services);
                }
            }
            catch (ReflectionTypeLoadException e)
            {
                var builder = new StringBuilder();
                foreach (var loaderException in e.LoaderExceptions)
                {
                    builder.AppendFormat("{0}\n", loaderException.Message);
                }

                throw new TypeLoadException(builder.ToString(), e);
            }
        }

        private static ImportDefinition BuildImportDefinition()
        {
            return new ImportDefinition(x => true, typeof(IStartupProvider).FullName, ImportCardinality.ZeroOrMore, false, false);
        }
    }
}