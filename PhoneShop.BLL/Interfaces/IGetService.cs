using PhoneShop.Domain.ViewModels;

namespace PhoneShop.BLL.Interfaces;

public interface IGetService<out TEntity>
{
	public TEntity Get(int id);
	public IEnumerable<TEntity> Get(string name);
	public IEnumerable<TEntity> GetAll();
}
