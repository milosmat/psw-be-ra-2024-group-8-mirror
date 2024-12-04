using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using FluentResults;
using Microsoft.AspNetCore.Mvc;

namespace Explorer.API.Controllers.Tourist
{
    public class VisitedCheckpointController : ControllerBase
    {
        private readonly IVisitedCheckpointService _service;

        public VisitedCheckpointController(IVisitedCheckpointService service)
        {
            _service = service;
        }

        [HttpPost]
        public Result<VisitedCheckpointDTO> Create(VisitedCheckpointDTO newEntity)
        {
            var defaultResult = new VisitedCheckpointDTO
            {
                CheckpointId = 0,
                VisitTime = DateTime.MinValue,
                Secret = "Nema podataka"
            };

            return Result.Ok(defaultResult);
        }

        public object GetAll(int v1, int v2)
        {
            throw new NotImplementedException();
        }
    }
}
