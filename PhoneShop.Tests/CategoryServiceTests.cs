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
public class CategoryServiceTests
{
	private Mock<IRepository<Category>> _mockRepo = default!;
	private CategoryService _service = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_mockRepo = new Mock<IRepository<Category>>();
		_service = new CategoryService(_mockRepo.Object);
	}

	[TestMethod]
	public void GetAll_ShouldReturnAllCategorys()
	{
		var categories = new List<Category> { new(), new() };
		_mockRepo.Setup(x => x.ReadAll()).Returns(categories);

		var result = _service.GetAll();

		Assert.AreEqual(categories.Count, result.Count());
		_mockRepo.Verify(x => x.ReadAll(), Times.Once);
	}
}
