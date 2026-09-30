using BusinessObjects;

namespace Repositories
{
    public interface ISystemAccountRepository
    {
        List<SystemAccount> GetAll();
        SystemAccount? GetById(short id);
        SystemAccount? GetByEmail(string email);
        SystemAccount? CheckLogin(string email, string password);
        void Add(SystemAccount account);
        void Update(SystemAccount account);
        bool Delete(short id);
        List<SystemAccount> Search(string? keyword);
        short GetNextAccountId();
    }
}
