using PhoneShop.Domain.Models;

namespace PhoneShop.Domain.ViewModels;

public class SaleViewModel(Sale sale)
{
	public int Id { get; set; } = sale.Id;
	public string ProductName { get; set; } = sale.Product.Name;
	public string CustomerFullName { get; set; } = $"{sale.Customer.Surname} {sale.Customer.Name} {sale.Customer.Midname}";
	public string Price { get; set; } = $"{sale.Price:F2}";
	public string Date { get; set; } = sale.Date.ToShortDateString();

	public SaleViewModel() : this(new()) { }

}
