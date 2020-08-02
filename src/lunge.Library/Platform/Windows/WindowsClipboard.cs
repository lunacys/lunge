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
using System.Text;

namespace lunge.Library.Platform.Windows
{
    /// <inheritdoc />
    /// <summary>
    ///     https://github.com/ppy/osu-framework/blob/master/osu.Framework/Platform/Windows/WindowsClipboard.cs
    /// </summary>
    public class WindowsClipboard : Clipboard
    {
        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool IsClipboardFormatAvailable(uint format);

        [DllImport("User32.dll")]
        private static extern IntPtr GetClipboardData(uint uFormat);

        [DllImport("user32.dll")]
        private static extern IntPtr SetClipboardData(uint uFormat, IntPtr hMem);

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool OpenClipboard(IntPtr hWndNewOwner);

        [DllImport("user32.dll")]
        private static extern bool EmptyClipboard();

        [DllImport("User32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool CloseClipboard();

        [DllImport("Kernel32.dll")]
        private static extern IntPtr GlobalLock(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalAlloc(uint uFlags, UIntPtr dwBytes);

        [DllImport("Kernel32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool GlobalUnlock(IntPtr hMem);

        [DllImport("Kernel32.dll")]
        private static extern int GlobalSize(IntPtr hMem);

        [DllImport("kernel32.dll")]
        private static extern IntPtr GlobalFree(IntPtr hMem);

        private const uint cf_unicodetext = 13U;

        public override string GetText()
        {
            if (!IsClipboardFormatAvailable(cf_unicodetext))
                return null;

            try
            {
                if (!OpenClipboard(IntPtr.Zero))
                    return null;

                var handle = GetClipboardData(cf_unicodetext);
                if (handle == IntPtr.Zero)
                    return null;

                var pointer = IntPtr.Zero;

                try
                {
                    pointer = GlobalLock(handle);

                    if (pointer == IntPtr.Zero)
                        return null;

                    var size = GlobalSize(handle);
                    var buff = new byte[size];

                    Marshal.Copy(pointer, buff, 0, size);

                    return Encoding.Unicode.GetString(buff).TrimEnd('\0');
                }
                finally
                {
                    if (pointer != IntPtr.Zero)
                        GlobalUnlock(handle);
                }
            }
            finally
            {
                CloseClipboard();
            }
        }

        public override void SetText(string selectedText)
        {
            try
            {
                if (!OpenClipboard(IntPtr.Zero))
                    return;

                EmptyClipboard();

                var bytes = ((uint)selectedText.Length + 1) * 2;

                var source = Marshal.StringToHGlobalUni(selectedText);

                const int gmem_movable = 0x0002;
                const int gmem_zeroinit = 0x0040;
                const int ghnd = gmem_movable | gmem_zeroinit;

                // IMPORTANT: SetClipboardData requires memory that was acquired with GlobalAlloc using GMEM_MOVABLE.
                var hGlobal = GlobalAlloc(ghnd, (UIntPtr)bytes);

                try
                {
                    var target = GlobalLock(hGlobal);
                    if (target == IntPtr.Zero)
                        return;

                    try
                    {
                        unsafe
                        {
                            Buffer.MemoryCopy((void*)source, (void*)target, bytes, bytes);
                        }
                    }
                    finally
                    {
                        if (target != IntPtr.Zero)
                            GlobalUnlock(target);

                        Marshal.FreeHGlobal(source);
                    }

                    if (SetClipboardData(cf_unicodetext, hGlobal).ToInt64() != 0)
                    {
                        // IMPORTANT: SetClipboardData takes ownership of hGlobal upon success.
                        hGlobal = IntPtr.Zero;
                    }
                }
                finally
                {
                    if (hGlobal != IntPtr.Zero)
                        GlobalFree(hGlobal);
                }
            }
            finally
            {
                CloseClipboard();
            }
        }
    }
}
