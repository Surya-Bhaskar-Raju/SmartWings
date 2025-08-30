using SmartWings.Domain;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SmartWings.Infrastructure.Contracts
{
    public interface INotificationRepository
    {
        Task AddNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId);
    }
}