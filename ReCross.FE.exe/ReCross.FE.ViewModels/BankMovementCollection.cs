using System.Collections.Generic;
using ReCross.BE.DataObjects;
using ReCross.FE.Utils;
using Strills.WPF.Data.Objects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class BankMovementCollection : ExtendedObservableCollection<ReCrossEntities, BankMovement>
{
	public BankMovementCollection()
	{
	}

	public BankMovementCollection(IEnumerable<BankMovement> entities)
		: base(entities)
	{
		string[] parentKeys = new string[0];
		string key = MenuItemEnum.BankMovements.ToString().ToLower();
		foreach (BankMovement entity in entities)
		{
			bool hasRecordPermission = entity.Id > 0 && ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) && ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(entity.Id);
			EntityObjectExtended.SetPermissionRecords(entity, ReCrossApp.Current.UserGenericPermissions, ReCrossApp.Current.UserRecordPermissions, entity.Id, key, parentKeys, hasRecordPermission);
		}
	}

	protected override void InsertItem(int index, BankMovement item)
	{
		base.CurrentDataContext.AddToMovements(item);
		base.InsertItem(index, item);
	}

	protected override void RemoveItem(int index)
	{
		base.CurrentDataContext.DeleteObject(base[index]);
		base.RemoveItem(index);
	}
}
