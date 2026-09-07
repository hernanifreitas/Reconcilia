using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.Data;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class PersonEntityViewModel : ViewModelBase<ReCrossEntities, PersonEntity, PersonEntityCollection>
{
	public CollectionViewSource SoftwareEntityCVS = new CollectionViewSource();

	public PersonEntity CurrentItem
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

	public CollectionViewSource SoftwareEntityList
	{
		get
		{
			return SoftwareEntityCVS;
		}
		set
		{
			SoftwareEntityCVS = value;
		}
	}

	protected override PersonEntityCollection InitilizeDataCollection()
	{
		return null;
	}

	protected override void InitilizeCustomDataCollections()
	{
		base.InitilizeCustomDataCollections();
		SoftwareEntityCVS.Source = CompositeCollectionAdapter.SetupCompositeCollection(base.CurrentDataContext.Entities.OfType<SoftwareEntity>().ToList(), addEmptyItem: true);
	}

	protected override void ProccessAfterChange(object currentEntity, FormAction currentAction)
	{
		ReCrossApp.Current.CurrentDataContext.ParamData.Customers++;
	}

	public void SetCollection(IEnumerable<PersonEntity> collection)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		PersonEntityCollection source = (base.CurrentDataCollection = ((collection == null) ? new PersonEntityCollection(new List<PersonEntity>()) : new PersonEntityCollection(collection)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}
}
