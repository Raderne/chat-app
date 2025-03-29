using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ChatMessagesApp.Infrastructure.Migrations.Application
{
    /// <inheritdoc />
    public partial class FixConversation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DemandId",
                table: "Conversations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Conversations_DemandId",
                table: "Conversations",
                column: "DemandId");

            migrationBuilder.AddForeignKey(
                name: "FK_Conversations_Demands_DemandId",
                table: "Conversations",
                column: "DemandId",
                principalTable: "Demands",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Conversations_Demands_DemandId",
                table: "Conversations");

            migrationBuilder.DropIndex(
                name: "IX_Conversations_DemandId",
                table: "Conversations");

            migrationBuilder.DropColumn(
                name: "DemandId",
                table: "Conversations");
        }
    }
}
