using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Shared.Serialization;

public class CustomJsonSerializer : ISerializer
{

    public string Serialize<T>(T value) where T : class?
    {
        if (value == null)
        {
            return "{}";
        }
        var serializer = new JsonSerializer();
        var stringWriter = new StringWriter();
        using (var writer = new JsonTextWriter(stringWriter))
        {
            writer.QuoteName = false;
            serializer.Serialize(writer,value);            
        }
        return stringWriter.ToString();
    }

    public string RegexSerialize<T>(T value) where T : class?
    {
        if (value == null)
        {
            return "{}";
        }
        string jsonText = JsonConvert.SerializeObject(value);
        string regexPattern = "\"([^\"]+)\":"; // pattern "property": => property: // neo4j syntax
        return Regex.Replace(jsonText, regexPattern, "$1:");
    }
}