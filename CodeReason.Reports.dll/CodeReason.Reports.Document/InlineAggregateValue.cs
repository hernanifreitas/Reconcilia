using System;
using System.Collections.Generic;

namespace CodeReason.Reports.Document;

public class InlineAggregateValue : InlineHasValue
{
	private string _aggregateGroup = null;

	private ReportAggregateValueType _aggregateValueType = ReportAggregateValueType.Count;

	private string _emptyValue = "";

	private string _errorValue = "!ERROR!";

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

	public ReportAggregateValueType AggregateValueType
	{
		get
		{
			return _aggregateValueType;
		}
		set
		{
			_aggregateValueType = value;
		}
	}

	public string EmptyValue
	{
		get
		{
			return _emptyValue;
		}
		set
		{
			_emptyValue = value;
		}
	}

	public string ErrorValue
	{
		get
		{
			return _errorValue;
		}
		set
		{
			_errorValue = value;
		}
	}

	public string ComputeAndFormat(Dictionary<string, List<object>> values)
	{
		if (values == null || values.Count <= 0)
		{
			return _emptyValue;
		}
		if (!values.ContainsKey(_aggregateGroup))
		{
			return _emptyValue;
		}
		decimal? num = null;
		bool flag = false;
		long num2 = 0L;
		foreach (object item in values[_aggregateGroup])
		{
			num2++;
			if (_aggregateValueType == ReportAggregateValueType.Count)
			{
				continue;
			}
			if (item == null)
			{
				return _errorValue;
			}
			decimal result;
			if (item is TimeSpan timeSpan)
			{
				result = Convert.ToDecimal(timeSpan.Ticks);
				flag = true;
			}
			else if (!decimal.TryParse(item.ToString(), out result))
			{
				return _errorValue;
			}
			switch (_aggregateValueType)
			{
			case ReportAggregateValueType.Average:
			case ReportAggregateValueType.Sum:
				num = (num.HasValue ? (num + (decimal?)result) : new decimal?(result));
				break;
			case ReportAggregateValueType.Maximum:
			{
				if (!num.HasValue)
				{
					num = result;
					break;
				}
				decimal num3 = result;
				decimal? num4 = num;
				if (num3 > num4.GetValueOrDefault() && num4.HasValue)
				{
					num = result;
				}
				break;
			}
			case ReportAggregateValueType.Minimum:
			{
				if (!num.HasValue)
				{
					num = result;
					break;
				}
				decimal num3 = result;
				decimal? num4 = num;
				if (num3 < num4.GetValueOrDefault() && num4.HasValue)
				{
					num = result;
				}
				break;
			}
			default:
				throw new NotSupportedException($"The aggregate value type {_aggregateValueType.ToString()} is not supported yet!");
			}
		}
		if (_aggregateValueType == ReportAggregateValueType.Count)
		{
			num = num2;
		}
		if (!num.HasValue)
		{
			return _emptyValue;
		}
		if (_aggregateValueType == ReportAggregateValueType.Average)
		{
			num /= (decimal?)num2;
		}
		if (flag)
		{
			return TimeSpan.FromTicks(Convert.ToInt64(num)).ToString();
		}
		return InlineHasValue.FormatValue(num, Format);
	}
}
