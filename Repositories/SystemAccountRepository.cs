using BusinessObjects;
using DataAccessObjects;

namespace Repositories
{
    public class SystemAccountRepository : ISystemAccountRepository
    {
        public List<SystemAccount> GetAll() => SystemAccountDAO.Instance.GetAll();

        public SystemAccount? GetById(short id) => SystemAccountDAO.Instance.GetById(id);

        public SystemAccount? GetByEmail(string email) => SystemAccountDAO.Instance.GetByEmail(email);

        public SystemAccount? CheckLogin(string email, string password) => SystemAccountDAO.Instance.CheckLogin(email, password);

        public void Add(SystemAccount account) => SystemAccountDAO.Instance.Add(account);

        public void Update(SystemAccount account) => SystemAccountDAO.Instance.Update(account);

        public bool Delete(short id) => SystemAccountDAO.Instance.Delete(id);

        public List<SystemAccount> Search(string? keyword) => SystemAccountDAO.Instance.Search(keyword);

        public short GetNextAccountId() => SystemAccountDAO.Instance.GetNextAccountId();
    }
}
