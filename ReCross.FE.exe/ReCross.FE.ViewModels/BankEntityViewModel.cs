using System.Collections.Generic;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class BankEntityViewModel : ViewModelBase<ReCrossEntities, BankEntity, BankEntityCollection>
{
	public CollectionViewSource BankEntityTypeCVS = new CollectionViewSource();

	protected ListCollectionView BankEntityTypeLCV { get; set; }

	public BankEntity CurrentItem
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

	protected override BankEntityCollection InitilizeDataCollection()
	{
		return null;
	}

	protected override void ProccessAfterChange(object currentEntity, FormAction currentAction)
	{
		ReCrossApp.Current.CurrentDataContext.ParamData.BankEntities++;
	}

	public void SetCollection(IEnumerable<BankEntity> collection)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		BankEntityCollection source = (base.CurrentDataCollection = ((collection == null) ? new BankEntityCollection(new List<BankEntity>()) : new BankEntityCollection(collection)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}
}
