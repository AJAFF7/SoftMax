using Microsoft.EntityFrameworkCore;
using BlazorAuthApp.Api.Models;

namespace BlazorAuthApp.Api.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Username).IsUnique();
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.HasIndex(e => e.Email);
        });

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.HasOne(a => a.User)
                .WithMany(u => u.Appointments)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(a => a.Doctor)
                .WithMany(d => d.Appointments)
                .HasForeignKey(a => a.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(e => e.AppointmentDate);
            entity.HasIndex(e => e.Status);
        });

        // Seed some doctors
        modelBuilder.Entity<Doctor>().HasData(
            new Doctor
            {
                Id = 1,
                FirstName = "Sarah",
                LastName = "Johnson",
                Specialization = "Cardiology",
                Description = "Experienced cardiologist specializing in heart disease prevention and treatment.",
                Email = "sarah.johnson@hospital.com",
                Phone = "+1-555-0101",
                ImageUrl = "https://ui-avatars.com/api/?name=Sarah+Johnson&background=4F46E5&color=fff&size=200",
                ConsultationFee = 150.00m,
                YearsOfExperience = 12,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Doctor
            {
                Id = 2,
                FirstName = "Michael",
                LastName = "Chen",
                Specialization = "Pediatrics",
                Description = "Compassionate pediatrician dedicated to children's health and wellness.",
                Email = "michael.chen@hospital.com",
                Phone = "+1-555-0102",
                ImageUrl = "https://ui-avatars.com/api/?name=Michael+Chen&background=10B981&color=fff&size=200",
                ConsultationFee = 120.00m,
                YearsOfExperience = 8,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Doctor
            {
                Id = 3,
                FirstName = "Emily",
                LastName = "Rodriguez",
                Specialization = "Dermatology",
                Description = "Expert dermatologist focusing on skin health and cosmetic procedures.",
                Email = "emily.rodriguez@hospital.com",
                Phone = "+1-555-0103",
                ImageUrl = "https://ui-avatars.com/api/?name=Emily+Rodriguez&background=F59E0B&color=fff&size=200",
                ConsultationFee = 130.00m,
                YearsOfExperience = 10,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Doctor
            {
                Id = 4,
                FirstName = "David",
                LastName = "Patel",
                Specialization = "Orthopedics",
                Description = "Skilled orthopedic surgeon specializing in joint and bone treatments.",
                Email = "david.patel@hospital.com",
                Phone = "+1-555-0104",
                ImageUrl = "https://ui-avatars.com/api/?name=David+Patel&background=EF4444&color=fff&size=200",
                ConsultationFee = 160.00m,
                YearsOfExperience = 15,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            },
            new Doctor
            {
                Id = 5,
                FirstName = "Lisa",
                LastName = "Thompson",
                Specialization = "Neurology",
                Description = "Renowned neurologist with expertise in brain and nervous system disorders.",
                Email = "lisa.thompson@hospital.com",
                Phone = "+1-555-0105",
                ImageUrl = "https://ui-avatars.com/api/?name=Lisa+Thompson&background=8B5CF6&color=fff&size=200",
                ConsultationFee = 180.00m,
                YearsOfExperience = 18,
                IsAvailable = true,
                CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            }
        );
    }
}
