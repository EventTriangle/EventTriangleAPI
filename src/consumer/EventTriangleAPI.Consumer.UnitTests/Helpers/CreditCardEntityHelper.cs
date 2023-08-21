using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.UnitTests.Helpers;

public class CreditCardEntityHelper
{
    private const string CardNumber = "1234567890123456";
    private const string Cvv = "123"; 
    private const string Expiration = "12/12"; 
    
    public static CreditCardEntity CreateSuccess()
    {
        return new CreditCardEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    }

    public static CreditCardEntity CreateWithUserId(string userId)
    {
        return new CreditCardEntity(
            userId,
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardEntity CreateWithHolderName(string holderName)
    {
        return new CreditCardEntity(
            Guid.NewGuid().ToString(),
            holderName,
            CardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    }
    
    public static CreditCardEntity CreateWithCardNumber(string cardNumber)
    {
        return new CreditCardEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            cardNumber,
            Cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardEntity CreateWithCvv(string cvv)
    {
        return new CreditCardEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            cvv,
            Expiration,
            PaymentNetwork.MasterCard);
    } 
    
    public static CreditCardEntity CreateWithExpiration(string expiration)
    {
        return new CreditCardEntity(
            Guid.NewGuid().ToString(),
            Guid.NewGuid().ToString(),
            CardNumber,
            Cvv,
            expiration,
            PaymentNetwork.MasterCard);
    } 
}