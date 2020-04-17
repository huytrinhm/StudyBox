using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace StudyBox
{
    /// <summary>
    /// Interaction logic for Note.xaml
    /// </summary>
    public partial class Note : Window
    {
        public Note()
        {
            InitializeComponent();
            rectangle.Background = new SolidColorBrush(Properties.Settings.Default.Color);

            Thread thr = new Thread(() =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(()=> { try { LoadFiles(); } catch { } }));
            });thr.IsBackground = true;thr.Start();
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

        public delegate void AVoidDelegate();
        void LoadFiles()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\StudyBoxTexts\";

            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }

            List<string> files = new List<string>(Directory.GetFiles(basePath, "*.txt", SearchOption.AllDirectories).ToList());
            wMain.Items.Clear();
            ContextMenu cntx = new ContextMenu();
            StackPanel stkm = new StackPanel();
            TextBlock txt = new TextBlock() { Text = "Nhập tên mới vào đây:", Margin = new Thickness(5) };
            TextBox txtC2 = new TextBox() { Margin = new Thickness(5) };
            StackPanel stk2 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(5), Width = 200 };
            Button btn1 = new Button() { Content = "Ok", Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style, Width = 95, Margin = new Thickness(2), Command = DialogHost.CloseDialogCommand, CommandParameter = false };
            Button btn2 = new Button() { Content = "Hủy", Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style, Width = 95, Margin = new Thickness(2), Command = DialogHost.CloseDialogCommand, CommandParameter = false };
            btn1.Click += (sender, arg) => {
                try
                {
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    string fTxt;
                    fTxt = txtC2.Text;
                    txtC2.Text = "";
                    if (!Regex.IsMatch(fTxt, @".+\.txt$"))
                    {
                        fTxt = fTxt + ".txt";
                    }
                    if (!(fTxt.Contains("/") | fTxt.Contains("\\") | fTxt.Contains(":") | fTxt.Contains("*") | fTxt.Contains("?") | fTxt.Contains("\"") | fTxt.Contains("<") | fTxt.Contains(">") | fTxt.Contains("|")))
                    {
                        if (!File.Exists(basePath + fTxt))
                        {
                            if (File.Exists(basePath + (((wMain.SelectedItem as ListViewItem).Content as StackPanel).Children[1] as TextBlock).Text + ".txt"))
                            {
                                File.Move(basePath + (((wMain.SelectedItem as ListViewItem).Content as StackPanel).Children[1] as TextBlock).Text + ".txt", basePath + fTxt);
                            }
                            else
                            {
                                _EShowDialog("Lỗi: File không tồn tại.");
                            }
                            Thread thr = new Thread(() =>
                            {
                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => { try { LoadFiles(); } catch { } }));
                            }); thr.IsBackground = true; thr.Start();


                        }
                        else
                        {
                            _ShowDialog2();
                        }
                    }
                    else
                    {

                        _ShowDialog();
                    }


                }
                catch { }

            };
            stk2.Children.Add(btn1);
            stk2.Children.Add(btn2);
            stkm.Children.Add(txt);
            stkm.Children.Add(txtC2);
            stkm.Children.Add(stk2);

            cntx.Items.Add(new MenuItem() { Header = "Đổi tên", Command = DialogHost.OpenDialogCommand, CommandParameter = stkm });
            MenuItem menuItem1 = new MenuItem() { Header = "Tải lại" }; menuItem1.Click += MenuItem_Click;
            MenuItem menuItem2 = new MenuItem() { Header = "Thêm" }; menuItem2.Click += MenuItem_Click_1;
            MenuItem menuItem3 = new MenuItem() { Header = "Xóa" }; menuItem3.Click += MenuItem_Click_2;
            cntx.Items.Add(menuItem1);
            cntx.Items.Add(menuItem2);
            cntx.Items.Add(menuItem3);
            foreach (string fileName in files)
            {
                ListViewItem lstvw = new ListViewItem();
                StackPanel stk = new StackPanel() {Orientation = Orientation.Horizontal };
                stk.Children.Add(new PackIcon());
                stk.Children.Add(new TextBlock() {Text =  System.IO.Path.GetFileNameWithoutExtension(fileName) });
                lstvw.Content = stk; lstvw.ToolTip = System.IO.Path.GetFileNameWithoutExtension(fileName);lstvw.ContextMenu = cntx ;
                lstvw.PreviewMouseLeftButtonUp += Btn_Click;
                wMain.Items.Add(lstvw);
            }
            if (wMain.Items.Count <= 0)
            {
                EmptyLabel.Visibility = Visibility.Visible;
            }
            else
            {
                EmptyLabel.Visibility = Visibility.Collapsed;
            }

        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread txt = new Thread(() => {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => {
                        try
                        {
                            Process txt_pro = new Process();
                            txt_pro.StartInfo.FileName = @"notepad.exe";
                            txt_pro.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                            txt_pro.StartInfo.Arguments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\StudyBoxTexts\" + (((sender as ListViewItem).Content as StackPanel).Children[1] as TextBlock).Text + ".txt";
                            txt_pro.Start();

                        }
                        catch { }
                    }));
                }); txt.IsBackground = true; txt.Start();

            }
            catch { }

        }


        private async void AddFile(byte mode)
        {
            try
            {
                if (mode == 0)
                {
                    string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\StudyBoxTexts\";

                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    string fTxt;
                    fTxt = txtC.Text;
                    txtC.Text = "";
                    if (!Regex.IsMatch(fTxt, @".+\.txt$"))
                    {
                        fTxt = fTxt + ".txt";
                    }
                    if (!(fTxt.Contains('/') | fTxt.Contains('\\') | fTxt.Contains(@":") | fTxt.Contains('*') | fTxt.Contains('?') | fTxt.Contains('"') | fTxt.Contains('<') | fTxt.Contains('>') | fTxt.Contains('|')))
                    {
                        if (!File.Exists(basePath + fTxt))
                        {
                            if (!(fTxt.Length > 150))
                            {
                                File.Create(basePath + fTxt);
                                Thread thr = new Thread(() =>
                                {
                                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => { try { LoadFiles(); } catch { } }));
                                }); thr.IsBackground = true; thr.Start();

                                Thread txt = new Thread(() => {
                                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => {
                                        try
                                        {
                                            Process txt_pro = new Process();
                                            txt_pro.StartInfo.FileName = @"notepad.exe";
                                            txt_pro.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                                            txt_pro.StartInfo.Arguments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\StudyBoxTexts\" + fTxt;
                                            txt_pro.Start();

                                        }
                                        catch { }
                                    }));
                                }); txt.IsBackground = true; txt.Start();

                            }
                            else
                            {
                                _EShowDialog("Lỗi: Tên file không được quá 147 ký tự.");
                            }

                        }
                        else
                        {
                            _ShowDialog2();
                        }
                    }
                    else
                    {

                        _ShowDialog();
                    }

                }
                else
                {
                    StackPanel stk = new StackPanel();
                    TextBlock txt = new TextBlock() { Text = "Nhập tên file vào đây:", Margin = new Thickness(5) };
                    TextBox txtC2 = new TextBox() { Margin = new Thickness(5) };
                    StackPanel stk2 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(5), Width = 200 };
                    Button btn1 = new Button() { Content = "Ok", Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style, Width = 95, Margin = new Thickness(2), Command = DialogHost.CloseDialogCommand, CommandParameter = false };
                    Button btn2 = new Button() { Content = "Hủy", Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style, Width = 95, Margin = new Thickness(2), Command = DialogHost.CloseDialogCommand, CommandParameter = false };
                    btn1.Click += (sender, arg) => {
                        string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\StudyBoxTexts\";

                        if (!Directory.Exists(basePath))
                        {
                            Directory.CreateDirectory(basePath);
                        }
                        string fTxt;
                        fTxt = txtC2.Text;
                        txtC2.Text = "";
                        if (!Regex.IsMatch(fTxt, @".+\.txt$"))
                        {
                            fTxt = fTxt + ".txt";
                        }
                        if (!(fTxt.Contains("/") | fTxt.Contains("\\") | fTxt.Contains(":") | fTxt.Contains("*") | fTxt.Contains("?") | fTxt.Contains("\"") | fTxt.Contains("<") | fTxt.Contains(">") | fTxt.Contains("|")))
                        {
                            if (!File.Exists(basePath + fTxt))
                            {
                                File.Create(basePath + fTxt);
                                Thread thr = new Thread(() =>
                                {
                                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => { try { LoadFiles(); } catch { } }));
                                }); thr.IsBackground = true; thr.Start();

                                Thread txtt = new Thread(() => {
                                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => {
                                        try
                                        {
                                            Process txt_pro = new Process();
                                            txt_pro.StartInfo.FileName = @"notepad.exe";
                                            txt_pro.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                                            txt_pro.StartInfo.Arguments = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\StudyBoxTexts\" + fTxt;
                                            txt_pro.Start();

                                        }
                                        catch { }
                                    }));
                                }); txtt.IsBackground = true; txtt.Start();

                            }
                            else
                            {
                                _ShowDialog2();
                            }
                        }
                        else
                        {

                            _ShowDialog();
                        }

                    };
                    stk2.Children.Add(btn1);
                    stk2.Children.Add(btn2);
                    stk.Children.Add(txt);
                    stk.Children.Add(txtC2);
                    stk.Children.Add(stk2);
                    object result = await DialogHost.Show(stk, "runDialog3");


                }

            }
            catch
            {

            }

        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            AddFile(0);
        }
        async void _ShowDialog()
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
            txt2.Text = "Tên file không được chứa ký tự /\\*?:\"<>|";


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

            object result = await DialogHost.Show(stk, "runDialog2");
        }
        async void _ShowDialog2()
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
            txt2.Text = "Tên file bị trùng.";


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

            object result = await DialogHost.Show(stk, "runDialog");
        }

        async void _EShowDialog(string content)
        {
            TextBlock txt1 = new TextBlock();
            txt1.HorizontalAlignment = HorizontalAlignment.Center;
            txt1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt1.Margin = new Thickness(4);
            txt1.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.FontSize = 18;
            txt1.Text = content;

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
            stk.Children.Add(btn);

            object result = await DialogHost.Show(stk, "erunDialog");
        }



        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {
            Thread thr = new Thread(() =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(()=> { try { LoadFiles(); } catch { } }));
            }); thr.IsBackground = true; thr.Start();

        }

        private void MenuItem_Click_1(object sender, RoutedEventArgs e)
        {
            AddFile(1);

        }

        private void MenuItem_Click_2(object sender, RoutedEventArgs e)
        {
            DeleteFile();
        }

        void DeleteFile()
        {
            string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\StudyBoxTexts\";
            if (File.Exists(basePath + (((wMain.SelectedItem as ListViewItem).Content as StackPanel).Children[1] as TextBlock).Text + ".txt"))
            {
                File.Delete(basePath + (((wMain.SelectedItem as ListViewItem).Content as StackPanel).Children[1] as TextBlock).Text + ".txt");
            }
            else
            {
                _EShowDialog("Lỗi: File Không tồn tại.");
            }
            Thread thr = new Thread(() =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(()=> { try { LoadFiles(); } catch { } }));
            }); thr.IsBackground = true; thr.Start();


        }


        private void txtKey_TextChanged(object sender, TextChangedEventArgs e)
        {
            Thread thr = new Thread(() =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() =>
                {
                    try
                    {
                                            
                    int countFiles = 0;
                    if (txtKey.Text.Trim() != "" && txtKey.Text != null)
                    {
                        foreach (ListViewItem btn in wMain.Items)
                        {
                            if (((btn.Content as StackPanel).Children[1] as TextBlock).Text.ToLower().Contains(txtKey.Text.Trim().ToLower()))
                            {
                                btn.Visibility = Visibility.Visible;
                                btn.Background = new SolidColorBrush(Colors.Gold);
                            }
                            else
                            {
                                btn.Visibility = Visibility.Collapsed;
                                btn.Background = new SolidColorBrush(Colors.Transparent);
                            }
                            countFiles++;
                        }
                        if (countFiles <= 0)
                        {
                            EmptyLabel.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            EmptyLabel.Visibility = Visibility.Collapsed;
                        }
                    }
                    else
                    {
                        foreach (ListViewItem btn in wMain.Items)
                        {
                            btn.Visibility = Visibility.Visible;
                            btn.Background = new SolidColorBrush(Colors.Transparent);
                        }
                        if (wMain.Items.Count <= 0)
                        {
                            EmptyLabel.Visibility = Visibility.Visible;
                        }
                        else
                        {
                            EmptyLabel.Visibility = Visibility.Collapsed;
                        }
                    }

                }
                    catch { }

                }));

            }); thr.IsBackground = true; thr.Start();
        }

    }
}
