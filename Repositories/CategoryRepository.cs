using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        public List<Category> GetAll() => CategoryDAO.Instance.GetAll();

        public List<Category> GetActiveCategories() => CategoryDAO.Instance.GetActiveCategories();

        public Category? GetById(short id) => CategoryDAO.Instance.GetById(id);

        public void Add(Category category) => CategoryDAO.Instance.Add(category);

        public void Update(Category category) => CategoryDAO.Instance.Update(category);

        public bool Delete(short id) => CategoryDAO.Instance.Delete(id);

        public bool HasNewsArticles(short id) => CategoryDAO.Instance.HasNewsArticles(id);

        public List<Category> Search(string? keyword) => CategoryDAO.Instance.Search(keyword);
    }
}
