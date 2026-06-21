using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Eligibility.Api.Infrastructure.Data.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EligibilityResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ApplicationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CustomerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Decision = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RuleVersion = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    EvaluatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    RequestedAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DeclaredMonthlyIncome = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ExistingEmiObligations = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DebtToIncomeRatio = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DecisionSummary = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EligibilityResults", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RuleResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RuleCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    RuleName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Passed = table.Column<bool>(type: "bit", nullable: false),
                    ActualValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExpectedValue = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Explanation = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    EligibilityResultId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RuleResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RuleResults_EligibilityResults_EligibilityResultId",
                        column: x => x.EligibilityResultId,
                        principalTable: "EligibilityResults",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EligibilityResults_ApplicationId",
                table: "EligibilityResults",
                column: "ApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_RuleResults_EligibilityResultId",
                table: "RuleResults",
                column: "EligibilityResultId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RuleResults");

            migrationBuilder.DropTable(
                name: "EligibilityResults");
        }
    }
}
