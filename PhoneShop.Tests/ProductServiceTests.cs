using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PhoneShop.BLL.Implementations;
using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using System.Collections.Generic;

namespace PhoneShop.Tests;

[TestClass]
public class ProductServiceTests
{
	private Mock<IRepository<Product>> _mockRepository = default!;
	private ProductService _productService = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_mockRepository = new Mock<IRepository<Product>>();
		_productService = new ProductService(_mockRepository.Object);
	}

	[TestMethod]
	[DataRow(1)]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(4)]
	[DataRow(50)]
	[DataRow(100)]
	public void Get_ProductExists_ReturnsProduct(int id)
	{
		var product = new Product { Id = id };
		_mockRepository.Setup(x => x.Read(id)).Returns(product);

		var result = _productService.Get(id);

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
	public void Get_ProductNameExists_ReturnsProduct(string name)
	{
		var products = new List<Product> { new() { Name = name } };
		_mockRepository.Setup(x => x.ReadAll()).Returns(products);

		var result = _productService.Get(name);

		Assert.IsNotNull(result);
		Assert.AreEqual(name, result.First().Name);
	}


	[TestMethod]
	public void Add_ValidProduct_AddsProduct()
	{
		var sale = new Product { Id = 1, Phone = new() { Name = "iPhone" } };
		_mockRepository.Setup(x => x.Create(sale)).Verifiable();

		_productService.Add(sale);

		_mockRepository.Verify(x => x.Create(sale), Times.Once);
	}

	[TestMethod]
	public void Remove_ProductExists_RemovesProduct()
	{
		var sale = new Product { Id = 2, Phone = new() { Name = "iPhone" } };
		_mockRepository.Setup(x => x.Delete(sale)).Verifiable();

		_productService.Remove(sale);

		_mockRepository.Verify(x => x.Delete(sale), Times.Once);
	}
}
