﻿namespace lunge.Library.Discord.RPC.Logging
{
	/// <summary>
	/// Level of logging to employ.
	/// </summary>
	public enum LogLevel
	{
		/// <summary>
		/// Info, Warning and Errors are logged
		/// </summary>
		Info,
		
		/// <summary>
		/// Warning and Errors are logged
		/// </summary>
		Warning,

		/// <summary>
		/// Only Errors are logged
		/// </summary>
		Error,

		/// <summary>
		/// Nothing is logged
		/// </summary>
		None
	}
}
