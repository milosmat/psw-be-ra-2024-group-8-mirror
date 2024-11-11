using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;

public interface ITourProblemRepository : ICrudRepository<TourProblem>
{
    TourProblem Get(long id);
    //void Delete(long id);
    //TourProblem Create(TourProblem tourProblem);
    TourProblem Update(TourProblem tourProblem);
    List<TourProblem> GetAllForUser(User user);
    //List<TourProblem> GetAll();
    //List<TourProblem> GetAllForTourist(long touristId);
    //List<TourProblem> GetAllForAuthor(long authorId);
    //public List<TourProblem> GetTourProblemWhere(Expression<Func<TourProblem, bool>> expression);
}
