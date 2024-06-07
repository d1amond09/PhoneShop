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
public class ProducerServiceTests
{
	private Mock<IRepository<Producer>> _mockRepo = default!;
	private ProducerService _service = default!;

	[TestInitialize]
	public void TestInitialize()
	{
		_mockRepo = new Mock<IRepository<Producer>>();
		_service = new ProducerService(_mockRepo.Object);
	}

	[TestMethod]
	public void GetAll_ShouldReturnAllProducers()
	{
		var producers = new List<Producer> { new(), new() };
		_mockRepo.Setup(x => x.ReadAll()).Returns(producers);

		var result = _service.GetAll();

		Assert.AreEqual(producers.Count, result.Count());
		_mockRepo.Verify(x => x.ReadAll(), Times.Once);
	}
}