using System.Collections;
using System.Data;
using PhoneShop.DAL.Interfaces;
using MySql.Data.MySqlClient;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.DAO;

public class PhoneTypeDAO : IModelDAO<PhoneType>
{
	private DataTable _phoneTypes;

	private readonly MySqlConnection _connection;

	public PhoneTypeDAO(MySqlConnection connection)
	{
		_connection = connection;
		_phoneTypes = Select();
	}
	public DataTable Select()
	{
		MySqlDataAdapter adapter = new("SELECT * FROM phone_types", _connection);
		_phoneTypes = new();

		_connection.Open();
		adapter.Fill(_phoneTypes);
		_connection.Close();
		return _phoneTypes;
	}

	public void Delete(int id)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			DeleteCommand = new("DELETE FROM phone_types " +
								"WHERE PhoneTypeId = @PhoneTypeId", _connection)
		};
		adapter.DeleteCommand.Parameters.AddWithValue("@PhoneTypeId", id);
		adapter.DeleteCommand.ExecuteNonQuery();
		adapter.Update(_phoneTypes);
		_phoneTypes.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Update(int id, PhoneType newPhoneType)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			UpdateCommand = new("UPDATE phone_types " +
								"SET (Name = @Name) " +
								"WHERE PhoneTypeId = @PhoneTypeId", _connection)
		};
		adapter.UpdateCommand.Parameters.AddWithValue("@PhoneTypeId", id);
		adapter.UpdateCommand.Parameters.AddWithValue("@Name", newPhoneType.Name);

		adapter.Update(_phoneTypes);
		_phoneTypes.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Insert(PhoneType phoneType)
	{
		_connection.Open();
		DataRow row = _phoneTypes.NewRow();
		row["Name"] = phoneType.Name;

		_phoneTypes.Rows.Add(row);

		MySqlDataAdapter adapter = new()
		{
			InsertCommand = new("INSERT INTO phone_types (Name) " +
								"VALUES (@Name, _connection)")
		};

		adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 100, "Name");

		adapter.Update(_phoneTypes);
		_phoneTypes.AcceptChanges();
		_connection.Close();
		Select();
	}


	public IEnumerator<PhoneType> GetEnumerator()
	{
		var phoneTypes = from p in _phoneTypes.AsEnumerable()
						select new
						{
							Id = p.Field<int>("PhoneTypeId"),
							Name = p.Field<string>("Name"),
						};

		foreach (var phone in phoneTypes)
		{
			ArgumentNullException.ThrowIfNull(phone.Name);
			yield return new PhoneType
			{
				Id = phone.Id,
				Name = phone.Name,
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
