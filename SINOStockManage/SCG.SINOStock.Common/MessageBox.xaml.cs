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

namespace SCG.SINOStock.Common
{

    /// <summary>
    /// MessageBox.xaml 的交互逻辑
    /// </summary>
    public partial class MessageBox : Window
    {
        #region 静态调用变量、方法
        private static MessageBox toolMessageBox = null;

        /// <summary>
        /// 弹出自定义提示框
        /// </summary>
        /// <param name="strContent">需要提示的内容</param>
        /// <returns></returns>
        public static LED_MessageBoxResult Show(string strContent)
        {
            LED_DialogParameters paerameter = new LED_DialogParameters(strContent);
            return Show(paerameter);
        }
        /// <summary>
        /// 弹出自定义选择对话框
        /// </summary>
        /// <param name="strContent">需要提示的内容</param>
        /// <returns></returns>
        public static LED_MessageBoxResult Show(string strContent, LED_MessageBoxButton btnMessageBoxButton)
        {
            LED_DialogParameters paerameter = new LED_DialogParameters("询问", strContent, btnMessageBoxButton);
            return Show(paerameter);
        }
        /// <summary>
        /// 弹出对话框
        /// </summary>
        /// <param name="paerameter"></param>
        /// <returns></returns>
        public static LED_MessageBoxResult Show(LED_DialogParameters paerameter)
        {
            if (toolMessageBox == null)
                toolMessageBox = new MessageBox();
            //return toolMessageBox.Show_Ex(messageBoxText);

            return toolMessageBox.ShowDialog(paerameter);
        }


        #endregion

        #region 实例调用字段
        private LED_MessageBoxResult _messageBoxResult = LED_MessageBoxResult.Cancel;
        #endregion

        public MessageBox()
        {
            InitializeComponent();
        }

