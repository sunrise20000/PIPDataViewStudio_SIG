using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;

namespace PIPDataViewStudio.Convert
{
	public class IsReadSuccessful2Color : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			SolidColorBrush brush = null;
			bool b = bool.Parse(value.ToString());
			if (b)
				brush = new SolidColorBrush(Colors.Transparent);
			else
				brush = new SolidColorBrush(Colors.Red);
			return brush;
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
}
