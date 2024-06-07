using PhoneShop.Domain.Models;
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.BLL.Interfaces;

public interface IProductService : IService<Product, ProductViewModel>, IXmlReport
{
}
