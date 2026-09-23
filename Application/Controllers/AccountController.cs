using gdb.Application.Services;
using gdb.Application.Services.Implementations;
using gdb.Domain.Models;
using gdb.Infrastructure.Repositories.Contracts;
using gdb.Infrastructure.Repositories.Implementations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gdb.Application.Controllers
{
    internal class AccountController
    {
        private IAccountService _accountService;
        private IAccountRepository _accountRepository;
        public AccountController()
        {
            _accountService = AccountServiceFactory.Create();
            _accountRepository = new AccountRepositoryInMemory();
        }
        
        //Boundary Class 
        public IAccount GetAccount(string accNo)
        {
            if (string.IsNullOrEmpty(accNo))
            {
                throw new ArgumentException("Account number cannot be null or empty.", nameof(accNo));
            }

            IAccount account =  _accountService.GetAccount(accNo);
            return account;
        }

        public List<IAccount> GetAllAccounts()
        {
             List<IAccount> accounts = _accountRepository.GetAllAccounts();
            return accounts;
        }
    }
}
    