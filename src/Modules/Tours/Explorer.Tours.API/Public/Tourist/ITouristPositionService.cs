using Explorer.Tours.API.Dtos;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.API.Public.Tourist
{
    public interface ITouristPositionService
    {
        Result<TouristPositionDto> GetPosition(int id);
        Result<TouristPositionDto> SetPosition(int id, double latitude, double longitude);
    }
}
