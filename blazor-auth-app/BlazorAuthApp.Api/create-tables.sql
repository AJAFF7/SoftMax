-- Create Doctors table
CREATE TABLE IF NOT EXISTS "Doctors" (
    "Id" SERIAL PRIMARY KEY,
    "FirstName" VARCHAR(100) NOT NULL,
    "LastName" VARCHAR(100) NOT NULL,
    "Specialization" VARCHAR(100) NOT NULL,
    "Description" VARCHAR(500) NOT NULL,
    "Email" VARCHAR(200) NOT NULL,
    "Phone" VARCHAR(20) NOT NULL,
    "ImageUrl" VARCHAR(500) NOT NULL,
    "ConsultationFee" DECIMAL NOT NULL,
    "YearsOfExperience" INTEGER NOT NULL,
    "IsAvailable" BOOLEAN NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL
);

-- Create Appointments table
CREATE TABLE IF NOT EXISTS "Appointments" (
    "Id" SERIAL PRIMARY KEY,
    "UserId" INTEGER NOT NULL,
    "DoctorId" INTEGER NOT NULL,
    "AppointmentDate" TIMESTAMP WITH TIME ZONE NOT NULL,
    "TimeSlot" VARCHAR(50) NOT NULL,
    "PatientName" VARCHAR(100) NOT NULL,
    "PatientEmail" VARCHAR(200) NOT NULL,
    "PatientPhone" VARCHAR(20) NOT NULL,
    "Symptoms" VARCHAR(500) NOT NULL,
    "Status" VARCHAR(50) NOT NULL,
    "CreatedAt" TIMESTAMP WITH TIME ZONE NOT NULL,
    "UpdatedAt" TIMESTAMP WITH TIME ZONE,
    CONSTRAINT "FK_Appointments_Doctors_DoctorId" FOREIGN KEY ("DoctorId") REFERENCES "Doctors" ("Id") ON DELETE RESTRICT,
    CONSTRAINT "FK_Appointments_Users_UserId" FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE RESTRICT
);

-- Create indexes
CREATE INDEX IF NOT EXISTS "IX_Appointments_AppointmentDate" ON "Appointments" ("AppointmentDate");
CREATE INDEX IF NOT EXISTS "IX_Appointments_DoctorId" ON "Appointments" ("DoctorId");
CREATE INDEX IF NOT EXISTS "IX_Appointments_Status" ON "Appointments" ("Status");
CREATE INDEX IF NOT EXISTS "IX_Appointments_UserId" ON "Appointments" ("UserId");
CREATE INDEX IF NOT EXISTS "IX_Doctors_Email" ON "Doctors" ("Email");

-- Seed doctors
INSERT INTO "Doctors" ("Id", "FirstName", "LastName", "Specialization", "Description", "Email", "Phone", "ImageUrl", "ConsultationFee", "YearsOfExperience", "IsAvailable", "CreatedAt")
VALUES 
(1, 'Sarah', 'Johnson', 'Cardiology', 'Experienced cardiologist specializing in heart disease prevention and treatment.', 'sarah.johnson@hospital.com', '+1-555-0101', 'https://ui-avatars.com/api/?name=Sarah+Johnson&background=4F46E5&color=fff&size=200', 150.00, 12, true, '2024-01-01 00:00:00+00'),
(2, 'Michael', 'Chen', 'Pediatrics', 'Compassionate pediatrician dedicated to children''s health and wellness.', 'michael.chen@hospital.com', '+1-555-0102', 'https://ui-avatars.com/api/?name=Michael+Chen&background=10B981&color=fff&size=200', 120.00, 8, true, '2024-01-01 00:00:00+00'),
(3, 'Emily', 'Rodriguez', 'Dermatology', 'Expert dermatologist focusing on skin health and cosmetic procedures.', 'emily.rodriguez@hospital.com', '+1-555-0103', 'https://ui-avatars.com/api/?name=Emily+Rodriguez&background=F59E0B&color=fff&size=200', 130.00, 10, true, '2024-01-01 00:00:00+00'),
(4, 'David', 'Patel', 'Orthopedics', 'Skilled orthopedic surgeon specializing in joint and bone treatments.', 'david.patel@hospital.com', '+1-555-0104', 'https://ui-avatars.com/api/?name=David+Patel&background=EF4444&color=fff&size=200', 160.00, 15, true, '2024-01-01 00:00:00+00'),
(5, 'Lisa', 'Thompson', 'Neurology', 'Renowned neurologist with expertise in brain and nervous system disorders.', 'lisa.thompson@hospital.com', '+1-555-0105', 'https://ui-avatars.com/api/?name=Lisa+Thompson&background=8B5CF6&color=fff&size=200', 180.00, 18, true, '2024-01-01 00:00:00+00'),
(6, 'James', 'Wilson', 'Psychiatry', 'Experienced psychiatrist specializing in mental health and behavioral therapy.', 'james.wilson@hospital.com', '+1-555-0106', 'https://ui-avatars.com/api/?name=James+Wilson&background=06B6D4&color=fff&size=200', 140.00, 14, true, '2024-01-01 00:00:00+00'),
(7, 'Amanda', 'Martinez', 'Oncology', 'Dedicated oncologist focused on cancer treatment and patient care.', 'amanda.martinez@hospital.com', '+1-555-0107', 'https://ui-avatars.com/api/?name=Amanda+Martinez&background=EC4899&color=fff&size=200', 170.00, 16, true, '2024-01-01 00:00:00+00'),
(8, 'Robert', 'Lee', 'Gastro', 'Expert gastroenterologist treating digestive system disorders.', 'robert.lee@hospital.com', '+1-555-0108', 'https://ui-avatars.com/api/?name=Robert+Lee&background=84CC16&color=fff&size=200', 145.00, 11, true, '2024-01-01 00:00:00+00'),
(9, 'Jennifer', 'Brown', 'Ophthalmology', 'Skilled ophthalmologist providing comprehensive eye care services.', 'jennifer.brown@hospital.com', '+1-555-0109', 'https://ui-avatars.com/api/?name=Jennifer+Brown&background=F97316&color=fff&size=200', 135.00, 9, true, '2024-01-01 00:00:00+00')
ON CONFLICT ("Id") DO NOTHING;

-- Add the migration record to prevent EF Core from trying to run migrations again
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" VARCHAR(150) NOT NULL PRIMARY KEY,
    "ProductVersion" VARCHAR(32) NOT NULL
);

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20260218090814_AddDoctorAppointments', '10.0.0')
ON CONFLICT DO NOTHING;
