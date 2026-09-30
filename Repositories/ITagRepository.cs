using BusinessObjects;

namespace Repositories
{
    public interface ITagRepository
    {
        List<Tag> GetAll();
        Tag? GetById(int id);
        List<Tag> GetTagsByArticleId(string articleId);
        void Add(Tag tag);
    }
}
