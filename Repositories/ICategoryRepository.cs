using BusinessObjects;

namespace Repositories
{
    public interface ICategoryRepository
    {
        List<Category> GetAll();
        List<Category> GetActiveCategories();
        Category? GetById(short id);
        void Add(Category category);
        void Update(Category category);
        bool Delete(short id);
        bool HasNewsArticles(short id);
        List<Category> Search(string? keyword);
    }
}
