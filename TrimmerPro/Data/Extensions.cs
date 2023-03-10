namespace TrimmerPro.Data;

public static class Extensions
{
    public static StringContent AsJson(this object obj)
    {
        return new StringContent(JsonSerializer.Serialize(obj), Encoding.UTF8, MediaTypeNames.Application.Json);
    }
}