namespace PhoneShop.BLL.Interfaces;

public interface IXmlDateReport
{
	public void SaveXmlReportProducersCountSales(string filePath, DateTime begin, DateTime end);
	public void SaveXmlReportTypePhoneCountSalesProfit(string filePath, DateTime begin, DateTime end);
}
