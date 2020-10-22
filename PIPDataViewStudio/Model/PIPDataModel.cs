using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIPDataViewStudio.Model
{
	public class PIPDataModel
	{

		public SleevesInfo SleevesInfoModel { get; set; } = new SleevesInfo();
		private DateTime timeStart = DateTime.Now;
		public bool IsEmpty1 { get; set; }
		public bool IsEmpty2 { get; set; }
		public bool IsEmpty3 { get; set; }
		public bool IsEmpty4 { get; set; }
		public Int32 Index { get; set; }
		public double Ticks { get { return (SleevesInfoModel.ReceivedTime - timeStart).TotalSeconds; } }
		public DateTime TimeReceived1 { get { return SleevesInfoModel.ReceivedTime.ToUniversalTime(); } }

		public PIPDataModel(List<Int16> SleeveHeight,DateTime TimeStart, DateTime TimeReceived ,List<Int16> StatusList, Int32 Index, string Comment = "")
		{
			SleevesInfoModel.SleeveHeight1 = SleeveHeight[0];
			SleevesInfoModel.SleeveHeight2 = SleeveHeight[1];
			SleevesInfoModel.SleeveHeight3 = SleeveHeight[2];
			SleevesInfoModel.SleeveHeight4 = SleeveHeight[3];
			SleevesInfoModel.Status1 = StatusList[0];
			SleevesInfoModel.Status2 = StatusList[1];
			SleevesInfoModel.Status3 = StatusList[2];
			SleevesInfoModel.Status4 = StatusList[3];
			this.Index = Index;

			this.IsEmpty1 = SleevesInfoModel.Status4 == 0;
			this.IsEmpty2 = SleevesInfoModel.Status2 == 0;
			this.IsEmpty3 = SleevesInfoModel.Status3 == 0;
			this.IsEmpty4 = SleevesInfoModel.Status4 == 0;
			this.SleevesInfoModel.Comment = Comment;
			this.SleevesInfoModel.ReceivedTime = TimeReceived;
			timeStart = TimeStart;
		}

	}
}
