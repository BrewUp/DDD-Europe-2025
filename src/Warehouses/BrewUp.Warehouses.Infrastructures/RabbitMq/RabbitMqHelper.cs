using BrewUp.Infrastructure.RabbitMq;
using BrewUp.Warehouses.Domain.CommandHandlers;
using BrewUp.Warehouses.ReadModel.EventHandlers;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Muflone;
using Muflone.Transport.RabbitMQ;
using Muflone.Transport.RabbitMQ.Models;

namespace BrewUp.Warehouses.Infrastructures.RabbitMq;

public static class RabbitMqHelper
{
	public static IServiceCollection AddRabbitMqForWarehousesModule(this IServiceCollection services,
		RabbitMqSettings rabbitMqSettings)
	{
		var serviceProvider = services.BuildServiceProvider();
		var loggerFactory = serviceProvider.GetRequiredService<ILoggerFactory>();

		var rabbitMqConfiguration = new RabbitMQConfiguration(rabbitMqSettings.Host, rabbitMqSettings.Username,
			rabbitMqSettings.Password, rabbitMqSettings.ExchangeCommandName, rabbitMqSettings.ExchangeEventName, "WarehouseClient");

		services.AddMufloneTransportRabbitMQ(loggerFactory, rabbitMqConfiguration);
		
		services.AddCommandHandler<UpdateAvailabilityDueToProductionOrderCommandHandler>();

		services.AddDomainEventHandler<AvailabilityUpdatedDueToProductionOrderEventHandler>();
		services.AddDomainEventHandler<AvailabilityUpdatedDueToProductionOrderForIntegrationEventHandler>();

		return services;
	}
}