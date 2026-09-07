using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;

namespace CodeReason.Reports.Barcode;

public class BarcodeC128 : BarcodeBase
{
	public enum Code128SubType
	{
		A,
		B,
		BC,
		C
	}

	private static char[] _keysA;

	private static char[] _keysB;

	private static string[] _keysC;

	protected string[] _codeTable = new string[107]
	{
		"101111", "111011", "111110", "010112", "010211", "020111", "011102", "011201", "021101", "110102",
		"110201", "120101", "001121", "011021", "011120", "002111", "012011", "012110", "112100", "110021",
		"110120", "102101", "112001", "201020", "200111", "210011", "210110", "201101", "211001", "211100",
		"101012", "101210", "121010", "000212", "020012", "020210", "001202", "021002", "021200", "100202",
		"120002", "120200", "001022", "001220", "021020", "002012", "002210", "022010", "202010", "100220",
		"120020", "102002", "102200", "102020", "200012", "200210", "220010", "201002", "201200", "221000",
		"203000", "110300", "320000", "000113", "000311", "010013", "010310", "030011", "030110", "001103",
		"001301", "011003", "011300", "031001", "031100", "130100", "110003", "302000", "130001", "023000",
		"000131", "010031", "010130", "003101", "013001", "013100", "300101", "310001", "310100", "101030",
		"103010", "301010", "000032", "000230", "020030", "003002", "003200", "300002", "300200", "002030",
		"003020", "200030", "300020", "100301", "100103", "100121", "1220001"
	};

	private Code128SubType _barcodeSubType = Code128SubType.BC;

	public Code128SubType BarcodeSubType
	{
		get
		{
			return _barcodeSubType;
		}
		set
		{
			_barcodeSubType = value;
			RedrawAll();
		}
	}

	private static void GenerateKeysA()
	{
		for (int i = 0; i < 64; i++)
		{
			_keysA[i] = (char)(32 + i);
		}
		for (int i = 64; i < 96; i++)
		{
			_keysA[i] = (char)i;
		}
		for (int i = 96; i < _keysA.Length; i++)
		{
			_keysA[i] = '0';
		}
	}

	private static void GenerateKeysB()
	{
		for (int i = 0; i < 96; i++)
		{
			_keysB[i] = (char)(32 + i);
		}
		for (int i = 96; i < _keysB.Length; i++)
		{
			_keysB[i] = '0';
		}
	}

	private static void GenerateKeysC()
	{
		for (int i = 0; i < 100; i++)
		{
			_keysC[i] = $"{i:00}";
		}
		for (int i = 100; i < _keysC.Length; i++)
		{
			_keysC[i] = "";
		}
	}

	static BarcodeC128()
	{
		_keysA = new char[107];
		_keysB = new char[107];
		_keysC = new string[107];
		GenerateKeysA();
		GenerateKeysB();
		GenerateKeysC();
	}

	public BarcodeC128()
	{
		RedrawAll();
	}

