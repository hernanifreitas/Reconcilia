using System.Collections.Generic;
using ReCross.BE.DataObjects;
using ReCross.FE.Utils;
using Strills.WPF.Data.Objects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class ReconciliationResultCollection : ExtendedObservableCollection<ReCrossEntities, ReconciliationResult>
{
	public ReconciliationResultCollection()
	{
	}

	public ReconciliationResultCollection(IEnumerable<ReconciliationResult> entities)
		: base(entities)
	{
		string[] parentKeys = new string[1] { MenuItemEnum.Parametrization.ToString().ToLower() };
		string key = SubMenuItemEnum.ReconciliationResults.ToString().ToLower();
		foreach (ReconciliationResult entity in entities)
		{
			bool hasRecordPermission = entity.Id > 0 && ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) && ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(entity.Id);
			EntityObjectExtended.SetPermissionRecords(entity, ReCrossApp.Current.UserGenericPermissions, ReCrossApp.Current.UserRecordPermissions, entity.Id, key, parentKeys, hasRecordPermission);
		}
	}

	protected override void InsertItem(int index, ReconciliationResult item)
	{
		base.CurrentDataContext.AddToReconciliationResults(item);
		base.InsertItem(index, item);
	}

	protected override void RemoveItem(int index)
	{
		base.CurrentDataContext.DeleteObject(base[index]);
		base.RemoveItem(index);
	}
}
