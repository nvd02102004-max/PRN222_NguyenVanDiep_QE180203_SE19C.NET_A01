using BusinessObjects;

namespace Services
{
    public interface ITagService
    {
        List<Tag> GetAllTags();
        Tag? GetTagById(int id);
        List<Tag> GetTagsByArticleId(string articleId);
        void CreateTag(Tag tag);
    }
}
