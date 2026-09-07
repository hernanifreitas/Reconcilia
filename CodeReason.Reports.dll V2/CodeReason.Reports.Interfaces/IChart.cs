using System;
using System.Data;

namespace CodeReason.Reports.Interfaces;

public interface IChart : ICloneable
{
	string TableColumns { get; set; }

	string TableName { get; set; }

	string[] DataColumns { get; set; }

	DataView DataView { get; set; }

	void UpdateChart();
}
