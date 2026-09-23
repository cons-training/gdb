using GDB.App.Application.Dtos;

namespace GDB.App.Infrastructure.Repositories.Contracts
{
    public interface ITransactionRepository
    {
        List<ViewRecentTransactionsResponseDto> GetRecentTransactions(
        string accountNumber);
    }
}
