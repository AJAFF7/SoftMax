namespace BlazorAuthApp.Models
{
    public class Assistant
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ETagBarcode { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsActive { get; set; }
    }

    public class AssistantRegisterRequest
    {
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class AssistantLoginRequest
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }

    public class AssistantLoginInfo
    {
        public Assistant Assistant { get; set; } = new();
        public DateTime LoginTime { get; set; }
        public DateTime LastActivity { get; set; }
        public string SessionDuration => GetSessionDuration();

        private string GetSessionDuration()
        {
            var duration = DateTime.Now - LoginTime;
            if (duration.TotalHours >= 1)
                return $"{(int)duration.TotalHours}h {duration.Minutes}m";
            else if (duration.TotalMinutes >= 1)
                return $"{(int)duration.TotalMinutes}m";
            else
                return $"{(int)duration.TotalSeconds}s";
        }
    }
}
