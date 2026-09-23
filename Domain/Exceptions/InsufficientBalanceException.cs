using System;

namespace gdb.Domain.Exceptions
{
    /// <summary>
    /// Purpose: Thrown when withdrawal amount exceeds available funds.
    /// </summary>
    public class Insufficient_balanceException : AccountException
    {
        public Insufficient_balanceException(string message = "") : base(message) { }
    }
}
