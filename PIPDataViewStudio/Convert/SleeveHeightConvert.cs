using PIPDataViewStudio.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PIPDataViewStudio.Convert
{
	public class SleeveHeightConvert : IValueConverter
	{
		static SleeveLimitHeight _originalValue;
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			_originalValue = ((SleeveLimitHeight)value);

			if (parameter.ToString() == "Track1")
				return ((SleeveLimitHeight)value).Track1;
			if (parameter.ToString() == "Track2")
				return ((SleeveLimitHeight)value).Track2;
			if (parameter.ToString() == "Track3")
				return ((SleeveLimitHeight)value).Track3;
			if (parameter.ToString() == "Track4")
				return ((SleeveLimitHeight)value).Track4;
			return _originalValue;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (parameter.ToString() == "Track1")
				_originalValue = new SleeveLimitHeight(Int16.Parse(value.ToString()), _originalValue.Track2, _originalValue.Track3, _originalValue.Track4);
			if (parameter.ToString() == "Track2")
				_originalValue = new SleeveLimitHeight(_originalValue.Track1, Int16.Parse(value.ToString()), _originalValue.Track3, _originalValue.Track4);
			if (parameter.ToString() == "Track3")
				_originalValue = new SleeveLimitHeight(_originalValue.Track1, _originalValue.Track2, Int16.Parse(value.ToString()),  _originalValue.Track4);
			if (parameter.ToString() == "Track4")
				_originalValue = new SleeveLimitHeight(_originalValue.Track1, _originalValue.Track2, _originalValue.Track3,Int16.Parse(value.ToString()));

			return _originalValue;
		}
	}
}
