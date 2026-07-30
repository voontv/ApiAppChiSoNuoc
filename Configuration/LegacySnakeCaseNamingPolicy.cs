using System.Text;
using System.Text.Json;

namespace ReadMeter.Api.Configuration;

public sealed class LegacySnakeCaseNamingPolicy : JsonNamingPolicy
{
    public static LegacySnakeCaseNamingPolicy Instance { get; } = new();

    public override string ConvertName(string name)
    {
        if (string.IsNullOrEmpty(name))
        {
            return name;
        }

        var result = new StringBuilder(name.Length + 8);
        for (var index = 0; index < name.Length; index++)
        {
            var current = name[index];
            if (char.IsUpper(current) && index > 0 && name[index - 1] != '_')
            {
                result.Append('_');
            }

            result.Append(char.ToLowerInvariant(current));
        }

        return result.ToString();
    }
}
