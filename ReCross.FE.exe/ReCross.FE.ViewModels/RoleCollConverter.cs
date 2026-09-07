using System;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Utils;
using Strills.WPF.UI.Windows;

namespace ReCross.FE.ViewModels;

public class RoleCollConverter : IMultiValueConverter
{
	private static RoleCollConverter _instance = new RoleCollConverter();

	public static RoleCollConverter Instance => _instance;

	private RoleCollConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		RoleViewModel roleViewModel = (RoleViewModel)parameter;
		if (ReCrossApp.Current.StateData.IsLoggedIn)
		{
			IQueryable<Role> queryable = from r in CompiledQueries.GetRoleCollection(ReCrossApp.Current.CurrentDataContext)
				where r.OwnerId.HasValue
				select r;
			IQueryable<PersonEntity> queryable2 = CompiledQueries.GetPersonEntityCollection(ReCrossApp.Current.CurrentDataContext);
			IQueryable<CompanyEntity> queryable3 = CompiledQueries.GetCompanyEntityCollection(ReCrossApp.Current.CurrentDataContext);
			bool flag = false;
			if (queryable2 != null)
			{
				foreach (PersonEntity current in queryable2)
				{
					if (queryable == null || queryable.Count() == 0 || queryable.Where((Role r) => r.OwnerId == (long?)current.Id).Count() == 0)
					{
						flag = true;
						Role role = Role.CreateRole(-1L, string.Empty, generated: false, canCreate: false, canRead: true, canUpdate: false, canDelete: false);
						role.OwnerId = current.Id;
						string id = SubMenuItemEnum.Persons.ToString();
						role.Functionality = ReCrossEntities.CurrentContext.Functionalities.Where((Functionality f) => f.Id == id).First();
						role.FillAbbreviationAndDescription();
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
					}
				}
			}
			if (queryable3 != null)
			{
				foreach (CompanyEntity current2 in queryable3)
				{
					if (queryable == null || queryable.Count() == 0 || queryable.Where((Role r) => r.OwnerId == (long?)current2.Id).Count() == 0)
					{
						flag = true;
						Role role = Role.CreateRole(-1L, string.Empty, generated: false, canCreate: false, canRead: true, canUpdate: false, canDelete: false);
						role.OwnerId = current2.Id;
						string id2 = SubMenuItemEnum.Companies.ToString();
						role.Functionality = ReCrossEntities.CurrentContext.Functionalities.Where((Functionality f) => f.Id == id2).First();
						role.FillAbbreviationAndDescription();
						ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.AddToRoles(role);
					}
				}
			}
			if (flag)
			{
				ApplicationBase<ReCrossEntities>.Current.CurrentDataContext.SaveChanges();
			}
			queryable = CompiledQueries.GetRoleCollection(ReCrossApp.Current.CurrentDataContext);
			roleViewModel.SetCollection(queryable);
		}
		return roleViewModel.DataList;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
