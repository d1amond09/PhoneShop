using PhoneShop.Domain.Interfaces;

namespace PhoneShop.Domain.Models.Access;

public class User(int id, Role role, string login, string password) : IModel
{
	public int Id { get; set; } = id;
	public Role Role { get; set; } = role;
	public string Login { get; set; } = login;
	public string Password { get; set; } = password;

	public User() : this(0, new(), "", "") { }
}
