using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixEmailConf : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailConfirmations_Users_UserId1",
                table: "EmailConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_EmailConfirmations_UserId",
                table: "EmailConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_EmailConfirmations_UserId1",
                table: "EmailConfirmations");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "EmailConfirmations");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmations_UserId",
                table: "EmailConfirmations",
                column: "UserId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_EmailConfirmations_UserId",
                table: "EmailConfirmations");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "EmailConfirmations",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmations_UserId",
                table: "EmailConfirmations",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_EmailConfirmations_UserId1",
                table: "EmailConfirmations",
                column: "UserId1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_EmailConfirmations_Users_UserId1",
                table: "EmailConfirmations",
                column: "UserId1",
                principalTable: "Users",
                principalColumn: "Id");
        }
    }
}
