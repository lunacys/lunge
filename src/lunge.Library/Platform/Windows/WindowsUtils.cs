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

using System.Diagnostics;
using System.IO;
using System.Reflection;
using Microsoft.Win32;

namespace lunge.Library.Platform.Windows
{
    public class WindowsUtils : Utils
    {
        public override void HighlightInFileManager(string path)
        {
            Process.Start("explorer.exe", "/select, \"" + path.Replace("/", "\\") + "\"");
        }

        public override void OpenNatively(string path)
        {
            Process.Start("explorer.exe", "\"" + path.Replace("/", "\\") + "\"");
        }

        public override void RegisterURIScheme(string scheme, string friendlyName)
        {
            using (var key = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\" + scheme))
            {
                // This returns Something.dll, on Windows the published executable is usually called Something.exe.
                var applicationLocation = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, "exe");

                key.SetValue("", "URL:" + friendlyName);
                key.SetValue("URL Protocol", "");

                using (var defaultIcon = key.CreateSubKey("DefaultIcon"))
                {
                    defaultIcon.SetValue("", applicationLocation + ",1");
                }

                using (var commandKey = key.CreateSubKey(@"shell\open\command"))
                {
                    commandKey.SetValue("", "\"" + applicationLocation + "\" \"%1\"");
                }
            }
        }
    }
}