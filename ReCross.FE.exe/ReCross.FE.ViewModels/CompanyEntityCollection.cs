using System.Collections.Generic;
using ReCross.BE.DataObjects;
using ReCross.FE.Utils;
using Strills.WPF.Data.Objects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class CompanyEntityCollection : ExtendedObservableCollection<ReCrossEntities, CompanyEntity>
{
	public CompanyEntityCollection()
	{
	}

	public CompanyEntityCollection(IEnumerable<CompanyEntity> entities)
		: base(entities)
	{
		string[] parentKeys = new string[1] { MenuItemEnum.Customers.ToString().ToLower() };
		string key = SubMenuItemEnum.Companies.ToString().ToLower();
		string key2 = MenuItemEnum.Customers.ToString().ToLower();
		foreach (CompanyEntity entity in entities)
		{
			bool flag = entity.Id > 0 && ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) && ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(entity.Id);
			bool flag2 = entity.Id > 0 && ReCrossApp.Current.UserRecordPermissions.ContainsKey(key2) && ReCrossApp.Current.UserRecordPermissions[key2].ContainsKey(entity.Id);
			EntityObjectExtended.SetPermissionRecords(entity, ReCrossApp.Current.UserGenericPermissions, ReCrossApp.Current.UserRecordPermissions, entity.Id, key, parentKeys, (!flag2) ? flag : flag2);
			entity.PermissionData.CanCreate = ReCrossApp.Current.UserGenericPermissions[key].CanCreate;
		}
	}

	protected override void InsertItem(int index, CompanyEntity item)
	{
		base.CurrentDataContext.AddToEntities(item);
		base.InsertItem(index, item);
	}

	protected override void RemoveItem(int index)
	{
		base.CurrentDataContext.DeleteObject(base[index]);
		base.RemoveItem(index);
	}
}
