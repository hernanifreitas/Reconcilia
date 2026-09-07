using System;
using System.Collections.Generic;
using System.Data.Objects;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ReCross.BE.DataObjects;
using ReCross.FE.Utils;
using Strills.WPF.Security;
using Strills.WPF.UI.Commands;

namespace ReCross.FE.ViewModels;

public class LoginViewModel
{
	private RelayCommand _cmdLogin;

	private RelayCommand _cmdClean;

	public bool Enabled => !ReCrossApp.Current.StateData.IsEditingMode;

	public User CurrentItem { get; set; }

	public ICommand CMDLogin => _cmdLogin;

	public ICommand CMDClean => _cmdClean;

	public LoginViewModel()
	{
		try
		{
			CurrentItem = new User();
			CurrentItem.Username = string.Empty;
			CurrentItem.Password = string.Empty;
			_cmdLogin = new RelayCommand(delegate
			{
				Login();
			}, (object param) => CanLogin());
			_cmdClean = new RelayCommand(delegate
			{
				Clean();
			});
		}
		catch (Exception ex)
		{
			string message = ex.Message;
		}
	}

	private bool CanLogin()
	{
		return !string.IsNullOrEmpty(CurrentItem.Username) && !string.IsNullOrEmpty(CurrentItem.Password);
	}

