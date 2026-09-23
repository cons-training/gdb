using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdb.Domain.Enums;
using gdb.Domain.Exceptions;

namespace gdb.Domain.Models
{
    public class SavingsAccount : Account
    {
        private decimal _min_balance;
        private double _interestRate;

        public SavingsAccount(string accountNumber, string name, int age, decimal balance, AccountStatus status, AccountPrivilege privilege, string pin, decimal min_balance = 1000.0m, double interestRate = 4.0)
            : base(accountNumber, name, age, balance, Enums.AccountType.SAVINGS , status, privilege, pin)
        {
            this._min_balance = min_balance;
            this._interestRate = interestRate;
        }

        public override void ProcessDebit(decimal amount)
        {
            if (_balance - amount < _min_balance)
                throw new Minimum_balanceViolationException($"Cannot breach minimum _balance of Rs {_min_balance:F2}");
            _balance -= amount;
        }

        public void ApplyInterest() => _balance += _balance * (decimal)(_interestRate / 100.0);
        public decimal Min_balance => _min_balance;
        public double InterestRate => _interestRate;
    }
}
