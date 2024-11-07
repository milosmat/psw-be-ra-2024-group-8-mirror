using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain.Entities;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class TourPurchaseTokenService : CrudService<TourPurchaseTokenDTO, TourPurchaseToken>, ITourPurchaseTokenService
    {
        public TourPurchaseTokenService(ICrudRepository<TourPurchaseToken> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
