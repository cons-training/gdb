using GDB.App.Application.Services.Contracts;
using GDB.App.Application.Services.Implementations;

namespace GDB.App.Application.Services
{
    internal class TransactionServiceFactory
    {

        public static ITransactionService Create()
        {
            return new TransactionService();
        }
    }
}
