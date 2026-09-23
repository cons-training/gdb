using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdb.Domain.Enums;
using gdb.Domain.Exceptions;

namespace gdb.Domain.Models
{
    public class FixedDepositAccount : Account
    {
        private int _tenureMonths;
        private double interestRate;

        public FixedDepositAccount(string accountNumber, string name, int age, decimal balance, AccountStatus status, AccountPrivilege privilege, string pin, int tenureMonths = 12, double interestRate = 6.5)
            : base(accountNumber, name, age, balance, Enums.AccountType.FIXED_DEPOSIT, status, privilege, pin)
        {
            this._tenureMonths = tenureMonths;
            this.interestRate = interestRate;
        }

        public override void ProcessDebit(decimal amount)
        {
            throw new AccountException("Premature withdrawal not permitted on Fixed Deposit Account");
        }

        public decimal CalculateMaturityAmount() => _balance * (decimal)Math.Pow(1 + interestRate / 100.0 / 12, 12 * (_tenureMonths / 12.0));
        public int TenureMonths => _tenureMonths;
        public double InterestRate => interestRate;
    }
}
