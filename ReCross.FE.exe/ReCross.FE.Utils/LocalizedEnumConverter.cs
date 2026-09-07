using System;
using ReCross.FE.Properties;
using Strills.WPF.Localization.Converters;

namespace ReCross.FE.Utils;

internal class LocalizedEnumConverter : ResourceEnumConverter
{
	public LocalizedEnumConverter(Type type)
		: base(type, ReCross.FE.Properties.Resources.ResourceManager)
	{
	}
}
