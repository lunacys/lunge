// MIT License

// Copyright (c) 2018 Swan

// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:

// The above copyright notice and this permission notice shall be included in all
// copies or substantial portions of the Software.

// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
// SOFTWARE.

using System;
using System.IO;
using lunge.Library.Debugging.Logging;

namespace lunge.Library.Platform
{
    public static class NativeAssemblies
    {
        /// <summary>
        ///     The architecture of the CPU.
        /// </summary>
        public static string Architecture => IntPtr.Size == 4 ? "x86" : "x64";

        /// <summary>
        ///     The base directory of the executable.
        /// </summary>
        public static string BaseDirectory => AppDomain.CurrentDomain.BaseDirectory;

        /// <summary>
        ///     The directory of all native libs.
        /// </summary>
        public static string NativeDirectory => $"{BaseDirectory}/{Architecture}/";

        /// <summary>
        ///     Copies native assemblies to the executing path
        /// </summary>
        public static void Copy()
        {
            foreach (var file in Directory.GetFiles(NativeDirectory))
            {
                try
                {
                    var path = Path.Combine(BaseDirectory, Path.GetFileName(file));

                    if (!File.Exists(path))
                        File.Copy(file, path, true);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
