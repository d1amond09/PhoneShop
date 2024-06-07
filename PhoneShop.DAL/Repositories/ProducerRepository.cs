using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.DAL.Repositories;

public class ProducerRepository(IDbContext dbContext) : IRepository<Producer>
{
	private readonly IDbContext _dbContext = dbContext;

	public bool Create(Producer model)
	{
		_dbContext.Producers.Insert(model);
		return true;
	}

	public bool Delete(Producer model)
	{
		_dbContext.Producers.Delete(model.Id);
		return true;
	}

	public Producer Read(int id)
	{
		Producer? producer = _dbContext.Producers.FirstOrDefault(p => p.Id == id);
		ArgumentNullException.ThrowIfNull(producer);
		return producer;
	}

	public IEnumerable<Producer> ReadAll()
		=> _dbContext.Producers;

	public bool Update(Producer oldModel, Producer newModel)
	{
		_dbContext.Producers.Update(oldModel.Id, newModel);
		return true;
	}
}
