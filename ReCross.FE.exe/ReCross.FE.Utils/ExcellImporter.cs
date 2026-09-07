using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.ViewModels;
using Strills.WPF.Localization;

namespace ReCross.FE.Utils;

public class ExcellImporter
{
	private const int C_BreakPageMaxCount = 30;

	public const string C_Debit_Code = "D";

	public const string C_Credit_Code = "C";

	public const string C_Debit_Signal = "-";

	public const string C_Credit_Signal = "+";

	public const string C_FileName = "Movements";

	public const string C_DefaultExt = ".xls";

	public const string C_Filter = "Excell documents (*.xls,*.xlsx)|*.xls;*.xlsx";

	public static List<char> CharList = new List<char>(new char[26]
	{
		'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
		'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
		'u', 'v', 'w', 'x', 'y', 'z'
	});

	public static OpenFileDialog GetFileDialog()
	{
		OpenFileDialog openFileDialog = new OpenFileDialog();
		openFileDialog.FileName = "Movements";
		openFileDialog.DefaultExt = ".xls";
		openFileDialog.Filter = "Excell documents (*.xls,*.xlsx)|*.xls;*.xlsx";
		return openFileDialog;
	}

	public static int GetColumnNumber(object column)
	{
		int result = 0;
		if (column != null)
		{
			if (int.TryParse(column.ToString(), out result))
			{
				return result;
			}
			string text = column.ToString();
			char[] array = text.ToCharArray();
			foreach (char c in array)
			{
				result += CharList.IndexOf(char.Parse(c.ToString().ToLower())) + 1;
			}
		}
		return result;
	}

	public static int GetColumnNumber(object column, ref string color)
	{
		string[] array = column.ToString().Split('_');
		if (array != null && array.Length == 2)
		{
			color = array[1];
			return GetColumnNumber(array[0]);
		}
		return GetColumnNumber(column);
	}

	private static List<object> GetColumnNumbers(string column)
	{
		List<object> list = new List<object>();
		if (column != null)
		{
			int result = 0;
			if (int.TryParse(column.ToString(), out result))
			{
				list.Add(result);
				return list;
			}
			string[] array = column.ToString().Split(',');
			if (array != null && array.Length > 0)
			{
				string[] array2 = array;
				foreach (string text in array2)
				{
					result = 0;
					char[] array3 = text.ToCharArray();
					foreach (char c in array3)
					{
						result += CharList.IndexOf(char.Parse(c.ToString().ToLower())) + 1;
					}
					list.Add(result);
				}
			}
		}
		return list;
	}

	private static string GetDescriptionByColumns(Worksheet workSheet, int currentRow, List<object> descrColumns)
	{
		string result = null;
		if (descrColumns.Count > 0)
		{
			List<string> list = new List<string>();
			foreach (int descrColumn in descrColumns)
			{
				list.Add(((dynamic)((Range)(dynamic)workSheet.Cells[currentRow, descrColumn]).Value2 != null) ? ((dynamic)((Range)(dynamic)workSheet.Cells[currentRow, descrColumn]).Value2).ToString() : string.Empty);
			}
			result = string.Join(" / ", list.ToArray());
		}
		return result;
	}

	public static bool CellHasValidValueForDebitOrCredit(object cellValue, ref string debitCreditDCValue)
	{
		bool flag = false;
		string text = cellValue?.ToString();
		if (!string.IsNullOrEmpty(text))
		{
			text = text.Trim().ToUpper();
			int num;
			switch (text)
			{
			default:
				num = ((text == "+") ? 1 : 0);
				break;
			case "D":
			case "C":
			case "-":
				num = 1;
				break;
			}
			flag = (byte)num != 0;
			if (flag)
			{
				debitCreditDCValue = text;
			}
		}
		return flag;
	}

