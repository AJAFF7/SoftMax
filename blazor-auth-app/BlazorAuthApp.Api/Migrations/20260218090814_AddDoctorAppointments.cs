using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace BlazorAuthApp.Api.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorAppointments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Doctors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Specialization = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ImageUrl = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ConsultationFee = table.Column<decimal>(type: "numeric", nullable: false),
                    YearsOfExperience = table.Column<int>(type: "integer", nullable: false),
                    IsAvailable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Doctors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PasswordHash = table.Column<string>(type: "text", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Email = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    Phone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Appointments",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    UserId = table.Column<int>(type: "integer", nullable: false),
                    DoctorId = table.Column<int>(type: "integer", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeSlot = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PatientName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    PatientEmail = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    PatientPhone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Symptoms = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    Status = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Appointments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Appointments_Doctors_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "Doctors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Appointments_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Doctors",
                columns: new[] { "Id", "ConsultationFee", "CreatedAt", "Description", "Email", "FirstName", "ImageUrl", "IsAvailable", "LastName", "Phone", "Specialization", "YearsOfExperience" },
                values: new object[,]
                {
                    { 1, 150.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Experienced cardiologist specializing in heart disease prevention and treatment.", "sarah.johnson@hospital.com", "Sarah", "https://ui-avatars.com/api/?name=Sarah+Johnson&background=4F46E5&color=fff&size=200", true, "Johnson", "+1-555-0101", "Cardiology", 12 },
                    { 2, 120.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Compassionate pediatrician dedicated to children's health and wellness.", "michael.chen@hospital.com", "Michael", "https://ui-avatars.com/api/?name=Michael+Chen&background=10B981&color=fff&size=200", true, "Chen", "+1-555-0102", "Pediatrics", 8 },
                    { 3, 130.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Expert dermatologist focusing on skin health and cosmetic procedures.", "emily.rodriguez@hospital.com", "Emily", "https://ui-avatars.com/api/?name=Emily+Rodriguez&background=F59E0B&color=fff&size=200", true, "Rodriguez", "+1-555-0103", "Dermatology", 10 },
                    { 4, 160.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Skilled orthopedic surgeon specializing in joint and bone treatments.", "david.patel@hospital.com", "David", "https://ui-avatars.com/api/?name=David+Patel&background=EF4444&color=fff&size=200", true, "Patel", "+1-555-0104", "Orthopedics", 15 },
                    { 5, 180.00m, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Renowned neurologist with expertise in brain and nervous system disorders.", "lisa.thompson@hospital.com", "Lisa", "https://ui-avatars.com/api/?name=Lisa+Thompson&background=8B5CF6&color=fff&size=200", true, "Thompson", "+1-555-0105", "Neurology", 18 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentDate",
                table: "Appointments",
                column: "AppointmentDate");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_DoctorId",
                table: "Appointments",
                column: "DoctorId");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Status",
                table: "Appointments",
                column: "Status");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_UserId",
                table: "Appointments",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_Email",
                table: "Doctors",
                column: "Email");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Appointments");

            migrationBuilder.DropTable(
                name: "Doctors");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
