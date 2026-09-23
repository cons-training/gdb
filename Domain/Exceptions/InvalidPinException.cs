using System;

namespace gdb.Domain.Exceptions
{
    /// <summary>
    /// Purpose: Thrown when entered _pin does not match account _pin.
    /// </summary>
    public class InvalidPinException : AccountException
    {
        public InvalidPinException(string message = "") : base(message) { }
    }
}
