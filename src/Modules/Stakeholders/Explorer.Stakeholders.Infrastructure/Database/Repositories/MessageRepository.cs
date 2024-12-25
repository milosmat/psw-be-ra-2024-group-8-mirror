using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories
{
    public class MessageRepository : CrudDatabaseRepository<Message, StakeholdersContext>, IMessageRepository
    {

        public MessageRepository(StakeholdersContext dbContext) : base(dbContext) { }

        public Message Update(Message aggregateRoot)
        {
            DbContext.Entry(aggregateRoot).State = EntityState.Modified;
            DbContext.Messages.Update(aggregateRoot);
            DbContext.SaveChanges();
            return aggregateRoot;
        }


    }
}
