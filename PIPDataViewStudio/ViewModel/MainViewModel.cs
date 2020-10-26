using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using Opc.Ua;
using OpcUaHelper;
using OpcUaHelper.Forms;
using PIPDataViewStudio.Model;
using PIPDataViewStudio.View;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using Opc.Ua.Client;
using SqlSugar;
using OfficeOpenXml;
using System.IO;
using System.Configuration;

namespace PIPDataViewStudio.ViewModel
{
	public class MainViewModel : ViewModelBase
	{
		#region Field
		const int TRACKNUMBER = 4;
		const int ROW = 30;
		const int TIME_READ_MILISECOND = 8000;
		const int MAX_COUNT = 10000;
		const string CONNECT_STRING = @"server=CFA874151001\ZENON_2012;uid=sa;pwd=SIGCombibloc;database=";
		const string DATABASE_NAME = "SleeveInfo";
		const string BACKUP_DATABASE_PATH = @"D:\";
		const string OPCSERVICE_ADDRESS = "opc.tcp://NVKSOJN8395X464:4980/Softing_dataFEED_OPC_Suite_Configuration1";//"opc.tcp://172.19.133.2:49380" /*"opc.tcp://10.20.16.37:4841"*/;
		string CURRENT_MODULE_PATH = AppDomain.CurrentDomain.BaseDirectory;
		const string EXPORT_PATH = "Export";


		private OpcUaClient opcUaClient = new OpcUaClient();
		private List<PIPDataModel> DataBuffer = new List<PIPDataModel>();
		private DistributionHelper DistributeDiagramHelper = new DistributionHelper();
		private List<List<DistributeModel>> _distributeDataList = new List<List<DistributeModel>>();
		private ObservableCollection<PIPDataModel> _dataCollect = new ObservableCollection<PIPDataModel>();
		private SqlSugarClient SugarClient;
		private List<string> SleeveHeightTags = new List<string>();
		private List<string> SleeveStatusTags = new List<string>();
		private List<string> SleevesTimeTags = new List<string>();
		private List<string> SleevesDateTags = new List<string>();
		private CancellationTokenSource cts = new CancellationTokenSource();
		private bool xStart = false;
		private bool _isOPCConnected = false;
		private bool _canStartEnabled = false;
		private bool _canStopEnabled = false;
		private string _strLog = "Ready";
		
		#endregion

		#region Ctr
		public MainViewModel()
		{
			try
			{
				InitOPCTags();
				CreateDB();
			}
			catch (Exception ex)
			{
				MessageBox.Show($"{ex.Message}----{ex.StackTrace}");
			}
		}
		#endregion

		#region Property
		public ObservableCollection<PIPDataModel> DataCollect
		{
			get { return _dataCollect; }
			set
			{
				_dataCollect = value;
				RaisePropertyChanged();
			}
		}
		
		public DataAnalysisHelper DataAnalysis { get; private set; }
		public ContentControl PageContent { get; set; }
		public List<List<DistributeModel>> DistributeDataList
		{
			get { return _distributeDataList; }
			set
			{
				_distributeDataList = value;
				RaisePropertyChanged();
			}
		}
		public bool IsOPCConnected
		{
			get { return _isOPCConnected; }
			set
			{
				if (_isOPCConnected != value)
				{
					_isOPCConnected = value;
					RaisePropertyChanged();
					StartStatusChanged(xStart);
				}
			}
		}

		public bool IsSqlServerConnected
		{
			get
			{
				return false;
			}
		}
		public bool CanStartEnabled
		{
			get { return _canStartEnabled; }
			set
			{
				if (_canStartEnabled != value)
				{
					_canStartEnabled = value;
					RaisePropertyChanged();
				}
			}
		}
		public bool CanStopEnabled
		{
			get { return _canStopEnabled; }
			set
			{
				if (_canStopEnabled != value)
				{
					_canStopEnabled = value;
					RaisePropertyChanged();
				}
			}
		}
		public LookUpModel LookupModel { get; set; } = new LookUpModel();

		public string StrLog {
			get { return _strLog; }
			set {
				_strLog = value;
				RaisePropertyChanged();
			}
		}
		#endregion

