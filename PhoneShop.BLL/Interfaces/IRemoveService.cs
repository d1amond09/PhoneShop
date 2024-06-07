namespace PhoneShop.BLL.Interfaces;

public interface IRemoveService<in TEntity>
{
	public void Remove(TEntity model);
}
