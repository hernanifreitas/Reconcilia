using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using Strills.WPF.UI.Commands;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class SearchViewModel : ViewModelBase<ReCrossEntities, BothMovement, BothMovementCollection>
{
	private RelayCommand _cmdSearch;

	private RelayCommand _cmdClean;

	public CollectionViewSource SearchCVS => CurrentCVS;

	public BothMovement CurrentItem
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

	public ICommand CMDSearch => _cmdSearch;

	public ICommand CMDClean => _cmdClean;

	public SearchViewModel()
	{
		Action<object> execute = delegate
		{
			Search();
		};
		_cmdSearch = new RelayCommand(execute);
		_cmdClean = new RelayCommand(delegate
		{
			Clean();
		});
	}

	private bool Search()
	{
		List<BothMovement> list = new List<BothMovement>();
		try
		{
			IQueryable<AccountMovement> queryable = null;
			IQueryable<BankMovement> queryable2 = null;
			CompiledQueries.MovementParams arg = new CompiledQueries.MovementParams
			{
				description = ReCrossApp.Current.CurrentDataContext.DataFilters.Description,
				minDate = ReCrossApp.Current.CurrentDataContext.DataFilters.FromDate,
				maxDate = ReCrossApp.Current.CurrentDataContext.DataFilters.ToDate,
				minValue = ReCrossApp.Current.CurrentDataContext.DataFilters.FromValue,
				maxValue = ReCrossApp.Current.CurrentDataContext.DataFilters.ToValue
			};
			if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity != null)
			{
				arg.entity = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity.Id;
			}
			if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankEntity != null)
			{
				arg.bankEntity = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankEntity.Id;
			}
			if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount != null)
			{
				arg.bankAccount = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentBankAccount.Id;
			}
			if (ReCrossApp.Current.CurrentDataContext.DataFilters.IsAccountMovement)
			{
				queryable = CompiledQueries.GetAccountMovs(ReCrossApp.Current.CurrentDataContext, arg);
				if (queryable != null && queryable.Count() > 0)
				{
					foreach (AccountMovement item in queryable)
					{
						list.Add(new BothMovement("Cont.", item.BankAccount, item.DebitAmount, item.CreditAmount, item.AccountDate, item.Description));
					}
				}
			}
			if (ReCrossApp.Current.CurrentDataContext.DataFilters.IsBankMovement)
			{
				queryable2 = CompiledQueries.GetBankMovs(ReCrossApp.Current.CurrentDataContext, arg);
				if (queryable2 != null && queryable2.Count() > 0)
				{
					foreach (BankMovement item2 in queryable2)
					{
						list.Add(new BothMovement("Banc.", item2.BankAccount, item2.DebitAmount, item2.CreditAmount, item2.TransactionDate, item2.Description));
					}
				}
			}
		}
		catch (Exception ex)
		{
			MessageBox.Show(ex.Message, "Erro", MessageBoxButton.OK, MessageBoxImage.Hand);
		}
		SearchCVS.Source = list;
		return true;
	}

	private bool Clean()
	{
		List<BothMovement> source = new List<BothMovement>();
		SearchCVS.Source = source;
		return true;
	}

	protected override BothMovementCollection InitilizeDataCollection()
	{
		return new BothMovementCollection(new List<BothMovement>());
	}

	protected override void ProccessAfterChange(object currentEntity, FormAction currentAction)
	{
	}
}
