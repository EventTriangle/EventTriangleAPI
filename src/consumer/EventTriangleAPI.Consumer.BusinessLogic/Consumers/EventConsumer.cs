using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Messages;
using MassTransit;

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
        SuspendUserCommandHandler suspendUserCommandHandler)
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
    }

    public async Task Consume(ConsumeContext<ContactCreatedEventMessage> context)
    {
        var message = context.Message;
        var command = new CreateContactCommand(message.UserId, message.ContactId);

        await _createContactCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<ContactDeletedEventMessage> context)
    {
        var message = context.Message;
        var command = new DeleteContactCommand(message.UserId, message.ContactId);

        await _deleteContactCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<CreditCardAddedEventMessage> context)
    {
        var message = context.Message;
        var command = new AddCreditCardCommand(
            message.UserId,
            message.HolderName,
            message.CardNumber,
            message.Cvv,
            message.Expiration,
            message.PaymentNetwork);

        await _addCreditCardCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<CreditCardChangedEventMessage> context)
    {
        var message = context.Message;
        var command = new ChangeCreditCardCommand(
            message.CardId, 
            message.UserId, 
            message.HolderName,
            message.CardNumber,
            message.Cvv,
            message.Expiration,
            message.PaymentNetwork);

        await _changeCreditCardCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<CreditCardDeletedEventMessage> context)
    {
        var message = context.Message;
        var command = new DeleteCreditCardCommand(message.UserId, message.CardId);

        await _deleteCreditCardCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<SupportTicketOpenedEventMessage> context)
    {
        var message = context.Message;
        var command = new OpenSupportTicketCommand(message.UserId, message.WalletId, message.TicketReason);

        await _openSupportTicketCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<SupportTicketResolvedEventMessage> context)
    {
        var message = context.Message;
        var command = new ResolveSupportTicketCommand(message.TicketId, message.TicketJustification);
        
        await _resolveSupportTicketCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<TransactionCardToUserCreatedEventMessage> context)
    {
        var message = context.Message;
        var command = new CreateTransactionCardToUserCommand(message.CreditCardId, message.ToUserId, message.Amount);

        await _createTransactionCardToUserCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<TransactionUserToUserCreatedEventMessage> context)
    {
        var message = context.Message;
        var command = new CreateTransactionUserToUserCommand(message.FromUserId, message.ToUserId, message.Amount);

        await _createTransactionUserToUserCommandHandler.HandleAsync(command);
    }
    
    public async Task Consume(ConsumeContext<TransactionRollBackedEventMessage> context)
    {
        var message = context.Message;
        var command = new RollBackTransactionCommand(message.TransactionId);

        await _rollBackTransactionCommandHandler.HandleAsync(command);
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
        var command = new NotSuspendUserCommand(message.UserId);

        await _notSuspendUserCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<UserRoleUpdatedEventMessage> context)
    {
        var message = context.Message;
        var command = new UpdateUserRoleCommand(message.UserId, message.UserRole);

        await _updateUserRoleCommandHandler.HandleAsync(command);
    }

    public async Task Consume(ConsumeContext<UserSuspendedEventMessage> context)
    {
        var message = context.Message;
        var command = new SuspendUserCommand(message.UserId);

        await _suspendUserCommandHandler.HandleAsync(command);
    }
}