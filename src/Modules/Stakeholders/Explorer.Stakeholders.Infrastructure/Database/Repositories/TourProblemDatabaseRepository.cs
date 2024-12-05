using Explorer.Stakeholders.Core.Domain.RepositoryInterfaces;
using Explorer.Stakeholders.Core.Domain.TourProblems;
using Explorer.Stakeholders.Core.Domain;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Stakeholders.Infrastructure.Database.Repositories;

public class TourProblemDatabaseRepository : ITourProblemRepository
{
    private readonly IUserRepository _userRepository;
    private readonly StakeholdersContext _context;

    public TourProblemDatabaseRepository(StakeholdersContext dbContext, IUserRepository userRepository)
    {
        _context = dbContext;
        _userRepository = userRepository;
    }

    public TourProblem Create(TourProblem tourProblem)
    {
        _context.TourProblems.Add(tourProblem);
        _context.SaveChanges();
        return tourProblem;
    }

    /*public void Delete(long id)
    {
        throw new NotImplementedException();
    }*/

    public TourProblem Get(long id)
    {
        return _context.TourProblems.Where(t => t.Id == id)
            .Include(t => t.ProblemComments!)
            .FirstOrDefault();
    }

    /*public List<TourProblem> GetAll()
    {
        return DbContext.TourProblems
            .Include(t => t.ProblemComments)
            .ToList();
    }*/

    /*public List<TourProblem> GetAllForAuthor(long authorId)
    {
        throw new NotImplementedException();
    }*/

    public List<TourProblem> GetAllForUser(long userId)
    {
        User user = _userRepository.GetUser(userId);
        if (user != null)
        {
            if (user.Role == UserRole.Administrator)
            {
                return _context.TourProblems
                .Include(t => t.ProblemComments)
                .ToList();
            }
            if (user.Role == UserRole.Author)
            {
                return GetProblemsWhere(t => t.AuthorId == user.Id);
            }
            if (user.Role == UserRole.Tourist)
            {
                return GetProblemsWhere(t => t.TouristId == user.Id);
            }
            else
            {
                var newList = new List<TourProblem>();
                return newList;
            }
        }
        var secList = new List<TourProblem>();
        return secList;
    }

    public TourProblem Update(TourProblem tourProblem)
    {
        _context.Entry(tourProblem).State = EntityState.Modified;
        _context.SaveChanges();
        return tourProblem;
    }

    private List<TourProblem> GetProblemsWhere(Expression<Func<TourProblem, bool>> expression)
    {
        return _context.TourProblems
            .Where(expression)
            .Include(t => t.ProblemComments!) // Ensure non-null collection
            .ToList();
    }
}

