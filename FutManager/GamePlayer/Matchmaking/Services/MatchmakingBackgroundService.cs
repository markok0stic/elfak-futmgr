using GamePlayer.Matchmaking.Requests;
using Shared.Streaming;

namespace GamePlayer.Matchmaking.Services;

internal sealed class MatchmakingBackgroundService: BackgroundService
{
    private readonly IScoresGenerator _scoresGenerator;
    private readonly MatchmakingRequestsChannel _matchmakingRequestsChannel;
    private readonly IStreamPublisher _streamPublisher;
    private readonly ILogger<MatchmakingBackgroundService> _logger;

    private int _runningStatus;

    public MatchmakingBackgroundService(IScoresGenerator scoresGenerator, 
        MatchmakingRequestsChannel matchmakingRequestsChannel, IStreamPublisher streamPublisher, ILogger<MatchmakingBackgroundService> logger)
    {
        _scoresGenerator = scoresGenerator;
        _matchmakingRequestsChannel = matchmakingRequestsChannel;
        _streamPublisher = streamPublisher;
        _logger = logger;
    }
    

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("Matchmaking background service has started.");
        await foreach (var request in _matchmakingRequestsChannel.Requests.Reader.ReadAllAsync(stoppingToken))
        {
            _logger.LogInformation($"Matchmaking background service has received the request: '{request.GetType().Name}'.");
            
            var _ = request switch
            {
                StartMatchmaking => StartGeneratorAsync(),
                StopMatchmaking => StopGeneratorAsync(),
                _ => Task.CompletedTask
            };
        }
        
        _logger.LogInformation("Matchmaking background service has stopped.");
    }
    
    private async Task StartGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 1) == 1)
        {
            _logger.LogInformation("Scores generator is already running.");
            return;
        }

        await foreach (var scores in _scoresGenerator.StartAsync())
        {
            _logger.LogInformation("Publishing the scores...");
            await _streamPublisher.PublishAsync("scores", scores);
        }
    }
    
    private async Task StopGeneratorAsync()
    {
        if (Interlocked.Exchange(ref _runningStatus, 0) == 0)
        {
            _logger.LogInformation("Scores generator is not running.");
            return;
        }

        await _scoresGenerator.StopAsync();
    }
}