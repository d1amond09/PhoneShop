using PhoneShop.Domain.Interfaces;

namespace PhoneShop.Domain.Models;

public class Phone(int id, string name, Producer producer, PhoneType type) : IModel
{
	public int Id { get; set; } = id;
	public string Name { get; set; } = name;
	public Producer Producer { get; set; } = producer;
	public PhoneType Type { get; set; } = type;

    public Phone() : this(0, "", new(), new()) {  }
}
