using gdb.Application.Controllers;
using gdb.Application.Services;
using gdb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gdb.Presentation
{
    internal class Home
    {
        public void Start()
        {
            Console.WriteLine("Welcome to GDB");

            Console.WriteLine("1. Create Account\n2. View Account" +
                "\n3. View All Accounts\n4. View Balance" +
                "\n5. View Recent Transactions\n6. Withdraw" +
                "\n7. Deposit\n8. Transfer Funds\n9.Change Pin" +
                "\n0. Exit");
            Console.WriteLine("Enter your choice");
            int choice = int.Parse(Console.ReadLine());
            switch (choice)
            {
                case 1:
                    CreateAccount();
                    break;
                case 2:
                    ViewAccount();
                    break;
                case 3:
                    ViewAllAccounts();
                    break;
                case 4:
                    ViewBalance();
                    break;
                case 5:
                    ViewRecentTransactions();
                    break;
                case 6:
                    Withdraw();
                    break;
                case 7:
                    Deposit();
                    break;
                case 8:
                    TransferFunds();
                    break;
                case 9:
                    ChangePin();
                    break;
                case 0:
                    Exit();
                    break;
                default: break;

            }
        }

        public void CreateAccount()
        {

        }
        public void ViewAccount()
        {
            //Accept account Number ot get the account info
            Console.WriteLine("Enter The Account Number: ");
            string accNo = Console.ReadLine() ?? string.Empty;

            //Contact the DB to get the Account Info
            var account = new AccountController().GetAccount(accNo!);

            //Display the Account Info
            Console.WriteLine("Name: " + account.Name);

        }
        public void ViewAllAccounts()
        {
            Console.WriteLine("View All Accounts");
            var accounts = new AccountController().GetAllAccounts();
            foreach (var account in accounts)
            {
                Console.WriteLine("Account Number: " + account.AccountNumber + ", Name: " + account.Name);
            }
        }
        public void ViewBalance()
        {
              Console.WriteLine("Enter The Account Number: ");
            string accNo = Console.ReadLine() ?? string.Empty;

            //Contact the DB to get the Account Info
            var account = new AccountController().GetAccount(accNo!);
            Console.WriteLine("Name: " + account.Name + ", Balance: " + account.Balance);
        }
        public void ViewRecentTransactions()
        {
            Console.WriteLine("View Recent Transactions");
            
        }
        public void Withdraw()
        {

        }
        public void Deposit()
        {

        }
        public void TransferFunds()
        {
            TransferService transferService = new TransferService();
            Console.WriteLine("Enter the source account number: ");
            string sourceAccountNumber = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Enter the destination account number: ");
            string destinationAccountNumber = Console.ReadLine() ?? string.Empty;
            Console.WriteLine("Enter the amount to transfer: ");
            decimal amount = decimal.Parse(Console.ReadLine() ?? "0");
            Console.WriteLine("Enter PIN for the source account: ");
            string pin = Console.ReadLine() ?? string.Empty;
            TransactionStatus x = transferService.TransferFunds(sourceAccountNumber, destinationAccountNumber,pin, amount);
        }
        public void ChangePin()
        {

        }
        public void Exit()
        {

        }
    }
}
// introduce transaction service which has withdraw, deposit, transfer