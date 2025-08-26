using Explorer.BuildingBlocks.Core.Dtos;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Explorer.Tours.Core.UseCases.Author
{
    public class PaymentBundleService: IPaymentBundleService
    {
        private readonly IBundleService _bundleService;
        public PaymentBundleService(IBundleService bundleService)
        {
            _bundleService = bundleService;
        }
        public Result<BundleDTO> Get(int bundleId)
        {
            var result = _bundleService.Get(bundleId);

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }

            var mappedResult = MapToBuildingBlocksBundleDTO(result.Value);
            return Result.Ok(mappedResult);
        }

        public Result<List<BundleTourDTO>> GetAllTours(int bundleId)
        {
            var result = _bundleService.GetAllTours(bundleId);

            if (result.IsFailed)
            {
                return Result.Fail(result.Errors);
            }

            var mappedResults = result.Value.Select(MapToBuildingBlocksBundleTourDTO).ToList();
            return Result.Ok(mappedResults);
        }

        private BundleDTO MapToBuildingBlocksBundleDTO(Explorer.Tours.API.Dtos.BundleDTO source)
        {
            return new BundleDTO
            {
                Id = source.Id,
                Name = source.Name,
                CustomPrice = source.CustomPrice,
                TotalToursPriceCalculated = source.TotalToursPriceCalculated,
                PublishedDate = source.PublishedDate,
                ArchivedDate = source.ArchivedDate,
                AuthorId = source.AuthorId,
                Status = source.Status,
                Tours = source.Tours.Select(MapToBuildingBlocksBundleTourDTO).ToList()
            };
        }

        private BundleTourDTO MapToBuildingBlocksBundleTourDTO(Explorer.Tours.API.Dtos.BundleTourDTO source)
        {
            return new BundleTourDTO
            {
                Id = source.Id,
                TourId = source.TourId,
                Name = source.Name,
                Price = source.Price,
                BundleId = source.BundleId
            };
        }
    }
}
