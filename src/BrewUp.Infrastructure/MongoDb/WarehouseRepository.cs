using BrewUp.Persistence.Services;
using BrewUp.Shared.Entities;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace BrewUp.Infrastructure.MongoDb;

public class WarehouseRepository : IRepository
{
	private readonly IMongoDatabase _database;
	private readonly ILogger _logger;

	public WarehouseRepository(IMongoClient mongoClient,
		ILoggerFactory loggerFactory)
	{
		_logger = loggerFactory.CreateLogger(GetType());
		_database = mongoClient.GetDatabase("Sales");
	}

	public async Task<T> GetByIdAsync<T>(string id) where T : EntityBase
	{
		var type = typeof(T).Name;
		try
		{
			var collection = _database.GetCollection<T>(typeof(T).Name);
			var filter = Builders<T>.Filter.Eq("_id", id);
			return (await collection.FindAsync(filter)).FirstOrDefault();
		}
		catch (Exception e)
		{
			_logger.LogError("GetById: Error reading DTO: {Type}, Message: {EMessage}, StackTrace: {EStackTrace}", type,
				e.Message, e.StackTrace);
			throw;
		}
	}

	public async Task InsertAsync<T>(T entity) where T : EntityBase
	{
		var type = typeof(T).Name;
		try
		{
			var collection = _database.GetCollection<T>(type);
			await collection.InsertOneAsync(entity);
		}
		catch (Exception e)
		{
			_logger.LogError($"Insert: Error saving DTO: {type}, Message: {e.Message}, StackTrace: {e.StackTrace}");
			throw;
		}
	}

	public async Task UpdateAsync<T>(T entity) where T : EntityBase
	{
		var type = typeof(T).Name;
		try
		{
			var collection = _database.GetCollection<T>(type);
			await collection.ReplaceOneAsync(x => x.Id == entity.Id, entity);
		}
		catch (Exception e)
		{
			_logger.LogError($"Update: Error saving DTO: {type}, Message: {e.Message}, StackTrace: {e.StackTrace}");
			throw;
		}
	}

	public async Task DeleteAsync<T>(T entity) where T : EntityBase
	{
		var type = typeof(T).Name;
		try
		{
			var collection = _database.GetCollection<T>(typeof(T).Name);
			var filter = Builders<T>.Filter.Eq("_id", entity.Id);
			await collection.FindOneAndDeleteAsync(filter);
		}
		catch (Exception e)
		{
			_logger.LogError($"Delete: Error saving DTO: {type}, Message: {e.Message}, StackTrace: {e.StackTrace}");
			throw;
		}
	}
}
