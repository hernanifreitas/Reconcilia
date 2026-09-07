using System;
using System.Globalization;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.Localization;

namespace ReCross.FE.ViewModels;

public class MatchDifferenceConverter : IValueConverter
{
	private static MatchDifferenceConverter _instance = new MatchDifferenceConverter();

	public static MatchDifferenceConverter Instance => _instance;

	private MatchDifferenceConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		ReconciliationViewModel reconciliationViewModel = (ReconciliationViewModel)parameter;
		decimal num = 0m;
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.HasDataSelected)
		{
			decimal value2 = 0m;
			decimal value3 = 0m;
			if (reconciliationViewModel.SelectedAccountMovements != null)
			{
				foreach (AccountMovement selectedAccountMovement in reconciliationViewModel.SelectedAccountMovements)
				{
					value2 += selectedAccountMovement.Amount;
				}
			}
			if (reconciliationViewModel.SelectedBankMovements != null)
			{
				foreach (BankMovement selectedBankMovement in reconciliationViewModel.SelectedBankMovements)
				{
					value3 += selectedBankMovement.Amount;
				}
			}
			num = Math.Abs(Math.Abs(value2) - Math.Abs(value3));
		}
		NumberFormatInfo numberFormatInfo = (NumberFormatInfo)CultureManager.Culture.NumberFormat.Clone();
		numberFormatInfo.CurrencySymbol = string.Empty;
		return string.Format(numberFormatInfo, "{0:C2}", new object[1] { num });
	}

	public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
