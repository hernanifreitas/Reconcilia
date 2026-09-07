using System;
using System.Collections.Generic;
using System.Data;

namespace CodeReason.Reports;

public class ReportData
{
	private Dictionary<string, object> _reportDocumentValues = new Dictionary<string, object>();

	private List<DataTable> _dataTables = new List<DataTable>();

	private bool _showUnknownValues = true;

	public Dictionary<string, object> ReportDocumentValues => _reportDocumentValues;

	public List<DataTable> DataTables => _dataTables;

	public bool ShowUnknownValues
	{
		get
		{
			return _showUnknownValues;
		}
		set
		{
			_showUnknownValues = value;
		}
	}

	public DataTable GetDataTableByName(string tableName)
	{
		foreach (DataTable dataTable in _dataTables)
		{
			if (dataTable == null || dataTable.TableName == null || !dataTable.TableName.Equals(tableName, StringComparison.InvariantCultureIgnoreCase))
			{
				continue;
			}
			return dataTable;
		}
		return null;
	}

	public void SetDataRowValuesToDocumentValues(DataRow dataRow, string prefix)
	{
		if (prefix == null)
		{
			prefix = "";
		}
		foreach (DataColumn column in dataRow.Table.Columns)
		{
			_reportDocumentValues[prefix + column.ColumnName] = dataRow[column];
		}
	}

	public void SetDataRowValuesToDocumentValues(DataRow dataRow)
	{
		SetDataRowValuesToDocumentValues(dataRow, "");
	}

	public void SetDataRowValuesToDocumentValues(DataRowView dataRowView, string prefix)
	{
		if (prefix == null)
		{
			prefix = "";
		}
		foreach (DataColumn column in dataRowView.Row.Table.Columns)
		{
			_reportDocumentValues[prefix + column.ColumnName] = dataRowView.Row[column];
		}
	}

	public void SetDataRowValuesToDocumentValues(DataRowView dataRowView)
	{
		SetDataRowValuesToDocumentValues(dataRowView, "");
	}
}
