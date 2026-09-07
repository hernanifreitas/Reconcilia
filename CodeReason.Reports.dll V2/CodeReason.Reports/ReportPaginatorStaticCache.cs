using System.Collections.Generic;
using System.Reflection;

namespace CodeReason.Reports;

internal static class ReportPaginatorStaticCache
{
	private static Dictionary<string, ReportContextValueType> _reportContextValueTypes;

	static ReportPaginatorStaticCache()
	{
		_reportContextValueTypes = new Dictionary<string, ReportContextValueType>(20);
		FieldInfo[] fields = typeof(ReportContextValueType).GetFields();
		foreach (FieldInfo fieldInfo in fields)
		{
			if ((fieldInfo.Attributes & FieldAttributes.Static) != FieldAttributes.PrivateScope)
			{
				_reportContextValueTypes.Add(fieldInfo.Name.ToLowerInvariant(), (ReportContextValueType)fieldInfo.GetRawConstantValue());
			}
		}
	}

	public static ReportContextValueType? GetReportContextValueTypeByName(string name)
	{
		if (name == null)
		{
			return null;
		}
		name = name.ToLowerInvariant();
		if (!_reportContextValueTypes.ContainsKey(name))
		{
			return null;
		}
		return _reportContextValueTypes[name];
	}
}
