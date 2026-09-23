using gdb.Domain.Enums;
using gdb.Domain.Models;
using gdb.Domain.Exceptions;
using gdb.Infrastructure.Repositories.Contracts;
using gdb.Infrastructure.Repositories;


namespace gdb.Application.Services
{
    internal class TransferService
    {
        private readonly IAccountRepository _accountRepository;

        public TransferService()
        {
            // When TransferService is constructed, Repository is created via factory
            _accountRepository = AccountRepositoryFactory.Create("InMemory");
        }

        /*
         * Transfer workflow (sketch)
         * 0. Receive fromAcc, toAcc, pin, amount
         * 0.1 Return TransactionStatus (SUCCESS / PENDING / FAILURE)
         * 1. Check if from account is active - else throw
         * 2. Check if to account is active - else throw
         * 3. Check if fromAcc pin is valid - else throw
         * 4. Check sufficient funds / per-type rules - else throw
         * 5. Withdraw from fromAcc
         * 6. Deposit into toAcc
         * 6.1 update the database (balance of the from & toaccount) 
         * 7. Log transfer (not implemented here)
         * 8. Return status
         */

        public TransactionStatus TransferFunds(string fromAccountNumber, string toAccountNumber, string pin, decimal amount)
        {
            TransactionStatus status = TransactionStatus.PENDING;

            try
            {
                if (string.IsNullOrWhiteSpace(fromAccountNumber)) throw new ArgumentException("fromAccountNumber is required", nameof(fromAccountNumber));
                if (string.IsNullOrWhiteSpace(toAccountNumber)) throw new ArgumentException("toAccountNumber is required", nameof(toAccountNumber));
                if (string.IsNullOrWhiteSpace(pin)) throw new ArgumentException("pin is required", nameof(pin));
                if (amount <= 0m) throw new ArgumentException("amount must be positive", nameof(amount));

                var fromAccount = GetAccount(fromAccountNumber);
                CheckIfAccountIsActive(fromAccount);
                CheckIfPinIsValid(fromAccount, pin);

                var toAccount = GetAccount(toAccountNumber);
                CheckIfAccountIsActive(toAccount);

                // Delegate debit/credit to domain objects which encapsulate rules.
                if (fromAccountNumber == toAccountNumber)
                    throw new ArgumentException("Cannot transfer to the same account.");
                fromAccount.Withdraw(amount, pin);
                toAccount.Deposit(amount);

                // Persist changes via repository if required by implementation (not shown).
                status = TransactionStatus.SUCCESS;
            }
            catch (InactiveAccountException)
            {
                status = TransactionStatus.FAILURE;
            }
            catch (InvalidPinException)
            {
                status = TransactionStatus.FAILURE;
            }
            catch (Exception)
            {
                status = TransactionStatus.FAILURE;
            }

            return status;
        }

        private bool CheckIfPinIsValid(IAccount account, string pin)
        {
            if (!account.Validate_pin(pin))
                throw new InvalidPinException();

            return true;
        }

        private bool CheckIfAccountIsActive(IAccount account)
        {
            if (account.Status != AccountStatus.ACTIVE)
                throw new InactiveAccountException();

            return true;
        }

        // SRP - to get account information only
        private IAccount GetAccount(string accountNumber)
        {
            var account = _accountRepository.GetAccount(accountNumber)
                ?? throw new InvalidOperationException($"Account '{accountNumber}' not found.");

            return account;
        }

    }
}