using System.Collections.Generic;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class SoftwareEntityViewModel : ViewModelBase<ReCrossEntities, SoftwareEntity, SoftwareEntityCollection>
{
	public CollectionViewSource SoftwareEntityTypeCVS = new CollectionViewSource();

	protected ListCollectionView SoftwareEntityTypeLCV { get; set; }

	public SoftwareEntity CurrentItem
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

	protected override SoftwareEntityCollection InitilizeDataCollection()
	{
		return null;
	}

	protected override void ProccessAfterChange(object currentEntity, FormAction currentAction)
	{
		ReCrossApp.Current.CurrentDataContext.ParamData.SoftwareEntities++;
	}

	public void SetCollection(IEnumerable<SoftwareEntity> collection)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		SoftwareEntityCollection source = (base.CurrentDataCollection = ((collection == null) ? new SoftwareEntityCollection(new List<SoftwareEntity>()) : new SoftwareEntityCollection(collection)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}
}
