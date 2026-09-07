using System;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class ReconciliationConverter : IMultiValueConverter
{
	private static ReconciliationConverter _instance = new ReconciliationConverter();

	public static ReconciliationConverter Instance => _instance;

	private ReconciliationConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationViewModel reconciliationViewModel = (ReconciliationViewModel)parameter;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount != null)
		{
			CompiledQueries.ReconciliationResultParams arg = new CompiledQueries.ReconciliationResultParams
			{
				month = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth,
				year = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear,
				bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id
			};
			IQueryable<Reconciliation> queryable = CompiledQueries.GetReconciliations(ReCrossApp.Current.CurrentDataContext, arg);
			reconciliationViewModel.SetReconciliations((ObjectQuery<Reconciliation>)queryable);
		}
		else
		{
			reconciliationViewModel.SetReconciliations(null);
		}
		return reconciliationViewModel.ReconciliationCVS;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
