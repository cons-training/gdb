using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace gdb.Domain.Enums
{
    public enum AccountType
    {
        SAVINGS,
        CURRENT,
        FIXED_DEPOSIT,
        SALARY
    }

    public enum AccountStatus
    {
        ACTIVE,
        INACTIVE,
        SUSPENDED,
        CLOSED,
        FROZEN
    }

    public enum AccountPrivilege
    {
        PREMIUM,
        GOLD,
        SILVER
    }

    public enum TransactionStatus
    {
        SUCCESS,
        PENDING,
        FAILURE
    }
}
