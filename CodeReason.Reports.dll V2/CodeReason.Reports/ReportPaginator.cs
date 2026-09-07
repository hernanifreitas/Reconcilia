using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using CodeReason.Reports.Document;
using CodeReason.Reports.Interfaces;

namespace CodeReason.Reports;

public class ReportPaginator : DocumentPaginator
{
	protected DocumentPaginator _paginator = null;

	protected FlowDocument _flowDocument = null;

	protected ReportDocument _report = null;

	protected ReportData _data = null;

	protected Block _blockPageHeader = null;

	protected Block _blockPageFooter = null;

	protected ArrayList _reportContextValues = null;

	protected ReportPaginatorDynamicCache _dynamicCache = null;

	private int _pageCount = 0;

	private Size _pageSize = Size.Empty;

	public override bool IsPageCountValid => _paginator.IsPageCountValid;

	public override int PageCount => _pageCount;

	public override Size PageSize
	{
		get
		{
			return _pageSize;
		}
		set
		{
			_pageSize = value;
		}
	}

	public override IDocumentPaginatorSource Source => _paginator.Source;

	public ReportPaginator(ReportDocument report, ReportData data)
	{
		_report = report;
		_data = data;
		_flowDocument = report.CreateFlowDocument();
		_pageSize = new Size(_flowDocument.PageWidth, _flowDocument.PageHeight);
		if (_flowDocument.PageHeight == double.NaN)
		{
			throw new ArgumentException("Flow document must have a specified page height");
		}
		if (_flowDocument.PageWidth == double.NaN)
		{
			throw new ArgumentException("Flow document must have a specified page width");
		}
		_dynamicCache = new ReportPaginatorDynamicCache(_flowDocument);
		ArrayList flowDocumentVisualListByType = _dynamicCache.GetFlowDocumentVisualListByType(typeof(SectionReportHeader));
		if (flowDocumentVisualListByType.Count > 1)
		{
			throw new ArgumentException("Flow document can have only one report header section");
		}
		if (flowDocumentVisualListByType.Count == 1)
		{
			_blockPageHeader = (SectionReportHeader)flowDocumentVisualListByType[0];
		}
		ArrayList flowDocumentVisualListByType2 = _dynamicCache.GetFlowDocumentVisualListByType(typeof(SectionReportFooter));
		if (flowDocumentVisualListByType2.Count > 1)
		{
			throw new ArgumentException("Flow document can have only one report footer section");
		}
		if (flowDocumentVisualListByType2.Count == 1)
		{
			_blockPageFooter = (SectionReportFooter)flowDocumentVisualListByType2[0];
		}
		_paginator = ((IDocumentPaginatorSource)_flowDocument).DocumentPaginator;
		Block block = _flowDocument.Blocks.FirstBlock;
		while (block != null)
		{
			Block block2 = block;
			block = block.NextBlock;
			if (block2 == _blockPageHeader || block2 == _blockPageFooter)
			{
				_flowDocument.Blocks.Remove(block2);
			}
		}
		_reportContextValues = _dynamicCache.GetFlowDocumentVisualListByInterface(typeof(IInlineContextValue));
		FillData();
	}

	protected void RememberAggregateValue(Dictionary<string, List<object>> aggregateValues, string aggregateGroups, object value)
	{
		if (string.IsNullOrEmpty(aggregateGroups))
		{
			return;
		}
		string[] array = aggregateGroups.Split(',', ';');
		List<object> value2 = null;
		string[] array2 = array;
		foreach (string text in array2)
		{
			if (string.IsNullOrEmpty(text))
			{
				continue;
			}
			string text2 = text.Trim();
			if (!string.IsNullOrEmpty(text2))
			{
				if (!aggregateValues.TryGetValue(text2, out value2))
				{
					value2 = (aggregateValues[text2] = new List<object>());
				}
				value2.Add(value);
			}
		}
	}

