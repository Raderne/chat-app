using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatMessagesApp.Infrastructure.Migrations.Application
{
    /// <inheritdoc />
    public partial class AddConversationEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Message_IsRead",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "Messages");

            migrationBuilder.AddColumn<Guid>(
                name: "ConversationId",
                table: "Messages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "MessageStatus",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ParticipantIds = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Uuid = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: true),
                    RowVersion = table.Column<byte[]>(type: "varbinary(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Message_MessageStatus",
                table: "Messages",
                column: "MessageStatus");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages",
                column: "ConversationId",
                principalTable: "Conversations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Conversations_ConversationId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Message_MessageStatus",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ConversationId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "MessageStatus",
                table: "Messages");

            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "Messages",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Message_IsRead",
                table: "Messages",
                column: "IsRead");
        }
    }
}
