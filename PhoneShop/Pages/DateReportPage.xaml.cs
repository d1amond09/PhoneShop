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
using PhoneShop.BLL.Interfaces;

namespace PhoneShop.Pages;

public partial class DateReportPage : Page
{
	private const string folderPath = @"..\..\..\data\reports\";
	private readonly IXmlDateReport _service;
	private readonly string _fullName;
	public DateReportPage(IXmlDateReport service, string fileName)
	{
		_fullName = folderPath + fileName;
		InitializeComponent();
		_service = service;
		DataSet dataSet = new();
		_service.SaveXmlReportProducersCountSales(folderPath + @"producers_count_sales.xml", DateTime.MinValue, DateTime.MaxValue);
		_service.SaveXmlReportTypePhoneCountSalesProfit(folderPath + @"type_phones_count_sales_profit.xml", DateTime.MinValue, DateTime.MaxValue);
		dataSet.ReadXml(_fullName);
		dataReport.ItemsSource = dataSet.Tables[0].DefaultView;
	}

	private void UpdateButton_Click(object sender, RoutedEventArgs e)
	{
		if (dateBegin.SelectedDate is DateTime begin &&
		    dateEnd.SelectedDate is DateTime end)
		{
			_service.SaveXmlReportProducersCountSales(folderPath + @"producers_count_sales.xml", begin, end);
			_service.SaveXmlReportTypePhoneCountSalesProfit(folderPath + @"type_phones_count_sales_profit.xml", begin, end);
			DataSet dataSet = new();
			dataSet.ReadXml(_fullName);
			dataReport.ItemsSource = dataSet.Tables[0].DefaultView;
		}
	}
}
