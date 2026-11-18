using FluxPayAPI.Shared.DTO.Abstractions;
using FluxPayAPI.Shared.DTO.Responses.Errors;

namespace FluxPayAPI.Shared.Application.Abstractions;

public interface ICommandHandler<in TCommand, TResponse> where TCommand : ICommand
{
    Task<IResult<TResponse, Error>> HandleAsync(TCommand command);
}