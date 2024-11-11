using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class TourProblemDatabaseRepository : CrudDatabaseRepository<TourProblem, StakeholdersContext>, ITourProblemRepository
{
    public TourProblemDatabaseRepository(StakeholdersContext dbContext) : base(dbContext) { }

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

    /*public List<TourProblem> GetAll()
    {
        return DbContext.TourProblems
            .Include(t => t.ProblemComments)
            .ToList();
    }*/

    public List<TourProblem> GetAllForAuthor(long authorId)
    {
        throw new NotImplementedException();
    }

    public List<TourProblem> GetAllForUser(User user)
    {
        if(user.Role == UserRole.Administrator)
        {
            return DbContext.TourProblems
            .Include(t => t.ProblemComments)
            .ToList();
        }
        if (user.Role == UserRole.Author)
        {
            return GetProblemsWhere(t => t.Author == user);
        }
        else
        {
            return GetProblemsWhere(t => t.Tourist == user);
        }
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

