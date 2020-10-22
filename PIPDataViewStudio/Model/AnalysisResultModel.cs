using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIPDataViewStudio.Model
{
    public class AnalysisResultModel
    {
		public string TrackName { get; set; }
		public double MaxHeight { get; set; }
		public double MinHeight { get; set; }
		public double AverageHeight { get; set; }
		public DateTime TimeStart { get; set; }
		public DateTime TimeEnd { get; set; }
		public uint ItemCount { get; set; }
		public string Summary { get { return $" {TimeStart.ToString(@"yyyy/MM/dd hh:mm:ss")} -- {TimeEnd.ToString(@"yyyy/MM/dd hh:mm:ss")}\r\n Total {ItemCount} items have been calculated"; } }

    }
}
