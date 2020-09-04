using Microsoft.EntityFrameworkCore.Migrations;

namespace bankAccounts.Migrations
{
    public partial class MoreMigrations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_users_OwnerOfAccountUserId",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_OwnerOfAccountUserId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "OwnerOfAccountUserId",
                table: "transactions");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "transactions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_UserId",
                table: "transactions",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_users_UserId",
                table: "transactions",
                column: "UserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_transactions_users_UserId",
                table: "transactions");

            migrationBuilder.DropIndex(
                name: "IX_transactions_UserId",
                table: "transactions");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "transactions");

            migrationBuilder.AddColumn<int>(
                name: "OwnerOfAccountUserId",
                table: "transactions",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_OwnerOfAccountUserId",
                table: "transactions",
                column: "OwnerOfAccountUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_transactions_users_OwnerOfAccountUserId",
                table: "transactions",
                column: "OwnerOfAccountUserId",
                principalTable: "users",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
