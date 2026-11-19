using System.Text.RegularExpressions;

namespace FluxPayAPI.Consumer.Presentation.Routing;

public partial class CustomParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object value)
    {
        if (value == null)
        {
            return null;
        }
        
        return WordBoundary().Replace(value.ToString() ?? string.Empty, "$1-$2").ToLower();
    }

    [GeneratedRegex("([a-z])([A-Z])", RegexOptions.None, 1000)]
    private static partial Regex WordBoundary();
}