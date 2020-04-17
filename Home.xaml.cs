using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace StudyBox
{
    
    /// <summary>
    /// Interaction logic for Home.xaml
    /// </summary>
    public partial class Home : Page
    {
        public static bool IsStart = false;
        public static bool IsErrorNotified = false;
        private static Thread t;
        public Home()
        {
            InitializeComponent();
            if (t == null)
            {
                t = new Thread(Block);
                t.IsBackground = true;
                t.Start();
            }
            
        }
        public delegate void AVoidDelegate();
        

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            IsStart = !IsStart;
            if (!IsStart)
            {
                Start.Background = new SolidColorBrush(Properties.Settings.Default.Color);
                Start.BorderBrush = new SolidColorBrush(Properties.Settings.Default.Color);
                Start.Content = "Bắt đầu";
                try
                {
                    Un_EditHosts();
                }
                catch
                {
                    if (!IsErrorNotified)
                    {
                        IsErrorNotified = true;
                        ShowDialog();
                    }
                }
            }
            else
            {
                Start.Background = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF11B1B"));
                Start.BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF11B1B"));
                Start.Content = "Dừng";
                try
                {
                    EditHosts();

                }
                catch
                {
                    if (!IsErrorNotified)
                    {
                        IsErrorNotified = true;
                        ShowDialog();
                    }
                }
            }
        }

        async void ShowDialog()
        {
            TextBlock txt1 = new TextBlock();
            txt1.HorizontalAlignment = HorizontalAlignment.Center;
            txt1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt1.Margin = new Thickness(4);
            txt1.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.FontSize = 18;
            txt1.Text = "Lưu ý!";

            TextBlock txt2 = new TextBlock();
            txt2.HorizontalAlignment = HorizontalAlignment.Center;
            txt2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt2.Margin = new Thickness(4);
            txt2.TextWrapping = TextWrapping.WrapWithOverflow;
            txt2.FontSize = 18;
            txt2.Text = "Tài khoản không phải Admin sẽ không thể sử dụng chức năng:";

            TextBlock txt3 = new TextBlock();
            txt3.HorizontalAlignment = HorizontalAlignment.Center;
            txt3.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt3.Margin = new Thickness(4);
            txt3.TextWrapping = TextWrapping.WrapWithOverflow;
            txt3.FontSize = 18;
            txt3.Text = "Chặn trang web";

            TextBlock txt4 = new TextBlock();
            txt4.HorizontalAlignment = HorizontalAlignment.Center;
            txt4.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt4.Margin = new Thickness(4);
            txt4.TextWrapping = TextWrapping.WrapWithOverflow;
            txt4.FontSize = 18;
            txt4.Text = "Chặn 1 số ứng dụng";

            Button btn = new Button();
            Style style = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn.Style = style;
            btn.Width = 115;
            btn.Height = 30;
            btn.Margin = new Thickness(5);
            btn.Command = DialogHost.CloseDialogCommand;
            btn.CommandParameter = false;
            btn.Content = "Đồng ý";

            StackPanel stk = new StackPanel();
            stk.Width = 250;
            stk.Children.Add(txt1);
            stk.Children.Add(txt2);
            stk.Children.Add(txt3);
            stk.Children.Add(txt4);
            stk.Children.Add(btn);

            object result = await DialogHost.Show(stk,"hDialog");
        }
        async void ShowDialog2()
        {
            TextBlock txt1 = new TextBlock();
            txt1.HorizontalAlignment = HorizontalAlignment.Center;
            txt1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt1.Margin = new Thickness(4);
            txt1.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.FontSize = 18;
            txt1.Text = "Lưu ý!";

            TextBlock txt2 = new TextBlock();
            txt2.HorizontalAlignment = HorizontalAlignment.Center;
            txt2.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt2.Margin = new Thickness(4);
            txt2.TextWrapping = TextWrapping.WrapWithOverflow;
            txt2.FontSize = 18;
            txt2.Text = "Vui lòng kiểm tra kết nối mạng!";


            Button btn = new Button();
            Style style = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn.Style = style;
            btn.Width = 115;
            btn.Height = 30;
            btn.Margin = new Thickness(5);
            btn.Command = DialogHost.CloseDialogCommand;
            btn.CommandParameter = false;
            btn.Content = "Đồng ý";

            StackPanel stk = new StackPanel();
            stk.Width = 250;
            stk.Children.Add(txt1);
            stk.Children.Add(txt2);
            stk.Children.Add(btn);

            object result = await DialogHost.Show(stk, "hDialog");
        }

        private static string GetPath(int ProcessId)
        {
            var buffer = new StringBuilder(1024);
            IntPtr hprocess = OpenProcess(ProcessAccessFlags.QueryLimitedInformation, false, ProcessId);
            if (hprocess != IntPtr.Zero)
            {
                try
                {
                    int size = buffer.Capacity;
                    if (QueryFullProcessImageName(hprocess, 0, buffer, out size))
                    {
                        return buffer.ToString();
                    }
                    else
                    {
                        return "";
                    }
                }
                finally
                {
                    CloseHandle(hprocess);
                }
            }
            else
            {
                return "";
            }
        }

        [DllImport("kernel32.dll")]
        private static extern bool QueryFullProcessImageName(IntPtr hprocess, int dwFlags,
                       StringBuilder lpExeName, out int size);
        [DllImport("kernel32.dll")]
        private static extern IntPtr OpenProcess(ProcessAccessFlags dwDesiredAccess, bool bInheritHandle, int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool CloseHandle(IntPtr hHandle);

        [Flags]
        public enum ProcessAccessFlags : uint
        {
            All = 0x001F0FFF,
            Terminate = 0x00000001,
            CreateThread = 0x00000002,
            VirtualMemoryOperation = 0x00000008,
            VirtualMemoryRead = 0x00000010,
            DuplicateHandle = 0x00000040,
            CreateProcess = 0x000000080,
            SetQuota = 0x00000100,
            SetInformation = 0x00000200,
            QueryInformation = 0x00000400,
            QueryLimitedInformation = 0x00001000,
            Synchronize = 0x00100000
        }

        public void Block()
        {
            List<Process> processes = new List<Process>();
            List<string> subBlockAppList = new List<string>(MainWindow.blockAppList);
            while (true)
            {
                if (IsStart)
                {
                    subBlockAppList = new List<string>(MainWindow.blockAppList);
                    foreach (string name in subBlockAppList)
                    {
                        processes = Process.GetProcesses().ToList();
                        foreach (Process process in processes)
                        {
                            if (IsStart)
                            {
                                if (subBlockAppList.Contains(GetPath(process.Id)))
                                {
                                    try
                                    {
                                        process.Kill();
                                    }
                                    catch
                                    { }
                                }

                            }

                        }

                    }
                }
            }
        }

        public static void EditHosts()
        {
            string path = @"C:\Windows\System32\drivers\etc\HOSTS";
            
            if (File.Exists(path))
            {
                FileStream f1 = File.OpenRead(path);
                FileStream f2 = File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\TempHosts");
                f1.CopyTo(f2);
                f1.Close();
                f2.Close();
                //File.Delete(path);
            }
            if (!Directory.Exists(@"C:\Windows\System32\drivers\etc"))
            {
                Directory.CreateDirectory(@"C:\Windows\System32\drivers\etc");
            }
            StreamWriter fs = File.AppendText(path);

            string strValue = "\n";
            foreach (string item in MainWindow.blockWebList)
            {
                strValue += "127.0.0.1 " + item + "\n";
            }

            char[] value = new UTF8Encoding(true).GetChars(new UTF8Encoding(true).GetBytes(strValue));
            fs.Write(value, 0, value.Length);

            fs.Close();
            
        }

        public static void Un_EditHosts()
        {
            string path = @"C:\Windows\System32\drivers\etc\HOSTS";
            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + @"\TempHosts"))
            {
                FileStream f1 = File.OpenRead(AppDomain.CurrentDomain.BaseDirectory + @"\TempHosts");
                if (f1.Length != 0)
                {
                    FileStream f2 = File.Create(path);
                    f1.CopyTo(f2);
                    f1.Close();
                    f2.Close();
                }
            }
            else
            {
                FileStream f1 = File.OpenRead(path);
                FileStream f2 = File.Create(AppDomain.CurrentDomain.BaseDirectory + @"\TempHosts");
                f1.CopyTo(f2);
                f1.Close();
                f2.Close();
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            RunApp runApp = new RunApp();
            runApp.ShowDialog();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Thread brush = new Thread(() => {
                Process brush_pro = new Process();
                brush_pro.StartInfo.FileName = @"mspaint.exe";
                brush_pro.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                brush_pro.Start();
            }); brush.IsBackground = true;brush.Start();
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            Calendar calen = new Calendar();
            calen.ShowDialog();
        }

        private void Button_Click_3(object sender, RoutedEventArgs e)
        {
            Thread calc = new Thread(() => {
                Process calc_pro = new Process();
                calc_pro.StartInfo.FileName = @"calc.exe";
                calc_pro.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                calc_pro.Start();
            }); calc.IsBackground = true; calc.Start();

        }

        private void Button_Click_4(object sender, RoutedEventArgs e)
        {
            bool IsConnected;
            try
            {
                WebClient wc = new WebClient();
                wc.OpenRead("http://www.example.com/");
                IsConnected = true;
            }
            catch
            {
                IsConnected = false;
                ShowDialog2();
            }
            if (IsConnected)
            {
                Translate trans = new Translate();
                trans.ShowDialog();
            }
        }

        private void Button_Click_5(object sender, RoutedEventArgs e)
        {
            bool IsConnected;
            try
            {
                WebClient wc = new WebClient();
                wc.OpenRead("http://www.example.com/");
                IsConnected = true;
            }
            catch
            {
                IsConnected = false;
                ShowDialog2();
            }
            if (IsConnected)
            {
                Wikipedia wiki = new Wikipedia();
                wiki.ShowDialog();
            }
        }

        private void Button_Click_6(object sender, RoutedEventArgs e)
        {
            Note note = new Note();
            try
            {
                note.ShowDialog();
            }
            catch
            {
            }
        }
    }
}
