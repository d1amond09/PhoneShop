using MySql.Data.MySqlClient;
using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.DAL.Repositories;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.BLL.Implementations;

public class UserService(IRepository<User> userRepository) : IService<User, UserViewModel>
{
	private readonly IRepository<User> _userRepository = userRepository;

	public UserViewModel Get(int id)
	{
		UserViewModel userVM = new(_userRepository.Read(id));
		return userVM;
	}

	public IEnumerable<UserViewModel> GetAll()
	{
		List<UserViewModel> viewModels = [];

		IEnumerable<User> users = _userRepository.ReadAll();
		foreach (User user in users)
		{
			viewModels.Add(new(user));
		}
		return viewModels;
	}

	public void Add(User model)
	{
		try
		{
			_userRepository.Create(model);
		}
		catch(Exception ex)
		{
			throw new Exception(ex.Message);
		}
	}

	public void Remove(User model)
	{
		_userRepository.Delete(model);
	}

	public IEnumerable<UserViewModel> Get(string name)
		=> GetAll().Where(x => x.Login.Contains(name));

	IEnumerable<User> IGetModelService<User>.GetAll()
		=> _userRepository.ReadAll();


	IEnumerable<User> IGetModelService<User>.Get(string name)
		=> _userRepository.ReadAll().Where(x => x.Login.Contains(name));
}
