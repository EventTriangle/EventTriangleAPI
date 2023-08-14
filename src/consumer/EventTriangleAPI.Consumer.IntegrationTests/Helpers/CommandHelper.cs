using EventTriangleAPI.Consumer.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.IntegrationTests.Helpers;

public static class CommandHelper
{
    private const string DimaEmail = "dima@gmail.com";
    private const string AliceEmail = "alice@gmail.com";
    private const string BobEmail = "bob@gmail.com";

    public static CreateUserCommand CreateUserDimaCommand()
    {
        return new CreateUserCommand(
            Guid.NewGuid().ToString(),
            DimaEmail,
            UserRole.Admin,
            UserStatus.Active);
    }
    
    public static CreateUserCommand CreateUserAliceCommand()
    {
        return new CreateUserCommand(
            Guid.NewGuid().ToString(),
            AliceEmail,
            UserRole.User,
            UserStatus.Active);
    }
    
    public static CreateUserCommand CreateUserBobCommand()
    {
        return new CreateUserCommand(
            Guid.NewGuid().ToString(),
            BobEmail,
            UserRole.User,
            UserStatus.Active);
    }
}