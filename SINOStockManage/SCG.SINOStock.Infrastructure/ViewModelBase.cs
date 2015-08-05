using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.Prism.ViewModel;
using SCG.SINOStock.WCFService;
using System.Windows;
using System.ComponentModel;
using System.Threading;
using System.IO.Ports;
using System.Configuration;
using System.Data;

namespace SCG.SINOStock.Infrastructure
{
    public class ViewModelBase : NotificationObject
    {
        #region 时间聚合器
        protected IEventAggregator _eventAggregator;
        #endregion
        #region 系统关键属性
        protected bool _isBusy = true;
        public bool IsBusy
        {
            get { return this._isBusy; }
            protected set
            {
                if (value != this._isBusy)
                {
                    this._isBusy = value;
                    this.RaisePropertyChanged("IsBusy");
                    this.RaisePropertyChanged("IsVisibility");
                }
            }
        }
     
        public Visibility IsVisibility
        {
            get
            {
                if (IsBusy)
                    return Visibility.Hidden;
                else
                    return Visibility.Visible;
            }
        }
        #endregion

        protected Exception _lastError;
        private ISINOStockServiceProxy _proxy;
        protected ISINOStockServiceProxy Proxy
        {
            get
            {
                if (_proxy == null)
                {
                    _proxy = new SINOStockServiceProxy();
                    _proxy.PropertyChanged += (s, e) =>
                    {
                        switch (e.PropertyName)
                        {
                            case "ActiveCallCount":
                                IsBusy = (Proxy.ActiveCallCount == 0);
                                break;
                        }
                    };
                }
                return _proxy;
            }
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
                    ScanComExecute(strReceive.Trim(),null);
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
    }
}
