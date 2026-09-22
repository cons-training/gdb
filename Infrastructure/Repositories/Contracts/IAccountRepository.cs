using gdb.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gdb.Infrastructure.Repositories.Contracts
{
    internal interface IAccountRepository
    {
        IAccount GetAccount(string accountNumber);
        List<IAccount> GetAllAccounts();
    }
}
