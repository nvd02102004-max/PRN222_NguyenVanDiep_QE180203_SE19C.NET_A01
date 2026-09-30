using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class TagDAO
    {
        private static TagDAO? _instance;
        private static readonly object _instanceLock = new object();

        private TagDAO() { }

        public static TagDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new TagDAO();
                    }
                    return _instance;
                }
            }
        }

        public List<Tag> GetAll()
        {
            using var context = new FUNewsManagementContext();
            return context.Tags.ToList();
        }

        public Tag? GetById(int id)
        {
            using var context = new FUNewsManagementContext();
            return context.Tags.Find(id);
        }

        public List<Tag> GetTagsByArticleId(string articleId)
        {
            using var context = new FUNewsManagementContext();
            return context.NewsTags
                .Where(nt => nt.NewsArticleId == articleId)
                .Select(nt => nt.Tag)
                .ToList();
        }

        public void Add(Tag tag)
        {
            using var context = new FUNewsManagementContext();
            if (tag.TagId == 0)
            {
                tag.TagId = (context.Tags.Max(t => (int?)t.TagId) ?? 0) + 1;
            }
            context.Tags.Add(tag);
            context.SaveChanges();
        }
    }
}
