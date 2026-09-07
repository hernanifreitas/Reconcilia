using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CodeReason.Reports.Interfaces;

namespace CodeReason.Reports.Barcode;

public class BarcodeBase : Canvas, IPropertyValue, IHasValue
{
	public static readonly DependencyProperty AggregateGroupProperty = DependencyProperty.Register("AggregateGroup", typeof(string), typeof(BarcodeBase), new UIPropertyMetadata(null));

	public static readonly DependencyProperty BrushBarsProperty = DependencyProperty.Register("BrushBars", typeof(Brush), typeof(BarcodeBase), new UIPropertyMetadata(Brushes.Black));

	public static readonly DependencyProperty FontFamilyProperty = DependencyProperty.Register("FontFamily", typeof(FontFamily), typeof(BarcodeBase), new UIPropertyMetadata(new FontFamily()));

	public static readonly DependencyProperty FontStretchProperty = DependencyProperty.Register("FontStretch", typeof(FontStretch), typeof(BarcodeBase), new UIPropertyMetadata(default(FontStretch)));

	public static readonly DependencyProperty FontStyleProperty = DependencyProperty.Register("FontStyle", typeof(FontStyle), typeof(BarcodeBase), new UIPropertyMetadata(default(FontStyle)));

	public static readonly DependencyProperty FontWeightProperty = DependencyProperty.Register("FontWeight", typeof(FontWeight), typeof(BarcodeBase), new UIPropertyMetadata(default(FontWeight)));

	public static readonly DependencyProperty FormatProperty = DependencyProperty.Register("Format", typeof(string), typeof(BarcodeBase), new UIPropertyMetadata(null));

	public static readonly DependencyProperty PropertyNameProperty = DependencyProperty.Register("PropertyName", typeof(string), typeof(BarcodeBase), new UIPropertyMetadata(null));

	public static readonly DependencyProperty ShowTextProperty = DependencyProperty.Register("ShowText", typeof(bool), typeof(BarcodeBase), new UIPropertyMetadata(true));

	public static readonly DependencyProperty TextProperty = DependencyProperty.Register("Text", typeof(string), typeof(BarcodeBase), new UIPropertyMetadata(null));

	public static readonly DependencyProperty ValueProperty = DependencyProperty.Register("Value", typeof(object), typeof(BarcodeBase), new UIPropertyMetadata(null));

	public static readonly RoutedEvent ValueChangedEvent = EventManager.RegisterRoutedEvent("ValueChanged", RoutingStrategy.Bubble, typeof(RoutedPropertyChangedEventHandler<decimal>), typeof(BarcodeBase));

	public virtual string AggregateGroup
	{
		get
		{
			return (string)GetValue(AggregateGroupProperty);
		}
		set
		{
			SetValue(AggregateGroupProperty, value);
		}
	}

	public virtual Brush BrushBars
	{
		get
		{
			return (Brush)GetValue(BrushBarsProperty);
		}
		set
		{
			SetValue(BrushBarsProperty, value);
			RedrawAll();
		}
	}

	public virtual FontFamily FontFamily
	{
		get
		{
			return (FontFamily)GetValue(FontFamilyProperty);
		}
		set
		{
			SetValue(FontFamilyProperty, value);
			RedrawAll();
		}
	}

	public virtual FontStretch FontStretch
	{
		get
		{
			return (FontStretch)GetValue(FontStretchProperty);
		}
		set
		{
			SetValue(FontStretchProperty, value);
		}
	}

	public virtual FontStyle FontStyle
	{
		get
		{
			return (FontStyle)GetValue(FontStyleProperty);
		}
		set
		{
			SetValue(FontStyleProperty, value);
			RedrawAll();
		}
	}

	public virtual FontWeight FontWeight
	{
		get
		{
			return (FontWeight)GetValue(FontWeightProperty);
		}
		set
		{
			SetValue(FontWeightProperty, value);
			RedrawAll();
		}
	}

	public virtual string Format
	{
		get
		{
			return (string)GetValue(FormatProperty);
		}
		set
		{
			SetValue(FormatProperty, value);
			RedrawAll();
		}
	}

	public virtual string PropertyName
	{
		get
		{
			return (string)GetValue(PropertyNameProperty);
		}
		set
		{
			SetValue(PropertyNameProperty, value);
			RedrawAll();
		}
	}

	public virtual bool ShowText
	{
		get
		{
			return (bool)GetValue(ShowTextProperty);
		}
		set
		{
			SetValue(ShowTextProperty, value);
			RedrawAll();
		}
	}

	public virtual string Text
	{
		get
		{
			return (string)GetValue(TextProperty);
		}
		set
		{
			SetValue(TextProperty, value);
			RedrawAll();
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
			RedrawAll();
		}
	}

	protected virtual void OnValueChanged(RoutedPropertyChangedEventArgs<decimal> args)
	{
		RaiseEvent(args);
	}

	protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
	{
		base.OnRenderSizeChanged(sizeInfo);
		RedrawAll();
	}

	public virtual void RedrawAll()
	{
		base.Children.Clear();
	}
}
