using CodeReason.Reports.Interfaces;

namespace CodeReason.Reports.Document;

public class InlineTableCellValue : InlinePropertyValue, ITableCellValue, IHasValue, IAggregateValue
{
	private string _aggregateGroup = null;

	public string AggregateGroup
	{
		get
		{
			return _aggregateGroup;
		}
		set
		{
			_aggregateGroup = value;
		}
	}
}
