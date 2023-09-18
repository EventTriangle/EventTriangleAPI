using EventTriangleAPI.Sender.Application.Services;
using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Sender.BusinessLogic.Models.Requests;
using EventTriangleAPI.Sender.Domain.Entities;
using EventTriangleAPI.Shared.Application.Extensions;
using EventTriangleAPI.Shared.DTO.Responses.Errors;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.Annotations;

namespace EventTriangleAPI.Sender.Presentation.Controllers;

[Authorize(Roles = "Admin")]
[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly UpdateUserRoleCommandHandler _updateUserRoleCommandHandler;
    private readonly SuspendUserCommandHandler _suspendUserCommandHandler;
    private readonly NotSuspendUserCommandHandler _notSuspendUserCommandHandler;
    private readonly UserClaimsService _userClaimsService;

    public UsersController(
        UserClaimsService userClaimsService, 
        SuspendUserCommandHandler suspendUserCommandHandler, 
        NotSuspendUserCommandHandler notSuspendUserCommandHandler, 
        UpdateUserRoleCommandHandler updateUserRoleCommandHandler)
    {
        _userClaimsService = userClaimsService;
        _suspendUserCommandHandler = suspendUserCommandHandler;
        _notSuspendUserCommandHandler = notSuspendUserCommandHandler;
        _updateUserRoleCommandHandler = updateUserRoleCommandHandler;
    }

    /// <summary>
    /// Suspend a user
    /// </summary>
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserSuspendedEvent), StatusCodes.Status200OK)]
    [HttpPost("suspend")]
    public async Task<IActionResult> SuspendUser([FromBody] SuspendUserRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new SuspendUserCommand(requesterId, request.UserId);
        var result = await _suspendUserCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Update user's role
    /// </summary>
    [SwaggerOperation(Description = "")]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserRoleUpdatedEvent), StatusCodes.Status200OK)]
    [HttpPut("role")]
    public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new UpdateUserRoleCommand(requesterId, request.UserId, request.UserRole);
        var result = await _updateUserRoleCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
    
    /// <summary>
    /// Stop suspending the user
    /// </summary>
    [SwaggerOperation(Description = "")]
    [ProducesResponseType(typeof(Error), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserNotSuspendedEvent), StatusCodes.Status200OK)]
    [HttpDelete("suspend")]
    public async Task<IActionResult> NotSuspendUser([FromBody] NotSuspendUserRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new NotSuspendUserCommand(requesterId, request.UserId);
        var result = await _notSuspendUserCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
}