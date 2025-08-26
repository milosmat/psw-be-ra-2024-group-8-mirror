using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Payments.API.Dtos;
using Explorer.Payments.API.Public.Tourist;
using Explorer.Payments.Core.Domain;
using Explorer.Payments.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;
using FluentResults;

namespace Explorer.Payments.Core.UseCases.Tourist
{
    public class TourPurchaseTokenService : CrudService<TourPurchaseTokenDTO, TourPurchaseToken>, ITourPurchaseTokenService
    {
        private readonly ITourPurchaseTokenRepository _tourPurchaseTokenRepository;
        private readonly IMapper _mapper;

        public TourPurchaseTokenService(
            ITourPurchaseTokenRepository tourPurchaseTokenRepository,
            IMapper mapper
        ) : base(tourPurchaseTokenRepository, mapper)
        {
            _tourPurchaseTokenRepository = tourPurchaseTokenRepository;
            _mapper = mapper;
        }

        
        public Result<List<long>> GetToursIdsFromToken(long touristId)
        {
            try
            {
                var tokens = _tourPurchaseTokenRepository.GetAllTokens()
               .Where(token => token.TouristId == touristId && token.Status == TourPurchaseToken.TokenStatus.Active)
               .ToList();

                if (!tokens.Any())
                {
                    return Result.Fail("No active tokens found for the given tourist.");
                }

                var tourIds = tokens.Select(token => token.TourId).ToList();
                return Result.Ok(tourIds);
            }
            catch(Exception ex)
            {
                throw new Exception("An error occured while collecting the tokens: ", ex);
            }

        }



    }
}
