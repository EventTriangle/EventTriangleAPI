using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Shared.DTO.Commands;

public class Command<TBody> : ICommand<TBody>
{
    public TBody Body { get; }

    public Command(TBody body)
    {
        Body = body;
    }
}