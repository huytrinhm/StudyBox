using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Threading;
using System.Windows.Media.Animation;
using Microsoft.Win32;
using System.ComponentModel;
using System.Data.OleDb;
using System.Windows.Forms;

namespace StudyBox
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region Variables
        public static NotifyIcon grandIcon = new NotifyIcon();
        public static WindowState ws;
        public static List<string> blockAppList = new List<string>();
        public static List<string> SavedApp = new List<string>();
        public static List<string> SavedAppDis = new List<string>();
        public static List<string> blockWebList = new List<string>();
        public static List<SavedApps> savedApps = new List<SavedApps>();
        public static List<JobItem> JobMon = new List<JobItem>();
        public static List<JobItem> JobTue = new List<JobItem>();
        public static List<JobItem> JobWed = new List<JobItem>();
        public static List<JobItem> JobThu = new List<JobItem>();
        public static List<JobItem> JobFri = new List<JobItem>();
        public static List<JobItem> JobSat = new List<JobItem>();
        public static List<JobItem> JobSun = new List<JobItem>();
        private bool _ShowingDialog;
        private bool _AllowClose;
        private Page homePage = new Home();
        private Page dicPage = new Dictionary();
        private Page revPage = new Review();
        private Page goalPage = new Goal();
        private Page setPage;
        private List<System.Windows.Controls.Button> Home_Btn;
        private List<System.Windows.Controls.Button> Home_Btn2;
        private System.Windows.Controls.Button home_Startbtn;
        private Grid Dic_grid;
        private bool UpdatingDB = true;
        #endregion
        public MainWindow()
        {
            InitializeComponent();

            

            IsEnabled = false;
            try
            {
                RegistryKey regkey = Registry.CurrentUser.CreateSubKey("Software\\StudyBox");
                RegistryKey start = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run");
                start.SetValue("StudyBox", AppDomain.CurrentDomain.BaseDirectory + "\\StudyBox.exe");
                regkey.SetValue("Index", "1");
            }
            catch (Exception)
            {
                
            }
            Thread update = new Thread(() => { UpdateDB(); });
            update.Start();
            while (UpdatingDB) { }
            Home_Btn = new List<System.Windows.Controls.Button>(((WrapPanel)((Grid)((Grid)((DialogHost)((ScrollViewer)((Grid)homePage.Content).Children[0]).Content).Content).Children[0]).Children[0]).Children.Cast<System.Windows.Controls.Button>().ToList());
            Home_Btn2 = new List<System.Windows.Controls.Button>(((WrapPanel)((Grid)((Grid)((DialogHost)((ScrollViewer)((Grid)homePage.Content).Children[0]).Content).Content).Children[0]).Children[1]).Children.Cast<System.Windows.Controls.Button>().ToList());
            home_Startbtn = (((((homePage.Content as Grid).Children[0] as ScrollViewer).Content as DialogHost).Content as Grid).Children[1] as Grid).Children[0] as System.Windows.Controls.Button;
            Dic_grid = ((((((dicPage.Content as Grid).Children[0] as DialogHost).Content as DialogHost).Content as DialogHost).Content as DialogHost).Content as DialogHost).Content as Grid;
            foreach (System.Windows.Controls.Button btn in Home_Btn)
            {
                btn.Foreground = new SolidColorBrush(Properties.Settings.Default.Color);
            }
            foreach (System.Windows.Controls.Button btn in Home_Btn2)
            {
                btn.Foreground = new SolidColorBrush(Properties.Settings.Default.Color);
            }
            if (!Home.IsStart)
            {
                home_Startbtn.Background = new SolidColorBrush(Properties.Settings.Default.Color);
                home_Startbtn.BorderBrush = new SolidColorBrush(Properties.Settings.Default.Color);
                home_Startbtn.Content = "Bắt đầu";
            }
            else
            {
                home_Startbtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF11B1B"));
                home_Startbtn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF11B1B"));
                home_Startbtn.Content = "Dừng";
            }
            Main.Navigate(homePage);


            setPage = new Setting(this);
            try
            {
                string wallpaper = Registry.GetValue("HKEY_CURRENT_USER\\Control Panel\\Desktop", "WallPaper", System.AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Wall.jpg").ToString();
                ImageBrush myBrush = new ImageBrush();
                Image image = new Image();
                image.Source = new BitmapImage(new Uri(wallpaper));
                myBrush.ImageSource = image.Source;
                gMain.Background = myBrush;
            }
            catch
            {
                gMain.Background = new ImageBrush() { ImageSource = (new Image() { Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Wall.jpg")) }).Source };
            }
            rectangle.Background = new SolidColorBrush(Properties.Settings.Default.Color);

            System.Windows.Application.Current.Resources["PrimaryHueLightBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
            System.Windows.Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
            System.Windows.Application.Current.Resources["PrimaryHueDarkBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
            System.Windows.Application.Current.Resources["SecondaryAccentBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
            grandIcon.Icon = new System.Drawing.Icon(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Icon.ico");
            grandIcon.Visible = true;
            grandIcon.BalloonTipIcon = ToolTipIcon.Info;
            ImageBrush myBrush2 = new ImageBrush();
            Image image2 = new Image();
            image2.Source = new BitmapImage(new Uri(AppDomain.CurrentDomain.BaseDirectory + "\\Resources\\Icon.ico"));
            this.Icon = image2.Source;
            IsEnabled = true;
        }

        #region Events
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
        
        private void BtnMax_Click(object sender, RoutedEventArgs e)
        {
            if (WindowState == WindowState.Normal)
            {
                WindowState = WindowState.Maximized;
                mIcon.Kind = PackIconKind.WindowRestore;
                rectangle.Visibility = Visibility.Collapsed;

                ws = WindowState.Maximized;

                Dic_grid.ColumnDefinitions[0].Width = new GridLength(260);

                (dicPage.FindName("gen_col") as GridViewColumn).Width = 222;

                I1.Height = 100;
                I2.Height = 100;
                I3.Height = 100;
                I4.Height = 100;
                I5.Height = 100;


                Space.Height = 175;

                EasingDoubleKeyFrame a1 = (EasingDoubleKeyFrame)Resources["Val"];
                EasingDoubleKeyFrame a2 = (EasingDoubleKeyFrame)Resources["Val2"];

                a1.Value = 295;
                a2.Value = 295;

                EasingDoubleKeyFrame _a1 = (EasingDoubleKeyFrame)Resources["_Val"];
                EasingDoubleKeyFrame _a2 = (EasingDoubleKeyFrame)Resources["_Val2"];

                _a1.Value = 70;
                _a2.Value = 70;

                EasingDoubleKeyFrame ab = (EasingDoubleKeyFrame)Resources["Valb"];
                EasingDoubleKeyFrame _ab = (EasingDoubleKeyFrame)Resources["_Valb"];

                ab.Value = 295;
                _ab.Value = 70;


                Ic1.Height = 45;
                Ic1.Width = 45;
                T1.FontSize = 30;

                Ic2.Height = 45;
                Ic2.Width = 45;
                T2.FontSize = 30;

                Ic3.Height = 45;
                Ic3.Width = 45;
                T3.FontSize = 30;

                Ic4.Height = 45;
                Ic4.Width = 45;
                T4.FontSize = 28;

                Ic5.Height = 45;
                Ic5.Width = 45;
                T5.FontSize = 30;

                BeginStoryboard((Storyboard)Resources["MenuClose2"]);

                ButtonOpenMenu.Visibility = Visibility.Visible;
                ButtonCloseMenu.Visibility = Visibility.Collapsed;

                ButtonCloseMenu.Width = 80;
                ButtonCloseMenu.Height = 80;
                ButtonOpenMenu.Width = 80;
                ButtonOpenMenu.Height = 80;

                IcBt1.Width = 35;
                IcBt1.Height = 35;
                IcBt2.Width = 35;
                IcBt2.Height = 35;

                ButtonOpenMenu.Margin = new Thickness(-8, 5, -7, 0);

                cMain.Margin = new Thickness(95, 55, 30, 30);
                cMain.Height = 675;


                foreach (System.Windows.Controls.Button btn in Home_Btn)
                {
                    btn.Width = 250;
                    btn.Height = 250;
                    btn.Margin = new Thickness(10);
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Width = 135;
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Height = 135;
                    ((TextBlock)((StackPanel)btn.Content).Children[1]).FontSize = 43;
                    ((TextBlock)((StackPanel)btn.Content).Children[1]).FontSize = 43;

                }
                foreach (System.Windows.Controls.Button btn in Home_Btn2)
                {
                    btn.Width = 250;
                    btn.Height = 250;
                    btn.Margin = new Thickness(10);
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Width = 135;
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Height = 135;
                    ((TextBlock)((StackPanel)btn.Content).Children[1]).FontSize = 43;
                    ((TextBlock)((StackPanel)btn.Content).Children[1]).FontSize = 43;

                }
            }
            else
            {
                WindowState = WindowState.Normal;
                mIcon.Kind = PackIconKind.WindowMaximize;
                rectangle.Visibility = Visibility.Visible;
                rectangle.Width = 820;

                ws = WindowState.Normal;

                Dic_grid.ColumnDefinitions[0].Width = new GridLength(160);

                (dicPage.FindName("gen_col") as GridViewColumn).Width = 122;

                I1.Height = 55;
                I2.Height = 55;
                I3.Height = 55;
                I4.Height = 55;
                I5.Height = 55;


                Space.Height = 75;
                EasingDoubleKeyFrame a1 = (EasingDoubleKeyFrame)Resources["Val"];
                EasingDoubleKeyFrame a2 = (EasingDoubleKeyFrame)Resources["Val2"];

                a1.Value = 175;
                a2.Value = 175;

                EasingDoubleKeyFrame _a1 = (EasingDoubleKeyFrame)Resources["_Val"];
                EasingDoubleKeyFrame _a2 = (EasingDoubleKeyFrame)Resources["_Val2"];

                _a1.Value = 45;
                _a2.Value = 45;

                EasingDoubleKeyFrame ab = (EasingDoubleKeyFrame)Resources["Valb"];
                EasingDoubleKeyFrame _ab = (EasingDoubleKeyFrame)Resources["_Valb"];

                ab.Value = 175;
                _ab.Value = 45;


                Ic1.Height = 25;
                Ic1.Width = 25;
                T1.FontSize = 16;

                Ic2.Height = 25;
                Ic2.Width = 25;
                T2.FontSize = 16;

                Ic3.Height = 25;
                Ic3.Width = 25;
                T3.FontSize = 16;

                Ic4.Height = 25;
                Ic4.Width = 25;
                T4.FontSize = 14;

                Ic5.Height = 25;
                Ic5.Width = 25;
                T5.FontSize = 16;
                

                BeginStoryboard((Storyboard)Resources["MenuClose2"]);

                ButtonOpenMenu.Visibility = Visibility.Visible;
                ButtonCloseMenu.Visibility = Visibility.Collapsed;

                ButtonCloseMenu.Width = 60;
                ButtonCloseMenu.Height = 60;
                ButtonOpenMenu.Width = 60;
                ButtonOpenMenu.Height = 60;

                IcBt1.Width = 25;
                IcBt1.Height = 25;
                IcBt2.Width = 25;
                IcBt2.Height = 25;


                ButtonOpenMenu.Margin = new Thickness(-8,5,-7,0);

                cMain.Margin = new Thickness(55, 40, 10, 10);

                cMain.Height = 400;

                foreach (System.Windows.Controls.Button btn in Home_Btn)
                {
                    btn.Width = 130;
                    btn.Height = 130;
                    btn.Margin = new Thickness(5);
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Width = 70;
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Height = 70;
                    ((TextBlock)((StackPanel)btn.Content).Children[1]).FontSize = 22;
                    ((TextBlock)((StackPanel)btn.Content).Children[1]).FontSize = 22;

                }
                foreach (System.Windows.Controls.Button btn in Home_Btn2)
                {
                    btn.Width = 130;
                    btn.Height = 130;
                    btn.Margin = new Thickness(5);
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Width = 70;
                    ((PackIcon)((StackPanel)btn.Content).Children[0]).Height = 70;
                    ((TextBlock)((StackPanel)btn.Content).Children[1]).FontSize = 22;
                    ((TextBlock)((StackPanel)btn.Content).Children[1]).FontSize = 22;

                }

            }
        }

        private void BtnMini_Click(object sender, RoutedEventArgs e)
        {
            WindowState = WindowState.Minimized;
        }

        private void rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                this.DragMove();
            }
            catch { }
            finally { }
        }

        private void ButtonOpenMenu_Click(object sender, RoutedEventArgs e)
        {
            ButtonCloseMenu.Visibility = Visibility.Visible;
            ButtonOpenMenu.Visibility = Visibility.Collapsed;
        }

        private void ButtonCloseMenu_Click(object sender, RoutedEventArgs e)
        {

            ButtonOpenMenu.Visibility = Visibility.Visible;
            ButtonCloseMenu.Visibility = Visibility.Collapsed;
            

        }

        #region Drawer
        private void I1_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            foreach (System.Windows.Controls.Button btn in Home_Btn)
            {
                btn.Foreground = new SolidColorBrush(Properties.Settings.Default.Color);
            }
            foreach (System.Windows.Controls.Button btn in Home_Btn2)
            {
                btn.Foreground = new SolidColorBrush(Properties.Settings.Default.Color);
            }
            if (!Home.IsStart)
            {
                home_Startbtn.Background = new SolidColorBrush(Properties.Settings.Default.Color);
                home_Startbtn.BorderBrush = new SolidColorBrush(Properties.Settings.Default.Color);
                home_Startbtn.Content = "Bắt đầu";
            }
            else
            {
                home_Startbtn.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF11B1B"));
                home_Startbtn.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF11B1B"));
                home_Startbtn.Content = "Dừng";
            }

            Main.Navigate(homePage);
            if (ButtonCloseMenu.Visibility == Visibility.Visible)
            {
                BeginStoryboard((Storyboard)Resources["MenuClose2"]);
                ButtonOpenMenu.Visibility = Visibility.Visible;
                ButtonCloseMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void I2_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(dicPage);
            if (ButtonCloseMenu.Visibility == Visibility.Visible)
            {
                BeginStoryboard((Storyboard)Resources["MenuClose2"]);
                ButtonOpenMenu.Visibility = Visibility.Visible;
                ButtonCloseMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void I3_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(revPage);
            if (ButtonCloseMenu.Visibility == Visibility.Visible)
            {
                BeginStoryboard((Storyboard)Resources["MenuClose2"]);
                ButtonOpenMenu.Visibility = Visibility.Visible;
                ButtonCloseMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void I4_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(goalPage);
            if (ButtonCloseMenu.Visibility == Visibility.Visible)
            {
                BeginStoryboard((Storyboard)Resources["MenuClose2"]);
                ButtonOpenMenu.Visibility = Visibility.Visible;
                ButtonCloseMenu.Visibility = Visibility.Collapsed;
            }
        }

        private void I5_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            Main.Navigate(setPage);
            if (ButtonCloseMenu.Visibility == Visibility.Visible)
            {
                BeginStoryboard((Storyboard)Resources["MenuClose2"]);
                ButtonOpenMenu.Visibility = Visibility.Visible;
                ButtonCloseMenu.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        protected override async void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);

            if (_AllowClose) return;

            e.Cancel = true;

            if (_ShowingDialog) return;

            TextBlock txt1 = new TextBlock();
            txt1.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txt1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt1.Margin = new Thickness(4);
            txt1.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.FontSize = 18;
            txt1.Text = "Bạn có chắc?";

            TextBlock txt2 = new TextBlock();
            txt2.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txt2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt2.Margin = new Thickness(4);
            txt2.TextWrapping = TextWrapping.WrapWithOverflow;
            txt2.FontSize = 18;
            txt2.Text = "Chương trình sẽ đóng";

            TextBlock txt3 = new TextBlock();
            txt3.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
            txt3.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt3.Margin = new Thickness(4);
            txt3.TextWrapping = TextWrapping.WrapWithOverflow;
            txt3.FontSize = 18;
            txt3.Text = "Tất cả các chức năng sẽ dừng lại!";



            System.Windows.Controls.Button btn1 = new System.Windows.Controls.Button();
            Style style = System.Windows.Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn1.Style = style;
            btn1.Width = 115;
            btn1.Height = 30;
            btn1.Margin = new Thickness(5);
            btn1.Command = DialogHost.CloseDialogCommand;
            btn1.CommandParameter = true;
            btn1.Content = "Đồng ý";

            System.Windows.Controls.Button btn2 = new System.Windows.Controls.Button();
            Style style2 = System.Windows.Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn2.Style = style2;
            btn2.Width = 115;
            btn2.Height = 30;
            btn2.Margin = new Thickness(5);
            btn2.Command = DialogHost.CloseDialogCommand;
            btn2.CommandParameter = false;
            btn2.Content = "Hủy";


            DockPanel dck = new DockPanel();
            dck.Children.Add(btn1);
            dck.Children.Add(btn2);

            StackPanel stk = new StackPanel();
            stk.Width = 250;
            stk.Children.Add(txt1);
            stk.Children.Add(txt2);
            stk.Children.Add(txt3);
            stk.Children.Add(dck);

            _ShowingDialog = true;
            object result = await DialogHost.Show(stk,"Dialog");
            _ShowingDialog = false;
            if (result is bool boolResult && boolResult)
            {
                _AllowClose = true;
                Close();
            }
        }

        private void BtnClose_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void MWin_Closing(object sender, CancelEventArgs e)
        {
            Properties.Settings.Default.Save();
            Home.Un_EditHosts();
        }
        #endregion

        #region Function
        private void UpdateDB()
        {
            OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + @"\StudyBoxDB.accdb" + "';Persist Security Info=False;");
            try
            {
                connection.Open();
                OleDbCommand command1 = new OleDbCommand("select * from BlockList", connection);
                OleDbDataReader reader1 = command1.ExecuteReader();
                OleDbCommand command2 = new OleDbCommand("select * from BlockWebList", connection);
                OleDbDataReader reader2 = command2.ExecuteReader();
                OleDbCommand command3 = new OleDbCommand("select * from SavedApp", connection);
                OleDbDataReader reader3 = command3.ExecuteReader();
                OleDbCommand command4 = new OleDbCommand("select * from JobMon", connection);
                OleDbDataReader reader4 = command4.ExecuteReader();
                OleDbCommand command5 = new OleDbCommand("select * from JobTue", connection);
                OleDbDataReader reader5 = command5.ExecuteReader();
                OleDbCommand command6 = new OleDbCommand("select * from JobWed", connection);
                OleDbDataReader reader6 = command6.ExecuteReader();
                OleDbCommand command7 = new OleDbCommand("select * from JobThu", connection);
                OleDbDataReader reader7 = command7.ExecuteReader();
                OleDbCommand command8 = new OleDbCommand("select * from JobFri", connection);
                OleDbDataReader reader8 = command8.ExecuteReader();
                OleDbCommand command9 = new OleDbCommand("select * from JobSat", connection);
                OleDbDataReader reader9 = command9.ExecuteReader();
                OleDbCommand command10 = new OleDbCommand("select * from JobSun", connection);
                OleDbDataReader reader10 = command10.ExecuteReader();


                blockAppList.Clear();
                while (reader1.Read())
                {
                    blockAppList.Add(reader1["AppName"].ToString());
                }

                blockWebList.Clear();
                while (reader2.Read())
                {
                    blockWebList.Add(reader2["WebName"].ToString());
                }

                SavedApp.Clear();
                while (reader3.Read())
                {
                    SavedApp.Add(reader3["AppName"].ToString());
                    SavedAppDis.Add(reader3["DisplayName"].ToString());
                }
                JobMon.Clear();
                while (reader4.Read())
                {
                    JobMon.Add(new JobItem( reader4["Content"].ToString(), reader4["StartTime"].ToString(), reader4["EndTime"].ToString()));
                }
                JobTue.Clear();
                while (reader5.Read())
                {
                    JobTue.Add(new JobItem( reader5["Content"].ToString(), reader5["StartTime"].ToString(), reader5["EndTime"].ToString()));
                }
                JobWed.Clear();
                while (reader6.Read())
                {
                    JobWed.Add(new JobItem( reader6["Content"].ToString(), reader6["StartTime"].ToString(), reader6["EndTime"].ToString()));
                }
                JobThu.Clear();
                while (reader7.Read())
                {
                    JobThu.Add(new JobItem( reader7["Content"].ToString(), reader7["StartTime"].ToString(), reader7["EndTime"].ToString()));
                }
                JobFri.Clear();
                while (reader8.Read())
                {
                    JobFri.Add(new JobItem( reader8["Content"].ToString(), reader8["StartTime"].ToString(), reader8["EndTime"].ToString()));
                }
                JobSat.Clear();
                while (reader9.Read())
                {
                    JobSat.Add(new JobItem( reader9["Content"].ToString(), reader9["StartTime"].ToString(), reader9["EndTime"].ToString()));
                }
                JobSun.Clear();
                while (reader10.Read())
                {
                    JobSun.Add(new JobItem( reader10["Content"].ToString(), reader10["StartTime"].ToString(), reader10["EndTime"].ToString()));
                }

                connection.Close();

                savedApps.Clear();
                foreach (string Name in SavedApp)
                {
                    savedApps.Add(new SavedApps() { DisplayName = SavedAppDis[SavedApp.IndexOf(Name)], AppPath = Name });
                }

                UpdatingDB = false;
            }
            catch
            {
                connection.Close();
                UpdatingDB = false;
            }

        }
        #endregion
    }
}
