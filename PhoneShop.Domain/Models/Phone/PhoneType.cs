using PhoneShop.Domain.Interfaces;

namespace PhoneShop.Domain.Models;

public class PhoneType(int id, string name) : IModel
{
	public int Id { get; set; } = id;
	public string Name { get; set; } = name;

	public PhoneType() : this(0, "") { }
}
