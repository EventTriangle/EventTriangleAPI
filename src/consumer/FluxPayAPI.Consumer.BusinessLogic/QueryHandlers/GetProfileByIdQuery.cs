using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetProfileByIdQuery(string UserId) : ICommand;