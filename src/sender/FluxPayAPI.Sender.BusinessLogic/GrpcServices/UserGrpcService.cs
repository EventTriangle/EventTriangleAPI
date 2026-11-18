using FluxPayAPI.Sender.BusinessLogic.CommandHandlers;
using FluxPayAPI.Shared.Application.Proto;
using FluxPayAPI.Shared.DTO.Enums;
using Grpc.Core;

namespace FluxPayAPI.Sender.BusinessLogic.GrpcServices;

public class UserGrpcService : User.UserBase
{
    private readonly CreateUserCommandHandler _createUserCommandHandler;

    public UserGrpcService(CreateUserCommandHandler createUserCommandHandler)
    {
        _createUserCommandHandler = createUserCommandHandler;
    }

    public override async Task<CreateUserReply> CreateUser(CreateUserRequest request, ServerCallContext context)
    {
        try
        {
            var command = new CreateUserCommand(
                request.UserId, 
                request.Email,
                (UserRole)request.UserRole, 
                (UserStatus)request.UserStatus);
            
            await _createUserCommandHandler.HandleAsync(command);

            return new CreateUserReply { IsSuccess = true };
        }
        catch
        {
            return new CreateUserReply { IsSuccess = false };
        }
    }
}