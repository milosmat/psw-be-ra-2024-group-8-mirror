using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Explorer.BuildingBlocks.Core.UseCases;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class NotificationRepository : CrudDatabaseRepository<Notification, StakeholdersContext>, INotificationRepository
    {
        private readonly StakeholdersContext _context;

        public NotificationRepository(StakeholdersContext context)
            : base(context) // Pozivanje osnovne klase sa StakeholdersContext
        {
            _context = context;
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByUserIdAsync(int userId)
        {
            return await _context.Notifications
                .Where(n => n.FollowerId == userId)
                .Include(n => n.Message) // Include Message, ako želimo kompletne podatke
                .ToListAsync();
        }

    }
}
