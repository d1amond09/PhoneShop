using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.DAL.Repositories;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.BLL.Implementations;

public class PhoneService(IRepository<Phone> phoneRepository) : IService<Phone, PhoneViewModel>
{
	private readonly IRepository<Phone> _phoneRepository = phoneRepository;

	public void Add(Phone model)
	{
		_phoneRepository.Create(model);
	}

	public PhoneViewModel Get(int id)
	{
		PhoneViewModel phoneVM = new(_phoneRepository.Read(id));
		return phoneVM;
	}

	public IEnumerable<PhoneViewModel> GetAll()
	{
		List<PhoneViewModel> viewModels = [];

		IEnumerable<Phone> Phones = _phoneRepository.ReadAll();
		foreach(Phone Phone in Phones)
		{
			viewModels.Add(new(Phone));
		}
		return viewModels;
	}

	public IEnumerable<PhoneViewModel> Get(string name)
	=> GetAll().Where(x => x.Name.Contains(name));


	IEnumerable<Phone> IGetModelService<Phone>.GetAll()
		=> _phoneRepository.ReadAll();

	public void Remove(Phone model)
	{
		_phoneRepository.Delete(model);
	}

	IEnumerable<Phone> IGetModelService<Phone>.Get(string name)
		=> _phoneRepository.ReadAll().Where(x => x.Name.Contains(name));
}
