using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using PhoneShop.BLL.Implementations;
using PhoneShop.BLL.Interfaces;

namespace PhoneShop.Pages;

public partial class ReportPage : Page
{
	private const string folderPath = @"..\..\..\data\reports\";
	private readonly IXmlReport _service;
	private readonly string _fullName;

	public ReportPage(IXmlReport service, string fileName)
	{
		_fullName = folderPath + fileName;
		_service = service;
		_service.SaveXmlReportRemains(folderPath + @"products.xml");
		_service.SaveXmlReportPriceProducers(folderPath + @"producers.xml");
		InitializeComponent();
		DataSet dataSet = new();
		dataSet.ReadXml(_fullName);
		dataReport.ItemsSource = dataSet.Tables[0].DefaultView;
	}

}
