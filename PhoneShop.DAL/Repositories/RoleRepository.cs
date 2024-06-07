using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.Repositories;

public class RoleRepository(IDbContext dbContext) : IRepository<Role>
{
	private readonly IDbContext _dbContext = dbContext;
	public bool Create(Role model)
	{
		_dbContext.Roles.Insert(model);
		return true;
	}

	public bool Delete(Role model)
	{
		_dbContext.Roles.Delete(model.Id);
		return true;
	}

	public Role Read(int id)
	{
		Role? model = ReadAll().FirstOrDefault(x => x.Id == id);
		ArgumentNullException.ThrowIfNull(model);
		return model;
	}

	public IEnumerable<Role> ReadAll()
		=> _dbContext.Roles;

	public bool Update(Role oldModel, Role newModel)
	{
		_dbContext.Roles.Update(oldModel.Id, newModel);
		return true;
	}
}
