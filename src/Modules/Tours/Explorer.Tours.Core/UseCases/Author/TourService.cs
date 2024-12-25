using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Stakeholders.Core.Domain;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using Explorer.Tours.Core.Domain.ValueObjects;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Author
{
    public class TourService : CrudService<TourDTO, Tour>, ITourService
    {
        private readonly ICrudRepository<TourCheckpoint> _tourCheckpointRepository;
        private readonly ICrudRepository<Equipment> _equipmentRepository;
        private readonly ITourRepository tourRepository;
        private readonly IArticleService _articleService;
        private readonly IMapper _mapper;

        private readonly ICrudRepository<TourReview> _tourReviewRepository;

        public TourService(ICrudRepository<Tour> repository, IMapper mapper,
            ICrudRepository<TourCheckpoint> tourCheckpointRepository,
            ICrudRepository<Equipment> equipmentRepository, ITourRepository tourRepository,
            ICrudRepository<TourReview> tourReviewRepository, IArticleService articleService) : base(repository, mapper)
        {
            _mapper = mapper;
            _tourCheckpointRepository = tourCheckpointRepository;
            _equipmentRepository = equipmentRepository;
            this.tourRepository = tourRepository;
            _tourReviewRepository = tourReviewRepository;
            _articleService = articleService;
        }

        public Result AddEquipment(int tourId, EquipmentDto equipmentDto)
        {
            var equipment = _mapper.Map<Equipment>(equipmentDto);
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var result = tour.AddEquipment(equipment);
            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return result;
        }

        public Result RemoveEquipment(int tourId, EquipmentDto equipmentDto)
        {
            var equipment = _mapper.Map<Equipment>(equipmentDto);
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var result = tour.RemoveEquipment(equipment);
            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return result;
        }

        public Result AddCheckpoint(int tourId, TourCheckpointDto checkpointDto)
        {
            var checkpoint = _mapper.Map<TourCheckpoint>(checkpointDto);
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var result = tour.AddCheckpoint(checkpoint);
            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return result;
        }

        public Result RemoveCheckpoint(int tourId, TourCheckpointDto checkpointDto)
        {
            var checkpoint = _mapper.Map<TourCheckpoint>(checkpointDto);
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var result = tour.RemoveCheckpoint(checkpoint);
            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return result;
        }

        public Result<PagedResult<TourCheckpointDto>> GetPagedCheckpoint(int page, int pageSize)
        {
            var checkpoints = _tourCheckpointRepository.GetPaged(page, pageSize);
            if (checkpoints == null)
            {
                return Result.Fail("No checkpoints found.");
            }

            var checkpointDtos = checkpoints.Results.Select(cp => _mapper.Map<TourCheckpointDto>(cp)).ToList();
            var pagedResult = new PagedResult<TourCheckpointDto>(checkpointDtos, checkpoints.TotalCount);
            return Result.Ok(pagedResult);
        }
        public new Result<PagedResult<TourDTO>> GetPaged(int page, int pageSize)
        {
            var result = tourRepository.GetPaged(page, pageSize);

            return MapToDto(result);
        }
        public new Result<TourDTO> Update(TourDTO entity)
        {
            try
            {
                var result = tourRepository.Update(MapToDomain(entity));
                return MapToDto(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<List<long>> GetCheckpointIds(int tourId)
        {
            try
            {
                //var tourResult = tourRepository.Get(tourId);
                var tourResult = CrudRepository.Get(tourId, t => t.TourCheckpoints);
                List<long> result = new List<long>();
                foreach (var item in tourResult.TourCheckpoints)
                {
                    result.Add(item.Id);
                }
                return Result.Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<List<long>> GetEquipmentIds(int tourId)
        {
            try
            {
                var tourResult = CrudRepository.Get(tourId);
                List<long> result = new List<long>();
                foreach (var item in tourResult.Equipments)
                {
                    result.Add(item.Id);
                }
                return Result.Ok(result);
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
        }

        public Result<TourCheckpointDto> CreateCheckpoint(TourCheckpointDto checkpointDto)
        {
            var checkpoint = _mapper.Map<TourCheckpoint>(checkpointDto);
            var createdCheckpoint = _tourCheckpointRepository.Create(checkpoint);

            if (createdCheckpoint == null)
            {
                return Result.Fail("Failed to create checkpoint.");
            }

            return Result.Ok(_mapper.Map<TourCheckpointDto>(createdCheckpoint));
        }

        public Result<TourCheckpointDto> GetCheckpoint(int id)
        {
            var checkpoint = _tourCheckpointRepository.Get(id);
            if (checkpoint == null)
            {
                return Result.Fail("Checkpoint not found.");
            }

            return Result.Ok(_mapper.Map<TourCheckpointDto>(checkpoint));
        }

        public Result<TourCheckpointDto> UpdateCheckpoint(int id, TourCheckpointDto checkpointDto)
        {
            var checkpointObj = _tourCheckpointRepository.Get(id);
            _mapper.Map(checkpointDto, checkpointObj);
            var updatedCheckpoint = _tourCheckpointRepository.Update(checkpointObj);

            if (updatedCheckpoint == null)
            {
                return Result.Fail("Failed to update checkpoint.");
            }

            return Result.Ok(_mapper.Map<TourCheckpointDto>(updatedCheckpoint));
        }

        public Result DeleteCheckpoint(int id)
        {
            try
            {
                _tourCheckpointRepository.Delete(id);
                return Result.Ok();
            }
            catch (KeyNotFoundException)
            {
                return Result.Fail("Checkpoint not found.");
            }
        }

        public Result<PagedResult<EquipmentDto>> GetPagedEquipment(int page, int pageSize)
        {
            var equipments = _equipmentRepository.GetPaged(page, pageSize);
            if (equipments == null)
            {
                return Result.Fail("No equipment found.");
            }

            var equipmentDtos = equipments.Results.Select(e => _mapper.Map<EquipmentDto>(e)).ToList();
            var pagedResult = new PagedResult<EquipmentDto>(equipmentDtos, equipments.TotalCount);
            return Result.Ok(pagedResult);
        }

        public Result<EquipmentDto> CreateEquipment(EquipmentDto equipmentDto)
        {
            var equipment = _mapper.Map<Equipment>(equipmentDto);

            var createdEquipment = _equipmentRepository.Create(equipment);

            if (createdEquipment == null)
            {
                return Result.Fail("Failed to create equipment.");
            }

            return Result.Ok(_mapper.Map<EquipmentDto>(createdEquipment));
        }

        public Result<EquipmentDto> UpdateEquipment(int id, EquipmentDto equipmentDto)
        {
            var equipmentObj = _equipmentRepository.Get(id);
            _mapper.Map(equipmentDto, equipmentObj);
            var updatedEquipment = _equipmentRepository.Update(equipmentObj);

            if (updatedEquipment == null)
            {
                return Result.Fail("Failed to update equipment.");
            }

            return Result.Ok(_mapper.Map<EquipmentDto>(updatedEquipment));
        }

        public Result<EquipmentDto> GetEquipment(int id)
        {
            var equipment = _equipmentRepository.Get(id);
            if (equipment == null)
            {
                return Result.Fail("Equipment not found.");
            }

            return Result.Ok(_mapper.Map<EquipmentDto>(equipment));
        }

        public Result DeleteEquipment(int id)
        {
            try
            {
                _equipmentRepository.Delete(id);
                return Result.Ok();
            }
            catch (KeyNotFoundException)
            {
                return Result.Fail("Equipment not found.");
            }
        }


        public Result<PagedResult<TourReviewDto>> GetPagedReviews(int tourId, int page, int pageSize)
        {

            //var tour = CrudRepository.Get(tourId, t => t.TourReviews);
            Tour tour = tourRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");
            //var reviews = _tourReviewRepository.GetPaged(page, pageSize).Results.FindAll(r => r.Tour.Id == tourId);

            var reviewDtos = tour.TourReviews.Select(r => _mapper.Map<TourReviewDto>(r)).ToList();


            /*var tour = CrudRepository.Get(tourId, t => t.TourReviews);
            ;
            if (tour == null)
                return Result.Fail("Tour not found.");*/
            //var reviews = _tourReviewRepository.GetPaged(page, pageSize).Results.FindAll(r => r.Tour.Id == tourId);
            //tour.TourReviews.ForEach(r => _tourReviewRepository.Get(r.Id, tr => tr.Personn));
            //var reviewDtos = tour.TourReviews.Select(r => _mapper.Map<TourReviewDto>(r)).ToList();
            //reviewDtos.ForEach(r => r.Tour = _mapper.Map<TourDTO>(tour));

            var pagedResult = new PagedResult<TourReviewDto>(reviewDtos, tour.TourReviews.Count);

            return Result.Ok(pagedResult);
        }

        public Result<TourReviewDto> AddReview(int tourId, TourReviewDto reviewDto)
        {
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var review = _mapper.Map<TourReview>(reviewDto);
            var result = tour.AddTourReview(review);

            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return Result.Ok(_mapper.Map<TourReviewDto>(review));
        }

        public Result<TourReviewDto> UpdateReview(int reviewId, TourReviewDto reviewDto)
        {
            var tour = CrudRepository.Get(reviewId, t => t.TourReviews);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var review = tour.TourReviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null)
                return Result.Fail("Review not found.");

            _mapper.Map(reviewDto, review);
            CrudRepository.Update(tour);

            return Result.Ok(_mapper.Map<TourReviewDto>(review));
        }

        public Result DeleteReview(int reviewId)
        {
            var tour = CrudRepository.Get(reviewId, t => t.TourReviews);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var review = tour.TourReviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null)
                return Result.Fail("Review not found.");

            tour.TourReviews.Remove(review);
            CrudRepository.Update(tour);

            return Result.Ok();
        }

        public Result ArchiveTour(int tourId)
        {
            try
            {
                var tour = CrudRepository.Get(tourId);
                tour.SetArchived();
                CrudRepository.Update(tour);
                return Result.Ok();
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }


        public Result PublishTour(int tourId)
        {
            try
            {
                Tour tour = tourRepository.Get(tourId);
                var result = tour.setPublished();
                CrudRepository.Update(tour);


                var articleDto = new ArticleDTO
                {
                    TourId = tourId,
                    AuthorId = tour.AuthorId,
                    Title = $"Explore {tour.Name}",
                    Content = tour.Description,
                    TourDescription = tour.Description,
                    Weight = tour.Weight,
                    Tags = tour.Tags,
                    Price = tour.Price,
                    LengthInKm = tour.LengthInKm,
                    Checkpoints = tour.TourCheckpoints.Select(c => c.CheckpointName).ToList(), // Pretpostavljamo da je Name atribut
                    EquipmentList = tour.Equipments.Select(e => e.Name).ToList() // Pretpostavljamo da je Name atribut
                };


                var articleResult = _articleService.CreateArticle(tourId, tour.AuthorId, articleDto);
                if (articleResult.IsFailed)
                    return Result.Fail("Tour published, but failed to create article.");

                return result;
            }
            catch (KeyNotFoundException e)
            {
                return Result.Fail(FailureCode.NotFound).WithError(e.Message);
            }
            catch (ArgumentException e)
            {
                return Result.Fail(FailureCode.InvalidArgument).WithError(e.Message);
            }
        }

        public Result<TourCheckpointDto> AddNewCheckpoint(long tourId, TourCheckpointDto tourCheckpoint)
        {
            Tour tour = tourRepository.Get(tourId);
            TourCheckpoint checkpoint = new TourCheckpoint(tourCheckpoint.Latitude, tourCheckpoint.Longitude, tourCheckpoint.CheckpointName,
                tourCheckpoint.CheckpointDescription, tourCheckpoint.Image);
            tour.AddNewCheckpoint(checkpoint);
            CrudRepository.Update(tour);
            return tourCheckpoint;

        }

        public Result<TravelTimeDTO> AddNewTravelTime(long tourId, TravelTimeDTO tourTravelTime)
        {
            Tour tour = CrudRepository.Get(tourId);
            TravelTime travelTime = new TravelTime(tourTravelTime.Time, (TransportType)tourTravelTime.TransportType);
            tour.AddNewTravelTime(travelTime);
            CrudRepository.Update(tour);
            return tourTravelTime;
        }

        public Result<DailyAgendaDTO> AddNewDailyAgenda(long tourId, DailyAgendaDTO dailyAgenda)
        {
            Tour tour = tourRepository.Get(tourId);
            DailyAgenda newDailyAgenda = new DailyAgenda(dailyAgenda.Day, dailyAgenda.StartDestination,
                dailyAgenda.BetweenDestinations, dailyAgenda.EndDestination, dailyAgenda.Description);
            tour.AddNewDailyAgenda(newDailyAgenda);
            CrudRepository.Update(tour);
            return dailyAgenda;
        }

        //NOVO















        /*public Result<PagedResult<TourReviewDto>> GetPagedReviews(int tourId, int page, int pageSize)
        {
            var tour = CrudRepository.Get(tourId, t => t.TourReviews);
            if (tour == null)
                return Result.Fail("Tour not found.");
            //var reviews = _tourReviewRepository.GetPaged(page, pageSize).Results.FindAll(r => r.Tour.Id == tourId);
            var reviewDtos = tour.TourReviews.Select(r => _mapper.Map<TourReviewDto>(r)).ToList();
            var pagedResult = new PagedResult<TourReviewDto>(reviewDtos, tour.TourReviews.Count);

            return Result.Ok(pagedResult);
        }

        public Result<TourReviewDto> AddReview(int tourId, TourReviewDto reviewDto)
        {
            var tour = CrudRepository.Get(tourId);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var review = _mapper.Map<TourReview>(reviewDto);
            var result = tour.AddTourReview(review);

            if (result.IsSuccess)
                CrudRepository.Update(tour);

            return Result.Ok(_mapper.Map<TourReviewDto>(review));
        }

        public Result<TourReviewDto> UpdateReview(int reviewId, TourReviewDto reviewDto)
        {
            var tour = CrudRepository.Get(reviewId, t => t.TourReviews);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var review = tour.TourReviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null)
                return Result.Fail("Review not found.");

            _mapper.Map(reviewDto, review);
            CrudRepository.Update(tour);

            return Result.Ok(_mapper.Map<TourReviewDto>(review));
        }

        public Result DeleteReview(int reviewId)
        {
            var tour = CrudRepository.Get(reviewId, t => t.TourReviews);
            if (tour == null)
                return Result.Fail("Tour not found.");

            var review = tour.TourReviews.FirstOrDefault(r => r.Id == reviewId);
            if (review == null)
                return Result.Fail("Review not found.");

            tour.TourReviews.Remove(review);
            CrudRepository.Update(tour);

            return Result.Ok();
        }*/






        public Result<List<TourDTO>> GetAllTours()
        {
            try
            {
                var tours = tourRepository.GetPaged(1, int.MaxValue);
                var publishedTours = tours.Results.Where(t => t.Status == TourStatus.PUBLISHED).ToList();
                foreach (var tour in publishedTours)
                {
                    var checkpoint = _tourCheckpointRepository.Get(tour.Id);
                    tour.AddCheckpoint(checkpoint);

                }
                var tourDtos = _mapper.Map<List<TourDTO>>(publishedTours);
                return Result.Ok(tourDtos);
            }
            catch (Exception e)
            {
                return Result.Fail("Error retrieving all tours").WithError(e.Message);
            }
        }

        public Result<List<TourDTO>> GetToursByAuthorId(int authorId)
        {
            try
            {
                // Dobavljanje svih tura iz repozitorijuma
                var tours = tourRepository.GetPaged(1, int.MaxValue);

                // Filtriranje tura na osnovu AuthorId i statusa (PUBLISHED)
                var authorTours = tours.Results
                    .Where(t => t.AuthorId == authorId && t.Status == TourStatus.PUBLISHED)
                    .ToList();

                // Dodavanje kontrolnih tačaka svakoj turi
               // foreach (var tour in authorTours)
                //{
                  //  var checkpoint = _tourCheckpointRepository.Get(tour.Id);
                   // tour.AddCheckpoint(checkpoint);
                //}

                // Mapiranje rezultata u DTO
                var tourDtos = _mapper.Map<List<TourDTO>>(authorTours);

                return Result.Ok(tourDtos);
            }
            catch (Exception e)
            {
                // Vraćanje greške ako nešto pođe po zlu
                return Result.Fail($"Error retrieving tours for author with ID {authorId}").WithError(e.Message);
            }
        }

        public Result<List<TourDTO>> GetPurchasedTourForTourist(List<long> tourIds)
        {
            try
            {
                var results = tourRepository.GetAllByIds(tourIds);
                if (!results.Any())
                {
                    return Result.Fail("No tours purchased for tourist.");
                }

                var purchasedTours = _mapper.Map<List<TourDTO>>(results);
                return Result.Ok(purchasedTours);
            }
            catch(Exception e)
            {
                return Result.Fail($"An error occurred while collecting the tours: {e.Message}");
            }
        }


    }
}


