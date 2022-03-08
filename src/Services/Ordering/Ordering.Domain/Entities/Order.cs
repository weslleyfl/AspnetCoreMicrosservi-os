using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Common;
using Ordering.Domain.Events;
using Ordering.Domain.ValueObjects;

namespace Ordering.Domain.Entities
{
    public class Order : EntityBase, IAggregateRoot, IHasDomainEvent
    {
        public string UserName { get; set; }
        public decimal TotalPrice { get; set; }

        // BillingAddress
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
        public Address BillingAddress { get; set; }

        // Payment
        public string CardName { get; set; }
        public string CardNumber { get; set; }
        public string Expiration { get; set; }
        public string CVV { get; set; }
        public int PaymentMethod { get; set; }


        public OrderStatus OrderStatus { get; private set; }
        private int _orderStatusId;

        public List<DomainEvent> DomainEvents { get; set; } = new List<DomainEvent>();


        public Order()
        {
            // Aquí se está obligando ese “contrato” o “business rules” especificando que cada vez que se crea un entity Order, un evento va a existir
            DomainEvents.Add(new OrderCreatedDomainEvent(this));
        }
        


    }
}
