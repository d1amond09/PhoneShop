using System.Diagnostics.Metrics;
using PhoneShop.Domain.Interfaces;

namespace PhoneShop.Domain.Models;

public class Customer(int id, string surname, string name, string midname) : IModel
{
	public int Id { get; set; } = id;
	public string Surname { get; set; } = surname;
	public string Name { get; set; } = name;
	public string Midname { get; set; } = midname;

    public Customer() : this(0, "", "", "") { }

	public static bool operator ==(Customer customer1, Customer customer2)
	{
		if(customer1.Surname == customer2.Surname &&
		   customer1.Name == customer2.Name && 
		   customer1.Midname == customer2.Midname)
			return true;
		return false;
	}

	public static bool operator !=(Customer customer1, Customer customer2)
	{
		return !(customer1 == customer2);
	}

	public override bool Equals(object? obj)
	{
		if (ReferenceEquals(this, obj))
		{
			return true;
		}

		if (obj is null)
		{
			return false;
		}
		return false;
	}

	public override int GetHashCode()
	{
		throw new NotImplementedException();
	}
}
