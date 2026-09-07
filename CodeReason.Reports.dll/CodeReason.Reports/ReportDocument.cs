using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Packaging;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media.Imaging;
using System.Windows.Xps.Packaging;
using System.Windows.Xps.Serialization;
using CodeReason.Reports.Document;

namespace CodeReason.Reports;

public class ReportDocument
{
	private double _pageHeaderHeight = 0.0;

	private double _pageFooterHeight = 0.0;

	private double _pageHeight = double.NaN;

	private double _pageWidth = double.NaN;

	private string _reportName = "";

	private string _reportTitle = "";

	private string _xamlImagePath = "";

	private string _xamlData = "";

	private CompressionOption _xpsCompressionOption = CompressionOption.NotCompressed;

	public double PageHeaderHeight
	{
		get
		{
			return _pageHeaderHeight;
		}
		set
		{
			_pageHeaderHeight = value;
		}
	}

	public double PageFooterHeight
	{
		get
		{
			return _pageFooterHeight;
		}
		set
		{
			_pageFooterHeight = value;
		}
	}

	public double PageHeight => _pageHeight;

	public double PageWidth => _pageWidth;

	public string ReportName
	{
		get
		{
			return _reportName;
		}
		set
		{
			_reportName = value;
		}
	}

	public string ReportTitle
	{
		get
		{
			return _reportTitle;
		}
		set
		{
			_reportTitle = value;
		}
	}

	public string XamlImagePath
	{
		get
		{
			return _xamlImagePath;
		}
		set
		{
			_xamlImagePath = value;
		}
	}

	public string XamlData
	{
		get
		{
			return _xamlData;
		}
		set
		{
			_xamlData = value;
		}
	}

	public CompressionOption XpsCompressionOption
	{
		get
		{
			return _xpsCompressionOption;
		}
		set
		{
			_xpsCompressionOption = value;
		}
	}

	public event EventHandler<DataRowBoundEventArgs> DataRowBound = null;

	public event GetPageCompletedEventHandler GetPageCompleted = null;

	public event EventHandler<ImageErrorEventArgs> ImageError = null;

	public event EventHandler<ImageEventArgs> ImageProcessing = null;

	public event EventHandler<ImageEventArgs> ImageProcessed = null;

	public void FireEventGetPageCompleted(GetPageCompletedEventArgs ea)
	{
		if (GetPageCompleted != null)
		{
			GetPageCompleted(this, ea);
		}
	}

	public void FireEventDataRowBoundEventArgs(DataRowBoundEventArgs ea)
	{
		if (DataRowBound != null)
		{
			DataRowBound(this, ea);
		}
	}

	public FlowDocument CreateFlowDocument()
	{
		MemoryStream memoryStream = new MemoryStream();
		byte[] bytes = Encoding.UTF8.GetBytes(_xamlData);
		memoryStream.Write(bytes, 0, bytes.Length);
		memoryStream.Position = 0L;
		FlowDocument flowDocument = XamlReader.Load(memoryStream) as FlowDocument;
		if (flowDocument.PageHeight == double.NaN)
		{
			throw new ArgumentException("Flow document must have a specified page height");
		}
		if (flowDocument.PageWidth == double.NaN)
		{
			throw new ArgumentException("Flow document must have a specified page width");
		}
		_pageHeight = flowDocument.PageHeight;
		_pageWidth = flowDocument.PageWidth;
		DocumentWalker documentWalker = new DocumentWalker();
		List<SectionReportHeader> list = documentWalker.Walk<SectionReportHeader>(flowDocument);
		List<SectionReportFooter> list2 = documentWalker.Walk<SectionReportFooter>(flowDocument);
		List<ReportProperties> list3 = documentWalker.Walk<ReportProperties>(flowDocument);
		if (list3.Count > 0)
		{
			if (list3.Count > 1)
			{
				throw new ArgumentException($"Flow document must have only one ReportProperties section, but it has {list3.Count}");
			}
			ReportProperties reportProperties = list3[0];
			if (reportProperties.ReportName != null)
			{
				ReportName = reportProperties.ReportName;
			}
			if (reportProperties.ReportTitle != null)
			{
				ReportTitle = reportProperties.ReportTitle;
			}
			if (list.Count > 0)
			{
				PageHeaderHeight = list[0].PageHeaderHeight;
			}
			if (list2.Count > 0)
			{
				PageFooterHeight = list2[0].PageFooterHeight;
			}
			DependencyObject dependencyObject = reportProperties.Parent;
			if (dependencyObject is FlowDocument)
			{
				((FlowDocument)dependencyObject).Blocks.Remove(reportProperties);
				dependencyObject = null;
			}
			if (dependencyObject is Section)
			{
				((Section)dependencyObject).Blocks.Remove(reportProperties);
				dependencyObject = null;
			}
		}
		flowDocument.PageHeight = _pageHeight - _pageHeight * (PageHeaderHeight + PageFooterHeight) / 100.0;
		List<Image> list4 = (List<Image>)(documentWalker.Tag = new List<Image>());
		documentWalker.VisualVisited += walker_VisualVisited;
		documentWalker.Walk(flowDocument);
		foreach (Image item in list4)
		{
			if (ImageProcessing != null)
			{
				ImageProcessing(this, new ImageEventArgs(this, item));
			}
			try
			{
				if (item.Tag is string)
				{
					item.Source = new BitmapImage(new Uri("file:///" + Path.Combine(_xamlImagePath, item.Tag.ToString())));
				}
			}
			catch (Exception exception)
			{
				if (ImageError == null)
				{
					throw;
				}
				bool flag = false;
				lock (ImageError)
				{
					ImageErrorEventArgs e = new ImageErrorEventArgs(exception, this, item);
					Delegate[] invocationList = ImageError.GetInvocationList();
					foreach (Delegate obj in invocationList)
					{
						obj.DynamicInvoke(this, e);
						if (e.Handled)
						{
							flag = true;
							break;
						}
					}
				}
				if (!flag)
				{
					throw;
				}
			}
			if (ImageProcessed != null)
			{
				ImageProcessed(this, new ImageEventArgs(this, item));
			}
		}
		return flowDocument;
	}

