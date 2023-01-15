using System.Threading.Channels;
using GamePlayer.Matchmaking.Requests;

namespace GamePlayer.Matchmaking.Services;

internal sealed class MatchmakingRequestsChannel
{
    public readonly Channel<IMatchmakingRequest> Requests = Channel.CreateUnbounded<IMatchmakingRequest>();
}