using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using MySql.Data.MySqlClient;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;
using PhoneShop.DAL.Interfaces;

namespace PhoneShop.DAL.DAO;

public class UserDAO : IModelDAO<User>
{
	private DataTable _users;
	private readonly IEnumerable<Role> _roles;
	private readonly MySqlConnection _connection;

	public UserDAO(MySqlConnection connection, IEnumerable<Role> roles)
	{
		_connection = connection;
		_users = Select();
		_roles = roles;
	}

	public DataTable Select()
	{
		MySqlDataAdapter adapter = new("SELECT * FROM users", _connection);
		_users = new();

		_connection.Open();
		adapter.Fill(_users);
		_connection.Close();
		return _users;
	}

	public void Delete(int  id)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			DeleteCommand = new("DELETE FROM users " +
								"WHERE UserId = @UserId", _connection)
		};
		adapter.DeleteCommand.Parameters.AddWithValue("@UserId", id);
		adapter.DeleteCommand.ExecuteNonQuery();
		adapter.Update(_users);
		_users.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Update(int id, User newUser)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			UpdateCommand = new("UPDATE users " +
								"SET (RoleId = @RoleId, Login = @Login, Password = @Password) " +
								"WHERE UserId = @UserId", _connection)
		};
		adapter.UpdateCommand.Parameters.AddWithValue("@UserId", id);
		adapter.UpdateCommand.Parameters.AddWithValue("@RoleId", newUser.Role.Id);
		adapter.UpdateCommand.Parameters.AddWithValue("@Login", newUser.Login);
		adapter.UpdateCommand.Parameters.AddWithValue("@Password", newUser.Password);

		adapter.Update(_users);
		_users.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Insert(User user)
	{
		_connection.Open();
		DataRow row = _users.NewRow();

		row["UserId"] = user.Id;
		row["RoleId"] = user.Role.Id;
		row["Login"] = user.Login;
		row["Password"] = user.Password;

		_users.Rows.Add(row);

		MySqlDataAdapter adapter = new()
		{
			InsertCommand = new("INSERT INTO users (RoleId, Login, Password) " +
								"VALUES (@RoleId, @Login, @Password)", _connection)
		};

		adapter.InsertCommand.Parameters.Add("@RoleId", MySqlDbType.Int32, 4, "RoleId");
		adapter.InsertCommand.Parameters.Add("@Login", MySqlDbType.VarChar, 100, "Login");
		adapter.InsertCommand.Parameters.Add("@Password", MySqlDbType.VarChar, 100, "Password");

		adapter.Update(_users);
		_users.AcceptChanges();
		_connection.Close();
		Select();
	}

	public IEnumerator<User> GetEnumerator()
	{
		var users = from u in _users.AsEnumerable()
						select new
						{
							Id = u.Field<int>("UserId"),
							RoleId = u.Field<int>("RoleId"),
							Login = u.Field<string>("Login"),
							Password = u.Field<string>("Password"),
						};

		foreach (var user in users)
		{
			ArgumentNullException.ThrowIfNull(user.Login);
			ArgumentNullException.ThrowIfNull(user.Password);
			yield return new User
			{
				Id = user.Id,
				Role = _roles.First(x => x.Id == user.RoleId),
				Login = user.Login,
				Password = user.Password,
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
