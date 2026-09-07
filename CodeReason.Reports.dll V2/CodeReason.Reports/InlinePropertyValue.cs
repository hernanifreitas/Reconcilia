using System.Windows;
using CodeReason.Reports.Interfaces;

namespace CodeReason.Reports;

public abstract class InlinePropertyValue : InlineHasValue, IPropertyValue, IHasValue
{
	public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(InlinePropertyValue), new UIPropertyMetadata(null));

	public virtual string PropertyName
	{
		get
		{
			return (string)GetValue(PropertyNameProperty);
		}
		set
		{
			SetValue(PropertyNameProperty, value);
		}
	}
}
