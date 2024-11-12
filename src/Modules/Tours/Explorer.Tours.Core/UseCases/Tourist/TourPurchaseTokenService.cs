using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    public class TourPurchaseTokenService : CrudService<TourPurchaseTokenDTO, TourPurchaseToken>, ITourPurchaseTokenService
    {
        private readonly ITourPurchaseTokenRepository _tourPurchaseTokenRepository;
        private readonly ICrudRepository<Tour> _tourRepository;
        private readonly IMapper _mapper;

        public TourPurchaseTokenService(
            ITourPurchaseTokenRepository tourPurchaseTokenRepository,
            ICrudRepository<Tour> tourRepository,
            IMapper mapper
        ) : base(tourPurchaseTokenRepository, mapper)
        {
            _tourPurchaseTokenRepository = tourPurchaseTokenRepository;
            _tourRepository = tourRepository;
            _mapper = mapper;
        }

        // Metoda za vraćanje svih kupljenih tura za određenog korisnika
        public Result<List<TourDTO>> GetPurchasedToursByTouristId(long touristId)
        {
            // Pronađi sve tokene za određenog turistu
            var tokens = _tourPurchaseTokenRepository.GetAllTokens()
                .Where(t => t.TouristId == touristId && t.Status == TourPurchaseToken.TokenStatus.Active)
                .ToList();

            // Proveri da li je pronađen barem jedan token
            if (tokens == null || tokens.Count == 0)
                return Result.Fail("Nema pronađenih kupljenih tura za ovog korisnika.");

            // Dohvati ture koristeći TourId iz tokena
            var tours = tokens
                .Select(token => _tourRepository.Get(token.TourId))
                .Where(tour => tour != null)
                .ToList();

            // Mapiraj ture u DTO format
            var tourDtos = _mapper.Map<List<TourDTO>>(tours);

            return Result.Ok(tourDtos);
        }


    }
}
