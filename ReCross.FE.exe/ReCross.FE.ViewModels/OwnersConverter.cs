using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class OwnersConverter : IMultiValueConverter
{
	private static OwnersConverter _instance = new OwnersConverter();

	public static OwnersConverter Instance => _instance;

	private OwnersConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		RoleViewModel roleViewModel = (RoleViewModel)parameter;
		try
		{
			if (values != null && values.Count() == 1)
			{
				Functionality functionality = (Functionality)values[0];
				object obj = null;
				if (functionality != null)
				{
					switch (functionality.Abbreviation)
					{
					case "Persons":
						obj = CompiledQueries.GetPersonEntityCollection(ReCrossEntities.CurrentContext);
						break;
					case "Companies":
						obj = CompiledQueries.GetCompanyEntityCollection(ReCrossEntities.CurrentContext);
						break;
					case "BankEntities":
						obj = CompiledQueries.GetBankEntityCollection(ReCrossEntities.CurrentContext);
						break;
					}
				}
				roleViewModel.SetOwners((obj != null) ? obj : null);
			}
			else
			{
				roleViewModel.SetOwners(null);
			}
		}
		catch
		{
		}
		return roleViewModel.OwnerCVS.Source;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
