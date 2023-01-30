namespace Shared.Serialization;

public interface ISerializer
{
    string Serialize<T>(T value) where T : class?;
    string RegexSerialize<T>(T value) where T : class?;
}