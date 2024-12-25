using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Explorer.BuildingBlocks.Core.UseCases;
using Explorer.Tours.API.Dtos;
using Explorer.Tours.API.Public.Author;
using Explorer.Tours.Core.Domain;
using Explorer.Tours.Core.Domain.RepositoryInterfaces;
using FluentResults;

namespace Explorer.Tours.Core.UseCases.Author
{
    public class ArticleService : CrudService<ArticleDTO, Bundle>, IArticleService
    {
        private readonly ICrudRepository<BundleTour> _bundleTourRepository;
        private readonly IArticleRepository _articleRepository;
        private readonly IMapper _mapper;

        public ArticleService(ICrudRepository<Bundle> repository, IMapper mapper,
            ICrudRepository<BundleTour> bundleTourRepository,
            IArticleRepository articleRepository) : base(repository, mapper)
        {
            _mapper = mapper;
            _articleRepository = articleRepository;
        }

        public Result<ArticleDTO> CreateArticle(long tourId, long authorId, ArticleDTO articleDto)
        {
            try
            {
                // Kreiramo entitet Article koristeći podatke iz ArticleDTO
                var article = new Article(
                    tourId: tourId,
                    authorId: authorId,
                    title: articleDto.Title,
                    content: articleDto.Content,
                    tourDescription: articleDto.TourDescription,
                    weight: articleDto.Weight,
                    tags: articleDto.Tags,
                    price: articleDto.Price,
                    lengthInKm: articleDto.LengthInKm,
                    checkpoints: articleDto.Checkpoints,
                    equipmentList: articleDto.EquipmentList,
                    isPublished: articleDto.IsPublished
                );

                // Čuvamo članak u bazi putem repozitorijuma
                var createdArticle = _articleRepository.Create(article);

                // Ako članak nije uspešno sačuvan, vraćamo grešku
                if (createdArticle == null)
                    return Result.Fail("Failed to create article.");

                // Mapiramo kreirani entitet Article u DTO i vraćamo ga kao uspešan rezultat
                var articleDtoResult = _mapper.Map<ArticleDTO>(createdArticle);
                return Result.Ok(articleDtoResult);
            }
            catch (Exception ex)
            {
                // U slučaju bilo koje greške tokom procesa, vraćamo grešku
                return Result.Fail($"An error occurred while creating the article: {ex.Message}");
            }
        }


        public Result<ArticleDTO> GetArticleByTourId(long tourId)
        {
            var article = _articleRepository.GetAll().FirstOrDefault(a => a.TourId == tourId);
            if (article == null)
                return Result.Fail("Article not found.");

            return Result.Ok(_mapper.Map<ArticleDTO>(article));
        }

        public Result<List<ArticleDTO>> GetArticlesByAuthorId(long authorId)
        {
            var articles = _articleRepository.GetAll().Where(a => a.AuthorId == authorId).ToList();
            if (!articles.Any())
                return Result.Fail("No articles found for this author.");

            return Result.Ok(_mapper.Map<List<ArticleDTO>>(articles));
        }

        public Result UpdateArticle(long articleId, ArticleDTO articleDto)
        {
            var article = _articleRepository.Get(articleId);
            if (article == null)
                return Result.Fail("Article not found.");

            article.Update(articleDto.Title, articleDto.Content);
            _articleRepository.Update(article);
            return Result.Ok();
        }

        public Result<List<ArticleDTO>> GetAllArticles()
        {
            try
            {
                // Dobijamo sve članke iz repozitorijuma
                var articles = _articleRepository.GetAll().ToList();

                // Ako nema članaka, vraćamo grešku
                if (!articles.Any())
                    return Result.Fail("No articles found.");

                // Mapiramo listu članaka u listu DTO objekata
                var articlesDto = _mapper.Map<List<ArticleDTO>>(articles);

                // Vraćamo uspešan rezultat sa mapiranim DTO-ima
                return Result.Ok(articlesDto);
            }
            catch (Exception ex)
            {
                // U slučaju greške, vraćamo grešku sa porukom
                return Result.Fail($"An error occurred while fetching all articles: {ex.Message}");
            }
        }

        public Result<List<ArticleDTO>> GetPublishedArticles()
        {
            // Dobavljamo sve članke i filtriramo samo objavljene
            var articles = _articleRepository.GetAll().Where(a => a.IsPublished).ToList();

            // Ako nema objavljenih članaka, vraćamo grešku
            if (!articles.Any())
                return Result.Fail("No published articles found.");

            // Mapiramo članke u DTO i vraćamo ih
            return Result.Ok(_mapper.Map<List<ArticleDTO>>(articles));
        }

        public Result<List<ArticleDTO>> GetUnpublishedArticlesByAuthorId(long authorId)
        {
            var articles = _articleRepository.GetAll()
                .Where(a => a.AuthorId == authorId && !a.IsPublished)
                .ToList();

            if (!articles.Any())
                return Result.Fail("No unpublished articles found for this author.");

            return Result.Ok(_mapper.Map<List<ArticleDTO>>(articles));
        }

        public Result<List<ArticleDTO>> GetPublishedArticlesByAuthorId(long authorId)
        {
            var articles = _articleRepository.GetAll()
                .Where(a => a.AuthorId == authorId && a.IsPublished)
                .ToList();

            if (!articles.Any())
                return Result.Fail("No unpublished articles found for this author.");

            return Result.Ok(_mapper.Map<List<ArticleDTO>>(articles));
        }

        public Result PublishArticle(long articleId)
        {
            var article = _articleRepository.Get(articleId);

            if (article == null)
            {
                return Result.Fail("Article not found.");
            }

            // Postavljamo članak kao objavljen
            article.Publish();

            // Ažuriramo članak u bazi
            _articleRepository.Update(article);

            return Result.Ok();
        }

    }
}
