using Explorer.Tours.API.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Administration
{
    public interface ITourService
    {
        Task<TourDTO> CreateTourAsync(TourDTO tourDto);
        Task<TourDTO> GetTourByIdAsync(Guid id);
        Task<IEnumerable<TourDTO>> GetAllToursAsync();
        Task UpdateTourAsync(Guid id, TourDTO tourDto);
        Task DeleteTourAsync(Guid id);
    }
}
