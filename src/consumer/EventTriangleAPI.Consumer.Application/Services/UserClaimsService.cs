using Microsoft.AspNetCore.Http;
using Microsoft.Identity.Web;

namespace EventTriangleAPI.Consumer.Application.Services;

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
            throw new Exception("User Id not found");
        }

        return userId;
    }
}