using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PhoneShop.BLL.Implementations;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using System.Collections.Generic;
using System.Xml.Linq;

namespace PhoneShop.Tests;

[TestClass]
public class SaleServiceTests
{
	private Mock<IRepository<Sale>> _mockRepository = default!;
	private SaleService _saleService = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_mockRepository = new Mock<IRepository<Sale>>();
		_saleService = new SaleService(_mockRepository.Object);
	}

	[TestMethod]
	[DataRow(1)]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(4)]
	[DataRow(50)]
	[DataRow(100)]
	public void Get_SaleExists_ReturnsSale(int id)
	{
		var sale = new Sale { Id = id };
		_mockRepository.Setup(x => x.Read(id)).Returns(sale);

		var result = _saleService.Get(id);

		Assert.IsNotNull(result);
		Assert.AreEqual(id, result.Id);
	}

	[TestMethod]
	[DataRow("iPhone")]
	[DataRow("Galaxy")]
	[DataRow("Nokia 3310")]
	[DataRow("POCO X3")]
	[DataRow("Pixel")]
	[DataRow("")]
	public void Get_SaleNameExists_ReturnsSale(string name)
	{
		var sales = new List<Sale> { new() { Product = new() { Name = name } } };
		_mockRepository.Setup(x => x.ReadAll()).Returns(sales);

		var result = _saleService.Get(name);
		 
		Assert.IsNotNull(result);
		Assert.AreEqual(name, result.First().ProductName);
	}

	[TestMethod]
	public void Add_ValidSale_AddsSale()
	{
		var sale = new Sale { Id = 1, Product = new() { Name = "iPhone" } };
		_mockRepository.Setup(x => x.Create(sale)).Verifiable();

		_saleService.Add(sale);

		_mockRepository.Verify(x => x.Create(sale), Times.Once);
	}

	[TestMethod]
	public void Remove_SaleExists_RemovesSale()
	{
		var sale = new Sale { Id = 2, Product = new() { Name = "iPhone" } };
		_mockRepository.Setup(x => x.Delete(sale)).Verifiable();

		_saleService.Remove(sale);

		_mockRepository.Verify(x => x.Delete(sale), Times.Once);
	}

	[TestMethod]
	public void AllEnableProducers_ValidDates_ReturnsProducers()
	{
		var sales = new List<Sale>
		{
			new() { Date = DateTime.Now.AddDays(-1), Product = new Product { Phone = new Phone { Producer = new Producer { Name = "Apple" } } } },
			new() { Date = DateTime.Now, Product = new Product { Phone = new Phone { Producer = new Producer { Name = "Samsung" } } } }
		};
		_mockRepository.Setup(x => x.ReadAll()).Returns(sales);

		var result = _saleService.AllEnableProducers(DateTime.Now.AddDays(-2), DateTime.Now);

		Assert.IsNotNull(result);
		Assert.AreEqual(2, result.Count());
	}

	[TestMethod]
	[DataRow("Apple", 2)]
	[DataRow("Samsung", 3)]
	public void CountByProducer_ValidProducerName_ReturnsCount(string producerName, int expectedCount)
	{
		var sales = new List<Sale>
		{
			new() { Product = new Product { Phone = new Phone { Producer = new Producer { Name = producerName } } } },
			new() { Product = new Product { Phone = new Phone { Producer = new Producer { Name = producerName } } } },
			new() { Product = new Product { Phone = new Phone { Producer = new Producer { Name = "Samsung" } } } }
		};
		_mockRepository.Setup(x => x.ReadAll()).Returns(sales);


		var result = _saleService.CountByProducer(producerName);

		Assert.AreEqual(expectedCount, result);
	}

	[TestMethod]
	[DataRow("Smartphone", 100.0)]
	public void ProfitByType_ValidTypeName_ReturnsProfit(string typeName, double expectedProfit)
	{
		var sales = new List<Sale>
		{
			new() { Price = 200, Product = new Product { Price = 150, Phone = new Phone { Type = new PhoneType { Name = typeName } } } },
			new() { Price = 250, Product = new Product { Price = 200, Phone = new Phone { Type = new PhoneType { Name = typeName } } } }
		};
		_mockRepository.Setup(x => x.ReadAll()).Returns(sales);

		var result = _saleService.ProfitByType(typeName);

		Assert.AreEqual(expectedProfit, result);
	}

	[TestMethod]
	public void AllEnableTypePhones_ValidDates_ReturnsTypePhones()
	{
		var sales = new List<Sale>
		{
			new() { Date = DateTime.Now.AddDays(-1), Product = new Product { Phone = new Phone { Type = new PhoneType { Name = "Smartphone" } } } },
			new() { Date = DateTime.Now, Product = new Product { Phone = new Phone { Type = new PhoneType { Name = "Tablet" } } } }
		};
		_mockRepository.Setup(x => x.ReadAll()).Returns(sales);
		 
		var result = _saleService.AllEnableTypePhones(DateTime.Now.AddDays(-2), DateTime.Now);

		Assert.IsNotNull(result);
		Assert.AreEqual(2, result.Count());
	}

}
