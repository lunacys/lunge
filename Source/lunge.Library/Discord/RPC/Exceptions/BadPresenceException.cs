using System;

namespace lunge.Library.Discord.RPC.Exceptions
{
	class BadPresenceException : Exception
	{
		public BadPresenceException(string message) : base(message) { }
	}
}
