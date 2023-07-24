using EventTriangleAPI.Sender.Domain.Proto;
using EventTriangleAPI.Sender.Persistence;
using EventTriangleAPI.Shared.Domain.Enums;
using EventTriangleAPI.Shared.Domain.Events;
using Grpc.Core;

namespace EventTriangleAPI.Sender.BusinessLogic.GrpcServices;

public class UserGrpcService : User.UserBase
{
    private readonly DatabaseContext _context;

    public UserGrpcService(DatabaseContext context)
    {
        _context = context;
    }

    public override async Task<CreateUserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        try
        {
            var userCreatedEvent = new UserCreatedEvent(
                request.UserId, 
                (UserRole)request.UserRole, 
                (UserStatus)request.UserStatus);

            _context.UserCreatedEvents.Add(userCreatedEvent);
            await _context.SaveChangesAsync();

            return new CreateUserReply {WasUserCreated = true};
        }
        catch
        {
            return new CreateUserReply {WasUserCreated = false};
        }
    }
}