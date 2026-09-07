using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.FE.Utils;
using Strills.WPF.Localization.Converters;
using Strills.WPF.UI.Views;
using Strills.WPF.UI.Windows;

namespace ReCross.FE.ViewModels;

public class RoleViewModel : ViewModelBase<ReCrossEntities, Role, RoleCollection>
{
	public CollectionViewSource FunctionalityCVS = new CollectionViewSource();

	public CollectionViewSource OwnerCVS = new CollectionViewSource();

	protected ListCollectionView FunctionalityLCV { get; set; }

	protected ListCollectionView OwnerLCV { get; set; }

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

	public CollectionViewSource OwnerList
	{
		get
		{
			return OwnerCVS;
		}
		set
		{
			OwnerCVS = value;
		}
	}

	protected override RoleCollection InitilizeDataCollection()
	{
		if (ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.Functionalities == null || ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.Functionalities.Count() == 0)
		{
			List<KeyValuePair<string, string>> values = ResourceEnumConverter.GetValues(typeof(MenuItemEnum));
			if (values != null && values.Count > 0)
			{
				foreach (KeyValuePair<string, string> item in values)
				{
					Functionality functionality = Functionality.CreateFunctionality(item.Key, item.Key, item.Value, generated: true);
					ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToFunctionalities(functionality);
				}
				ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.SaveChanges();
			}
			values = ResourceEnumConverter.GetValues(typeof(SubMenuItemEnum));
			if (values != null && values.Count > 0)
			{
				foreach (KeyValuePair<string, string> item2 in values)
				{
					Functionality functionality = Functionality.CreateFunctionality(item2.Key, item2.Key, item2.Value, generated: true);
					ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToFunctionalities(functionality);
				}
				ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.SaveChanges();
			}
			values = ResourceEnumConverter.GetValues(typeof(OperationEnum));
			if (values != null && values.Count > 0)
			{
				foreach (KeyValuePair<string, string> item3 in values)
				{
					Functionality functionality = Functionality.CreateFunctionality(item3.Key, item3.Key, item3.Value, generated: true);
					ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToFunctionalities(functionality);
				}
				ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.SaveChanges();
			}
		}
		else
		{
			string autoLinkId = OperationEnum.AutoLink.ToString();
			IQueryable<Functionality> queryable = ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.Functionalities.Where((Functionality rec) => rec.Id == autoLinkId);
			if (queryable == null || queryable.Count() == 0)
			{
				List<KeyValuePair<string, string>> values = ResourceEnumConverter.GetValues(typeof(OperationEnum));
				foreach (KeyValuePair<string, string> item4 in values)
				{
					if (autoLinkId == item4.Key)
					{
						Functionality functionality = Functionality.CreateFunctionality(item4.Key, item4.Key, item4.Value, generated: true);
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToFunctionalities(functionality);
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.SaveChanges();
						queryable = ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.Functionalities.Where((Functionality rec) => rec.Id == autoLinkId);
						if (queryable != null && queryable.Count() == 1)
						{
							Functionality functionality2 = queryable.ToArray()[0];
							Role role = Role.CreateRole(-1L, string.Empty, generated: true, canCreate: true, canRead: true, canUpdate: true, canDelete: true);
							role.Functionality = functionality2;
							role.FillAbbreviationAndDescription();
							ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
							role = Role.CreateRole(-1L, string.Empty, generated: true, canCreate: false, canRead: false, canUpdate: false, canDelete: false);
							role.Functionality = functionality2;
							role.FillAbbreviationAndDescription();
							ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
							ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.SaveChanges();
						}
						break;
					}
				}
			}
		}
		if (ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.Roles == null || ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.Roles.Count() == 0)
		{
			List<Functionality> list = ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.Functionalities.ToList();
			if (list != null && list.Count > 0)
			{
				foreach (Functionality item5 in list)
				{
					bool flag = false;
					try
					{
						object obj = Enum.Parse(typeof(MenuItemEnum), item5.Id);
					}
					catch (Exception ex)
					{
						try
						{
							string message = ex.Message;
							object obj = Enum.Parse(typeof(SubMenuItemEnum), item5.Id);
						}
						catch (Exception ex2)
						{
							string message = ex2.Message;
							object obj = Enum.Parse(typeof(OperationEnum), item5.Id);
							flag = true;
						}
					}
					if (!flag)
					{
						Role role = Role.CreateRole(-1L, string.Empty, generated: true, canCreate: true, canRead: true, canUpdate: true, canDelete: true);
						role.Functionality = item5;
						role.FillAbbreviationAndDescription();
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
						role = Role.CreateRole(-1L, string.Empty, generated: true, canCreate: true, canRead: true, canUpdate: true, canDelete: false);
						role.Functionality = item5;
						role.FillAbbreviationAndDescription();
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
						role = Role.CreateRole(-1L, string.Empty, generated: true, canCreate: false, canRead: true, canUpdate: true, canDelete: false);
						role.Functionality = item5;
						role.FillAbbreviationAndDescription();
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
						role = Role.CreateRole(-1L, string.Empty, generated: true, canCreate: false, canRead: true, canUpdate: false, canDelete: false);
						role.Functionality = item5;
						role.FillAbbreviationAndDescription();
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
						role = Role.CreateRole(-1L, string.Empty, generated: true, canCreate: false, canRead: false, canUpdate: false, canDelete: false);
						role.Functionality = item5;
						role.FillAbbreviationAndDescription();
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
					}
					else
					{
						Role role = Role.CreateRole(-1L, string.Empty, generated: true, canCreate: true, canRead: true, canUpdate: true, canDelete: true);
						role.Functionality = item5;
						role.FillAbbreviationAndDescription();
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
						role = Role.CreateRole(-1L, string.Empty, generated: true, canCreate: false, canRead: false, canUpdate: false, canDelete: false);
						role.Functionality = item5;
						role.FillAbbreviationAndDescription();
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
					}
					ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.SaveChanges();
				}
			}
		}
		return null;
	}

	protected override void InitilizeCustomDataCollections()
	{
		base.InitilizeCustomDataCollections();
		FunctionalityCVS.Source = base.CurrentDataContext.Functionalities.ToList();
		FunctionalityLCV = (ListCollectionView)FunctionalityCVS.View;
	}

	public void SetCollection(IEnumerable<Role> collection)
	{
		CollectionViewSource currentCVS = CurrentCVS;
		RoleCollection source = (base.CurrentDataCollection = ((collection == null) ? new RoleCollection(new List<Role>()) : new RoleCollection(collection)));
		currentCVS.Source = source;
		base.CurrentLCV = (ListCollectionView)CurrentCVS.View;
		InitilizeCustomDataCollections();
	}

	public void SetOwners(object owners)
	{
		OwnerCVS.Source = ((owners != null) ? owners : null);
		OwnerLCV = (ListCollectionView)OwnerCVS.View;
	}
}
