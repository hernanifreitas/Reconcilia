using System;
using System.Windows.Controls;

namespace CodeReason.Reports;

public class ImageEventArgs : EventArgs
{
	public Image Image { get; protected set; }

	public ReportDocument ReportDocument { get; protected set; }

	public ImageEventArgs()
		: this(null, null)
	{
	}

	public ImageEventArgs(ReportDocument report)
		: this(report, null)
	{
	}

	public ImageEventArgs(ReportDocument report, Image image)
	{
		ReportDocument = report;
		Image = image;
	}
}
