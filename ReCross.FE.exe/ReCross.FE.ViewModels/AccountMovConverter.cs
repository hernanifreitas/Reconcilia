using System;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class AccountMovConverter : IMultiValueConverter
{
	private static AccountMovConverter _instance = new AccountMovConverter();

	public static AccountMovConverter Instance => _instance;

	private AccountMovConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationViewModel reconciliationViewModel = (ReconciliationViewModel)parameter;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount != null)
		{
			CompiledQueries.MovementParams arg = new CompiledQueries.MovementParams
			{
				maxDate = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate,
				bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id
			};
			try
			{
				IQueryable<AccountMovement> queryable = CompiledQueries.GetReconcilingAccountMovs(ReCrossApp.Current.CurrentDataContext, arg);
				reconciliationViewModel.SetAccountMovs((ObjectQuery<AccountMovement>)queryable);
			}
			catch (Exception ex)
			{
				reconciliationViewModel.SetAccountMovs(null);
				MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
		else
		{
			reconciliationViewModel.SetAccountMovs(null);
		}
		return reconciliationViewModel.AccountMovCVS;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
