using PhoneShop.Domain.Interfaces;

namespace PhoneShop.DAL.Interfaces;

public interface IRepository<TModel> where TModel : IModel
{
	public IEnumerable<TModel> ReadAll();
	public TModel Read(int id);
	public bool Create(TModel model);
	public bool Update(TModel oldModel, TModel newModel);
	public bool Delete(TModel model);
}