        protected LED_MessageBoxResult ShowDialog(LED_DialogParameters messageBoxText)
        {
            toolMessageBox.setMessageinfo(messageBoxText);
            toolMessageBox.Topmost = true;
            if (toolMessageBox.Visibility != Visibility.Visible)
            {
                toolMessageBox.btnOK.Focus();
                toolMessageBox.ShowDialog();
            }

            return _messageBoxResult;
        }
        public void setMessageinfo(LED_DialogParameters parameters)
        {
            TitleInfo.Content = parameters.Hander;//设置标题


//                       parameters.Content = @"Starting with SIMATIC IT 6.3 version, most of the COM Interfaces for the 
///           integration of the Components of the Production Suite with Production Modeler 
///           are deprecated (refer to the Notes Page for the detailed list)";
           // parameters.Content = @"随后民警对男子随身携带的物品进行检查，发现他裤子内裆部竟然藏着一包用卫生纸包裹严实的小袋，而袋子里装着大量白色粉末。“经鉴定，男子携带的可疑物是冰毒，约重49克。”民警说，男子是四川人，今年43岁，是替人带毒品到宝鸡的。随后民警对男子随身携带的物品进行检查，发现他裤子内裆部竟然藏着一包用卫生纸包裹严实的小袋，而袋子里装着大量白色粉末。“经鉴定，男子携带的可疑物是冰毒，约重49克。”民警说，男子是四川人，今年43岁，是替人带毒品到宝鸡的。";

             boardMain.Height = this.Height = this.MaxHeight = 120;
           // boardMain.Height = this.Height = this.MaxHeight =25 * Encoding.Default.GetBytes(parameters.Content).Length / 28;
            MessageInfo.Text = parameters.Content;//设置内容
            //   boardMain.Height = this.Height = this.MaxHeight = 500;

            _messageBoxResult = LED_MessageBoxResult.Cancel;

            switch (parameters.MessageBoxButton)
            {
                case LED_MessageBoxButton.OK://显示默认对话框（确认对话框）
                    btnOK.Visibility = Visibility.Visible;

                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    btnCancel.Visibility = Visibility.Collapsed;
                    break;
                case LED_MessageBoxButton.OKCancel://显示确认、取消对话框
                    btnOK.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;

                    btnYes.Visibility = Visibility.Collapsed;
                    btnNo.Visibility = Visibility.Collapsed;
                    break;
                case LED_MessageBoxButton.YesNoCancel://显示是、否、取消对话框
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Visible;
                    btnOK.Visibility = Visibility.Collapsed;
                    break;
                case LED_MessageBoxButton.YesNo://显示是、否对话框
                    btnYes.Visibility = Visibility.Visible;
                    btnNo.Visibility = Visibility.Visible;
                    btnCancel.Visibility = Visibility.Collapsed;
                    btnOK.Visibility = Visibility.Collapsed;
                    break;
                default:
                    break;
            }
        }
        private void btnOK_Click(object sender, RoutedEventArgs e)
        {
            _messageBoxResult = LED_MessageBoxResult.OK;
            toolMessageBox.Close();
        }
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            Clipboard.SetText(MessageInfo.Text);
            // toolMessageBox.Close();
        }
        private void btnYes_Click(object sender, RoutedEventArgs e)
        {
            _messageBoxResult = LED_MessageBoxResult.Yes;
            toolMessageBox.Close();
        }
        private void btnNo_Click(object sender, RoutedEventArgs e)
        {
            _messageBoxResult = LED_MessageBoxResult.No;
            toolMessageBox.Close();
        }
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            _messageBoxResult = LED_MessageBoxResult.Cancel;
            toolMessageBox.Close();
        }
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();
        }

        private void Image_MouseDown(object sender, MouseButtonEventArgs e)
        {
            this.Close();

        }
        protected override void OnClosing(System.ComponentModel.CancelEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }
    }
    public class LED_DialogParameters
    {
        public LED_DialogParameters()
        {

        }
        public LED_DialogParameters(string strContent)
        {
            this.Content = strContent;
        }
        public LED_DialogParameters(string strHander, string strContent)
        {
            this.Hander = strHander;
            this.Content = strContent;
        }
        public LED_DialogParameters(string strHander, string strContent, LED_MessageBoxButton btnMessageBoxButton)
        {
            this.Hander = strHander;
            this.Content = strContent;
            this.MessageBoxButton = btnMessageBoxButton;
        }
        private string _hander = "提示";
        /// <summary>
        /// 弹出框标题，默认为“提示"
        /// </summary>
        public string Hander
        {
            get { return _hander; }
            set { _hander = value; }
        }

        private string _okButtonContent = "确定";
        /// <summary>
        /// 确定按钮文字，默认为”确定"
        /// </summary>
        public string OkButtonContent
        {
            get { return _okButtonContent; }
            set { _okButtonContent = value; }
        }

        private string content;
        /// <summary>
        /// 弹出框内容
        /// </summary>
        public string Content
        {
            get { return content; }
            set { content = value; }
        }

        private LED_MessageBoxButton _messageBoxButton;
        /// <summary>
        /// 指定用户单击的消息框按钮
        /// </summary>
        public LED_MessageBoxButton MessageBoxButton
        {
            get { return _messageBoxButton; }
            set { _messageBoxButton = value; }
        }


    }
    // 摘要:
    //     指定用户单击的消息框按钮
    //     方法返回。
    public enum LED_MessageBoxResult
    {
        // 摘要:
        //     消息框未返回值。
        None = 0,
        //
        // 摘要:
        //     消息框的结果值为“确定”。
        OK = 1,
        //
        // 摘要:
        //     消息框的结果值为“取消”。
        Cancel = 2,
        //
        // 摘要:
        //     消息框的结果值为“是”。
        Yes = 6,
        //
        // 摘要:
        //     消息框的结果值为“否”。
        No = 7,
    }
    // 摘要:
    //     指定显示在消息框上的按钮。用作 Overload:System.Windows.MessageBox.Show 方法的参数。
    public enum LED_MessageBoxButton
    {
        // 摘要:
        //     消息框显示“确定”按钮。
        OK = 0,
        //
        // 摘要:
        //     消息框显示“确定”和“取消”按钮。
        OKCancel = 1,
        //
        // 摘要:
        //     消息框显示“是”、“否”和“取消”按钮。
        YesNoCancel = 3,
        //
        // 摘要:
        //     消息框显示“是”和“否”按钮。
        YesNo = 4,
    }
}
