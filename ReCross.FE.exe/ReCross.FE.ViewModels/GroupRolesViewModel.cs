using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class GroupRolesViewModel : ViewModelBase<ReCrossEntities, Role, GroupRolesCollection>, IEntityRoles
{
	public CollectionViewSource FunctionalityCVS = new CollectionViewSource();

	public CollectionViewSource RoleCVS { get; set; }

	public ListCollectionView CurrentCollection => base.CurrentLCV;

	public Role CurrentItem
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

	public CollectionViewSource FunctionalityList
	{
		get
		{
			return FunctionalityCVS;
		}
		set
		{
			FunctionalityCVS = value;
		}
	}

	protected override GroupRolesCollection InitilizeDataCollection()
	{
		base.IsDetailView = true;
		return base.CurrentDataCollection;
	}

	protected override void InitilizeCustomDataCollections()
	{
		RoleCVS = new CollectionViewSource();
		base.InitilizeCustomDataCollections();
		FunctionalityCVS.Source = base.CurrentDataContext.Functionalities.ToList();
	}

	public void SetRoles(List<Role> roles)
	{
		RoleCVS.Source = ((roles != null) ? roles : null);
	}

	public object SetGroupRoles(IEnumerable<Role> roles)
	{
		if (!base.IsDataResetForbidden)
		{
			CollectionViewSource currentCVS = CurrentCVS;
			GroupRolesCollection source = (base.CurrentDataCollection = ((roles == null) ? new GroupRolesCollection(new List<Role>()) : new GroupRolesCollection(roles)));
			currentCVS.Source = source;
			base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		}
		base.IsDataResetForbidden = false;
		return CurrentCVS.Source;
	}

	public int TradeRole(Role newRecord)
	{
		int currentPosition = base.CurrentLCV.CurrentPosition;
		if (base.CurrentLCV.IsEditingItem && base.CurrentLCV.CanCancelEdit)
		{
			base.IsDataResetForbidden = true;
			base.CurrentLCV.CancelEdit();
			base.CurrentDataCollection.RemoveAt(currentPosition);
			base.CurrentDataCollection.Insert(currentPosition, newRecord);
			base.CurrentLCV.EditItem(newRecord);
		}
		if (base.CurrentLCV.IsAddingNew && base.CurrentLCV.CanAddNew)
		{
			((Role)base.CurrentLCV.CurrentAddItem).Id = newRecord.Id;
			((Role)base.CurrentLCV.CurrentAddItem).Abbreviation = newRecord.Abbreviation;
			((Role)base.CurrentLCV.CurrentAddItem).Description = newRecord.Description;
			((Role)base.CurrentLCV.CurrentAddItem).Owner = newRecord.Owner;
			((Role)base.CurrentLCV.CurrentAddItem).OwnerId = newRecord.OwnerId;
			((Role)base.CurrentLCV.CurrentAddItem).CanCreate = newRecord.CanCreate;
			((Role)base.CurrentLCV.CurrentAddItem).CanRead = newRecord.CanRead;
			((Role)base.CurrentLCV.CurrentAddItem).CanUpdate = newRecord.CanUpdate;
			((Role)base.CurrentLCV.CurrentAddItem).CanDelete = newRecord.CanDelete;
		}
		return currentPosition;
	}

	[SpecialName]
	bool IEntityRoles.get_IsAdding()
	{
		return base.IsAdding;
	}

	[SpecialName]
	bool IEntityRoles.get_IsEditing()
	{
		return base.IsEditing;
	}
}