	public static void ImportExcellAccountMovements(string fileName, BankAccount bankAccount, MovementParam param, bool deleteImported)
	{
		decimal? num = 0m;
		decimal? num2 = 0m;
		Microsoft.Office.Interop.Excel.Application application = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
		Workbook workbook = application.Workbooks.Open(fileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
		Worksheet worksheet = (Worksheet)(dynamic)workbook.ActiveSheet;
		object obj = 1;
		List<object> columnNumbers = GetColumnNumbers(param.Description);
		object columnIndex = GetColumnNumber(param.Date);
		string text = param.Debit;
		string text2 = param.Credit;
		object obj2 = null;
		if (param.Credit == param.Debit)
		{
			string[] array = param.Credit.Split('&');
			string text3 = ((array != null && array.Length == 2) ? array[1] : null);
			text = (text2 = ((array != null && array.Length == 2) ? array[0] : param.Credit));
			if (!string.IsNullOrEmpty(text3))
			{
				obj2 = GetColumnNumber(text3);
			}
		}
		string color = string.Empty;
		object columnIndex2 = GetColumnNumber(text, ref color);
		string color2 = string.Empty;
		object columnIndex3 = GetColumnNumber(text2, ref color2);
		object obj3 = null;
		string column = param.Balance;
		object obj4 = null;
		object obj5 = null;
		if (param.Balance.IndexOf('&') > 0)
		{
			string[] array = param.Balance.Split('&');
			string text3 = ((array != null && array.Length == 2) ? array[1] : null);
			column = ((array != null && array.Length == 2) ? array[0] : param.Balance);
			if (!string.IsNullOrEmpty(text3))
			{
				obj3 = GetColumnNumber(text3);
			}
		}
		else if (param.Balance.IndexOf('+') > 0)
		{
			string[] array = param.Balance.Split('+');
			if (array != null && array.Length == 2)
			{
				obj4 = GetColumnNumber(array[0]);
				obj5 = GetColumnNumber(array[1]);
			}
		}
		string color3 = string.Empty;
		object obj6 = null;
		if (obj4 == null && obj5 == null)
		{
			obj6 = GetColumnNumber(column, ref color3);
		}
		if (!string.IsNullOrEmpty(color2))
		{
			string[] array = param.Credit.Split('_');
			text2 = ((array != null && array.Length == 2) ? array[0] : param.Credit);
		}
		if (!string.IsNullOrEmpty(color))
		{
			string[] array = param.Debit.Split('_');
			text = ((array != null && array.Length == 2) ? array[0] : param.Debit);
		}
		List<AccountMovement> list = new List<AccountMovement>();
		int i = int.Parse(obj.ToString());
		string arg = string.Empty;
		try
		{
			bool flag = false;
			int num3 = 0;
			for (i = int.Parse(obj.ToString()); i < worksheet.Cells.Rows.Count; i++)
			{
				arg = "Initialization";
				string empty = string.Empty;
				DateTime result = default(DateTime);
				string text4 = null;
				decimal? debitAmount = null;
				decimal? creditAmount = null;
				double result2 = -1.0;
				string text5 = null;
				bool flag2 = false;
				decimal result3;
				if ((bool)(columnNumbers.Count > 0 && (obj2 == null || ExcellImporter.CellHasValidValueForDebitOrCredit((dynamic)((Range)(dynamic)worksheet.Cells[i, obj2]).Value2, ref text4)) && (obj3 == null || ExcellImporter.CellHasValidValueForDebitOrCredit((dynamic)((Range)(dynamic)worksheet.Cells[i, obj3]).Value2, ref text5))) && (bool)((obj6 == null && ((dynamic)((Range)(dynamic)worksheet.Cells[i, obj4]).Value2 != null || (dynamic)((Range)(dynamic)worksheet.Cells[i, obj5]).Value2 != null)) || (obj6 != null && (dynamic)((Range)(dynamic)worksheet.Cells[i, obj6]).Value2 != null && ((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2 != null || (dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2 != null))))
				{
					string text6 = (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex]).Value2 != null) ? ((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex]).Value2).ToString() : null);
					if (!string.IsNullOrEmpty(text6))
					{
						bool flag3 = DateTime.TryParse(text6, CultureManager.Culture, DateTimeStyles.None, out result) || double.TryParse(text6, out result2);
						if (result2 >= 0.0)
						{
							try
							{
								result = DateTime.FromOADate(result2);
								flag3 = result.Month == ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth && Math.Abs(result.Year - ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear) <= 1;
							}
							catch (Exception ex)
							{
								string message = ex.Message;
								flag3 = false;
							}
						}
						if ((bool)(flag3 && (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2 != null && decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3)) || ((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2 != null && decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3)))))
						{
							bool flag4 = false;
							foreach (object item in columnNumbers)
							{
								if ((bool)((dynamic)((Range)(dynamic)worksheet.Cells[i, item]).Value2 != null && !string.IsNullOrEmpty(((dynamic)((Range)(dynamic)worksheet.Cells[i, item]).Value2).ToString())))
								{
									flag4 = true;
									break;
								}
							}
							flag2 = flag4;
						}
					}
				}
				if (flag2)
				{
					arg = "Description";
					empty = GetDescriptionByColumns(worksheet, i, columnNumbers);
					arg = "Date";
					if (result2 >= 0.0)
					{
						result = DateTime.FromOADate(result2);
					}
					if (ReCrossEntities.CurrentContext.DataFilters.CurrentYear != result.Year)
					{
						while (ReCrossEntities.CurrentContext.DataFilters.CurrentYear != result.Year)
						{
							result = result.AddYears((ReCrossEntities.CurrentContext.DataFilters.CurrentYear > result.Year) ? 1 : (-1));
						}
					}
					if (result.Month != ReCrossEntities.CurrentContext.DataFilters.CurrentMonth || result.Year != ReCrossEntities.CurrentContext.DataFilters.CurrentYear)
					{
						MessageBox.Show("Mês e/ou ano de importação diferente do(s) seleccionado(s) nos filtros.", "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
						application.Quit();
						return;
					}
					arg = "Debit/Credit";
					if (text2 == text)
					{
						if (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2 != null) && (decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3) ? true : false))
						{
							if (string.IsNullOrEmpty(color2) && string.IsNullOrEmpty(color))
							{
								if (string.IsNullOrEmpty(text4))
								{
									if (result3 <= 0m)
									{
										creditAmount = Math.Abs(result3);
									}
									else
									{
										debitAmount = -result3;
									}
								}
								else if (text4 == "D" || text4 == "-")
								{
									debitAmount = -Math.Abs(result3);
								}
								else
								{
									creditAmount = Math.Abs(result3);
								}
							}
							else
							{
								Interior interior = ((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Interior;
								Microsoft.Office.Interop.Excel.Font font = ((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Font;
								Color color4 = ColorTranslator.FromOle((int)(dynamic)font.Color);
								if (color4.Name == color)
								{
									debitAmount = -Math.Abs(result3);
								}
								else if (color4.Name == color2)
								{
									creditAmount = Math.Abs(result3);
								}
								else
								{
									Color color5 = ColorTranslator.FromOle((int)(dynamic)interior.Color);
									if (color5.Name == color)
									{
										debitAmount = -Math.Abs(result3);
									}
									else if (color5.Name == color2)
									{
										creditAmount = Math.Abs(result3);
									}
								}
							}
						}
					}
					else
					{
						if (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2 != null) && (decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3) ? true : false))
						{
							debitAmount = -Math.Abs(result3);
						}
						if (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2 != null) && (decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3) ? true : false))
						{
							creditAmount = Math.Abs(result3);
						}
					}
					arg = "Balance";
					string text7 = null;
					if (obj4 == null && obj5 == null)
					{
						text7 = ((dynamic)((Range)(dynamic)worksheet.Cells[i, obj6]).Value2).ToString();
						if (decimal.TryParse(text7, NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3))
						{
							if (!string.IsNullOrEmpty(color3))
							{
								Interior interior = ((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Interior;
								Microsoft.Office.Interop.Excel.Font font = ((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Font;
								if (ColorTranslator.FromOle((int)(dynamic)font.Color).Name == color3)
								{
									num2 = -Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
								else
								{
									num2 = Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
							}
							else if (!string.IsNullOrEmpty(text5))
							{
								if (text5 == "C" || text5 == "+")
								{
									num2 = -Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
								else
								{
									num2 = Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
							}
							else
							{
								num2 = ((result3 > 0m) ? result3 : (-Math.Abs(result3)));
								if (!flag)
								{
									num = num2;
								}
							}
						}
					}
					else
					{
						bool flag5 = false;
						object value = ((Range)(dynamic)worksheet.Cells[i, obj4]).Value2;
						if (value != null)
						{
							text7 = value.ToString();
							if (text7 != string.Empty && decimal.TryParse(text7, NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3))
							{
								flag5 = true;
								num2 = Math.Abs(result3);
								if (!flag)
								{
									num = num2;
								}
							}
						}
						if (!flag5)
						{
							value = ((Range)(dynamic)worksheet.Cells[i, obj5]).Value2;
							if (value != null)
							{
								text7 = value.ToString();
								if (text7 != string.Empty && decimal.TryParse(text7, NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3))
								{
									flag5 = true;
									num2 = -Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
							}
						}
					}
					if ((creditAmount.HasValue || debitAmount.HasValue) && result.ToShortDateString() != DateTime.Today.ToShortDateString())
					{
						AccountMovement accountMovement = AccountMovement.CreateAccountMovement(-1L, empty, result, imported: true, result);
						if (creditAmount.HasValue && creditAmount.Value > 0m)
						{
							accountMovement.CreditAmount = creditAmount;
						}
						else
						{
							accountMovement.DebitAmount = debitAmount;
						}
						accountMovement.BankAccount = bankAccount;
						list.Add(accountMovement);
						num3 = 0;
						flag = true;
					}
					else
					{
						num3++;
					}
				}
				else
				{
					num3++;
				}
				if (num3 == 30 && flag)
				{
					break;
				}
				if (num3 > 30)
				{
					MessageBox.Show("Após 30 linhas de processamento não foram encontrados movimentos para a parametrização corrente.\n\nPor favor, verifique parametrização ou ficheiro a importar (ex.: se é do tipo contabilístico).", "", MessageBoxButton.OK, MessageBoxImage.Hand);
					application.Quit();
					return;
				}
			}
		}
		catch (Exception ex)
		{
			application.Quit();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Erro na linha: {0} quando tentava obter o campo: {1}", i, arg);
			stringBuilder.AppendLine();
			stringBuilder.Append(ex.Message + "\n\n" + ex.StackTrace);
			MessageBox.Show(stringBuilder.ToString(), "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
			return;
		}
		if (list.Count > 0)
		{
			if (deleteImported)
			{
				IQueryable<AccountMovement> queryable = from mov in ReCrossApp.Current.CurrentDataContext.Movements.OfType<AccountMovement>()
					where mov.BankAccount.Id == ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id && mov.AccountDate <= ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate && mov.AccountDate >= ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMinDate
					select mov;
				if (queryable != null)
				{
					List<AccountMovement> list2 = queryable.ToList();
					foreach (AccountMovement item2 in list2)
					{
						ReCrossApp.Current.CurrentDataContext.DeleteObject(item2);
					}
				}
			}
			foreach (AccountMovement item3 in list)
			{
				ReCrossApp.Current.CurrentDataContext.AddToMovements(item3);
			}
			try
			{
				ReCrossApp.Current.CurrentDataContext.SaveChanges();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
			}
			ManageReconciliationData(isAccountMovementType: true, (list == null || list.Count <= 1) ? num2 : ((list[0].ValueDate <= list[list.Count - 1].ValueDate) ? num2 : num));
			try
			{
				ReCrossApp.Current.CurrentDataContext.SaveChanges();
				ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
			}
		}
		application.Quit();
	}

	public static void ImportExcellBankMovements(string fileName, BankAccount bankAccount, MovementParam param, bool deleteImported)
	{
		decimal? num = 0m;
		decimal? num2 = 0m;
		Microsoft.Office.Interop.Excel.Application application = (Microsoft.Office.Interop.Excel.Application)Activator.CreateInstance(Type.GetTypeFromCLSID(new Guid("00024500-0000-0000-C000-000000000046")));
		Workbook workbook = application.Workbooks.Open(fileName, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value, Missing.Value);
		Worksheet worksheet = (Worksheet)(dynamic)workbook.ActiveSheet;
		object obj = 1;
		List<object> columnNumbers = GetColumnNumbers(param.Description);
		object columnIndex = GetColumnNumber(param.Date);
		string text = param.Debit;
		string text2 = param.Credit;
		object obj2 = null;
		if (param.Credit == param.Debit)
		{
			string[] array = param.Credit.Split('&');
			string text3 = ((array != null && array.Length == 2) ? array[1] : null);
			text = (text2 = ((array != null && array.Length == 2) ? array[0] : param.Credit));
			if (!string.IsNullOrEmpty(text3))
			{
				obj2 = GetColumnNumber(text3);
			}
		}
		string color = string.Empty;
		object columnIndex2 = GetColumnNumber(text, ref color);
		string color2 = string.Empty;
		object columnIndex3 = GetColumnNumber(text2, ref color2);
		object obj3 = null;
		string column = param.Balance;
		object obj4 = null;
		object obj5 = null;
		if (param.Balance.IndexOf('&') > 0)
		{
			string[] array = param.Balance.Split('&');
			string text3 = ((array != null && array.Length == 2) ? array[1] : null);
			column = ((array != null && array.Length == 2) ? array[0] : param.Balance);
			if (!string.IsNullOrEmpty(text3))
			{
				obj3 = GetColumnNumber(text3);
			}
		}
		else if (param.Balance.IndexOf('+') > 0)
		{
			string[] array = param.Balance.Split('+');
			if (array != null && array.Length == 2)
			{
				obj4 = GetColumnNumber(array[0]);
				obj5 = GetColumnNumber(array[1]);
			}
		}
		string color3 = string.Empty;
		object obj6 = null;
		if (obj4 == null && obj5 == null)
		{
			obj6 = GetColumnNumber(column, ref color3);
		}
		if (!string.IsNullOrEmpty(color2))
		{
			string[] array = param.Credit.Split('_');
			text2 = ((array != null && array.Length == 2) ? array[0] : param.Credit);
		}
		if (!string.IsNullOrEmpty(color))
		{
			string[] array = param.Debit.Split('_');
			text = ((array != null && array.Length == 2) ? array[0] : param.Debit);
		}
		List<BankMovement> list = new List<BankMovement>();
		int i = int.Parse(obj.ToString());
		string arg = string.Empty;
		try
		{
			bool flag = false;
			int num3 = 0;
			for (i = int.Parse(obj.ToString()); i < worksheet.Cells.Rows.Count; i++)
			{
				arg = "Initialization";
				string empty = string.Empty;
				DateTime result = default(DateTime);
				string text4 = null;
				decimal? debitAmount = null;
				decimal? creditAmount = null;
				double result2 = -1.0;
				string text5 = null;
				bool flag2 = false;
				decimal result3;
				if ((bool)(columnNumbers.Count > 0 && (obj2 == null || ExcellImporter.CellHasValidValueForDebitOrCredit((dynamic)((Range)(dynamic)worksheet.Cells[i, obj2]).Value2, ref text4)) && (obj3 == null || ExcellImporter.CellHasValidValueForDebitOrCredit((dynamic)((Range)(dynamic)worksheet.Cells[i, obj3]).Value2, ref text5))) && (bool)((obj6 == null && ((dynamic)((Range)(dynamic)worksheet.Cells[i, obj4]).Value2 != null || (dynamic)((Range)(dynamic)worksheet.Cells[i, obj5]).Value2 != null)) || (obj6 != null && (dynamic)((Range)(dynamic)worksheet.Cells[i, obj6]).Value2 != null && ((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2 != null || (dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2 != null))))
				{
					string text6 = (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex]).Value2 != null) ? ((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex]).Value2).ToString() : null);
					if (!string.IsNullOrEmpty(text6))
					{
						bool flag3 = DateTime.TryParse(text6, CultureManager.Culture, DateTimeStyles.None, out result) || double.TryParse(text6, out result2);
						if (result2 >= 0.0)
						{
							try
							{
								result = DateTime.FromOADate(result2);
								flag3 = result.Month == ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth && Math.Abs(result.Year - ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear) <= 1;
							}
							catch (Exception ex)
							{
								string message = ex.Message;
								flag3 = false;
							}
						}
						if ((bool)(flag3 && (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2 != null && decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3)) || ((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2 != null && decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3)))))
						{
							bool flag4 = false;
							foreach (object item in columnNumbers)
							{
								if ((bool)((dynamic)((Range)(dynamic)worksheet.Cells[i, item]).Value2 != null && !string.IsNullOrEmpty(((dynamic)((Range)(dynamic)worksheet.Cells[i, item]).Value2).ToString())))
								{
									flag4 = true;
									break;
								}
							}
							flag2 = flag4;
						}
					}
				}
				if (flag2)
				{
					arg = "Description";
					empty = GetDescriptionByColumns(worksheet, i, columnNumbers);
					arg = "Date";
					if (result2 >= 0.0)
					{
						result = DateTime.FromOADate(result2);
					}
					if (ReCrossEntities.CurrentContext.DataFilters.CurrentYear != result.Year)
					{
						while (ReCrossEntities.CurrentContext.DataFilters.CurrentYear != result.Year)
						{
							result = result.AddYears((ReCrossEntities.CurrentContext.DataFilters.CurrentYear > result.Year) ? 1 : (-1));
						}
					}
					if (result.Month != ReCrossEntities.CurrentContext.DataFilters.CurrentMonth || result.Year != ReCrossEntities.CurrentContext.DataFilters.CurrentYear)
					{
						MessageBox.Show("Mês e/ou ano de importação diferente do(s) seleccionado(s) nos filtros.", "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
						application.Quit();
						return;
					}
					arg = "Debit/Credit";
					if (text2 == text)
					{
						if (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2 != null) && (decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3) ? true : false))
						{
							if (string.IsNullOrEmpty(color2) && string.IsNullOrEmpty(color))
							{
								if (string.IsNullOrEmpty(text4))
								{
									if (result3 > 0m)
									{
										creditAmount = result3;
									}
									else
									{
										debitAmount = result3;
									}
								}
								else if (text4 == "C" || text4 == "+")
								{
									creditAmount = Math.Abs(result3);
								}
								else
								{
									debitAmount = -Math.Abs(result3);
								}
							}
							else
							{
								Interior interior = ((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Interior;
								Microsoft.Office.Interop.Excel.Font font = ((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Font;
								Color color4 = ColorTranslator.FromOle((int)(dynamic)font.Color);
								if (color4.Name == color)
								{
									debitAmount = -Math.Abs(result3);
								}
								else if (color4.Name == color2)
								{
									creditAmount = Math.Abs(result3);
								}
								else
								{
									Color color5 = ColorTranslator.FromOle((int)(dynamic)interior.Color);
									if (color5.Name == color)
									{
										debitAmount = -Math.Abs(result3);
									}
									else if (color5.Name == color2)
									{
										creditAmount = Math.Abs(result3);
									}
								}
							}
						}
					}
					else
					{
						if (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2 != null) && (decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3) ? true : false))
						{
							debitAmount = -Math.Abs(result3);
						}
						if (((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2 != null) && (decimal.TryParse(((dynamic)((Range)(dynamic)worksheet.Cells[i, columnIndex3]).Value2).ToString(), NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3) ? true : false))
						{
							creditAmount = Math.Abs(result3);
						}
					}
					arg = "Bankroll";
					string text7 = null;
					if (obj4 == null && obj5 == null)
					{
						text7 = ((dynamic)((Range)(dynamic)worksheet.Cells[i, obj6]).Value2).ToString();
						if (decimal.TryParse(text7, NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3))
						{
							if (!string.IsNullOrEmpty(color3))
							{
								Interior interior = ((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Interior;
								Microsoft.Office.Interop.Excel.Font font = ((Range)(dynamic)worksheet.Cells[i, columnIndex2]).Font;
								if (ColorTranslator.FromOle((int)(dynamic)font.Color).Name == color3)
								{
									num2 = Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
								else
								{
									num2 = -Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
							}
							else if (!string.IsNullOrEmpty(text5))
							{
								if (text5 == "C" || text5 == "+")
								{
									num2 = Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
								else
								{
									num2 = -Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
							}
							else
							{
								num2 = result3;
								if (!flag)
								{
									num = num2;
								}
							}
						}
					}
					else
					{
						bool flag5 = false;
						object value = ((Range)(dynamic)worksheet.Cells[i, obj4]).Value2;
						if (value != null)
						{
							text7 = value.ToString();
							if (text7 != string.Empty && decimal.TryParse(text7, NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3))
							{
								flag5 = true;
								num2 = -Math.Abs(result3);
								if (!flag)
								{
									num = num2;
								}
							}
						}
						if (!flag5)
						{
							value = ((Range)(dynamic)worksheet.Cells[i, obj5]).Value2;
							if (value != null)
							{
								text7 = value.ToString();
								if (text7 != string.Empty && decimal.TryParse(text7, NumberStyles.Number | NumberStyles.AllowParentheses, CultureManager.Culture, out result3))
								{
									flag5 = true;
									num2 = Math.Abs(result3);
									if (!flag)
									{
										num = num2;
									}
								}
							}
						}
					}
					if ((creditAmount.HasValue || debitAmount.HasValue) && result.ToShortDateString() != DateTime.Today.ToShortDateString())
					{
						BankMovement bankMovement = BankMovement.CreateBankMovement(-1L, empty, result, imported: true, result);
						if (creditAmount.HasValue && creditAmount.Value > 0m)
						{
							bankMovement.CreditAmount = creditAmount;
						}
						else
						{
							bankMovement.DebitAmount = debitAmount;
						}
						bankMovement.BankAccount = bankAccount;
						list.Add(bankMovement);
						num3 = 0;
						flag = true;
					}
					else
					{
						num3++;
					}
				}
				else
				{
					num3++;
				}
				if (num3 == 30 && flag)
				{
					break;
				}
				if (num3 > 30)
				{
					MessageBox.Show("Após 30 linhas de processamento não foram encontrados movimentos para a parametrização corrente.\n\nPor favor, verifique parametrização ou ficheiro a importar (ex.: se é do tipo bancário).", "", MessageBoxButton.OK, MessageBoxImage.Hand);
					application.Quit();
					return;
				}
			}
		}
		catch (Exception ex)
		{
			application.Quit();
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendFormat("Erro na linha: {0} quando tentava obter o campo: {1}", i, arg);
			stringBuilder.AppendLine();
			stringBuilder.Append(ex.Message + "\n\n" + ex.StackTrace);
			MessageBox.Show(stringBuilder.ToString());
			return;
		}
		if (list.Count > 0)
		{
			if (deleteImported)
			{
				IQueryable<BankMovement> queryable = from mov in ReCrossApp.Current.CurrentDataContext.Movements.OfType<BankMovement>()
					where mov.BankAccount.Id == ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id && mov.TransactionDate <= ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate && mov.TransactionDate >= ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMinDate
					select mov;
				if (queryable != null)
				{
					List<BankMovement> list2 = queryable.ToList();
					foreach (BankMovement item2 in list2)
					{
						ReCrossApp.Current.CurrentDataContext.DeleteObject(item2);
					}
				}
			}
			foreach (BankMovement item3 in list)
			{
				ReCrossApp.Current.CurrentDataContext.AddToMovements(item3);
			}
			try
			{
				ReCrossApp.Current.CurrentDataContext.SaveChanges();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
			}
			ManageReconciliationData(isAccountMovementType: false, (list == null || list.Count <= 1) ? num2 : ((list[0].ValueDate <= list[list.Count - 1].ValueDate) ? num2 : num));
			try
			{
				ReCrossApp.Current.CurrentDataContext.SaveChanges();
				ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
			}
		}
		application.Quit();
	}

	public static void ManageReconciliationData(bool isAccountMovementType, decimal? balance)
	{
		if (balance.HasValue)
		{
			balance = Math.Round(balance.Value, 2);
		}
		long bankAccountId = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id;
		short month = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth;
		short year = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear;
		short num = 0;
		short num2 = 0;
		CompiledQueries.ReconciliationResultParams arg = new CompiledQueries.ReconciliationResultParams
		{
			bankAccount = bankAccountId
		};
		if (month == 1)
		{
			num = 12;
			num2 = (short)(year - 1);
		}
		else
		{
			num = (short)(month - 1);
			num2 = year;
		}
		IQueryable<Reconciliation> queryable = ReCrossApp.Current.CurrentDataContext.Reconciliations.Where((Reconciliation rec) => rec.BankAccount.Id == bankAccountId && rec.Month == month && rec.Year == year);
		if (queryable != null)
		{
			List<Reconciliation> list = queryable.ToList();
			foreach (Reconciliation item in list)
			{
				ReCrossApp.Current.CurrentDataContext.DeleteObject(item);
			}
		}
		arg.month = num;
		arg.year = num2;
		ReconciliationResult reconciliationResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
		if (reconciliationResult == null)
		{
			reconciliationResult = ReconciliationResult.CreateReconciliationResult(-1L, num, num2, closed: true);
			reconciliationResult.BankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount;
			ReCrossApp.Current.CurrentDataContext.AddToReconciliationResults(reconciliationResult);
		}
		arg.month = month;
		arg.year = year;
		ReconciliationResult reconciliationResult2 = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
		decimal? num3 = null;
		decimal num4 = 0m;
		decimal num5 = 0m;
		decimal value = 0m;
		CompiledQueries.MovementParams arg2 = new CompiledQueries.MovementParams
		{
			minDate = new DateTime(year, month, 1),
			maxDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
			bankAccount = bankAccountId
		};
		if (isAccountMovementType)
		{
			num3 = ((reconciliationResult2 != null) ? reconciliationResult2.ReasonBalance : balance);
			IQueryable<AccountMovement> queryable2 = CompiledQueries.GetAccountMovements(ReCrossApp.Current.CurrentDataContext, arg2);
			if (queryable2 != null && queryable2.Count() > 0)
			{
				AccountMovement[] array = queryable2.ToArray();
				foreach (AccountMovement accountMovement in array)
				{
					num5 += (accountMovement.CreditAmount.HasValue ? accountMovement.CreditAmount.Value : 0m);
					value += (accountMovement.DebitAmount.HasValue ? accountMovement.DebitAmount.Value : 0m);
				}
				if (reconciliationResult.ReasonBalance.HasValue)
				{
					num4 = reconciliationResult.ReasonBalance.Value;
					num4 = num4 - num5 + Math.Abs(value);
				}
				else
				{
					num4 = (balance.HasValue ? balance.Value : 0m);
					reconciliationResult.ReasonBalance = num4 + num5 - Math.Abs(value);
				}
			}
			if (reconciliationResult2 == null)
			{
				ReconciliationResult reconciliationResult3 = ReconciliationResult.CreateReconciliationResult(-1L, (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth, (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear, closed: false);
				reconciliationResult3.ReasonBalance = num4;
				reconciliationResult3.BankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount;
				ReCrossApp.Current.CurrentDataContext.AddToReconciliationResults(reconciliationResult3);
			}
			else
			{
				reconciliationResult2.ReasonBalance = num4;
			}
			if (!num3.HasValue || num3.Value != num4)
			{
				decimal? num7 = reconciliationResult2.ReasonBalance;
				while (num7.HasValue)
				{
					if (month == 12)
					{
						month = 1;
						year++;
					}
					else
					{
						month++;
					}
					num7 = AccountMovementViewModel.RecalculateResultReasonBalance(bankAccountId, month, year, num7.Value);
				}
			}
		}
		else
		{
			num3 = ((reconciliationResult2 != null) ? reconciliationResult2.Bankroll : balance);
			IQueryable<BankMovement> queryable3 = CompiledQueries.GetBankMovements(ReCrossApp.Current.CurrentDataContext, arg2);
			if (queryable3 != null && queryable3.Count() > 0)
			{
				BankMovement[] array2 = queryable3.ToArray();
				foreach (BankMovement bankMovement in array2)
				{
					num5 += (bankMovement.CreditAmount.HasValue ? bankMovement.CreditAmount.Value : 0m);
					value += (bankMovement.DebitAmount.HasValue ? bankMovement.DebitAmount.Value : 0m);
				}
				if (reconciliationResult.Bankroll.HasValue)
				{
					num4 = reconciliationResult.Bankroll.Value;
					num4 = num4 + num5 - Math.Abs(value);
				}
				else
				{
					num4 = (balance.HasValue ? balance.Value : 0m);
					reconciliationResult.Bankroll = num4 - num5 + Math.Abs(value);
				}
			}
			if (reconciliationResult2 == null)
			{
				ReconciliationResult reconciliationResult3 = ReconciliationResult.CreateReconciliationResult(-1L, (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth, (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear, closed: false);
				reconciliationResult3.Bankroll = num4;
				reconciliationResult3.BankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount;
				ReCrossApp.Current.CurrentDataContext.AddToReconciliationResults(reconciliationResult3);
			}
			else
			{
				reconciliationResult2.Bankroll = num4;
			}
			if (!num3.HasValue || num3.Value != num4)
			{
				decimal? num7 = reconciliationResult2.Bankroll;
				while (num7.HasValue)
				{
					if (month == 12)
					{
						month = 1;
						year++;
					}
					else
					{
						month++;
					}
					num7 = BankMovementViewModel.RecalculateResultBankroll(bankAccountId, month, year, num7.Value);
				}
			}
		}
		ReCrossApp.Current.StateData.IsLoading = false;
	}

	public static bool IsRoconciledAndClosed()
	{
		bool result = false;
		CompiledQueries.ReconciliationResultParams arg = new CompiledQueries.ReconciliationResultParams
		{
			bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id,
			month = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth,
			year = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear
		};
		ReconciliationResult reconciliationResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
		if (reconciliationResult != null && reconciliationResult.Closed)
		{
			MessageBox.Show("Não pode importar movimentos para meses reconciliados e já fechados.", "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
			result = true;
		}
		return result;
	}
}
