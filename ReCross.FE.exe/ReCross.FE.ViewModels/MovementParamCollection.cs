using System.Collections.Generic;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class MovementParamCollection : ExtendedObservableCollection<ReCrossEntities, MovementParam>
{
	public MovementParamCollection()
	{
	}

	public MovementParamCollection(IEnumerable<MovementParam> entities)
		: base(entities)
	{
	}

	protected override void InsertItem(int index, MovementParam item)
	{
		base.CurrentDataContext.AddToMovementParams(item);
		base.InsertItem(index, item);
	}

	protected override void RemoveItem(int index)
	{
		base.CurrentDataContext.DeleteObject(base[index]);
		base.RemoveItem(index);
	}
}
