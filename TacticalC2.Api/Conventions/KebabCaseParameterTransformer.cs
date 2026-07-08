using System.Text.RegularExpressions;

namespace TacticalC2.Api.Conventions;

public partial class KebabCaseParameterTransformer : IOutboundParameterTransformer
{
    public string? TransformOutbound(object? value)
    {
        return value is null ? null : MyRegex().Replace(value.ToString() ?? "", "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])")]
    private static partial Regex MyRegex();
}