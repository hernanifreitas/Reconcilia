using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.Localization;

namespace ReCross.FE.ViewModels;

public class AccountMovSumConverter : IValueConverter
{
	private static AccountMovSumConverter _instance = new AccountMovSumConverter();

	public static AccountMovSumConverter Instance => _instance;

	private AccountMovSumConverter()
	{
	}

	public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
	{
		decimal num = 0m;
		decimal value2 = 0m;
		decimal value3 = 0m;
		if (value != null && parameter != null)
		{
			ReconciliationViewModel reconciliationViewModel = (ReconciliationViewModel)parameter;
			if (reconciliationViewModel.AccountMovCVS != null && reconciliationViewModel.AccountMovCVS.Source != null)
			{
				List<AccountMovement> list = (List<AccountMovement>)reconciliationViewModel.AccountMovCVS.Source;
				foreach (AccountMovement item in list)
				{
					num += item.Amount;
					value2 += (item.CreditAmount.HasValue ? item.CreditAmount.Value : 0m);
					value3 += (item.DebitAmount.HasValue ? item.DebitAmount.Value : 0m);
				}
			}
			reconciliationViewModel.AccountCredit = Math.Abs(value2);
			reconciliationViewModel.AccountDebit = Math.Abs(value3);
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
