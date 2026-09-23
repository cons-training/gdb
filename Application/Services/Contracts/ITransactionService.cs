using GDB.App.Application.Dtos;

namespace GDB.App.Application.Services.Contracts
{
    public interface ITransactionService
    {
        List<ViewRecentTransactionsResponseDto> GetRecentTransactions(
           string accountNumber);
        DepositResponseDto Deposit(string accountNumber, decimal amount);

        WithdrawResponseDto Withdraw(string accountNumber, string pin, decimal amount);

        TranferFundsResponseDto TransferFunds(
            string fromAccountNumber,
            string toAccountNumber,
            string pin,
            decimal amount);
    }
}
