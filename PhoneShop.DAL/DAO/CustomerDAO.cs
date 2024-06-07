using System.Collections;
using System.Data;
using PhoneShop.DAL.Interfaces;
using MySql.Data.MySqlClient;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.DAO;

public class CustomerDAO : IModelDAO<Customer>
{
	private DataTable _customers;

	private readonly MySqlConnection _connection;

	public CustomerDAO(MySqlConnection connection)
	{
		_connection = connection;
		_customers = Select();
	}

	public DataTable Select()
	{
		MySqlDataAdapter adapter = new("SELECT * FROM customers", _connection);
		_customers = new();

		_connection.Open();
		adapter.Fill(_customers);
		_connection.Close();
		return _customers;
	}

	public void Delete(int id)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			DeleteCommand = new("DELETE FROM customers " +
								"WHERE CustomerId = @CustomerId", _connection)
		};
		adapter.DeleteCommand.Parameters.AddWithValue("@CustomerId", id);
		adapter.DeleteCommand.ExecuteNonQuery();
		adapter.Update(_customers);
		_customers.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Update(int id, Customer newCustomer)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			UpdateCommand = new("UPDATE customers " +
								"SET (`Name` = @Name, Surname = @Surname, Midname = @Midname) " +
								"WHERE CustomerId = @CustomerId", _connection)
		};
		adapter.UpdateCommand.Parameters.AddWithValue("@CustomerId", id);
		adapter.UpdateCommand.Parameters.AddWithValue("@Name", newCustomer.Name);
		adapter.UpdateCommand.Parameters.AddWithValue("@Surname", newCustomer.Surname);
		adapter.UpdateCommand.Parameters.AddWithValue("@Midname", newCustomer.Midname);

		adapter.Update(_customers);
		_customers.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Insert(Customer customer)
	{
		_connection.Open();
		DataRow row = _customers.NewRow();
		row["CustomerId"] = customer.Id;
		row["Name"] = customer.Name;
		row["Surname"] = customer.Surname;
		row["Midname"] = customer.Midname;

		_customers.Rows.Add(row);

		MySqlDataAdapter adapter = new()
		{
			InsertCommand = new("INSERT INTO customers (`Name`, Surname, Midname) " +
								"VALUES (@Name, @Surname, @Midname)", _connection)
		};

		adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 100, "Name");
		adapter.InsertCommand.Parameters.Add("@Surname", MySqlDbType.VarChar, 100, "Surname");
		adapter.InsertCommand.Parameters.Add("@Midname", MySqlDbType.VarChar, 100, "Midname");

		adapter.Update(_customers);
		_customers.AcceptChanges();
		_connection.Close();
		Select();
	}


	public IEnumerator<Customer> GetEnumerator()
	{
		var customers = from p in _customers.AsEnumerable()
						select new
						{
							Id = p.Field<int>("CustomerId"),
							Name = p.Field<string>("Name"),
							Surname = p.Field<string>("Surname"),
							Midname = p.Field<string>("Midname"),
						};

		foreach (var Customer in customers)
		{
			ArgumentNullException.ThrowIfNull(Customer.Name);
			yield return new Customer
			{
				Id = Customer.Id,
				Name = Customer.Name,
				Surname = Customer.Surname,
				Midname = Customer.Midname,
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
