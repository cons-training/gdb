using gdb.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gdb.Infrastructure.Repositories.Implementations
{
    public class AccountsPrivilege
    {
        // Privilege -> Daily transfer limit
        private Dictionary<AccountPrivilege, decimal> _dailyLimits =new Dictionary<AccountPrivilege, decimal>
            {
                { AccountPrivilege.PREMIUM, 100000m },
                { AccountPrivilege.GOLD, 50000m },
                { AccountPrivilege.SILVER, 25000m }
            };
        

        // Privilege -> Total transactions
        private Dictionary<AccountPrivilege, int> _totalTransactions =new Dictionary<AccountPrivilege, int>
            {
                { AccountPrivilege.PREMIUM, 0 },
                { AccountPrivilege.GOLD, 0 },
                { AccountPrivilege.SILVER, 0 }
            };

        public decimal GetDailyLimit(AccountPrivilege privilege)
        {

            return _dailyLimits[privilege];
        }

    }
}
