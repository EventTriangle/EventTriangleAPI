using EventTriangleAPI.Shared.DTO.Enums;

namespace EventTriangleAPI.Consumer.BusinessLogic.Models;

public record UserDto(
    string Id,
    string Email, 
    UserRole UserRole,
    UserStatus UserStatus,
    WalletDto Wallet);