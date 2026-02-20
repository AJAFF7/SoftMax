using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace BlazorAuthApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddAssistantSupport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CheckedInByAssistantId",
                table: "Appointments",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Assistants",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FullName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    PhoneNumber = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ETagBarcode = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assistants", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_CheckedInByAssistantId",
                table: "Appointments",
                column: "CheckedInByAssistantId");

            migrationBuilder.CreateIndex(
                name: "IX_Assistants_Email",
                table: "Assistants",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assistants_ETagBarcode",
                table: "Assistants",
                column: "ETagBarcode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Assistants_Username",
                table: "Assistants",
                column: "Username",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Appointments_Assistants_CheckedInByAssistantId",
                table: "Appointments",
                column: "CheckedInByAssistantId",
                principalTable: "Assistants",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Appointments_Assistants_CheckedInByAssistantId",
                table: "Appointments");

            migrationBuilder.DropTable(
                name: "Assistants");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_CheckedInByAssistantId",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "CheckedInByAssistantId",
                table: "Appointments");
        }
    }
}
