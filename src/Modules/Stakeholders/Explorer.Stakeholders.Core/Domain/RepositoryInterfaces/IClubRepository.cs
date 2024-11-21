using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.Clubs;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface IClubRepository: ICrudRepository<Club>
    {
        PagedResult<Club> GetPaged(int page, int pageSize);
        Club Create(Club newClub);
        void Delete(long id);
        Club Update(Club updatedClub);
        Club Get(int id);
        Club GetClubWithMembershipRequests(long clubId);
    }
}
