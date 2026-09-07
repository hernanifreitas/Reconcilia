using System;
using System.Globalization;
using System.Windows.Data;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class BankEntityCollConverter : IMultiValueConverter
{
	private static BankEntityCollConverter _instance = new BankEntityCollConverter();

	public static BankEntityCollConverter Instance => _instance;

	private BankEntityCollConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		BankEntityViewModel bankEntityViewModel = (BankEntityViewModel)parameter;
		bankEntityViewModel.SetCollection(CompiledQueries.GetBankEntityCollection(ReCrossApp.Current.CurrentDataContext));
		return bankEntityViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
