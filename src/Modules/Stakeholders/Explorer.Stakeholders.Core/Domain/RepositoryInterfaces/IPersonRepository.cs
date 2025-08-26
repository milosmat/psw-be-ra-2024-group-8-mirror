using Explorer.BuildingBlocks.Core.UseCases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IPersonRepository: ICrudRepository<Person>
    {
        Person? GetByUserId(long  userId);
        List<Person> GetByUserIds(List<long> userIds);
    }
}
