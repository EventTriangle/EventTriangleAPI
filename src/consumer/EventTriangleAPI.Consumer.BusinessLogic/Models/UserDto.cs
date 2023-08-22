using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public class UserDto
{
    public string Id { get; set; }

    public string Email { get; set; }
    
    public UserRole UserRole { get; set; }
    
    public UserStatus UserStatus { get; set; }

    public WalletDto Wallet { get; set; }
}