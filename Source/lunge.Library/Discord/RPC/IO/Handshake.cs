﻿using Newtonsoft.Json;

namespace lunge.Library.Discord.RPC.IO
{
	internal class Handshake
	{       
		/// <summary>
		/// Version of the IPC API we are using
		/// </summary>
		[JsonProperty("v")]
		public int Version { get; set; }

		/// <summary>
		/// The ID of the app.
		/// </summary>
		[JsonProperty("client_id")]
		public string ClientID { get; set; }
	}
}
