using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Utils;

namespace ReCross.FE.ViewModels;

public class CompanyEntityCollConverter : IMultiValueConverter
{
	private static CompanyEntityCollConverter _instance = new CompanyEntityCollConverter();

	public static CompanyEntityCollConverter Instance => _instance;

	private CompanyEntityCollConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		CompanyEntityViewModel companyEntityViewModel = (CompanyEntityViewModel)parameter;
		if (ReCrossApp.Current.StateData.IsLoggedIn)
		{
			IQueryable<CompanyEntity> queryable = CompiledQueries.GetCompanyEntityCollection(ReCrossApp.Current.CurrentDataContext);
			if (!ReCrossApp.Current.UserIsAdmin)
			{
				string key = SubMenuItemEnum.Companies.ToString().ToLower();
				List<CompanyEntity> list = new List<CompanyEntity>();
				foreach (CompanyEntity item in queryable)
				{
					if (ReCrossApp.Current.UserGenericPermissions.ContainsKey(key) && ReCrossApp.Current.UserGenericPermissions[key].CanRead && ((ReCrossApp.Current.IsActiveForOwners && (!ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) || !ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(item.Id))) || (ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) && ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(item.Id) && ReCrossApp.Current.UserRecordPermissions[key][item.Id].CanRead)))
					{
						list.Add(item);
					}
				}
				companyEntityViewModel.SetCollection(list);
			}
			else
			{
				companyEntityViewModel.SetCollection(queryable);
			}
		}
		return companyEntityViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