		#region Command
		public RelayCommand RunCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					try
					{
						Task.Run(() =>
						{
							ReadDataFromOPC();
						});
					}
					catch (Exception ex)
					{
						ClientUtils.HandleException("Error", ex);
					}
					//xStop = false;
				});
			}
		}
		public RelayCommand StopCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					cts.Cancel();
					xStart = false;
					StartStatusChanged(xStart);
					//opcUaClient.RemoveSubscription("EventA");
					//xStop = true;
				});
			}
		}
		public RelayCommand ConnectCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					ConnectOPCUA();
					IsOPCConnected = opcUaClient.Connected;
				});
			}
		}
		public RelayCommand ExportCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					try
					{
						ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;
						using (ExcelPackage package = new ExcelPackage())
						{
							ExcelWorksheet sheet = package.Workbook.Worksheets.Add("Sheet1");
							string[] ColumnHeaders = new string[] {"Index", "TimeReceived" , "Track1" , "Track2" , "Track3" , "Track4" };
							for (int j = 1; j < ColumnHeaders.Count()+1; j++)
							{
								sheet.Column(j).Width = j==1 ? 10:30;
								sheet.Cells[1, j].Value = ColumnHeaders[j - 1];
								sheet.Cells[1, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
								sheet.Cells[1, j].Style.Fill.BackgroundColor.SetColor (System.Drawing.Color.FromArgb(255, 128, 128, 128));
								sheet.Cells[1, j].Style.Font.Size = 20;
								sheet.Cells[1, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
								sheet.Cells[1, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
								sheet.Cells[1, j].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
							}

							//Record the data from second row
							int i = 2;
							foreach (var it in DataCollect) // row
							{
								sheet.Cells[i, 1].Value = it.Index;
								sheet.Cells[i, 2].Value = it.SleevesInfoModel.ReceivedTime.ToString(@"yyyy/MM/dd HH:mm:ss.fff");
								sheet.Cells[i, 3].Value = it.SleevesInfoModel.SleeveHeight1;
								sheet.Cells[i, 4].Value = it.SleevesInfoModel.SleeveHeight2;
								sheet.Cells[i, 5].Value = it.SleevesInfoModel.SleeveHeight3;
								sheet.Cells[i, 6].Value = it.SleevesInfoModel.SleeveHeight4;

								//var foreColor = it.IsForeColor ? System.Drawing.Color.Red : System.Drawing.Color.Black;
								//sheet.Cells[i, 1].Style.Font.Color.SetColor(foreColor);
								for (int j = 1; j < ColumnHeaders.Count() + 1; j++) //column
								{
									sheet.Cells[i, j].Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
									var backgroundColor = i % 2 == 0 ? System.Drawing.Color.FromArgb(255, 197, 217, 241) : System.Drawing.Color.FromArgb(255, 235, 241, 222);
									sheet.Cells[i, j].Style.Fill.BackgroundColor.SetColor(backgroundColor);
									sheet.Cells[i, j].Style.Border.BorderAround(OfficeOpenXml.Style.ExcelBorderStyle.Thin);
									sheet.Cells[i, j].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
									sheet.Cells[i, j].Style.VerticalAlignment = OfficeOpenXml.Style.ExcelVerticalAlignment.Center;
								}
								i++;
							}
							var now = DateTime.Now;
							string fileName = $"{CURRENT_MODULE_PATH}{EXPORT_PATH}\\{now.ToString("MM月dd日HH时mm分ss秒")}.xlsx";
							using (Stream stream = new FileStream(fileName, FileMode.Create))
							{
								package.SaveAs(stream);
								MessageBox.Show($"Export all the items to {fileName} successfully", "Successful", MessageBoxButton.OK, MessageBoxImage.Information);
							}
						}
					}
					catch (Exception ex)
					{
						MessageBox.Show(ex.Message, "Error", MessageBoxButton.OKCancel, MessageBoxImage.Error);
					}
				});
			}
		}
		public RelayCommand AnalysisCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					DataAnalysis = new DataAnalysisHelper(DataCollect);
				});
			}
		}
		public RelayCommand HistogramCommand
		{
			get
			{
				return new RelayCommand(() =>
				{
					DistributeDataList.Clear();
					DistributeDiagramHelper.SetPipDataModel(DataCollect);
					DistributeDataList.Add(DistributeDiagramHelper.GetTrackDistribute(TRACK.TRACK1));
					DistributeDataList.Add(DistributeDiagramHelper.GetTrackDistribute(TRACK.TRACK2));
					DistributeDataList.Add(DistributeDiagramHelper.GetTrackDistribute(TRACK.TRACK3));
					DistributeDataList.Add(DistributeDiagramHelper.GetTrackDistribute(TRACK.TRACK4));
					DistributeDataList.Add(DistributeDiagramHelper.GetTrackDistribute(TRACK.TRACKALL));
				});
			}
		}
		public RelayCommand QueryCommand { get { return new RelayCommand(()=> {
			var CommandStartTime = DateTime.Now;
			Int16 MinHeight1 = LookupModel.MinHeight.Track1, MinHeight2 = LookupModel.MinHeight.Track2, MinHeight3 = LookupModel.MinHeight.Track3, MinHeight4 = LookupModel.MinHeight.Track4;
			Int16 MaxHeight1 = LookupModel.MaxHeight.Track1, MaxHeight2 = LookupModel.MaxHeight.Track2, MaxHeight3 = LookupModel.MaxHeight.Track3, MaxHeight4 = LookupModel.MaxHeight.Track4;
			bool xEnableTrack1 = MinHeight1 <= 0 && MaxHeight1 <= 0;
			bool xEnableTrack2 = MinHeight2 <= 0 && MaxHeight2 <= 0;
			bool xEnableTrack3 = MinHeight3 <= 0 && MaxHeight3 <= 0;
			bool xEnableTrack4 = MinHeight4 <= 0 && MaxHeight4 <= 0;

			var collect = SugarClient.Queryable<SleevesInfo>().Where(it => 
			(it.ReceivedTime <= LookupModel.TimeEnd && it.ReceivedTime >= LookupModel.TimeStart) &&
			(xEnableTrack1 == true || it.SleeveHeight1 >= MinHeight1 && it.SleeveHeight1 <= MaxHeight1) &&
			(xEnableTrack2 == true || it.SleeveHeight2 >= MinHeight2 && it.SleeveHeight2 <= MaxHeight2) &&
			(xEnableTrack3 == true || it.SleeveHeight3 >= MinHeight3 && it.SleeveHeight3 <= MaxHeight3) &&
			(xEnableTrack4 == true || it.SleeveHeight4 >= MinHeight4 && it.SleeveHeight4 <= MaxHeight4)).OrderBy(it => it.ReceivedTime).ToArray();

			// FIll the data to the DataCollect
			
			if (collect != null && collect.Count() > 0)
			{
				List<PIPDataModel> retList = new List<PIPDataModel>();
				var timeStart = collect.ElementAt(0).ReceivedTime;
				foreach (var it in collect)
				{
					retList.Add(new PIPDataModel(new List<short> { it.SleeveHeight1, it.SleeveHeight2, it.SleeveHeight3, it.SleeveHeight4 }, 
																	timeStart, 
																	it.ReceivedTime, 
																	new List<short> { it.Status1, it.Status2, it.Status3, it.Status4 },
																	retList.Count + 1));
				}
				DataCollect = new ObservableCollection<PIPDataModel>(retList);
				ShowLog($"Total {DataCollect.Count} items found  |  {(DateTime.Now - CommandStartTime).TotalSeconds.ToString("0.00")} seconds");
			}
		}); } }
		#endregion

		#region  Privage function

		private async void ConnectOPCUA()
		{
			try
			{
				var OpcServiceAddress = ConfigurationManager.AppSettings["OpcServiceAddress"];
				var dt = DateTime.Parse("15:26:07.683");
				opcUaClient.UserIdentity = new UserIdentity("Administrator", "SIGCombibloc");
				await opcUaClient.ConnectServer(OpcServiceAddress);
				MessageBox.Show("Connect successfully", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
				//opcUaClient.AddSubscription("EventA", $"ns=2;s=Siemens_1.RecordInformation.RecordTrack1Time[0]", EventACallback);
			}
			catch (Exception ex)
			{
				ClientUtils.HandleException("Connected Failed", ex);
			}


			/*FormBrowseServer formBrowseServer = new FormBrowseServer(OPCService_Address);
			formBrowseServer.ShowDialog();*/
		}

		private int ReadDataFromOPC()
		{
			Application.Current.Dispatcher.Invoke(() =>
			{
				DataCollect.Clear();
				ShowLog("Ready");
			});
			DateTime TimeStart = DateTime.Now;
			var LHeight = new List<Int16>();
			var LStatus = new List<Int16>();
			List<PIPDataModel> modelList = new List<PIPDataModel>();
			cts = new CancellationTokenSource();
			DateTime OriginPLCTime = DateTime.Now;
			Int32 IndexCount = 0;


			//
			
			while (!cts.Token.IsCancellationRequested)
			{

				StartStatusChanged(true);

				//opcUaClient.WriteNode<string>(SleevesTimeTags[0],DateTime.Now.ToShortTimeString());

				var SleeveHeightRecord = opcUaClient.ReadNodes<Int16[]>(SleeveHeightTags.ToArray());
				var SleeveStatusRecord = opcUaClient.ReadNodes<Int16[]>(SleeveStatusTags.ToArray());
				var SleeveTimeRecord = opcUaClient.ReadNodes<string>(SleevesTimeTags.ToArray());
				var SleeveDateRecord = opcUaClient.ReadNodes<string>(SleevesDateTags.ToArray());
				
				if (SleeveHeightRecord[0] == null || SleeveStatusRecord[0] == null || SleeveTimeRecord[0] == null || SleeveDateRecord[0] == null)
				{
					cts.Cancel();
					xStart = false;
					StartStatusChanged(xStart);
					break;
				}

				if (!xStart)
				{
					//TimeStart = DateTime.Parse($"{SleeveDateRecord[0]} {SleeveTimeRecord[0]}");
					OriginPLCTime = DateTime.Parse($"{SleeveDateRecord[0]} {SleeveTimeRecord[0]}");
					TimeStart = DateTime.Now;
					xStart = true;
					StartStatusChanged(xStart);
				}

				modelList.Clear();

				short UpdateIndex = 0;
				// Just fetch top 30 data from list
				TimeSpan TimeSpan2OriginPLCTime = new TimeSpan(0);
				
				for (int i = 0; i < ROW; i++)
				{
					var TimeSpan2FirstPackage = DateTime.Parse($"{SleeveDateRecord[i]} {SleeveTimeRecord[i]}") - DateTime.Parse($"{SleeveDateRecord[0]} {SleeveTimeRecord[0]}");
					if (TimeSpan2FirstPackage.TotalSeconds >= 0)
					{
						UpdateIndex++;
					}
					else
					{
						Console.WriteLine($"--------------UpdateIndex={UpdateIndex}--------------------------");
						break;
					}
				}

				int Border = UpdateIndex - 12;
				// Everytime update 10 items.
				for (int i = 0; i < 10; i++)
				{
					int index = Border < 0 ? (30 + Border + i) % 30 : Border + i;
					LHeight.Clear();
					LStatus.Clear();
					for (int m = 0; m < TRACKNUMBER; m++)
					{
						LHeight.Add(SleeveHeightRecord[m][index]);
						LStatus.Add(SleeveStatusRecord[m][index]);
					}
					TimeSpan2OriginPLCTime = DateTime.Parse($"{SleeveDateRecord[index]} {SleeveTimeRecord[index]}") - OriginPLCTime;
					var pipDataModel = new PIPDataModel(LHeight, TimeStart, TimeStart + TimeSpan2OriginPLCTime, LStatus, modelList.Count + 1);
					modelList.Add(pipDataModel);
				}

				var listCommit = AnalysisData(modelList);
				DataBuffer = new List<PIPDataModel>(modelList);
				var CommitDataModel = from data in listCommit select data.SleevesInfoModel;

				//foreach (var data in listCommit)
				//{
				//	SugarClient.Insertable(data.SleevesInfoModel).ExecuteCommand();
				//}
				if (CommitDataModel != null && CommitDataModel.Count() > 0)
				{
					foreach (var d in listCommit)
					{
						d.Index = ++IndexCount;
					}
					SugarClient.Insertable(CommitDataModel.ToArray()).ExecuteCommand();
				}
				//SugarClient.CommitTran();

				Application.Current.Dispatcher.Invoke(() =>
				{

					//DataCollect = new ObservableCollection<PIPDataModel>(listCommit);
					DataCollect = new ObservableCollection<PIPDataModel>(DataCollect.Concat(listCommit));
					if (DataCollect.Count > 10000)
						for (int i = 0; i < listCommit.Count; i++)
							DataCollect.RemoveAt(0);
				});

				Thread.Sleep(TIME_READ_MILISECOND);
			}
			Application.Current.Dispatcher.Invoke(()=> {
				ShowLog($"Total {DataCollect.Count} Items added");
			});
			return 0;
		}

		private void EventACallback(string Key, MonitoredItem item, MonitoredItemNotificationEventArgs e)
		{
			switch (Key)
			{
				case "EventA":
					Application.Current.Dispatcher.Invoke(() =>
					{
						ReadDataFromOPC();
					});
					break;
				default:
					break;
			}
		}

		private List<PIPDataModel> AnalysisData(List<PIPDataModel> data)
		{
			var retList = new List<PIPDataModel>();
			foreach (var d in data)
			{
				var datafind = DataBuffer.Where(it => it.TimeReceived1.Equals(d.TimeReceived1));

				if (datafind == null || datafind.Count() == 0)
				{
					retList.Add(d);
				}
			}
			Console.WriteLine($"添加了{retList.Count}个记录");
			return retList;
		}

		private void CreateDB()
		{

			var connString = ConfigurationManager.AppSettings["ConnString"];
			SugarClient = new SqlSugarClient(new ConnectionConfig()
			{
				ConnectionString = connString,
				DbType = DbType.SqlServer,
				IsAutoCloseConnection = true,
				InitKeyType = InitKeyType.Attribute

			});
			SugarClient.Open();
			CreateTable(false, 50, typeof(SleevesInfo));
		
		}

		private void CreateTable(bool Backup = false, int StringDefaultLength = 50, params Type[] types)
		{
			SugarClient.CodeFirst.SetStringDefaultLength(StringDefaultLength);
			SugarClient.DbMaintenance.CreateDatabase(DATABASE_NAME, BACKUP_DATABASE_PATH);
			if (Backup)
			{
				SugarClient.CodeFirst.BackupTable().InitTables(types);
			}
			else
			{
				SugarClient.CodeFirst.InitTables(types);
			}
		}

		private void InitOPCTags()
		{
			SleeveHeightTags.Clear();
			SleeveStatusTags.Clear();
			SleevesTimeTags.Clear();
			SleevesDateTags.Clear();
			for (int i = 0; i < TRACKNUMBER; i++)
			{
				SleeveHeightTags.Add($"ns=2;s=Siemens_1.MeasureHalfFIlling.TrackRecord{i + 1}");
				SleeveStatusTags.Add($"ns=2;s=Siemens_1.RecordInformation.RecordTrack{i + 1}Status");
			}
			for (int j = 0; j < ROW; j++)
			{
				SleevesTimeTags.Add($"ns=2;s=Siemens_1.RecordInformation.RecordTrack1Time[{j}]");
				SleevesDateTags.Add($"ns=2;s=Siemens_1.RecordInformation.RecordTrack1Date[{j}]");
			}
		}

		private void StartStatusChanged(bool newStatus)
		{
			CanStartEnabled = IsOPCConnected && !newStatus;
			CanStopEnabled = IsOPCConnected && newStatus;
		}

		private void ShowLog(string StrLog)
		{
			this.StrLog = StrLog;
		}
		#endregion
	}
}