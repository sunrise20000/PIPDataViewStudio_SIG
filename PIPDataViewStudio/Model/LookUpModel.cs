using PIPDataViewStudio.CLass;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PIPDataViewStudio.Model
{
	public class LookUpModel
	{
		public Int16[] MinHeight { get; set; } = new Int16[4];
		public Int16[] MaxHeight { get; set; } = new Int16[4];

		[EditorAttribute(typeof(DetailedDateTime), typeof(UITypeEditor))]
		public DateTime TimeStart { get; set; } = DateTime.Now;

		public DateTime TimeEnd { get; set; } = DateTime.Now - new TimeSpan(0, 2, 0, 0);
		//public TRACK Track { get; set; }
	}
}
