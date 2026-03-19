using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TaskFlow.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RemoveTaskOrderUniqueConstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_ProjectId_Order",
                table: "Tasks");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId_Order",
                table: "Tasks",
                columns: new[] { "ProjectId", "Order" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Tasks_ProjectId_Order",
                table: "Tasks");

            migrationBuilder.CreateIndex(
                name: "IX_Tasks_ProjectId_Order",
                table: "Tasks",
                columns: new[] { "ProjectId", "Order" },
                unique: true);
        }
    }
}
