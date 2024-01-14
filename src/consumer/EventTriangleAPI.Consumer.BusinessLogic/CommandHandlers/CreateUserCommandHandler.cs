using EventTriangleAPI.Consumer.BusinessLogic.Models;
using EventTriangleAPI.Consumer.Domain.Constants;
using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Enums;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserDto>
{
    private readonly DatabaseContext _context;
    private readonly IConfiguration _configuration;

    public CreateUserCommandHandler(DatabaseContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<IResult<UserDto, Error>> HandleAsync(CreateUserCommand command)
    {
        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.UserId);

        if (user != null)
        {
            return new Result<UserDto>(new DbEntityExistsError(ResponseMessages.UserAlreadyExists));
        }
        
        var wallet = new WalletEntity(0);
        var newUser = new UserEntity(command.UserId, command.Email, wallet.Id, command.UserRole, command.UserStatus);

        _context.WalletEntities.Add(wallet);
        _context.UserEntities.Add(newUser);
        await _context.SaveChangesAsync();

        var areThereSeeds = _configuration.GetValue<bool>(AppSettingsConstants.ShouldCreateSeeds);
        var areAdminSeedsRequired = _configuration.GetValue<bool>(AppSettingsConstants.ShouldCreateSeedsForAdmin);
        
        if (newUser.UserRole == UserRole.Admin && areThereSeeds && areAdminSeedsRequired)
        {
            await CreateSeedsForAdmin(newUser.Id);
        }
        
        var userDto = new UserDto(newUser.Id, newUser.Email, newUser.UserRole, newUser.UserStatus, newUser.WalletId, null);
        
        return new Result<UserDto>(userDto);
    }

    private async Task CreateSeedsForAdmin(string userId)
    {
        var user = await _context.UserEntities
            .Include(x => x.Wallet)
            .Include(userEntity => userEntity.Contacts)
            .FirstOrDefaultAsync(x => x.Id == userId);
        
        user.Wallet.UpdateBalance(55944903);
        
        // transactions seeds
        var alice = await _context.UserEntities.FirstAsync(x => x.Id == UserSeedIdConstants.AliceId);
        var bob = await _context.UserEntities.FirstAsync(x => x.Id == UserSeedIdConstants.BobId);

        var transactionList = new List<TransactionEntity>();

        var random = new Random();
        
        for (var i = 0; i < 30; i++)
        {
            var num = random.Next(1, 3);

            var transaction = num == 1 
                ? new TransactionEntity(userId, alice.Id, 5000, TransactionType.FromUserToUser, DateTime.UtcNow) 
                : new TransactionEntity(alice.Id, userId, 5000, TransactionType.FromUserToUser, DateTime.UtcNow);

            transactionList.Add(transaction);
        }
        
        for (var i = 0; i < 30; i++)
        {
            var num = random.Next(1, 3);

            var transaction = num == 1 
                ? new TransactionEntity(userId, bob.Id, 10000, TransactionType.FromUserToUser, DateTime.UtcNow) 
                : new TransactionEntity(bob.Id, userId, 10000, TransactionType.FromUserToUser, DateTime.UtcNow);

            transactionList.Add(transaction);
        }
        
        _context.TransactionEntities.AddRange(transactionList);
        await _context.SaveChangesAsync();
        
        // contacts
        var userAliceContact = new ContactEntity(userId, alice.Id);
        var userBobContact = new ContactEntity(userId, bob.Id);
        
        user.Contacts.AddRange(new [] {userAliceContact, userBobContact});
        await _context.SaveChangesAsync();
        
        // creditCards
        var creditCardList = new List<CreditCardEntity>();

        for (int i = 0; i < 5; i++)
        {
            var randomCardNumber = string.Concat(random.Next(1000, 9999), random.Next(1000, 9999), random.Next(1000, 9999), random.Next(1000, 9999));
            var randomCvv = random.Next(100, 999).ToString();
            var creditCard = new CreditCardEntity(userId, Guid.NewGuid().ToString(), randomCardNumber, randomCvv, "01/11", PaymentNetwork.MasterCard);
            creditCardList.Add(creditCard);
        }

        _context.CreditCardEntities.AddRange(creditCardList);
        await _context.SaveChangesAsync();
        
        // tickets
        var aliceTransaction = await _context.TransactionEntities.FirstAsync(x => x.FromUserId == alice.Id || x.ToUserId == alice.Id);
        var bobTransaction = await _context.TransactionEntities.FirstAsync(x => x.FromUserId == bob.Id || x.ToUserId == bob.Id);
        
        var aliceSupportTicket = new SupportTicketEntity(alice.Id, alice.WalletId, aliceTransaction.Id, 
            "Can you roll back the transaction, please?", DateTime.UtcNow);
        var bobSupportTicket = new SupportTicketEntity(bob.Id, bob.WalletId, bobTransaction.Id, 
            "Can you please cancel this transaction?", DateTime.UtcNow);
        
        _context.SupportTicketEntities.AddRange(aliceSupportTicket, bobSupportTicket);
        await _context.SaveChangesAsync();
    }
}