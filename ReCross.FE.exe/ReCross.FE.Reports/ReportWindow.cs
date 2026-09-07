using System;
using System.Data;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Xps.Packaging;
using CodeReason.Reports;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Properties;
using Strills.WPF.Localization;
using Strills.WPF.Reports;

namespace ReCross.FE.Reports;

public partial class ReportWindow : Window, IComponentConnector
{
	private bool _savedAlready;

	private DateTime _now;

	private bool _firstActivated = true;

	private XpsManager _manager;

	private XpsDocument _xpsDocument;

	public ReportType ReportType { get; set; }

	public ReportWindow()
	{
		InitializeComponent();
	}

	public ReportWindow(bool saved)
	{
		_savedAlready = saved;
		InitializeComponent();
	}

	private void Window_Activated(object sender, EventArgs e)
	{
		if (!_firstActivated)
		{
			return;
		}
		_firstActivated = false;
		try
		{
			ReconciliationResult reconciliationResult = null;
			CompiledQueries.ReconciliationResultParams arg = new CompiledQueries.ReconciliationResultParams
			{
				bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id,
				month = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth,
				year = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear
			};
			if (!_savedAlready)
			{
				_manager = new XpsManager();
				string fullyQualifiedName = Assembly.GetExecutingAssembly().GetModules()[0].FullyQualifiedName;
				string directoryName = Path.GetDirectoryName(fullyQualifiedName);
				ReportDocument reportDocument = new ReportDocument();
				StreamReader streamReader = null;
				ReportData reportData = new ReportData();
				DataTable dataTable = null;
				decimal num = 0m;
				decimal num2 = 0m;
				StringBuilder stringBuilder = new StringBuilder();
				reportData.ReportDocumentValues.Add("ReportName", ReCrossEntities.CurrentContext.DataFilters.CurrentEntity.Description);
				_now = DateTime.Now;
				reportData.ReportDocumentValues.Add("CompanyName", Settings.Default.CompanyName + " (" + ReCrossApp.Current.UserFullName + ")");
				reportData.ReportDocumentValues.Add("PrintDate", _now);
				switch (ReportType)
				{
				case ReportType.AccountMovement:
				case ReportType.BankMovement:
					streamReader = new StreamReader(new FileStream(directoryName + "\\Reports\\Templates\\Movements.xaml", FileMode.Open, FileAccess.Read));
					stringBuilder.AppendFormat("Mês {0}/{1}", ReCrossEntities.CurrentContext.DataFilters.CurrentMonth, ReCrossEntities.CurrentContext.DataFilters.CurrentYear);
					reportData.ReportDocumentValues.Add("MovementsDate", stringBuilder.ToString());
					reportData.ReportDocumentValues.Add("BankAccount", "- Conta: " + ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.AccountNumber + " (" + ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankEntity.Abbreviation + ")");
					reconciliationResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
					dataTable = new DataTable("Movement");
					dataTable.Columns.Add("Date", typeof(string));
					dataTable.Columns.Add("Credit", typeof(decimal));
					dataTable.Columns.Add("Debit", typeof(decimal));
					dataTable.Columns.Add("Description", typeof(string));
					break;
				case ReportType.Reconciliation:
					streamReader = new StreamReader(new FileStream(directoryName + "\\Reports\\Templates\\Reconciliation.xaml", FileMode.Open, FileAccess.Read));
					stringBuilder = new StringBuilder();
					stringBuilder.AppendFormat("Mês {0}/{1}", ReCrossEntities.CurrentContext.DataFilters.CurrentMonth, ReCrossEntities.CurrentContext.DataFilters.CurrentYear);
					reportData.ReportDocumentValues.Add("ReconciliationDate", stringBuilder.ToString());
					reportData.ReportDocumentValues.Add("BankAccount", "- Conta: " + ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.AccountNumber + " (" + ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankEntity.Abbreviation + ")");
					reconciliationResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
					dataTable = new DataTable("Reconciliation");
					dataTable.Columns.Add("Date", typeof(string));
					dataTable.Columns.Add("Description", typeof(string));
					dataTable.Columns.Add("AccountDebit", typeof(decimal));
					dataTable.Columns.Add("AccountCredit", typeof(decimal));
					dataTable.Columns.Add("BankDebit", typeof(decimal));
					dataTable.Columns.Add("BankCredit", typeof(decimal));
					break;
				case ReportType.ReconciledMovements:
					streamReader = new StreamReader(new FileStream(directoryName + "\\Reports\\Templates\\ReconciledMovements.xaml", FileMode.Open, FileAccess.Read));
					stringBuilder = new StringBuilder();
					stringBuilder.AppendFormat("Mês {0}/{1}", ReCrossEntities.CurrentContext.DataFilters.CurrentMonth, ReCrossEntities.CurrentContext.DataFilters.CurrentYear);
					reportData.ReportDocumentValues.Add("ReconciliationDate", stringBuilder.ToString());
					reportData.ReportDocumentValues.Add("BankAccount", "- Conta: " + ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.AccountNumber + " (" + ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankEntity.Abbreviation + ")");
					reconciliationResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
					dataTable = new DataTable("Reconciliation");
					dataTable.Columns.Add("AccountMovementsGroup", typeof(string));
					dataTable.Columns.Add("BankMovementsGroup", typeof(string));
					break;
				}
				reportDocument.XamlData = streamReader.ReadToEnd();
				reportDocument.XamlImagePath = Path.Combine(Environment.CurrentDirectory, "Templates\\");
				streamReader.Close();
				CompiledQueries.MovementParams arg2 = new CompiledQueries.MovementParams
				{
					minDate = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMinDate,
					maxDate = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate,
					bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id
				};
				switch (ReportType)
				{
				case ReportType.AccountMovement:
				{
					reportData.ReportDocumentValues.Add("ReportTitle", "Movimentos Contabilísticos");
					if (reconciliationResult != null)
					{
						num = (reconciliationResult.ReasonBalance.HasValue ? reconciliationResult.ReasonBalance.Value : 0m);
					}
					reportData.ReportDocumentValues.Add("Balance", num);
					IQueryable<AccountMovement> queryable3 = CompiledQueries.GetAccountMovements(ReCrossApp.Current.CurrentDataContext, arg2);
					foreach (AccountMovement item in queryable3)
					{
						dataTable.Rows.Add(item.AccountDate.ToShortDateString(), item.DebitAmount.HasValue ? Math.Abs(item.DebitAmount.Value) : 0m, item.CreditAmount.HasValue ? Math.Abs(item.CreditAmount.Value) : 0m, item.Description);
					}
					break;
				}
				case ReportType.BankMovement:
				{
					reportData.ReportDocumentValues.Add("ReportTitle", "Movimentos Bancários");
					if (reconciliationResult != null)
					{
						num = (reconciliationResult.Bankroll.HasValue ? reconciliationResult.Bankroll.Value : 0m);
					}
					reportData.ReportDocumentValues.Add("Balance", num);
					IQueryable<BankMovement> queryable2 = CompiledQueries.GetBankMovements(ReCrossApp.Current.CurrentDataContext, arg2);
					foreach (BankMovement item2 in queryable2)
					{
						dataTable.Rows.Add(item2.TransactionDate.ToShortDateString(), item2.DebitAmount.HasValue ? Math.Abs(item2.DebitAmount.Value) : 0m, item2.CreditAmount.HasValue ? Math.Abs(item2.CreditAmount.Value) : 0m, item2.Description);
					}
					break;
				}
				case ReportType.Reconciliation:
				{
					reportData.ReportDocumentValues.Add("ReportTitle", "Reconciliação");
					if (reconciliationResult != null)
					{
						num = (reconciliationResult.ReasonBalance.HasValue ? reconciliationResult.ReasonBalance.Value : 0m);
						num2 = (reconciliationResult.Bankroll.HasValue ? reconciliationResult.Bankroll.Value : 0m);
					}
					reportData.ReportDocumentValues.Add("ReasonBalance", num);
					reportData.ReportDocumentValues.Add("Bankroll", num2);
					IOrderedQueryable<Movement> orderedQueryable = from mov in ReCrossApp.Current.CurrentDataContext.Movements
						where mov.ValueDate <= ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate && mov.BankAccount.Id == ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id && mov.Reconciliation == null
						orderby mov.ValueDate
						select mov;
					foreach (Movement item3 in orderedQueryable)
					{
						if (item3 is AccountMovement)
						{
							AccountMovement accountMovement = (AccountMovement)item3;
							dataTable.Rows.Add(accountMovement.AccountDate.ToShortDateString(), accountMovement.Description, accountMovement.DebitAmount.HasValue ? Math.Abs(accountMovement.DebitAmount.Value) : 0m, accountMovement.CreditAmount.HasValue ? Math.Abs(accountMovement.CreditAmount.Value) : 0m, 0m, 0m);
						}
						else if (item3 is BankMovement)
						{
							BankMovement bankMovement = (BankMovement)item3;
							dataTable.Rows.Add(bankMovement.TransactionDate.ToShortDateString(), bankMovement.Description, 0m, 0m, bankMovement.DebitAmount.HasValue ? Math.Abs(bankMovement.DebitAmount.Value) : 0m, bankMovement.CreditAmount.HasValue ? Math.Abs(bankMovement.CreditAmount.Value) : 0m);
						}
					}
					break;
				}
				case ReportType.ReconciledMovements:
				{
					reportData.ReportDocumentValues.Add("ReportTitle", "Movimentos Reconciliados");
					IQueryable<Reconciliation> queryable = CompiledQueries.GetReconciliations(ReCrossApp.Current.CurrentDataContext, arg);
					if (queryable == null || queryable.Count() <= 0)
					{
						break;
					}
					foreach (Reconciliation item4 in queryable)
					{
						dataTable.Rows.Add(item4.AccountMovementAmmountsWithDescrGroup, item4.BankMovementAmmountsWithDescrGroup);
					}
					break;
				}
				}
				reportData.DataTables.Add(dataTable);
				DateTime now = DateTime.Now;
				object title = base.Title;
				base.Title = string.Concat(title, " - generated in ", (DateTime.Now - now).TotalMilliseconds, "ms");
				_xpsDocument = reportDocument.CreateXpsDocument(reportData);
			}
			else
			{
				string text = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg)?.FileName;
				if (string.IsNullOrEmpty(text))
				{
					MessageBox.Show("Reconciliação não tem nome de ficheiro associado para abrir.", "", MessageBoxButton.OK, MessageBoxImage.Hand);
					return;
				}
				_xpsDocument = new XpsDocument(text, FileAccess.Read);
			}
			documentViewer.Document = _xpsDocument.GetFixedDocumentSequence();
		}
		catch (Exception ex)
		{
			MessageBox.Show(string.Concat(ex.Message, "\r\n\r\n", ex.GetType(), "\r\n", ex.StackTrace), ex.GetType().ToString(), MessageBoxButton.OK, MessageBoxImage.Hand);
		}
	}

	private string GetMonthString(int month)
	{
		return (month <= 9) ? ("0" + month) : month.ToString();
	}

	public string SaveDocument(ReportType reportType)
	{
		string text = (Settings.Default.RecDocPath.EndsWith(Path.DirectorySeparatorChar.ToString()) ? string.Empty : Path.DirectorySeparatorChar.ToString());
		string text2 = Settings.Default.RecDocPath + text + ReCrossEntities.CurrentContext.DataFilters.CurrentEntity.Abbreviation + Path.DirectorySeparatorChar;
		string text3 = string.Format(CultureManager.Culture.DateTimeFormat, "{0:d}", new object[1] { DateTime.Parse(_now.ToString(), CultureManager.Culture, DateTimeStyles.None) });
		text3 = text3 + "_" + string.Format(CultureManager.Culture.DateTimeFormat, "{0:HH_mm_ss}", new object[1] { DateTime.Parse(_now.ToString(), CultureManager.Culture, DateTimeStyles.None) });
		string text4 = ((reportType == ReportType.ReconciledMovements) ? "Mov_" : string.Empty) + ReCrossEntities.CurrentContext.DataFilters.CurrentEntity.Abbreviation + "_" + GetMonthString(ReCrossEntities.CurrentContext.DataFilters.CurrentMonth) + "_" + ReCrossEntities.CurrentContext.DataFilters.CurrentYear + "__" + text3 + ".xps";
		_manager.SaveXps(_xpsDocument, text2, text4);
		return text2 + text4;
	}
}
