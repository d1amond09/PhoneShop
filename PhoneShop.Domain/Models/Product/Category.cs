using PhoneShop.Domain.Interfaces;

namespace PhoneShop.Domain.Models;

public class Category(int id, string name, int markup) : IModel
{
	public int Id { get; set; } = id;
	public string Name { get; set; } = name;
	public int Markup { get; set; } = markup;

	public Category() : this(0, "", 0) { }
}
