using EventTriangleAPI.Shared.Application.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;

namespace EventTriangleAPI.Sender.Application.Services;

public class UserClaimsService
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UserClaimsService(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public string GetUserId()
    {
        var context = _httpContextAccessor.HttpContext;
        
        var userId = context?.User.Claims.FirstOrDefault(x => x.Type == ClaimConstants.NameIdentifierId)?.Value;

        if (userId == null)
        {
            throw new UserClaimsException("UserId not found");
        }

        return userId;
    }
}