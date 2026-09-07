using System.Collections.Generic;
using System.Windows.Data;
using ReCross.BE.DataObjects;

namespace ReCross.FE.ViewModels;

internal interface IEntityRoles
{
	CollectionViewSource RoleCVS { get; set; }

	ListCollectionView CurrentCollection { get; }

	bool IsAdding { get; }

	bool IsEditing { get; }

	void SetRoles(List<Role> roles);

	int TradeRole(Role newRecord);
}
