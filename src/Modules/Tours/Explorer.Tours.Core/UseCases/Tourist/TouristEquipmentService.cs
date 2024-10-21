using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Tourist;
using Explorer.Tours.Core.Domain;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Tourist
{
    internal class TouristEquipmentService : CrudService<TouristEquipmentDTO, TouristEquipment>, ITouristEquipmentService
    {
        public TouristEquipmentService(ICrudRepository<TouristEquipment> crudRepository, IMapper mapper) : base(crudRepository, mapper) { }

        public Result<TouristEquipmentDTO> FindByTouristAndEquipment(long touristId, long equipmentId)
        {
            int page = 1;
            int pageSize = 1000;
            int totalFetched = 0;
            Result<PagedResult<TouristEquipmentDTO>> pagedResult;

            try
            {
                do
                {
                    // Preuzmi paginirane rezultate za trenutnu stranicu
                    pagedResult = GetPaged(page, pageSize);

                    if (!pagedResult.IsSuccess)
                    {
                        return Result.Fail(FailureCode.NotFound).WithError("Failed to retrieve paged results.");
                    }

                    // Proveri da li u rezultatima postoji traženi TouristEquipmentDTO
                    var touristEquipment = pagedResult.Value.Results
                        .FirstOrDefault(te => te.TouristId == touristId && te.EquipmentId == equipmentId);

                    if (touristEquipment != null)
                    {
                        return Result.Ok(touristEquipment);
                    }

                    // Ažuriraj broj preuzetih zapisa i pređi na sledeću stranicu
                    totalFetched += pagedResult.Value.Results.Count;
                    page++;
                }
                // Nastavi paginaciju dokle god smo preuzeli manje zapisa nego što je ukupan broj
                while (totalFetched < pagedResult.Value.TotalCount);

                return Result.Fail(FailureCode.NotFound).WithError("TouristEquipment not found.");
            }
            catch (Exception e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }
    }
}