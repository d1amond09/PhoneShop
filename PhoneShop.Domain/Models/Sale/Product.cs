using PhoneShop.Domain.Interfaces;

namespace PhoneShop.Domain.Models;

public class Product(int id, string name, Phone phone, Category category, double price) : IModel
{
	public int Id { get; set; } = id;
	public string Name { get; set; } = name;
	public Phone Phone { get; set; } = phone;
	public Category Category { get; set; } = category;
	public double Price { get; set; } = price;

	public Product() : this(0, "", new(), new(), 0) { }
}
