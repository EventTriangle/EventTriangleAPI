namespace EventTriangleAPI.Shared.DTO.Abstractions;

public interface ICommand<out TBody>
{
    public TBody Body { get; }
}