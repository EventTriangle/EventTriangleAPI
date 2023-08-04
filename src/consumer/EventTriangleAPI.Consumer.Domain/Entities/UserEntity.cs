using EventTriangleAPI.Consumer.Domain.Entities.Validation;
using EventTriangleAPI.Shared.DTO.Enums;
using FluentValidation;

namespace EventTriangleAPI.Consumer.Domain.Entities;

public class UserEntity
{
    public string Id { get; private set; }

    public string Email { get; private set; }
    
    public UserRole UserRole { get; private set; }
    
    public UserStatus UserStatus { get; private set; }

    public Guid WalletId { get; set; }
    
    public WalletEntity Wallet { get; private set; }

    public List<ContactEntity> Contacts { get; private set; } = new();
    
    public UserEntity(string id, string email, Guid walletId, UserRole userRole, UserStatus userStatus)
    {
        Id = id;
        Email = email;
        UserRole = userRole;
        UserStatus = userStatus;
        WalletId = walletId;

        new UserEntityValidator().ValidateAndThrow(this);
    }
}