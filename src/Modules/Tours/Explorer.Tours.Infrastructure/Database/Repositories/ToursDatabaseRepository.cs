using AutoMapper;
using Explorer.BuildingBlocks.Core.Domain;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.BuildingBlocks.Infrastructure.Database;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.ValueObjects;
using FluentResults;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Infrastructure.Database.Repositories
{
    public class ToursDatabaseRepository : CrudDatabaseRepository<Tour, ToursContext>, ITourRepository
    {
        public ToursDatabaseRepository(ToursContext dbContext) : base(dbContext)
        {
        }

        public new Tour? Get(long id)
        {
            return DbContext.Tours.Where(t => t.Id == id)
                .Include(t => t.TourCheckpoints).Include(t => t.Equipments).Include(t => t.TourReviews).ThenInclude(t => t.Personn).FirstOrDefault();
        }
        public new PagedResult<Tour> GetPaged(int page, int pageSize)
        {
            var task = DbContext.Tours.Include(t => t.TourCheckpoints!).Include(t => t.Equipments).GetPagedById(page, pageSize);
            task.Wait();
            return task.Result;
        }
        public new Tour Update(Tour aggregateRoot)
        {
           if(aggregateRoot.Id <= 0)
            {
                throw new KeyNotFoundException("The provided Tour ID is invalid.");
            }
            else
            {
                DbContext.Entry(aggregateRoot).State = EntityState.Modified;
                DbContext.SaveChanges();
                return aggregateRoot;
            }
                
            
        }

        
    }
}
