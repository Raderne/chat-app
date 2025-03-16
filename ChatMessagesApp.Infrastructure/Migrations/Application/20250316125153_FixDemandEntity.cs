using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatMessagesApp.Infrastructure.Migrations.Application
{
    /// <inheritdoc />
    public partial class FixDemandEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CreatedByUserId",
                table: "Demands",
                newName: "ToUserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ToUserId",
                table: "Demands",
                newName: "CreatedByUserId");
        }
    }
}
