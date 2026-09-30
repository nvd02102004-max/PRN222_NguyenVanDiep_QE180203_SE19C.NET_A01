using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class NewsArticleDAO
    {
        private static NewsArticleDAO? _instance;
        private static readonly object _instanceLock = new object();

        private NewsArticleDAO() { }

        public static NewsArticleDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new NewsArticleDAO();
                    }
                    return _instance;
                }
            }
        }

        public List<NewsArticle> GetAll()
        {
            using var context = new FUNewsManagementContext();
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }

        public List<NewsArticle> GetActiveNews()
        {
            using var context = new FUNewsManagementContext();
            return context.NewsArticles
                .Where(n => n.NewsStatus == true)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }

        public NewsArticle? GetById(string id)
        {
            using var context = new FUNewsManagementContext();
            return context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .FirstOrDefault(n => n.NewsArticleId == id);
        }

        public List<NewsArticle> GetByAuthor(short authorId)
        {
            using var context = new FUNewsManagementContext();
            return context.NewsArticles
                .Where(n => n.CreatedById == authorId)
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .OrderByDescending(n => n.CreatedDate)
                .ToList();
        }

        public void Add(NewsArticle article, List<int>? tagIds)
        {
            using var context = new FUNewsManagementContext();
            if (string.IsNullOrWhiteSpace(article.NewsArticleId))
            {
                article.NewsArticleId = Guid.NewGuid().ToString("N").Substring(0, 10);
            }

            article.CreatedDate ??= DateTime.Now;

            context.NewsArticles.Add(article);
            context.SaveChanges();

            if (tagIds != null && tagIds.Count > 0)
            {
                foreach (var tagId in tagIds.Distinct())
                {
                    context.NewsTags.Add(new NewsTag
                    {
                        NewsArticleId = article.NewsArticleId,
                        TagId = tagId
                    });
                }
                context.SaveChanges();
            }
        }

        public void Update(NewsArticle article, List<int>? tagIds)
        {
            using var context = new FUNewsManagementContext();
            var existing = context.NewsArticles
                .Include(n => n.NewsTags)
                .FirstOrDefault(n => n.NewsArticleId == article.NewsArticleId);

            if (existing != null)
            {
                existing.NewsTitle = article.NewsTitle;
                existing.Headline = article.Headline;
                existing.NewsContent = article.NewsContent;
                existing.NewsSource = article.NewsSource;
                existing.CategoryId = article.CategoryId;
                existing.NewsStatus = article.NewsStatus;
                existing.UpdatedById = article.UpdatedById;
                existing.ModifiedDate = DateTime.Now;

                // Sync tags
                context.NewsTags.RemoveRange(existing.NewsTags);
                if (tagIds != null && tagIds.Count > 0)
                {
                    foreach (var tagId in tagIds.Distinct())
                    {
                        context.NewsTags.Add(new NewsTag
                        {
                            NewsArticleId = existing.NewsArticleId,
                            TagId = tagId
                        });
                    }
                }

                context.SaveChanges();
            }
        }

        public bool Delete(string id)
        {
            using var context = new FUNewsManagementContext();
            var article = context.NewsArticles
                .Include(n => n.NewsTags)
                .FirstOrDefault(n => n.NewsArticleId == id);

            if (article != null)
            {
                context.NewsTags.RemoveRange(article.NewsTags);
                context.NewsArticles.Remove(article);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<NewsArticle> Search(string? keyword, short? categoryId, bool? activeOnly)
        {
            using var context = new FUNewsManagementContext();
            var query = context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .AsQueryable();

            if (activeOnly.HasValue && activeOnly.Value)
            {
                query = query.Where(n => n.NewsStatus == true);
            }

            if (categoryId.HasValue && categoryId.Value > 0)
            {
                query = query.Where(n => n.CategoryId == categoryId.Value);
            }

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(n => (n.NewsTitle != null && n.NewsTitle.ToLower().Contains(keyword)) ||
                                         (n.Headline != null && n.Headline.ToLower().Contains(keyword)) ||
                                         (n.NewsContent != null && n.NewsContent.ToLower().Contains(keyword)));
            }

            return query.OrderByDescending(n => n.CreatedDate).ToList();
        }

        public List<NewsArticle> GetReport(DateTime? startDate, DateTime? endDate)
        {
            using var context = new FUNewsManagementContext();
            var query = context.NewsArticles
                .Include(n => n.Category)
                .Include(n => n.CreatedBy)
                .Include(n => n.NewsTags)
                    .ThenInclude(nt => nt.Tag)
                .AsQueryable();

            if (startDate.HasValue)
            {
                var start = startDate.Value.Date;
                query = query.Where(n => n.CreatedDate >= start);
            }

            if (endDate.HasValue)
            {
                var end = endDate.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(n => n.CreatedDate <= end);
            }

            // Descending order as required
            return query.OrderByDescending(n => n.CreatedDate).ToList();
        }
    }
}
