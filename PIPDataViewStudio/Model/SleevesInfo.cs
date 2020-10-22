using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;
namespace PIPDataViewStudio.Model
{
	[SugarTable("SleevesInfo")]
	public class SleevesInfo
	{
		public DateTime ReceivedTime { get; set; }
		public Int16 SleeveHeight1 { get; set; }
		public Int16 SleeveHeight2 { get; set; }
		public Int16 SleeveHeight3 { get; set; }
		public Int16 SleeveHeight4 { get; set; }
		public Int16 Status1 { get; set; } = 0;
		public Int16 Status2 { get; set; } = 0;
		public Int16 Status3 { get; set; } = 0;
		public Int16 Status4 { get; set; } = 0;
		public string Comment { get; set; } = "";

	}
}
