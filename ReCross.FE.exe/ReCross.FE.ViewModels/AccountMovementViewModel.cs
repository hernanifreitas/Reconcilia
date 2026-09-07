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

public class AccountMovementViewModel : ViewModelBase<ReCrossEntities, AccountMovement, AccountMovementCollection>
{
	private ICommand _cmdLoad;

	private RelayCommand _cmdImport;

	private RelayCommand _cmdPrint;

	private RelayCommand _cmdRemoveAll;

	public CollectionViewSource AccountMovementCVS => CurrentCVS;

	public AccountMovement CurrentItem
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

	public ICommand CMDLoad => _cmdLoad;

	public ICommand CMDImport => _cmdImport;

	public ICommand CMDPrint => _cmdPrint;

	public ICommand CMDRemoveAll => _cmdRemoveAll;

	public AccountMovementViewModel()
	{
		Action<object> execute = delegate
		{
			Load();
		};
		_cmdLoad = new RelayCommand(execute);
		_cmdImport = new RelayCommand(delegate
		{
			Import();
		});
		_cmdPrint = new RelayCommand(delegate
		{
			Print();
		});
		_cmdRemoveAll = new RelayCommand(delegate
		{
			RemoveAll();
		});
	}

	private bool Load()
	{
		ReCrossApp.Current.StateData.IsLoading = true;
		return true;
	}

