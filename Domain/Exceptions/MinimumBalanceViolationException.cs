using System;

namespace gdb.Domain.Exceptions
{
    /// <summary>
    /// Purpose: Thrown when withdrawal breaches minimum _balance requirement.
    /// </summary>
    public class Minimum_balanceViolationException : AccountException
    {
        public Minimum_balanceViolationException(string message = "") : base(message) { }
    }
}
