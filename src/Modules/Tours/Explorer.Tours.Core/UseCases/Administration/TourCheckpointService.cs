using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Administration;
using Explorer.Tours.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Administration;

public class TourCheckpointService : CrudService<TourCheckpointDto, TourCheckpoint>, ITourCheckpointService
{
    public TourCheckpointService(ICrudRepository<TourCheckpoint> repository, IMapper mapper) : base(repository, mapper) { }
}

