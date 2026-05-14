using Goverment.Data;
using Goverment.Interfaces;
using Goverment.Models;
using Microsoft.EntityFrameworkCore;

namespace Goverment.Repositories
{
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _context;

        public NotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetAllNotificationsAsync()
        {
            return await _context.Notifications
                .Include(n => n.User)
                .ToListAsync();
        }

        public async Task<Notification?> GetNotificationByIdAsync(int notificationId)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .FirstOrDefaultAsync(n => n.NotificationID == notificationId);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Where(n => n.UserID == userId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByCategoryAsync(string category)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Where(n => n.Category == category)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByStatusAsync(string status)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Where(n => n.Status == status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Include(n => n.User)
                .Where(n => n.UserID == userId && n.Status == "Unread")
                .ToListAsync();
        }

        public async Task<Notification> CreateNotificationAsync(Notification notification)
        {
            _context.Notifications.Add(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<Notification> UpdateNotificationAsync(Notification notification)
        {
            _context.Notifications.Update(notification);
            await _context.SaveChangesAsync();
            return notification;
        }

        public async Task<bool> DeleteNotificationAsync(int notificationId)
        {
            var notification = await _context.Notifications.FindAsync(notificationId);
            if (notification == null) return false;
            _context.Notifications.Remove(notification);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
