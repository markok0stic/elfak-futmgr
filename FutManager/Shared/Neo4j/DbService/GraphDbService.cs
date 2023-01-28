using Shared.Models.Football_Player_Models;
using Shared.Models.MatchModels;

namespace Shared.Neo4j.DbService;

public interface IGraphDbService
{
    Task InsertMatch(Match match);
    Task UpdateMatchResult();
    Task AddMatchScore(Score score);
}

public class GraphDbService: IGraphDbService
{
    public async Task InsertMatch(Match match)
    {
        
    }

    public async Task UpdateMatchResult()
    {
        
    }

    public async Task AddMatchScore(Score score)
    {
        
    }
}