using System.Windows;
using System.Windows.Documents;

namespace CodeReason.Reports.Document;

public class SectionDataGroup : Section
{
	public static readonly DependencyProperty DataGroupProperty = DependencyProperty.Register("DataGroupName", typeof(string), typeof(SectionDataGroup), new UIPropertyMetadata(""));

	public string DataGroupName
	{
		get
		{
			return (string)GetValue(DataGroupProperty);
		}
		set
		{
			SetValue(DataGroupProperty, value);
		}
	}
}
