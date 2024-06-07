using System.Collections;
using System.Data;
using PhoneShop.DAL.Interfaces;
using MySql.Data.MySqlClient;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.DAO;

public class RoleDAO : IModelDAO<Role>
{
	private DataTable _roles;

	private readonly MySqlConnection _connection;

	public RoleDAO(MySqlConnection connection)
	{
		_connection = connection;
		_roles = Select();
	}

	public DataTable Select()
	{
		MySqlDataAdapter adapter = new("SELECT * FROM roles", _connection);
		_roles = new();

		_connection.Open();
		adapter.Fill(_roles);
		_connection.Close();
		return _roles;
	}

	public void Delete(int id)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			DeleteCommand = new("DELETE FROM roles " +
								"WHERE RoleId = @RoleId", _connection)
		};
		adapter.DeleteCommand.Parameters.AddWithValue("@RoleId", id);
		adapter.DeleteCommand.ExecuteNonQuery();
		adapter.Update(_roles);
		_roles.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Update(int id, Role newRole)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			UpdateCommand = new("UPDATE roles " +
								"SET (Name = @Name)" +
								"WHERE RoleId = @RoleId", _connection)
		};
		adapter.UpdateCommand.Parameters.AddWithValue("@RoleId", id);
		adapter.UpdateCommand.Parameters.AddWithValue("@Name", newRole.Name);

		adapter.Update(_roles);
		_roles.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Insert(Role role)
	{
		_connection.Open();
		DataRow row = _roles.NewRow();
		row["Name"] = role.Name;

		_roles.Rows.Add(row);

		MySqlDataAdapter adapter = new()
		{
			InsertCommand = new("INSERT INTO roles (Name) " +
											"VALUES (@Name)", _connection)
		};
		adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 100, "Name");


		adapter.Update(_roles);
		_roles.AcceptChanges();
		_connection.Close();
		Select();
	}

	public IEnumerator<Role> GetEnumerator()
	{
		var roles = from r in _roles.AsEnumerable()
						select new
						{
							Id = r.Field<int>("RoleId"),
							Name = r.Field<string>("Name"),
						};

		foreach (var role in roles)
		{
			ArgumentNullException.ThrowIfNull(role.Name);
			yield return new Role
			{
				Id = role.Id,
				Name = role.Name,
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
