using Neo4jClient;
using Shared.Neo4j.Enums;
using Shared.Serialization;

namespace Shared.Neo4j.DbService;

public interface IGraphDbService<T, in TQ> where T: class where TQ: class?
{
    Task AddNode(T node);
    Task MakeRelationship(T startingNode, TQ destinationNode, RelationshipTypes type);
    Task<T?> GetNode(int id);
    Task<IEnumerable<T>> GetNodes(int page);
    Task<int> GetNextId(string model);
}

public class GraphDbService<T,TQ>: IGraphDbService<T,TQ> where T : class where TQ : class?
{
    private readonly IGraphClient _graphClient;
    private readonly ISerializer _customSerializer;
    private readonly int _pageSize = 10;
    
    public GraphDbService(IGraphClient graphClient, ISerializer customSerializer)
    {
        _graphClient = graphClient;
        _customSerializer = customSerializer;
    }
    
    public async Task AddNode(T node)
    {
        var query = $"(m:{typeof(T).Name} {_customSerializer.RegexSerialize(node)})";
        await _graphClient.Cypher
            .Create(query)
            .ExecuteWithoutResultsAsync();
    }

    public async Task<T?> GetNode(int id)
    {
        return (await _graphClient.Cypher
            .Match($"(n:{typeof(T).Name} {{Id: {id} }})")
            .Return<T>("n")
            .ResultsAsync).FirstOrDefault();
    }

    public async Task<IEnumerable<T>> GetNodes(int page)
    {
        var skip = _pageSize * page;
        return await _graphClient.Cypher
            .Match($"(n:{typeof(T).Name})")
            .Return<T>("n")
            .Skip(skip)
            .Limit(_pageSize)
            .ResultsAsync;
    }
    
    public async Task MakeRelationship(T startingNode, TQ destinationNode, RelationshipTypes type)
    {
        if(destinationNode == null)
            return;
        var dynIdStart = (dynamic)startingNode;
        var dynIdDest = (dynamic)destinationNode;
        await _graphClient.Cypher
            .Match($"(x:{typeof(T).Name}), (y:{typeof(TQ).Name})")
            .Where($"x.Id = {dynIdStart.Id} AND y.Id = {dynIdDest.Id}")
            .Create($"(x)-[:{type.ToString()}]->(y)")
            .ExecuteWithoutResultsAsync();
    }
    
    public async Task<int> GetNextId(string model)
    {
        var maxId = 0;
        try
        {
            var data = await _graphClient.Cypher
                .Match($"(n:{model})")
                .Return<int?>("max(n.Id)").ResultsAsync;
            
            maxId = Convert.ToInt32(data.FirstOrDefault());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }

        return ++maxId;
    }
}