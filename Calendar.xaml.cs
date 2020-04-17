using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace StudyBox
{
    /// <summary>
    /// Interaction logic for Calendar.xaml
    /// </summary>
    public partial class Calendar : Window
    {
        private bool IsChangingDates = false;
        public Calendar()
        {
            InitializeComponent();
            rectangle.Background = new SolidColorBrush(Properties.Settings.Default.Color);
            CalMain.DisplayDateEnd = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).AddYears(100);
            CalMain.SelectedDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
            //if (cmbDate.Items.Count!=0)
            //{
            //    cmbDate.Items.Clear();
            //}
            //if (cmbMonth.Items.Count != 0)
            //{
            //    cmbMonth.Items.Clear();
            //}
            //if (cmbYear.Items.Count != 0)
            //{
            //    cmbYear.Items.Clear();
            //}
            for (int i = 1900; i <= new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Year + 100; i++)
            {
                cmbYear.Items.Add(i);
            }
            for (int i = 1; i <= 12; i++)
            {
                cmbMonth.Items.Add(i);
            }
            cmbYear.SelectedItem = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Year;
            cmbMonth.SelectedItem = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Month;
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

        private void CalMain_SelectedDatesChanged(object sender, SelectionChangedEventArgs e)
        {
            Mouse.Capture(null);
            cmbYear.SelectedItem = ((DateTime)CalMain.SelectedDate).Year;
            cmbMonth.SelectedItem = ((DateTime)CalMain.SelectedDate).Month;
            cmbDate.SelectedItem = ((DateTime)CalMain.SelectedDate).Day;
        }

        private void cmbYear_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CalMain.SelectedDate = new DateTime((int)cmbYear.SelectedItem, ((DateTime)CalMain.SelectedDate).Month, ((DateTime)CalMain.SelectedDate).Day);
                CalMain.DisplayDate = new DateTime((int)cmbYear.SelectedItem, ((DateTime)CalMain.SelectedDate).Month, ((DateTime)CalMain.SelectedDate).Day);

            }
            catch
            {
                cmbDate.SelectedItem = DateTime.Now.Day;
                CalMain.SelectedDate = new DateTime((int)cmbYear.SelectedItem, ((DateTime)CalMain.SelectedDate).Month, ((DateTime)CalMain.SelectedDate).Day);
                CalMain.DisplayDate = new DateTime((int)cmbYear.SelectedItem, ((DateTime)CalMain.SelectedDate).Month, ((DateTime)CalMain.SelectedDate).Day);
            }


            try
            {
                //cmbDate.SelectedItem = null;
                if (cmbDate.Items.Count != 0)
                {
                    IsChangingDates = true;
                    cmbDate.Items.Clear();
                    IsChangingDates = false;
                }
            }
            catch
            {
            }
            //for (int i = 1; i < cmbDate.Items.Count; i+=0)
            //{
            //    cmbDate.Items.RemoveAt(0);
            //}
            try
            {
                switch ((int)cmbMonth.SelectedItem)
                {
                    case 1:
                    case 3:
                    case 5:
                    case 7:
                    case 8:
                    case 10:
                    case 12:
                        for (int i = 1; i <= 31; i++)
                        {
                            cmbDate.Items.Add(i);
                        }
                        break;
                    case 2:
                        if (((int)cmbYear.SelectedItem % 4 == 0 && (int)cmbYear.SelectedItem % 100 != 0) || (int)cmbYear.SelectedItem % 400 == 0)
                        {
                            //cmbDate.Items.Clear();
                            for (int i = 1; i <= 29; i++)
                            {
                                cmbDate.Items.Add(i);
                            }
                            break;
                        }
                        else
                        {
                            //cmbDate.Items.Clear();
                            for (int i = 1; i <= 28; i++)
                            {
                                cmbDate.Items.Add(i);
                            }
                            break;
                        }
                    default:
                        //cmbDate.Items.Clear();
                        for (int i = 1; i <= 30; i++)
                        {
                            cmbDate.Items.Add(i);
                        }
                        break;
                }

            }
            catch
            {}
            cmbDate.SelectedItem = ((DateTime)CalMain.SelectedDate).Day;

        }

        private void btnToday_Click(object sender, RoutedEventArgs e)
        {
            CalMain.SelectedDate = new DateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Year, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Month, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Day);
            CalMain.DisplayDate = new DateTime(new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Year, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Month, new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day).Day);
        }

        private void cmbMonth_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                CalMain.SelectedDate = new DateTime(((DateTime)CalMain.SelectedDate).Year, (int)cmbMonth.SelectedItem, ((DateTime)CalMain.SelectedDate).Day);
                CalMain.DisplayDate = new DateTime(((DateTime)CalMain.SelectedDate).Year, (int)cmbMonth.SelectedItem, ((DateTime)CalMain.SelectedDate).Day);

            }
            catch
            {
                cmbDate.SelectedItem = DateTime.Now.Day;
                CalMain.SelectedDate = new DateTime(((DateTime)CalMain.SelectedDate).Year, (int)cmbMonth.SelectedItem, ((DateTime)CalMain.SelectedDate).Day);
                CalMain.DisplayDate = new DateTime(((DateTime)CalMain.SelectedDate).Year, (int)cmbMonth.SelectedItem, ((DateTime)CalMain.SelectedDate).Day);

            }

            //cmbDate.SelectedItem = null;
            if (cmbDate.Items.Count != 0)
            {
                IsChangingDates = true;
                cmbDate.Items.Clear();
                IsChangingDates = false;


            }
            //for (int i = 1; i < cmbDate.Items.Count; i+=0)
            //{
            //    cmbDate.Items.RemoveAt(0);
            //}
            switch ((int)cmbMonth.SelectedItem)
            {
                case 1:
                case 3:
                case 5:
                case 7:
                case 8:
                case 10:
                case 12:
                    for (int i = 1; i <= 31; i++)
                    {
                        cmbDate.Items.Add(i);
                    }
                    break;
                case 2:
                    if (((int)cmbYear.SelectedItem % 4 == 0 && (int)cmbYear.SelectedItem % 100 != 0) || (int)cmbYear.SelectedItem % 400 == 0)
                    {
                        //cmbDate.Items.Clear();
                        for (int i = 1; i <= 29; i++)
                        {
                            cmbDate.Items.Add(i);
                        }
                        break;
                    }
                    else
                    {
                        //cmbDate.Items.Clear();
                        for (int i = 1; i <= 28; i++)
                        {
                            cmbDate.Items.Add(i);
                        }
                        break;
                    }
                default:
                    //cmbDate.Items.Clear();
                    for (int i = 1; i <= 30; i++)
                    {
                        cmbDate.Items.Add(i);
                    }
                    break;
            }
            cmbDate.SelectedItem = ((DateTime)CalMain.SelectedDate).Day;

        }

        private void cmbDate_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!IsChangingDates)
            {
                CalMain.SelectedDate = new DateTime(((DateTime)CalMain.SelectedDate).Year, ((DateTime)CalMain.SelectedDate).Month, (int)cmbDate.SelectedItem);
                CalMain.DisplayDate = new DateTime(((DateTime)CalMain.SelectedDate).Year, ((DateTime)CalMain.SelectedDate).Month, (int)cmbDate.SelectedItem);
            }
        }
    }
}
