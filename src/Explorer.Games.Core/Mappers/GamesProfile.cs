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
                .ForMember(dest => dest.LastCheckedDate, opt => opt.MapFrom(src => src.LastCheckedDate)) // Mapping LastCheckedDate
                .ReverseMap()
                .ForMember(dest => dest.Scores, opt => opt.MapFrom(src => src.Scores))
                .ForMember(dest => dest.LastCheckedDate, opt => opt.MapFrom(src => src.LastCheckedDate)); // Mapping LastCheckedDate for reverse


            // Mapiranje između GameScore i GameScoreDTO
            CreateMap<GameScore, GameScoreDTO>()
                            .ForMember(dest => dest.AchievedAt, opt => opt.MapFrom(src => src.AchievedAt)) // Explicit mapping
                            .ReverseMap()
                            .ForMember(dest => dest.AchievedAt, opt => opt.MapFrom(src => src.AchievedAt)); // Explicit mapping for reverse

        }
    }
}
