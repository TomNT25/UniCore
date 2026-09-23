using System;
using System.Collections.Generic;
using System.Text;

namespace UniCore.Helper.ExceptionHandler
{
    public class DatabaseOperationException : Exception
    {
        public DatabaseOperationException(string message) : base(message) { }

        public DatabaseOperationException(string message, Exception innerException)
            : base(message, innerException) { }
    }
}
