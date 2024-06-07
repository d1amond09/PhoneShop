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
public class PhoneTypeServiceTests
{
	private Mock<IRepository<PhoneType>> _mockRepo = default!;
	private PhoneTypeService _service = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_mockRepo = new Mock<IRepository<PhoneType>>();
		_service = new PhoneTypeService(_mockRepo.Object);
	}

	[TestMethod]
	public void GetAll_ShouldReturnAllPhoneTypes()
	{
		var phoneTypes = new List<PhoneType> { new(), new() };
		_mockRepo.Setup(x => x.ReadAll()).Returns(phoneTypes);

		var result = _service.GetAll();

		Assert.AreEqual(phoneTypes.Count, result.Count());
		_mockRepo.Verify(x => x.ReadAll(), Times.Once);
	}
}