using System.Collections.Generic;
using System.Linq;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Views;

namespace ReCross.FE.ViewModels;

public class UserTypeViewModel : ViewModelBase<ReCrossEntities, UserType, UserTypeCollection>
{
	protected override UserTypeCollection InitilizeDataCollection()
	{
		if (base.CurrentDataContext.UserTypes != null && base.CurrentDataContext.UserTypes.Count() > 0)
		{
			return new UserTypeCollection(base.CurrentDataContext.UserTypes);
		}
		return new UserTypeCollection(new List<UserType>());
	}
}
