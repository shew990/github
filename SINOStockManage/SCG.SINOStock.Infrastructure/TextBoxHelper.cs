using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

/**
 *   命名空间:   SCG.SINOStock.Infrastructure
 *   文件名:     TextBoxHelper
 *   说明:       
 *   创建时间:   2014/2/9 13:17:02
 *   作者:       liende
 */
namespace SCG.SINOStock.Infrastructure
{
    public static class TextBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty = DependencyProperty.RegisterAttached("Password", typeof(string), typeof(TextBoxHelper), new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));
        public static readonly DependencyProperty AttachProperty = DependencyProperty.RegisterAttached("Attach", typeof(bool), typeof(TextBoxHelper), new PropertyMetadata(false, Attach));
        public static readonly DependencyProperty AutoSelectAllProperty = DependencyProperty.RegisterAttached("AutoSelectAll", typeof(bool), typeof(TextBoxHelper), new FrameworkPropertyMetadata((bool)false, new PropertyChangedCallback(OnAutoSelectAllChanged)));
        private static readonly DependencyProperty IsUpdatingProperty = DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(TextBoxHelper));
     

        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox passwordBox = sender as TextBox;
            passwordBox.TextChanged -= PasswordChanged;
            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Text = (string)e.NewValue;
            }
            passwordBox.TextChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            TextBox passwordBox = sender as TextBox;
            if (passwordBox == null) return;
            if ((bool)e.OldValue)
            {
                passwordBox.TextChanged -= PasswordChanged;
            }
            if ((bool)e.NewValue)
            {
                passwordBox.TextChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            TextBox passwordBox = sender as TextBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Text);
            SetIsUpdating(passwordBox, false);
        }

        #region 设置选中/焦点
        public static bool GetAutoSelectAll(DependencyObject d)
        {
            return (bool)d.GetValue(AutoSelectAllProperty);
        }

        public static void SetAutoSelectAll(DependencyObject d, bool value)
        {
            d.SetValue(AutoSelectAllProperty, value);
        }

        private static void OnAutoSelectAllChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textBox = d as TextBox;
            if (textBox != null)
            {
                var flag = (bool)e.NewValue;
                if (flag)
                {
                    textBox.GotFocus += TextBoxOnGotFocus;
                }
                else
                {
                    textBox.GotFocus -= TextBoxOnGotFocus;
                }
            }
        }

        private static void TextBoxOnGotFocus(object sender, RoutedEventArgs e)
        {
            var textBox = sender as TextBox;
            if (textBox != null)
            {
                textBox.SelectAll();
            }
        }

        #endregion
    }
}
