using GDB.App.Infrastructure.Repositories.Contracts;
using GDB.App.Infrastructure.Repositories.Implementations;

namespace GDB.App.Infrastructure.Repositories
{
    class AccountRepositoryFactory
    {



        public static IAccountRepository Create(string choice)
        {


            IAccountRepository repository = null;


            if (choice.Equals("DB"))

                repository = new AccountRepositoryDB();

            else if (choice.Equals("InMemory"))

                repository = new AccountRepositoryInMemory();


            return repository;

        }

    }
}







