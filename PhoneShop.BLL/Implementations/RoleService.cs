using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.DAL.Repositories;
using PhoneShop.Domain.Models.Access;
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.BLL.Implementations;

public class RoleService(IRepository<Role> roleRepository) : IGetService<Role>
{
	private readonly IRepository<Role> _roleRepository = roleRepository;

	public Role Get(int id)
	{
		return _roleRepository.Read(id);
	}

	public IEnumerable<Role> Get(string name)
		=> _roleRepository.ReadAll().Where(x => x.Name.Contains(name));

	public IEnumerable<Role> GetAll()
		=> _roleRepository.ReadAll();
}
