using PhoneShop.Domain.Interfaces;

namespace PhoneShop.Domain.Models.Access;

public class Role(int id, string name) : IModel
{
	public int Id { get; set; } = id;
	public string Name { get; set; } = name;

	public Role() : this(0, "") { }
}
