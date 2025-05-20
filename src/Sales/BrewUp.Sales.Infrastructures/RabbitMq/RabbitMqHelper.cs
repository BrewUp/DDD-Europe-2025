using BrewUp.Infrastructure.RabbitMq;
using BrewUp.Sales.Acl;
using BrewUp.Sales.Domain.CommandHandlers;
using BrewUp.Sales.ReadModel.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Transport.RabbitMQ;
using Muflone.Transport.RabbitMQ.Models;

namespace BrewUp.Sales.Infrastructures.RabbitMq;

public static class RabbitMqHelper
{
	public static IServiceCollection AddRabbitMqForSalesModule(this IServiceCollection services,
		RabbitMqSettings rabbitMqSettings)
	{
		var serviceProvider = services.BuildServiceProvider();
		var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

		var rabbitMqConfiguration = new RabbitMQConfiguration(rabbitMqSettings.Host, rabbitMqSettings.Username,
			rabbitMqSettings.Password, rabbitMqSettings.ExchangeCommandName, rabbitMqSettings.ExchangeEventName, "SalesClient");

		services.AddMufloneTransportRabbitMQ(loggerFactory, rabbitMqConfiguration);
		
		services.AddCommandHandler<CreateSalesOrderCommandHandler>();
		services.AddCommandHandler<UpdateAvailabilityDueToWarehousesNotificationCommandHandler>();

		services.AddDomainEventHandler<AvailabilityUpdatedDueToWarehousesNotificationEventHandler>();
		services.AddDomainEventHandler<SalesOrderCreatedEventHandlerAsync>();
		services.AddIntegrationEventHandler<AvailabilityUpdatedForNotificationEventHandler>();

		return services;
	}
}