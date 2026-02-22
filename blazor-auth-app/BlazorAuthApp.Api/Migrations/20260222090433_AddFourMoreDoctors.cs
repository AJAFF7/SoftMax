using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorAuthApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddFourMoreDoctors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "ConsultationFee", "CreatedAt", "Description", "Email", "FirstName", "ImageUrl", "IsAvailable", "LastName", "Phone", "Specialization", "YearsOfExperience" },
                values: new object[,]
                {
                    { 6, 140.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Expert eye specialist providing comprehensive eye care and vision solutions.", "james.williams@hospital.com", "James", "https://ui-avatars.com/api/?name=James+Williams&background=06B6D4&color=fff&size=200", true, "Williams", "+1-555-0106", "Ophthalmology", 14 },
                    { 7, 145.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Caring psychiatrist specializing in mental health and emotional well-being.", "maria.garcia@hospital.com", "Maria", "https://ui-avatars.com/api/?name=Maria+Garcia&background=EC4899&color=fff&size=200", true, "Garcia", "+1-555-0107", "Psychiatry", 11 },
                    { 8, 170.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Highly skilled surgeon with expertise in various surgical procedures.", "robert.lee@hospital.com", "Robert", "https://ui-avatars.com/api/?name=Robert+Lee&background=14B8A6&color=fff&size=200", true, "Lee", "+1-555-0108", "General Surgery", 16 },
                    { 9, 155.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Dedicated endocrinologist treating hormone disorders and metabolic conditions.", "amanda.miller@hospital.com", "Amanda", "https://ui-avatars.com/api/?name=Amanda+Miller&background=F97316&color=fff&size=200", true, "Miller", "+1-555-0109", "Endocrinology", 13 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Doctors",
                keyColumn: "Id",
                keyValue: 9);
        }
    }
}
