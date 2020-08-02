﻿using lunge.Library.Discord.RPC.RPC.Payload;

namespace lunge.Library.Discord.RPC.RPC.Commands
{
	class SubscribeCommand : ICommand
	{
		public ServerEvent Event { get; set; }
		public bool IsUnsubscribe { get; set; }
		
		public IPayload PreparePayload(long nonce)
		{
			return new EventPayload(nonce)
			{
				Command = IsUnsubscribe ? Command.Unsubscribe : Command.Subscribe,
				Event = Event
			};
		}
	}
}
