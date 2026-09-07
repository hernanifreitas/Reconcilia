using System;
using System.CodeDom.Compiler;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace ReCross.FE.ReCrossService;

[Serializable]
[GeneratedCode("System.Runtime.Serialization", "4.0.0.0")]
[DataContract(Name = "Customer", Namespace = "http://schemas.datacontract.org/2004/07/ReCross.License")]
[DebuggerStepThrough]
public class Customer : IExtensibleDataObject, INotifyPropertyChanged
{
	[NonSerialized]
	private ExtensionDataObject extensionDataField;

	[OptionalField]
	private bool AllowedField;

	[OptionalField]
	private string IDField;

	[OptionalField]
	private int LicenseCountField;

	[OptionalField]
	private DateTime LicenseDateField;

	[OptionalField]
	private int LicenseDaysField;

	[Browsable(false)]
	public ExtensionDataObject ExtensionData
	{
		get
		{
			return extensionDataField;
		}
		set
		{
			extensionDataField = value;
		}
	}

	[DataMember]
	public bool Allowed
	{
		get
		{
			return AllowedField;
		}
		set
		{
			if (!AllowedField.Equals(value))
			{
				AllowedField = value;
				RaisePropertyChanged("Allowed");
			}
		}
	}

	[DataMember]
	public string ID
	{
		get
		{
			return IDField;
		}
		set
		{
			if (!object.ReferenceEquals(IDField, value))
			{
				IDField = value;
				RaisePropertyChanged("ID");
			}
		}
	}

	[DataMember]
	public int LicenseCount
	{
		get
		{
			return LicenseCountField;
		}
		set
		{
			if (!LicenseCountField.Equals(value))
			{
				LicenseCountField = value;
				RaisePropertyChanged("LicenseCount");
			}
		}
	}

	[DataMember]
	public DateTime LicenseDate
	{
		get
		{
			return LicenseDateField;
		}
		set
		{
			if (!LicenseDateField.Equals(value))
			{
				LicenseDateField = value;
				RaisePropertyChanged("LicenseDate");
			}
		}
	}

	[DataMember]
	public int LicenseDays
	{
		get
		{
			return LicenseDaysField;
		}
		set
		{
			if (!LicenseDaysField.Equals(value))
			{
				LicenseDaysField = value;
				RaisePropertyChanged("LicenseDays");
			}
		}
	}

	public event PropertyChangedEventHandler PropertyChanged;

	protected void RaisePropertyChanged(string propertyName)
	{
		PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
	}
}
