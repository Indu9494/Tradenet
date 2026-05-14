using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Goverment.Migrations
{
    /// <inheritdoc />
    public partial class AddSubsidyTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Subsidies",
                columns: table => new
                {
                    SubsidyID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BusinessID = table.Column<int>(type: "int", nullable: false),
                    ProgramID = table.Column<int>(type: "int", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    ApplicationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ApprovalDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DisbursementDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RejectionReason = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Notes = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subsidies", x => x.SubsidyID);
                    table.ForeignKey(
                        name: "FK_Subsidies_Businesses_BusinessID",
                        column: x => x.BusinessID,
                        principalTable: "Businesses",
                        principalColumn: "BusinessID",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subsidies_TradePrograms_ProgramID",
                        column: x => x.ProgramID,
                        principalTable: "TradePrograms",
                        principalColumn: "ProgramID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subsidies_BusinessID",
                table: "Subsidies",
                column: "BusinessID");

            migrationBuilder.CreateIndex(
                name: "IX_Subsidies_ProgramID",
                table: "Subsidies",
                column: "ProgramID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subsidies");
        }
    }
}
