using EventTriangleAPI.Consumer.Domain.Entities;
using EventTriangleAPI.Consumer.Persistence;
using EventTriangleAPI.Shared.Application.Abstractions;
using EventTriangleAPI.Shared.DTO.Abstractions;
using EventTriangleAPI.Shared.DTO.Responses;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.EntityFrameworkCore;

namespace EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;

public class CreateUserCommandHandler : ICommandHandler<CreateUserCommand, UserEntity>
{
    private readonly DatabaseContext _context;

    public CreateUserCommandHandler(DatabaseContext context)
    {
        _context = context;
    }

    public async Task<IResult<UserEntity, Error>> HandleAsync(CreateUserCommand command)
    {
        var user = await _context.UserEntities.FirstOrDefaultAsync(x => x.Id == command.UserId);

        if (user != null)
        {
            return new Result<UserEntity>(new DbEntityExistsError("User already exists"));
        }
        
        var wallet = new WalletEntity(0);
        var newUser = new UserEntity(command.UserId, command.Email, wallet.Id, command.UserRole, command.UserStatus);

        _context.WalletEntities.Add(wallet);
        _context.UserEntities.Add(newUser);
        await _context.SaveChangesAsync();

        return new Result<UserEntity>(newUser);
    }
}