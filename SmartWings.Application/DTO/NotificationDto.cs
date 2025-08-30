namespace SmartWings.Application.DTO
{
    public class NotificationCreateDto
    {
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
    }

    public class NotificationReadDto
    {
        public Guid NotificationId { get; set; }
        public Guid BookingId { get; set; }
        public Guid UserId { get; set; }
        public string Message { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsRead { get; set; }
    }
}
