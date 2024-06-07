using System.Collections;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using MySql.Data.MySqlClient;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;
using PhoneShop.DAL.Interfaces;

namespace PhoneShop.DAL.DAO;

public class SaleDAO : IModelDAO<Sale>
{
	private DataTable _sales;

	private readonly MySqlConnection _connection;
	private readonly IEnumerable<Customer> _customers;
	private readonly IEnumerable<Product> _products;
	public SaleDAO(MySqlConnection connection, IEnumerable<Customer> customers, IEnumerable<Product> products)
	{
		_connection = connection;
		_customers = customers;
		_products = products;
		_sales = Select();
	}

	public DataTable Select()
	{
		MySqlDataAdapter adapter = new("SELECT * FROM sales", _connection);
		_sales = new();

		_connection.Open();
		adapter.Fill(_sales);
		_connection.Close();
		return _sales;
	}

	public void Delete(int id)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			DeleteCommand = new("DELETE FROM sales " +
								"WHERE SaleId = @SaleId", _connection)
		};
		adapter.DeleteCommand.Parameters.AddWithValue("@SaleId", id);

		adapter.DeleteCommand.ExecuteNonQuery();
		adapter.Update(_sales);
		_sales.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Update(int id, Sale newSale)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			UpdateCommand = new("UPDATE sales " +
								"SET (ProductId = @ProductId, CustomerId = @CustomerId, SaleDate = @SaleDate) " +
								"WHERE SaleId = @SaleId", _connection)
		};
		adapter.UpdateCommand.Parameters.AddWithValue("@SaleId", id);
		adapter.UpdateCommand.Parameters.AddWithValue("@ProductId", newSale.Product.Id);
		adapter.UpdateCommand.Parameters.AddWithValue("@CustomerId", newSale.Customer.Id);
		adapter.UpdateCommand.Parameters.AddWithValue("@SaleDate", Convert.ToDateTime(newSale.Date));

		adapter.Update(_sales);
		_sales.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Insert(Sale sale)
	{
		_connection.Open();
		DataRow row = _sales.NewRow();
		row["SaleId"] = sale.Id;
		row["ProductId"] = sale.Product.Id;
		row["CustomerId"] = sale.Customer.Id;
		row["SaleDate"] = sale.Date;

		_sales.Rows.Add(row);

		MySqlDataAdapter adapter = new()
		{
			InsertCommand = new("INSERT INTO sales (ProductId, CustomerId, SaleDate) " +
											"VALUES (@ProductId, @CustomerId, @SaleDate)", _connection)
		};

		adapter.InsertCommand.Parameters.Add("@ProductId", MySqlDbType.Int32, 4, "ProductId");
		adapter.InsertCommand.Parameters.Add("@CustomerId", MySqlDbType.Int32, 100, "CustomerId");
		adapter.InsertCommand.Parameters.Add("@SaleDate", MySqlDbType.Date, 3, "SaleDate");

		adapter.Update(_sales);
		_sales.AcceptChanges();
		_connection.Close();
		Select();
	}

	public IEnumerator<Sale> GetEnumerator()
	{
		var sales = from p in _sales.AsEnumerable()
						select new
						{
							Id = p.Field<int>("SaleId"),
							ProductId = p.Field<int>("ProductId"),
							CustomerId = p.Field<int>("CustomerId"),
							Date = p.Field<DateTime>("SaleDate"),
						};

		foreach (var sale in sales)
		{
			Product product = _products.First(x => x.Id == sale.ProductId);
			Customer customer = _customers.First(x => x.Id == sale.CustomerId);
			yield return new Sale
			{
				Id = sale.Id,
				Date = sale.Date,
				Product = product,
				Customer = customer,
				Price = product.Price + product.Category.Markup * 0.01 * product.Price
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
