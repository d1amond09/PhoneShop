using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PhoneShop.BLL.Implementations;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.Tests;

[TestClass]
public class CustomerServiceTests
{
	private Mock<IRepository<Customer>> _mockRepo = default!;
	private CustomerService _service = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_mockRepo = new Mock<IRepository<Customer>>();
		_service = new CustomerService(_mockRepo.Object);
	}

	[TestMethod]
	public void Add_ShouldCallCreate_Once()
	{
		var customer = new Customer();

		_service.Add(customer);

		_mockRepo.Verify(x => x.Create(customer), Times.Once);
	}

	[TestMethod]
	public void Remove_customerExists_Removescustomer()
	{
		var customer = new Customer { Id = 2, Name = "Alex" };
		_mockRepo.Setup(x => x.Delete(customer)).Verifiable();

		_service.Remove(customer);

		_mockRepo.Verify(x => x.Delete(customer), Times.Once);
	}

	[TestMethod]
	public void GetAll_ShouldReturnAllcustomers()
	{
		var customers = new List<Customer> { new(), new() };
		_mockRepo.Setup(x => x.ReadAll()).Returns(customers);

		var result = _service.GetAll();

		Assert.AreEqual(customers.Count, result.Count());
		_mockRepo.Verify(x => x.ReadAll(), Times.Once);
	}
}
