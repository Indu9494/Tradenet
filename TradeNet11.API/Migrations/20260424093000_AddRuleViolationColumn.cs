using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TradeNet11.API.Migrations
{
    /// <inheritdoc />
    public partial class AddRuleViolationColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RuleViolation",
                table: "ComplianceRecords",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RuleViolation",
                table: "ComplianceRecords");
        }
    }
}
