using BusinessObjects;
using Repositories;

namespace Services
{
    public class TagService : ITagService
    {
        private readonly ITagRepository _tagRepo;

        public TagService(ITagRepository tagRepo)
        {
            _tagRepo = tagRepo;
        }

        public TagService()
        {
            _tagRepo = new TagRepository();
        }

        public List<Tag> GetAllTags() => _tagRepo.GetAll();

        public Tag? GetTagById(int id) => _tagRepo.GetById(id);

        public List<Tag> GetTagsByArticleId(string articleId) => _tagRepo.GetTagsByArticleId(articleId);

        public void CreateTag(Tag tag) => _tagRepo.Add(tag);
    }
}
