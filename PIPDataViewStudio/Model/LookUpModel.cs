using PIPDataViewStudio.CLass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Xceed.Wpf.Toolkit.PropertyGrid.Attributes;

namespace PIPDataViewStudio.Model
{
	public class LookUpModel
	{
		[Category("Height")]
		[PropertyOrderAttribute(0)]
		public SleeveLimitHeight MinHeight { get; set; }
		[Category("Height")]
		[PropertyOrderAttribute(1)]
		public SleeveLimitHeight MaxHeight { get; set; }

		[Category("DateTime")]
		[PropertyOrderAttribute(2)]
		//[Editor(typeof(DetailedDateTime), typeof(UITypeEditor))]
		public DateTime TimeStart { get; set; } = DateTime.Now;

		[Category("DateTime")]
		[PropertyOrderAttribute(3)]
		//[Editor(typeof(DetailedDateTime), typeof(UITypeEditor))]
		public DateTime TimeEnd { get; set; } = DateTime.Now - new TimeSpan(0, 2, 0, 0);
		//public TRACK Track { get; set; }
	}
}
