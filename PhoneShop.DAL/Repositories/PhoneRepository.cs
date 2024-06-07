using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.DAL.Repositories;

public class PhoneRepository(IDbContext dbContext) : IRepository<Phone>
{
	private readonly IDbContext _dbContext = dbContext;

	public bool Create(Phone model)
	{
		_dbContext.Phones.Insert(model);
		return true;
	}

	public bool Delete(Phone model)
	{
		_dbContext.Phones.Delete(model.Id);
		return true;
	}

	public Phone Read(int id)
	{
		Phone? phone = ReadAll().FirstOrDefault(x => x.Id == id);
		ArgumentNullException.ThrowIfNull(phone);
		return phone;
	}

	public IEnumerable<Phone> ReadAll()
		=> _dbContext.Phones;

	public bool Update(Phone oldModel, Phone newModel)
	{
		_dbContext.Phones.Update(oldModel.Id, newModel);
		return true;
	}
}
