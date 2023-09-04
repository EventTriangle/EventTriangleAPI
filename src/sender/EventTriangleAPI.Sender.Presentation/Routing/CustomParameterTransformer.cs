using System.Text.RegularExpressions;

namespace EventTriangleAPI.Sender.Presentation.Routing;

public class CustomParameterTransformer : IOutboundParameterTransformer
{
    public string TransformOutbound(object value)
    {
        if (value == null)
        {
            return null;
        }

        return Regex.Replace(value.ToString() ?? string.Empty, "([a-z])([A-Z])", "$1-$2").ToLower();
    }
}