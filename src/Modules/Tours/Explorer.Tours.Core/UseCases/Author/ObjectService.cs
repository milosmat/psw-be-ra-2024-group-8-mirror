using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Object = Explorer.Tours.Core.Domain.Object;

namespace Explorer.Tours.Core.UseCases.Author
{
    public class ObjectService : CrudService<ObjectDTO, Object>, IObjectService
    {
        public ObjectService(ICrudRepository<Object> repository, IMapper mapper) : base(repository, mapper)
        {
        }
    }
}
