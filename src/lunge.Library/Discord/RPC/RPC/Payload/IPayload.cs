﻿using lunge.Library.Discord.RPC.Converters;
using Newtonsoft.Json;

namespace lunge.Library.Discord.RPC.RPC.Payload
{
	/// <summary>
	/// Base Payload that is received by both client and server
	/// </summary>
	internal abstract class IPayload
	{
		/// <summary>
		/// The type of payload
		/// </summary>
		[JsonProperty("cmd"), JsonConverter(typeof(EnumSnakeCaseConverter))]
		public Command Command { get; set; }

		/// <summary>
		/// A incremental value to help identify payloads
		/// </summary>
		[JsonProperty("nonce")]
		public string Nonce { get; set; }

		public IPayload() { }
		public IPayload(long nonce)
		{
			Nonce = nonce.ToString();
		}

		public override string ToString()
		{
			return "Payload || Command: " + Command.ToString() + ", Nonce: " + (Nonce != null ? Nonce.ToString() : "NULL");
		}
	}
}

