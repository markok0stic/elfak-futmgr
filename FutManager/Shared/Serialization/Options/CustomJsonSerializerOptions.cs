namespace Shared.Serialization.Options;

public class CustomJsonSerializerOptions
{
    public bool UseEqualAndPrefixForNode { get; set; } = false;
    public bool RemoveCurlyBrackets { get; set; } = false;
}