using GDB.App.Domain.Enums;

namespace GDB.App.Application.Dtos
{
    public class TranferFundsResponseDto
    {
        public string FromAccountNumber { get; set; }

        public string ToAccountNumber { get; set; }

        public decimal Amount { get; set; }

        public decimal FromAccountBalance { get; set; }

        public decimal ToAccountBalance { get; set; }

        public TransactionStatus TransactionStat { get; set; }

    }
}
