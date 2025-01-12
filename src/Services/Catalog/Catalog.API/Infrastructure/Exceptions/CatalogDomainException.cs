﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Catalog.API.Infrastructure.Exceptions
{
    /// <summary>
    /// Exception type for app exceptions
    /// </summary>
    public class CatalogDomainException : Exception
    {
        public CatalogDomainException() : base()
        {
        }

        public CatalogDomainException(string message) : base(message)
        {
        }

        public CatalogDomainException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
