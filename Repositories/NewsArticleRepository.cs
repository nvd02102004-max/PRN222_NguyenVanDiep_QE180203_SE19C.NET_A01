using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class NewsArticleRepository : INewsArticleRepository
    {
        public List<NewsArticle> GetAll() => NewsArticleDAO.Instance.GetAll();

        public List<NewsArticle> GetActiveNews() => NewsArticleDAO.Instance.GetActiveNews();

        public NewsArticle? GetById(string id) => NewsArticleDAO.Instance.GetById(id);

        public List<NewsArticle> GetByAuthor(short authorId) => NewsArticleDAO.Instance.GetByAuthor(authorId);

        public void Add(NewsArticle article, List<int>? tagIds) => NewsArticleDAO.Instance.Add(article, tagIds);

        public void Update(NewsArticle article, List<int>? tagIds) => NewsArticleDAO.Instance.Update(article, tagIds);

        public bool Delete(string id) => NewsArticleDAO.Instance.Delete(id);

        public List<NewsArticle> Search(string? keyword, short? categoryId, bool? activeOnly) =>
            NewsArticleDAO.Instance.Search(keyword, categoryId, activeOnly);

        public List<NewsArticle> GetReport(DateTime? startDate, DateTime? endDate) =>
            NewsArticleDAO.Instance.GetReport(startDate, endDate);
    }
}
