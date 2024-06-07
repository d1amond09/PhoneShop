using System.Collections;
using System.Data;
using PhoneShop.DAL.Interfaces;
using MySql.Data.MySqlClient;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.DAO;

public class SoldProductDAO : IModelDAO<Product>
{
	private DataTable _products;

	private readonly MySqlConnection _connection;

	private readonly IEnumerable<Phone> _phones;
	private readonly IEnumerable<Category> _categories;

	public SoldProductDAO(MySqlConnection connection, IEnumerable<Phone> phones, IEnumerable<Category> categories)
	{
		_connection = connection;
		_phones = phones;
		_categories = categories;
		_products = Select();
	}

	public DataTable Select()
	{
		MySqlDataAdapter adapter = new("SELECT * FROM sold_products", _connection);
		_products = new();

		_connection.Open();
		adapter.Fill(_products);
		_connection.Close();
		return _products;
	}


	public void Delete(int id)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			DeleteCommand = new("DELETE FROM sold_products " +
								"WHERE ProductId = @ProductId", _connection)
		};
		adapter.DeleteCommand.Parameters.AddWithValue("@ProductId", id);
		adapter.DeleteCommand.ExecuteNonQuery();
		adapter.Update(_products);
		_products.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Update(int id, Product newProduct)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			UpdateCommand = new("UPDATE sold_products " +
								"SET (`Name` = @Name, CategoryId = @CategoryId, PhoneId = @PhoneId, Price = @Price) " +
								"WHERE ProductId = @ProductId", _connection)
		};
		adapter.UpdateCommand.Parameters.AddWithValue("@ProductId", id);
		adapter.UpdateCommand.Parameters.AddWithValue("@Name", newProduct.Name);
		adapter.UpdateCommand.Parameters.AddWithValue("@CategoryId", newProduct.Category.Id);
		adapter.UpdateCommand.Parameters.AddWithValue("@PhoneId", newProduct.Phone.Id);
		adapter.UpdateCommand.Parameters.AddWithValue("@Price", newProduct.Price);

		adapter.Update(_products);
		_products.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Insert(Product product)
	{
		_connection.Open();
		DataRow row = _products.NewRow();
		row["ProductId"] = product.Id;
		row["Name"] = product.Name;
		row["CategoryId"] = product.Category.Id;
		row["PhoneId"] = product.Phone.Id;
		row["Price"] = product.Price;

		_products.Rows.Add(row);

		MySqlDataAdapter adapter = new()
		{
			InsertCommand = new("INSERT INTO sold_products (ProductId, `Name`, CategoryId, PhoneId, Price) " +
											"VALUES (@ProductId, @Name, @CategoryId, @PhoneId, @Price)", _connection)
		};
		adapter.InsertCommand.Parameters.Add("@ProductId", MySqlDbType.Int32, 4, "ProductId");
		adapter.InsertCommand.Parameters.Add("@CategoryId", MySqlDbType.Int32, 4, "CategoryId");
		adapter.InsertCommand.Parameters.Add("@PhoneId", MySqlDbType.Int32, 4, "PhoneId");
		adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 100, "Name");
		adapter.InsertCommand.Parameters.Add("@Price", MySqlDbType.Decimal, 8, "Price");

		adapter.Update(_products);
		_products.AcceptChanges();
		_connection.Close();
		Select();
	}

	public IEnumerator<Product> GetEnumerator()
	{
		var categories = from p in _products.AsEnumerable()
						select new
						{
							Id = p.Field<int>("ProductId"),
							Name = p.Field<string>("Name"),
							Price = p.Field<decimal>("Price"),
							CategoryId = p.Field<int>("CategoryId"),
							PhoneId = p.Field<int>("PhoneId"),
							
						};

		foreach (var product in categories)
		{
			ArgumentNullException.ThrowIfNull(product.Name);
			yield return new Product
			{
				Id = product.Id,
				Name = product.Name,
				Price = (double) product.Price,
				Category = _categories.First(x => x.Id == product.CategoryId),
				Phone = _phones.First(x => x.Id == product.PhoneId),
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
