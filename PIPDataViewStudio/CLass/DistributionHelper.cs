using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PIPDataViewStudio.Model
{
	public enum TRACK
	{
		TRACK1 = 0,
		TRACK2,
		TRACK3,
		TRACK4,
		TRACKALL,
	}

    public class DistributionHelper
    {
		private List<PIPDataModel> _data = new List<PIPDataModel>();
		private List<List<double>> _analysisData = new List<List<double>>();

		public DistributionHelper(ICollection<PIPDataModel> data)
		{
			SetPipDataModel(data);
		}

		public DistributionHelper() { }

		public List<DistributeModel> GetTrackDistribute(TRACK Track)
		{
			return GetDistribute(GetTrackData(Track),20);
		}

		private List<DistributeModel> GetDistribute(List<double> dataList, int SampleCount)
		{
			
			var list = new List<DistributeModel>();
			if (dataList == null || dataList.Count == 0)
				return list;
			var max = dataList.Max();
			var min = dataList.Min();

			var delta = (max - min)/SampleCount;
			double sum = min;
			double sum1 = min + delta;
			if (min == max)
			{
				list.Add(new DistributeModel()
				{
					Count = dataList.Count(),
					SleeveHeight = min
				});
			}
			else
			{
				while (sum < max)
				{
					list.Add(new DistributeModel()
					{
						Count = dataList.Where(it => it >= sum && it < sum1).Count(),
						SleeveHeight = sum
					});
					sum = sum1;
					sum1 = sum1 + delta;
				}
			}
			
			return list;
		}

		private List<List<double>> AnalysisAllTrackData()
		{
			var list = new List<List<double>>() { new List<double>(),new List<double>(), new List<double>(), new List<double>(), new List<double>()};
			
			foreach (var it in _data)
			{
				list[0].Add(it.SleevesInfoModel.SleeveHeight1);
				list[1].Add(it.SleevesInfoModel.SleeveHeight2);
				list[2].Add(it.SleevesInfoModel.SleeveHeight3);
				list[3].Add(it.SleevesInfoModel.SleeveHeight4);
				list[4].Add(it.SleevesInfoModel.SleeveHeight1);
				list[4].Add(it.SleevesInfoModel.SleeveHeight2);
				list[4].Add(it.SleevesInfoModel.SleeveHeight3);
				list[4].Add(it.SleevesInfoModel.SleeveHeight4);
			}
			return list;
		}

		private List<double> GetTrackData(TRACK Track)
		{
			if (Track == TRACK.TRACKALL)
				return _analysisData[4];
			else
				return _analysisData[(int)Track];
		}

		public void SetPipDataModel(ICollection<PIPDataModel> DataCollect)
		{
			_data = new List<PIPDataModel>(DataCollect);
			_analysisData = AnalysisAllTrackData();
		}
	}
}
