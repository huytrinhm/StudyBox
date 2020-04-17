using Microsoft.Win32;
using System;
using System.Collections.Generic;
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
    /// Interaction logic for Goal.xaml
    /// </summary>
    public partial class Goal : Page, INotifyPropertyChanged
    {
        private ObservableCollection<JobItem> _List;
        public ObservableCollection<JobItem> List { get => _List; set { _List = value; OnPropertyChanged("List"); } }

        int lst_idx = 0;
        string TableName = "JobMon";
        static List<JobItem> TableJob = MainWindow.JobMon;

        private JobItem _SelectedItem;
        public JobItem SelectedItem
        {
            get => _SelectedItem;
            set
            {
                _SelectedItem = value;
                OnPropertyChanged("SelectedItem");
                if (SelectedItem != null)
                {
                    mContent = SelectedItem.Content;
                    StartText = SelectedItem.StartText;
                    EndText = SelectedItem.EndText;
                    StartTime = SelectedItem.StartTime;
                    EndTime = SelectedItem.EndTime;
                }
            }
        }

        private string _Content;
        public string mContent { get => _Content; set { _Content = value; OnPropertyChanged("Content"); } }

        private string _StartText;
        public string StartText { get => _StartText; set { _StartText = value; OnPropertyChanged("StartText"); } }

        private string _EndText;
        public string EndText { get => _EndText; set { _EndText = value; OnPropertyChanged("EndText"); } }

        private DateTime _StartTime;
        public DateTime StartTime {
            get => _StartTime;
            set {
                _StartTime = value;
                OnPropertyChanged("StartTime");
                if(StartTime != null)
                {
                    StartText = (StartTime.Hour.ToString().Length == 2 ? StartTime.Hour.ToString() : "0" + StartTime.Hour.ToString()) + ":" + (StartTime.Minute.ToString().Length == 2 ? StartTime.Minute.ToString() : "0" + StartTime.Minute.ToString());
                }
            }
        }

        private DateTime _EndTime;
        public DateTime EndTime {
            get => _EndTime;
            set {
                _EndTime = value;
                OnPropertyChanged("EndTime");
                if (EndTime != null)
                {
                    EndText = (EndTime.Hour.ToString().Length == 2 ? EndTime.Hour.ToString() : "0" + EndTime.Hour.ToString()) + ":" + (EndTime.Minute.ToString().Length == 2 ? EndTime.Minute.ToString() : "0" + EndTime.Minute.ToString());
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string newName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(newName));
        }


        public Goal()
        {
            InitializeComponent();
            DataContext = this;
            List = new ObservableCollection<JobItem>(MainWindow.JobMon);
            StartText = "07:00";
            EndText = "08:00";
            StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 7, 0, 0);
            EndTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, 8, 0, 0);
            
        }



        public delegate void AVoidDelegate();


        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (SelectedItem != null)
                {
                    
                    OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + @"\StudyBoxDB.accdb" + "';Persist Security Info=False;");
                    try
                    {
                        connection.Open();
                        OleDbCommand command1 = new OleDbCommand("delete from "+TableName+" where StartTime='" + SelectedItem.StartText + "'", connection);
                        command1.ExecuteNonQuery();

                        connection.Close();
                    }
                    catch
                    {
                        connection.Close();
                    }

                    TableJob.Remove(SelectedItem);

                    
                    List.Remove(SelectedItem);

                }

            }
            catch { }

        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!(string.IsNullOrEmpty(mContent) | !sPick.SelectedTime.HasValue | !ePick.SelectedTime.HasValue))
                {
                    var displayList = TableJob.Where(x => x.StartTime == StartTime);
                    if (!(displayList == null || displayList.Count() != 0))
                    {
                        var app = new JobItem( mContent.Trim(), StartText, EndText );

                        
                        OleDbConnection connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source='" + AppDomain.CurrentDomain.BaseDirectory + @"\StudyBoxDB.accdb" + "';Persist Security Info=False;");
                        try
                        {
                            connection.Open();
                            OleDbCommand command1 = new OleDbCommand("insert into " + TableName + " values('" + mContent.Trim() + "','" + StartText + "','" + EndText + "')", connection);
                            command1.ExecuteNonQuery();

                            connection.Close();
                        }
                        catch
                        {
                            connection.Close();
                        }

                        TableJob.Add(app);

                        List.Add(app);
                    }
                }

            }
            catch { }

        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            lst_idx = mListBox.SelectedIndex;

            switch (lst_idx)
            {
                case 0:
                    List = new ObservableCollection<JobItem>(MainWindow.JobMon);
                    TableName = "JobMon";
                    TableJob = MainWindow.JobMon;
                    break;

                case 1:
                    List = new ObservableCollection<JobItem>(MainWindow.JobTue);
                    TableName = "JobTue";
                    TableJob = MainWindow.JobTue;
                    break;

                case 2:
                    List = new ObservableCollection<JobItem>(MainWindow.JobWed);
                    TableName = "JobWed";
                    TableJob = MainWindow.JobWed;
                    break;

                case 3:
                    List = new ObservableCollection<JobItem>(MainWindow.JobThu);
                    TableName = "JobThu";
                    TableJob = MainWindow.JobThu;
                    break;

                case 4:
                    List = new ObservableCollection<JobItem>(MainWindow.JobFri);
                    TableName = "JobFri";
                    TableJob = MainWindow.JobFri;
                    break;

                case 5:
                    List = new ObservableCollection<JobItem>(MainWindow.JobSat);
                    TableName = "JobSat";
                    TableJob = MainWindow.JobSat;
                    break;

                case 6:
                    List = new ObservableCollection<JobItem>(MainWindow.JobSun);
                    TableName = "JobSun";
                    TableJob = MainWindow.JobSun;
                    break;

                default:
                    List = new ObservableCollection<JobItem>(MainWindow.JobMon);
                    TableName = "JobMon";
                    TableJob = MainWindow.JobMon;
                    break;
            }
        }
    }
}
