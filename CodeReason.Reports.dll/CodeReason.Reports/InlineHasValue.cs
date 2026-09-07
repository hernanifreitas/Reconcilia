using System;
using System.Windows;
using System.Windows.Documents;
using CodeReason.Reports.Interfaces;

namespace CodeReason.Reports;

public abstract class InlineHasValue : Run, IHasValue
{
	public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(string), typeof(InlineHasValue), new UIPropertyMetadata(null));

	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(InlineHasValue), new UIPropertyMetadata(null));

	public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(InlineHasValue));

	public virtual string Format
	{
		get
		{
			return (string)GetValue(FormatProperty);
		}
		set
		{
			SetValue(FormatProperty, value);
		}
	}

	public virtual object Value
	{
		get
		{
			return GetValue(ValueProperty);
		}
		set
		{
			SetValue(ValueProperty, value);
			base.Text = FormatValue(value, Format);
		}
	}

	protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
	{
		RaiseEvent(args);
	}

	public static string FormatValue(object value, string format)
	{
		if (value == null)
		{
			return "";
		}
		if (string.IsNullOrEmpty(format))
		{
			return value.ToString();
		}
		Type type = value.GetType();
		if (type == typeof(DateTime))
		{
			return ((DateTime)value).ToString(format);
		}
		if (type == typeof(decimal))
		{
			return ((decimal)value).ToString(format);
		}
		if (type == typeof(double))
		{
			return ((double)value).ToString(format);
		}
		if (type == typeof(float))
		{
			return ((float)value).ToString(format);
		}
		if (type == typeof(int))
		{
			return ((int)value).ToString(format);
		}
		if (type == typeof(long))
		{
			return ((long)value).ToString(format);
		}
		if (type == typeof(short))
		{
			return ((short)value).ToString(format);
		}
		if (type == typeof(uint))
		{
			return ((uint)value).ToString(format);
		}
		if (type == typeof(ulong))
		{
			return ((ulong)value).ToString(format);
		}
		if (type == typeof(ushort))
		{
			return ((ushort)value).ToString(format);
		}
		return value.ToString();
	}
}
