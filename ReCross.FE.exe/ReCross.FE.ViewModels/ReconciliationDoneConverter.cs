using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReCross.FE.ViewModels;

public class ReconciliationDoneConverter : IMultiValueConverter
{
	private static ReconciliationDoneConverter _instance = new ReconciliationDoneConverter();

	public static ReconciliationDoneConverter Instance => _instance;

	private ReconciliationDoneConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationViewModel viewModel = (ReconciliationViewModel)parameter;
		ReconciliationViewModel.SetReconciliationResults(ref viewModel);
		bool flag = false;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.HasDataSelected && viewModel.CurrentResult != null && viewModel.CurrentResult.ReasonBalance.HasValue && viewModel.CurrentResult.Bankroll.HasValue)
		{
			flag = viewModel.PreviousResult != null && viewModel.PreviousResult.Closed && viewModel.CurrentResult.Closed;
		}
		return (!flag) ? Visibility.Hidden : Visibility.Visible;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
