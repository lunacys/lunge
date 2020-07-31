using System;

namespace lunge.Library.Discord.RPC.Exceptions
{
	class InvalidConfigurationException : Exception
	{
		public InvalidConfigurationException(string message) : base(message) { }
	}
}
