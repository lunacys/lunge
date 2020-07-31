using lunge.Library.Discord.RPC.RPC.Payload;

namespace lunge.Library.Discord.RPC.RPC.Commands
{
	interface ICommand
	{
		IPayload PreparePayload(long nonce);
	}
}
