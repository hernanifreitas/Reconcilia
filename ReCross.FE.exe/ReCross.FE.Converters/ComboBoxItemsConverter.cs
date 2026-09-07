using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;
using ReCross.BE.Utils;
using ReCross.FE.Utils;
using Strills.WPF.Data;

namespace ReCross.FE.Converters;

public class ComboBoxItemsConverter : IMultiValueConverter
{
	private static ComboBoxItemsConverter _instance = new ComboBoxItemsConverter();

	public static ComboBoxItemsConverter Instance => _instance;

	private ComboBoxItemsConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		try
		{
			if (ReCrossApp.Current.StateData.IsLoggedIn)
			{
				CompositeCollection compositeCollection = new CompositeCollection();
				CollectionContainer collectionContainer = new CollectionContainer();
				CollectionViewSource collectionViewSource = new CollectionViewSource();
				if (parameter is string)
				{
					switch (parameter.ToString())
					{
					case "SoftwareEntities":
					{
						List<SoftwareEntity> list4 = ReCrossApp.Current.CurrentDataContext.Entities.OfType<SoftwareEntity>().ToList();
						list4.Sort((SoftwareEntity a, SoftwareEntity b) => a.ShortName.CompareTo(b.ShortName));
						collectionViewSource.Source = CompositeCollectionAdapter.SetupCompositeCollection(list4, addEmptyItem: true);
						break;
					}
					case "BankEntities":
					{
						List<BankEntity> list2 = ReCrossApp.Current.CurrentDataContext.Entities.OfType<BankEntity>().ToList();
						list2.Sort((BankEntity a, BankEntity b) => a.ShortName.CompareTo(b.ShortName));
						collectionViewSource.Source = CompositeCollectionAdapter.SetupCompositeCollection(list2, addEmptyItem: true);
						break;
					}
					case "Groups":
					{
						List<Group> list3 = ReCrossApp.Current.CurrentDataContext.Groups.ToList();
						list3.Sort((Group a, Group b) => a.Abbreviation.CompareTo(b.Abbreviation));
						collectionViewSource.Source = CompositeCollectionAdapter.SetupCompositeCollection(list3, addEmptyItem: true);
						break;
					}
					case "Customers":
					{
						List<Entity> list = new List<Entity>();
						IQueryable<CompanyEntity> queryable = CompiledQueries.GetCompanyEntityCollection(ReCrossEntities.CurrentContext);
						IQueryable<PersonEntity> queryable2 = CompiledQueries.GetPersonEntityCollection(ReCrossEntities.CurrentContext);
						if (queryable != null || queryable2 != null)
						{
							string key = MenuItemEnum.Customers.ToString().ToLower();
							if (ReCrossApp.Current.UserGenericPermissions[key].CanRead)
							{
								foreach (CompanyEntity item in queryable)
								{
									string key2 = SubMenuItemEnum.Companies.ToString().ToLower();
									if (ReCrossApp.Current.UserGenericPermissions[key2].CanRead && item.Id > 0 && ((ReCrossApp.Current.UserRecordPermissions.ContainsKey(key2) && ReCrossApp.Current.UserRecordPermissions[key2].ContainsKey(item.Id)) ? ReCrossApp.Current.UserRecordPermissions[key2][item.Id].CanRead : ((ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) && ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(item.Id)) ? ReCrossApp.Current.UserRecordPermissions[key][item.Id].CanRead : ReCrossApp.Current.IsActiveForOwners)))
									{
										list.Add(item);
									}
								}
								foreach (PersonEntity item2 in queryable2)
								{
									string key2 = SubMenuItemEnum.Persons.ToString().ToLower();
									if (ReCrossApp.Current.UserGenericPermissions[key2].CanRead && item2.Id > 0 && ((ReCrossApp.Current.UserRecordPermissions.ContainsKey(key2) && ReCrossApp.Current.UserRecordPermissions[key2].ContainsKey(item2.Id)) ? ReCrossApp.Current.UserRecordPermissions[key2][item2.Id].CanRead : ((ReCrossApp.Current.UserRecordPermissions.ContainsKey(key) && ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(item2.Id)) ? ReCrossApp.Current.UserRecordPermissions[key][item2.Id].CanRead : ReCrossApp.Current.IsActiveForOwners)))
									{
										list.Add(item2);
									}
								}
							}
						}
						list.Sort((Entity a, Entity b) => a.ShortName.CompareTo(b.ShortName));
						collectionViewSource.Source = CompositeCollectionAdapter.SetupCompositeCollection(list, addEmptyItem: false);
						break;
					}
					}
					collectionContainer.Collection = (IEnumerable)collectionViewSource.Source;
					compositeCollection.Add(collectionContainer);
					return compositeCollection;
				}
			}
		}
		catch
		{
		}
		return null;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
