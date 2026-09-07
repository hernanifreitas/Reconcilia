using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ReCross.FE.Utils;

namespace ReCross.FE.ViewModels;

public class ReconciliationUndoneConverter : IMultiValueConverter
{
	private static ReconciliationUndoneConverter _instance = new ReconciliationUndoneConverter();

	public static ReconciliationUndoneConverter Instance => _instance;

	private ReconciliationUndoneConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationViewModel viewModel = (ReconciliationViewModel)parameter;
		ReconciliationViewModel.SetReconciliationResults(ref viewModel);
		bool flag = false;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.HasDataSelected && viewModel.CurrentResult != null)
		{
			flag = viewModel.CurrentResult.Closed && (viewModel.NextResult == null || !viewModel.NextResult.Closed) && viewModel.PreviousResult != null;
		}
		return (!flag || !ReCrossApp.Current.UserGenericPermissions[OperationEnum.CloseReOpen.ToString().ToLower()].CanRead) ? Visibility.Hidden : Visibility.Visible;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
