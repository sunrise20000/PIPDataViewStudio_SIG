using Microsoft.VisualStudio.TestTools.UnitTesting;
using PIPDataViewStudio.ViewModel;
using PIPDataViewStudio.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace PIPDataViewStudio.ViewModel.Tests
{
	[TestClass()]
	public class MainViewModelTests
	{
		private MainViewModel vm = new MainViewModel();
		List<PIPDataModel> modelList = new List<PIPDataModel>()
		{
			new PIPDataModel(new List<Int16>(){ 20,20,2,20,20}, DateTime.Now, DateTime.Now, new List<short>(){ 1,1,1,1}),
			new PIPDataModel(new List<Int16>(){ 40,20,2,20,20}, DateTime.Now - new TimeSpan(1,0,0,0), DateTime.Now - new TimeSpan(1,0,0,0), new List<short>(){ 1,1,1,1}),
			new PIPDataModel(new List<Int16>(){ 60,20,2,20,20}, DateTime.Now - new TimeSpan(2,0,0,0), DateTime.Now - new TimeSpan(2,0,0,0), new List<short>(){ 1,1,1,1}),
			new PIPDataModel(new List<Int16>(){ 80,20,2,20,20}, DateTime.Now - new TimeSpan(3,0,0,0), DateTime.Now - new TimeSpan(3,0,0,0), new List<short>(){ 1,1,1,1}),
			new PIPDataModel(new List<Int16>(){ 100,20,2,20,20}, DateTime.Now - new TimeSpan(4,0,0,0), DateTime.Now - new TimeSpan(4,0,0,0), new List<short>(){ 1,1,1,1}),
		};
		[TestMethod()]
		public void CreateDBTest()
		{
			//var data = from d in modelList select d.SleevesInfoModel;
			//vm.ReadDataFromOPC();
			var data = from d in modelList select d.SleevesInfoModel;
			vm.CreateDB();
			vm.CreateTable();
			vm.SugarClient.Insertable(data.ToArray()).ExecuteCommand();
			//vm.SugarClient.CommitTran();
		}

	}
}