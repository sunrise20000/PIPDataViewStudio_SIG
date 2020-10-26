using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace PIPDataViewStudio.Convert
{
	public class DateTime2StringWithoutMS : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
		{
			return ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss");
		}

		public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
		{
			if (DateTime.TryParse(value.ToString(), out DateTime date))
			{
				return date;
			}
			else
				return DateTime.Now;
		}
	}
}
