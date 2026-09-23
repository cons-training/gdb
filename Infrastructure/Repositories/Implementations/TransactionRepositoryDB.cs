using GDB.App.Application.Dtos;
using GDB.App.Domain.Enums;
using GDB.App.Infrastructure.Repositories.Contracts;
using System.Data.SqlClient;

namespace GDB.App.Infrastructure.Repositories.Implementations
{
    public class TransactionRepositoryDB : ITransactionRepository
    {
        public List<ViewRecentTransactionsResponseDto> GetRecentTransactions(
            string accountNumber)
        {
            List<ViewRecentTransactionsResponseDto> transactions =
                new List<ViewRecentTransactionsResponseDto>();

            using (SqlConnection connection =
                   DataBaseConnectionManager.GetConnection())
            {
                connection.Open();

                string query = @"
                    SELECT TOP 10

                        t.TransactionId,

                        fromAccount.AccountNumber AS FromAccountNumber,

                        toAccount.AccountNumber AS ToAccountNumber,

                        t.Amount,

                        tt.Code AS TransactionType,

                        ts.Code AS TransactionStatus,

                        t.Timestamp,

                        t.BalanceAfterFrom,

                        t.BalanceAfterTo

                    FROM Transactions t

                    LEFT JOIN Accounts fromAccount
                        ON t.FromAccountId = fromAccount.AccountId

                    LEFT JOIN Accounts toAccount
                        ON t.ToAccountId = toAccount.AccountId

                    INNER JOIN TransactionTypes tt
                        ON t.TransactionTypeId = tt.TransactionTypeId

                    INNER JOIN TransactionStatuses ts
                        ON t.TransactionStatusId = ts.TransactionStatusId

                    WHERE
                        t.FromAccountId =
                        (
                            SELECT AccountId
                            FROM Accounts
                            WHERE AccountNumber = @AccountNumber
                        )

                        OR

                        t.ToAccountId =
                        (
                            SELECT AccountId
                            FROM Accounts
                            WHERE AccountNumber = @AccountNumber
                        )

                    ORDER BY t.Timestamp DESC";

                using (SqlCommand command =
                       new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue(
                        "@AccountNumber",
                        accountNumber);

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ViewRecentTransactionsResponseDto transaction =
                                new ViewRecentTransactionsResponseDto();

                            transaction.TransactionId =
                                Convert.ToInt32(
                                    reader["TransactionId"]);

                            transaction.FromAccountNumber =
                                reader["FromAccountNumber"] == DBNull.Value
                                    ? null
                                    : reader["FromAccountNumber"].ToString();

                            transaction.ToAccountNumber =
                                reader["ToAccountNumber"] == DBNull.Value
                                    ? null
                                    : reader["ToAccountNumber"].ToString();

                            transaction.Amount =
                                Convert.ToDecimal(
                                    reader["Amount"]);

                            transaction.TransactionType =
                                (TransactionType)Enum.Parse(
                                    typeof(TransactionType),
                                    reader["TransactionType"].ToString(),
                                    true);

                            transaction.TransactionStatus =
                                (TransactionStatus)Enum.Parse(
                                    typeof(TransactionStatus),
                                    reader["TransactionStatus"].ToString(),
                                    true);

                            transaction.Timestamp =
                                Convert.ToDateTime(
                                    reader["Timestamp"]);

                            transaction.BalanceAfterFrom =
                                reader["BalanceAfterFrom"] == DBNull.Value
                                    ? null
                                    : Convert.ToDecimal(
                                        reader["BalanceAfterFrom"]);

                            transaction.BalanceAfterTo =
                                reader["BalanceAfterTo"] == DBNull.Value
                                    ? null
                                    : Convert.ToDecimal(
                                        reader["BalanceAfterTo"]);

                            transactions.Add(transaction);
                        }
                    }
                }
            }

            return transactions;
        }
    }
}