using System;

namespace gdb.Domain.Exceptions
{
    /// <summary>
    /// Purpose: Thrown when deposit or withdrawal amount <= 0.
    /// </summary>
    public class InvalidAmountException : AccountException
    {
        public InvalidAmountException(string message = "") : base(message) { }
    }
}
