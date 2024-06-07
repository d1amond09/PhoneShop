using System.Collections;
using System.Data;
using MySql.Data.MySqlClient;
using PhoneShop.DAL.Interfaces;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;

namespace PhoneShop.DAL.DAO;

public class CategoryDAO : IModelDAO<Category>
{
	private DataTable _categories;

	private readonly MySqlConnection _connection;

	public CategoryDAO(MySqlConnection connection)
	{
		_connection = connection;
		_categories = Select();
	}

	public DataTable Select()
	{
		MySqlDataAdapter adapter = new("SELECT * FROM categories", _connection);
		_categories = new();

		_connection.Open();
		adapter.Fill(_categories);
		_connection.Close();
		return _categories;
	}

	public void Delete(int id)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			DeleteCommand = new("DELETE FROM categories " +
								"WHERE CategoryId = @CategoryId", _connection)
		};
		adapter.DeleteCommand.Parameters.AddWithValue("@CategoryId", id);
		adapter.DeleteCommand.ExecuteNonQuery();
		adapter.Update(_categories);
		_categories.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Update(int id, Category newCategory)
	{
		_connection.Open();
		MySqlDataAdapter adapter = new()
		{
			UpdateCommand = new("UPDATE phones " +
								"SET (Name = @Name, Markup = @Markup) " +
								"WHERE CategoryId = @CategoryId", _connection)
		};
		adapter.UpdateCommand.Parameters.AddWithValue("@CustomerId", id);
		adapter.UpdateCommand.Parameters.AddWithValue("@Name", newCategory.Name);
		adapter.UpdateCommand.Parameters.AddWithValue("@Markup", newCategory.Markup);

		adapter.Update(_categories);
		_categories.AcceptChanges();
		_connection.Close();
		Select();
	}

	public void Insert(Category phone)
	{
		_connection.Open();
		DataRow row = _categories.NewRow();
		row["Name"] = phone.Name;
		row["Markup"] = phone.Markup;

		_categories.Rows.Add(row);

		MySqlDataAdapter adapter = new()
		{
			InsertCommand = new("INSERT INTO phones (Name, Markup) " +
								"VALUES (@Name, @Markup)", _connection)
		};

		adapter.InsertCommand.Parameters.Add("@Name", MySqlDbType.VarChar, 100, "Name");
		adapter.InsertCommand.Parameters.Add("@Markup", MySqlDbType.Int32, 4, "Markup");

		adapter.Update(_categories);
		_categories.AcceptChanges();
		_connection.Close();
		Select();
	}

	public IEnumerator<Category> GetEnumerator()
	{
		var categories = from p in _categories.AsEnumerable()
						select new
						{
							Id = p.Field<int>("CategoryId"),
							Name = p.Field<string>("Name"),
							Markup = p.Field<int>("Markup"),
						};

		foreach (var category in categories)
		{
			ArgumentNullException.ThrowIfNull(category.Name);
			yield return new Category
			{
				Id = category.Id,
				Name = category.Name,
				Markup = category.Markup,
			};
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
		return GetEnumerator();
	}
}
