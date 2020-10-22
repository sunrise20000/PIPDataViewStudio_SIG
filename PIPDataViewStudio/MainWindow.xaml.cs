using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using DevExpress.Xpf.Ribbon;
using PIPDataViewStudio.View;

namespace PIPDataViewStudio
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : DXRibbonWindow
	{
		public MainWindow()
		{
			InitializeComponent();
		}

		private void DataGridView_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
		{
			NvFrame.Navigate(typeof(DataGridView),NvFrame.DataContext);
		}

		private void ChartView_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
		{
			NvFrame.Navigate(typeof(ChartView),NvFrame.DataContext);
		}
		private void AnalysisView_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
		{
			NvFrame.Navigate(typeof(AnalysisView), NvFrame.DataContext);
		}

		private void Histogram_ItemClick(object sender, DevExpress.Xpf.Bars.ItemClickEventArgs e)
		{
			NvFrame.Navigate(typeof(HistogramView), RootWindow.DataContext);
		}
	}
}
