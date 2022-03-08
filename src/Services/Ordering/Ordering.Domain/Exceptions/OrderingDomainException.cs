using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ordering.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class OrderingDomainException : Exception
    {
        public OrderingDomainException() : base()
        {
        }

        public OrderingDomainException(string message) : base(message)
        {
        }

        public OrderingDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
