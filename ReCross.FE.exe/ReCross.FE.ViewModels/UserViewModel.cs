using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.Data;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class UserViewModel : ViewModelBase<ReCrossEntities, User, UserCollection>
{
	public CollectionViewSource UserTypeCVS = new CollectionViewSource();

	public User CurrentItem
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

	public CollectionViewSource UserTypeList
	{
		get
		{
			return UserTypeCVS;
		}
		set
		{
			UserTypeCVS = value;
		}
	}

	protected override UserCollection InitilizeDataCollection()
	{
		return null;
	}

	protected override void InitilizeCustomDataCollections()
	{
		base.InitilizeCustomDataCollections();
		UserTypeCVS.Source = CompositeCollectionAdapter.SetupCompositeCollection(base.CurrentDataContext.UserTypes.ToList(), addEmptyItem: true);
	}

	public void SetCollection(IEnumerable<User> collection)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		UserCollection source = (base.CurrentDataCollection = ((collection == null) ? new UserCollection(new List<User>()) : new UserCollection(collection)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}
}
