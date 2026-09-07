using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Windows.Data;
using ReCross.BE.DataObjects;

namespace ReCross.FE.ViewModels;

public class BankEntityConverter : IMultiValueConverter
{
	private static BankEntityConverter _instance = new BankEntityConverter();

	public static BankEntityConverter Instance => _instance;

	private BankEntityConverter()
	{
	}

	public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
	{
		MovementFilterViewModel movementFilterViewModel = (MovementFilterViewModel)parameter;
		try
		{
			if (ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity != null)
			{
				ObjectQuery<Entity> objectQuery = ReCrossApp.Current.CurrentDataContext.Entities.Include("MovementParam");
				IQueryable<BankEntity> queryable = from rec in objectQuery.OfType<BankEntity>()
					join bA in ReCrossApp.Current.CurrentDataContext.BankAccounts on new
					{
						EntityId = ReCrossApp.Current.CurrentDataContext.DataFilters.CurrentEntity.Id,
						BankEntityId = rec.Id
					} equals new
					{
						EntityId = bA.Entity.Id,
						BankEntityId = bA.BankEntity.Id
					}
					select rec;
				if (objectQuery != null && queryable != null)
				{
					queryable = queryable.Distinct();
					foreach (BankEntity item in queryable)
					{
						foreach (Entity item2 in (IEnumerable<Entity>)objectQuery)
						{
							if (item.Id == item2.Id)
							{
								item.MovementParam = item2.MovementParam;
								break;
							}
						}
					}
				}
				movementFilterViewModel.SetBankEntities(queryable?.ToList());
			}
			else
			{
				movementFilterViewModel.SetBankEntities(null);
			}
		}
		catch
		{
		}
		return movementFilterViewModel.BankEntityCVS.Source;
	}

	public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
	{
		throw new NotImplementedException();
	}
}
