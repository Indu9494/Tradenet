using Microsoft.EntityFrameworkCore;
using TradeNet11.Data;
using TradeNet11.Interfaces;
using TradeNet11.Models;

namespace TradeNet11.Repositories
{
    public class ComplianceNotificationRepository : IComplianceNotificationRepository
    {
        private readonly AppDbContext _context;

        public ComplianceNotificationRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<ComplianceNotification>> GetAllAsync()
        {
            return await _context.ComplianceNotifications
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<ComplianceNotification>> GetUnreadByOfficerAsync(int officerId)
        {
            return await _context.ComplianceNotifications
                .Where(n => n.AssignedOfficerId == officerId && !n.IsRead)
                .OrderByDescending(n => n.CreatedAt)
                .ToListAsync();
        }

        public async Task<ComplianceNotification?> GetByIdAsync(int id)
        {
            return await _context.ComplianceNotifications.FindAsync(id);
        }

        public async Task AddAsync(ComplianceNotification notification)
        {
            _context.ComplianceNotifications.Add(notification);
            await _context.SaveChangesAsync();
        }

        public async Task MarkAsReadAsync(int id)
        {
            var notification = await _context.ComplianceNotifications.FindAsync(id);
            if (notification != null)
            {
                notification.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}
