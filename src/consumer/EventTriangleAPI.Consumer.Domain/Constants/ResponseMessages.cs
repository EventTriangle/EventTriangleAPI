namespace EventTriangleAPI.Consumer.Domain.Constants;

public static class ResponseMessages
{
    public const string RequesterNotFound = "Requester not found.";
    public const string UserNotFound = "User not found.";
    public const string CannotCreateCreditCardWithSameCardNumber = "You cannot add 2 credit cards with the same card number.";
    public const string CreditCardNotFound = "Credit card not found.";
    public const string CannotTransferMoreMoneyThanYouHave = "You can't transfer more money than you have.";
    public const string UserAlreadyExists = "User already exists.";
    public const string ContactNotFound = "Contact not found.";
    public const string RequesterIsNotAdmin = "Requester is not admin.";
    public const string WalletNotFound = "Wallet not found.";
    public const string SupportTicketNotFound = "Support ticket not found.";
    public const string TransactionTicketNotFound = "Transaction ticket not found.";
    public const string PageCannotBeLessThanOne = "Page can't be less than one.";
    public const string TransactionNotFound = "Transaction not found.";
    public const string RequesterIsSuspended = "Requester is suspended.";
    public const string CannotSuspendAdmin = "You can't suspend an admin.";
    public const string SupportTicketAlreadyExists = "Support ticket already exists.";
    public const string TransactionHasAlreadyBeenRolledBack = "Transaction has already been rolled back.";
    public const string InternalError = "Internal error.";
}