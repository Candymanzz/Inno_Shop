using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class PasswordResetreletions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "Used",
                table: "PasswordResetTokens",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "PasswordResetTokens",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "UserId1",
                table: "EmailConfirmations",
                type: "uuid",
                nullable: true);

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EmailConfirmations_Users_UserId1",
                table: "EmailConfirmations");

            migrationBuilder.DropIndex(
                name: "IX_EmailConfirmations_UserId1",
                table: "EmailConfirmations");

            migrationBuilder.DropColumn(
                name: "UserId1",
                table: "EmailConfirmations");

            migrationBuilder.AlterColumn<bool>(
                name: "Used",
                table: "PasswordResetTokens",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: false);

            migrationBuilder.AlterColumn<string>(
                name: "Token",
                table: "PasswordResetTokens",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(200)",
                oldMaxLength: 200);
        }
    }
}
