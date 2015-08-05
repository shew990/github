using SCG.SINOStock.ServiceRule;
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

namespace SCG.SINOStock.Views
{
    /// <summary>
    /// ToolChangePwd.xaml 的交互逻辑
    /// </summary>
    public partial class ToolChangePwd : Window
    {
        private AccountRule _accountRule = null;
        public ToolChangePwd()
        {
            InitializeComponent();
            _accountRule = new AccountRule();
        }

        private void BtnPrint_Click(object sender, RoutedEventArgs e)
        {
            string strOldPwd = txtOldPwd.Password.Trim();
            string strNewPwd = txtNewPwd.Password.Trim();
            string strCheckPwd = txtCheckPwd.Password.Trim();

            if (string.IsNullOrEmpty(strOldPwd))
            {
                Common.MessageBox.Show("请填写旧密码");
                txtOldPwd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(strNewPwd))
            {
                Common.MessageBox.Show("请填写新密码");
                txtNewPwd.Focus();
                return;
            }
            if (string.IsNullOrEmpty(strCheckPwd))
            {
                Common.MessageBox.Show("请填写确认密码");
                txtCheckPwd.Focus();
                return;
            }
            if (strNewPwd.GetHashCode() != strCheckPwd.GetHashCode())
            {
                Common.MessageBox.Show("新密码与确认密码不一致");
                return;
            }
            string ErrMsg = string.Empty;

            if (!_accountRule.ChangePwd(strOldPwd, strNewPwd, ref ErrMsg))
            {
                Common.MessageBox.Show(ErrMsg);
            }
            else
            {
                Common.MessageBox.Show("修改密码成功");
                //  txtCheckPwd.Text = txtNewPwd.Text = txtOldPwd.Text = string.Empty;
                this.Close();
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
        private void Label_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            this.DragMove();

        }
    }
}
