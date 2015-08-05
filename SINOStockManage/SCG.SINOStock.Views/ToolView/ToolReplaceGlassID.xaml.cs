using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace SCG.SINOStock.Views
{
    /// <summary>
    /// ToolReplaceGlassID.xaml 的交互逻辑
    /// </summary>
    public partial class ToolReplaceGlassID : Window
    {
        protected IEventAggregator _eventAggregator;
        public ToolReplaceGlassID()
        {
            InitializeComponent();
            _eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            this._eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {

                switch (param.cmdName)
                {
                    case CmdName.New:

                        txtGlassID.Text = string.Empty;
                        ReadComTest();
                        break;

                    case CmdName.Close:
                        this.Close();
                        break;
                    default:
                        break;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "ToolReplaceGlassIDView");


            ScanComExecute = (s, e) =>
            {
                txtGlassID.Text = s.ToString();
                isDispose = false;
            };
        }
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }



        protected void ReadComTest()
        {
            // Common.ReadCOM.serialport = new SerialPort();
            if (Common.ReadCOM.serialport.IsOpen)
            {
                Common.ReadCOM.serialport.Close();
            }
            Common.ReadCOM.serialport = new SerialPort();

            DataSet ds = new DataSet();
            portName = ConfigurationManager.AppSettings["PortName"].ToString();
            baudRate = int.Parse(ConfigurationManager.AppSettings["BaudRate"]);
            dataBits = int.Parse(ConfigurationManager.AppSettings["DataBits"]);
            serialNumber = ConfigurationManager.AppSettings["SerialNumber"].ToString();
            SocketPort = int.Parse(ConfigurationManager.AppSettings["SocketPort"]);
            this.OpenPort();
            Common.ReadCOM.serialport.DataReceived += DataReceived;
        }
        #region 读取串口
        //  SerialPort serialport = new SerialPort();
        private string portName = "";   //串口
        private int baudRate = 0;         //速率
        private int dataBits = 0;         //数据位        
        string strReceive = "";
        string serialNumber = "";
        int SocketPort = 0;
        protected bool isDispose = false;//是否在处理COM数据,true为正在处理，false空闲
        private void DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                Thread.Sleep(100);
                strReceive = Common.ReadCOM.serialport.ReadExisting();
                if (string.IsNullOrEmpty(strReceive))
                {
                    return;
                }
                else
                {
                    if (!isDispose)
                    {
                        BackgroundWorker bgWorker = new BackgroundWorker();
                        bgWorker.DoWork += new DoWorkEventHandler(bgWorker_DoWork);
                        bgWorker.RunWorkerAsync(strReceive);
                        return;
                    }
                }

            }
            catch (System.Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        private void bgWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            isDispose = true;
            //this.Invoke((EventHandler)(delegate
            //{
            //    this.txtGlassID.Text = strReceive;
            //    this.txtGlassID.Refresh();
            //    KeyEventArgs ev = new KeyEventArgs(Keys.Enter);
            //    txtGlassID_KeyDown(sender, ev);
            //    isDispose = false;

            //}));

            ThreadHelper.BeginInvokeOnUIThread(() =>
            {
                //  GlassID = strReceive.Trim();

                if (ScanComExecute != null)
                {
                    ScanComExecute(strReceive.Trim(), null);
                }
                //  isDispose = false;
                //    AddStockDetailAsyns();
            });
        }
        private void OpenPort()
        {
            if (Common.ReadCOM.serialport.IsOpen)
            {
                Common.ReadCOM.serialport.Close();
            }
            Common.ReadCOM.serialport.PortName = portName;
            Common.ReadCOM.serialport.BaudRate = baudRate;
            Common.ReadCOM.serialport.Parity = Parity.None;
            Common.ReadCOM.serialport.DataBits = dataBits;
            Common.ReadCOM.serialport.StopBits = StopBits.One;
            //打开串口
            try
            {
                Common.ReadCOM.serialport.Open();
            }
            catch (Exception ex)
            {
                Common.MessageBox.Show(ex.Message);
            }
        }

        protected EventHandler ScanComExecute;
        #endregion

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            _eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                cmdName = CmdName.SaveGlassID,
                Entity = txtGlassID.Text.ToString(),
                Target = "ModifyGlassIDViewModel",
            });
            this.Close();
        }

        private void BtnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
