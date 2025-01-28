using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Shared.Application.Abstractions;

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand
{
    Task<IResult<TResponse, Error>> HandleAsync(TCommand command);
}