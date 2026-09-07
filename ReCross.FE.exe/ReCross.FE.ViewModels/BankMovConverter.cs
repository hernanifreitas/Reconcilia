using System;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class BankMovConverter : IMultiValueConverter
{
	private static BankMovConverter _instance = new BankMovConverter();

	public static BankMovConverter Instance => _instance;

	private BankMovConverter()
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
				IQueryable<BankMovement> queryable = CompiledQueries.GetReconcilingBankMovs(ReCrossApp.Current.CurrentDataContext, arg);
				reconciliationViewModel.SetBankMovs((ObjectQuery<BankMovement>)queryable);
			}
			catch (Exception ex)
			{
				reconciliationViewModel.SetBankMovs(null);
				MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
		}
		else
		{
			reconciliationViewModel.SetBankMovs(null);
		}
		return reconciliationViewModel.BankMovCVS;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
