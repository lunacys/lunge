﻿namespace lunge.Library.Discord.RPC.Logging
{
	/// <summary>
	/// Ignores all log events
	/// </summary>
	public class NullLogger : ILogger
	{
		public LogLevel Level { get; set; }
		public void Info(string message, params object[] args)
		{
			//Null Logger, so no messages are acutally sent
		}
		public void Warning(string message, params object[] args)
		{
			//Null Logger, so no messages are acutally sent 
		}
		public void Error(string message, params object[] args)
		{
			//Null Logger, so no messages are acutally sent
		}
	}
}
