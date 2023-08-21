using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTriangleAPI.Sender.Persistence.Migrations
{
    public partial class Inialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactCreatedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    ContactId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactCreatedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ContactDeletedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    ContactId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactDeletedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditCardAddedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    HolderName = table.Column<string>(type: "text", nullable: true),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    Cvv = table.Column<string>(type: "text", nullable: true),
                    Expiration = table.Column<string>(type: "text", nullable: true),
                    PaymentNetwork = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardAddedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditCardChangedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    HolderName = table.Column<string>(type: "text", nullable: true),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    Cvv = table.Column<string>(type: "text", nullable: true),
                    Expiration = table.Column<string>(type: "text", nullable: true),
                    PaymentNetwork = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardChangedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditCardDeletedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardDeletedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportTicketOpenedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketReason = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketOpenedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportTicketResolvedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketJustification = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketResolvedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionCardToUserCreatedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    CreditCardId = table.Column<Guid>(type: "uuid", nullable: false),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionCardToUserCreatedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionRollBackedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    TransactionId = table.Column<Guid>(type: "uuid", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionRollBackedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionUserToUserCreatedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    ToUserId = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionUserToUserCreatedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserCreatedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    Email = table.Column<string>(type: "text", nullable: true),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    UserStatus = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserCreatedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserNotSuspendedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserNotSuspendedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserRoleUpdatedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoleUpdatedEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserSuspendedEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    RequesterId = table.Column<string>(type: "text", nullable: true),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSuspendedEvents", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactCreatedEvents");

            migrationBuilder.DropTable(
                name: "ContactDeletedEvents");

            migrationBuilder.DropTable(
                name: "CreditCardAddedEvents");

            migrationBuilder.DropTable(
                name: "CreditCardChangedEvents");

            migrationBuilder.DropTable(
                name: "CreditCardDeletedEvents");

            migrationBuilder.DropTable(
                name: "SupportTicketOpenedEvents");

            migrationBuilder.DropTable(
                name: "SupportTicketResolvedEvents");

            migrationBuilder.DropTable(
                name: "TransactionCardToUserCreatedEvents");

            migrationBuilder.DropTable(
                name: "TransactionRollBackedEvents");

            migrationBuilder.DropTable(
                name: "TransactionUserToUserCreatedEvents");

            migrationBuilder.DropTable(
                name: "UserCreatedEvents");

            migrationBuilder.DropTable(
                name: "UserNotSuspendedEvents");

            migrationBuilder.DropTable(
                name: "UserRoleUpdatedEvents");

            migrationBuilder.DropTable(
                name: "UserSuspendedEvents");
        }
    }
}
