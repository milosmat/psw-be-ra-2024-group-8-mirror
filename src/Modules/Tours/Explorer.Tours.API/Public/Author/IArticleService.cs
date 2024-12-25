using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Explorer.Tours.API.Dtos;
using FluentResults;

namespace Explorer.Tours.API.Public.Author
{
    public interface IArticleService
    {
        Result<ArticleDTO> Create(ArticleDTO articleDto);
        Result<ArticleDTO> CreateArticle(long tourId, long authorId, ArticleDTO articleDto);
        Result<ArticleDTO> GetArticleByTourId(long tourId);
        Result<List<ArticleDTO>> GetArticlesByAuthorId(long authorId);
        Result UpdateArticle(long articleId, ArticleDTO articleDto);
        Result<List<ArticleDTO>> GetAllArticles();
        Result<List<ArticleDTO>> GetPublishedArticles();
        Result<List<ArticleDTO>> GetPublishedArticlesByAuthorId(long authorId);
        Result<List<ArticleDTO>> GetUnpublishedArticlesByAuthorId(long authorId);
        Result PublishArticle(long articleId);



    }
}
