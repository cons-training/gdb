using GDB.App.Domain.Models;

namespace GDB.App.Infrastructure.Repositories.Contracts
{
    public interface IAccountRepository
    {
        IAccount GetAccount(string accountNumber);
        void CloseAccount(string accountNumber);
        void SaveAccount(IAccount account, string pin);
        List<IAccount> GetAllAccounts();
        void SaveAccounts(IAccount fromAccount, IAccount toAccount);
        void ChangePin(string accountNumber, string oldPin, string newPin);
        void UpdateBalance(string accountNumber, decimal balance);

    }
}
