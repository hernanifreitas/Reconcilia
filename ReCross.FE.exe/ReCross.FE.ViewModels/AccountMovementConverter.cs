using System;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class AccountMovementConverter : IMultiValueConverter
{
	private static AccountMovementConverter _instance = new AccountMovementConverter();

	public static AccountMovementConverter Instance => _instance;

	private AccountMovementConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		AccountMovementViewModel accountMovementViewModel = (AccountMovementViewModel)parameter;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount != null)
		{
			CompiledQueries.MovementParams arg = new CompiledQueries.MovementParams
			{
				minDate = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMinDate,
				maxDate = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate,
				bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id
			};
			IQueryable<AccountMovement> queryable = CompiledQueries.GetAccountMovements(ReCrossApp.Current.CurrentDataContext, arg);
			accountMovementViewModel.SetAccountMovements((ObjectQuery<AccountMovement>)queryable);
		}
		else
		{
			accountMovementViewModel.SetAccountMovements(null);
		}
		return accountMovementViewModel.AccountMovementCVS;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
