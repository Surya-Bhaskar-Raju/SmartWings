namespace SmartWings.Domain;

public class OtpRecord // Represents a one-time password record (For Password Reset)
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Otp { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsUsed { get; set; }
}