using System.Collections.Generic;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class CompanyEntityViewModel : ViewModelBase<ReCrossEntities, CompanyEntity, CompanyEntityCollection>
{
	public CollectionViewSource CompanyEntityTypeCVS = new CollectionViewSource();

	protected ListCollectionView CompanyEntityTypeLCV { get; set; }

	public CompanyEntity CurrentItem
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

	protected override CompanyEntityCollection InitilizeDataCollection()
	{
		return null;
	}

	protected override void InitilizeCustomDataCollections()
	{
		base.InitilizeCustomDataCollections();
	}

	protected override void ProccessAfterChange(object currentEntity, FormAction currentAction)
	{
		ReCrossApp.Current.CurrentDataContext.ParamData.Customers++;
	}

	public void SetCollection(IEnumerable<CompanyEntity> collection)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		CompanyEntityCollection source = (base.CurrentDataCollection = ((collection == null) ? new CompanyEntityCollection(new List<CompanyEntity>()) : new CompanyEntityCollection(collection)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}
}
