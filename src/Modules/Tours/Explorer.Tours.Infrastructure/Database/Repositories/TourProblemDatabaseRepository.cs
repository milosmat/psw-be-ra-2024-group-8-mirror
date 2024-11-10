using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.TourProblems;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories;

public class TourProblemDatabaseRepository : CrudDatabaseRepository<TourProblem, ToursContext>, ITourProblemRepository
{
    public TourProblemDatabaseRepository(ToursContext dbContext) : base(dbContext) {}

    /*public TourProblem Create(TourProblem tourProblem)
    {
        throw new NotImplementedException();
    }

    public void Delete(long id)
    {
        throw new NotImplementedException();
    }*/

    public TourProblem Get(long id)
    {
        return DbContext.TourProblems.Where(t => t.Id == id)
            .Include(t => t.ProblemComments!)
            .FirstOrDefault();
    }

    public List<TourProblem> GetAll()
    {
        return DbContext.TourProblems
            .Include(t => t.ProblemComments)
            .ToList();
    }

    public List<TourProblem> GetAllForAuthor(long authorId)
    {
        throw new NotImplementedException();
    }

    public List<TourProblem> GetAllForTourist(long touristId)
    {
        return GetProblemsWhere(t => t.UserId == touristId);
    }

    public TourProblem Update(TourProblem tourProblem)
    {
        DbContext.Entry(tourProblem).State = EntityState.Modified;
        DbContext.SaveChanges();
        return tourProblem;
    }

    private List<TourProblem> GetProblemsWhere(Expression<Func<TourProblem, bool>> expression)
    {
        return DbContext.TourProblems.Where(expression)
            .Include(t => t.ProblemComments)
            .ToList();
    }
}
