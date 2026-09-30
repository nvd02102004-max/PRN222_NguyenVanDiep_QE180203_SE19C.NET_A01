using BusinessObjects;

namespace Services
{
    public interface INewsArticleService
    {
        List<NewsArticle> GetAllNews();
        List<NewsArticle> GetActiveNews();
        NewsArticle? GetNewsById(string id);
        List<NewsArticle> GetNewsByAuthor(short authorId);
        void CreateNews(NewsArticle article, List<int>? tagIds);
        void UpdateNews(NewsArticle article, List<int>? tagIds);
        bool DeleteNews(string id);
        List<NewsArticle> SearchNews(string? keyword, short? categoryId, bool? activeOnly);
        List<NewsArticle> GetReport(DateTime? startDate, DateTime? endDate);
    }
}
