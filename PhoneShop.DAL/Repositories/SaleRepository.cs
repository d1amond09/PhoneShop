using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.Repositories;

public class SaleRepository(IDbContext dbContext) : IRepository<Sale>
{
	private readonly IDbContext _dbContext = dbContext;
	public bool Create(Sale model)
	{
		_dbContext.Sales.Insert(model);
		return true;
	}

	public bool Delete(Sale model)
	{
		_dbContext.Sales.Delete(model.Id);
		return true;
	}

	public Sale Read(int id)
	{
		Sale? sale = ReadAll().FirstOrDefault(x => x.Id == id);
		ArgumentNullException.ThrowIfNull(sale);
		return sale;
	}

	public IEnumerable<Sale> ReadAll()
	{
		return _dbContext.Sales;
	}

	public bool Update(Sale oldModel, Sale newModel)
	{
		_dbContext.Sales.Update(oldModel.Id, newModel);
		return true;
	}
}
