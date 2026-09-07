using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Documents;

namespace CodeReason.Reports;

public class MultipleReportPaginator : DocumentPaginator
{
	private List<ReportPaginator> _reportPaginators = new List<ReportPaginator>();

	private List<DocumentPage> _firstPages = new List<DocumentPage>();

	private int _pageCount = 0;

	private Size _pageSize = Size.Empty;

	public override bool IsPageCountValid => true;

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

	public override IDocumentPaginatorSource Source => null;

	public MultipleReportPaginator(ReportDocument report, IEnumerable<ReportData> data)
	{
		if (data == null)
		{
			throw new ArgumentException("Need at least two ReportData objects");
		}
		_pageCount = 0;
		int num = 0;
		foreach (ReportData datum in data)
		{
			if (datum != null)
			{
				ReportPaginator reportPaginator = new ReportPaginator(report, datum);
				_reportPaginators.Add(reportPaginator);
				DocumentPage page = reportPaginator.GetPage(0);
				if (page != DocumentPage.Missing && page.Size != Size.Empty)
				{
					_pageSize = reportPaginator.PageSize;
				}
				_firstPages.Add(page);
				_pageCount += reportPaginator.PageCount;
				num++;
			}
		}
		if (_reportPaginators.Count <= 0 || num < 2)
		{
			throw new ArgumentException("Need at least two ReportData objects");
		}
	}

	public override DocumentPage GetPage(int pageNumber)
	{
		int num = 0;
		int num2 = 0;
		ReportPaginator reportPaginator = null;
		foreach (ReportPaginator reportPaginator2 in _reportPaginators)
		{
			int pageCount = reportPaginator2.PageCount;
			if (pageNumber >= num + pageCount)
			{
				num += pageCount;
				num2++;
				continue;
			}
			reportPaginator = reportPaginator2;
			break;
		}
		if (reportPaginator == null)
		{
			return DocumentPage.Missing;
		}
		DocumentPage documentPage = null;
		documentPage = ((pageNumber != 0) ? reportPaginator.GetPage(pageNumber - num) : _firstPages[num2]);
		if (documentPage == DocumentPage.Missing)
		{
			return DocumentPage.Missing;
		}
		_pageSize = documentPage.Size;
		return documentPage;
	}
}
