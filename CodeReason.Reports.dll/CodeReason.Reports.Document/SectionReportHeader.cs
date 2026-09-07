using System.Windows;
using System.Windows.Documents;

namespace CodeReason.Reports.Document;

public class SectionReportHeader : Section
{
	public static readonly DependencyProperty PageHeaderHeightProperty = DependencyProperty.Register("PageHeaderHeight", typeof(double), typeof(ReportProperties), new UIPropertyMetadata(2.0));

	public double PageHeaderHeight
	{
		get
		{
			return (double)GetValue(PageHeaderHeightProperty);
		}
		set
		{
			SetValue(PageHeaderHeightProperty, value);
		}
	}
}
