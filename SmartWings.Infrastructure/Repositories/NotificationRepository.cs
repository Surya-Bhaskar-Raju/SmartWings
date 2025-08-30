using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SmartWings.Domain;
using SmartWings.Infrastructure.DataContext;
using SmartWings.Infrastructure.Contracts;

namespace SmartWings.Infrastructure.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly FlightDbContext _context;

        public NotificationRepository(FlightDbContext context)
        {
            _context = context;
        }

        public async Task AddNotificationAsync(Notification notification)
        {
            await _context.Notifications.AddAsync(notification);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(Guid userId)
        {
            return await _context.Notifications
                .Where(n => n.UserId == userId)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }
    }
}