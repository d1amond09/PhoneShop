using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using PhoneShop.BLL.Implementations;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.Tests;

[TestClass]
public class RoleServiceTests
{
	private Mock<IRepository<Role>> _mockRepo = default!;
	private RoleService _service = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_mockRepo = new Mock<IRepository<Role>>();
		_service = new RoleService(_mockRepo.Object);
	}

	[TestMethod]
	public void GetAll_ShouldReturnAllRoles()
	{
		var roles = new List<Role> { new(), new() };
		_mockRepo.Setup(x => x.ReadAll()).Returns(roles);

		var result = _service.GetAll();

		Assert.AreEqual(roles.Count, result.Count());
		_mockRepo.Verify(x => x.ReadAll(), Times.Once);
	}
}
