using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Crm.DbMigrations.App.Migrations
{
    /// <inheritdoc />
    public partial class Modify_WithdrawalRequest_AddField_Fee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "Fee",
                table: "WithdrawalRequests",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Fee",
                table: "WithdrawalRequests");
        }
    }
}
