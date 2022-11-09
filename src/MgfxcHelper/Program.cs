namespace MgfxcHelper
{
    class Program
    {
        private static Dictionary<string, string> _paramMap = new Dictionary<string, string>();

        private static readonly string DirectoryParam = "/Directory:";
        private static readonly string PlatformParam = "/Platform:";
        
        static void Main(string[] args)
        {
            for (int i = 0; i < args.Length; i += 2)
            {
                var name = args[i];
                var value = args[i + 1];

                _paramMap[name] = value;
            }

            var files = Directory.EnumerateFiles(_paramMap[DirectoryParam], "*.fx", SearchOption.AllDirectories);
            foreach (var file in files)
            {
                var fullPath = Path.GetFullPath(file).Replace("\\", "/");
                var path = Path.GetRelativePath(".", file);
                var noEx = Path.GetFileNameWithoutExtension(file);
                var procStr = $"mgfxc {file.Replace("\\", "/")} {file.Replace(".fx", ".mgfxo")} /Profile:{_paramMap[PlatformParam]}";
                Console.WriteLine(procStr);
            }
            

            Console.WriteLine();   
        }
    }
}