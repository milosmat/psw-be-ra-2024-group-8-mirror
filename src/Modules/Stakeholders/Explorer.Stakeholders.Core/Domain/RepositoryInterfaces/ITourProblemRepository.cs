using Explorer.Stakeholders.Core.Domain.TourProblems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Core.Domain.RepositoryInterfaces
{
    public interface ITourProblemRepository
    {
        TourProblem Get(long id);
        TourProblem Create(TourProblem tourProblem);
        TourProblem Update(TourProblem tourProblem);
        List<TourProblem> GetAllForUser(long userId);
        List<TourProblem> GetAll();
    }
}
