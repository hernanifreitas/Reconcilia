using System.Windows;
using System.Windows.Documents;

namespace CodeReason.Reports.Document;

public class SectionReportFooter : Section
{
	public static readonly DependencyProperty PageFooterHeightProperty = DependencyProperty.Register("PageFooterHeight", typeof(double), typeof(ReportProperties), new UIPropertyMetadata(2.0));

	public double PageFooterHeight
	{
		get
		{
			return (double)GetValue(PageFooterHeightProperty);
		}
		set
		{
			SetValue(PageFooterHeightProperty, value);
		}
	}
}
