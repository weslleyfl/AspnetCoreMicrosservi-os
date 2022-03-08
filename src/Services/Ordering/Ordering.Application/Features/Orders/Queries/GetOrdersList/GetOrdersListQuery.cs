using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Ordering.Application.Models;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    /// <summary>
    /// Get Orders With Pagination
    /// </summary>
    public class GetOrdersListQuery : IRequest<PaginatedList<OrdersViewModel>>
    {
     
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public string UserName { get; set; }

        public GetOrdersListQuery(string userName)
        {
            UserName = userName ?? throw new ArgumentNullException(nameof(userName));
        }
    }
}
