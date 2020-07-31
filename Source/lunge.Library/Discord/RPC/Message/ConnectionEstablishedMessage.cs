﻿namespace lunge.Library.Discord.RPC.Message
{
	/// <summary>
	/// The connection to the discord client was succesfull. This is called before <see cref="MessageType.Ready"/>.
	/// </summary>
	public class ConnectionEstablishedMessage : IMessage
	{
		public override MessageType Type { get { return MessageType.ConnectionEstablished; } }

		/// <summary>
		/// The pipe we ended up connecting too
		/// </summary>
		public int ConnectedPipe { get; internal set; }
	}
}
