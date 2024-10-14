using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.Core.Domain;

namespace Explorer.Tours.Core.UseCases.Administration
{
    public class TourService : CrudService<TourDTO, Tour>
    {
        public TourService(ICrudRepository<Tour> repository, IMapper mapper) : base(repository, mapper) { }

    }
}
