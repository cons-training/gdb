using gdb.Domain.Enums;
using gdb.Domain.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gdb.Domain.Models
{
    public interface IAccount
    {
        void Deposit(decimal amount);

        void Withdraw(decimal amount, string pin);

        bool Validate_pin(string entered_pin);

        bool Change_pin(string old_pin, string new_pin);

        void DisplayAccountInfo();

        string AccountNumber { get; }
        string Name { get; }
        int Age { get; }
        decimal Balance { get; }
        AccountType AccountType { get; }
        AccountStatus Status { get; }
    }
}