	private bool Import()
	{
		if (!ExcellImporter.IsRoconciledAndClosed())
		{
			SoftwareEntity sw = null;
			MovementParam movementParam = null;
			IQueryable<PersonEntity> queryable = from rec in ReCrossApp.Current.CurrentDataContext.Entities.OfType<PersonEntity>()
				where ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity.Id == rec.Id
				select rec;
			if (queryable != null && ((ObjectQuery<PersonEntity>)queryable).Count() == 1)
			{
				sw = queryable.First().SoftwareEntity;
				movementParam = sw.MovementParam;
			}
			else
			{
				IQueryable<CompanyEntity> queryable2 = from rec in ReCrossApp.Current.CurrentDataContext.Entities.OfType<CompanyEntity>()
					where ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity.Id == rec.Id
					select rec;
				if (queryable2 != null && ((ObjectQuery<CompanyEntity>)queryable2).Count() == 1)
				{
					sw = queryable2.First().SoftwareEntity;
					if (sw != null)
					{
						movementParam = sw.MovementParam;
					}
				}
			}
			if (movementParam == null && sw != null)
			{
				IQueryable<MovementParam> queryable3 = ReCrossApp.Current.CurrentDataContext.MovementParams.Where((MovementParam rec) => sw.Id == rec.Entity.Id);
				if (queryable3 != null && ((ObjectQuery<MovementParam>)queryable3).Count() == 1)
				{
					movementParam = queryable3.First();
				}
			}
			if (movementParam == null)
			{
				MessageBox.Show("Não existe parametrização de importação para o Cliente seleccionado (programa associado)!", "", MessageBoxButton.OK, MessageBoxImage.Hand);
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
								messageBoxResult2 = MessageBox.Show("Tem a certeza que deseja eliminar?", "Confirmação", MessageBoxButton.YesNo, MessageBoxImage.Question);
							}
							ExcellImporter.ImportExcellAccountMovements(fileDialog.FileName, ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount, movementParam, messageBoxResult2 == MessageBoxResult.Yes);
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
		reportWindow.ReportType = ReportType.AccountMovement;
		reportWindow.Show();
		return true;
	}

	private bool RemoveAll()
	{
		bool result = false;
		MessageBoxResult messageBoxResult = MessageBox.Show("Tem a certeza que deseja remover todos os movimentos deste mês?", "Remover Movimentos", MessageBoxButton.YesNo, MessageBoxImage.Question);
		if (messageBoxResult == MessageBoxResult.Yes)
		{
			IQueryable<AccountMovement> queryable = from mov in ReCrossApp.Current.CurrentDataContext.Movements.OfType<AccountMovement>().Include("Reconciliation")
				where mov.BankAccount.Id == ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id && mov.AccountDate <= ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMaxDate && mov.AccountDate >= ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMinDate
				select mov;
			if (queryable != null)
			{
				List<AccountMovement> list = queryable.ToList();
				foreach (AccountMovement item in list)
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

	protected override AccountMovementCollection InitilizeDataCollection()
	{
		return new AccountMovementCollection(new List<AccountMovement>());
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
		decimal? reasonBalance = reconciliationResult4.ReasonBalance;
		decimal num7 = ((reconciliationResult != null && reconciliationResult.ReasonBalance.HasValue) ? reconciliationResult.ReasonBalance.Value : 0m);
		decimal num8 = 0m;
		decimal value = 0m;
		CompiledQueries.MovementParams arg2 = new CompiledQueries.MovementParams
		{
			minDate = new DateTime(num2, num, 1),
			maxDate = new DateTime(num2, num, DateTime.DaysInMonth(num2, num)),
			bankAccount = id
		};
		IQueryable<AccountMovement> queryable = CompiledQueries.GetAccountMovements(ReCrossApp.Current.CurrentDataContext, arg2);
		if (queryable != null && queryable.Count() > 0)
		{
			AccountMovement[] array = queryable.ToArray();
			foreach (AccountMovement accountMovement in array)
			{
				num8 += (accountMovement.CreditAmount.HasValue ? accountMovement.CreditAmount.Value : 0m);
				value += (accountMovement.DebitAmount.HasValue ? accountMovement.DebitAmount.Value : 0m);
			}
			num7 = num7 - num8 + Math.Abs(value);
		}
		if (!reasonBalance.HasValue || reasonBalance.Value != num7)
		{
			reconciliationResult4.ReasonBalance = num7;
			decimal? num9 = reconciliationResult4.ReasonBalance;
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
				num9 = RecalculateResultReasonBalance(id, num, num2, num9.Value);
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

	public void SetAccountMovements(ObjectQuery<AccountMovement> movements)
	{
		base.CurrentDataCollection = ((movements != null) ? new AccountMovementCollection(movements.ToList()) : new AccountMovementCollection());
		CurrentCVS.Source = base.CurrentDataCollection;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
	}

	public static decimal? RecalculateResultReasonBalance(long bankAccountId, short month, short year, decimal baseReasonBalance)
	{
		decimal? result = null;
		decimal num = 0m;
		decimal value = 0m;
		IQueryable<ReconciliationResult> queryable = ReCrossApp.Current.CurrentDataContext.ReconciliationResults.Where((ReconciliationResult res) => res.BankAccount.Id == bankAccountId && res.Month == month && res.Year == year);
		if (queryable != null && queryable.Count() == 1)
		{
			ReconciliationResult reconciliationResult = queryable.ToList()[0];
			decimal? reasonBalance = reconciliationResult.ReasonBalance;
			decimal num2 = baseReasonBalance;
			CompiledQueries.MovementParams arg = new CompiledQueries.MovementParams
			{
				minDate = new DateTime(year, month, 1),
				maxDate = new DateTime(year, month, DateTime.DaysInMonth(year, month)),
				bankAccount = bankAccountId
			};
			IQueryable<AccountMovement> queryable2 = CompiledQueries.GetAccountMovements(ReCrossApp.Current.CurrentDataContext, arg);
			if (queryable2 != null && queryable2.Count() > 0)
			{
				AccountMovement[] array = queryable2.ToArray();
				foreach (AccountMovement accountMovement in array)
				{
					num += (accountMovement.CreditAmount.HasValue ? accountMovement.CreditAmount.Value : 0m);
					value += (accountMovement.DebitAmount.HasValue ? accountMovement.DebitAmount.Value : 0m);
				}
				num2 = num2 - num + Math.Abs(value);
			}
			if (!reasonBalance.HasValue || reasonBalance.Value != num2)
			{
				result = (reasonBalance.HasValue ? new decimal?(num2) : ((decimal?)null));
				reconciliationResult.ReasonBalance = num2;
			}
		}
		return result;
	}
}
