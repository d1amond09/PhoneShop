using System.Xml;
using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.DAL.Repositories;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.Models.Access;
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.BLL.Implementations;

public class SaleService(IRepository<Sale> saleRepository) : ISaleService
{
	private readonly IRepository<Sale> _saleRepository = saleRepository;

	public void Add(Sale model)
	{
		_saleRepository.Create(model);
	}

	public IEnumerable<SaleViewModel> GetAll()
	{
		List<SaleViewModel> viewModels = [];

		IEnumerable<Sale> sales = _saleRepository.ReadAll();
		foreach(Sale sale in sales)
		{
			viewModels.Add(new(sale));
		}
		return viewModels;
	}

	public IEnumerable<SaleViewModel> Get(string name)
		=> GetAll().Where(x => x.ProductName.Contains(name));

	public SaleViewModel Get(int id)
	{
		SaleViewModel saleVM = new(_saleRepository.Read(id));
		return saleVM;
	}

	IEnumerable<Sale> IGetModelService<Sale>.GetAll()
		=> _saleRepository.ReadAll();

	public void Remove(Sale sale)
	{
		_saleRepository.Delete(sale);
	}

	IEnumerable<Sale> IGetModelService<Sale>.Get(string name)
		=> _saleRepository.ReadAll().Where(x => x.Product.Name.Contains(name));


	public IEnumerable<string> AllEnableProducers(DateTime begin, DateTime end)
	{
		return _saleRepository.ReadAll()
							  .Where(s => s.Date >= begin && s.Date <= end)
							  .Select(s => s.Product.Phone.Producer.Name).Distinct();
	}

	public IEnumerable<string> AllEnableTypePhones(DateTime begin, DateTime end)
	{
		return _saleRepository.ReadAll()
							  .Where(s => s.Date >= begin && s.Date <= end)
							  .Select(s => s.Product.Phone.Type.Name).Distinct();
	}

	public int CountByProducer(string name)
	{
		return _saleRepository.ReadAll().Where(x => x.Product.Phone.Producer.Name == name).Count();
	}

	public int CountByType(string name)
	{
		return _saleRepository.ReadAll().Where(x => x.Product.Phone.Type.Name == name).Count();
	}

	public double ProfitByType(string name)
	{
		return _saleRepository.ReadAll().Where(x => x.Product.Phone.Type.Name == name).Sum(x => x.Price) -
			   _saleRepository.ReadAll().Where(x => x.Product.Phone.Type.Name == name).Sum(x => x.Product.Price);
	}

	public void SaveXmlReportProducersCountSales(string filePath, DateTime begin, DateTime end)
	{
		XmlDocument doc = new();

		XmlElement reportElement = doc.CreateElement("Отчет");
		doc.AppendChild(reportElement);

		foreach (string producer in AllEnableProducers(begin, end))
		{
			XmlElement producerElement = doc.CreateElement("Производители");
			reportElement.AppendChild(producerElement);

			XmlElement nameElement = doc.CreateElement("Имя-производителя");
			nameElement.InnerText = producer;
			producerElement.AppendChild(nameElement);

			XmlElement priceElement = doc.CreateElement("Количествово-проданных-товаров");
			priceElement.InnerText = $"{CountByProducer(producer)}";
			producerElement.AppendChild(priceElement);
		}
		doc.Save(filePath);
	}

	public void SaveXmlReportTypePhoneCountSalesProfit(string filePath, DateTime begin, DateTime end)
	{
		XmlDocument doc = new();

		XmlElement reportElement = doc.CreateElement("Отчет");
		doc.AppendChild(reportElement);

		foreach (string type in AllEnableTypePhones(begin, end))
		{
			XmlElement typeElement = doc.CreateElement("Виды");
			reportElement.AppendChild(typeElement);

			XmlElement nameElement = doc.CreateElement("Вид");
			nameElement.InnerText = type;
			typeElement.AppendChild(nameElement);

			XmlElement countElement = doc.CreateElement("Количествово-проданных-товаров");
			countElement.InnerText = $"{CountByType(type)}";
			typeElement.AppendChild(countElement);

			XmlElement profitElement = doc.CreateElement("Прибыль");
			profitElement.InnerText = $"{ProfitByType(type):F2}";
			typeElement.AppendChild(profitElement);
		}
		doc.Save(filePath);
	}
}
