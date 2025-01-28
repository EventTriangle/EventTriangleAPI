using EventTriangleAPI.Shared.DTO.Abstractions;

namespace EventTriangleAPI.Consumer.BusinessLogic.QueryHandlers;

public record GetCreditCardsQuery(string RequesterId) : ICommand;