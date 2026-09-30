using BusinessObjects;

namespace Services
{
    public interface ISystemAccountService
    {
        List<SystemAccount> GetAllAccounts();
        SystemAccount? GetAccountById(short id);
        SystemAccount? GetAccountByEmail(string email);
        SystemAccount? Authenticate(string email, string password);
        void CreateAccount(SystemAccount account);
        void UpdateAccount(SystemAccount account);
        bool DeleteAccount(short id);
        List<SystemAccount> SearchAccounts(string? keyword);
        short GetNextAccountId();
    }
}
