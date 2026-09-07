using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Utils;

namespace ReCross.FE.ViewModels;

public class PersonEntityCollConverter : IMultiValueConverter
{
	private static PersonEntityCollConverter _instance = new PersonEntityCollConverter();

	public static PersonEntityCollConverter Instance => _instance;

	private PersonEntityCollConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		PersonEntityViewModel personEntityViewModel = (PersonEntityViewModel)parameter;
		if (ReCrossApp.Current.StateData.IsLoggedIn)
		{
			IQueryable<PersonEntity> queryable = CompiledQueries.GetPersonEntityCollection(ReCrossApp.Current.CurrentDataContext);
			if (!ReCrossApp.Current.UserIsAdmin)
			{
				string key = SubMenuItemEnum.Persons.ToString().ToLower();
				List<PersonEntity> list = new List<PersonEntity>();
				foreach (PersonEntity item in queryable)
				{
					if (ReCrossApp.Current.UserGenericPermissions.ContainsKey(key) && ReCrossApp.Current.UserGenericPermissions[key].CanRead && ((ReCrossApp.Current.IsActiveForOwners && (!ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) || !ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(item.Id))) || (ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) && ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(item.Id) && ReCrossApp.Current.UserRecordPermissions[key][item.Id].CanRead)))
					{
						list.Add(item);
					}
				}
				personEntityViewModel.SetCollection(list);
			}
			else
			{
				personEntityViewModel.SetCollection(queryable);
			}
		}
		return personEntityViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
