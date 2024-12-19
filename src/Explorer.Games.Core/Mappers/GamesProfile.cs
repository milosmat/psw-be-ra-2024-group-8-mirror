using AutoMapper;
using Explorer.Games.Core.Domain;
using Explorer.Games.API.Dtos;

namespace Explorer.Games.Core.Mappers
{
    public class GamesProfile : Profile
    {
        public GamesProfile()
        {
            // Mapiranje između Game i GameDTO
            CreateMap<Game, GameDTO>()
                .ForMember(dest => dest.Scores, opt => opt.MapFrom(src => src.Scores))
                .ReverseMap();

            // Mapiranje između GameScore i GameScoreDTO
            CreateMap<GameScore, GameScoreDTO>().ReverseMap();
        }
    }
}
