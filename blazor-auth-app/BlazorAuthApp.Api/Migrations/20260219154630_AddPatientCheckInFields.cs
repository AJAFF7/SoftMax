using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BlazorAuthApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddPatientCheckInFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "FinishedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsFinished",
                table: "Appointments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PatientArrived",
                table: "Appointments",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PatientArrivedAt",
                table: "Appointments",
                type: "timestamp with time zone",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FinishedAt",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "IsFinished",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PatientArrived",
                table: "Appointments");

            migrationBuilder.DropColumn(
                name: "PatientArrivedAt",
                table: "Appointments");
        }
    }
}
