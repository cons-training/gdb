using GDB.App.Domain.Enums;
using GDB.App.Domain.Models;
using GDB.App.Infrastructure.Repositories.Contracts;
using GDB.App.Infrastructure.Repositories.Queries.cs;
using System.Data.SqlClient;

namespace GDB.App.Infrastructure.Repositories.Implementations
{
    internal class AccountRepositoryDB : IAccountRepository
    {
        public IAccount GetAccount(string accountNumber)
        {
            using (SqlConnection connection =
                   DataBaseConnectionManager.GetConnection())
            {
                connection.Open();

                using (SqlCommand command =
                       new SqlCommand(
                           AccountQueries.GetAccount,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@AccountNumber",
                        accountNumber);

                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return CreateAccount(reader);
                        }
                    }
                }
            }

            return null;
        }


        public void SaveAccount(IAccount account, string pin)
        {
            using (SqlConnection connection =
                   DataBaseConnectionManager.GetConnection())
            {
                connection.Open();

                SqlTransaction transaction =
                    connection.BeginTransaction();

                try
                {
                    long accountId;

                    using (SqlCommand command =
                           new SqlCommand(
                               AccountQueries.InsertAccount,
                               connection,
                               transaction))
                    {
                        command.Parameters.AddWithValue(
                            "@AccountNumber",
                            account.AccountNumber);

                        command.Parameters.AddWithValue(
                            "@Name",
                            account.Name);

                        command.Parameters.AddWithValue(
                            "@Age",
                            account.Age);

                        command.Parameters.AddWithValue(
                            "@AccountType",
                            GetAccountTypeCode(
                                account.AccountType));

                        command.Parameters.AddWithValue(
                            "@Balance",
                            account.Balance);

                        command.Parameters.AddWithValue(
                            "@AccountStatus",
                            GetAccountStatusCode(
                                account.Status));

                        command.Parameters.AddWithValue(
                            "@AccountPrivilege",
                            GetAccountPrivilegeCode(
                                account.Privilege));

                        command.Parameters.AddWithValue(
                            "@Pin",
                            pin);

                        accountId =
                            Convert.ToInt64(
                                command.ExecuteScalar());
                    }

                    SaveAccountType(
                        account,
                        accountId,
                        connection,
                        transaction);

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        private void SaveAccountType(
            IAccount account,
            long accountId,
            SqlConnection connection,
            SqlTransaction transaction)
        {
            if (account is SavingsAccount savings)
            {
                SaveSavingsAccount(
                    savings,
                    accountId,
                    connection,
                    transaction);
            }
            else if (account is CurrentAccount current)
            {
                SaveCurrentAccount(
                    current,
                    accountId,
                    connection,
                    transaction);
            }
            else if (account is FixedDepositAccount fixedDeposit)
            {
                SaveFixedDepositAccount(
                    fixedDeposit,
                    accountId,
                    connection,
                    transaction);
            }
            else if (account is SalaryAccount salary)
            {
                SaveSalaryAccount(
                    salary,
                    accountId,
                    connection,
                    transaction);
            }
        }


        private void SaveSavingsAccount(
            SavingsAccount savings,
            long accountId,
            SqlConnection connection,
            SqlTransaction transaction)
        {
            using (SqlCommand command =
                   new SqlCommand(
                       AccountQueries.InsertSavingsAccount,
                       connection,
                       transaction))
            {
                command.Parameters.AddWithValue(
                    "@AccountId",
                    accountId);

                command.Parameters.AddWithValue(
                    "@InterestRate",
                    savings.InterestRate / 100.0);

                command.Parameters.AddWithValue(
                    "@MinimumBalance",
                    savings.MinBalance);

                command.ExecuteNonQuery();
            }
        }


        private void SaveCurrentAccount(
            CurrentAccount current,
            long accountId,
            SqlConnection connection,
            SqlTransaction transaction)
        {
            using (SqlCommand command =
                   new SqlCommand(
                       AccountQueries.InsertCurrentAccount,
                       connection,
                       transaction))
            {
                command.Parameters.AddWithValue(
                    "@AccountId",
                    accountId);

                command.Parameters.AddWithValue(
                    "@OverdraftLimit",
                    current.OverdraftLimit);

                command.ExecuteNonQuery();
            }
        }


        private void SaveFixedDepositAccount(
            FixedDepositAccount fixedDeposit,
            long accountId,
            SqlConnection connection,
            SqlTransaction transaction)
        {
            using (SqlCommand command =
                   new SqlCommand(
                       AccountQueries.InsertFixedDepositAccount,
                       connection,
                       transaction))
            {
                command.Parameters.AddWithValue(
                    "@AccountId",
                    accountId);

                command.Parameters.AddWithValue(
                    "@InterestRate",
                    fixedDeposit.InterestRate / 100.0);

                command.Parameters.AddWithValue(
                    "@TenureMonths",
                    fixedDeposit.TenureMonths);

                command.Parameters.AddWithValue(
                    "@PrincipalAmount",
                    fixedDeposit.Balance);

                command.ExecuteNonQuery();
            }
        }


        private void SaveSalaryAccount(
            SalaryAccount salary,
            long accountId,
            SqlConnection connection,
            SqlTransaction transaction)
        {
            using (SqlCommand command =
                   new SqlCommand(
                       AccountQueries.InsertSalaryAccount,
                       connection,
                       transaction))
            {
                command.Parameters.AddWithValue(
                    "@AccountId",
                    accountId);

                command.Parameters.AddWithValue(
                    "@EmployerName",
                    salary.EmployerName);

                command.Parameters.AddWithValue(
                    "@InactiveMonths",
                    salary.InactiveMonths);

                command.Parameters.AddWithValue(
                    "@SalaryAmount",
                    salary.Balance);

                command.ExecuteNonQuery();
            }
        }


        public void UpdateBalance(
            string accountNumber,
            decimal balance)
        {
            using SqlConnection connection =
                DataBaseConnectionManager.GetConnection();

            connection.Open();

            using SqlCommand command =
                new SqlCommand(
                    AccountQueries.UpdateBalance,
                    connection);

            command.Parameters.AddWithValue(
                "@Balance",
                balance);

            command.Parameters.AddWithValue(
                "@AccountNumber",
                accountNumber);

            command.ExecuteNonQuery();
        }


        public void CloseAccount(
            string accountNumber)
        {
            using (SqlConnection connection =
                   DataBaseConnectionManager.GetConnection())
            {
                connection.Open();

                using (SqlCommand command =
                       new SqlCommand(
                           AccountQueries.CloseAccount,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@AccountNumber",
                        accountNumber);

                    int rowsAffected =
                        command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception(
                            "Account not found.");
                    }
                }
            }
        }


        public List<IAccount> GetAllAccounts()
        {
            List<IAccount> accounts =
                new List<IAccount>();

            using (SqlConnection connection =
                   DataBaseConnectionManager.GetConnection())
            {
                connection.Open();

                using (SqlCommand command =
                       new SqlCommand(
                           AccountQueries.GetAllAccounts,
                           connection))
                {
                    using (SqlDataReader reader =
                           command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            accounts.Add(
                                CreateAccount(reader));
                        }
                    }
                }
            }

            return accounts;
        }


        public void SaveAccounts(
            IAccount fromAccount,
            IAccount toAccount)
        {
            using (SqlConnection connection =
                   DataBaseConnectionManager.GetConnection())
            {
                connection.Open();

                SqlTransaction transaction =
                    connection.BeginTransaction();

                try
                {
                    using (SqlCommand command =
                           new SqlCommand(
                               AccountQueries.SaveAccounts,
                               connection,
                               transaction))
                    {
                        command.Parameters.AddWithValue(
                            "@Balance",
                            fromAccount.Balance);

                        command.Parameters.AddWithValue(
                            "@AccountNumber",
                            fromAccount.AccountNumber);

                        command.ExecuteNonQuery();

                        command.Parameters["@Balance"].Value =
                            toAccount.Balance;

                        command.Parameters["@AccountNumber"].Value =
                            toAccount.AccountNumber;

                        command.ExecuteNonQuery();
                    }

                    transaction.Commit();
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }


        private IAccount CreateAccount(
            SqlDataReader reader)
        {
            string accountNumber =
                reader["AccountNumber"].ToString();

            string name =
                reader["Name"].ToString();

            int age =
                Convert.ToInt32(reader["Age"]);

            decimal balance =
                Convert.ToDecimal(reader["Balance"]);

            AccountType accountType =
                ConvertAccountType(
                    reader["AccountType"].ToString());

            AccountStatus status =
                ConvertAccountStatus(
                    reader["AccountStatus"].ToString());

            AccountPrivilege privilege =
                ConvertAccountPrivilege(
                    reader["AccountPrivilege"].ToString());

            string pin =
                reader["Pin"].ToString();

            switch (accountType)
            {
                case AccountType.Savings:
                    return CreateSavingsAccount(
                        reader,
                        accountNumber,
                        name,
                        age,
                        balance,
                        accountType,
                        status,
                        pin,
                        privilege);

                case AccountType.Current:
                    return CreateCurrentAccount(
                        reader,
                        accountNumber,
                        name,
                        age,
                        balance,
                        accountType,
                        status,
                        pin,
                        privilege);

                case AccountType.FixedDeposit:
                    return CreateFixedDepositAccount(
                        reader,
                        accountNumber,
                        name,
                        age,
                        balance,
                        accountType,
                        status,
                        pin,
                        privilege);

                case AccountType.Salary:
                    return CreateSalaryAccount(
                        reader,
                        accountNumber,
                        name,
                        age,
                        balance,
                        accountType,
                        status,
                        pin,
                        privilege);

                default:
                    throw new Exception(
                        "Unknown account type.");
            }
        }


        private IAccount CreateSavingsAccount(
            SqlDataReader reader,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            AccountType accountType,
            AccountStatus status,
            string pin,
            AccountPrivilege privilege)
        {
            decimal minBalance =
                reader["SavingsMinimumBalance"] == DBNull.Value
                ? 1000.0m
                : Convert.ToDecimal(
                    reader["SavingsMinimumBalance"]);

            double interestRate =
                reader["SavingsInterestRate"] == DBNull.Value
                ? 4.0
                : Convert.ToDouble(
                    reader["SavingsInterestRate"]) * 100;

            return new SavingsAccount(
                accountNumber,
                name,
                age,
                balance,
                accountType,
                status,
                pin,
                privilege,
                minBalance,
                interestRate);
        }


        private IAccount CreateCurrentAccount(
            SqlDataReader reader,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            AccountType accountType,
            AccountStatus status,
            string pin,
            AccountPrivilege privilege)
        {
            decimal overdraftLimit =
                reader["OverdraftLimit"] == DBNull.Value
                ? 25000.0m
                : Convert.ToDecimal(
                    reader["OverdraftLimit"]);

            return new CurrentAccount(
                accountNumber,
                name,
                age,
                balance,
                accountType,
                status,
                pin,
                privilege,
                overdraftLimit);
        }


        private IAccount CreateFixedDepositAccount(
            SqlDataReader reader,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            AccountType accountType,
            AccountStatus status,
            string pin,
            AccountPrivilege privilege)
        {
            int tenureMonths =
                reader["TenureMonths"] == DBNull.Value
                ? 12
                : Convert.ToInt32(
                    reader["TenureMonths"]);

            double interestRate =
                reader["FixedDepositInterestRate"] == DBNull.Value
                ? 6.5
                : Convert.ToDouble(
                    reader["FixedDepositInterestRate"]) * 100;

            return new FixedDepositAccount(
                accountNumber,
                name,
                age,
                balance,
                accountType,
                status,
                pin,
                privilege,
                tenureMonths,
                interestRate);
        }


        private IAccount CreateSalaryAccount(
            SqlDataReader reader,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            AccountType accountType,
            AccountStatus status,
            string pin,
            AccountPrivilege privilege)
        {
            string employerName =
                reader["EmployerName"] == DBNull.Value
                ? "TechCorp"
                : reader["EmployerName"].ToString();

            return new SalaryAccount(
                accountNumber,
                name,
                age,
                balance,
                accountType,
                status,
                pin,
                privilege,
                employerName);
        }


        //============================================================
        //       5. CHANGE PIN
        //============================================================

        public void ChangePin(
            string accountNumber,
            string oldPin,
            string newPin)
        {
            using (SqlConnection connection =
                   DataBaseConnectionManager.GetConnection())
            {
                connection.Open();

                using (SqlCommand command =
                       new SqlCommand(
                           AccountQueries.ChangePin,
                           connection))
                {
                    command.Parameters.AddWithValue(
                        "@AccountNumber",
                        accountNumber);

                    command.Parameters.AddWithValue(
                        "@OldPin",
                        oldPin);

                    command.Parameters.AddWithValue(
                        "@NewPin",
                        newPin);

                    int rowsAffected =
                        command.ExecuteNonQuery();

                    if (rowsAffected == 0)
                    {
                        throw new Exception(
                            "Account not found or current PIN is incorrect.");
                    }
                }
            }
        }



        private AccountType ConvertAccountType(
            string value)
        {
            switch (value)
            {
                case "SAVINGS":
                    return AccountType.Savings;

                case "CURRENT":
                    return AccountType.Current;

                case "FIXED_DEPOSIT":
                    return AccountType.FixedDeposit;

                case "SALARY":
                    return AccountType.Salary;

                default:
                    throw new Exception(
                        "Invalid account type: " + value);
            }
        }


        private AccountStatus ConvertAccountStatus(
            string value)
        {
            return (AccountStatus)Enum.Parse(
                typeof(AccountStatus),
                value,
                true);
        }


        private AccountPrivilege ConvertAccountPrivilege(
            string value)
        {
            return (AccountPrivilege)Enum.Parse(
                typeof(AccountPrivilege),
                value,
                true);
        }


        private string GetAccountTypeCode(
            AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.Savings:
                    return "SAVINGS";

                case AccountType.Current:
                    return "CURRENT";

                case AccountType.FixedDeposit:
                    return "FIXED_DEPOSIT";

                case AccountType.Salary:
                    return "SALARY";

                default:
                    throw new Exception(
                        "Invalid account type.");
            }
        }


        private string GetAccountStatusCode(
            AccountStatus status)
        {
            return status.ToString().ToUpper();
        }


        private string GetAccountPrivilegeCode(
            AccountPrivilege privilege)
        {
            return privilege.ToString().ToUpper();
        }
    }
}