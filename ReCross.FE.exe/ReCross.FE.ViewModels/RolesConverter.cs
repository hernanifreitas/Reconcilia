using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;

namespace ReCross.FE.ViewModels;

public class RolesConverter : IMultiValueConverter
{
	private static RolesConverter _instance = new RolesConverter();

	public static RolesConverter Instance => _instance;

	private RolesConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		IEntityRoles entityRoles = (IEntityRoles)parameter;
		try
		{
			if (values != null && values.Count() == 2 && values[0] != null)
			{
				Functionality current = (Functionality)values[0];
				if (entityRoles.IsAdding)
				{
					entityRoles.SetRoles(ReCrossApp.Current.CurrentDataContext.Roles.Where((Role rec) => rec.Functionality.Id == current.Id && rec.OwnerId != (long?)null)?.ToList());
				}
				else if (entityRoles.IsEditing)
				{
					bool getWithOwners = ((Role)values[1]).OwnerId.HasValue;
					entityRoles.SetRoles(ReCrossApp.Current.CurrentDataContext.Roles.Where((Role rec) => rec.Functionality.Id == current.Id && (getWithOwners ? (rec.OwnerId != (long?)null) : (rec.OwnerId == (long?)null)))?.ToList());
				}
			}
			else
			{
				entityRoles.SetRoles(null);
			}
		}
		catch
		{
		}
		return entityRoles.RoleCVS.Source;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
