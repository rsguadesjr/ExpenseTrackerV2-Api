using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExpenseTracker.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCategoryDeleteBehaviorNull : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                table: "Transaction",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                table: "Transaction");

            migrationBuilder.AddForeignKey(
                name: "FK_Transaction_Category_CategoryId",
                table: "Transaction",
                column: "CategoryId",
                principalTable: "Category",
                principalColumn: "Id");
        }
    }
}
