using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Threading;
using CefSharp;

namespace StudyBox
{
    /// <summary>
    /// Interaction logic for Wikipedia.xaml
    /// </summary>
    public partial class Wikipedia : Window
    {
        public Wikipedia()
        {
            InitializeComponent();
            rectangle.Background = new SolidColorBrush(Properties.Settings.Default.Color);
        }
        private void BtnMini_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
            finally { }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (bMain.CanGoBack)
            {
                bMain.Back();
                back.IsEnabled = true;
            }
            else
            {
                back.IsEnabled = false;
            }
            if (!bMain.CanGoForward)
            {
                forw.IsEnabled = false;
            }
            else
            {
                forw.IsEnabled = true;
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (!bMain.CanGoBack)
            {
                back.IsEnabled = false;
            }
            else
            {
                back.IsEnabled = true;
            }
            if (bMain.CanGoForward)
            {
                bMain.Forward();
                forw.IsEnabled = true;
            }
            else
            {
                forw.IsEnabled = false;
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            bMain.Reload(true);
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            bMain.Load("https://vi.m.wikipedia.org/wiki/index.php?search="+txtKey.Text);
            if (!bMain.CanGoBack)
            {
                back.IsEnabled = false;
            }
            else
            {
                back.IsEnabled = true;
            }
            if (!bMain.CanGoForward)
            {
                forw.IsEnabled = false;
            }
            else
            {
                forw.IsEnabled = true;
            }
        }

        private void bMain_FrameLoadEnd(object sender, FrameLoadEndEventArgs e)
        {
            Dispatcher.BeginInvoke(DispatcherPriority.Normal,new AVoidDelegate(()=> {
                if (!bMain.CanGoBack)
                {
                    back.IsEnabled = false;
                }
                else
                {
                    back.IsEnabled = true;
                }
                if (!bMain.CanGoForward)
                {
                    forw.IsEnabled = false;
                }
                else
                {
                    forw.IsEnabled = true;
                }
            }));
        }
        public delegate void AVoidDelegate();
    }
}
