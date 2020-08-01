/*
   MIT License
   
   Copyright (c) 2018 Swan
   
   Permission is hereby granted, free of charge, to any person obtaining a copy
   of this software and associated documentation files (the "Software"), to deal
   in the Software without restriction, including without limitation the rights
   to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
   copies of the Software, and to permit persons to whom the Software is
   furnished to do so, subject to the following conditions:
   
   The above copyright notice and this permission notice shall be included in all
   copies or substantial portions of the Software.
   
   THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
   IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
   FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
   AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
   LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
   OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
   SOFTWARE.
 */

using System;
using lunge.Library.Discord.RPC;
using lunge.Library.Discord.RPC.Logging;

namespace lunge.Library.Discord
{
    public static class DiscordManager
    {
        /// <summary>
        ///     The Discord RPC client.
        /// </summary>
        public static DiscordRpcClient Client { get; private set; }

        /// <summary>
        ///    The application id received from https://discordapp.com/developers.
        /// </summary>
        public static string AppId { get; private set; }

        /// <summary>
        ///     Creates a new Discord RPC client to use throughout the game.
        ///     For more documentation on how to actually use the client: https://github.com/Lachee/discord-rpc-csharp
        /// </summary>
        /// <param name="appId">The application id, received from discordapp.com/developers</param>
        /// <param name="logLevel">The level of logging for the client.</param>
        public static void CreateClient(string appId, LogLevel logLevel = LogLevel.None)
        {
            if (Client != null && !Client.Disposed)
                throw new InvalidOperationException("DiscordRpcClient already is initialized and hasn't been disposed.");

            AppId = appId;

            Client = new DiscordRpcClient(AppId, true, -1)
            {
                Logger = new ConsoleLogger { Level = logLevel }
            };

            Client.Initialize();
        }

        /// <summary>
        ///     Disposes of the current client.
        /// </summary>
        public static void Dispose() => Client?.Dispose();
    }
}