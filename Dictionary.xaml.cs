using MaterialDesignThemes.Wpf;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace StudyBox
{
    /// <summary>
    /// Interaction logic for Dictionary.xaml
    /// </summary>
    public partial class Dictionary : Page, INotifyPropertyChanged
    {
        public Dictionary()
        {
            InitializeComponent();
            DataContext = this;
            cmb2.ItemsSource = Fonts.SystemFontFamilies;
            cmb2.SelectedIndex = 0;
            cmb.ItemsSource = FontSizes;
            cmb.SelectedIndex = 23;
            LoadList();
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((lst_Fmt.Items[0] as ListBoxItem).IsSelected)
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Bold);
                    }
                }
                else
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(TextElement.FontWeightProperty, FontWeights.Normal);
                    }
                }

                if ((lst_Fmt.Items[1] as ListBoxItem).IsSelected)
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Italic);
                    }
                }
                else
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(TextElement.FontStyleProperty, FontStyles.Normal);
                    }
                }

                if ((lst_Fmt.Items[2] as ListBoxItem).IsSelected)
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, TextDecorations.Underline);
                    }
                }
                else
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(Inline.TextDecorationsProperty, null);
                    }
                }

            }
            catch
            {

            }

        }

        private void ListBox_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if ((lst_Alg.Items[0] as ListBoxItem).IsSelected)
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Left);
                    }
                }

                if ((lst_Alg.Items[1] as ListBoxItem).IsSelected)
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Center);
                    }
                }

                if ((lst_Alg.Items[2] as ListBoxItem).IsSelected)
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Right);
                    }
                }

                if ((lst_Alg.Items[3] as ListBoxItem).IsSelected)
                {
                    if (!mRichTxt.Selection.IsEmpty)
                    {
                        mRichTxt.Selection.ApplyPropertyValue(Paragraph.TextAlignmentProperty, TextAlignment.Justify);
                    }
                }


            }
            catch { }
        }

        private void mRichTxt_SelectionChanged(object sender, RoutedEventArgs e)
        {
            UpdateToggleButtonState();
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ApplyPropertyValueToSelectedText(TextElement.FontSizeProperty, cmb.SelectedItem);
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            SaveDocument();
        }
        void ApplyPropertyValueToSelectedText(DependencyProperty formattingProperty, object value)
        {
            try
            {
                if (value == null | mRichTxt.Selection.IsEmpty)
                    return;

                mRichTxt.Selection.ApplyPropertyValue(formattingProperty, value);

            }
            catch { return; }
        }
        private void UpdateSelectedFontSize()
        {
            object value = mRichTxt.Selection.GetPropertyValue(TextElement.FontSizeProperty);
            cmb.SelectedValue = (value == DependencyProperty.UnsetValue) ? null : value;
        }
        public double[] FontSizes
        {
            get
            {
                return new double[] 
                {
                    3.0, 4.0, 5.0, 6.0, 6.5, 7.0, 7.5, 8.0, 8.5, 9.0, 9.5, 10.0,
                    10.5, 11.0, 11.5, 12.0, 12.5, 13.0, 13.5, 14.0, 15.0, 16.0,
                    17.0, 18.0, 19.0, 20.0, 22.0, 24.0, 26.0, 28.0, 30.0, 32.0,
                    34.0, 36.0, 38.0, 40.0, 44.0, 48.0, 52.0, 56.0, 60.0, 64.0,
                    68.0, 72.0, 76.0, 80.0, 88.0, 96.0, 104.0, 112.0, 120.0,
                    128.0, 136.0, 144.0
                };
            }
        }

        private void cmb2_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FontFamily editValue = (FontFamily)cmb2.SelectedItem;
            ApplyPropertyValueToSelectedText(TextElement.FontFamilyProperty, editValue);
        }
        private void UpdateSelectedFontFamily()
        {
            object value = mRichTxt.Selection.GetPropertyValue(TextElement.FontFamilyProperty);
            FontFamily currentFontFamily = (FontFamily)((value == DependencyProperty.UnsetValue) ? null : value);
            if (currentFontFamily != null)
            {
                cmb2.SelectedItem = currentFontFamily;
            }
        }
        void UpdateItemCheckedState(ListBoxItem button, DependencyProperty formattingProperty, object expectedValue)
        {
            object currentValue = mRichTxt.Selection.GetPropertyValue(formattingProperty);
            button.IsSelected = (currentValue == DependencyProperty.UnsetValue) ? false : currentValue != null && currentValue.Equals(expectedValue);
        }
        private void UpdateToggleButtonState()
        {
            UpdateItemCheckedState(lst_Fmt.Items[0] as ListBoxItem, TextElement.FontWeightProperty, FontWeights.Bold);
            UpdateItemCheckedState(lst_Fmt.Items[1] as ListBoxItem, TextElement.FontStyleProperty, FontStyles.Italic);
            UpdateItemCheckedState(lst_Fmt.Items[2] as ListBoxItem, Inline.TextDecorationsProperty, TextDecorations.Underline);
            UpdateItemCheckedState(lst_Alg.Items[0] as ListBoxItem, Paragraph.TextAlignmentProperty, TextAlignment.Left);
            UpdateItemCheckedState(lst_Alg.Items[1] as ListBoxItem, Paragraph.TextAlignmentProperty, TextAlignment.Center);
            UpdateItemCheckedState(lst_Alg.Items[2] as ListBoxItem, Paragraph.TextAlignmentProperty, TextAlignment.Right);
            UpdateItemCheckedState(lst_Alg.Items[3] as ListBoxItem, Paragraph.TextAlignmentProperty, TextAlignment.Justify);
            UpdateSelectedFontSize();
            UpdateSelectedFontFamily();
        }
        async void ShowDialog()
        {
            TextBlock txt1 = new TextBlock();
            txt1.HorizontalAlignment = HorizontalAlignment.Center;
            txt1.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FFF53B3B"));
            txt1.Margin = new Thickness(4);
            txt1.TextWrapping = TextWrapping.WrapWithOverflow;
            txt1.FontSize = 18;
            txt1.Text = "Lỗi!";



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

            object result = await DialogHost.Show(stk, "Set");
        }
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Add();

        }
        private async void Add()
        {
            try
            {
                StackPanel stk = new StackPanel();
                TextBlock txt = new TextBlock() { Text = "Nhập tên vào đây:", Margin = new Thickness(5) };
                TextBox txtC2 = new TextBox() { Margin = new Thickness(5) };
                StackPanel stk2 = new StackPanel() { Orientation = Orientation.Horizontal, Margin = new Thickness(5), Width = 200 };
                Button btn1 = new Button() { Content = "Ok", Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style, Width = 95, Margin = new Thickness(2), Command = DialogHost.CloseDialogCommand, CommandParameter = false };
                Button btn2 = new Button() { Content = "Hủy", Style = Application.Current.FindResource("MaterialDesignFlatButton") as Style, Width = 95, Margin = new Thickness(2), Command = DialogHost.CloseDialogCommand, CommandParameter = false };
                btn1.Click += (sender, arg) => {
                    string basePath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\StudyBoxRichTexts\";

                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    string fTxt;
                    fTxt = txtC2.Text;
                    txtC2.Text = "";
                    if (!Regex.IsMatch(fTxt, @".+\.xaml$"))
                    {
                        fTxt = fTxt + ".xaml";
                    }
                    if (!(fTxt.Contains("/") | fTxt.Contains("\\") | fTxt.Contains(":") | fTxt.Contains("*") | fTxt.Contains("?") | fTxt.Contains("\"") | fTxt.Contains("<") | fTxt.Contains(">") | fTxt.Contains("|")))
                    {
                        if (!File.Exists(basePath + fTxt))
                        {
                            mRichTxt.Document = new FlowDocument(new Paragraph());

                            TextRange t = new TextRange(mRichTxt.Document.ContentStart, mRichTxt.Document.ContentEnd);
                            FileStream file = new FileStream(basePath + fTxt, FileMode.Create);
                            t.Save(file, DataFormats.XamlPackage);
                            file.Close();

                            Thread thr = new Thread(() =>
                            {
                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => { try {

                                        filesList.Add(new MyFile() { Name = fTxt.Remove(fTxt.Length - 5), AppPath = basePath + fTxt });

                                    } catch { } }));
                            }); thr.IsBackground = true; thr.Start();

                            Thread txtt = new Thread(() => {
                                Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => {
                                    try
                                    {
                                        string text = File.ReadAllText(basePath + fTxt);

                                        TextRange t2 = new TextRange(mRichTxt.Document.ContentStart,
                                                                        mRichTxt.Document.ContentEnd);
                                        FileStream file2 = new FileStream(basePath + fTxt, FileMode.Open);
                                        t2.Load(file2, DataFormats.XamlPackage);
                                        file.Close();

                                    }
                                    catch
                                    {
                                        ShowDialog();
                                    }
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
            catch
            { }
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
            txt2.Text = "Tên không được chứa ký tự /\\*?:\"<>|";


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
            txt2.Text = "Tên bị trùng.";


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

        public delegate void AVoidDelegate();
        void SaveDocument()
        {
            try
            {
                TextRange t = new TextRange(mRichTxt.Document.ContentStart, mRichTxt.Document.ContentEnd);
                FileStream file = new FileStream(((genlst.SelectedItem) as MyFile).AppPath, FileMode.Create);
                t.Save(file, DataFormats.XamlPackage);
                file.Close();

            }
            catch
            {

                ShowDialog();
            }
        }
        protected void HandleDoubleClick(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
            try
            {
                var app = ((ListViewItem)sender).Content as MyFile;

                var path = app.AppPath;
                string text = File.ReadAllText(path);

                TextRange t = new TextRange(mRichTxt.Document.ContentStart,
                                                mRichTxt.Document.ContentEnd);
                FileStream file = new FileStream(path, FileMode.Open);
                t.Load(file, DataFormats.XamlPackage);
                file.Close();
            }
            catch
            {

                ShowDialog();
            }


        }
        private ObservableCollection<FileInfo> _files;
        public ObservableCollection<FileInfo> files { get => _files; set { _files = value; OnPropertyChanged("files"); } }

        private ObservableCollection<MyFile> _filesList;
        public ObservableCollection<MyFile> filesList { get => _filesList; set { _filesList = value; OnPropertyChanged("filesList"); } }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string newName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(newName));
        }

        void LoadList()
        {
            string basepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + @"\StudyBoxRichTexts\";
            if (!Directory.Exists(basepath))
            {
                Directory.CreateDirectory(basepath);
            }
            DirectoryInfo d = new DirectoryInfo(basepath);
            files = new ObservableCollection<FileInfo>(d.GetFiles("*.xaml"));
            filesList = new ObservableCollection<MyFile>();
            filesList.Clear();
            foreach (FileInfo file in files)
            {
                filesList.Add(new MyFile() { Name = file.Name.Remove(file.Name.Length - 5), AppPath = file.FullName });
            }
        }

        private void Button_Click_2(object sender, RoutedEventArgs e)
        {
            try
            {
                if (File.Exists((genlst.SelectedItem as MyFile).AppPath))
                {
                    File.Delete((genlst.SelectedItem as MyFile).AppPath);
                    mRichTxt.Document = new FlowDocument(new Paragraph());
                    filesList.Remove(genlst.SelectedItem as MyFile);
                }
            }
            catch
            {
                
            }
        }
    }
}
