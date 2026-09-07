namespace CodeReason.Reports.Interfaces;

public interface IPropertyValue : IHasValue
{
	string PropertyName { get; set; }
}
