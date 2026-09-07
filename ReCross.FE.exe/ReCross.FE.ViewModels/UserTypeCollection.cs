using System.Collections.Generic;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class UserTypeCollection : ExtendedObservableCollection<ReCrossEntities, UserType>
{
	public UserTypeCollection()
	{
	}

	public UserTypeCollection(IEnumerable<UserType> entities)
		: base(entities)
	{
	}

	protected override void InsertItem(int index, UserType item)
	{
		base.CurrentDataContext.AddToUserTypes(item);
		base.InsertItem(index, item);
	}

	protected override void RemoveItem(int index)
	{
		base.CurrentDataContext.DeleteObject(base[index]);
		base.RemoveItem(index);
	}
}
