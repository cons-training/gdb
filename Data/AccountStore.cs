using gdb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gdb.Data
{
    internal class AccountStore
    {
        public static DataSet CreateDataSet()
        {
            var ds = new DataSet("GDBDataSet");

            // Create tables
            CreateAccountTable(ds);
            CreateSavingsTable(ds);
            CreateCurrentTable(ds);
            CreateFixedDepositTable(ds);
            CreateSalaryTable(ds);

            // Create relationships (foreign-key-like DataRelations)
            CreateRelations(ds);

            // Populate realistic sample data (Indian banking context, INR, example account numbers)
            PopulateSampleData(ds);

            return ds;
        }

        // ACCOUNT table (master)
        private static void CreateAccountTable(DataSet ds)
        {
            // Main ACCOUNT table
            DataTable accountTable = new DataTable("ACCOUNT");

            DataColumn accountId = accountTable.Columns.Add("AccountId", typeof(long));
            accountId.AutoIncrement = true;
            accountId.AutoIncrementSeed = 1;
            accountId.AutoIncrementStep = 1;
            accountId.AllowDBNull = false;
            accountTable.PrimaryKey = new DataColumn[] { accountId };

            accountTable.Columns.Add("AccountNumber", typeof(string));
            accountTable.Columns.Add("Name", typeof(string));
            accountTable.Columns.Add("Age", typeof(int));
            accountTable.Columns.Add("AccountType", typeof(string));
            accountTable.Columns.Add("Balance", typeof(decimal));
            accountTable.Columns.Add("Status", typeof(string));
            accountTable.Columns.Add("Privilege", typeof(string));
            accountTable.Columns.Add("PIN", typeof(string));

            // Add table to DataSet
            ds.Tables.Add(accountTable);
        }

        // SAVINGS_ACCOUNT (specialized)
        private static void CreateSavingsTable(DataSet ds)
        {
            var dt = new DataTable("SAVINGS_ACCOUNT");
            var dcAccountId = new DataColumn("AccountId", typeof(long)) { AllowDBNull = false };
            var dcInterestRate = new DataColumn("InterestRate", typeof(decimal)) { AllowDBNull = false, DefaultValue = 0.035m };
            var dcMinimumBalance = new DataColumn("MinimumBalance", typeof(decimal)) { AllowDBNull = false, DefaultValue = 1000.00m };
            var dcWithdrawalLimit = new DataColumn("WithdrawalLimit", typeof(int)) { AllowDBNull = false, DefaultValue = 6 };

            dt.Columns.Add(dcAccountId);
            dt.Columns.Add(dcInterestRate);
            dt.Columns.Add(dcMinimumBalance);
            dt.Columns.Add(dcWithdrawalLimit);

            dt.PrimaryKey = new[] { dcAccountId };

            ds.Tables.Add(dt);
        }

        // CURRENT_ACCOUNT (specialized)
        private static void CreateCurrentTable(DataSet ds)
        {
            var dt = new DataTable("CURRENT_ACCOUNT");
            var dcAccountId = new DataColumn("AccountId", typeof(long)) { AllowDBNull = false };
            var dcOverdraftLimit = new DataColumn("OverdraftLimit", typeof(decimal)) { AllowDBNull = false, DefaultValue = 0.00m };
            var dcInterestRate = new DataColumn("InterestRate", typeof(decimal)) { AllowDBNull = false, DefaultValue = 0.0m };
            var dcMinimumBalance = new DataColumn("MinimumBalance", typeof(decimal)) { AllowDBNull = false, DefaultValue = 0.00m };

            dt.Columns.Add(dcAccountId);
            dt.Columns.Add(dcOverdraftLimit);
            dt.Columns.Add(dcInterestRate);
            dt.Columns.Add(dcMinimumBalance);

            dt.PrimaryKey = new[] { dcAccountId };

            ds.Tables.Add(dt);
        }

        // FIXED_DEPOSIT_ACCOUNT (specialized)
        private static void CreateFixedDepositTable(DataSet ds)
        {
            var dt = new DataTable("FIXED_DEPOSIT_ACCOUNT");
            var dcAccountId = new DataColumn("AccountId", typeof(long)) { AllowDBNull = false };
            var dcPrincipalAmount = new DataColumn("PrincipalAmount", typeof(decimal)) { AllowDBNull = false, DefaultValue = 0.00m };
            var dcInterestRate = new DataColumn("InterestRate", typeof(decimal)) { AllowDBNull = false, DefaultValue = 0.06m };
            var dcStartDate = new DataColumn("StartDate", typeof(DateTime)) { AllowDBNull = false, DefaultValue = DateTime.UtcNow };
            var dcMaturityDate = new DataColumn("MaturityDate", typeof(DateTime)) { AllowDBNull = false };
            var dcMaturityAmount = new DataColumn("MaturityAmount", typeof(decimal)) { AllowDBNull = false, DefaultValue = 0.00m };
            var dcTenureMonths = new DataColumn("TenureMonths", typeof(int)) { AllowDBNull = false, DefaultValue = 12 };
            var dcAutoRenew = new DataColumn("AutoRenew", typeof(bool)) { AllowDBNull = false, DefaultValue = false };

            dt.Columns.Add(dcAccountId);
            dt.Columns.Add(dcPrincipalAmount);
            dt.Columns.Add(dcInterestRate);
            dt.Columns.Add(dcStartDate);
            dt.Columns.Add(dcMaturityDate);
            dt.Columns.Add(dcMaturityAmount);
            dt.Columns.Add(dcTenureMonths);
            dt.Columns.Add(dcAutoRenew);

            dt.PrimaryKey = new[] { dcAccountId };

            ds.Tables.Add(dt);
        }

        // SALARY_ACCOUNT (specialized)
        // Note: Employer table removed per request. EmployerId remains as an integer column.
        private static void CreateSalaryTable(DataSet ds)
        {
            var dt = new DataTable("SALARY_ACCOUNT");
            var dcAccountId = new DataColumn("AccountId", typeof(long)) { AllowDBNull = false };
            var dcEmployerId = new DataColumn("EmployerId", typeof(int)) { AllowDBNull = false };
            var dcEmployeeId = new DataColumn("EmployeeId", typeof(string)) { AllowDBNull = false, MaxLength = 64 };
            var dcSalaryCreditDay = new DataColumn("SalaryCreditDay", typeof(int)) { AllowDBNull = false, DefaultValue = 1 };
            var dcSalaryAmount = new DataColumn("SalaryAmount", typeof(decimal)) { AllowDBNull = false, DefaultValue = 0.00m };

            dt.Columns.Add(dcAccountId);
            dt.Columns.Add(dcEmployerId);
            dt.Columns.Add(dcEmployeeId);
            dt.Columns.Add(dcSalaryCreditDay);
            dt.Columns.Add(dcSalaryAmount);

            dt.PrimaryKey = new[] { dcAccountId };

            ds.Tables.Add(dt);
        }

        // Create DataRelations to mimic FKs between ACCOUNT and specialized tables only.
        private static void CreateRelations(DataSet ds)
        {
            AddRelation(ds, "AccountSavings", "ACCOUNT", "AccountId", "SAVINGS_ACCOUNT", "AccountId");
            AddRelation(ds, "AccountCurrent", "ACCOUNT", "AccountId", "CURRENT_ACCOUNT", "AccountId");
            AddRelation(ds, "AccountFixedDeposit", "ACCOUNT", "AccountId", "FIXED_DEPOSIT_ACCOUNT", "AccountId");
            AddRelation(ds, "AccountSalary", "ACCOUNT", "AccountId", "SALARY_ACCOUNT", "AccountId");
        }

        private static void AddRelation(DataSet ds, string relationName, string parentTable, string parentColumnName, string childTable, string childColumnName)
        {
            var parent = ds.Tables[parentTable].Columns[parentColumnName];
            var child = ds.Tables[childTable].Columns[childColumnName];

            var rel = new DataRelation(relationName, parent, child, createConstraints: true);
            ds.Relations.Add(rel);

            // Prevent child rows without parent rows by default (enforced via Constraint)
            rel.ChildKeyConstraint.DeleteRule = Rule.None;
            rel.ChildKeyConstraint.UpdateRule = Rule.None;
        }

        // Populate sample data. Transactions and Employer entries have been removed.
        private static void PopulateSampleData(DataSet ds)
        {
            var accounts = ds.Tables["ACCOUNT"];
            var savings = ds.Tables["SAVINGS_ACCOUNT"];
            var current = ds.Tables["CURRENT_ACCOUNT"];
            var fd = ds.Tables["FIXED_DEPOSIT_ACCOUNT"];
            var salary = ds.Tables["SALARY_ACCOUNT"];

            long[] accountIds = new long[12];

            // Helper to add an account
            void AddAccount(
                int index,
                string accountNumber,
                string name,
                int age,
                string accountType,
                decimal balance,
                string status,
                string privilege,
                string pin)
            {
                var row = accounts.NewRow();

                row["AccountNumber"] = accountNumber;
                row["Name"] = name;
                row["Age"] = age;
                row["AccountType"] = accountType;
                row["Balance"] = Math.Round(balance, 2);
                row["Status"] = status;
                row["Privilege"] = privilege;
                row["PIN"] = pin;

                accounts.Rows.Add(row);

                // Save the auto-generated AccountId
                accountIds[index] = (long)row["AccountId"];
            }

            // Add 12 accounts
            AddAccount(0, "1000001001", "Customer 1", 30, "SAVINGS",
                25000.75m, "ACTIVE", "PREMIUM", "1234");

            AddAccount(1, "1000001002", "Customer 2", 35, "SAVINGS",
                150000.00m, "ACTIVE", "GOLD", "1234");

            AddAccount(2, "1000001003", "Customer 3", 28, "SAVINGS",
                5000.00m, "ACTIVE", "SILVER", "1234");

            AddAccount(3, "1000001004", "Customer 4", 40, "SAVINGS",
                30000.00m, "ACTIVE", "SILVER", "1234");

            AddAccount(4, "1000001005", "Customer 5", 45, "CURRENT",
                125000.00m, "ACTIVE", "GOLD", "1234");

            AddAccount(5, "1000001006", "Customer 6", 32, "CURRENT",
                50000.00m, "ACTIVE", "SILVER", "1234");

            AddAccount(6, "1000001007", "Customer 7", 38, "CURRENT",
                75000.00m, "ACTIVE", "PREMIUM", "1234");

            AddAccount(7, "1000001008", "Customer 8", 50, "FIXED_DEPOSIT",
                100000.00m, "ACTIVE", "GOLD", "1234");

            AddAccount(8, "1000001009", "Customer 9", 42, "FIXED_DEPOSIT",
                200000.00m, "ACTIVE", "PREMIUM", "1234");

            AddAccount(9, "1000001010", "Customer 10", 29, "SALARY",
                35000.00m, "ACTIVE", "SILVER", "1234");

            AddAccount(10, "1000001011", "Customer 11", 33, "SALARY",
                48000.00m, "ACTIVE", "GOLD", "1234");

            AddAccount(11, "1000001012", "Customer 12", 27, "SAVINGS",
                12000.00m, "ACTIVE", "SILVER", "1234");


            // Add specialized rows using the generated AccountIds

            // Savings accounts: 0, 1, 2, 3, 11
            savings.Rows.Add(accountIds[0], 0.035m, 500.00m, 6);
            savings.Rows.Add(accountIds[1], 0.04m, 1000.00m, 6);
            savings.Rows.Add(accountIds[2], 0.03m, 200.00m, 4);
            savings.Rows.Add(accountIds[3], 0.0375m, 500.00m, 6);
            savings.Rows.Add(accountIds[11], 0.033m, 500.00m, 4);

            // Current accounts: 4, 5, 6
            current.Rows.Add(accountIds[4], 200000.00m, 0.00m, 0.00m);
            current.Rows.Add(accountIds[5], 50000.00m, 0.00m, 0.00m);
            current.Rows.Add(accountIds[6], 75000.00m, 0.00m, 0.00m);

            // Fixed deposit accounts: 7, 8
            fd.Rows.Add(
                accountIds[7], 100000.00m, 0.06m,
                new DateTime(2022, 2, 1),
                new DateTime(2023, 2, 1),
                106000.00m, 12, false);

            fd.Rows.Add(
                accountIds[8], 200000.00m, 0.065m,
                new DateTime(2021, 6, 10),
                new DateTime(2024, 6, 10),
                240000.00m, 36, true);

            // Salary accounts: 9, 10
            salary.Rows.Add(accountIds[9], 1, "EMP1009", 5, 50000.00m);
            salary.Rows.Add(accountIds[10], 2, "EMP1010", 1, 60000.00m);
        }
    }
}
