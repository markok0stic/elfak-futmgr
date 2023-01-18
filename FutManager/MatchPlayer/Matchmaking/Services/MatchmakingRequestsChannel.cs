using System.Threading.Channels;
using MatchPlayer.Matchmaking.Requests;

namespace MatchPlayer.Matchmaking.Services;

internal sealed class MatchmakingRequestsChannel
{
    public readonly Channel<IMatchmakingRequest> Requests = Channel.CreateUnbounded<IMatchmakingRequest>();
}