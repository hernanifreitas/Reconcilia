using System;
using System.Data;
using System.Windows.Documents;

namespace CodeReason.Reports;

public class DataRowBoundEventArgs : EventArgs
{
	public DataRow DataRow { get; protected set; }

	public ReportDocument ReportDocument { get; protected set; }

	public string TableName { get; set; }

	public TableRow TableRow { get; set; }

	public DataRowBoundEventArgs()
		: this(null, null)
	{
	}

	public DataRowBoundEventArgs(ReportDocument report)
		: this(report, null)
	{
	}

	public DataRowBoundEventArgs(ReportDocument report, DataRow row)
	{
		ReportDocument = report;
		DataRow = row;
	}
}
