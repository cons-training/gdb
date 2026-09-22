using gdb.Domain.Enums;
using gdb.Domain.Models;
using gdb.Infrastructure.Repositories.Implementations;

namespace gdb.Domain
{
    public static class AccountFactory
    {
        public static Account CreateAccount(
            AccountType accountType,
            string accountNumber,
            string name,
            int age,
            decimal balance,
            AccountStatus status,
            AccountPrivilege privilege,
            string pin)
        {
            switch (accountType)
            {
                case AccountType.SAVINGS:
                    return new SavingsAccount(
                        accountNumber,
                        name,
                        age,
                        balance,
                        status,
                        privilege,
                        pin);

                case AccountType.CURRENT:
                    return new CurrentAccount(
                        accountNumber,
                        name,
                        age,
                        balance,
                        status,
                        privilege,
                        pin);

                case AccountType.FIXED_DEPOSIT:
                    return new FixedDepositAccount(
                        accountNumber,
                        name,
                        age,
                        balance,
                        status,
                        privilege,
                        pin);

                case AccountType.SALARY:
                    return new SalaryAccount(
                        accountNumber,
                        name,
                        age,
                        balance,
                        status,
                        privilege,
                        pin);

                default:
                    throw new ArgumentException("Invalid account type");
            }
        }
    }
}