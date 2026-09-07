using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using ReCross.FE.Utils;

namespace ReCross.FE.ViewModels;

public class ReconciliationCanCloseConverter : IMultiValueConverter
{
	private static ReconciliationCanCloseConverter _instance = new ReconciliationCanCloseConverter();

	public static ReconciliationCanCloseConverter Instance => _instance;

	private ReconciliationCanCloseConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationViewModel viewModel = (ReconciliationViewModel)parameter;
		ReconciliationViewModel.SetReconciliationResults(ref viewModel);
		bool flag = false;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.HasDataSelected && viewModel.CurrentResult != null && viewModel.CurrentResult.ReasonBalance.HasValue && viewModel.CurrentResult.Bankroll.HasValue)
		{
			flag = viewModel.PreviousResult != null && viewModel.PreviousResult.Closed && !viewModel.CurrentResult.Closed && viewModel.CurrentResult.ReasonBalance + (decimal?)viewModel.AccountCredit - (decimal?)viewModel.AccountDebit + (decimal?)viewModel.BankCredit - (decimal?)viewModel.BankDebit == viewModel.CurrentResult.Bankroll;
		}
		return (!flag || !ReCrossApp.Current.UserGenericPermissions[OperationEnum.CloseReOpen.ToString().ToLower()].CanRead) ? Visibility.Hidden : Visibility.Visible;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
