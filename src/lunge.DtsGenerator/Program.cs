namespace lunge.DtsGenerator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            /*Console.WriteLine("Started Generator");

            var generator = new GeneratorTests();

            var sw = new Stopwatch();
            sw.Start();
            generator.SaveGenerated();
            sw.Stop();

            Console.WriteLine($"Generation Done. Time Elapsed: {sw.ElapsedMilliseconds} ms.");*/

            using var game = new GameRoot();
            game.Run();
        }
    }
}