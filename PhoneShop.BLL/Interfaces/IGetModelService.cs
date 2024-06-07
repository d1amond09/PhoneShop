using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PhoneShop.Domain.Interfaces;

namespace PhoneShop.BLL.Interfaces;

public interface IGetModelService<out TEntity> where TEntity : IModel
{
	public IEnumerable<TEntity> GetAll();
	public IEnumerable<TEntity> Get(string name);
}
