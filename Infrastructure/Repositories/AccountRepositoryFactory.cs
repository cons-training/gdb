
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using gdb.Infrastructure.Repositories.Contracts;
using gdb.Infrastructure.Repositories.Implementations;


namespace gdb.Infrastructure.Repositories
{
    internal class AccountRepositoryFactory
    {
        
        public static IAccountRepository Create(string choice)
        {
            if (string.IsNullOrWhiteSpace(choice))
                throw new ArgumentException("choice is required", nameof(choice));

            //if (choice.Equals("DB", StringComparison.OrdinalIgnoreCase))
            //    return new AccountRepositoryDB();

            if (choice.Equals("InMemory", StringComparison.OrdinalIgnoreCase))
                return new AccountRepositoryInMemory();

            throw new ArgumentException($"Unknown repository choice: {choice}", nameof(choice));
        }

    }
}
