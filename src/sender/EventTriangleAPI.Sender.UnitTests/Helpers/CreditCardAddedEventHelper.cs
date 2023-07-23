using EventTriangleAPI.Shared.Domain.Entities;
using EventTriangleAPI.Shared.Domain.Enums;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public static class CreditCardAddedEventHelper
{
    private const string CardNumber = "1234567890123456";
    private const string Cvv = "123"; 
    private const string Expiration = "12/12"; 
    
    public static CreditCardAddedEvent CreateSuccess()
    {
        return new CreditCardAddedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardAddedEvent CreateWithCardId(Guid cardId)
    {
        return new CreditCardAddedEvent(
            cardId,
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardAddedEvent CreateWithUserId(string userId)
    {
        return new CreditCardAddedEvent(
            Guid.NewGuid(),
            userId,
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardAddedEvent CreateWithHolderName(string holderName)
    {
        return new CreditCardAddedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            holderName,
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    }
    
    public static CreditCardAddedEvent CreateWithCardNumber(string cardNumber)
    {
        return new CreditCardAddedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            cardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardAddedEvent CreateWithCvv(string cvv)
    {
        return new CreditCardAddedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardAddedEvent CreateWithExpiration(string expiration)
    {
        return new CreditCardAddedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            expiration,
            PaymentNetwork.MasterCard);
    } 
}