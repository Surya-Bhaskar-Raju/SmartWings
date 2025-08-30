using SmartWings.Domain;

namespace SmartWings.Infrastructure.Contracts
{
    public interface INotificationRepository
    {
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification?> GetNotificationByIdAsync(Guid id);
        Task<bool> DeleteNotificationAsync(Guid id);
    }
}
