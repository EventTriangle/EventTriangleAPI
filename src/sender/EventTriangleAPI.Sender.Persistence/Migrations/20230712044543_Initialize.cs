using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTriangleAPI.Sender.Persistence.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactId = table.Column<Guid>(type: "uuid", nullable: false),
                    ContactEventType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CreditCardEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CardId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    HolderName = table.Column<string>(type: "text", nullable: true),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    Cvv = table.Column<string>(type: "text", nullable: true),
                    PaymentNetwork = table.Column<int>(type: "integer", nullable: false),
                    CardEventType = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportTicketEvents",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    Username = table.Column<string>(type: "text", nullable: true),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketReason = table.Column<string>(type: "text", nullable: true),
                    TicketStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketEvents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionEventEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    From = table.Column<string>(type: "text", nullable: true),
                    To = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    CreateAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionEventEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "UserEventEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<Guid>(type: "uuid", nullable: false),
                    UserEventType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEventEntities", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ContactEvents");

            migrationBuilder.DropTable(
                name: "CreditCardEvents");

            migrationBuilder.DropTable(
                name: "SupportTicketEvents");

            migrationBuilder.DropTable(
                name: "TransactionEventEntities");

            migrationBuilder.DropTable(
                name: "UserEventEntities");
        }
    }
}
