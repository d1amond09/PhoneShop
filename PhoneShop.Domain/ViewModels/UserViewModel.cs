using PhoneShop.Domain.Models.Access;

namespace PhoneShop.Domain.ViewModels;

public class UserViewModel(User user)
{
	public int Id { get; set; } = user.Id;
	public string Role { get; set; } = user.Role.Name;
	public string Login { get; set; } = user.Login;
	public string Password { get; set; } = user.Password;
}
