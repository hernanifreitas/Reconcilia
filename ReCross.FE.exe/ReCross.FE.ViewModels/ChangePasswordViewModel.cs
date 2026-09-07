using System;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using ReCross.BE.DataObjects;
using Strills.WPF.UI.Commands;

namespace ReCross.FE.ViewModels;

public class ChangePasswordViewModel
{
	private RelayCommand _cmdChangePassword;

	private RelayCommand _cmdCancel;

	public bool Enabled => !ReCrossApp.Current.StateData.IsEditingMode;

	public User CurrentItem { get; set; }

	public ICommand CMDChangePassword => _cmdChangePassword;

	public ICommand CMDCancel => _cmdCancel;

	public ChangePasswordViewModel()
	{
		try
		{
			IQueryable<User> queryable = ReCrossApp.Current.CurrentDataContext.Users.Where((User user) => user.Id == ReCrossApp.Current.UserId);
			if (queryable != null && queryable.ToArray().Count() == 1)
			{
				CurrentItem = queryable.ToArray()[0];
				CurrentItem.BeginEdit();
				CurrentItem.Password = string.Empty;
				CurrentItem.NewPassword = string.Empty;
				CurrentItem.ConfirmPassword = string.Empty;
			}
		}
		catch (Exception ex)
		{
			CurrentItem = new User();
			string message = ex.Message;
		}
		_cmdChangePassword = new RelayCommand(delegate
		{
			ChangePassword();
		}, (object param) => CanChangePassword());
		_cmdCancel = new RelayCommand(delegate
		{
			Cancel();
		});
	}

	private bool CanChangePassword()
	{
		return !string.IsNullOrEmpty(CurrentItem.Password) && !string.IsNullOrEmpty(CurrentItem.NewPassword) && !string.IsNullOrEmpty(CurrentItem.ConfirmPassword) && CurrentItem.NewPassword == CurrentItem.ConfirmPassword;
	}

	private bool ChangePassword()
	{
		bool flag = CurrentItem.AuthenticateUser(CurrentItem.Password);
		if (flag)
		{
			CurrentItem.Password = CurrentItem.NewPassword;
			CurrentItem.EndEdit();
			ReCrossApp.Current.CurrentDataContext.SaveChanges();
			MessageBox.Show("Dados actualizados com sucesso!", "Confirmação", MessageBoxButton.OK, MessageBoxImage.Asterisk);
			((MainWindow)ReCrossApp.Current.Windows[0]).SetPreviousScreen();
			CurrentItem.Password = string.Empty;
			CurrentItem.NewPassword = string.Empty;
			CurrentItem.ConfirmPassword = string.Empty;
		}
		else
		{
			MessageBox.Show("Passord actual inválida!");
		}
		return flag;
	}

	private bool Cancel()
	{
		CurrentItem.CancelEdit();
		((MainWindow)ReCrossApp.Current.Windows[0]).SetPreviousScreen();
		return true;
	}
}
