using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using MediatR;
using Ordering.Application.Contracts.Persistence;
using Ordering.Application.Mappings;
using Ordering.Application.Models;

namespace Ordering.Application.Features.Orders.Queries.GetOrdersList
{
    /// <summary>
    /// Get Orders With Pagination Query Handler
    /// </summary>
    public class GetOrdersListQueryHandler : IRequestHandler<GetOrdersListQuery, PaginatedList<OrdersViewModel>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public GetOrdersListQueryHandler(IOrderRepository orderRepository, IMapper mapper)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
            _mapper = mapper;
        }

        public async Task<PaginatedList<OrdersViewModel>> Handle(GetOrdersListQuery request, CancellationToken cancellationToken)
        {
            var orderList = await _orderRepository.GetOrdersByUserName(request.UserName);

            return await orderList
                .AsQueryable()
                .ProjectTo<OrdersViewModel>(_mapper.ConfigurationProvider)
                .PaginatedListAsync(request.PageNumber, request.PageSize);

        }
    }
}