	private void walker_VisualVisited(object sender, object visitedObject, bool start)
	{
		if (visitedObject is Image && sender is DocumentWalker { Tag: List<Image> tag })
		{
			tag.Add((Image)visitedObject);
		}
	}

	public XpsDocument CreateXpsDocument(ReportData data)
	{
		MemoryStream stream = new MemoryStream();
		Package package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite);
		string text = "pack://report.xps";
		PackageStore.RemovePackage(new Uri(text));
		PackageStore.AddPackage(new Uri(text), package);
		XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.NotCompressed, text);
		XpsSerializationManager xpsSerializationManager = new XpsSerializationManager(new XpsPackagingPolicy(xpsDocument), batchMode: false);
		DocumentPaginator documentPaginator = ((IDocumentPaginatorSource)CreateFlowDocument()).DocumentPaginator;
		ReportPaginator serializedObject = new ReportPaginator(this, data);
		xpsSerializationManager.SaveAsXaml(serializedObject);
		return xpsDocument;
	}

	public XpsDocument CreateXpsDocument(IEnumerable<ReportData> data)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		int num = 0;
		ReportData reportData = null;
		foreach (ReportData datum in data)
		{
			if (reportData == null)
			{
				reportData = datum;
			}
			num++;
		}
		if (num == 1)
		{
			return CreateXpsDocument(reportData);
		}
		MemoryStream stream = new MemoryStream();
		Package package = Package.Open(stream, FileMode.Create, FileAccess.ReadWrite);
		string text = "pack://report.xps";
		PackageStore.RemovePackage(new Uri(text));
		PackageStore.AddPackage(new Uri(text), package);
		XpsDocument xpsDocument = new XpsDocument(package, CompressionOption.NotCompressed, text);
		XpsSerializationManager xpsSerializationManager = new XpsSerializationManager(new XpsPackagingPolicy(xpsDocument), batchMode: false);
		DocumentPaginator documentPaginator = ((IDocumentPaginatorSource)CreateFlowDocument()).DocumentPaginator;
		MultipleReportPaginator serializedObject = new MultipleReportPaginator(this, data);
		xpsSerializationManager.SaveAsXaml(serializedObject);
		return xpsDocument;
	}

	public XpsDocument CreateXpsDocument(ReportData data, string fileName)
	{
		Package package = Package.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
		string text = "pack://report.xps";
		PackageStore.RemovePackage(new Uri(text));
		PackageStore.AddPackage(new Uri(text), package);
		XpsDocument xpsPackage = new XpsDocument(package, _xpsCompressionOption, text);
		XpsSerializationManager xpsSerializationManager = new XpsSerializationManager(new XpsPackagingPolicy(xpsPackage), batchMode: false);
		DocumentPaginator documentPaginator = ((IDocumentPaginatorSource)CreateFlowDocument()).DocumentPaginator;
		ReportPaginator serializedObject = new ReportPaginator(this, data);
		xpsSerializationManager.SaveAsXaml(serializedObject);
		xpsSerializationManager.Commit();
		package.Close();
		return new XpsDocument(fileName, FileAccess.Read);
	}

	public XpsDocument CreateXpsDocument(IEnumerable<ReportData> data, string fileName)
	{
		if (data == null)
		{
			throw new ArgumentNullException("data");
		}
		int num = 0;
		ReportData reportData = null;
		foreach (ReportData datum in data)
		{
			if (reportData == null)
			{
				reportData = datum;
			}
			num++;
		}
		if (num == 1)
		{
			return CreateXpsDocument(reportData);
		}
		Package package = Package.Open(fileName, FileMode.Create, FileAccess.ReadWrite);
		string text = "pack://report.xps";
		PackageStore.RemovePackage(new Uri(text));
		PackageStore.AddPackage(new Uri(text), package);
		XpsDocument xpsPackage = new XpsDocument(package, _xpsCompressionOption, text);
		XpsSerializationManager xpsSerializationManager = new XpsSerializationManager(new XpsPackagingPolicy(xpsPackage), batchMode: false);
		DocumentPaginator documentPaginator = ((IDocumentPaginatorSource)CreateFlowDocument()).DocumentPaginator;
		MultipleReportPaginator serializedObject = new MultipleReportPaginator(this, data);
		xpsSerializationManager.SaveAsXaml(serializedObject);
		xpsSerializationManager.Commit();
		package.Close();
		return new XpsDocument(fileName, FileAccess.Read);
	}
}
