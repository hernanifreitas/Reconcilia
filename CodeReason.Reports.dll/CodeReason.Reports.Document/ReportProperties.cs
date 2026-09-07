using System.Windows;
using System.Windows.Documents;

namespace CodeReason.Reports.Document;

public class ReportProperties : Section
{
	public static readonly DependencyProperty ReportNameProperty = DependencyProperty.Register("ReportName", typeof(string), typeof(ReportProperties), new UIPropertyMetadata(null));

	public static readonly DependencyProperty ReportTitleProperty = DependencyProperty.Register("ReportTitle", typeof(string), typeof(ReportProperties), new UIPropertyMetadata(null));

	public string ReportName
	{
		get
		{
			return (string)GetValue(ReportNameProperty);
		}
		set
		{
			SetValue(ReportNameProperty, value);
		}
	}

	public string ReportTitle
	{
		get
		{
			return (string)GetValue(ReportTitleProperty);
		}
		set
		{
			SetValue(ReportTitleProperty, value);
		}
	}
}
