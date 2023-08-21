using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EventTriangleAPI.Consumer.Persistence.Migrations
{
    public partial class Initialize : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ContactEntities",
                columns: table => new
                {
                    UserId = table.Column<string>(type: "text", nullable: false),
                    ContactId = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ContactEntities", x => new { x.UserId, x.ContactId });
                });

            migrationBuilder.CreateTable(
                name: "CreditCardEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    HolderName = table.Column<string>(type: "text", nullable: true),
                    CardNumber = table.Column<string>(type: "text", nullable: true),
                    Cvv = table.Column<string>(type: "text", nullable: true),
                    Expiration = table.Column<string>(type: "text", nullable: true),
                    PaymentNetwork = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CreditCardEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SupportTicketEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    UserId = table.Column<string>(type: "text", nullable: true),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketReason = table.Column<string>(type: "text", nullable: true),
                    TicketJustification = table.Column<string>(type: "text", nullable: true),
                    TicketStatus = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupportTicketEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransactionEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FromUserId = table.Column<string>(type: "text", nullable: true),
                    ToUserId = table.Column<string>(type: "text", nullable: true),
                    Amount = table.Column<decimal>(type: "numeric", nullable: false),
                    TransactionState = table.Column<int>(type: "integer", nullable: false),
                    TransactionType = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransactionEntities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WalletEntities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Balance = table.Column<decimal>(type: "numeric", nullable: false),
                    LastTransactionId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WalletEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WalletEntities_TransactionEntities_LastTransactionId",
                        column: x => x.LastTransactionId,
                        principalTable: "TransactionEntities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserEntities",
                columns: table => new
                {
                    Id = table.Column<string>(type: "text", nullable: false),
                    Email = table.Column<string>(type: "text", nullable: true),
                    UserRole = table.Column<int>(type: "integer", nullable: false),
                    UserStatus = table.Column<int>(type: "integer", nullable: false),
                    WalletId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEntities", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserEntities_WalletEntities_WalletId",
                        column: x => x.WalletId,
                        principalTable: "WalletEntities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContactEntities_ContactId",
                table: "ContactEntities",
                column: "ContactId");

            migrationBuilder.CreateIndex(
                name: "IX_CreditCardEntities_UserId",
                table: "CreditCardEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketEntities_UserId",
                table: "SupportTicketEntities",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_SupportTicketEntities_WalletId",
                table: "SupportTicketEntities",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntities_FromUserId",
                table: "TransactionEntities",
                column: "FromUserId");

            migrationBuilder.CreateIndex(
                name: "IX_TransactionEntities_ToUserId",
                table: "TransactionEntities",
                column: "ToUserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserEntities_WalletId",
                table: "UserEntities",
                column: "WalletId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletEntities_LastTransactionId",
                table: "WalletEntities",
                column: "LastTransactionId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContactEntities_UserEntities_ContactId",
                table: "ContactEntities",
                column: "ContactId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ContactEntities_UserEntities_UserId",
                table: "ContactEntities",
                column: "UserId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_CreditCardEntities_UserEntities_UserId",
                table: "CreditCardEntities",
                column: "UserId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTicketEntities_UserEntities_UserId",
                table: "SupportTicketEntities",
                column: "UserId",
                principalTable: "UserEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SupportTicketEntities_WalletEntities_WalletId",
                table: "SupportTicketEntities",
                column: "WalletId",
                principalTable: "WalletEntities",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionEntities_UserEntities_FromUserId",
                table: "TransactionEntities",
                column: "FromUserId",
                principalTable: "UserEntities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TransactionEntities_UserEntities_ToUserId",
                table: "TransactionEntities",
                column: "ToUserId",
                principalTable: "UserEntities",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransactionEntities_UserEntities_FromUserId",
                table: "TransactionEntities");

            migrationBuilder.DropForeignKey(
                name: "FK_TransactionEntities_UserEntities_ToUserId",
                table: "TransactionEntities");

            migrationBuilder.DropTable(
                name: "ContactEntities");

            migrationBuilder.DropTable(
                name: "CreditCardEntities");

            migrationBuilder.DropTable(
                name: "SupportTicketEntities");

            migrationBuilder.DropTable(
                name: "UserEntities");

            migrationBuilder.DropTable(
                name: "WalletEntities");

            migrationBuilder.DropTable(
                name: "TransactionEntities");
        }
    }
}
