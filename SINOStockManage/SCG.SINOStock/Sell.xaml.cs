using Microsoft.Practices.Prism.Events;
using Microsoft.Practices.ServiceLocation;
using SCG.SINOStock.Infrastructure;
using SCG.SINOStock.ServiceRule;
using SCG.SINOStock.WCFService.SINOStockService;
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
using System.Windows.Shapes;

//反正错了 反正输了 反正自己陪自己快乐！！！
namespace SCG.SINOStock
{
    /// <summary>
    /// Sell.xaml 的交互逻辑
    /// </summary>
    public partial class Sell : Window
    {
        private IEventAggregator eventAggregator;
        private AccountRule _rule;
        public Sell()
        {
           
            InitializeComponent();
            _rule = new AccountRule();
            UserLoginView = new Lazy<Views.UserLoginView>();
            this.ContentLogin.Content = UserLoginView.Value;
            GotoLoginView();


            eventAggregator = ServiceLocator.Current.GetInstance<IEventAggregator>();
            eventAggregator.GetEvent<CmdEvent>().Subscribe(param =>
            {
                switch (param.cmdViewName)
                {
                    case CmdViewName.AccountView://账户列表界面
                        GotoAccountView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.New,
                            Target = "AccountViewModel",
                        });
                        break;
                    case CmdViewName.MainView://主界面
                        labWelcome.Text = Common.ServiceDataLocator.GetInstance<Account>().Name;
                        GotoMainView();
                        break;
                    case CmdViewName.LoginView://登录界面
                        if (Common.MessageBox.Show("确定退出系统吗？", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                        {
                            if (Common.ReadCOM.serialport.IsOpen)
                            {
                                Common.ReadCOM.serialport.Close();
                            }
                            _rule.ExitCurrentAccountAsyns();
                            Common.ServiceDataLocator.Clear();
                            GotoLoginView();
                        }
                        break;
                    case CmdViewName.FormworkMainView://模板列表界面
                        GotoFormworkMainView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                      {
                          cmdName = CmdName.New,
                          Target = "FormworkMainViewModel",
                      });
                        break;
                    case CmdViewName.StockInMainView://入库
                        GotoStockInMainView(param);
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            cmdName = CmdName.New,
                            Tag = param.Tag,
                            Target = "StockInMainViewModel",
                        });
                        break;
                    case CmdViewName.StockOutMainView://出库
                        GotoStockOutMainView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            cmdName = CmdName.New,
                            Tag = param.Tag,
                            Target = "StockOutMainViewModel",
                        });
                        break;
                    case CmdViewName.RoleMainView://角色列表
                        GotoRoleMainView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.New,
                            Target = "RoleMainViewModel",
                        });
                        break;
                    case CmdViewName.RoleMainDetailView://角色明细
                        GotoRoleDetailView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            Target = "RoleDetailViewModel",
                        });
                        break;
                    case CmdViewName.QualityInfoMainView://品质信息列表
                        GotoQualityInfoMainView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = CmdName.New,
                            Target = "QualityInfoMainViewModel",
                        });
                        break;
                    case CmdViewName.QualityInfoDetailView://品质信息明细
                        GotoQualityInfoDetailView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            Target = "QualityInfoDetailViewModel",
                        });
                        break;
                    case CmdViewName.FormworkDetailView://模版明细
                        GotoFormworkDetailView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            Target = "FormworkDetailViewModel",
                        });
                        break;
                    case CmdViewName.Process_JianBaoView://减薄
                        GotoProcess_JianBaoView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            cmdName = CmdName.New,
                            Tag = param.Tag,
                            Target = "Process_JianBaoViewModel",
                        });
                        break;
                    case CmdViewName.Process_PaoGuangView://抛光
                        GotoProcess_PaoGuangView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            Tag = param.Tag,
                            Target = "Process_PaoGuangViewModel",
                        });
                        break;
                    case CmdViewName.Process_DuMoView://镀膜
                        GotoProcess_DuMoView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            Target = "Process_DuMoViewModel",
                        });
                        break;
                    case CmdViewName.AccountDetailView://账户明细
                        GotoAccountDetailView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            Target = "AccountDetailViewModel",
                        });
                        break;
                    case CmdViewName.Process_FanGongView://返工
                        GotoProcess_FanGongView();
                        break;
                    case CmdViewName.ImportStockLotView:
                        GotoImportStockLotView();
                        break;
                    case CmdViewName.ModifyGlassIDView:
                        GotoModifyGlassIDView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            cmdName = CmdName.New,
                            Target = "ModifyGlassIDViewModel",
                        });
                        break;
                    case CmdViewName.ToolScanGlassID_StockInView:
                        GotoToolScanGlassID_StockInView(param);
                        break;
                    case CmdViewName.ToolScanGlassID_PaoGuangView:
                        GotoToolScanGlassID_PaoGuangView(param);
                        break;
                    case CmdViewName.ToolEnterNoToPrintView:
                        GotoToolEnterNoToPrintView(param);
                        break;
                    case CmdViewName.ToolScanGlassID_JianBaoView:
                        GotoToolScanGlassID_JianBaoView(param);
                        break;
                    case CmdViewName.ToolScanGlassID_StockOutView:
                        GotoToolScanGlassID_StockOutView(param);
                        break;
                    case CmdViewName.StockLotMainView:
                        GotoStockLotMainView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            cmdName = param.cmdName,
                            //  Entity = param.Entity,
                            Target = "StockLotMainViewModel",
                        });
                        break;
                    case CmdViewName.StockLotDetailView:
                        GotoStockLotDetailView();
                        eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                        {
                            Entity = param.Entity,
                            Target = "StockLotDetailViewModel",
                        });
                        break;
                    case CmdViewName.ToolExportGlassIDsView:
                        GotoToolExportGlassIDsView(param);
                        break;
                    case CmdViewName.ToolReplaceGlassID:
                        GotoToolReplaceGlassID(param);
                        break;
                    case CmdViewName.AgainEnterNoPrint:
                        GotoAgainEnterNoPrint(param);
                        break;
                    case CmdViewName.CloseApplication:
                        if (!(Common.MessageBox.Show("确定直接关闭系统吗？", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes))
                        {
                            //e.Cancel = true;
                        }
                        else
                        {
                            _rule.ExitCurrentAccountAsyns();
                            Application.Current.Shutdown();
                        }
                        break;
                    default:
                        break;
                }
            }, ThreadOption.UIThread, true, p => p.Target == "Sell");
        }

        #region 界面处理
        private void GotoLoginView()
        {
            this.PMain.Visibility = Visibility.Hidden;
            this.PLogin.Visibility = Visibility.Visible;
            this.ContentMain.Content = null;
            this.Title = "SINO生产管理系统 v2.0.3.7[登录]";
        }

        private void GotoMainView()
        {
            if (Common.ReadCOM.serialport.IsOpen)
            {
                Common.ReadCOM.serialport.Close();
            }
            this.PMain.Visibility = Visibility.Visible;
            this.PLogin.Visibility = Visibility.Hidden;
            if (MenuMainView == null)
                MenuMainView = new Lazy<Views.MenuMainView>();
            this.ContentMain.Content = MenuMainView.Value;
            this.Title = "SINO生产管理系统[主界面]";
            eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {

                cmdName = CmdName.New,
                Target = "MenuMainViewModel",
            });
        }

        private void GotoAccountView()
        {
            if (AccountView == null)
                AccountView = new Lazy<Views.AccountView>();
            this.ContentMain.Content = AccountView.Value;
            this.Title = "SINO生产管理系统[用户管理]";
        }

        private void GotoFormworkMainView()
        {
            if (FormworkMainView == null)
                FormworkMainView = new Lazy<Views.FormworkMainView>();
            this.ContentMain.Content = FormworkMainView.Value;
            this.Title = "SINO生产管理系统[模版管理]";
        }

        private void GotoStockInMainView(CmdEventParam param)
        {
            if (StockInMainView == null)
                StockInMainView = new Lazy<Views.StockInMainView>();
            this.ContentMain.Content = StockInMainView.Value;

            bool bCondition = (bool)param.Entity;
            if (bCondition)
                this.Title = "SINO生产管理系统[入库管理]";
            else
                this.Title = "SINO生产管理系统[入库管理(无对比)]";


        }
        private void GotoStockLotMainView()
        {
            if (StockLotMainView == null)
                StockLotMainView = new Lazy<Views.StockLotMainView>();
            this.ContentMain.Content = StockLotMainView.Value;
            this.Title = "SINO生产管理系统[LotNo查询]";


        }
        private void GotoStockLotDetailView()
        {
            if (StockLotDetailView == null)
                StockLotDetailView = new Lazy<Views.StockLotDetailView>();
            this.ContentMain.Content = StockLotDetailView.Value;
            this.Title = "SINO生产管理系统[LotNo查询]";


        }
        private void GotoStockOutMainView()
        {
            if (StockOutMainView == null)
                StockOutMainView = new Lazy<Views.StockOutMainView>();
            this.ContentMain.Content = StockOutMainView.Value;
            this.Title = "SINO生产管理系统[镀膜后检验]";
        }

        private void GotoRoleMainView()
        {
            if (RoleMainView == null)
                RoleMainView = new Lazy<Views.RoleMainView>();
            this.ContentMain.Content = RoleMainView.Value;
            this.Title = "SINO生产管理系统[角色管理]";
        }

        private void GotoRoleDetailView()
        {
            if (RoleDetailView == null)
                RoleDetailView = new Lazy<Views.RoleDetailView>();
            this.ContentMain.Content = RoleDetailView.Value;
            this.Title = "SINO生产管理系统[角色添加/修改]";
        }
        private void GotoQualityInfoMainView()
        {
            if (QualityInfoMainView == null)
                QualityInfoMainView = new Lazy<Views.QualityInfoMainView>();
            this.ContentMain.Content = QualityInfoMainView.Value;
            this.Title = "SINO生产管理系统[品质信息管理]";
        }

        private void GotoQualityInfoDetailView()
        {
            if (QualityInfoDetailView == null)
                QualityInfoDetailView = new Lazy<Views.QualityInfoDetailView>();
            this.ContentMain.Content = QualityInfoDetailView.Value;
            this.Title = "SINO生产管理系统[品质信息添加/修改]";
        }
        private void GotoFormworkDetailView()
        {
            if (FormworkDetailView == null)
                FormworkDetailView = new Lazy<Views.FormworkDetailView>();
            this.ContentMain.Content = FormworkDetailView.Value;
            this.Title = "SINO生产管理系统[模版添加/修改]";
        }

        private void GotoProcess_JianBaoView()
        {
            if (Process_JianBaoView == null)
                Process_JianBaoView = new Lazy<Views.Process_JianBaoView>();
            this.ContentMain.Content = Process_JianBaoView.Value;
            this.Title = "SINO生产管理系统[减薄后检验]";
        }

        private void GotoProcess_PaoGuangView()
        {
            if (Process_PaoGuangView == null)
                Process_PaoGuangView = new Lazy<Views.Process_PaoGuangView>();
            this.ContentMain.Content = Process_PaoGuangView.Value;
            this.Title = "SINO生产管理系统[抛光后检验]";
        }

        private void GotoProcess_DuMoView()
        {
            if (Process_DuMoView == null)
                Process_DuMoView = new Lazy<Views.Process_DuMoView>();
            this.ContentMain.Content = Process_DuMoView.Value;
            this.Title = "SINO生产管理系统[镀膜后检验]";
        }

        private void GotoAccountDetailView()
        {
            if (AccountDetailView == null)
                AccountDetailView = new Lazy<Views.AccountDetailView>();
            this.ContentMain.Content = AccountDetailView.Value;
            this.Title = "SINO生产管理系统[镀膜]";
        }

        private void GotoProcess_FanGongView()
        {
            if (Process_FanGongView == null)
                Process_FanGongView = new Lazy<Views.Process_FanGongView>();
            this.ContentMain.Content = Process_FanGongView.Value;
            this.Title = "SINO生产管理系统[返工]";
        }
        private void GotoImportStockLotView()
        {
            if (ImportStockLotView == null)
                ImportStockLotView = new Lazy<Views.ImportStockLotView>();
            this.ContentMain.Content = ImportStockLotView.Value;
            this.Title = "SINO生产管理系统[导入Glass ID]";
        }
        private void GotoModifyGlassIDView()
        {
            if (ModifyGlassIDView == null)
                ModifyGlassIDView = new Lazy<Views.ModifyGlassIDView>();
            this.ContentMain.Content = ModifyGlassIDView.Value;
            this.Title = "SINO生产管理系统[GlssID后台]";
        }




        private void GotoToolScanGlassID_StockInView(CmdEventParam param)
        {
            // Views.ToolScanGlassID_StockIn view = new Views.ToolScanGlassID_StockIn();

            if (ToolScanGlassID_StockIn == null)
                ToolScanGlassID_StockIn = new Views.ToolScanGlassID_StockIn();
            eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                cmdName = param.cmdName,
                Entity = param.Entity,
                Target = "ToolScanGlassID_StockInViewModel",
                SourceTarget = param.SourceTarget,

            });
            //     Views.ToolScanGlassID_JianBao view = new Views.ToolScanGlassID_JianBao();
            ToolScanGlassID_StockIn.ShowDialog();
        }
        private void GotoToolScanGlassID_JianBaoView(CmdEventParam param)
        {
            // Views.ToolScanGlassID_JianBao view = new Views.ToolScanGlassID_JianBao();
            if (ToolScanGlassID_JianBao == null)
            {
                ToolScanGlassID_JianBao = new Views.ToolScanGlassID_JianBao();
            }

            eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                Entity = param.Entity,
                cmdName = param.cmdName,
                Tag = param.Tag,
                Target = "ToolScanGlassID_JianBaoViewModel",
                SourceTarget = param.SourceTarget,

            });
            ToolScanGlassID_JianBao.ShowDialog();

        }
        private void GotoToolScanGlassID_PaoGuangView(CmdEventParam param)
        {
            //Views.ToolScanGlassID_PaoGuang view = new Views.ToolScanGlassID_PaoGuang();
            if (ToolScanGlassID_PaoGuang == null)
                ToolScanGlassID_PaoGuang = new Views.ToolScanGlassID_PaoGuang();

            eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                Entity = param.Entity,
                cmdName = param.cmdName,
                Tag = param.Tag,
                Target = "ToolScanGlassID_PaoGuangViewModel",
                SourceTarget = param.SourceTarget,

            });
            ToolScanGlassID_PaoGuang.ShowDialog();
        }
        private void GotoToolScanGlassID_StockOutView(CmdEventParam param)
        {
            //  Views.ToolScanGlassID_StockOut view = new Views.ToolScanGlassID_StockOut();
            if (ToolScanGlassID_StockOut == null)
                ToolScanGlassID_StockOut = new Views.ToolScanGlassID_StockOut();

            eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                Entity = param.Entity,
                cmdName = param.cmdName,
                Tag = param.Tag,
                Target = "ToolScanGlassID_StockOutViewModel",
                SourceTarget = param.SourceTarget,

            });
            ToolScanGlassID_StockOut.ShowDialog();
        }
        private void GotoToolEnterNoToPrintView(CmdEventParam param)
        {
            if (ToolEnterNoToPrint == null)
                ToolEnterNoToPrint = new Views.ToolEnterNoToPrint();
            eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                Entity = param.Entity,
                cmdName = param.cmdName,
                Tag = param.Tag,
                IsQiangDa = param.IsQiangDa,
                Target = "ToolEnterNoToPrintViewModel",
                Entity1=param.Entity1,
                SourceTarget = param.SourceTarget,

            });
            ToolEnterNoToPrint.ShowDialog();
        }

        private void GotoToolReplaceGlassID(CmdEventParam param)
        {
            if (ToolReplaceGlassID == null)
                ToolReplaceGlassID = new Views.ToolReplaceGlassID();
            eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                Entity = param.Entity,
                cmdName = param.cmdName,
                Tag = param.Tag,
                Target = "ToolReplaceGlassIDView",
                SourceTarget = param.SourceTarget,

            });
            ToolReplaceGlassID.ShowDialog();
        }
        private void GotoToolExportGlassIDsView(CmdEventParam param)
        {
            List<StockDetail> lst = param.Entity as List<StockDetail>;
            Views.ToolView.ExportGlassIDs view = new Views.ToolView.ExportGlassIDs(lst);
            view.ShowDialog();
        }
        private void GotoAgainEnterNoPrint(CmdEventParam param)
        {
            if (AgainEnterNoPrint == null)
                AgainEnterNoPrint = new Views.AgainEnterNoPrint();
            eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
            {
                Entity = param.Entity,
                cmdName = param.cmdName,
                Tag = param.Tag,
                Target = "AgainEnterNoPrint",
                SourceTarget = param.SourceTarget,
                Entity1=param.Entity1,

            });
            AgainEnterNoPrint.ShowDialog();
        }
        #endregion
        #region 界面属性
        public Lazy<Views.UserLoginView> UserLoginView { get; set; }
        public Lazy<Views.MenuMainView> MenuMainView { get; set; }

        public Lazy<Views.AccountView> AccountView { get; set; }
        public Lazy<Views.FormworkMainView> FormworkMainView { get; set; }
        public Lazy<Views.StockInMainView> StockInMainView { get; set; }
        public Lazy<Views.StockOutMainView> StockOutMainView { get; set; }
        public Lazy<Views.RoleMainView> RoleMainView { get; set; }
        public Lazy<Views.RoleDetailView> RoleDetailView { get; set; }
        public Lazy<Views.FormworkDetailView> FormworkDetailView { get; set; }
        public Lazy<Views.Process_JianBaoView> Process_JianBaoView { get; set; }
        public Lazy<Views.Process_PaoGuangView> Process_PaoGuangView { get; set; }
        public Lazy<Views.Process_DuMoView> Process_DuMoView { get; set; }
        public Lazy<Views.AccountDetailView> AccountDetailView { get; set; }
        public Lazy<Views.Process_FanGongView> Process_FanGongView { get; set; }
        public Lazy<Views.ImportStockLotView> ImportStockLotView { get; set; }
        //  public Lazy<Views.ToolScanGlassID_StockIn> ToolScanGlassID_StockIn { get; set; }

        public Lazy<Views.QualityInfoMainView> QualityInfoMainView { get; set; }
        public Lazy<Views.QualityInfoDetailView> QualityInfoDetailView { get; set; }
        public Lazy<Views.StockLotMainView> StockLotMainView { get; set; }
        public Lazy<Views.StockLotDetailView> StockLotDetailView { get; set; }
        public Lazy<Views.ModifyGlassIDView> ModifyGlassIDView { get; set; }
        #endregion
        #region ToolView属性
        public Views.ToolScanGlassID_StockIn ToolScanGlassID_StockIn { get; set; }
        public Views.ToolScanGlassID_JianBao ToolScanGlassID_JianBao { get; set; }
        public Views.ToolScanGlassID_PaoGuang ToolScanGlassID_PaoGuang { get; set; }
        public Views.ToolScanGlassID_StockOut ToolScanGlassID_StockOut { get; set; }
        public Views.ToolEnterNoToPrint ToolEnterNoToPrint { get; set; }
        public Views.AgainEnterNoPrint AgainEnterNoPrint { get; set; }

        public Views.ToolReplaceGlassID ToolReplaceGlassID { get; set; }
        #endregion

        private void btnHome_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            var Tmp = this.ContentMain.Content as Views.StockOutMainView;
            if (Tmp == null)
            {
               
                GotoMainView();
            }
            else
            {
                eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                {
                    cmdName = CmdName.Close,
                    cmdViewName = CmdViewName.MainView,
                    Target = "StockOutMainViewModel",

                });
            }
        }
        private void btnChangePassWord_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Views.ToolChangePwd view = new Views.ToolChangePwd();
            view.ShowDialog();
        }

        private void btnLogout_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            var Tmp = this.ContentMain.Content as Views.StockOutMainView;
            if (Tmp == null)
            {
                if (Common.MessageBox.Show("确定退出系统吗？", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes)
                {
                    if (Common.ReadCOM.serialport.IsOpen)
                    {
                        Common.ReadCOM.serialport.Close();
                    }
                    _rule.ExitCurrentAccountAsyns();
                    Common.ServiceDataLocator.Clear();
                    GotoLoginView();
                }
            }
            else
            {
                eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                {
                    cmdName = CmdName.Close,
                    cmdViewName = CmdViewName.LoginView,
                    Target = "StockOutMainViewModel",

                });
            }






        }

        private void Window_Closed_1(object sender, EventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Closing_1(object sender, System.ComponentModel.CancelEventArgs e)
        {

            var Tmp = this.ContentMain.Content as Views.StockOutMainView;
            if (Tmp == null)
            {
                if (!(Common.MessageBox.Show("确定直接关闭系统吗？", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes))
                {
                    e.Cancel = true;
                }
                else
                {
                    _rule.ExitCurrentAccountAsyns();
                }
            }
            else
            {
                eventAggregator.GetEvent<CmdEvent>().Publish(new CmdEventParam()
                {
                    cmdName = CmdName.Close,
                    cmdViewName = CmdViewName.CloseApplication,
                    Target = "StockOutMainViewModel",

                });
                e.Cancel = true;
            }




            //if (!(Common.MessageBox.Show("确定直接关闭系统吗？", Common.LED_MessageBoxButton.YesNo) == Common.LED_MessageBoxResult.Yes))
            //{
            //    e.Cancel = true;
            //}
            //else
            //{
            //    _rule.ExitCurrentAccountAsyns();
            //}
        }

    }
}
