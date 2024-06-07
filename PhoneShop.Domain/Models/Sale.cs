using PhoneShop.Domain.Interfaces;

namespace PhoneShop.Domain.Models;

public class Sale(int id, Product product, Customer customer, DateTime date) : IModel
{
	public int Id { get; set; } = id;
	public Product Product { get; set; } = product;
	public Customer Customer { get; set; } = customer;
	public DateTime Date { get; set; } = date;

	public double Price { get; set; }

	public Sale() : this(0, new(), new(), new()) { }
}
