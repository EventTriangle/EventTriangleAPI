using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.IntegrationTests.Helpers;

public static class AddCreditCardCommandHelper
{
    public static AddCreditCardCommand CreateCreditCardCommand(string requesterId)
    {
        var random = new Random();
        var randomInt = random.Next(10000000, 99999999);
        var cardNumber = $"{randomInt}{randomInt}";
        
        return new AddCreditCardCommand(
            requesterId,
            "User",
            cardNumber,
            "123",
            "11/11",
            PaymentNetwork.MasterCard);
    }
}