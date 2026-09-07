using System.Collections.Generic;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class ReconciliationResultViewModel : ViewModelBase<ReCrossEntities, ReconciliationResult, ReconciliationResultCollection>
{
	public CollectionViewSource ReconciliationResultCVS => CurrentCVS;

	public ReconciliationResult CurrentItem
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

	protected override ReconciliationResultCollection InitilizeDataCollection()
	{
		return null;
	}

	public void SetCollection(IEnumerable<ReconciliationResult> collection)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		ReconciliationResultCollection source = (base.CurrentDataCollection = ((collection == null) ? new ReconciliationResultCollection(new List<ReconciliationResult>()) : new ReconciliationResultCollection(collection)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}
}
