using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdb.Domain.Enums;
using gdb.Domain.Exceptions;

namespace gdb.Domain.Models
{
    public abstract class Account:IAccount
    {
        protected string _accountNumber;
        protected string _name;
        protected int _age;
        protected decimal _balance;
        protected AccountType _accountType;
        protected AccountStatus _status;
        protected string _pin;
        protected AccountPrivilege _privilege;

        public Account(string accountNumber, string name, int age, decimal balance, AccountType accountType, AccountStatus status, AccountPrivilege privilege, string pin)
        {
            if (age < 18) throw new ArgumentException("age must be >= 18");
            if (balance < 0.0m) throw new ArgumentException("balance cannot be negative");
            if (pin == null || pin.Length != 4) throw new ArgumentException("pin must be 4 digits");
            this._accountNumber = accountNumber;
            this._name = name;
            this._age = age;
            this._balance = balance;
            this._accountType = accountType;
            this._status = status;
            this._pin = pin;
            this._privilege = privilege;
        }

        public bool Validate_pin(string entered_pin) => _pin != null && _pin == entered_pin;

        public bool Change_pin(string old_pin, string new_pin)
        {
            if (!Validate_pin(old_pin)) return false;
            if (new_pin == null || new_pin.Length != 4) return false;
            _pin = new_pin;
            return true;
        }

        public void Deposit(decimal amount)
        {
            if (amount <= 0.0m) throw new InvalidAmountException("Deposit amount must be positive");
            _balance += amount;
        }

        public void Withdraw(decimal amount, string entered_pin)
        {
            if (!Validate_pin(entered_pin)) throw new InvalidPinException("Invalid _pin");
            if (_status != AccountStatus.ACTIVE) throw new InactiveAccountException("Account is not active");
            if (amount <= 0.0m) throw new InvalidAmountException("Withdrawal amount must be positive");
            ProcessDebit(amount);
        }

        public abstract void ProcessDebit(decimal amount);

        public void DisplayAccountInfo()
        {
            Console.WriteLine($"Account Number: {_accountNumber} | _name: {_name} | _balance: Rs {_balance:F2}");
        }

        public string AccountNumber => _accountNumber;
        public string Name => _name;
        public int Age => _age;
        public decimal Balance => _balance;
        public AccountType AccountType => _accountType;
        public AccountStatus Status => _status;
        public AccountPrivilege Privilege => _privilege;
    }
}
