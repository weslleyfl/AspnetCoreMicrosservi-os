using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ordering.Domain.Common;
using Ordering.Domain.Entities;

namespace Ordering.Domain.Events
{
    // https://dev.to/isaacojeda/ddd-cqrs-aplicando-domain-events-en-aspnet-core-o6n

    /// <summary>
    /// Pedido (Order) novo que sera registrado
    /// </summary>
    public class OrderCreatedDomainEvent : DomainEvent
    {
        public Order Order { get; }

        public OrderCreatedDomainEvent(Order order)
        {
            Order = order;
        }

    }
}
