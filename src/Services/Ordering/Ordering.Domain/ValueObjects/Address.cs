using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Common;

namespace Ordering.Domain.ValueObjects
{
    public class Address : ValueObject
    {
      
        public string AddressLine { get; private set; }
        public string Country { get; private set; }
        public string State { get; private set; }
        public string ZipCode { get; private set; }


        static Address() { }

        public Address() { }

        public Address(string addressLine, string country, string state, string zipCode)
        {

            AddressLine = addressLine;
            Country = country;
            State = state;
            ZipCode = zipCode;
        }

        public override string ToString()
        {
            return $"Rua {AddressLine} - {State}, {Country} - Cep {ZipCode}";
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time          
            yield return AddressLine;
            yield return Country;
            yield return State;
            yield return ZipCode;
        }
    }
}
