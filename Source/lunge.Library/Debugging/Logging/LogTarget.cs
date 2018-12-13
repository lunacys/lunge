using System.IO;

namespace lunge.Library.Debugging.Logging
{
    public enum LogTarget
    {
        /// <summary>
        /// Console target provided by <see cref="Console"/> class
        /// </summary>
        Console = 1,
        /// <summary>
        /// File target provided by <see cref="FileStream"/> class
        /// </summary>
        File = 2
    }
}

