using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class TagRepository : ITagRepository
    {
        public List<Tag> GetAll() => TagDAO.Instance.GetAll();

        public Tag? GetById(int id) => TagDAO.Instance.GetById(id);

        public List<Tag> GetTagsByArticleId(string articleId) => TagDAO.Instance.GetTagsByArticleId(articleId);

        public void Add(Tag tag) => TagDAO.Instance.Add(tag);
    }
}
