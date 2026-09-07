using System;
using System.Globalization;
using System.Windows.Data;
using ReCross.BE.Utils;

namespace ReCross.FE.ViewModels;

public class SoftwareEntityCollConverter : IMultiValueConverter
{
	private static SoftwareEntityCollConverter _instance = new SoftwareEntityCollConverter();

	public static SoftwareEntityCollConverter Instance => _instance;

	private SoftwareEntityCollConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		SoftwareEntityViewModel softwareEntityViewModel = (SoftwareEntityViewModel)parameter;
		softwareEntityViewModel.SetCollection(CompiledQueries.GetSoftwareEntityCollection(ReCrossApp.Current.CurrentDataContext));
		return softwareEntityViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
