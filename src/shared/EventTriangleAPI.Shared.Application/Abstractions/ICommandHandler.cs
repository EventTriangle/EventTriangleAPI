using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Commands;
using EventTriangleAPI.Shared.DTO.Responses.Errors;

namespace EventTriangleAPI.Shared.Application.Abstractions;

public interface ICommandHandler<in TBody, TResponse>
{
    Task<IResult<TResponse, Error>> HandleAsync(ICommand<TBody> command);
}