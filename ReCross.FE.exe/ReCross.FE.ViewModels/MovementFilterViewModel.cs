using System;
using System.Collections.Generic;
using System.Windows.Data;
using System.Windows.Input;
using ReCross.BE.DataObjects;
using Strills.WPF.Data;
using Strills.WPF.UI.Commands;

namespace ReCross.FE.ViewModels;

public class MovementFilterViewModel
{
	private ICommand _cmdChangeYear;

	private ICommand _cmdChangeMonth;

	public CollectionViewSource EntityCVS = new CollectionViewSource();

	public CollectionViewSource BankEntityCVS = new CollectionViewSource();

	public CollectionViewSource BankAccountCVS = new CollectionViewSource();

	protected ListCollectionView EntityLCV { get; set; }

	protected ListCollectionView BankEntityLCV { get; set; }

	protected ListCollectionView BankAccountLCV { get; set; }

	public ICommand CMDChangeYear => _cmdChangeYear;

	public ICommand CMDChangeMonth => _cmdChangeMonth;

	public CollectionViewSource EntityList
	{
		get
		{
			return EntityCVS;
		}
		set
		{
			EntityCVS = value;
		}
	}

	public MovementFilterViewModel()
	{
		SimpleCommand simpleCommand = new SimpleCommand();
		Action<object> executeDelegate = delegate(object x)
		{
			ChangeYear(int.Parse(x.ToString()));
		};
		simpleCommand.ExecuteDelegate = executeDelegate;
		_cmdChangeYear = simpleCommand;
		_cmdChangeMonth = new SimpleCommand
		{
			ExecuteDelegate = delegate(object x)
			{
				ChangeMonth(int.Parse(x.ToString()));
			}
		};
		InitilizeCustomDataCollections();
	}

	private void InitilizeCustomDataCollections()
	{
		try
		{
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	private bool ChangeYear(int increment)
	{
		ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentYear + increment;
		ReCrossApp.Current.CurrentDataContext.DataFilters.HasDataSelected = true;
		return true;
	}

	private bool ChangeMonth(int month)
	{
		ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentMonth = month;
		ReCrossApp.Current.CurrentDataContext.DataFilters.HasDataSelected = true;
		return true;
	}

	public void SetBankAccounts(List<BankAccount> bankAccounts)
	{
		BankAccountCVS.Source = CompositeCollectionAdapter.SetupCompositeCollection((bankAccounts != null) ? bankAccounts : null, addEmptyItem: true);
		BankAccountLCV = (ListCollectionView)BankAccountCVS.View;
	}

	public void SetBankEntities(List<BankEntity> bankEntities)
	{
		BankEntityCVS.Source = CompositeCollectionAdapter.SetupCompositeCollection((bankEntities != null) ? bankEntities : null, addEmptyItem: true);
		BankEntityLCV = (ListCollectionView)BankEntityCVS.View;
	}

	public void SetCustomers(List<Entity> customers)
	{
		EntityCVS.Source = ((customers != null) ? customers : null);
		BankEntityLCV = (ListCollectionView)EntityCVS.View;
	}
}
