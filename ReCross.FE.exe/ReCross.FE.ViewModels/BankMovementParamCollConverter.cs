using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Utils;
using Strills.WPF.Data.Objects;

namespace ReCross.FE.ViewModels;

public class BankMovementParamCollConverter : IMultiValueConverter
{
	private static BankMovementParamCollConverter _instance = new BankMovementParamCollConverter();

	public static BankMovementParamCollConverter Instance => _instance;

	private BankMovementParamCollConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		BankMovementParamViewModel bankMovementParamViewModel = (BankMovementParamViewModel)parameter;
		if (ReCrossApp.Current.StateData.IsLoggedIn)
		{
			IQueryable<MovementParam> queryable = CompiledQueries.GetBankEntityMovementParamCollection(ReCrossApp.Current.CurrentDataContext);
			if (queryable != null && queryable.Count() > 0)
			{
				string[] parentKeys = new string[1] { MenuItemEnum.Parametrization.ToString().ToLower() };
				string key = SubMenuItemEnum.BankMovementParam.ToString().ToLower();
				foreach (MovementParam item in queryable.ToList())
				{
					bool hasRecordPermission = item.Id > 0 && ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) && ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(item.Id);
					EntityObjectExtended.SetPermissionRecords(item, ReCrossApp.Current.UserGenericPermissions, ReCrossApp.Current.UserRecordPermissions, item.Id, key, parentKeys, hasRecordPermission);
				}
				bankMovementParamViewModel.SetCollection(queryable.ToList());
			}
			else
			{
				bankMovementParamViewModel.SetCollection(null);
			}
		}
		return bankMovementParamViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
