namespace BlazorAuthApp.Models;

public class Appointment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string PatientEmail { get; set; } = string.Empty;
    public string PatientPhone { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public bool PatientArrived { get; set; }
    public DateTime? PatientArrivedAt { get; set; }
    public bool IsFinished { get; set; }
    public DateTime? FinishedAt { get; set; }
    public DateTime CreatedAt { get; set; }
    public int? CheckedInByAssistantId { get; set; }
    public string? CheckedInByAssistantName { get; set; }
    public string? CheckedInByAssistantBarcode { get; set; }
    public Doctor? Doctor { get; set; }
}

public class CreateAppointmentRequest
{
    public int DoctorId { get; set; }
    public DateTime AppointmentDate { get; set; }
    public string TimeSlot { get; set; } = string.Empty;
    public string PatientName { get; set; } = string.Empty;
    public string PatientEmail { get; set; } = string.Empty;
    public string PatientPhone { get; set; } = string.Empty;
    public string Symptoms { get; set; } = string.Empty;
}
