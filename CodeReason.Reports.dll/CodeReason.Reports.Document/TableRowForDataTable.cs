using System.Windows.Documents;
using CodeReason.Reports.Interfaces;

namespace CodeReason.Reports.Document;

public class TableRowForDataTable : TableRow, ITableRowForDataTable
{
	private string _tableName = null;

	public string TableName
	{
		get
		{
			return _tableName;
		}
		set
		{
			_tableName = value;
		}
	}
}
