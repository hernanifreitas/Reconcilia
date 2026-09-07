using System;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class BankMovementConverter : IMultiValueConverter
{
	private static BankMovementConverter _instance = new BankMovementConverter();

	public static BankMovementConverter Instance => _instance;

	private BankMovementConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		BankMovementViewModel bankMovementViewModel = (BankMovementViewModel)parameter;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount != null)
		{
			CompiledQueries.MovementParams arg = new CompiledQueries.MovementParams
			{
				minDate = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMinDate,
				maxDate = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate,
				bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id
			};
			IQueryable<BankMovement> queryable = CompiledQueries.GetBankMovements(ReCrossApp.Current.CurrentDataContext, arg);
			bankMovementViewModel.SetBankMovements((ObjectQuery<BankMovement>)queryable);
		}
		else
		{
			bankMovementViewModel.SetBankMovements(null);
		}
		return bankMovementViewModel.BankMovementCVS;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
