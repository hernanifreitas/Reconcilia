using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using Microsoft.Win32;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Reports;
using ReCross.FE.Utils;
using Strills.WPF.UI.Commands;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class BankMovementViewModel : ViewModelBase<ReCrossEntities, BankMovement, BankMovementCollection>
{
	private RelayCommand _cmdImport;

	private RelayCommand _cmdPrint;

	private RelayCommand _cmdRemoveAll;

	public CollectionViewSource BankMovementCVS => CurrentCVS;

	public BankMovement CurrentItem
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

	public ICommand CMDImport => _cmdImport;

	public ICommand CMDPrint => _cmdPrint;

	public ICommand CMDRemoveAll => _cmdRemoveAll;

	public BankMovementViewModel()
	{
		Action<object> execute = delegate
		{
			Import();
		};
		_cmdImport = new RelayCommand(execute);
		_cmdPrint = new RelayCommand(delegate
		{
			Print();
		});
		_cmdRemoveAll = new RelayCommand(delegate
		{
			RemoveAll();
		});
	}

	private bool Import()
	{
		if (!ExcellImporter.IsRoconciledAndClosed())
		{
			if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankEntity.MovementParam == null)
			{
				MessageBox.Show("Não existe parametrização de importação para o Banco seleccionado!", "", MessageBoxButton.OK, MessageBoxImage.Hand);
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendFormat("Confirma que quer importar movimentos do cliente {0}, para o mês {1} e ano {2}?", ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity.Abbreviation, ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth, ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear);
				MessageBoxResult messageBoxResult = MessageBox.Show(stringBuilder.ToString(), "Questão", MessageBoxButton.YesNo, MessageBoxImage.Question);
				if (messageBoxResult == MessageBoxResult.Yes)
				{
					OpenFileDialog fileDialog = ExcellImporter.GetFileDialog();
					if (fileDialog.ShowDialog() == true)
					{
						try
						{
							MessageBoxResult messageBoxResult2 = MessageBox.Show("Deseja eliminar também os movimentos anteriormente para este mês?", "Questão", MessageBoxButton.YesNo, MessageBoxImage.Question);
							if (messageBoxResult2 == MessageBoxResult.Yes)
							{
								messageBoxResult2 = MessageBox.Show("Tem a certeza que deseja continuar?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
							}
							ExcellImporter.ImportExcellBankMovements(fileDialog.FileName, ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount, ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankEntity.MovementParam, messageBoxResult2 == MessageBoxResult.Yes);
						}
						catch (IOException ex)
						{
							MessageBox.Show(ex.Message);
						}
					}
				}
			}
		}
		ReCrossApp.Current.StateData.IsLoading = false;
		return true;
	}

	private bool Print()
	{
		ReportWindow reportWindow = new ReportWindow();
		reportWindow.ReportType = ReportType.BankMovement;
		reportWindow.Show();
		return true;
	}

	private bool RemoveAll()
	{
		bool result = false;
		MessageBoxResult messageBoxResult = MessageBox.Show("Tem a certeza que deseja remover todos os movimentos deste mês?", "Remover Movimentos", MessageBoxButton.YesNo, MessageBoxImage.Question);
		if (messageBoxResult == MessageBoxResult.Yes)
		{
			IQueryable<BankMovement> queryable = from mov in ReCrossApp.Current.CurrentDataContext.Movements.OfType<BankMovement>().Include("Reconciliation")
				where mov.BankAccount.Id == ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id && mov.TransactionDate <= ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate && mov.TransactionDate >= ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMinDate
				select mov;
			if (queryable != null)
			{
				List<BankMovement> list = queryable.ToList();
				foreach (BankMovement item in list)
				{
					if (item.Reconciliation != null)
					{
						MessageBox.Show("Não pode remover o movimento: '" + item.Description + "' porque está reconciliado.", "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
					}
					else
					{
						ReCrossApp.Current.CurrentDataContext.DeleteObject(item);
					}
				}
			}
			try
			{
				ReCrossApp.Current.CurrentDataContext.SaveChanges();
				ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount;
				result = true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
			}
		}
		return result;
	}

	protected override BankMovementCollection InitilizeDataCollection()
	{
		return new BankMovementCollection(new List<BankMovement>());
	}

	protected override void ProccessAfterChange(object currentEntity, FormAction currentAction)
	{
		base.ProccessAfterChange(currentEntity, currentAction);
		long id = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id;
		short num = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth;
		short num2 = (short)ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear;
		short num3 = 0;
		short num4 = 0;
		short num5 = 0;
		short num6 = 0;
		CompiledQueries.ReconciliationResultParams arg = new CompiledQueries.ReconciliationResultParams
		{
			bankAccount = id
		};
		if (num == 1)
		{
			num3 = 12;
			num4 = (short)(num2 - 1);
		}
		else
		{
			num3 = (short)(num - 1);
			num4 = num2;
		}
		if (num == 12)
		{
			num5 = 1;
			num6 = (short)(num2 + 1);
		}
		else
		{
			num5 = (short)(num + 1);
			num6 = num2;
		}
		bool flag = false;
		arg.month = num3;
		arg.year = num4;
		ReconciliationResult reconciliationResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
		if (reconciliationResult == null)
		{
			arg.month = num5;
			arg.year = num6;
			ReconciliationResult reconciliationResult2 = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
			flag = reconciliationResult2 == null;
		}
		if (flag || reconciliationResult == null)
		{
			return;
		}
		arg.month = num;
		arg.year = num2;
		ReconciliationResult reconciliationResult3 = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
		if (reconciliationResult3 == null)
		{
			return;
		}
		ReconciliationResult reconciliationResult4 = reconciliationResult3;
		decimal? bankroll = reconciliationResult4.Bankroll;
		decimal num7 = ((reconciliationResult != null && reconciliationResult.Bankroll.HasValue) ? reconciliationResult.Bankroll.Value : 0m);
		decimal num8 = 0m;
		decimal value = 0m;
		CompiledQueries.MovementParams arg2 = new CompiledQueries.MovementParams
		{
			minDate = new DateTime(num2, num, 1),
			maxDate = new DateTime(num2, num, DateTime.DaysInMonth(num2, num)),
			bankAccount = id
		};
		IQueryable<BankMovement> queryable = CompiledQueries.GetBankMovements(ReCrossApp.Current.CurrentDataContext, arg2);
		if (queryable != null && queryable.Count() > 0)
		{
			BankMovement[] array = queryable.ToArray();
			foreach (BankMovement bankMovement in array)
			{
				num8 += (bankMovement.CreditAmount.HasValue ? bankMovement.CreditAmount.Value : 0m);
				value += (bankMovement.DebitAmount.HasValue ? bankMovement.DebitAmount.Value : 0m);
			}
			num7 = num7 + num8 - Math.Abs(value);
		}
		if (!bankroll.HasValue || bankroll.Value != num7)
		{
			reconciliationResult4.Bankroll = num7;
			decimal? num9 = reconciliationResult4.Bankroll;
			while (num9.HasValue)
			{
				if (num == 12)
				{
					num = 1;
					num2++;
				}
				else
				{
					num++;
				}
				num9 = RecalculateResultBankroll(id, num, num2, num9.Value);
			}
			try
			{
				ReCrossApp.Current.CurrentDataContext.SaveChanges();
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.Message + "\n\n" + ex.StackTrace);
			}
		}
		ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear;
	}

	public void SetBankMovements(ObjectQuery<BankMovement> movements)
	{
		base.CurrentDataCollection = ((movements != null) ? new BankMovementCollection(movements.ToList()) : new BankMovementCollection());
		CurrentCVS.Source = base.CurrentDataCollection;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
	}

	public static decimal? RecalculateResultBankroll(long bankAccountId, short month, short year, decimal baseBankroll)
	{
		decimal? result = null;
		decimal num = 0m;
		decimal value = 0m;
		CompiledQueries.ReconciliationResultParams arg = new CompiledQueries.ReconciliationResultParams
		{
			month = month,
			year = year,
			bankAccount = bankAccountId
		};
		ReconciliationResult reconciliationResult = CompiledQueries.GetReconciliationResult(ReCrossApp.Current.CurrentDataContext, arg);
		if (reconciliationResult != null)
		{
			ReconciliationResult reconciliationResult2 = reconciliationResult;
			decimal? bankroll = reconciliationResult2.Bankroll;
			decimal num2 = baseBankroll;
			CompiledQueries.MovementParams arg2 = new CompiledQueries.MovementParams
			{
				minDate = new DateTime(year, month, 1),
				maxDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
				bankAccount = bankAccountId
			};
			IQueryable<BankMovement> queryable = CompiledQueries.GetBankMovements(ReCrossApp.Current.CurrentDataContext, arg2);
			if (queryable != null && queryable.Count() > 0)
			{
				BankMovement[] array = queryable.ToArray();
				foreach (BankMovement bankMovement in array)
				{
					num += (bankMovement.CreditAmount.HasValue ? bankMovement.CreditAmount.Value : 0m);
					value += (bankMovement.DebitAmount.HasValue ? bankMovement.DebitAmount.Value : 0m);
				}
				num2 = num2 + num - Math.Abs(value);
			}
			if (!bankroll.HasValue || bankroll.Value != num2)
			{
				result = (bankroll.HasValue ? new decimal?(num2) : ((decimal?)null));
				reconciliationResult2.Bankroll = num2;
			}
		}
		return result;
	}
}
