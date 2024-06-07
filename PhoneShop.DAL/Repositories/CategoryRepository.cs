using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.DAL.Repositories;

public class CategoryRepository(IDbContext dbContext) : IRepository<Category>
{
	private readonly IDbContext _dbContext = dbContext;

	public bool Create(Category model)
	{
		_dbContext.Categories.Insert(model);
		return true;
	}

	public bool Delete(Category model)
	{
		_dbContext.Categories.Delete(model.Id);
		return true;
	}

	public Category Read(int id)
	{
		Category? model = ReadAll().FirstOrDefault(x => x.Id == id);
		ArgumentNullException.ThrowIfNull(model);
		return model;
	}

	public IEnumerable<Category> ReadAll()
		=> _dbContext.Categories;

	public bool Update(Category oldModel, Category newModel)
	{
		_dbContext.Categories.Update(oldModel.Id, newModel);
		return true;
	}
}
