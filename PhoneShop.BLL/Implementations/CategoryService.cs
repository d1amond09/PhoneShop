using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.BLL.Implementations;

public class CategoryService(IRepository<Category> categoryRepository) : IGetService<Category>
{
	private readonly IRepository<Category> _categoryRepository = categoryRepository;
	public Category Get(int id)
	{
		return _categoryRepository.Read(id);
	}

	public IEnumerable<Category> Get(string name)
		=> GetAll().Where(x => x.Name.Contains(name));

	public IEnumerable<Category> GetAll()
		=> _categoryRepository.ReadAll();
}
