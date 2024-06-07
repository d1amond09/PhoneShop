
using PhoneShop.Domain.Models;
using System.Diagnostics;
using System.Xml.Linq;

namespace PhoneShop.Domain.ViewModels;

public class ProductViewModel(Product product)
{
	public int Id { get; set; } = product.Id;
	public string Name { get; set; } = product.Name;
	public string Phone { get; set; } = product.Phone.Name;
	public string Category { get; set; } = product.Category.Name;
	public double Price { get; set; } = product.Price;
}
