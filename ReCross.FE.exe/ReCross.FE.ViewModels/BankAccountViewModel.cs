using System.Collections.Generic;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class BankAccountViewModel : ViewModelBase<ReCrossEntities, BankAccount, BankAccountCollection>
{
	public CollectionViewSource EntityCVS = new CollectionViewSource();

	public BankAccount CurrentItem
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

	public BankAccountViewModel()
	{
		CurrentCVS.Source = new List<BankAccount>();
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
	}

	protected override BankAccountCollection InitilizeDataCollection()
	{
		return base.CurrentDataCollection;
	}

	protected override void InitilizeCustomDataCollections()
	{
		base.InitilizeCustomDataCollections();
	}

	protected override void ProccessAfterChange(object currentEntity, FormAction currentAction)
	{
		ReCrossApp.Current.CurrentDataContext.ParamData.BankAccounts++;
	}

	protected override void ProccessAfterDelete()
	{
		base.ProccessAfterDelete();
	}

	public void SetBankAccounts(IEnumerable<BankAccount> bankAccounts)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		BankAccountCollection source = (base.CurrentDataCollection = ((bankAccounts == null) ? new BankAccountCollection(new List<BankAccount>()) : new BankAccountCollection(bankAccounts)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}
}
