using System.ComponentModel;

namespace ReCross.FE.ViewModels;

public class NotifyBindings : INotifyPropertyChanged
{
	private int _notifyAccountMovSum = 0;

	private int _notifyBankMovSum = 0;

	private int _notifyAccountMov = 0;

	private int _notifyBankMov = 0;

	private int _notifyReconciliation = 0;

	private int _notifyMovementChecked = 0;

	public int NotifyAccountMovSum
	{
		get
		{
			return _notifyAccountMovSum;
		}
		set
		{
			_notifyAccountMovSum = value;
			OnPropertyChanged("NotifyAccountMovSum");
		}
	}

	public int NotifyBankMovSum
	{
		get
		{
			return _notifyBankMovSum;
		}
		set
		{
			_notifyBankMovSum = value;
			OnPropertyChanged("NotifyBankMovSum");
		}
	}

	public int NotifyAccountMov
	{
		get
		{
			return _notifyAccountMov;
		}
		set
		{
			_notifyAccountMov = value;
			OnPropertyChanged("NotifyAccountMov");
		}
	}

	public int NotifyBankMov
	{
		get
		{
			return _notifyBankMov;
		}
		set
		{
			_notifyBankMov = value;
			OnPropertyChanged("NotifyBankMov");
		}
	}

	public int NotifyReconciliation
	{
		get
		{
			return _notifyReconciliation;
		}
		set
		{
			_notifyReconciliation = value;
			OnPropertyChanged("NotifyReconciliation");
		}
	}

	public int NotifyMovementChecked
	{
		get
		{
			return _notifyMovementChecked;
		}
		set
		{
			_notifyMovementChecked = value;
			OnPropertyChanged("NotifyMovementChecked");
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void OnPropertyChanged(string name)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
	}
}
