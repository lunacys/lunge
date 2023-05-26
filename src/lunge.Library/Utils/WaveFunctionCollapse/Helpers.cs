using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace lunge.Library.Utils.WaveFunctionCollapse;

public static class Helpers
{
    public static int Random(this double[] weights, double r)
    {
        double sum = 0;
        for (int i = 0; i < weights.Length; i++) sum += weights[i];
        double threshold = r * sum;

        double partialSum = 0;
        for (int i = 0; i < weights.Length; i++)
        {
            partialSum += weights[i];
            if (partialSum >= threshold) return i;
        }
        return 0;
    }

    public static T? Get<T>(this XElement xelem, string attribute, T? defaultT = default)
    {
        var a = xelem.Attribute(attribute);

        if (a == null) 
            return defaultT;

        var type = typeof(T);
        var converter = TypeDescriptor.GetConverter(type);

        var res = converter.ConvertFromInvariantString(a.Value);
        if (res == null)
            return defaultT;

        return (T)res;
    }

    public static IEnumerable<XElement> Elements(this XElement xelement, params string[] names) => xelement.Elements().Where(e => names.Any(n => n == e.Name));
}

public static class BitmapHelper
{
    public static (int[], int, int) LoadBitmap(string filename)
    {
        throw new NotImplementedException();
        /*using var image = Image.Load<Bgra32>(filename);
        int width = image.Width, height = image.Height;
        int[] result = new int[width * height];
        image.CopyPixelDataTo(MemoryMarshal.Cast<int, Bgra32>(result));
        return (result, width, height);*/
    }

    public static void SaveBitmap(int[] data, int width, int height, string filename)
    {
        throw new NotImplementedException();
        /*fixed (int* pData = data)
        {
            using var image = Image.WrapMemory<Bgra32>(pData, width, height);
            image.SaveAsPng(filename);
        }*/
    }
}
