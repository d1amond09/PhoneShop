using System.Xml.Linq;
using PhoneShop.Domain.Models;

namespace PhoneShop.Domain.ViewModels;

public class PhoneViewModel(Phone phone)
{
	public int Id { get; set; } = phone.Id;
	public string Name { get; set; } = phone.Name;
	public string Producer { get; set; } = phone.Producer.Name;
	public string Type { get; set; } = phone.Type.Name;
}
