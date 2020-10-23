using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIPDataViewStudio.Model
{
	public struct SleeveLimitHeight
	{
		public Int16 Track1 { get; set; }
		public Int16 Track2 { get; set; }
		public Int16 Track3 { get; set; }
		public Int16 Track4 { get; set; }

		public SleeveLimitHeight(Int16 Track1, Int16 Track2, Int16 Track3, Int16 Track4)
		{
			this.Track1 = Track1;
			this.Track2 = Track2;
			this.Track3 = Track3;
			this.Track4 = Track4;

		}
	}
}
