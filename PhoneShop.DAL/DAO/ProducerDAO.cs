using System.Collections;
using System.Data;
using PhoneShop.DAL.Interfaces;
using MySql.Data.MySqlClient;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.DAO;

public class ProducerDAO : IModelDAO<Producer>
{
	private DataTable _producers;

	private readonly MySqlConnection _connection;

	public ProducerDAO(MySqlConnection connection)
	{
		_connection = connection;
		_producers = Select();
	}

	public DataTable Select()
	{
		MySqlDataAdapter adapter = new("SELECT * FROM producers", _connection);
		_producers = new();

		_connection.Open();
		adapter.Fill(_producers);
		_connection.Close();
		return _producers;
	}

	public void Delete(int id)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			DeleteCommand = new("DELETE FROM producers " +
								"WHERE ProductId = @ProductId", _connection)
		};
		adapter.DeleteCommand.Parameters.AddWithValue("@ProductId", id);
		adapter.DeleteCommand.ExecuteNonQuery();
		adapter.Update(_producers);
		_producers.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Update(int id, Producer newProducer)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			UpdateCommand = new("UPDATE producers " +
								"SET (Name = @Name) " +
								"WHERE ProducerId = @ProducerId", _connection)
		};
		adapter.UpdateCommand.Parameters.AddWithValue("@ProducerId", id);
		adapter.UpdateCommand.Parameters.AddWithValue("@Name", newProducer.Name);

		adapter.Update(_producers);
		_producers.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Insert(Producer producer)
	{
		_connection.Open();
		DataRow row = _producers.NewRow();
		row["ProducerId"] = producer.Name;
		row["Name"] = producer.Name;
		row["ProducerId"] = producer.Id;

		_producers.Rows.Add(row);

		MySqlDataAdapter adapter = new()
		{
			InsertCommand = new("INSERT INTO producers (Name) " +
								"VALUES (@Name, _connection)")
		};
		
		adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 100, "Name");

		adapter.Update(_producers);
		_producers.AcceptChanges();
		_connection.Close();
		Select();
	}

	public IEnumerator<Producer> GetEnumerator()
	{
		var producers = from p in _producers.AsEnumerable()
						select new
						{
							Id = p.Field<int>("ProducerId"),
							Name = p.Field<string>("Name"),
						};

		foreach (var producer in producers)
		{
			ArgumentNullException.ThrowIfNull(producer.Name);
			yield return new Producer
			{
				Id = producer.Id,
				Name = producer.Name,
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
