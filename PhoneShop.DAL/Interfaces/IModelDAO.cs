using System.Data;
using PhoneShop.Domain.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.DAL.Interfaces;

public interface IModelDAO<TEntity> : IEnumerable<TEntity> where TEntity : IModel
{
	public DataTable Select();
	public void Insert(TEntity phone);
	public void Delete(int id);
	public void Update(int id, TEntity newPhoneType);
}