	protected virtual void FillCharts(ArrayList charts)
	{
		Window window = null;
		foreach (IChart chart3 in charts)
		{
			if (chart3 == null)
			{
				continue;
			}
			Canvas canvas = chart3 as Canvas;
			if (string.IsNullOrEmpty(chart3.TableName) || string.IsNullOrEmpty(chart3.TableColumns))
			{
				continue;
			}
			DataTable dataTableByName = _data.GetDataTableByName(chart3.TableName);
			if (dataTableByName == null)
			{
				continue;
			}
			if (canvas != null)
			{
				IChart chart2 = (IChart)chart3.Clone();
				if (window == null)
				{
					window = new Window();
					window.WindowStyle = WindowStyle.None;
					window.BorderThickness = new Thickness(0.0);
					window.ShowInTaskbar = false;
					window.Left = 30000.0;
					window.Top = 30000.0;
					window.Show();
				}
				window.Width = canvas.Width + 2.0 * SystemParameters.BorderWidth;
				window.Height = canvas.Height + 2.0 * SystemParameters.BorderWidth;
				window.Content = chart2;
				chart2.DataColumns = null;
				chart2.DataView = dataTableByName.DefaultView;
				chart2.DataColumns = chart3.TableColumns.Split(',', ';');
				chart2.UpdateChart();
				RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)((window.Content as FrameworkElement).RenderSize.Width * 600.0 / 96.0), (int)((window.Content as FrameworkElement).RenderSize.Height * 600.0 / 96.0), 600.0, 600.0, PixelFormats.Pbgra32);
				renderTargetBitmap.Render(window);
				canvas.Children.Add(new Image
				{
					Source = renderTargetBitmap
				});
			}
			else
			{
				chart3.DataColumns = null;
				chart3.DataView = dataTableByName.DefaultView;
				chart3.DataColumns = chart3.TableColumns.Split(',', ';');
				chart3.UpdateChart();
			}
		}
		window?.Close();
	}

	protected virtual void FillData()
	{
		ArrayList flowDocumentVisualListByInterface = _dynamicCache.GetFlowDocumentVisualListByInterface(typeof(IInlineDocumentValue));
		ArrayList flowDocumentVisualListByInterface2 = _dynamicCache.GetFlowDocumentVisualListByInterface(typeof(ITableRowForDataTable));
		ArrayList flowDocumentVisualListByType = _dynamicCache.GetFlowDocumentVisualListByType(typeof(InlineAggregateValue));
		ArrayList flowDocumentVisualListByInterface3 = _dynamicCache.GetFlowDocumentVisualListByInterface(typeof(IChart));
		ArrayList flowDocumentVisualListByInterface4 = _dynamicCache.GetFlowDocumentVisualListByInterface(typeof(ITableRowForDynamicHeader));
		ArrayList flowDocumentVisualListByInterface5 = _dynamicCache.GetFlowDocumentVisualListByInterface(typeof(ITableRowForDynamicDataTable));
		List<Block> list = new List<Block>();
		if (_blockPageHeader != null)
		{
			list.Add(_blockPageHeader);
		}
		if (_blockPageFooter != null)
		{
			list.Add(_blockPageFooter);
		}
		DocumentWalker documentWalker = new DocumentWalker();
		flowDocumentVisualListByInterface.AddRange(documentWalker.TraverseBlockCollection<IInlineDocumentValue>(list));
		Dictionary<string, List<object>> dictionary = new Dictionary<string, List<object>>();
		FillCharts(flowDocumentVisualListByInterface3);
		foreach (IInlineDocumentValue item2 in flowDocumentVisualListByInterface)
		{
			if (item2 == null)
			{
				continue;
			}
			object value = null;
			if (item2.PropertyName != null && _data.ReportDocumentValues.TryGetValue(item2.PropertyName, out value))
			{
				item2.Value = value;
				RememberAggregateValue(dictionary, item2.AggregateGroup, value);
				continue;
			}
			if (_data.ShowUnknownValues && item2.Value == null)
			{
				item2.Value = "[" + ((item2.PropertyName != null) ? item2.PropertyName : "NULL") + "]";
			}
			RememberAggregateValue(dictionary, item2.AggregateGroup, null);
		}
		foreach (ITableRowForDynamicDataTable item3 in flowDocumentVisualListByInterface5)
		{
			if (!(item3 is TableRow { Parent: TableRowGroup parent }))
			{
				continue;
			}
			TableRow tableRow2 = null;
			DataTable dataTableByName = _data.GetDataTableByName(item3.TableName);
			for (int i = 0; i < dataTableByName.Rows.Count; i++)
			{
				tableRow2 = new TableRow();
				DataRow dataRow = dataTableByName.Rows[i];
				for (int j = 0; j < dataTableByName.Columns.Count; j++)
				{
					string text = dataRow[j].ToString();
					tableRow2.Cells.Add(new TableCell(new Paragraph(new Run(text))));
				}
				parent.Rows.Add(tableRow2);
			}
		}
		foreach (ITableRowForDynamicHeader item4 in flowDocumentVisualListByInterface4)
		{
			if (!(item4 is TableRow tableRow3))
			{
				continue;
			}
			DataTable dataTableByName = _data.GetDataTableByName(item4.TableName);
			foreach (DataRow row in dataTableByName.Rows)
			{
				string text = row[0].ToString();
				TableCell item = new TableCell(new Paragraph(new Run(text)));
				tableRow3.Cells.Add(item);
			}
		}
		foreach (ITableRowForDataTable item5 in flowDocumentVisualListByInterface2)
		{
			if (!(item5 is TableRow tableRow4))
			{
				continue;
			}
			DataTable dataTableByName = _data.GetDataTableByName(item5.TableName);
			List<ITableCellValue> list2;
			if (dataTableByName == null)
			{
				if (!_data.ShowUnknownValues)
				{
					continue;
				}
				foreach (TableCell item6 in (IEnumerable<TableCell>)tableRow4.Cells)
				{
					DocumentWalker documentWalker2 = new DocumentWalker();
					list2 = documentWalker2.TraverseBlockCollection<ITableCellValue>(item6.Blocks);
					foreach (ITableCellValue item7 in list2)
					{
						if (item7 is IPropertyValue propertyValue)
						{
							propertyValue.Value = "[" + propertyValue.PropertyName + "]";
							IAggregateValue aggregateValue = item7;
							if (aggregateValue != null)
							{
								RememberAggregateValue(dictionary, aggregateValue.AggregateGroup, null);
							}
						}
					}
				}
				continue;
			}
			list2 = new List<ITableCellValue>();
			foreach (TableCell item8 in (IEnumerable<TableCell>)tableRow4.Cells)
			{
				DocumentWalker documentWalker2 = new DocumentWalker();
				list2.AddRange(documentWalker2.TraverseBlockCollection<ITableCellValue>(item8.Blocks));
			}
			if (!(tableRow4.Parent is TableRowGroup tableRowGroup))
			{
				throw new InvalidDataException("ReportTableRow must have a TableRowGroup as parent");
			}
			List<TableRow> list3 = new List<TableRow>();
			foreach (TableRow item9 in (IEnumerable<TableRow>)tableRowGroup.Rows)
			{
				if (!(item9 is TableRowForDataTable obj))
				{
					list3.Add(XamlHelper.CloneTableRow(item9));
					continue;
				}
				string s = XamlWriter.Save(obj);
				List<TableRow> list4 = new List<TableRow>();
				for (int i = 0; i < dataTableByName.Rows.Count; i++)
				{
					list4.Add((TableRow)XamlHelper.LoadXamlFromString(s));
				}
				foreach (DataRow row2 in dataTableByName.Rows)
				{
					TableRow tableRow5 = list4[0];
					list4.RemoveAt(0);
					foreach (TableCell item10 in (IEnumerable<TableCell>)tableRow5.Cells)
					{
						DocumentWalker documentWalker2 = new DocumentWalker();
						List<ITableCellValue> list5 = documentWalker2.TraverseBlockCollection<ITableCellValue>(item10.Blocks);
						foreach (ITableCellValue item11 in list5)
						{
							if (!(item11 is IPropertyValue propertyValue2))
							{
								continue;
							}
							IAggregateValue aggregateValue = item11;
							try
							{
								object value = row2[propertyValue2.PropertyName];
								if (value == DBNull.Value)
								{
									value = null;
								}
								propertyValue2.Value = value;
								if (aggregateValue != null)
								{
									RememberAggregateValue(dictionary, aggregateValue.AggregateGroup, value);
								}
							}
							catch
							{
								if (_data.ShowUnknownValues)
								{
									propertyValue2.Value = "[" + propertyValue2.PropertyName + "]";
								}
								else
								{
									propertyValue2.Value = "";
								}
								if (aggregateValue != null)
								{
									RememberAggregateValue(dictionary, aggregateValue.AggregateGroup, null);
								}
							}
						}
					}
					list3.Add(tableRow5);
					_report.FireEventDataRowBoundEventArgs(new DataRowBoundEventArgs(_report, row2)
					{
						TableName = row2.Table.TableName,
						TableRow = tableRow5
					});
				}
			}
			tableRowGroup.Rows.Clear();
			foreach (TableRow item12 in list3)
			{
				tableRowGroup.Rows.Add(item12);
			}
		}
		foreach (InlineAggregateValue item13 in flowDocumentVisualListByType)
		{
			if (!string.IsNullOrEmpty(item13.AggregateGroup))
			{
				if (!dictionary.ContainsKey(item13.AggregateGroup))
				{
					item13.Text = item13.EmptyValue;
				}
				else
				{
					item13.Text = item13.ComputeAndFormat(dictionary);
				}
			}
		}
	}

	private ContainerVisual CloneVisualBlock(Block block, int pageNumber)
	{
		FlowDocument flowDocument = new FlowDocument();
		flowDocument.ColumnWidth = double.PositiveInfinity;
		flowDocument.PageHeight = _report.PageHeight;
		flowDocument.PageWidth = _report.PageWidth;
		flowDocument.PagePadding = new Thickness(0.0);
		string xamlText = XamlWriter.Save(block);
		Block item = XamlReader.Parse(xamlText) as Block;
		flowDocument.Blocks.Add(item);
		DocumentWalker documentWalker = new DocumentWalker();
		ArrayList arrayList = new ArrayList();
		arrayList.AddRange(documentWalker.Walk<IInlineContextValue>(flowDocument));
		FillContextValues(arrayList, pageNumber);
		DocumentPage page = ((IDocumentPaginatorSource)flowDocument).DocumentPaginator.GetPage(0);
		return (ContainerVisual)page.Visual;
	}

	protected virtual void FillContextValues(ArrayList list, int pageNumber)
	{
		foreach (IInlineContextValue item in list)
		{
			if (item == null)
			{
				continue;
			}
			ReportContextValueType? reportContextValueTypeByName = ReportPaginatorStaticCache.GetReportContextValueTypeByName(item.PropertyName);
			if (!reportContextValueTypeByName.HasValue)
			{
				if (_data.ShowUnknownValues)
				{
					item.Value = "<" + ((item.PropertyName != null) ? item.PropertyName : "NULL") + ">";
				}
				else
				{
					item.Value = "";
				}
				continue;
			}
			switch (reportContextValueTypeByName.Value)
			{
			case ReportContextValueType.PageNumber:
				item.Value = pageNumber;
				break;
			case ReportContextValueType.PageCount:
				item.Value = _pageCount;
				break;
			case ReportContextValueType.ReportName:
				item.Value = _report.ReportName;
				break;
			case ReportContextValueType.ReportTitle:
				item.Value = _report.ReportTitle;
				break;
			}
		}
	}

	public override DocumentPage GetPage(int pageNumber)
	{
		for (int i = 0; i < 2; i++)
		{
			if (pageNumber == 0)
			{
				_paginator.ComputePageCount();
				_pageCount = _paginator.PageCount;
			}
			FillContextValues(_reportContextValues, pageNumber + 1);
		}
		DocumentPage page = _paginator.GetPage(pageNumber);
		if (page == DocumentPage.Missing)
		{
			return DocumentPage.Missing;
		}
		_pageSize = page.Size;
		ContainerVisual containerVisual = new ContainerVisual();
		if (_blockPageHeader != null)
		{
			ContainerVisual containerVisual2 = CloneVisualBlock(_blockPageHeader, pageNumber + 1);
			containerVisual2.Offset = new Vector(0.0, 0.0);
			containerVisual.Children.Add(containerVisual2);
		}
		ContainerVisual containerVisual3 = new ContainerVisual();
		containerVisual3.Offset = new Vector(0.0, _report.PageHeaderHeight / 100.0 * _report.PageHeight);
		containerVisual3.Children.Add(page.Visual);
		containerVisual.Children.Add(containerVisual3);
		if (_blockPageFooter != null)
		{
			ContainerVisual containerVisual2 = CloneVisualBlock(_blockPageFooter, pageNumber + 1);
			containerVisual2.Offset = new Vector(0.0, _report.PageHeight - _report.PageFooterHeight / 100.0 * _report.PageHeight);
			containerVisual.Children.Add(containerVisual2);
		}
		Rect bleedBox = new Rect(page.BleedBox.Left, page.BleedBox.Top, page.BleedBox.Width, _report.PageHeight - (page.Size.Height - page.BleedBox.Size.Height));
		Rect contentBox = new Rect(page.ContentBox.Left, page.ContentBox.Top, page.ContentBox.Width, _report.PageHeight - (page.Size.Height - page.ContentBox.Size.Height));
		DocumentPage result = new DocumentPage(containerVisual, new Size(_report.PageWidth, _report.PageHeight), bleedBox, contentBox);
		_report.FireEventGetPageCompleted(new GetPageCompletedEventArgs(page, pageNumber, null, cancelled: false, null));
		return result;
	}
}
