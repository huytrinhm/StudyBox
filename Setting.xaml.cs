using MaterialDesignThemes.Wpf;
using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using System.Data.OleDb;
using System.ComponentModel;
using Microsoft.Win32;
using System.IO;

namespace StudyBox
{
    /// <summary>
    /// Interaction logic for Setting.xaml
    /// </summary>
    public partial class Setting : Page, INotifyPropertyChanged
    { 

        private ObservableCollection<string> _List;
        public ObservableCollection<string> List { get => _List; set { _List = value; OnPropertyChanged("List"); } }

        private ObservableCollection<string> _List2;
        public ObservableCollection<string> List2 { get => _List2; set { _List2 = value; OnPropertyChanged("List2"); } }

        private string _SelectedItem;
        public string SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem;
                }
            }
        }

        private string _SelectedItem2;
        public string SelectedItem2
        {
            get => _SelectedItem2;
            set
            {
                _SelectedItem2 = value;
                OnPropertyChanged("SelectedItem2");
                if (SelectedItem2 != null)
                {
                    DisplayName2 = SelectedItem2;
                }
            }
        }

        private string _DisplayName;

        private string _DisplayName2;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string newName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(newName));
        }

        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged("DisplayName"); } }

        public string DisplayName2 { get => _DisplayName2; set { _DisplayName2 = value; OnPropertyChanged("DisplayName2"); } }

        public Setting(MainWindow main)
        {
            InitializeComponent();
            DataContext = this;
            more.Foreground = new SolidColorBrush(Properties.Settings.Default.Color);
            List = new ObservableCollection<string>(MainWindow.blockAppList);
            List2 = new ObservableCollection<string>(MainWindow.blockWebList);
        }


        private void Button_Click(object sender, RoutedEventArgs e)
        {
            SetColor((sender as Button).Background);
        }
        private void SetColor(Brush color)
        {
            MainWindow mw;
            mw = (MainWindow)Application.Current.MainWindow;
            mw.rectangle.Background = color;
            Properties.Settings.Default.Color = ((SolidColorBrush)color).Color;
            Properties.Settings.Default.Save();
            more.Foreground = new SolidColorBrush(Properties.Settings.Default.Color);

            //PaletteHelper palette = new PaletteHelper();
            //List<Hue> pri = new List<Hue>();
            //pri.Add(new Hue("priHue", Properties.Settings.Default.Color, Colors.White));
            //List<Hue> acc = new List<Hue>();
            //acc.Add(new Hue("accHue", Properties.Settings.Default.Color, Colors.White));
            //Swatch swatch = new Swatch("swatch", pri, acc);
            //palette.ReplacePrimaryColor(swatch);
            Application.Current.Resources["PrimaryHueLightBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
            Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
            Application.Current.Resources["PrimaryHueDarkBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
            Application.Current.Resources["SecondaryAccentBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            if (Regex.IsMatch(txtC.Text,"^#(([0-9a-fA-F]){8})$"))
            {
                MainWindow mw;
                mw = (MainWindow)Application.Current.MainWindow;
                Color color = new Color();
                color = (Color)(ColorConverter.ConvertFromString(txtC.Text));
                color.A = 255;
                mw.rectangle.Background = new SolidColorBrush(color);
                Properties.Settings.Default.Color = color;
                Properties.Settings.Default.Save();
                more.Foreground = new SolidColorBrush(Properties.Settings.Default.Color);
                txtC.Text = "";
                Application.Current.Resources["PrimaryHueLightBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
                Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
                Application.Current.Resources["PrimaryHueDarkBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
                Application.Current.Resources["SecondaryAccentBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);

            }
            else
            {
                if (Regex.IsMatch(txtC.Text, "^#(([0-9a-fA-F]){6})$"))
                {
                    //string txt_color = txtC.Text.Replace("#","");

                    MainWindow mw;
                    mw = (MainWindow)Application.Current.MainWindow;
                    Color color = new Color();
                    color = (Color)ColorConverter.ConvertFromString(txtC.Text);
                    //color.R = Convert.ToByte(Char.GetNumericValue(txt_color[0])*10 + Char.GetNumericValue(txt_color[1]));
                    //color.G = Convert.ToByte(Char.GetNumericValue(txt_color[2]) * 10 + Char.GetNumericValue(txt_color[3]));
                    //color.B = Convert.ToByte(Char.GetNumericValue(txt_color[4]) * 10 + Char.GetNumericValue(txt_color[5]));
                    color.A = 255;
                    mw.rectangle.Background = new SolidColorBrush(color);
                    Properties.Settings.Default.Color = color;
                    Properties.Settings.Default.Save();
                    more.Foreground = new SolidColorBrush(Properties.Settings.Default.Color);
                    txtC.Text = "";
                    Application.Current.Resources["PrimaryHueLightBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
                    Application.Current.Resources["PrimaryHueMidBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
                    Application.Current.Resources["PrimaryHueDarkBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);
                    Application.Current.Resources["SecondaryAccentBrush"] = new SolidColorBrush(Properties.Settings.Default.Color);

                }
                else
                {
                    Error();

                }
            }

        }
        private async void Error()
        {
            TextBlock txt1 = new TextBlock
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B")),
                Margin = new Thickness(4),
                TextWrapping = TextWrapping.WrapWithOverflow,
                FontSize = 18,
                Text = "Sai định dạng mã màu!" + " (" + txtC.Text + ")"
            };

            Button btn1 = new Button();
            Style style = Application.Current.FindResource("MaterialDesignFlatButton") as Style;
            btn1.Style = style;
            btn1.Width = 115;
            btn1.Height = 30;
            btn1.Margin = new Thickness(5);
            btn1.Command = DialogHost.CloseDialogCommand;
            btn1.Content = "Đồng ý";

            StackPanel stk = new StackPanel
            {
                Width = 250
            };
            stk.Children.Add(txt1);
            stk.Children.Add(btn1);

            object result = await DialogHost.Show(stk, "Set");
            txtC.Text = "";
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedItem != null)
                {
                    //Delete in DB
                    OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + @"\StudyBoxDB.accdb" + "';Persist Security Info=False;");
                    try
                    {
                        connection.Open();
                        OleDbCommand command1 = new OleDbCommand("delete from BlockList where AppName='" + SelectedItem + "'", connection);
                        command1.ExecuteNonQuery();

                        connection.Close();
                    }
                    catch
                    {
                        connection.Close();
                    }

                    //Delete in blockAppList
                    MainWindow.blockAppList.Remove(SelectedItem);

                    //Delete in List
                    List.Remove(SelectedItem);

                }

            }
            catch { }

        }


        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(DisplayName))
                {
                    var displayList = MainWindow.blockAppList.Where(x => x == DisplayName);
                    if (!(displayList == null || displayList.Count() != 0))
                    {
                        var app = DisplayName.Trim();

                        //Add app to DB
                        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + @"\StudyBoxDB.accdb" + "';Persist Security Info=False;");
                        try
                        {
                            connection.Open();
                            OleDbCommand command1 = new OleDbCommand("insert into BlockList values('" + app + "')", connection);
                            command1.ExecuteNonQuery();

                            connection.Close();
                        }
                        catch
                        {
                            connection.Close();
                        }

                        //Add app to blockAppList
                        MainWindow.blockAppList.Add(app);
                        //Add app to List
                        List.Add(app);
                    }
                }

            }
            catch { }

        }

        private void Browse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                OpenFileDialog openFile = new OpenFileDialog
                {
                    Title = "Chọn ứng dụng cần chặn...",
                    ValidateNames = true,
                    DereferenceLinks = true,
                    DefaultExt = ".exe",
                    Filter = "Exe Files (.exe)|*.exe",
                    InitialDirectory = @"C:\"
                };

                bool? result = openFile.ShowDialog(Application.Current.MainWindow);

                if (result == true)
                {
                    if (String.Equals(Path.GetExtension(openFile.FileName), ".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        DisplayName = openFile.FileName;
                    }
                    else
                    {
                        MessageBox.Show("Bạn chưa chọn ứng dụng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    }
                }

            }
            catch { }
        }

        private void Add2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(DisplayName2))
                {
                    var displayList = MainWindow.blockWebList.Where(x => x == DisplayName2);
                    if (!(displayList == null || displayList.Count() != 0))
                    {
                        var web = DisplayName2.Trim();

                        //Add web to DB
                        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + @"\StudyBoxDB.accdb" + "';Persist Security Info=False;");
                        try
                        {
                            connection.Open();
                            OleDbCommand command1 = new OleDbCommand("insert into BlockWebList values('" + web + "')", connection);
                            command1.ExecuteNonQuery();

                            connection.Close();
                        }
                        catch
                        {
                            connection.Close();
                        }

                        //Add web to blockWebList
                        MainWindow.blockWebList.Add(web);
                        //Add web to List
                        List2.Add(web);

                        if (Home.IsStart)
                        {
                            Home.EditHosts();
                        }
                    }
                }

            }
            catch { }

        }


        private void Delete2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedItem2 != null)
                {
                    //Delete in DB
                    OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + @"\StudyBoxDB.accdb" + "';Persist Security Info=False;");
                    try
                    {
                        connection.Open();
                        OleDbCommand command1 = new OleDbCommand("delete from BlockWebList where WebName='" + SelectedItem2 + "'", connection);
                        command1.ExecuteNonQuery();

                        connection.Close();
                    }
                    catch
                    {
                        connection.Close();
                    }

                    //Delete in blockWebList
                    MainWindow.blockWebList.Remove(SelectedItem2);

                    //Delete in List
                    List2.Remove(SelectedItem2);

                    if (Home.IsStart)
                    {
                        Home.EditHosts();
                    }

                }

            }
            catch { }

        }
    }
}
