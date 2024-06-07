using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.BLL.Implementations;

public class ProducerService(IRepository<Producer> producerRepository) : IGetService<Producer>
{
	private readonly IRepository<Producer> _producerRepository = producerRepository;

	public Producer Get(int id)
	{
		return _producerRepository.Read(id);
	}

	public IEnumerable<Producer> Get(string name)
		=> _producerRepository.ReadAll().Where(x => x.Name.Contains(name));

	public IEnumerable<Producer> GetAll()
		=> _producerRepository.ReadAll();
}
