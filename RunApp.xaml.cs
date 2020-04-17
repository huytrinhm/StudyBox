using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data.OleDb;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace StudyBox
{
    /// <summary>
    /// Interaction logic for RunApp.xaml
    /// </summary>
    public partial class RunApp : Window, INotifyPropertyChanged
    {
        private ObservableCollection<SavedApps> _List;
        public ObservableCollection<SavedApps> List { get => _List; set { _List = value; OnPropertyChanged("List"); } }

        private SavedApps _SelectedItem;
        public SavedApps SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
                if (SelectedItem != null)
                {
                    DisplayName = SelectedItem.DisplayName;
                    AppPath = SelectedItem.AppPath;
                }
            }
        }
        private string _DisplayName;
        public string DisplayName { get => _DisplayName; set { _DisplayName = value; OnPropertyChanged("DisplayName"); } }

        private string _AppPath;
        public string AppPath { get => _AppPath; set { _AppPath = value; OnPropertyChanged("AppPath"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string newName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(newName));
        }
        


        public RunApp()
        {
            InitializeComponent();
            DataContext = this;
            rectangle.Background = new SolidColorBrush(Properties.Settings.Default.Color);
            List = new ObservableCollection<SavedApps>(MainWindow.savedApps);
        }

        private bool FilterList(object item)
        {
            if (string.IsNullOrEmpty(txtKey.Text))
                return true;
            else
                return ((item as SavedApps).DisplayName.IndexOf(txtKey.Text.Trim(), StringComparison.OrdinalIgnoreCase) >= 0);
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
                this.DragMove();
            }
            catch { }
            finally { }
        }

        public delegate void AVoidDelegate();

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
                        OleDbCommand command1 = new OleDbCommand("delete from SavedApp where AppName='" + SelectedItem.AppPath + "'", connection);
                        command1.ExecuteNonQuery();

                        connection.Close();
                    }
                    catch
                    {
                        connection.Close();
                    }

                    //Delete in savedApps
                    MainWindow.savedApps.Remove(SelectedItem);

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
                if (!(string.IsNullOrEmpty(DisplayName) | string.IsNullOrEmpty(AppPath)))
                {
                    var displayList = MainWindow.savedApps.Where(x => x.AppPath == AppPath);
                    if (!(displayList == null || displayList.Count() != 0))
                    {
                        var app = new SavedApps() { AppPath = AppPath.Trim(), DisplayName = DisplayName.Trim() };

                        //Add app to DB
                        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + @"\StudyBoxDB.accdb" + "';Persist Security Info=False;");
                        try
                        {
                            connection.Open();
                            OleDbCommand command1 = new OleDbCommand("insert into SavedApp values('" + AppPath.Trim() + "','" + DisplayName.Trim() + "')", connection);
                            command1.ExecuteNonQuery();

                            connection.Close();
                        }
                        catch
                        {
                            connection.Close();
                        }
                        
                        MainWindow.savedApps.Add(app);

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
                    Title = "Chọn ứng dụng cần lưu...",
                    ValidateNames = true,
                    DereferenceLinks = true,
                    DefaultExt = ".exe",
                    Filter = "Exe Files (.exe)|*.exe",
                    InitialDirectory = @"C:\"
                };

                bool? result = openFile.ShowDialog(this);

                if (result == true)
                {
                    if (string.Equals(Path.GetExtension(openFile.FileName), ".exe", StringComparison.OrdinalIgnoreCase))
                    {
                        AppPath = openFile.FileName;
                    }
                    else
                    {
                        MessageBox.Show("Bạn chưa chọn ứng dụng!", "Lỗi", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.OK);
                    }
                }

            }
            catch { }
        }




        private void TextBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
        {
            Thread thr = new Thread(() =>
            {
                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() =>
                {
                    try
                    {
                        CollectionViewSource.GetDefaultView(MainList.ItemsSource).Refresh();
                    }
                    catch { }

                }));

            }); thr.IsBackground = true; thr.Start();
        
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            CollectionView view = (CollectionView)CollectionViewSource.GetDefaultView(MainList.Items);
            view.Filter = FilterList;
        }

        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            var app = ((ListViewItem)sender).Content as SavedApps;

            var path = app.AppPath;

            Thread thr = new Thread(() => {
                Process process = new Process();
                process.StartInfo.FileName = path;
                process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
                process.Start();
            }); thr.IsBackground = true; thr.Start();

        }
    }
}
