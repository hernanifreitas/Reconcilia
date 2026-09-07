using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ReCross.FE.ViewModels;

public class ReconciliationForbiddenConverter : IMultiValueConverter
{
	private static ReconciliationForbiddenConverter _instance = new ReconciliationForbiddenConverter();

	public static ReconciliationForbiddenConverter Instance => _instance;

	private ReconciliationForbiddenConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationViewModel viewModel = (ReconciliationViewModel)parameter;
		ReconciliationViewModel.SetReconciliationResults(ref viewModel);
		bool flag = true;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.HasDataSelected)
		{
			flag = (viewModel.PreviousResult != null && !viewModel.PreviousResult.Closed) || !viewModel.CurrentResult.ReasonBalance.HasValue || !viewModel.CurrentResult.Bankroll.HasValue;
		}
		return flag ? Visibility.Hidden : Visibility.Visible;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
