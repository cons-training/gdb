using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdb.Domain.Enums;
using gdb.Domain.Exceptions;

namespace gdb.Domain.Models
{
    public class CurrentAccount : Account
    {
        private decimal _overdraftLimit;

        public CurrentAccount(string accountNumber, string name, int age, decimal balance, AccountStatus status, AccountPrivilege privilege, string pin, decimal overdraftLimit = 25000.0m)
            : base(accountNumber, name, age, balance, Enums.AccountType.CURRENT,  status, privilege, pin)
        {
            this._overdraftLimit = overdraftLimit;
        }

        public override void ProcessDebit(decimal amount)
        {
            if (amount > _balance + _overdraftLimit)
                throw new Insufficient_balanceException("Overdraft limit exceeded");
            _balance -= amount;
        }

        public decimal OverdraftLimit { get => _overdraftLimit; set => _overdraftLimit = value; }
    }
}
