using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class BankAccountsConverter : IMultiValueConverter
{
	private static BankAccountsConverter _instance = new BankAccountsConverter();

	public static BankAccountsConverter Instance => _instance;

	private BankAccountsConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		BankAccountViewModel bankAccountViewModel = (BankAccountViewModel)parameter;
		try
		{
			if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity != null)
			{
				CompiledQueries.BankAccountParams arg = new CompiledQueries.BankAccountParams
				{
					entity = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity.Id
				};
				IQueryable<BankAccount> bankAccounts = CompiledQueries.GetBankAccounts(ReCrossApp.Current.CurrentDataContext, arg);
				bankAccountViewModel.SetBankAccounts(bankAccounts);
			}
			else
			{
				bankAccountViewModel.SetBankAccounts(null);
			}
		}
		catch
		{
		}
		return bankAccountViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
