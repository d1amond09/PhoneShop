using PhoneShop.Domain.Models;
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.BLL.Interfaces;

public interface ISaleService : IService<Sale, SaleViewModel>, IXmlDateReport
{

}
