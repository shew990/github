using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Practices.Prism.Events;

namespace SCG.SINOStock.Infrastructure
{
    public class CmdEvent : CompositePresentationEvent<CmdEventParam>
    {

    }
    public class CmdEventParam
    {
        public CmdName cmdName { get; set; }
        public string customCmdName { get; set; }
        public object Entity { get; set; }
        public string Target { get; set; }
        public string SourceTarget { get; set; }
        public string Comment { get; set; }
        public CmdViewName cmdViewName { get; set; }

        public object Entity1 { get; set; }
        public string Tag { get; set; }
        /// <summary>
        /// 是否强打  true强打  false不强打
        /// </summary>
        public bool IsQiangDa { get; set; }
        // public ToolsKind Kind { get; set; }
        // public ValidationToolsKind ValidationKind { get; set; }
        //   public bool Multiple { get; set; }
       
    }
    public enum CmdName
    {
        New = 0,      //新增
        Edit,       //编辑
        Manager,    //管理
        Refresh,   //刷新
        Delete,     //删除
        SendEntity, //传输实体
        SendTag,
        Load,       //载入数据
        List,       //列表
        Copy,       //复制
        Close,      //关闭
        SaveGlassID,
        Enter,

        SendPrintData_Box,
        SendPrintData_Tray,
        SendPrintData_Box_Again,
        SendPrintData_Tray_Again,
        CancelPrint,
        //SelectItem, //选择项
        //SelectItems,//选择多项
        //CallTools,          //呼叫工具
        //CallToolView,       //呼叫工具视图（内置）
        //ReciveToolEntity,   //接收工具发送的实体数据
        //Reject,
        //Clear,
        //MainLoadView,
        //MainLoginView,
        //MainProgramView,
        //MainMenuView,
        //MainView,
        //AbortApp
    }
    public enum CmdViewName
    {
        MainView = 0,//主界面
        ConvertToBinaryView,
        LoginView,//登录界面
        AccountView,//用户管理主界面
        AccountDetailView,
        FormworkMainView,//模版管理主界面
        FormworkDetailView,//模版管理修改界面
        StockInMainView,//入库界面
        StockOutMainView,//出库界面
        RoleMainView,//角色管理主界面
        RoleMainDetailView,//角色管理修改界面

        Process_JianBaoView,//减薄
        Process_PaoGuangView,//抛光
        Process_DuMoView,//镀膜
        Process_FanGongView,//返工

        ImportStockLotView,//导入GlassID

        ToolScanGlassID_StockInView,
        ToolScanGlassID_JianBaoView,
        ToolScanGlassID_PaoGuangView,
        ToolScanGlassID_StockOutView,
        ToolScanGlassID_DuMoView,
        ToolEnterNoToPrintView,
        ToolReplaceGlassID,

        QualityInfoMainView,
        QualityInfoDetailView,
        StockLotMainView,
        StockLotDetailView,
        ToolExportGlassIDsView,
        ModifyGlassIDView,
        AgainEnterNoPrint,




        CloseApplication,

    }

}
