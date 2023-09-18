using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetProfileByIdQuery(string UserId) : ICommand;