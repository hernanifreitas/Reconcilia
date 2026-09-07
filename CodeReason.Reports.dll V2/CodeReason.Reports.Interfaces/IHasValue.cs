namespace CodeReason.Reports.Interfaces;

public interface IHasValue
{
	string Format { get; set; }

	object Value { get; set; }
}
