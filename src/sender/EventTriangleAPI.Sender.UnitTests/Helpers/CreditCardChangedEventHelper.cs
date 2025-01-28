using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Sender.UnitTests.Helpers;

public class CreditCardChangedEventHelper
{
    private const string CardNumber = "1234567890123456";
    private const string Cvv = "123"; 
    private const string Expiration = "12/12"; 
    
    public static CreditCardChangedEvent CreateSuccess()
    {
        return new CreditCardChangedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardChangedEvent CreateWithCardId(Guid cardId)
    {
        return new CreditCardChangedEvent(
            cardId,
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardChangedEvent CreateWithUserId(string userId)
    {
        return new CreditCardChangedEvent(
            Guid.NewGuid(),
            userId,
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardChangedEvent CreateWithHolderName(string holderName)
    {
        return new CreditCardChangedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            holderName,
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    }
    
    public static CreditCardChangedEvent CreateWithCardNumber(string cardNumber)
    {
        return new CreditCardChangedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            cardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardChangedEvent CreateWithCvv(string cvv)
    {
        return new CreditCardChangedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardChangedEvent CreateWithExpiration(string expiration)
    {
        return new CreditCardChangedEvent(
            Guid.NewGuid(),
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            expiration,
            PaymentNetwork.MasterCard);
    } 
}