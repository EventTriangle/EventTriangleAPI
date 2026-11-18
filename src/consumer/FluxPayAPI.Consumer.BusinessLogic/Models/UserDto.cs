using FluxPayAPI.Shared.DTO.Enums;

namespace FluxPayAPI.Consumer.BusinessLogic.Models;

public record UserDto(
    string Id,
    string Email, 
    UserRole UserRole,
    UserStatus UserStatus,
    Guid WalletId,
    WalletDto Wallet);