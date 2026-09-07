using System.Collections.Generic;
using ReCross.BE.DataObjects;
using ReCross.FE.Utils;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class BothMovementCollection : ExtendedObservableCollection<ReCrossEntities, BothMovement>
{
	public BothMovementCollection()
	{
	}

	public BothMovementCollection(IEnumerable<BothMovement> entities)
		: base(entities)
	{
		string text = MenuItemEnum.Search.ToString().ToLower();
	}

	protected override void InsertItem(int index, BothMovement item)
	{
		base.InsertItem(index, item);
	}

	protected override void RemoveItem(int index)
	{
		base.RemoveItem(index);
	}
}
