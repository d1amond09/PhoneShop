using System.Collections;
using System.Data;
using PhoneShop.DAL.Interfaces;
using MySql.Data.MySqlClient;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.DAO;

public class PhoneDAO : IModelDAO<Phone>
{
	private DataTable _phones;

	private readonly MySqlConnection _connection;

	private readonly IEnumerable<PhoneType> _phoneTypes;
	private readonly IEnumerable<Producer> _producers;


	public PhoneDAO(MySqlConnection connection, IEnumerable<PhoneType> phoneTypes, IEnumerable<Producer> producers)
	{
		_connection = connection;
		_phoneTypes = phoneTypes;
		_producers = producers;
		_phones = Select();
	}

	public DataTable Select()
	{
		MySqlDataAdapter adapter = new("SELECT * FROM phones", _connection);
		_phones = new();

		_connection.Open();
		adapter.Fill(_phones);
		_connection.Close();
		return _phones;
	}

	public void Delete(int id)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			DeleteCommand = new("DELETE FROM phones " +
								"WHERE PhoneId = @PhoneId", _connection)
		};
		adapter.DeleteCommand.Parameters.AddWithValue("@PhoneId", id);
		adapter.DeleteCommand.ExecuteNonQuery();
		adapter.Update(_phones);
		_phones.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Update(int id, Phone newPhone)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			UpdateCommand = new("UPDATE phones " +
								"SET (`Name` = @Name, ProducerId = @ProducerId, PhoneTypeId = @PhoneTypeId) " +
								"WHERE PhoneId = @PhoneId", _connection)
		};
		adapter.UpdateCommand.Parameters.AddWithValue("@PhoneId", id);
		adapter.UpdateCommand.Parameters.AddWithValue("@Name", newPhone.Name);
		adapter.UpdateCommand.Parameters.AddWithValue("@ProducerId", newPhone.Producer.Id);
		adapter.UpdateCommand.Parameters.AddWithValue("@PhoneTypeId", newPhone.Type.Id);

		adapter.Update(_phones);
		_phones.AcceptChanges();
		_connection.Close();
	}

	public void Insert(Phone phone)
	{
		_connection.Open();
		DataRow row = _phones.NewRow();
		row["PhoneId"] = phone.Id;
		row["Name"] = phone.Name;
		row["ProducerId"] = phone.Producer.Id;
		row["PhoneTypeId"] = phone.Type.Id;

		_phones.Rows.Add(row);

		MySqlDataAdapter adapter = new()
		{
			InsertCommand = new("INSERT INTO phones (`Name`, ProducerId, PhoneTypeId) " +
								"VALUES (@Name, @ProducerId, @PhoneTypeId)", _connection)
		};

		adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 100, "Name");
		adapter.InsertCommand.Parameters.Add("@ProducerId", MySqlDbType.Int32, 4, "ProducerId");
		adapter.InsertCommand.Parameters.Add("@PhoneTypeId", MySqlDbType.Int32, 4, "PhoneTypeId");

		adapter.Update(_phones);
		_phones.AcceptChanges();
		_connection.Close();
	}

	public IEnumerator<Phone> GetEnumerator()
	{
		var phones = from p in _phones.AsEnumerable()
						select new
						{
							Id = p.Field<int>("PhoneId"),
							Name = p.Field<string>("Name"),
							PhoneTypeId = p.Field<int>("PhoneTypeId"),
							ProducerId = p.Field<int>("ProducerId"),
						};

		foreach (var phone in phones)
		{
			ArgumentNullException.ThrowIfNull(phone.Name);
			yield return new Phone
			{
				Id = phone.Id,
				Name = phone.Name,
				Producer = _producers.First(x => x.Id == phone.ProducerId),
				Type = _phoneTypes.First(x => x.Id == phone.PhoneTypeId)
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
