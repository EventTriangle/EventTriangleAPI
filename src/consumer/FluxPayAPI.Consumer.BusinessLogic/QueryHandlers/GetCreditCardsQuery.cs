using FluxPayAPI.Shared.DTO.Abstractions;

namespace FluxPayAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetCreditCardsQuery(string RequesterId) : ICommand;