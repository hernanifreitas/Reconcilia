using System.Windows.Documents;
using CodeReason.Reports.Interfaces;

namespace CodeReason.Reports.Document;

public class TableRowForDynamicDataTable : TableRow, ITableRowForDynamicDataTable
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
