using System;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;

namespace ReCross.FE.ViewModels;

public class BankAccountConverter : IMultiValueConverter
{
	private static BankAccountConverter _instance = new BankAccountConverter();

	public static BankAccountConverter Instance => _instance;

	private BankAccountConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		MovementFilterViewModel movementFilterViewModel = (MovementFilterViewModel)parameter;
		try
		{
			if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity != null && ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankEntity != null)
			{
				IQueryable<BankAccount> queryable = ReCrossApp.Current.CurrentDataContext.BankAccounts.Where((BankAccount rec) => rec.Entity.Id == ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity.Id && rec.BankEntity.Id == ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankEntity.Id);
				movementFilterViewModel.SetBankAccounts((queryable != null) ? ((ObjectQuery<BankAccount>)queryable).ToList() : null);
			}
			else
			{
				movementFilterViewModel.SetBankAccounts(null);
			}
		}
		catch
		{
		}
		return movementFilterViewModel.BankAccountCVS.Source;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
