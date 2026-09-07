using System.Collections.Generic;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class ReconciliationCollection : ExtendedObservableCollection<ReCrossEntities, Reconciliation>
{
	public ReconciliationCollection()
	{
	}

	public ReconciliationCollection(IEnumerable<Reconciliation> entities)
		: base(entities)
	{
	}

	protected override void InsertItem(int index, Reconciliation item)
	{
		base.CurrentDataContext.AddToReconciliations(item);
		base.InsertItem(index, item);
	}

	protected override void RemoveItem(int index)
	{
		base.CurrentDataContext.DeleteObject(base[index]);
		base.RemoveItem(index);
	}
}
