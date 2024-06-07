using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.Repositories;

public class CustomerRepository(IDbContext dbContext) : IRepository<Customer>
{
	private readonly IDbContext _dbContext = dbContext;

	public bool Create(Customer model)
	{
		_dbContext.Customers.Insert(model);
		return true;
	}

	public bool Delete(Customer model)
	{
		_dbContext.Customers.Delete(model.Id);
		return true;
	}

	public Customer Read(int id)
	{
		Customer? model = ReadAll().FirstOrDefault(x => x.Id == id);
		ArgumentNullException.ThrowIfNull(model);
		return model;
	}

	public IEnumerable<Customer> ReadAll()
		=> _dbContext.Customers;

	public bool Update(Customer oldModel, Customer newModel)
	{
		_dbContext.Customers.Update(oldModel.Id, newModel);
		return true;
	}
}
