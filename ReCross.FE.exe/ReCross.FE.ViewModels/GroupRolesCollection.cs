using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class GroupRolesCollection : ExtendedObservableCollection<ReCrossEntities, Role>
{
	public GroupRolesCollection()
	{
	}

	public GroupRolesCollection(IEnumerable<Role> entities)
		: base(entities)
	{
	}

	private void current_PropertyChanged(object sender, PropertyChangedEventArgs e)
	{
	}

	protected override void InsertItem(int index, Role item)
	{
		((Group)ReCrossApp.Current.StateData.CurrentEditedRootRecord).Roles.Add(item);
		base.InsertItem(index, item);
	}

	protected override void RemoveItem(int index)
	{
		((Group)ReCrossApp.Current.StateData.CurrentEditedRootRecord).Roles.Remove(((Group)ReCrossApp.Current.StateData.CurrentEditedRootRecord).Roles.ElementAt(index));
		if (base[index].EntityState == EntityState.Added)
		{
			base.CurrentDataContext.DeleteObject(base[index]);
		}
		base.RemoveItem(index);
	}
}
