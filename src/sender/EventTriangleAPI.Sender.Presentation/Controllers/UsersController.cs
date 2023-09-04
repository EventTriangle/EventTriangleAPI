using EventTriangleAPI.Sender.Application.Services;
using EventTriangleAPI.Sender.BusinessLogic.CommandHandlers;
using EventTriangleAPI.Sender.BusinessLogic.Models.Requests;
using EventTriangleAPI.Shared.Application.Extensions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventTriangleAPI.Sender.Presentation.Controllers;

[Authorize]
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

    [HttpPost("suspect")]
    public async Task<IActionResult> SuspectUser([FromBody] SuspectUserRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new SuspendUserCommand(requesterId, request.UserId);
        var result = await _suspendUserCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
    
    [HttpPut("update-role")]
    public async Task<IActionResult> UpdateUserRole([FromBody] UpdateUserRoleRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new UpdateUserRoleCommand(requesterId, request.UserId, request.UserRole);
        var result = await _updateUserRoleCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
    
    [HttpDelete("suspect")]
    public async Task<IActionResult> NotSuspectUser([FromBody] NotSuspectUserRequest request)
    {
        var requesterId = _userClaimsService.GetUserId();

        var command = new NotSuspendUserCommand(requesterId, request.UserId);
        var result = await _notSuspendUserCommandHandler.HandleAsync(command);

        return result.ToActionResult();
    }
}