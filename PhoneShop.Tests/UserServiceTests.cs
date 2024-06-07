using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PhoneShop.BLL.Implementations;
using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.Tests;

[TestClass]
public class UserServiceTests
{
	private Mock<IRepository<User>> _mockRepo = default!;
	private UserService _service = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_mockRepo = new Mock<IRepository<User>>();
		_service = new UserService(_mockRepo.Object);
	}
	[TestMethod]
	public void Add_ShouldCallCreate_Once()
	{
		var user = new User();

		_service.Add(user);

		_mockRepo.Verify(x => x.Create(user), Times.Once);
	}

	[TestMethod]
	public void Remove_UserExists_RemovesUser()
	{
		var user = new User { Id = 2, Login = "adddmin" };
		_mockRepo.Setup(x => x.Delete(user)).Verifiable();

		_service.Remove(user);

		_mockRepo.Verify(x => x.Delete(user), Times.Once);
	}

	[TestMethod]
	[DataRow(1)]
	[DataRow(2)]
	[DataRow(3)]
	[DataRow(4)]
	[DataRow(50)]
	[DataRow(100)]
	public void Get_UserExists_ReturnsUser(int id)
	{
		var user = new User { Id = id };
		_mockRepo.Setup(x => x.Read(id)).Returns(user);

		var result = _service.Get(id);

		Assert.IsNotNull(result);
		Assert.AreEqual(id, result.Id);
	}

}
