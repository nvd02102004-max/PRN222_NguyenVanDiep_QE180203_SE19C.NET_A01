using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class CategoryDAO
    {
        private static CategoryDAO? _instance;
        private static readonly object _instanceLock = new object();

        private CategoryDAO() { }

        public static CategoryDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new CategoryDAO();
                    }
                    return _instance;
                }
            }
        }

        public List<Category> GetAll()
        {
            using var context = new FUNewsManagementContext();
            return context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.NewsArticles)
                .ToList();
        }

        public List<Category> GetActiveCategories()
        {
            using var context = new FUNewsManagementContext();
            return context.Categories
                .Where(c => c.IsActive == true)
                .Include(c => c.ParentCategory)
                .ToList();
        }

        public Category? GetById(short id)
        {
            using var context = new FUNewsManagementContext();
            return context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.NewsArticles)
                .FirstOrDefault(c => c.CategoryId == id);
        }

        public void Add(Category category)
        {
            using var context = new FUNewsManagementContext();
            context.Categories.Add(category);
            context.SaveChanges();
        }

        public void Update(Category category)
        {
            using var context = new FUNewsManagementContext();
            var existing = context.Categories.Find(category.CategoryId);
            if (existing != null)
            {
                existing.CategoryName = category.CategoryName;
                existing.CategoryDesciption = category.CategoryDesciption;
                existing.ParentCategoryId = category.ParentCategoryId;
                existing.IsActive = category.IsActive;
                context.SaveChanges();
            }
        }

        public bool HasNewsArticles(short id)
        {
            using var context = new FUNewsManagementContext();
            return context.NewsArticles.Any(n => n.CategoryId == id);
        }

        public bool Delete(short id)
        {
            using var context = new FUNewsManagementContext();
            // Business rule: Do not delete if category has news articles
            if (context.NewsArticles.Any(n => n.CategoryId == id))
            {
                throw new InvalidOperationException("Cannot delete category because it is already used in one or more news articles.");
            }

            var category = context.Categories.Find(id);
            if (category != null)
            {
                // If it is a parent category of other categories, set their parent to null
                var subCategories = context.Categories.Where(c => c.ParentCategoryId == id && c.CategoryId != id).ToList();
                foreach (var sub in subCategories)
                {
                    sub.ParentCategoryId = null;
                }

                context.Categories.Remove(category);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<Category> Search(string? keyword)
        {
            using var context = new FUNewsManagementContext();
            var query = context.Categories
                .Include(c => c.ParentCategory)
                .Include(c => c.NewsArticles)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(c => c.CategoryName.ToLower().Contains(keyword) ||
                                         c.CategoryDesciption.ToLower().Contains(keyword));
            }

            return query.ToList();
        }
    }
}
