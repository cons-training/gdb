using gdb.Data;
using gdb.Domain;
using gdb.Domain.Enums;
using gdb.Domain.Exceptions;
using gdb.Domain.Models;
using gdb.Infrastructure.Repositories.Contracts;
using gdb.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gdb.Infrastructure.Repositories.Implementations
{
    internal class AccountRepositoryInMemory : IAccountRepository
    {

        private readonly DataSet _dataSet;

        public AccountRepositoryInMemory()
        {
            _dataSet = AccountStore.CreateDataSet();
        }

        public IAccount GetLastTransaction(string accountNumber)
        {
            return GetAccount(accountNumber);
        }

        public List<IAccount> GetAllAccounts()
        {
            List<IAccount> accounts = new List<IAccount>();

            // 1. Get the ACCOUNT table
            DataTable accountTable = _dataSet.Tables["ACCOUNT"]!;

            // 2. Iterate through each row in the table
            foreach (DataRow row in accountTable.Rows)
            {
                // 3. Read values from the row
                string number = row["AccountNumber"].ToString()!;
                string name = row["Name"].ToString()!;
                int age = Convert.ToInt32(row["Age"]);
                decimal balance = Convert.ToDecimal(row["Balance"]);

                AccountType accountType = Enum.Parse<AccountType>(row["AccountType"].ToString()!);

                AccountStatus status =
                   Enum.Parse<AccountStatus>(row["Status"].ToString()!);

                AccountPrivilege privilege =
                   Enum.Parse<AccountPrivilege>(row["Privilege"].ToString()!);

                string pin = row["PIN"].ToString()!;

                // 4. Create the correct account object using AccountFactory
                Account account = AccountFactory.CreateAccount(
                   accountType,
                   number,
                   name,
                   age,
                   balance,
                   status,
                   privilege,
                   pin);

                // 5. Add it to the list
                accounts.Add(account);
            }

            return accounts;
        }

        public IAccount GetAccount(string accountNumber)
        {
            // 1. Get the ACCOUNT table
            DataTable accountTable = _dataSet.Tables["ACCOUNT"]!;

            // 2. Find the account row by AccountNumber
            DataRow[] rows = accountTable.Select(
               $"AccountNumber = '{accountNumber}'");

            // Account not found
            if (rows.Length == 0)
            {
               throw new AccountException("Account not found");
            }

            DataRow row = rows[0];
            if(row == null)
            {
                throw new AccountException("Account not found");
            }

            // 3. Read values from the row
            string number = row["AccountNumber"].ToString()!;
            string name = row["Name"].ToString()!;
            int age = Convert.ToInt32(row["Age"]);
            decimal balance = Convert.ToDecimal(row["Balance"]);

            AccountType accountType = Enum.Parse<AccountType>(row["AccountType"].ToString()!);

            AccountStatus status =
               Enum.Parse<AccountStatus>(row["Status"].ToString()!);

            AccountPrivilege privilege =
               Enum.Parse<AccountPrivilege>(row["Privilege"].ToString()!);

            string pin = row["PIN"].ToString()!;

            // 4. Create the correct account object using AccountFactory
            Account account = AccountFactory.CreateAccount(
               accountType,
               number,
               name,
               age,
               balance,
               status,
               privilege,
               pin);

            // 5. Return it as IAccount
            return account;

        }
    }
}
