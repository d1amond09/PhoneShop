using System.Xml;
using MySql.Data.MySqlClient;
using PhoneShop.BLL.Interfaces;
using PhoneShop.DAL.Interfaces;
using PhoneShop.DAL.Repositories;
using PhoneShop.Domain.Models;
using PhoneShop.Domain.ViewModels;

namespace PhoneShop.BLL.Implementations;

public class ProductService(IRepository<Product> productRepository) : IProductService
{
	private readonly IRepository<Product> _productRepository = productRepository;

	public void Add(Product model)
	{
		try
		{
			_productRepository.Create(model);

		}
		catch (Exception ex)
		{
			throw new Exception("", ex);
		}
	}

	public ProductViewModel Get(int id)
	{
		ProductViewModel productVM = new(_productRepository.Read(id));
		return productVM;
	}

	public IEnumerable<ProductViewModel> GetAll()
	{
		List<ProductViewModel> viewModels = [];

		IEnumerable<Product> products = _productRepository.ReadAll();
		foreach(Product Product in products)
		{
			viewModels.Add(new(Product));
		}
		return viewModels;
	}

	public IEnumerable<ProductViewModel> Get(string name)
		=> GetAll().Where(x => x.Name.Contains(name));

	IEnumerable<Product> IGetModelService<Product>.GetAll()
		=> _productRepository.ReadAll();

	public void Remove(Product model)
	{
		try
		{
			_productRepository.Delete(model);
		}
		catch (MySqlException)
		{
			throw new Exception("Данный продукт невозможно удалить, так как он используется в продажах. \n" +
								"Для начала удалите все продажи этого товара.");
		}
		catch (Exception ex)
		{
			throw new Exception("", ex);
		}
	}

	IEnumerable<Product> IGetModelService<Product>.Get(string name)
		=> _productRepository.ReadAll().Where(x => x.Name.Contains(name));

	public IEnumerable<Producer> AllEnableProducers()
	{
		return _productRepository.ReadAll().Select(p => p.Phone.Producer).Distinct();
	}

	public double AvgPriceBy(Producer producer)
	{
		return _productRepository.ReadAll().Where(p => p.Phone.Producer.Name == producer.Name).Average(x => x.Price);
	}

	public void SaveXmlReportRemains(string filePath)
	{
		XmlDocument doc = new();

		XmlElement reportElement = doc.CreateElement("Отчет");
		doc.AppendChild(reportElement);

		foreach (Product product in _productRepository.ReadAll())
		{
			XmlElement productElement = doc.CreateElement("Товар");
			reportElement.AppendChild(productElement);

			XmlElement nameElement = doc.CreateElement("Наименование");
			nameElement.InnerText = product.Name;
			productElement.AppendChild(nameElement);

			XmlElement phoneNameElement = doc.CreateElement("Название");
			phoneNameElement.InnerText = product.Phone.Name;
			productElement.AppendChild(phoneNameElement);

			XmlElement producerElement = doc.CreateElement("Производитель");
			producerElement.InnerText = product.Phone.Producer.Name;
			productElement.AppendChild(producerElement);

			XmlElement phoneTypeElement = doc.CreateElement("Вид");
			phoneTypeElement.InnerText = product.Phone.Type.Name;
			productElement.AppendChild(phoneTypeElement);

			XmlElement categoryElement = doc.CreateElement("Категория");
			categoryElement.InnerText = product.Category.Name;
			productElement.AppendChild(categoryElement);

			XmlElement markupElement = doc.CreateElement("Наценка");
			markupElement.InnerText = $"{product.Category.Markup}";
			productElement.AppendChild(markupElement);

			XmlElement priceElement = doc.CreateElement("Себестоимость");
			priceElement.InnerText = $"{product.Price}";
			productElement.AppendChild(priceElement);
		}
		doc.Save(filePath);
	}

	public void SaveXmlReportPriceProducers(string filePath)
	{
		XmlDocument doc = new();

		XmlElement reportElement = doc.CreateElement("Отчет");
		doc.AppendChild(reportElement);

		foreach (Producer producer in AllEnableProducers())
		{
			XmlElement producerElement = doc.CreateElement("Производитель");
			reportElement.AppendChild(producerElement);

			XmlElement nameElement = doc.CreateElement("Имя-производителя");
			nameElement.InnerText = producer.Name;
			producerElement.AppendChild(nameElement);

			XmlElement priceElement = doc.CreateElement("Средняя-себестоимость-товаров");
			priceElement.InnerText = $"{AvgPriceBy(producer)}";
			producerElement.AppendChild(priceElement);
		}
		doc.Save(filePath);
	}
}
