using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatMessagesApp.Infrastructure.Migrations.Application
{
    /// <inheritdoc />
    public partial class FixConversationV2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Message_RecipientId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Message_SenderId_RecipientId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "RecipientId",
                table: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                newName: "IX_Message_ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId_ConversationId",
                table: "Messages",
                columns: new[] { "SenderId", "ConversationId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Message_SenderId_ConversationId",
                table: "Messages");

            migrationBuilder.RenameIndex(
                name: "IX_Message_ConversationId",
                table: "Messages",
                newName: "IX_Messages_ConversationId");

            migrationBuilder.AddColumn<string>(
                name: "RecipientId",
                table: "Messages",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Message_RecipientId",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Message_SenderId_RecipientId",
                table: "Messages",
                columns: new[] { "SenderId", "RecipientId" });
        }
    }
}
