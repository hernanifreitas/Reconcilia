using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class SWMovementParamViewModel : ViewModelBase<ReCrossEntities, MovementParam, MovementParamCollection>
{
	public CollectionViewSource EntityCVS = new CollectionViewSource();

	protected ListCollectionView EntityLCV { get; set; }

	public MovementParam CurrentItem
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

	protected override MovementParamCollection InitilizeDataCollection()
	{
		return null;
	}

	protected override void InitilizeCustomDataCollections()
	{
		base.InitilizeCustomDataCollections();
		EntityCVS.Source = base.CurrentDataContext.Entities.OfType<SoftwareEntity>().ToList();
		EntityLCV = (ListCollectionView)EntityCVS.View;
	}

	public void SetCollection(IEnumerable<MovementParam> collection)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		MovementParamCollection source = (base.CurrentDataCollection = ((collection == null) ? new MovementParamCollection(new List<MovementParam>()) : new MovementParamCollection(collection)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}
}
