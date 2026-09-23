

//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Numerics;
//using System.Text;
//using System.Threading.Tasks;
//using GDB.App.Infrastructure.Repositories.Contracts;
//using GDB.App.Domain.Models;
//using GDB.App.Domain.Enums;
//using GDB.App.Domain.Exceptions;
//using GDB.App.Infrastructure.Repositories;

//namespace GDB.App.Application.Services.Implementations
//{
//    public class TransferService
//    {


//        private IAccountRepository _accountRepository;



//        public TransferService()

//        {

//            //When TRansfer Service is constructed, Repository is constructed
//            string choice = "InMemory";
//            _accountRepository = AccountRepositoryFactory.Create(choice);

//        }



//        /* 0.Recieve fromAcc, toAcc, pinNumber, amount

//        * 0.1 Return TransactionStatus(it can be success or failure)

//        * Add Enum TransactionStatus with success, pending and failure.

//        * 1.Check if from account is active-else throw Exception

//        * 2.Check if to account is active-else throw Exception

//        * 3.Check if fromAcc pin is valid-else throw Exception

//        * 4.Check if fromAcc has sufficient funds.-else throw Exception

//        * 5.0-get dailyLimit for Transfer

//        * create a seperate class PrivilegeRepository

//        * create a dictionary with key-privilege and value-decimal

//        * 5.Check if dailyTransferLimit has exceeded -else throw Exception

//        * get Transfers done by fromAcc-it is a method-in TransactionLog.cs

//        * 6.Withdraw from fromAcc-else throw Exception

//        * 7.Deposit into toAcc-else throw Exception

//        * 8.Log the transfer -create a seperate class-TransactionLog.cs

//        * create a dictionary with key-account and value-decimal

//        * 9.Return the status

//        */

//        public TransactionStatus TransferFunds(string fromAccountNumber, string toAccountNumber, string pin, decimal amount)

//        {

//            TransactionStatus status = TransactionStatus.Pending;

//            try
//            {

//                // Get Account info from the database

//                var fromAccount = GetAccount(fromAccountNumber);



//                //Chheck if account is active

//                CheckIfAccountIsActive(fromAccount);



//                // Get account information for to account

//                var toAccount = GetAccount(toAccountNumber);



//                //Chheck if account is active

//                CheckIfAccountIsActive(toAccount);



//                //Check if pin is valid

//                CheckIfPinIsValid(fromAccount,pin);
//                Console.WriteLine("BEFORE TRANSFER");
//                DisplayAccount("FROM ACCOUNT", fromAccount);
//                DisplayAccount("TO ACCOUNT", toAccount);

//                fromAccount.Withdraw(amount, pin);

//                toAccount.Deposit(amount);
//                _accountRepository.SaveAccounts(fromAccount, toAccount);

//                Console.WriteLine("AFTER TRANSFER");
//                DisplayAccount("FROM ACCOUNT", fromAccount);
//                DisplayAccount("TO ACCOUNT", toAccount);
//                status = TransactionStatus.Success;

//            }

//            catch (InactiveAccountException ex)
//            {

//                throw new InactiveAccountException();

//            }

//            catch (InvalidPinException ex)
//            {

//                throw new InvalidPinException();

//            }




//            return status;

//        }



//        private bool CheckIfPinIsValid(IAccount account, string pinNumber)
//        {



//            if (!account.ValidatePin(pinNumber)) 
//                throw new InvalidPinException();



//            return true;

//        }



//        private bool CheckIfAccountIsActive(IAccount account)
//        {



//            if (account.Status != AccountStatus.Active)
//                   throw new InactiveAccountException();



//            return true;

//        }



//        //SRP - Single Responsiblity Principle

//        //To get the acount information only

//        private IAccount GetAccount(string accountNumber)
//        {

//            // Get Account info from the database

//            //Moment you create ab object of a class inside a method, then you are directly

//            //dependent on the object. Tight Coupling

//            //AccountfactoryRepository creatae method is returning an interace

//            //therefore the TransferSerivce is programming to an interace and not implementation

//            //Once - Use and Dispose - Uses Relationship

//            //var accountRepository = AccountRepositoryFactory.Create();

//            var account = _accountRepository.GetAccount(accountNumber);



//            return account;

//        }
//        private void DisplayAccount(string message, IAccount account)
//        {
//            Console.WriteLine(message);
//            Console.WriteLine("--------------------------------");
//            Console.WriteLine($"Account Number : {account.AccountNumber}");
//            Console.WriteLine($"Name           : {account.Name}");
//            Console.WriteLine($"Balance        : {account.Balance}");
//            Console.WriteLine();
//        }



//    }
//}
