using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.Repositories;

public class PhoneTypeRepository(IDbContext dbContext) : IRepository<PhoneType>
{
	private readonly IDbContext _dbContext = dbContext;
	public bool Create(PhoneType model)
	{
		_dbContext.PhoneTypes.Insert(model);
		return true;
	}

	public bool Delete(PhoneType model)
	{
		_dbContext.PhoneTypes.Delete(model.Id);
		return true;
	}

	public PhoneType Read(int id)
	{
		PhoneType? model = ReadAll().FirstOrDefault(x => x.Id == id);
		ArgumentNullException.ThrowIfNull(model);
		return model;
	}

	public IEnumerable<PhoneType> ReadAll()
		=> _dbContext.PhoneTypes;

	public bool Update(PhoneType oldModel, PhoneType newModel)
	{
		_dbContext.PhoneTypes.Update(oldModel.Id, newModel);
		return true;
	}
}
