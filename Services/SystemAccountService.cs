using BusinessObjects;
using Repositories;

namespace Services
{
    public class SystemAccountService : ISystemAccountService
    {
        private readonly ISystemAccountRepository _accountRepo;

        public SystemAccountService(ISystemAccountRepository accountRepo)
        {
            _accountRepo = accountRepo;
        }

        public SystemAccountService()
        {
            _accountRepo = new SystemAccountRepository();
        }

        public List<SystemAccount> GetAllAccounts() => _accountRepo.GetAll();

        public SystemAccount? GetAccountById(short id) => _accountRepo.GetById(id);

        public SystemAccount? GetAccountByEmail(string email) => _accountRepo.GetByEmail(email);

        public SystemAccount? Authenticate(string email, string password) => _accountRepo.CheckLogin(email, password);

        public void CreateAccount(SystemAccount account) => _accountRepo.Add(account);

        public void UpdateAccount(SystemAccount account) => _accountRepo.Update(account);

        public bool DeleteAccount(short id) => _accountRepo.Delete(id);

        public List<SystemAccount> SearchAccounts(string? keyword) => _accountRepo.Search(keyword);

        public short GetNextAccountId() => _accountRepo.GetNextAccountId();
    }
}
