using gdb.Infrastructure.Repositories.Implementations;
using gdb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdb.Infrastructure.Repositories.Contracts;
using gdb.Infrastructure.Repositories;
using gdb.Application.Services;

namespace gdb.Application.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IAccountRepository _accountRepository;

        public AccountService()
        {
            // When TransferService is constructed, Repository is created via factory
            _accountRepository = AccountRepositoryFactory.Create("InMemory");
        }

        //Business Logic 
        public IAccount GetAccount(string accNo)
        {
            //Service-> Repository
            // var account = new AccountRepositoryInMemory().GetAccount(accNo);
            var account = _accountRepository.GetAccount(accNo);

            return account;
        }
    }
}
