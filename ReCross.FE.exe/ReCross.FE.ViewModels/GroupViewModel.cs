using System.Collections.Generic;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class GroupViewModel : ViewModelBase<ReCrossEntities, Group, GroupCollection>
{
	public CollectionViewSource UserCVS = new CollectionViewSource();

	protected ListCollectionView UserLCV { get; set; }

	public Group CurrentItem
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

	public CollectionViewSource UserList
	{
		get
		{
			return UserCVS;
		}
		set
		{
			UserCVS = value;
		}
	}

	protected override GroupCollection InitilizeDataCollection()
	{
		return null;
	}

	protected override void InitilizeCustomDataCollections()
	{
		base.InitilizeCustomDataCollections();
	}

	protected override void ProccessAfterChange(object currentEntity, FormAction currentAction)
	{
		ReCrossApp.Current.CurrentDataContext.ParamData.Groups++;
	}

	public void SetCollection(IEnumerable<Group> collection)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		GroupCollection source = (base.CurrentDataCollection = ((collection == null) ? new GroupCollection(new List<Group>()) : new GroupCollection(collection)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}
}
