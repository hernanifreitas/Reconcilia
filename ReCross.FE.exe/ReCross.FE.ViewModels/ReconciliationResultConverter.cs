using System;
using System.Globalization;
using System.Windows.Data;

namespace ReCross.FE.ViewModels;

public class ReconciliationResultConverter : IMultiValueConverter
{
	private static ReconciliationResultConverter _instance = new ReconciliationResultConverter();

	public static ReconciliationResultConverter Instance => _instance;

	private ReconciliationResultConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationViewModel viewModel = (ReconciliationViewModel)parameter;
		ReconciliationViewModel.SetReconciliationResults(ref viewModel);
		return viewModel.CurrentResult;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
