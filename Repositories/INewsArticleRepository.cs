using BusinessObjects;

namespace Repositories
{
    public interface INewsArticleRepository
    {
        List<NewsArticle> GetAll();
        List<NewsArticle> GetActiveNews();
        NewsArticle? GetById(string id);
        List<NewsArticle> GetByAuthor(short authorId);
        void Add(NewsArticle article, List<int>? tagIds);
        void Update(NewsArticle article, List<int>? tagIds);
        bool Delete(string id);
        List<NewsArticle> Search(string? keyword, short? categoryId, bool? activeOnly);
        List<NewsArticle> GetReport(DateTime? startDate, DateTime? endDate);
    }
}
