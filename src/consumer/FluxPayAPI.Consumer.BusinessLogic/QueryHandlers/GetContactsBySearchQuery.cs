using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetContactsBySearchQuery(string RequesterId, string Email) : ICommand;