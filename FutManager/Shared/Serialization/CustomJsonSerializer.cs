using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Shared.Serialization.Options;

namespace Shared.Serialization;

public class CustomJsonSerializer : ISerializer
{
    private readonly CustomJsonSerializerOptions _options;

    public CustomJsonSerializer(CustomJsonSerializerOptions options)
    {
        _options = options;
    }
    
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

    public string RegexSerialize<T>(T value, CustomJsonSerializerOptions? options) where T : class?
    {
        if (value == null)
        {
            return "{}";
        }
        string jsonText = JsonConvert.SerializeObject(value);
        return FormatByOptions(jsonText, options ?? _options);
    }

    private string FormatByOptions(string jsonText, CustomJsonSerializerOptions options)
    {
        string regexPattern = "\"([^\"]+)\":"; // pattern "property": => property: // neo4j syntax
        string customJson;
        if (options.UseEqualAndPrefixForNode)
            customJson = Regex.Replace(jsonText, regexPattern, "x.$1=");
        else
            customJson = Regex.Replace(jsonText, regexPattern, "$1:");

        if (options.RemoveCurlyBrackets)
            customJson = customJson.Remove(0,1).Remove(customJson.Length-2,1);

        return customJson;
    }
}