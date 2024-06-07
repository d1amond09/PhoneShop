using PhoneShop.Domain.Interfaces;

namespace PhoneShop.BLL.Interfaces;

public interface IService<TEntity, TEntityViewModel>
			   : IGetService<TEntityViewModel>,
				 IService<TEntity>
				 where TEntity : IModel
{

}

public interface IService<TEntity>
			   : IAddService<TEntity>,
			     IRemoveService<TEntity>,
				 IGetModelService<TEntity> where TEntity : IModel
{
}
