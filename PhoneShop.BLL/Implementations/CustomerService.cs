using System.Collections.Generic;
using System.Text.RegularExpressions;
using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.DAL.Repositories;
using PhoneShop.Domain.Models;

namespace PhoneShop.BLL.Implementations;

public class CustomerService(IRepository<Customer> customerRepository) : IService<Customer>
{
	private readonly IRepository<Customer> _customerRepository = customerRepository;

	public void Add(Customer model)
	{
		_customerRepository.Create(model);
	}

	public Customer Get(int id)
	{
		try
		{
			Customer? customer = GetAll().FirstOrDefault(c => c.Id == id);
			ArgumentNullException.ThrowIfNull(customer);
			return customer;
		}
		catch
		{
			throw new ArgumentNullException($"Покупатель с ID = {id} не найден");
		}
	}

	public IEnumerable<Customer> Get(string customerFullName)
	{
		IEnumerable<Customer> customers = [];
		Customer customerNew = new();

		string pattern = @"^(\w+)\s+(\w+)\s+(\w+)$";
		Match match = Regex.Match(customerFullName, pattern);

		if (match.Success)
		{
			string surname = match.Groups[1].Value;
			string name = match.Groups[2].Value;
			string middleName = match.Groups[3].Value;
			customerNew = new(0, surname, name, middleName);
			customers = GetAll().Where(c => c == customerNew);
		}
		else
		{
			throw new ArgumentException("Строка не соответствует формату ФИО!");
		}

		if(!customers.Any())
		{
			List<Customer> list = [customerNew];
			customers = list;
			return customers;
		}
		return customers;
	}

	public IEnumerable<Customer> GetAll()
		=> _customerRepository.ReadAll();

	public void Remove(Customer model)
	{
		_customerRepository.Delete(model);
	}
}