	private bool Login()
	{
		bool flag = false;
		User loggedUser = User.GetUserByUsername(CurrentItem.Username);
		if (loggedUser != null)
		{
			flag = loggedUser.AuthenticateUser(CurrentItem.Password);
		}
		if (loggedUser == null || !flag)
		{
			MessageBox.Show("Dados de autenticação inválidos!");
		}
		else
		{
			CurrentItem = new User();
			ReCrossApp.Current.UserFullName = loggedUser.FirstName + " " + loggedUser.LastName;
			ReCrossApp.Current.UserId = loggedUser.Id;
			ReCrossApp.Current.UserIsAdmin = loggedUser.UserType.Id == 1;
			if (ReCrossApp.Current.UserIsAdmin)
			{
				ReCrossApp.Current.IsActiveForOwners = true;
				foreach (MenuItemEnum value2 in Enum.GetValues(typeof(MenuItemEnum)))
				{
					Permission value = new Permission();
					ReCrossApp.Current.UserGenericPermissions.Add(value2.ToString().ToLower(), value);
				}
				foreach (SubMenuItemEnum value3 in Enum.GetValues(typeof(SubMenuItemEnum)))
				{
					Permission value = new Permission();
					ReCrossApp.Current.UserGenericPermissions.Add(value3.ToString().ToLower(), value);
				}
				foreach (OperationEnum value4 in Enum.GetValues(typeof(OperationEnum)))
				{
					Permission value = new Permission();
					ReCrossApp.Current.UserGenericPermissions.Add(value4.ToString().ToLower(), value);
				}
			}
			else if (loggedUser.Group != null)
			{
				IQueryable<Group> queryable = from rec in ReCrossApp.Current.CurrentDataContext.Groups.Include("Roles")
					where rec.Id == loggedUser.Group.Id
					select rec;
				if (queryable != null && queryable.ToArray().Count() == 1 && queryable.ToArray()[0].Roles != null)
				{
					ReCrossApp.Current.UserGenericPermissions.Clear();
					ReCrossApp.Current.UserRecordPermissions.Clear();
					ReCrossApp.Current.IsActiveForOwners = loggedUser.Group.IsActiveForOwners;
					EntityCollection<Role> roles = loggedUser.Group.Roles;
					if (roles != null)
					{
						foreach (Role current in roles)
						{
							if (current.Functionality == null)
							{
								IQueryable<Role> queryable2 = from rec in ReCrossApp.Current.CurrentDataContext.Roles.Include("Functionality")
									where current.Id == rec.Id
									select rec;
								if (queryable2 != null && ((ObjectQuery<Role>)queryable2).Count() == 1)
								{
									current.Functionality = queryable2.First().Functionality;
								}
							}
							string key = current.Functionality.Id.ToLower();
							Permission value = new Permission();
							value.CanCreate = current.CanCreate;
							value.CanRead = current.CanRead;
							value.CanUpdate = current.CanUpdate;
							value.CanDelete = current.CanDelete;
							if (current.OwnerId.HasValue)
							{
								if (!ReCrossApp.Current.UserRecordPermissions.ContainsKey(key))
								{
									ReCrossApp.Current.UserRecordPermissions.Add(key, new Dictionary<long, Permission>());
								}
								if (!ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(current.OwnerId.Value))
								{
									ReCrossApp.Current.UserRecordPermissions[key].Add(current.OwnerId.Value, value);
								}
								else
								{
									ReCrossApp.Current.UserRecordPermissions[key][current.OwnerId.Value] = value;
								}
							}
							else if (!ReCrossApp.Current.UserGenericPermissions.ContainsKey(key))
							{
								ReCrossApp.Current.UserGenericPermissions.Add(key, value);
							}
							else
							{
								ReCrossApp.Current.UserGenericPermissions[key] = value;
							}
						}
					}
					IQueryable<User> queryable3 = from rec in ReCrossApp.Current.CurrentDataContext.Users.Include("Roles")
						where rec.Id == loggedUser.Id
						select rec;
					if (queryable3 != null && queryable3.ToArray().Count() == 1 && queryable3.ToArray()[0].Roles != null)
					{
						roles = queryable3.ToArray()[0].Roles;
						if (roles != null)
						{
							foreach (Role current2 in roles)
							{
								if (current2.Functionality == null)
								{
									IQueryable<Role> queryable2 = from rec in ReCrossApp.Current.CurrentDataContext.Roles.Include("Functionality")
										where current2.Id == rec.Id
										select rec;
									if (queryable2 != null && ((ObjectQuery<Role>)queryable2).Count() == 1)
									{
										current2.Functionality = queryable2.First().Functionality;
									}
								}
								string key = current2.Functionality.Id.ToLower();
								Permission value = new Permission();
								value.CanCreate = current2.CanCreate;
								value.CanRead = current2.CanRead;
								value.CanUpdate = current2.CanUpdate;
								value.CanDelete = current2.CanDelete;
								if (current2.OwnerId.HasValue)
								{
									if (!ReCrossApp.Current.UserRecordPermissions.ContainsKey(key))
									{
										ReCrossApp.Current.UserRecordPermissions.Add(key, new Dictionary<long, Permission>());
									}
									if (ReCrossApp.Current.UserRecordPermissions[key].ContainsKey(current2.OwnerId.Value))
									{
										if (loggedUser.Group.IsOrEnabled)
										{
											ReCrossApp.Current.UserRecordPermissions[key][current2.OwnerId.Value].CanCreate |= value.CanCreate;
											ReCrossApp.Current.UserRecordPermissions[key][current2.OwnerId.Value].CanRead |= value.CanRead;
											ReCrossApp.Current.UserRecordPermissions[key][current2.OwnerId.Value].CanUpdate |= value.CanUpdate;
											ReCrossApp.Current.UserRecordPermissions[key][current2.OwnerId.Value].CanDelete |= value.CanDelete;
										}
										else
										{
											ReCrossApp.Current.UserRecordPermissions[key][current2.OwnerId.Value].CanCreate &= value.CanCreate;
											ReCrossApp.Current.UserRecordPermissions[key][current2.OwnerId.Value].CanRead &= value.CanRead;
											ReCrossApp.Current.UserRecordPermissions[key][current2.OwnerId.Value].CanUpdate &= value.CanUpdate;
											ReCrossApp.Current.UserRecordPermissions[key][current2.OwnerId.Value].CanDelete &= value.CanDelete;
										}
									}
									else
									{
										ReCrossApp.Current.UserRecordPermissions[key].Add(current2.OwnerId.Value, value);
									}
								}
								else if (!ReCrossApp.Current.UserGenericPermissions.ContainsKey(key))
								{
									ReCrossApp.Current.UserGenericPermissions.Add(key, value);
								}
								else if (loggedUser.Group.IsOrEnabled)
								{
									ReCrossApp.Current.UserGenericPermissions[key].CanCreate |= value.CanCreate;
									ReCrossApp.Current.UserGenericPermissions[key].CanRead |= value.CanRead;
									ReCrossApp.Current.UserGenericPermissions[key].CanUpdate |= value.CanUpdate;
									ReCrossApp.Current.UserGenericPermissions[key].CanDelete |= value.CanDelete;
								}
								else
								{
									ReCrossApp.Current.UserGenericPermissions[key].CanCreate &= value.CanCreate;
									ReCrossApp.Current.UserGenericPermissions[key].CanRead &= value.CanRead;
									ReCrossApp.Current.UserGenericPermissions[key].CanUpdate &= value.CanUpdate;
									ReCrossApp.Current.UserGenericPermissions[key].CanDelete &= value.CanDelete;
								}
							}
						}
					}
				}
			}
			ReCrossApp.Current.StartUpWindow();
			ReCrossApp.Current.StateData.IsLoggedIn = flag;
		}
		return true;
	}

	private bool Clean()
	{
		CurrentItem.Username = string.Empty;
		CurrentItem.Password = string.Empty;
		return true;
	}
}
