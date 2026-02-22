using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAuthApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class RemoveFaceDescriptor : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FaceDescriptor",
                table: "Assistants");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FaceDescriptor",
                table: "Assistants",
                type: "text",
                nullable: true);
        }
    }
}
