using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIPDataViewStudio.Model
{
    public class DataAnalysisHelper
    {
		private ICollection<PIPDataModel> _collect;
		public List<AnalysisResultModel> ResultList { get; set; } = new List<AnalysisResultModel>();
		public DataAnalysisHelper(ICollection<PIPDataModel> DataCollect)
		{
			_collect = DataCollect;
			if(_collect != null && _collect.Count != 0)
				Calculate();
			
		}
		
		private void Calculate()
		{

			//Track1
			ResultList = new List<AnalysisResultModel>() {

				new AnalysisResultModel() {
					 AverageHeight = _collect.Sum(it => it.SleevesInfoModel.SleeveHeight1) / _collect.Count,
					 MinHeight = _collect.OrderBy(it => it.SleevesInfoModel.SleeveHeight1).First().SleevesInfoModel.SleeveHeight1,
					 MaxHeight = _collect.OrderBy(it => it.SleevesInfoModel.SleeveHeight1).Last().SleevesInfoModel.SleeveHeight1,
					 TrackName = "Track1",
					 TimeStart = _collect.OrderBy(it => it.SleevesInfoModel.ReceivedTime).First().SleevesInfoModel.ReceivedTime,
					 TimeEnd = _collect.OrderBy(it => it.SleevesInfoModel.ReceivedTime).Last().SleevesInfoModel.ReceivedTime,
					 ItemCount = (uint)_collect.Count
					},
				new AnalysisResultModel() {
					 AverageHeight = _collect.Sum(it => it.SleevesInfoModel.SleeveHeight2) / _collect.Count,
					 MinHeight = _collect.OrderBy(it => it.SleevesInfoModel.SleeveHeight2).First().SleevesInfoModel.SleeveHeight2,
					 MaxHeight = _collect.OrderBy(it => it.SleevesInfoModel.SleeveHeight2).Last().SleevesInfoModel.SleeveHeight2,
					 TrackName = "Track2",
					 TimeStart = _collect.OrderBy(it => it.SleevesInfoModel.ReceivedTime).First().SleevesInfoModel.ReceivedTime,
					 TimeEnd = _collect.OrderBy(it => it.SleevesInfoModel.ReceivedTime).Last().SleevesInfoModel.ReceivedTime,
					 ItemCount = (uint)_collect.Count

					},
				new AnalysisResultModel() {
					 AverageHeight = _collect.Sum(it => it.SleevesInfoModel.SleeveHeight3) / _collect.Count,
					 MinHeight = _collect.OrderBy(it => it.SleevesInfoModel.SleeveHeight3).First().SleevesInfoModel.SleeveHeight3,
					 MaxHeight = _collect.OrderBy(it => it.SleevesInfoModel.SleeveHeight3).Last().SleevesInfoModel.SleeveHeight3,
					 TrackName = "Track3",
					 TimeStart = _collect.OrderBy(it => it.SleevesInfoModel.ReceivedTime).First().SleevesInfoModel.ReceivedTime,
					 TimeEnd = _collect.OrderBy(it => it.SleevesInfoModel.ReceivedTime).Last().SleevesInfoModel.ReceivedTime,
					 ItemCount = (uint)_collect.Count
					},
				new AnalysisResultModel() {
					 AverageHeight = _collect.Sum(it => it.SleevesInfoModel.SleeveHeight4) / _collect.Count,
					 MinHeight = _collect.OrderBy(it => it.SleevesInfoModel.SleeveHeight4).First().SleevesInfoModel.SleeveHeight4,
					 MaxHeight = _collect.OrderBy(it => it.SleevesInfoModel.SleeveHeight4).Last().SleevesInfoModel.SleeveHeight4,
					 TrackName = "Track4",
					 TimeStart = _collect.OrderBy(it => it.SleevesInfoModel.ReceivedTime).First().SleevesInfoModel.ReceivedTime,
					 TimeEnd = _collect.OrderBy(it => it.SleevesInfoModel.ReceivedTime).Last().SleevesInfoModel.ReceivedTime,
					 ItemCount = (uint)_collect.Count
					},
			};

		}		
	}
}
