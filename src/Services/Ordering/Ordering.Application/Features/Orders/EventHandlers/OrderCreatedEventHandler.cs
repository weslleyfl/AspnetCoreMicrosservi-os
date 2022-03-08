using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using Ordering.Domain.Entities;
using Ordering.Domain.Events;

namespace Ordering.Application.Features.Orders.EventHandlers
{
    public class OrderCreatedEventHandler : INotificationHandler<DomainEventNotification<OrderCreatedDomainEvent>>
    {
        private readonly ILogger<OrderCreatedEventHandler> _logger;
        private readonly IEmailService _emailService;

        public OrderCreatedEventHandler(ILogger<OrderCreatedEventHandler> logger, IEmailService emailService)
        {
            _logger = logger;
            _emailService = emailService;
        }

        public Task Handle(DomainEventNotification<OrderCreatedDomainEvent> notification, CancellationToken cancellationToken)
        {
            var domainEvent = notification.DomainEvent;

            // await SendMail(newOrder);

            _logger.LogInformation("CleanArchitecture Domain Event: {DomainEvent}", domainEvent.GetType().Name);

            return Task.CompletedTask;
        }


        private async Task SendMail(Order order)
        {
            var email = new Email() { To = "ezozkme@gmail.com", Body = $"Order was created.", Subject = "Order was created" };

            try
            {
                await _emailService.SendEmail(email);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Order {order.Id} failed due to an error with the mail service: {ex.Message}");
            }
        }
    }
}
