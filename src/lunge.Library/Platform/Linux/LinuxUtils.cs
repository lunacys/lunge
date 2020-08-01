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
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Reflection;

namespace lunge.Library.Platform.Linux
{
    public class LinuxUtils : Utils
    {
        public override void HighlightInFileManager(string path)
        {
            // There isn't really a standard way of doing this on Linux, so fall back to just opening the containing
            // folder.
            OpenNatively(Path.GetDirectoryName(path));
        }

        public override void OpenNatively(string path)
        {
            try
            {
                // Try opening via xdg-open.
                Process.Start("xdg-open", path);
            }
            catch (Win32Exception)
            {
                // No xdg-open? Oh well.
            }
        }

        public override void RegisterURIScheme(string scheme, string friendlyName)
        {
            // This returns Something.dll, on Linux the published executable is usually called Something.
            var applicationLocation = Path.ChangeExtension(Assembly.GetEntryAssembly().Location, null);

            var dataHome = Environment.GetEnvironmentVariable("XDG_DATA_HOME") ?? Path.Combine(Environment.GetEnvironmentVariable("HOME"), ".local/share");
            var desktopFilePath = Path.Combine(dataHome, $"applications/{friendlyName}.desktop");

            // Make a .desktop file.
            string[] contents = {
                "[Desktop Entry]",
                $"Name={friendlyName}",
                $"Exec={applicationLocation} %u",
                "Type=Application",
                "NoDisplay=true",
                $"MimeType=x-scheme-handler/{scheme};"
            };
            File.WriteAllLines(desktopFilePath, contents);

            // Register it as the handler.
            try
            {
                Process.Start("xdg-mime", $"default {friendlyName}.desktop x-scheme-handler/{scheme}");
            }
            catch (Win32Exception)
            {
                // No xdg-mime? Oh well.
            }
        }
    }
}