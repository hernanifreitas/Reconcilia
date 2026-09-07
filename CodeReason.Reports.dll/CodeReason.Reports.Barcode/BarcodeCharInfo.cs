namespace CodeReason.Reports.Barcode;

public class BarcodeCharInfo
{
	private int _index = -1;

	private double _left = -1.0;

	private string _text = "";

	public int Index
	{
		get
		{
			return _index;
		}
		set
		{
			_index = value;
		}
	}

	public double Left
	{
		get
		{
			return _left;
		}
		set
		{
			_left = value;
		}
	}

	public string Text
	{
		get
		{
			return _text;
		}
		set
		{
			_text = value;
		}
	}

	public BarcodeCharInfo(int index, double left, string text)
	{
		_index = index;
		_left = left;
		_text = text;
	}
}