	private static int IndexOfCharArray(char[] array, char ch)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == ch)
			{
				return i;
			}
		}
		return -1;
	}

	private static int IndexOfStringArray(string[] array, string str)
	{
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == str)
			{
				return i;
			}
		}
		return -1;
	}

	private double DrawCharByIndex(Canvas canvas, double left, double top, double height, int charIndex)
	{
		string text = _codeTable[charIndex];
		double num = 0.0;
		for (int i = 0; i < text.Length; i++)
		{
			double num2 = 0.0;
			switch (text[i])
			{
			case '0':
				num2 = 1.0;
				break;
			case '1':
				num2 = 2.0;
				break;
			case '2':
				num2 = 3.0;
				break;
			case '3':
				num2 = 4.0;
				break;
			}
			if (num2 < 1.0)
			{
				throw new ArgumentOutOfRangeException();
			}
			double num3 = num2;
			if (canvas != null && i % 2 == 0)
			{
				Rectangle rectangle = new Rectangle();
				rectangle.Fill = BrushBars;
				rectangle.RenderTransform = new TranslateTransform(left + num, top);
				rectangle.Width = num3;
				rectangle.Height = height;
				canvas.Children.Add(rectangle);
			}
			num += num3;
		}
		return num;
	}

	private List<int> GenerateCodeSequence(string code, out List<BarcodeCharInfo> charInfo)
	{
		List<int> list = new List<int>();
		charInfo = new List<BarcodeCharInfo>();
		bool flag = false;
		if (code == null)
		{
			code = "";
		}
		switch (_barcodeSubType)
		{
		case Code128SubType.A:
			list.Add(103);
			break;
		case Code128SubType.B:
			list.Add(104);
			break;
		case Code128SubType.BC:
			if (code.Length >= 2 && char.IsDigit(code[0]) && char.IsDigit(code[1]))
			{
				list.Add(105);
				break;
			}
			flag = true;
			list.Add(104);
			break;
		case Code128SubType.C:
			list.Add(105);
			break;
		}
		string text = "";
		for (int i = 0; i < code.Length; i++)
		{
			int num = -1;
			switch (_barcodeSubType)
			{
			case Code128SubType.A:
				num = IndexOfCharArray(_keysA, code[i]);
				charInfo.Add(new BarcodeCharInfo(i, -1.0, code[i].ToString()));
				break;
			case Code128SubType.B:
				num = IndexOfCharArray(_keysB, code[i]);
				charInfo.Add(new BarcodeCharInfo(i, -1.0, code[i].ToString()));
				break;
			case Code128SubType.BC:
				if (flag)
				{
					if (i <= code.Length - 2 && char.IsDigit(code[i]) && char.IsDigit(code[i + 1]))
					{
						flag = false;
						list.Add(99);
						charInfo.Add(new BarcodeCharInfo(i, -1.0, ""));
						text += code[i];
						if (text.Length != 2)
						{
							continue;
						}
						num = IndexOfStringArray(_keysC, text);
						charInfo.Add(new BarcodeCharInfo(i, -1.0, text));
						text = "";
					}
					else
					{
						num = IndexOfCharArray(_keysB, code[i]);
						charInfo.Add(new BarcodeCharInfo(i, -1.0, code[i].ToString()));
					}
				}
				else if (text.Length <= 0 && (i >= code.Length - 1 || !char.IsDigit(code[i]) || !char.IsDigit(code[i + 1])))
				{
					flag = true;
					list.Add(100);
					charInfo.Add(new BarcodeCharInfo(i, -1.0, ""));
					num = IndexOfCharArray(_keysB, code[i]);
					charInfo.Add(new BarcodeCharInfo(i, -1.0, code[i].ToString()));
				}
				else
				{
					text += code[i];
					if (text.Length != 2)
					{
						continue;
					}
					num = IndexOfStringArray(_keysC, text);
					charInfo.Add(new BarcodeCharInfo(i, -1.0, text));
					text = "";
				}
				break;
			case Code128SubType.C:
				text += code[i];
				if (text.Length == 2)
				{
					num = IndexOfStringArray(_keysC, text);
					charInfo.Add(new BarcodeCharInfo(i, -1.0, text));
					text = "";
					break;
				}
				continue;
			}
			if (num < 0)
			{
				throw new ArgumentOutOfRangeException("code", "The barcode value contains an unsupported character \"" + code[i] + "\"");
			}
			list.Add(num);
		}
		if (text.Length > 0)
		{
			throw new ArgumentException("Code 128C only supports a even number of characters", "code");
		}
		return list;
	}

	public override void RedrawAll()
	{
		base.Children.Clear();
		double num = base.ActualWidth;
		double num2 = base.ActualHeight;
		if (double.IsNaN(num) || num <= 0.0)
		{
			num = base.Width;
		}
		if (double.IsNaN(num2) || num2 <= 0.0)
		{
			num2 = base.Height;
		}
		if (double.IsNaN(num) || num <= 0.0 || double.IsNaN(num2) || num2 <= 0.0)
		{
			return;
		}
		Rect rect = new Rect(0.0, 0.0, num, num2);
		if (Value == null)
		{
			return;
		}
		string text = InlineHasValue.FormatValue(Value, Format);
		if (string.IsNullOrEmpty(text))
		{
			return;
		}
		List<int> list = GenerateCodeSequence(text, out var _);
		if (list.Count <= 0)
		{
			return;
		}
		if (Text != null && !string.IsNullOrEmpty(Text.ToString()))
		{
			text = Text.ToString();
		}
		int num3 = 0;
		double num4 = 0.0;
		double num5 = 0.0;
		for (int i = 0; i < list.Count; i++)
		{
			double num6 = DrawCharByIndex(null, num4, 0.0, 1.0, list[i]);
			if (i == 0)
			{
				num5 = num6;
			}
			num4 += num6;
			num3 = ((i != 0) ? (num3 + list[i] * i) : list[0]);
		}
		double num7 = num4 - num5;
		num4 += DrawCharByIndex(null, num4, 0.0, 1.0, num3 % 103);
		num4 += DrawCharByIndex(null, num4, 0.0, 1.0, 106);
		double num8 = num4;
		Canvas canvas = new Canvas();
		Label label = null;
		double num9 = 0.0;
		decimal num10 = 1m;
		TransformGroup transformGroup = null;
		if (ShowText)
		{
			label = new Label();
			label.Content = text;
			label.Padding = new Thickness(0.25);
			label.FontFamily = FontFamily;
			label.FontStretch = FontStretch;
			label.FontStyle = FontStyle;
			label.FontWeight = FontWeight;
			label.FontSize = 1.0;
			label.Measure(new Size(num, num2));
			num10 = (decimal)rect.Width / (decimal)num8 * (decimal)num7 / (decimal)label.DesiredSize.Width;
			num9 = label.DesiredSize.Height * (double)num10;
			transformGroup = new TransformGroup();
			transformGroup.Children.Add(new ScaleTransform((double)num10, (double)num10));
			label.RenderTransform = transformGroup;
		}
		num4 = 0.0;
		for (int i = 0; i < list.Count; i++)
		{
			double height = 1.0;
			if (ShowText && i > 0)
			{
				height = (rect.Height - (double)num10 * label.DesiredSize.Height / 2.0) / rect.Height;
			}
			double num6 = DrawCharByIndex(canvas, num4, 0.0, height, list[i]);
			num4 += num6;
		}
		num4 += DrawCharByIndex(canvas, num4, 0.0, 1.0, num3 % 103);
		num4 += DrawCharByIndex(canvas, num4, 0.0, 1.0, 106);
		if (ShowText)
		{
			transformGroup.Children.Add(new TranslateTransform(num5 * rect.Width / num4, rect.Height - num9));
			base.Children.Add(label);
		}
		TransformGroup transformGroup2 = new TransformGroup();
		transformGroup2.Children.Add(new ScaleTransform(rect.Width / num4, rect.Height - num9 / 2.0));
		transformGroup2.Children.Add(new TranslateTransform(rect.Left, rect.Top));
		canvas.RenderTransform = transformGroup2;
		base.Children.Add(canvas);
	}
}
