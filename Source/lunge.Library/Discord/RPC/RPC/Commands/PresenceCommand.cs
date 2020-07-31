﻿using lunge.Library.Discord.RPC.RPC.Payload;
using Newtonsoft.Json;

namespace lunge.Library.Discord.RPC.RPC.Commands
{
	internal class PresenceCommand : ICommand
	{
		/// <summary>
		/// The process ID
		/// </summary>
		[JsonProperty("pid")]
		public int PID { get; set; }

		/// <summary>
		/// The rich presence to be set. Can be null.
		/// </summary>
		[JsonProperty("activity")]
		public RichPresence Presence { get; set; }

		public IPayload PreparePayload(long nonce)
		{
			return new ArgumentPayload(this, nonce)
			{
				Command = Command.SetActivity
			};
		}
	}
}
