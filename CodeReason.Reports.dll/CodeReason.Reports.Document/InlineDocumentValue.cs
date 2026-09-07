using CodeReason.Reports.Interfaces;

namespace CodeReason.Reports.Document;

public class InlineDocumentValue : InlinePropertyValue, IInlineDocumentValue, IAggregateValue, IInlinePropertyValue, IPropertyValue, IHasValue
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
