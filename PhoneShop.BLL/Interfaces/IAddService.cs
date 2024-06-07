using PhoneShop.Domain.Interfaces;
using PhoneShop.Domain.Models;

namespace PhoneShop.BLL.Interfaces;

public interface IAddService<in TEntity> where TEntity : IModel
{
	public void Add(TEntity model);
}
