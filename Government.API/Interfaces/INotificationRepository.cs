using Government.API.Models;

namespace Government.API.Interfaces
{
    public interface INotificationRepository
    {
        Task<IEnumerable<Notification>> GetAllNotificationsAsync();
        Task<Notification?> GetNotificationByIdAsync(int notificationId);
        Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId);
        Task<IEnumerable<Notification>> GetNotificationsByCategoryAsync(string category);
        Task<IEnumerable<Notification>> GetNotificationsByStatusAsync(string status);
        Task<IEnumerable<Notification>> GetUnreadNotificationsByUserIdAsync(int userId);
        Task<Notification> CreateNotificationAsync(Notification notification);
        Task<Notification> UpdateNotificationAsync(Notification notification);
        Task<bool> DeleteNotificationAsync(int notificationId);
    }
}

