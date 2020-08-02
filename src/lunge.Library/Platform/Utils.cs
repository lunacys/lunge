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
using System.Runtime.InteropServices;
using lunge.Library.Platform.Linux;
using lunge.Library.Platform.Windows;

namespace lunge.Library.Platform
{
    public abstract class Utils
    {
        public static Utils NativeUtils
        {
            get
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return new WindowsUtils();
                }

                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return new LinuxUtils();
                }

                throw new NotImplementedException();
            }
        }

        /// <summary>
        ///     Opens a native file manager highlighting a file.
        /// </summary>
        /// <param name="path">path to the file to highlight</param>
        public abstract void HighlightInFileManager(string path);

        /// <summary>
        ///     Opens a file or folder natively. Folders are opened with a native file manager and files are opened
        ///     according to the native associations.
        /// </summary>
        /// <param name="path">path to the file or folder to open</param>
        public abstract void OpenNatively(string path);

        /// <summary>
        ///     Registers the current executable as a URI scheme handler.
        /// </summary>
        /// <param name="scheme">the url scheme to register, MUST NOT contain spaces, quotes or any other weird characters</param>
        /// <param name="friendlyName">a friendly name for the scheme, MUST NOT contain spaces, quotes or any other weird characters</param>
        public abstract void RegisterURIScheme(string scheme, string friendlyName);
    }
}