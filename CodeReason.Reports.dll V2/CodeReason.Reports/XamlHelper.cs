using System.IO;
using System.Windows;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Xml;

namespace CodeReason.Reports;

public static class XamlHelper
{
	public static object LoadXamlFromString(string s)
	{
		if (string.IsNullOrEmpty(s))
		{
			return null;
		}
		StringReader input = new StringReader(s);
		XmlReader reader = XmlReader.Create(input, new XmlReaderSettings());
		return XamlReader.Load(reader);
	}

	public static TableRow CloneTableRow(TableRow orig)
	{
		if (orig == null)
		{
			return null;
		}
		string s = XamlWriter.Save(orig);
		return (TableRow)LoadXamlFromString(s);
	}

	public static Block CloneBlock(Block orig)
	{
		if (orig == null)
		{
			return null;
		}
		string s = XamlWriter.Save(orig);
		return (Block)LoadXamlFromString(s);
	}

	public static UIElement CloneUIElement(UIElement orig)
	{
		if (orig == null)
		{
			return null;
		}
		string s = XamlWriter.Save(orig);
		return (UIElement)LoadXamlFromString(s);
	}

	public static void SaveImageBmp(Visual visual, Stream stream, int width, int height, double dpiX, double dpiY)
	{
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)((double)width * dpiX / 96.0), (int)((double)height * dpiY / 96.0), dpiX, dpiY, PixelFormats.Pbgra32);
		renderTargetBitmap.Render(visual);
		BmpBitmapEncoder bmpBitmapEncoder = new BmpBitmapEncoder();
		bmpBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
		bmpBitmapEncoder.Save(stream);
	}

	public static void SaveImagePng(Visual visual, Stream stream, int width, int height, double dpiX, double dpiY)
	{
		RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)((double)width * dpiX / 96.0), (int)((double)height * dpiY / 96.0), dpiX, dpiY, PixelFormats.Pbgra32);
		renderTargetBitmap.Render(visual);
		PngBitmapEncoder pngBitmapEncoder = new PngBitmapEncoder();
		pngBitmapEncoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
		pngBitmapEncoder.Save(stream);
	}
}
