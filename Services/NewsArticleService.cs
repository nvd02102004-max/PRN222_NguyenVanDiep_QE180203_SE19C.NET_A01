using BusinessObjects;
using Repositories;

namespace Services
{
    public class NewsArticleService : INewsArticleService
    {
        private readonly INewsArticleRepository _newsRepo;

        public NewsArticleService(INewsArticleRepository newsRepo)
        {
            _newsRepo = newsRepo;
        }

        public NewsArticleService()
        {
            _newsRepo = new NewsArticleRepository();
        }

        public List<NewsArticle> GetAllNews() => _newsRepo.GetAll();

        public List<NewsArticle> GetActiveNews() => _newsRepo.GetActiveNews();

        public NewsArticle? GetNewsById(string id) => _newsRepo.GetById(id);

        public List<NewsArticle> GetNewsByAuthor(short authorId) => _newsRepo.GetByAuthor(authorId);

        public void CreateNews(NewsArticle article, List<int>? tagIds) => _newsRepo.Add(article, tagIds);

        public void UpdateNews(NewsArticle article, List<int>? tagIds) => _newsRepo.Update(article, tagIds);

        public bool DeleteNews(string id) => _newsRepo.Delete(id);

        public List<NewsArticle> SearchNews(string? keyword, short? categoryId, bool? activeOnly) =>
            _newsRepo.Search(keyword, categoryId, activeOnly);

        public List<NewsArticle> GetReport(DateTime? startDate, DateTime? endDate) =>
            _newsRepo.GetReport(startDate, endDate);
    }
}
