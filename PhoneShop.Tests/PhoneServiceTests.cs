using Moq;
using PhoneShop.BLL.Implementations;
using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using System.Collections.Generic;
using System.Linq;

namespace PhoneShop.Tests;

[TestClass]
public class PhoneServiceTests
{
	private Mock<IRepository<Phone>> _mockRepo = default!;
	private PhoneService _service = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_mockRepo = new Mock<IRepository<Phone>>();
		_service = new PhoneService(_mockRepo.Object);
	}

	[TestMethod]
	public void Add_ShouldCallCreate_Once()
	{
		var phone = new Phone();

		_service.Add(phone);

		_mockRepo.Verify(x => x.Create(phone), Times.Once);
	}

	[TestMethod]
	public void Remove_PhoneExists_RemovesPhone()
	{
		var phone = new Phone { Id = 2, Name = "iPhone" };
		_mockRepo.Setup(x => x.Delete(phone)).Verifiable();

		_service.Remove(phone);

		_mockRepo.Verify(x => x.Delete(phone), Times.Once);
	}

	[TestMethod]
	[DataRow(1)]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(4)]
	[DataRow(50)]
	[DataRow(100)]
	public void Get_PhoneExists_ReturnsPhone(int id)
	{
		var phone = new Phone { Id = id };
		_mockRepo.Setup(x => x.Read(id)).Returns(phone);

		var result = _service.Get(id);

		Assert.IsNotNull(result);
		Assert.AreEqual(id, result.Id);
	}

	[TestMethod]
	public void GetAll_ShouldReturnAllPhones()
	{
		var phones = new List<Phone> { new Phone(), new Phone() };
		_mockRepo.Setup(x => x.ReadAll()).Returns(phones);

		var result = _service.GetAll();

		Assert.AreEqual(phones.Count, result.Count());
		_mockRepo.Verify(x => x.ReadAll(), Times.Once);
	}

}