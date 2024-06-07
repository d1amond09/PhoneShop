using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.Repositories;

public class UserRepository(IDbContext dbContext) : IRepository<User>
{
	private readonly IDbContext _dbContext = dbContext;

	public bool Create(User model)
	{
		_dbContext.Users.Insert(model);
		return true;
	}

	public bool Delete(User model)
	{
		_dbContext.Users.Delete(model.Id);
		return true;
	}

	public User Read(int id)
	{
		User? user = ReadAll().FirstOrDefault(x => x.Id == id);
		ArgumentNullException.ThrowIfNull(user);
		return user;
	}

	public IEnumerable<User> ReadAll()
		=> _dbContext.Users;

	public bool Update(User oldModel, User newModel)
	{
		_dbContext.Users.Update(oldModel.Id, newModel);
		return true;
	}
}
