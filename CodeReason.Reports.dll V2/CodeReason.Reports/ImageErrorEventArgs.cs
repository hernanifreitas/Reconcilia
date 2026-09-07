using System;
using System.Windows.Controls;

namespace CodeReason.Reports;

public class ImageErrorEventArgs : EventArgs
{
	public Exception Exception { get; protected set; }

	public bool Handled { get; set; }

	public Image Image { get; protected set; }

	public ReportDocument ReportDocument { get; protected set; }

	public ImageErrorEventArgs()
		: this(null, null, null)
	{
	}

	public ImageErrorEventArgs(Exception exception)
		: this(exception, null, null)
	{
	}

	public ImageErrorEventArgs(Exception exception, ReportDocument report)
		: this(exception, report, null)
	{
	}

	public ImageErrorEventArgs(Exception exception, ReportDocument report, Image image)
	{
		Exception = exception;
		Image = image;
		Handled = false;
		ReportDocument = report;
	}
}
