using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Messages;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.Consumers;

public class EventConsumer : 
    IConsumer<ContactCreatedEventMessage>,
    IConsumer<ContactDeletedEventMessage>,
    IConsumer<CreditCardAddedEventMessage>,
    IConsumer<CreditCardChangedEventMessage>,
    IConsumer<CreditCardDeletedEventMessage>,
    IConsumer<SupportTicketOpenedEventMessage>,
    IConsumer<SupportTicketResolvedEventMessage>,
    IConsumer<TransactionCardToUserCreatedEventMessage>,
    IConsumer<TransactionUserToUserCreatedEventMessage>,
    IConsumer<TransactionRollBackedEventMessage>,
    IConsumer<UserCreatedEventMessage>,
    IConsumer<UserNotSuspendedEventMessage>,
    IConsumer<UserRoleUpdatedEventMessage>,
    IConsumer<UserSuspendedEventMessage>
{
    private readonly DatabaseContext _context;
    
    public EventConsumer(DatabaseContext context)
    {
        _context = context;
    }

    public async Task Consume(ConsumeContext<ContactCreatedEventMessage> context)
    {
        var message = context.Message;
        
        var contact = new ContactEntity(message.UserId, message.ContactId);

        _context.ContactEntities.Add(contact);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<ContactDeletedEventMessage> context)
    {
        var message = context.Message;

        var contact = await _context.ContactEntities
            .FirstOrDefaultAsync(x => x.UserId == message.UserId && x.ContactId == message.ContactId);

        if (contact == null)
        {
            throw new NotImplementedException();
        }

        _context.ContactEntities.Remove(contact);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<CreditCardAddedEventMessage> context)
    {
        var message = context.Message;

        var creditCard = new CreditCardEntity(
            message.UserId, 
            message.HolderName, 
            message.CardNumber, 
            message.Cvv, 
            message.PaymentNetwork);

        _context.CreditCardEntities.Add(creditCard);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<CreditCardChangedEventMessage> context)
    {
        var message = context.Message;

        var creditCard = await _context.CreditCardEntities
            .FirstOrDefaultAsync(x => x.Id == message.CardId && x.UserId == message.UserId);

        if (creditCard == null)
        {
            throw new NotImplementedException();
        }

        creditCard.UpdateHolderName(message.HolderName);
        creditCard.UpdateCardNumber(message.CardNumber);
        creditCard.UpdateCvv(message.Cvv);
        creditCard.UpdatePaymentNetwork(message.PaymentNetwork);

        _context.CreditCardEntities.Update(creditCard);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<CreditCardDeletedEventMessage> context)
    {
        var message = context.Message;

        var creditCard = await _context.CreditCardEntities
            .FirstOrDefaultAsync(x => x.Id == message.CardId && x.UserId == message.UserId);

        if (creditCard == null)
        {
            throw new NotImplementedException();
        }

        _context.CreditCardEntities.Remove(creditCard);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<SupportTicketOpenedEventMessage> context)
    {
        var message = context.Message;

        var supportTicket = new SupportTicketEntity(message.UserId, message.WalletId, message.TicketReason);

        _context.SupportTicketEntities.Add(supportTicket);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<SupportTicketResolvedEventMessage> context)
    {
        var message = context.Message;

        var supportTicket = await _context.SupportTicketEntities
            .FirstOrDefaultAsync(x => x.Id == message.TicketId);

        if (supportTicket == null)
        {
            throw new NotImplementedException();
        }
        
        supportTicket.UpdateTicketJustification(message.TicketJustification);
        supportTicket.UpdateTicketStatus(TicketStatus.Resolved);

        _context.SupportTicketEntities.Update(supportTicket);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<TransactionCardToUserCreatedEventMessage> context)
    {
        var message = context.Message;

        var transaction = new TransactionEntity(
            message.ToUserId, 
            message.ToUserId, 
            message.Amount, 
            TransactionType.FromCardToUser);

        _context.TransactionEntities.Add(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<TransactionUserToUserCreatedEventMessage> context)
    {
        var message = context.Message;

        var transaction = new TransactionEntity(
            message.FromUserId, 
            message.ToUserId, 
            message.Amount, 
            TransactionType.FromUserToUser);

        _context.TransactionEntities.Add(transaction);
        await _context.SaveChangesAsync();
    }
    
    public async Task Consume(ConsumeContext<TransactionRollBackedEventMessage> context)
    {
        var message = context.Message;

        var transaction = await _context.TransactionEntities.FirstOrDefaultAsync(x => x.Id == message.TransactionId);

        if (transaction == null)
        {
            throw new NotImplementedException();
        }
        
        transaction.UpdateTransactionState(TransactionState.RolledBack);
        
        _context.TransactionEntities.Update(transaction);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<UserCreatedEventMessage> context)
    {
        var message = context.Message;

        var wallet = new WalletEntity(0);
        var user = new UserEntity(message.UserId, message.Email, wallet.Id, message.UserRole, message.UserStatus);

        _context.WalletEntities.Add(wallet);
        _context.UserEntities.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<UserNotSuspendedEventMessage> context)
    {
        var message = context.Message;

        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == message.UserId);

        if (user == null)
        {
            throw new NotImplementedException();
        }
        
        user.UpdateUserStatus(UserStatus.Active);

        _context.UserEntities.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<UserRoleUpdatedEventMessage> context)
    {
        var message = context.Message;

        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == message.UserId);
        
        if (user == null)
        {
            throw new NotImplementedException();
        }
        
        user.UpdateUserRole(message.UserRole);

        _context.UserEntities.Update(user);
        await _context.SaveChangesAsync();
    }

    public async Task Consume(ConsumeContext<UserSuspendedEventMessage> context)
    {
        var message = context.Message;

        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == message.UserId);
        
        if (user == null)
        {
            throw new NotImplementedException();
        }
        
        user.UpdateUserStatus(UserStatus.Suspended);

        _context.UserEntities.Update(user);
        await _context.SaveChangesAsync();
    }
}