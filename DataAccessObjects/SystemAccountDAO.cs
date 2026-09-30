using BusinessObjects;
using Microsoft.EntityFrameworkCore;

namespace DataAccessObjects
{
    public class SystemAccountDAO
    {
        private static SystemAccountDAO? _instance;
        private static readonly object _instanceLock = new object();

        private SystemAccountDAO() { }

        public static SystemAccountDAO Instance
        {
            get
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new SystemAccountDAO();
                    }
                    return _instance;
                }
            }
        }

        public List<SystemAccount> GetAll()
        {
            using var context = new FUNewsManagementContext();
            return context.SystemAccounts
                .Include(a => a.NewsArticles)
                .OrderBy(a => a.AccountId)
                .ToList();
        }

        public SystemAccount? GetById(short id)
        {
            using var context = new FUNewsManagementContext();
            return context.SystemAccounts
                .Include(a => a.NewsArticles)
                .FirstOrDefault(a => a.AccountId == id);
        }

        public SystemAccount? GetByEmail(string email)
        {
            using var context = new FUNewsManagementContext();
            return context.SystemAccounts
                .FirstOrDefault(a => a.AccountEmail != null && a.AccountEmail.ToLower() == email.Trim().ToLower());
        }

        public SystemAccount? CheckLogin(string email, string password)
        {
            using var context = new FUNewsManagementContext();
            return context.SystemAccounts
                .FirstOrDefault(a => a.AccountEmail != null &&
                                     a.AccountEmail.ToLower() == email.Trim().ToLower() &&
                                     a.AccountPassword == password);
        }

        public short GetNextAccountId()
        {
            using var context = new FUNewsManagementContext();
            short maxId = context.SystemAccounts.Max(a => (short?)a.AccountId) ?? 0;
            return (short)(maxId + 1);
        }

        public void Add(SystemAccount account)
        {
            using var context = new FUNewsManagementContext();
            if (account.AccountId == 0)
            {
                account.AccountId = GetNextAccountId();
            }
            context.SystemAccounts.Add(account);
            context.SaveChanges();
        }

        public void Update(SystemAccount account)
        {
            using var context = new FUNewsManagementContext();
            var existing = context.SystemAccounts.Find(account.AccountId);
            if (existing != null)
            {
                existing.AccountName = account.AccountName;
                existing.AccountEmail = account.AccountEmail;
                if (!string.IsNullOrEmpty(account.AccountPassword))
                {
                    existing.AccountPassword = account.AccountPassword;
                }
                if (account.AccountRole.HasValue)
                {
                    existing.AccountRole = account.AccountRole;
                }
                context.SaveChanges();
            }
        }

        public bool Delete(short id)
        {
            using var context = new FUNewsManagementContext();
            var account = context.SystemAccounts.Find(id);
            if (account != null)
            {
                // Cascade delete or check news articles
                var newsArticles = context.NewsArticles.Where(n => n.CreatedById == id).ToList();
                foreach (var news in newsArticles)
                {
                    news.CreatedById = null;
                }
                context.SystemAccounts.Remove(account);
                context.SaveChanges();
                return true;
            }
            return false;
        }

        public List<SystemAccount> Search(string? keyword)
        {
            using var context = new FUNewsManagementContext();
            var query = context.SystemAccounts.AsQueryable();

            if (!string.IsNullOrWhiteSpace(keyword))
            {
                keyword = keyword.Trim().ToLower();
                query = query.Where(a => (a.AccountName != null && a.AccountName.ToLower().Contains(keyword)) ||
                                         (a.AccountEmail != null && a.AccountEmail.ToLower().Contains(keyword)));
            }

            return query.OrderBy(a => a.AccountId).ToList();
        }
    }
}
