using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Utils;
using Strills.WPF.Data.Objects;

namespace ReCross.FE.ViewModels;

public class ReconciliationResultCollConverter : IMultiValueConverter
{
	private static ReconciliationResultCollConverter _instance = new ReconciliationResultCollConverter();

	public static ReconciliationResultCollConverter Instance => _instance;

	private ReconciliationResultCollConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationResultViewModel reconciliationResultViewModel = (ReconciliationResultViewModel)parameter;
		if (ReCrossApp.Current.StateData.IsLoggedIn && ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity != null)
		{
			CompiledQueries.ReconciliationResultParams arg = new CompiledQueries.ReconciliationResultParams
			{
				month = -1,
				year = -1,
				entity = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity.Id
			};
			IQueryable<ReconciliationResult> queryable = CompiledQueries.GetRecResults(ReCrossApp.Current.CurrentDataContext, arg);
			if (queryable != null && queryable.Count() > 0)
			{
				string[] parentKeys = new string[1] { MenuItemEnum.Parametrization.ToString().ToLower() };
				string key = SubMenuItemEnum.ReconciliationResults.ToString().ToLower();
				foreach (ReconciliationResult item in queryable.ToList())
				{
					bool hasRecordPermission = item.Id > 0 && ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) && ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(item.Id);
					EntityObjectExtended.SetPermissionRecords(item, ReCrossApp.Current.UserGenericPermissions, ReCrossApp.Current.UserRecordPermissions, item.Id, key, parentKeys, hasRecordPermission);
				}
				reconciliationResultViewModel.SetCollection(queryable.ToList());
			}
			else
			{
				reconciliationResultViewModel.SetCollection(null);
			}
		}
		return reconciliationResultViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
