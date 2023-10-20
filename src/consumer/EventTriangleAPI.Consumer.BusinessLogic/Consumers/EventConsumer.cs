using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Consumer.BusinessLogic.Hubs;
using EventTriangleAPI.Consumer.BusinessLogic.Models.Notifications;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Messages;
using MassTransit;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

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
    private readonly CreateContactCommandHandler _createContactCommandHandler;
    private readonly DeleteContactCommandHandler _deleteContactCommandHandler;
    private readonly AddCreditCardCommandHandler _addCreditCardCommandHandler;
    private readonly ChangeCreditCardCommandHandler _changeCreditCardCommandHandler;
    private readonly DeleteCreditCardCommandHandler _deleteCreditCardCommandHandler;
    private readonly OpenSupportTicketCommandHandler _openSupportTicketCommandHandler;
    private readonly ResolveSupportTicketCommandHandler _resolveSupportTicketCommandHandler;
    private readonly CreateTransactionCardToUserCommandHandler _createTransactionCardToUserCommandHandler;
    private readonly CreateTransactionUserToUserCommandHandler _createTransactionUserToUserCommandHandler;
    private readonly RollBackTransactionCommandHandler _rollBackTransactionCommandHandler;
    private readonly CreateUserCommandHandler _createUserCommandHandler;
    private readonly NotSuspendUserCommandHandler _notSuspendUserCommandHandler;
    private readonly UpdateUserRoleCommandHandler _updateUserRoleCommandHandler;
    private readonly SuspendUserCommandHandler _suspendUserCommandHandler;

    private readonly IHubContext<NotificationHub, INotificationHub> _hubContext;
    private readonly ILogger<EventConsumer> _logger;

    public EventConsumer(
        CreateContactCommandHandler createContactCommandHandler, 
        DeleteContactCommandHandler deleteContactCommandHandler, 
        AddCreditCardCommandHandler addCreditCardCommandHandler,
        ChangeCreditCardCommandHandler changeCreditCardCommandHandler, 
        DeleteCreditCardCommandHandler deleteCreditCardCommandHandler, 
        OpenSupportTicketCommandHandler openSupportTicketCommandHandler, 
        ResolveSupportTicketCommandHandler resolveSupportTicketCommandHandler, 
        CreateTransactionCardToUserCommandHandler createTransactionCardToUserCommandHandler, 
        CreateTransactionUserToUserCommandHandler createTransactionUserToUserCommandHandler, 
        RollBackTransactionCommandHandler rollBackTransactionCommandHandler, 
        CreateUserCommandHandler createUserCommandHandler, 
        NotSuspendUserCommandHandler notSuspendUserCommandHandler, 
        UpdateUserRoleCommandHandler updateUserRoleCommandHandler,
        SuspendUserCommandHandler suspendUserCommandHandler, 
        IHubContext<NotificationHub,
        INotificationHub> hubContext, 
        ILogger<EventConsumer> logger)
    {
        _createContactCommandHandler = createContactCommandHandler;
        _deleteContactCommandHandler = deleteContactCommandHandler;
        _addCreditCardCommandHandler = addCreditCardCommandHandler;
        _changeCreditCardCommandHandler = changeCreditCardCommandHandler;
        _deleteCreditCardCommandHandler = deleteCreditCardCommandHandler;
        _openSupportTicketCommandHandler = openSupportTicketCommandHandler;
        _resolveSupportTicketCommandHandler = resolveSupportTicketCommandHandler;
        _createTransactionCardToUserCommandHandler = createTransactionCardToUserCommandHandler;
        _createTransactionUserToUserCommandHandler = createTransactionUserToUserCommandHandler;
        _rollBackTransactionCommandHandler = rollBackTransactionCommandHandler;
        _createUserCommandHandler = createUserCommandHandler;
        _notSuspendUserCommandHandler = notSuspendUserCommandHandler;
        _updateUserRoleCommandHandler = updateUserRoleCommandHandler;
        _suspendUserCommandHandler = suspendUserCommandHandler;
        _hubContext = hubContext;
        _logger = logger;
    }

    public async Task Consume(ConsumeContext<ContactCreatedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new CreateContactCommand(message.RequesterId, message.ContactId);
            
            var result = await _createContactCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new ContactCreatingCanceledNotification(message.ContactId, result.Error.Message);

                await _hubContext.Clients.User(message.RequesterId).ContactCreatingCanceledAsync(notification);
            }            
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new ContactCreatingCanceledNotification(message.ContactId, ResponseMessages.InternalError);

            await _hubContext.Clients.User(message.RequesterId).ContactCreatingCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<ContactDeletedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new DeleteContactCommand(message.RequesterId, message.ContactId);

            var result = await _deleteContactCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new ContactDeletingCanceledNotification(message.ContactId, result.Error.Message);

                await _hubContext.Clients.User(message.RequesterId).ContactDeletingCanceledAsync(notification);
            }            
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new ContactDeletingCanceledNotification(message.ContactId, ResponseMessages.InternalError);

            await _hubContext.Clients.User(message.RequesterId).ContactDeletingCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<CreditCardAddedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new AddCreditCardCommand(
                message.RequesterId,
                message.HolderName,
                message.CardNumber,
                message.Cvv,
                message.Expiration,
                message.PaymentNetwork);

            var result = await _addCreditCardCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new CreditCardAddingCanceledNotification(
                    message.HolderName,
                    message.CardNumber,
                    message.Cvv,
                    message.Expiration,
                    message.PaymentNetwork,
                    result.Error.Message);

                await _hubContext.Clients.User(message.RequesterId).CreditCardAddingCanceledAsync(notification);
            }            
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new CreditCardAddingCanceledNotification(
                message.HolderName,
                message.CardNumber,
                message.Cvv,
                message.Expiration,
                message.PaymentNetwork,
                ResponseMessages.InternalError);

            await _hubContext.Clients.User(message.RequesterId).CreditCardAddingCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<CreditCardChangedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new ChangeCreditCardCommand(
                message.CardId, 
                message.RequesterId, 
                message.HolderName,
                message.CardNumber,
                message.Cvv,
                message.Expiration,
                message.PaymentNetwork);

            var result = await _changeCreditCardCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new CreditCardChangingCanceledNotification(
                    message.HolderName,
                    message.CardNumber,
                    message.Cvv,
                    message.Expiration,
                    message.PaymentNetwork,
                    result.Error.Message);

                await _hubContext.Clients.User(message.RequesterId).CreditCardChangingCanceledAsync(notification);
            }            
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new CreditCardChangingCanceledNotification(
                message.HolderName,
                message.CardNumber,
                message.Cvv,
                message.Expiration,
                message.PaymentNetwork,
                ResponseMessages.InternalError);

            await _hubContext.Clients.User(message.RequesterId).CreditCardChangingCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<CreditCardDeletedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new DeleteCreditCardCommand(message.RequesterId, message.CardId);

            var result = await _deleteCreditCardCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new CreditCardDeletingCanceledNotification(message.CardId, result.Error.Message);

                await _hubContext.Clients.User(message.RequesterId).CreditCardDeletingCanceledAsync(notification);
            }            
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new CreditCardDeletingCanceledNotification(message.CardId, ResponseMessages.InternalError);

            await _hubContext.Clients.User(message.RequesterId).CreditCardDeletingCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<SupportTicketOpenedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new OpenSupportTicketCommand(
                message.RequesterId, 
                message.WalletId,
                message.TransactionId,
                message.TicketReason,
                message.CreatedAt);

            var result = await _openSupportTicketCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new SupportTicketOpeningCanceledNotification(
                    message.WalletId,
                    message.TransactionId,
                    message.TicketReason,
                    result.Error.Message);

                await _hubContext.Clients.User(message.RequesterId).SupportTicketOpeningCanceledAsync(notification);
            }            
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new SupportTicketOpeningCanceledNotification(
                message.WalletId,
                message.TransactionId,
                message.TicketReason,
                ResponseMessages.InternalError);

            await _hubContext.Clients.User(message.RequesterId).SupportTicketOpeningCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<SupportTicketResolvedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new ResolveSupportTicketCommand(message.RequesterId, message.TicketId, message.TicketJustification);
        
            var result = await _resolveSupportTicketCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new SupportTicketResolvingCanceledNotification(
                    message.TicketId,
                    message.TicketJustification,
                    result.Error.Message);

                await _hubContext.Clients.User(message.RequesterId).SupportTicketResolvingCanceledAsync(notification);
            }            
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new SupportTicketResolvingCanceledNotification(
                message.TicketId,
                message.TicketJustification,
                ResponseMessages.InternalError);

            await _hubContext.Clients.User(message.RequesterId).SupportTicketResolvingCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<TransactionCardToUserCreatedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new CreateTransactionCardToUserCommand(
                message.CreditCardId, 
                message.RequesterId, 
                message.Amount, 
                message.CreatedAt);

            var result = await _createTransactionCardToUserCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new TransactionCanceledNotification(
                    message.RequesterId, 
                    message.RequesterId, 
                    message.Amount,
                    TransactionType.FromCardToUser,
                    message.CreatedAt,
                    result.Error.Message);
            
                await _hubContext.Clients.User(context.Message.RequesterId).TransactionCanceledAsync(notification);
                return;
            }
            
            await _hubContext.Clients.User(context.Message.RequesterId).TransactionSucceededAsync(result.Response);
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new TransactionCanceledNotification(
                message.RequesterId, 
                message.RequesterId, 
                message.Amount,
                TransactionType.FromCardToUser,
                message.CreatedAt,
                ResponseMessages.InternalError);
            
            await _hubContext.Clients.User(context.Message.RequesterId).TransactionCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<TransactionUserToUserCreatedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new CreateTransactionUserToUserCommand(
                message.RequesterId,
                message.ToUserId,
                message.Amount,
                message.CreatedAt);

            var result = await _createTransactionUserToUserCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new TransactionCanceledNotification(
                    message.RequesterId, 
                    message.ToUserId, 
                    message.Amount,
                    TransactionType.FromCardToUser,
                    message.CreatedAt,
                    result.Error.Message);
            
                await _hubContext.Clients.User(context.Message.RequesterId).TransactionCanceledAsync(notification);
                return;
            }
            
            await _hubContext.Clients.User(context.Message.RequesterId).TransactionSucceededAsync(result.Response);
            await _hubContext.Clients.User(context.Message.ToUserId).TransactionSucceededAsync(result.Response);
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new TransactionCanceledNotification(
                message.RequesterId, 
                message.ToUserId, 
                message.Amount,
                TransactionType.FromCardToUser,
                message.CreatedAt,
                ResponseMessages.InternalError);
            
            await _hubContext.Clients.User(context.Message.RequesterId).TransactionCanceledAsync(notification);
            throw;
        }
    }
    
    public async Task Consume(ConsumeContext<TransactionRollBackedEventMessage> context)
    {
        var message = context.Message;
        
        try
        {
            var command = new RollBackTransactionCommand(message.RequesterId, message.TransactionId);

            var result = await _rollBackTransactionCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new TransactionRollBackCanceledNotification(message.TransactionId, result.Error.Message);
            
                await _hubContext.Clients.User(context.Message.RequesterId).TransactionRollBackCanceledAsync(notification);
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new TransactionRollBackCanceledNotification(message.TransactionId, ResponseMessages.InternalError);
            
            await _hubContext.Clients.User(context.Message.RequesterId).TransactionRollBackCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<UserCreatedEventMessage> context)
    {
        var message = context.Message;
        var command = new CreateUserCommand(message.UserId, message.Email, message.UserRole, message.UserStatus);

        await _createUserCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<UserNotSuspendedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new NotSuspendUserCommand(message.RequesterId, message.UserId);

            var result = await _notSuspendUserCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new UserNotSuspendingCanceledNotification(message.UserId, result.Error.Message);
            
                await _hubContext.Clients.User(context.Message.RequesterId).UserNotSuspendingCanceledAsync(notification);
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new UserNotSuspendingCanceledNotification(message.UserId, ResponseMessages.InternalError);
            
            await _hubContext.Clients.User(context.Message.RequesterId).UserNotSuspendingCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<UserRoleUpdatedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new UpdateUserRoleCommand(message.RequesterId, message.UserId, message.UserRole);

            var result = await _updateUserRoleCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new UserRoleUpdatingCanceledNotification(
                    message.UserId,
                    message.UserRole,
                    result.Error.Message);
            
                await _hubContext.Clients.User(context.Message.RequesterId).UserRoleUpdatingCanceledAsync(notification);
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new UserRoleUpdatingCanceledNotification(
                message.UserId,
                message.UserRole,
                ResponseMessages.InternalError);
            
            await _hubContext.Clients.User(context.Message.RequesterId).UserRoleUpdatingCanceledAsync(notification);
            throw;
        }
    }

    public async Task Consume(ConsumeContext<UserSuspendedEventMessage> context)
    {
        var message = context.Message;

        try
        {
            var command = new SuspendUserCommand(message.RequesterId, message.UserId);

            var result = await _suspendUserCommandHandler.HandleAsync(command);

            if (!result.IsSuccess)
            {
                _logger.LogError(result.Error.Message);
                
                var notification = new UserSuspendingCanceledNotification(message.UserId, result.Error.Message);
            
                await _hubContext.Clients.User(context.Message.RequesterId).UserSuspendingCanceledAsync(notification);
            }
        }
        catch(Exception e)
        {
            _logger.LogError(e.Message);
            
            var notification = new UserSuspendingCanceledNotification(message.UserId, ResponseMessages.InternalError);
            
            await _hubContext.Clients.User(context.Message.RequesterId).UserSuspendingCanceledAsync(notification);
            throw;
        }
    }
}