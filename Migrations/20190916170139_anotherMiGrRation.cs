using Microsoft.EntityFrameworkCore.Migrations;

namespace bankAccounts.Migrations
{
    public partial class anotherMiGrRation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Amount",
                table: "transactions",
                nullable: false,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<float>(
                name: "Amount",
                table: "transactions",
                nullable: false,
                oldClrType: typeof(int));
        }
    }
}
