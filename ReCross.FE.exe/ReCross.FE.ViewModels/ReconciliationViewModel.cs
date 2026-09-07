using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Reports;
using Strills.WPF.Localization;
using Strills.WPF.UI.Commands;
using Strills.WPF.UI.Controls;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class ReconciliationViewModel : ViewModelBase<ReCrossEntities, Reconciliation, ReconciliationCollection>
{
	public const string C_Resx_ReconciliationView = "ReCross.FE.Views.ReconciliationView";

	public const string C_Msg_MovementsDontMatch = "Msg_MovementsDontMatch";

	public const string C_Msg_MovementsEmpty = "Msg_MovementsEmpty";

	public const string C_Msg_ManyToManyMovements = "Msg_ManyToManyMovements";

	public const string C_Msg_SelectMoreMovements = "Msg_SelectMoreMovements";

	public const string C_Msg_OwnMovements = "Msg_OwnMovements";

	private ListViewExtended _lVAccountMovs;

	private ListViewExtended _lVBankMovs;

	private ICommand _cmdUpdateItems;

	private RelayCommand _cmdLink;

	private RelayCommand _cmdAutoLink;

	private RelayCommand _cmdCalculate;

	private RelayCommand _cmdUncheck;

	private RelayCommand _cmdSelectAll;

	private ICommand _cmdUnlink;

	private ICommand _cmdUnlinkSelected;

	private RelayCommand _cmdReconcile;

	private RelayCommand _cmdPrint;

	private RelayCommand _cmdPrintMovements;

	private RelayCommand _cmdClose;

	private RelayCommand _cmdUnclose;

	public CollectionViewSource AccountMovCVS = new CollectionViewSource();

	public CollectionViewSource BankMovCVS = new CollectionViewSource();

	public CompiledQueries.ReconciliationResultParams ResultParams = default(CompiledQueries.ReconciliationResultParams);

	public CollectionViewSource ReconciliationCVS => CurrentCVS;

	public List<AccountMovement> SelectedAccountMovements { get; set; }

	public List<BankMovement> SelectedBankMovements { get; set; }

	protected ListCollectionView AccountMovLCV { get; set; }

	protected ListCollectionView BankMovLCV { get; set; }

	public decimal AccountDebit { get; set; }

	public decimal AccountCredit { get; set; }

	public decimal BankDebit { get; set; }

	public decimal BankCredit { get; set; }

	public NotifyBindings NotifyBindings { get; set; }

	public Reconciliation CurrentItem
	{
		get
		{
			return base.CurrentDataEntity;
		}
		set
		{
			base.CurrentDataEntity = value;
		}
	}

	public ReconciliationResult CurrentResult { get; set; }

	public ReconciliationResult PreviousResult { get; set; }

	public ReconciliationResult NextResult { get; set; }

	public ICommand CMDUpdateItems => _cmdUpdateItems;

	public ICommand CMDLink => _cmdLink;

	public ICommand CMDAutoLink => _cmdAutoLink;

	public ICommand CMDUncheck => _cmdUncheck;

	public ICommand CMDCalculate => _cmdCalculate;

	public ICommand CMDSelectAll => _cmdSelectAll;

	public ICommand CMDUnlink => _cmdUnlink;

	public ICommand CMDUnlinkSelected => _cmdUnlinkSelected;

	public ICommand CMDReconcile => _cmdReconcile;

	public ICommand CMDPrint => _cmdPrint;

	public ICommand CMDPrintMovements => _cmdPrintMovements;

	public ICommand CMDClose => _cmdClose;

	public ICommand CMDUnclose => _cmdUnclose;

	public ReconciliationViewModel()
	{
		AccountCredit = (AccountDebit = (BankCredit = (BankCredit = 0m)));
		NotifyBindings = new NotifyBindings();
		SimpleCommand simpleCommand = new SimpleCommand();
		Action<object> executeDelegate = delegate(object x)
		{
			UpdateItems(x as ListViewExtended);
		};
		simpleCommand.ExecuteDelegate = executeDelegate;
		_cmdUpdateItems = simpleCommand;
		_cmdLink = new RelayCommand(delegate
		{
			Link();
		});
		_cmdAutoLink = new RelayCommand(delegate
		{
			AutoLink();
		});
		_cmdUncheck = new RelayCommand(delegate
		{
			Uncheck();
		});
		_cmdCalculate = new RelayCommand(delegate
		{
			Calculate();
		});
		_cmdSelectAll = new RelayCommand(delegate
		{
			SelectAll();
		});
		_cmdUnlink = new SimpleCommand
		{
			ExecuteDelegate = delegate(object x)
			{
				Unlink(x as ListViewItemExtended);
			}
		};
		_cmdUnlinkSelected = new SimpleCommand
		{
			ExecuteDelegate = delegate(object x)
			{
				UnlinkSelected(x as IList);
			}
		};
		_cmdReconcile = new RelayCommand(delegate
		{
			Reconcile();
		});
		_cmdPrint = new RelayCommand(delegate
		{
			Print();
		});
		_cmdPrintMovements = new RelayCommand(delegate
		{
			PrintMovements();
		});
		_cmdClose = new RelayCommand(delegate
		{
			Close();
		});
		_cmdUnclose = new RelayCommand(delegate
		{
			Unclose();
		});
		CurrentResult = new ReconciliationResult();
	}

	protected override ReconciliationCollection InitilizeDataCollection()
	{
		if (base.CurrentDataContext.Reconciliations != null && base.CurrentDataContext.Reconciliations.Count() > 0)
		{
			return new ReconciliationCollection(base.CurrentDataContext.Reconciliations);
		}
		return new ReconciliationCollection(new List<Reconciliation>());
	}

	protected override void InitilizeCustomDataCollections()
	{
		base.InitilizeCustomDataCollections();
	}

	private bool UpdateItems(ListViewExtended listView)
	{
		if (listView.Name.Contains("AccountMovs"))
		{
			_lVAccountMovs = listView;
			SelectedAccountMovements = new List<AccountMovement>();
			foreach (AccountMovement selectedItem in listView.SelectedItems)
			{
				SelectedAccountMovements.Add(selectedItem);
			}
		}
		else if (listView.Name.Contains("BankMovs"))
		{
			_lVBankMovs = listView;
			SelectedBankMovements = new List<BankMovement>();
			foreach (BankMovement selectedItem2 in listView.SelectedItems)
			{
				SelectedBankMovements.Add(selectedItem2);
			}
		}
		NotifyBindings.NotifyMovementChecked++;
		return true;
	}

	private bool Link()
	{
		decimal num = 0m;
		decimal num2 = 0m;
		bool flag = SelectedAccountMovements != null && SelectedAccountMovements.Count > 0;
		bool flag2 = SelectedBankMovements != null && SelectedBankMovements.Count > 0;
		if (!flag && !flag2)
		{
			MessageBox.Show(ResourcesManager.GetValue("ReCross.FE.Views.ReconciliationView", "Msg_MovementsEmpty"), "", MessageBoxButton.OK, MessageBoxImage.Hand);
			return false;
		}
		if (flag && SelectedAccountMovements.Count > 1 && flag2 && SelectedBankMovements.Count > 1)
		{
			MessageBoxResult messageBoxResult = MessageBox.Show(ResourcesManager.GetValue("ReCross.FE.Views.ReconciliationView", "Msg_ManyToManyMovements"), "", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
			if (messageBoxResult == MessageBoxResult.No)
			{
				return false;
			}
		}
		if ((!flag && flag2 && SelectedBankMovements.Count > 1) || (!flag2 && flag && SelectedAccountMovements.Count > 1))
		{
			MessageBoxResult messageBoxResult = MessageBox.Show(ResourcesManager.GetValue("ReCross.FE.Views.ReconciliationView", "Msg_OwnMovements"), "", MessageBoxButton.YesNo, MessageBoxImage.Exclamation);
			if (messageBoxResult == MessageBoxResult.No)
			{
				return false;
			}
		}
		else if ((!flag2 && flag2 && SelectedAccountMovements.Count == 1) || (!flag && flag && SelectedBankMovements.Count == 1))
		{
			MessageBox.Show(ResourcesManager.GetValue("ReCross.FE.Views.ReconciliationView", "Msg_SelectMoreMovements"), "", MessageBoxButton.OK, MessageBoxImage.Hand);
			return false;
		}
		if (SelectedAccountMovements != null)
		{
			foreach (AccountMovement selectedAccountMovement in SelectedAccountMovements)
			{
				num += selectedAccountMovement.Amount;
			}
		}
		if (SelectedBankMovements != null)
		{
			foreach (BankMovement selectedBankMovement in SelectedBankMovements)
			{
				num2 += selectedBankMovement.Amount;
			}
		}
		if (num + num2 == 0m)
		{
			Reconciliation reconciliation = Reconciliation.CreateReconciliation(-1L, (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth, (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear, DateTime.Today);
			reconciliation.BankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount;
			base.CurrentDataContext.AddToReconciliations(reconciliation);
			try
			{
				base.CurrentDataContext.SaveChanges();
				bool flag3 = false;
				bool flag4 = false;
				if (SelectedAccountMovements != null)
				{
					foreach (AccountMovement selectedAccountMovement2 in SelectedAccountMovements)
					{
						AccountMovLCV.EditItem(selectedAccountMovement2);
						((AccountMovement)AccountMovLCV.CurrentEditItem).Reconciliation = reconciliation;
						AccountMovLCV.CommitEdit();
					}
					flag3 = SelectedAccountMovements.Count > 0;
				}
				if (SelectedBankMovements != null)
				{
					foreach (BankMovement selectedBankMovement2 in SelectedBankMovements)
					{
						BankMovLCV.EditItem(selectedBankMovement2);
						((BankMovement)BankMovLCV.CurrentEditItem).Reconciliation = reconciliation;
						BankMovLCV.CommitEdit();
					}
					flag4 = SelectedBankMovements.Count > 0;
				}
				try
				{
					base.CurrentDataContext.SaveChanges();
					NotifyBindings.NotifyMovementChecked = 0;
					CompiledQueries.MovementParams arg = new CompiledQueries.MovementParams
					{
						maxDate = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate,
						bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id
					};
					if (flag3)
					{
						IQueryable<AccountMovement> queryable = CompiledQueries.GetReconcilingAccountMovs(ReCrossApp.Current.CurrentDataContext, arg);
						SetAccountMovs((ObjectQuery<AccountMovement>)queryable);
					}
					if (flag4)
					{
						IQueryable<BankMovement> queryable2 = CompiledQueries.GetReconcilingBankMovs(ReCrossApp.Current.CurrentDataContext, arg);
						SetBankMovs((ObjectQuery<BankMovement>)queryable2);
					}
					NotifyBindings.NotifyReconciliation++;
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
			if (SelectedAccountMovements != null)
			{
				foreach (AccountMovement selectedAccountMovement3 in SelectedAccountMovements)
				{
					AccountMovLCV.Remove(selectedAccountMovement3);
				}
				SelectedAccountMovements.Clear();
			}
			if (SelectedBankMovements != null)
			{
				foreach (BankMovement selectedBankMovement3 in SelectedBankMovements)
				{
					AccountMovLCV.Remove(selectedBankMovement3);
				}
				SelectedBankMovements.Clear();
			}
			return true;
		}
		MessageBox.Show(ResourcesManager.GetValue("ReCross.FE.Views.ReconciliationView", "Msg_MovementsDontMatch"), "", MessageBoxButton.OK, MessageBoxImage.Hand);
		return false;
	}

	private bool AutoLink()
	{
		bool result = false;
		MessageBoxResult messageBoxResult = MessageBox.Show("Tem a certeza que deseja ligar movimentos contabilísticos com movimentos bancários automaticamente por valor (1 para 1)?", "Questão", MessageBoxButton.YesNo, MessageBoxImage.Question);
		if (messageBoxResult == MessageBoxResult.Yes)
		{
			Dictionary<decimal, AccountMovement> dictionary = new Dictionary<decimal, AccountMovement>();
			Dictionary<decimal, BankMovement> dictionary2 = new Dictionary<decimal, BankMovement>();
			List<decimal> list = new List<decimal>();
			List<decimal> list2 = new List<decimal>();
			foreach (AccountMovement item in (IEnumerable)AccountMovLCV)
			{
				if (!list.Contains(item.Amount))
				{
					if (dictionary.ContainsKey(item.Amount))
					{
						dictionary.Remove(item.Amount);
						list.Add(item.Amount);
					}
					else
					{
						dictionary.Add(item.Amount, item);
					}
				}
			}
			list.Clear();
			foreach (BankMovement item2 in (IEnumerable)BankMovLCV)
			{
				if (dictionary.ContainsKey(-item2.Amount) && !list2.Contains(item2.Amount))
				{
					if (dictionary2.ContainsKey(item2.Amount))
					{
						dictionary2.Remove(item2.Amount);
						list2.Add(item2.Amount);
					}
					else
					{
						dictionary2.Add(item2.Amount, item2);
					}
				}
			}
			list2.Clear();
			List<decimal> list3 = new List<decimal>(dictionary.Keys);
			foreach (decimal item3 in list3)
			{
				if (!dictionary2.ContainsKey(-item3))
				{
					dictionary.Remove(item3);
				}
			}
			if (dictionary.Count > 0 && dictionary.Count == dictionary2.Count)
			{
				foreach (decimal key in dictionary.Keys)
				{
					Reconciliation reconciliation = Reconciliation.CreateReconciliation(-1L, (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth, (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear, DateTime.Today);
					reconciliation.BankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount;
					base.CurrentDataContext.AddToReconciliations(reconciliation);
					try
					{
						base.CurrentDataContext.SaveChanges();
						AccountMovLCV.EditItem(dictionary[key]);
						((AccountMovement)AccountMovLCV.CurrentEditItem).Reconciliation = reconciliation;
						AccountMovLCV.CommitEdit();
						BankMovLCV.EditItem(dictionary2[-key]);
						((BankMovement)BankMovLCV.CurrentEditItem).Reconciliation = reconciliation;
						BankMovLCV.CommitEdit();
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.ToString());
					}
				}
				try
				{
					base.CurrentDataContext.SaveChanges();
					NotifyBindings.NotifyMovementChecked = 0;
					CompiledQueries.MovementParams arg = new CompiledQueries.MovementParams
					{
						maxDate = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate,
						bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id
					};
					if (dictionary.Count > 0)
					{
						IQueryable<AccountMovement> queryable = CompiledQueries.GetReconcilingAccountMovs(ReCrossApp.Current.CurrentDataContext, arg);
						SetAccountMovs((ObjectQuery<AccountMovement>)queryable);
					}
					if (dictionary2.Count > 0)
					{
						IQueryable<BankMovement> queryable2 = CompiledQueries.GetReconcilingBankMovs(ReCrossApp.Current.CurrentDataContext, arg);
						SetBankMovs((ObjectQuery<BankMovement>)queryable2);
					}
					NotifyBindings.NotifyReconciliation++;
					MessageBox.Show("Foram reconciliados " + dictionary.Count + " movimentos automaticamente.", "Informação", MessageBoxButton.OK, MessageBoxImage.Asterisk);
				}
				catch (Exception ex)
				{
					MessageBox.Show(ex.ToString());
				}
			}
			else
			{
				MessageBox.Show("Não existem movimentos unívocos por valor (1 para 1) que possam ser reconciliados automaticamente.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
			}
			if (SelectedAccountMovements != null)
			{
				SelectedAccountMovements.Clear();
			}
			if (SelectedBankMovements != null)
			{
				SelectedBankMovements.Clear();
			}
			return true;
		}
		return result;
	}

	private bool Uncheck()
	{
		if (_lVAccountMovs != null)
		{
			_lVAccountMovs.UnselectAll();
		}
		if (_lVBankMovs != null)
		{
			_lVBankMovs.UnselectAll();
		}
		if (SelectedAccountMovements != null)
		{
			SelectedAccountMovements.Clear();
		}
		if (SelectedBankMovements != null)
		{
			SelectedBankMovements.Clear();
		}
		NotifyBindings.NotifyMovementChecked = 0;
		return true;
	}

	private bool Calculate()
	{
		ReconciliationViewModel viewModel = this;
		SetReconciliationResults(ref viewModel);
		decimal? num = viewModel.CurrentResult.ReasonBalance + (decimal?)viewModel.AccountCredit - (decimal?)viewModel.AccountDebit + (decimal?)viewModel.BankCredit - (decimal?)viewModel.BankDebit - viewModel.CurrentResult.Bankroll;
		if (num.HasValue)
		{
			NumberFormatInfo numberFormatInfo = (NumberFormatInfo)CultureManager.Culture.NumberFormat.Clone();
			numberFormatInfo.CurrencySymbol = string.Empty;
			MessageBox.Show("Saldo Razão + Soma algébrica dos créditos e débitos - Saldo Bancário = " + string.Format(numberFormatInfo, "{0:C2}", new object[1] { num.Value }), "Calculo", MessageBoxButton.OK, MessageBoxImage.Asterisk);
		}
		return true;
	}

	private bool SelectAll()
	{
		return true;
	}

	private bool Unlink(ListViewItemExtended listViewItem)
	{
		if (CurrentResult == null || !CurrentResult.Closed)
		{
			Reconciliation entity = (Reconciliation)listViewItem.DataContext;
			base.CurrentDataContext.DeleteObject(entity);
			base.CurrentDataContext.SaveChanges();
			NotifyBindings.NotifyAccountMov++;
			NotifyBindings.NotifyBankMov++;
			NotifyBindings.NotifyReconciliation++;
		}
		else
		{
			MessageBox.Show("Não se pode desfazer ligação de movimento de contabilidade com bancário porque a reconciliação está fechada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		return true;
	}

	private bool UnlinkSelected(IList selectedItems)
	{
		if (CurrentResult == null || !CurrentResult.Closed)
		{
			if (selectedItems != null && selectedItems.Count > 0)
			{
				foreach (Reconciliation selectedItem in selectedItems)
				{
					base.CurrentDataContext.DeleteObject(selectedItem);
				}
				base.CurrentDataContext.SaveChanges();
				NotifyBindings.NotifyAccountMov++;
				NotifyBindings.NotifyBankMov++;
				NotifyBindings.NotifyReconciliation++;
			}
		}
		else
		{
			MessageBox.Show("Não se pode desfazer ligação de movimento de contabilidade com bancário porque a reconciliação está fechada.", "Aviso", MessageBoxButton.OK, MessageBoxImage.Exclamation);
		}
		return true;
	}

	private bool Reconcile()
	{
		return true;
	}

	private bool Print()
	{
		ReportWindow reportWindow = new ReportWindow(saved: true);
		reportWindow.ReportType = ReportType.Reconciliation;
		reportWindow.Show();
		return true;
	}

	private bool PrintMovements()
	{
		ReportWindow reportWindow = new ReportWindow();
		reportWindow.ReportType = ReportType.ReconciledMovements;
		reportWindow.Show();
		return true;
	}

	private bool Close()
	{
		ReportWindow reportWindow = new ReportWindow();
		reportWindow.ReportType = ReportType.Reconciliation;
		reportWindow.Show();
		try
		{
			string fileName = reportWindow.SaveDocument(ReportType.Reconciliation);
			CurrentResult.BeginEdit();
			CurrentResult.Closed = true;
			CurrentResult.FileName = fileName;
			CurrentResult.EndEdit();
			if (CurrentResult.CanBeSaved)
			{
				base.CurrentDataContext.SaveChanges();
				ReCrossEntities.CurrentContext.DataFilters.HasDataSelected = true;
			}
		}
		catch
		{
			MessageBox.Show("RECONILIAÇÃO NÃO FOI FECHADA!");
		}
		return true;
	}

	private bool Unclose()
	{
		MessageBoxResult messageBoxResult = MessageBox.Show("Tem a certeza que deseja continuar?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
		if (messageBoxResult == MessageBoxResult.Yes)
		{
			CurrentResult.BeginEdit();
			CurrentResult.Closed = false;
			CurrentResult.FileName = null;
			CurrentResult.EndEdit();
			if (CurrentResult.CanBeSaved)
			{
				base.CurrentDataContext.SaveChanges();
				MessageBoxResult messageBoxResult2 = MessageBox.Show("Deseja também desfazer todas ligações de movimentos contabilísticos com movimentos bancários de meses anteriores?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (messageBoxResult2 == MessageBoxResult.Yes)
				{
					CompiledQueries.ReconciliationResultParams arg = new CompiledQueries.ReconciliationResultParams
					{
						month = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth,
						year = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear,
						bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id
					};
					IQueryable<Reconciliation> queryable = CompiledQueries.GetReconciliations(ReCrossApp.Current.CurrentDataContext, arg);
					if (queryable != null && queryable.Count() > 0)
					{
						List<Reconciliation> list = new List<Reconciliation>();
						foreach (Reconciliation item in queryable)
						{
							if (item.Movements == null || item.Movements.Count <= 0)
							{
								continue;
							}
							foreach (Movement movement in item.Movements)
							{
								if ((short)movement.ValueDate.Year < arg.year || (short)movement.ValueDate.Month < arg.month)
								{
									list.Add(item);
									break;
								}
							}
						}
						UnlinkSelected(list.ToList());
					}
				}
				ReCrossEntities.CurrentContext.DataFilters.HasDataSelected = true;
			}
		}
		return true;
	}

	public void SetReconciliations(ObjectQuery<Reconciliation> reconciliations)
	{
		CurrentCVS.Source = reconciliations?.ToList();
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
	}

	public void SetAccountMovs(ObjectQuery<AccountMovement> movements)
	{
		AccountMovCVS.Source = movements?.ToList();
		AccountMovLCV = (ListCollectionView)AccountMovCVS.View;
		NotifyBindings.NotifyAccountMovSum++;
	}

	public void SetBankMovs(ObjectQuery<BankMovement> movements)
	{
		BankMovCVS.Source = movements?.ToList();
		BankMovLCV = (ListCollectionView)BankMovCVS.View;
		NotifyBindings.NotifyBankMovSum++;
	}

	public static void SetReconciliationResults(ref ReconciliationViewModel viewModel)
	{
		if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount == null)
		{
			return;
		}
		if (viewModel.CurrentResult == null || viewModel.CurrentResult.Month != ReCrossEntities.CurrentContext.DataFilters.CurrentMonth || viewModel.CurrentResult.Year == ReCrossEntities.CurrentContext.DataFilters.CurrentYear)
		{
			if (viewModel.ResultParams.month != (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth || viewModel.ResultParams.year != (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear || viewModel.ResultParams.bankAccount != ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id)
			{
				viewModel.ResultParams.month = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth;
				viewModel.ResultParams.year = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear;
				viewModel.ResultParams.bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id;
				ReconciliationResult reconciliationResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, viewModel.ResultParams);
				viewModel.CurrentResult = ((reconciliationResult != null) ? reconciliationResult : new ReconciliationResult());
				if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth == 1)
				{
					viewModel.ResultParams.month = 12;
					viewModel.ResultParams.year = (short)(ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear - 1);
				}
				else
				{
					viewModel.ResultParams.month = (short)(ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth - 1);
					viewModel.ResultParams.year = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear;
				}
				viewModel.PreviousResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, viewModel.ResultParams);
				if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth == 12)
				{
					viewModel.ResultParams.month = 1;
					viewModel.ResultParams.year = (short)(ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear + 1);
				}
				else
				{
					viewModel.ResultParams.month = (short)(ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth + 1);
					viewModel.ResultParams.year = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear;
				}
				viewModel.NextResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, viewModel.ResultParams);
				viewModel.ResultParams.month = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth;
				viewModel.ResultParams.year = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear;
				viewModel.ResultParams.bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id;
			}
		}
		else
		{
			viewModel.CurrentResult = new ReconciliationResult();
		}
	}
}
