using EventTriangleAPI.Consumer.IntegrationTests.Helpers;
using EventTriangleAPI.Shared.DTO.Responses;
using FluentAssertions;
using Xunit;

namespace EventTriangleAPI.Consumer.IntegrationTests.CommandHandlerTests.AddCreditCardCommandHandlerTests;

public class AddCreditCardTestThrowEntityNotFound : IntegrationTestBase
{
    [Fact]
    public async Task TestRequesterNotFound()
    {
        var addCreditCardCommand = AddCreditCardCommandHelper.CreateCreditCardCommand(Guid.NewGuid().ToString());
        var addCreditCardResult = await AddCreditCardCommandHandler.HandleAsync(addCreditCardCommand);

        addCreditCardResult.Error.Should().BeOfType<DbEntityNotFoundError>();
    }
}