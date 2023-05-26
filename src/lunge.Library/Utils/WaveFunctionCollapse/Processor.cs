using System.Xml.Linq;
using System;
using System.IO;

namespace lunge.Library.Utils.WaveFunctionCollapse;

public class Processor
{
    public void Process(string filename)
    {
        var folder = Directory.CreateDirectory("output");
        foreach (var file in folder.GetFiles()) file.Delete();

        Random random = new();
        XDocument xdoc = XDocument.Load(filename);

        foreach (XElement xelem in xdoc.Root.Elements("overlapping", "simpletiled"))
        {
            ModelBase model;
            string name = xelem.Get<string>("name");
            Console.WriteLine($"< {name}");

            bool isOverlapping = xelem.Name == "overlapping";
            int size = xelem.Get("size", isOverlapping ? 48 : 24);
            int width = xelem.Get("width", size);
            int height = xelem.Get("height", size);
            bool periodic = xelem.Get("periodic", false);
            string heuristicString = xelem.Get<string>("heuristic");
            var heuristic = heuristicString == "Scanline" ? ModelBase.Heuristic.Scanline : (heuristicString == "MRV" ? ModelBase.Heuristic.MRV : ModelBase.Heuristic.Entropy);

            if (isOverlapping)
            {
                int N = xelem.Get("N", 3);
                bool periodicInput = xelem.Get("periodicInput", true);
                int symmetry = xelem.Get("symmetry", 8);
                bool ground = xelem.Get("ground", false);

                model = new OverlappingModel(name, N, width, height, periodicInput, periodic, symmetry, ground, heuristic);
            }
            else
            {
                string subset = xelem.Get<string>("subset");
                bool blackBackground = xelem.Get("blackBackground", false);

                model = new SimpleTiledModel(name, subset, width, height, periodic, blackBackground, heuristic);
            }

            for (int i = 0; i < xelem.Get("screenshots", 2); i++)
            {
                for (int k = 0; k < 10; k++)
                {
                    Console.Write("> ");
                    int seed = random.Next();
                    bool success = model.Run(seed, xelem.Get("limit", -1));
                    if (success)
                    {
                        Console.WriteLine("DONE");
                        model.Save($"output/{name} {seed}.png");
                        if (model is SimpleTiledModel stmodel && xelem.Get("textOutput", false))
                            System.IO.File.WriteAllText($"output/{name} {seed}.txt", stmodel.TextOutput());
                        break;
                    }
                    else Console.WriteLine("CONTRADICTION");
                }
            }
        }
    }
}