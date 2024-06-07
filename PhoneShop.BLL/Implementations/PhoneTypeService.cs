using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.BLL.Implementations;

public class PhoneTypeService(IRepository<PhoneType> phoneTypeRepository) : IGetService<PhoneType>
{
	private readonly IRepository<PhoneType> _phoneTypeRepository = phoneTypeRepository;

	public PhoneType Get(int id)
	{
		return _phoneTypeRepository.Read(id);
	}

	public IEnumerable<PhoneType> Get(string name)
		=> GetAll().Where(x => x.Name.Contains(name));

	public IEnumerable<PhoneType> GetAll()
		=> _phoneTypeRepository.ReadAll();
}
