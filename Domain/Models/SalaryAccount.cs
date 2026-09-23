using System;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdb.Domain.Enums;
using gdb.Domain.Exceptions;

namespace gdb.Domain.Models
{
    public class SalaryAccount : Account
    {
        private string _employer_name;
        private int _inactiveMonths;

        public SalaryAccount(string accountNumber, string name, int age, decimal balance, AccountStatus status, AccountPrivilege privilege, string pin, string employer_name = "TechCorp")
            : base(accountNumber, name, age, balance, Enums.AccountType.SALARY, status, privilege, pin)
        {
            this._employer_name = employer_name;
            _inactiveMonths = 0;
        }

        public override void ProcessDebit(decimal amount)
        {
            if (amount > _balance)
                throw new Insufficient_balanceException("Insufficient funds in Salary account");
            _balance -= amount;
        }

        public void IncrementInactiveMonths()
        {
            _inactiveMonths++;
            if (_inactiveMonths >= 3) _status = AccountStatus.FROZEN;
        }

        public string Employer_name => _employer_name;
        public int InactiveMonths => _inactiveMonths;
    }
}
