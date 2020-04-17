using System.Collections.Generic;
using System.Threading;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using CefSharp;
using System.Net;

namespace StudyBox
{
    /// <summary>
    /// Interaction logic for Translate.xaml
    /// </summary>
    public partial class Translate : Window
    {
        private List<string> cmbInpl = new List<string>() { "Tự động", "Tiếng Việt", "English", "Afrikaans", "Albanian", "Amharic", "Arabic", "Armenian", "Azeerbaijani", "Basque", "Belarusian", "Bengali", "Bosnian", "Bulgarian", "Catalan", "Cebuano", "Chinese (Simplified)", "Chinese (Traditional)", "Corsican", "Croatian", "Czech", "Danish", "Dutch", "Esperanto", "Estonian", "Finnish", "French", "Frisian", "Galician", "Georgian", "German", "Greek", "Gujarati", "Haitian Creole", "Hausa", "Hawaiian", "Hebrew", "Hindi", "Hmong", "Hungarian", "Icelandic", "Igbo", "Indonesian", "Irish", "Italian", "Japanese", "Javanese", "Kannada", "Kazakh", "Khmer", "Korean", "Kurdish", "Kyrgyz", "Lao", "Latin", "Latvian", "Lithuanian", "Luxembourgish", "Macedonian", "Malagasy", "Malay", "Malayalam", "Maltese", "Maori", "Marathi", "Mongolian", "Myanmar (Burmese)", "Nepali", "Norwegian", "Nyanja (Chichewa)", "Pashto", "Persian", "Polish", "Portuguese (Portugal, Brazil)", "Punjabi", "Romanian", "Russian", "Samoan", "Scots Gaelic", "Serbian", "Sesotho", "Shona", "Sindhi", "Sinhala (Sinhalese)", "Slovak", "Slovenian", "Somali", "Spanish", "Sundanese", "Swahili", "Swedish", "Tagalog (Filipino)", "Tajik", "Tamil", "Telugu", "Thai", "Turkish", "Ukrainian", "Urdu", "Uzbek", "Welsh", "Xhosa", "Yiddish", "Yoruba", "Zulu" };
        private List<string> cmbOutl = new List<string>() { "Tiếng Việt", "English", "Afrikaans", "Albanian", "Amharic", "Arabic", "Armenian", "Azeerbaijani", "Basque", "Belarusian", "Bengali", "Bosnian", "Bulgarian", "Catalan", "Cebuano", "Chinese (Simplified)", "Chinese (Traditional)", "Corsican", "Croatian", "Czech", "Danish", "Dutch", "Esperanto", "Estonian", "Finnish", "French", "Frisian", "Galician", "Georgian", "German", "Greek", "Gujarati", "Haitian Creole", "Hausa", "Hawaiian", "Hebrew", "Hindi", "Hmong", "Hungarian", "Icelandic", "Igbo", "Indonesian", "Irish", "Italian", "Japanese", "Javanese", "Kannada", "Kazakh", "Khmer", "Korean", "Kurdish", "Kyrgyz", "Lao", "Latin", "Latvian", "Lithuanian", "Luxembourgish", "Macedonian", "Malagasy", "Malay", "Malayalam", "Maltese", "Maori", "Marathi", "Mongolian", "Myanmar (Burmese)", "Nepali", "Norwegian", "Nyanja (Chichewa)", "Pashto", "Persian", "Polish", "Portuguese (Portugal, Brazil)", "Punjabi", "Romanian", "Russian", "Samoan", "Scots Gaelic", "Serbian", "Sesotho", "Shona", "Sindhi", "Sinhala (Sinhalese)", "Slovak", "Slovenian", "Somali", "Spanish", "Sundanese", "Swahili", "Swedish", "Tagalog (Filipino)", "Tajik", "Tamil", "Telugu", "Thai", "Turkish", "Ukrainian", "Urdu", "Uzbek", "Welsh", "Xhosa", "Yiddish", "Yoruba", "Zulu" };
        public Translate()
        {
            InitializeComponent();
            rectangle.Background = new SolidColorBrush(Properties.Settings.Default.Color);
            txt.Text = "";
            cmbInp.ItemsSource = cmbInpl;
            cmbInp.SelectedItem = "Tự động";
            cmbOut.ItemsSource = cmbOutl;
            cmbOut.SelectedItem = "Tiếng Việt";
        }
        string GetLangCode(string langName)
        {
            string result = "";
            switch (langName)
            {
                case "Tự động":
                    result = "auto";
                    break;
                case "Afrikaans":
                    result = "af";
                    break;
                case "Albanian":
                    result = "sq";
                    break;
                case "Amharic":
                    result = "am";
                    break;
                case "Arabic":
                    result = "ar";
                    break;
                case "Armenian":
                    result = "hy";
                    break;
                case "Azeerbaijani":
                    result = "az";
                    break;
                case "Basque":
                    result = "eu";
                    break;
                case "Belarusian":
                    result = "be";
                    break;
                case "Bengali":
                    result = "bn";
                    break;
                case "Bosnian":
                    result = "bs";
                    break;
                case "Bulgarian":
                    result = "bg";
                    break;
                case "Catalan":
                    result = "ca";
                    break;
                case "Cebuano":
                    result = "ceb";
                    break;
                case "Chinese (Simplified)":
                    result = "zh-CN";
                    break;
                case "Chinese (Traditional)":
                    result = "zh-TW";
                    break;
                case "Corsican":
                    result = "co";
                    break;
                case "Croatian":
                    result = "hr";
                    break;
                case "Czech":
                    result = "cs";
                    break;
                case "Danish":
                    result = "da";
                    break;
                case "Dutch":
                    result = "nl";
                    break;
                case "English":
                    result = "en";
                    break;
                case "Esperanto":
                    result = "eo";
                    break;
                case "Estonian":
                    result = "et";
                    break;
                case "Finnish":
                    result = "fi";
                    break;
                case "French":
                    result = "fr";
                    break;
                case "Frisian":
                    result = "fy";
                    break;
                case "Galician":
                    result = "gl";
                    break;
                case "Georgian":
                    result = "ka";
                    break;
                case "German":
                    result = "de";
                    break;
                case "Greek":
                    result = "el";
                    break;
                case "Gujarati":
                    result = "gu";
                    break;
                case "Haitian Creole":
                    result = "ht";
                    break;
                case "Hausa":
                    result = "ha";
                    break;
                case "Hawaiian":
                    result = "haw";
                    break;
                case "Hebrew":
                    result = "iw";
                    break;
                case "Hindi":
                    result = "hi";
                    break;
                case "Hmong":
                    result = "hmn";
                    break;
                case "Hungarian":
                    result = "hu";
                    break;
                case "Icelandic":
                    result = "is";
                    break;
                case "Igbo":
                    result = "ig";
                    break;
                case "Indonesian":
                    result = "id";
                    break;
                case "Irish":
                    result = "ga";
                    break;
                case "Italian":
                    result = "it";
                    break;
                case "Japanese":
                    result = "ja";
                    break;
                case "Javanese":
                    result = "jw";
                    break;
                case "Kannada":
                    result = "kn";
                    break;
                case "Kazakh":
                    result = "kk";
                    break;
                case "Khmer":
                    result = "km";
                    break;
                case "Korean":
                    result = "ko";
                    break;
                case "Kurdish":
                    result = "ku";
                    break;
                case "Kyrgyz":
                    result = "ky";
                    break;
                case "Lao":
                    result = "lo";
                    break;
                case "Latin":
                    result = "la";
                    break;
                case "Latvian":
                    result = "lv";
                    break;
                case "Lithuanian":
                    result = "lt";
                    break;
                case "Luxembourgish":
                    result = "lb";
                    break;
                case "Macedonian":
                    result = "mk";
                    break;
                case "Malagasy":
                    result = "mg";
                    break;
                case "Malay":
                    result = "ms";
                    break;
                case "Malayalam":
                    result = "ml";
                    break;
                case "Maltese":
                    result = "mt";
                    break;
                case "Maori":
                    result = "mi";
                    break;
                case "Marathi":
                    result = "mr";
                    break;
                case "Mongolian":
                    result = "mn";
                    break;
                case "Myanmar (Burmese)":
                    result = "my";
                    break;
                case "Nepali":
                    result = "ne";
                    break;
                case "Norwegian":
                    result = "no";
                    break;
                case "Nyanja (Chichewa)":
                    result = "ny";
                    break;
                case "Pashto":
                    result = "ps";
                    break;
                case "Persian":
                    result = "fa";
                    break;
                case "Polish":
                    result = "pl";
                    break;
                case "Portuguese (Portugal, Brazil)":
                    result = "pt";
                    break;
                case "Punjabi":
                    result = "pa";
                    break;
                case "Romanian":
                    result = "ro";
                    break;
                case "Russian":
                    result = "ru";
                    break;
                case "Samoan":
                    result = "sm";
                    break;
                case "Scots Gaelic":
                    result = "gd";
                    break;
                case "Serbian":
                    result = "sr";
                    break;
                case "Sesotho":
                    result = "st";
                    break;
                case "Shona":
                    result = "sn";
                    break;
                case "Sindhi":
                    result = "sd";
                    break;
                case "Sinhala (Sinhalese)":
                    result = "si";
                    break;
                case "Slovak":
                    result = "sk";
                    break;
                case "Slovenian":
                    result = "sl";
                    break;
                case "Somali":
                    result = "so";
                    break;
                case "Spanish":
                    result = "es";
                    break;
                case "Sundanese":
                    result = "su";
                    break;
                case "Swahili":
                    result = "sw";
                    break;
                case "Swedish":
                    result = "sv";
                    break;
                case "Tagalog (Filipino)":
                    result = "tl";
                    break;
                case "Tajik":
                    result = "tg";
                    break;
                case "Tamil":
                    result = "ta";
                    break;
                case "Telugu":
                    result = "te";
                    break;
                case "Thai":
                    result = "th";
                    break;
                case "Turkish":
                    result = "tr";
                    break;
                case "Ukrainian":
                    result = "uk";
                    break;
                case "Urdu":
                    result = "ur";
                    break;
                case "Uzbek":
                    result = "uz";
                    break;
                case "Tiếng Việt":
                    result = "vi";
                    break;
                case "Welsh":
                    result = "cy";
                    break;
                case "Xhosa":
                    result = "xh";
                    break;
                case "Yiddish":
                    result = "yi";
                    break;
                case "Yoruba":
                    result = "yo";
                    break;
                case "Zulu":
                    result = "zu";
                    break;
                default:
                    result = "vi";
                    break;
            }
            return result;
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

        private void bMain_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void bMain_FrameLoadEnd(object sender, CefSharp.FrameLoadEndEventArgs e)
        {
            if (e.Frame.IsMain)
            {
                try
                {
                    Thread thr = new Thread(() =>
                    {
                        //document.evaluate('//*[@id="result_box"]/span', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;
                        Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => { try { txt.Text = WebUtility.HtmlDecode(bMain.EvaluateScriptAsync("document.evaluate('/html/body/div[3]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;").Result.Result.ToString()); } catch { txt.Text = ""; } }));
                    });thr.Start();
                }
                catch
                {
                }

            }
        }
        public delegate void AVoidDelegate();

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Thread thr = new Thread(() =>
                {
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => {
                        try
                        { txt.Text = "Đang dịch..."; }
                        catch { }
                    }));
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => {
                    try
                        {
                            LoadWeb("https://translate.google.com/m?&sl="+ GetLangCode(cmbInp.SelectedItem.ToString()) + "&tl="+ GetLangCode(cmbOut.SelectedItem.ToString()) + "&ie=UTF-8&prev=_m&q=" + WebUtility.UrlEncode(Inp.Text));
                        }
                        catch { }
                    }));
                    Thread.Sleep(5000);
                    Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => {
                        if (Inp.Text == "" | Inp.Text == null)
                        {
                            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => { try { txt.Text = ""; } catch { } }));
                        }
                        else
                        {
                            Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => { try { txt.Text = WebUtility.HtmlDecode(bMain.EvaluateScriptAsync("document.evaluate('/html/body/div[3]', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;").Result.Result.ToString()); } catch { txt.Text = ""; } }));
                        }
                    }));
                }); thr.Start();
            }
            catch
            {

            }
        }
        private void LoadWeb(string url)
        {
                bMain.Load(url);
        }

        private void bMain_LoadingStateChanged(object sender, LoadingStateChangedEventArgs e)
        {
            if (e.IsLoading == false)
            {
                try
                {
                    //document.evaluate('//*[@id="result_box"]/span', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;
                    //Dispatcher.BeginInvoke(DispatcherPriority.Normal, new AVoidDelegate(() => { txt.Text = bMain.EvaluateScriptAsync("document.evaluate('//*[@id=\"result_box\"]/span', document, null, XPathResult.FIRST_ORDERED_NODE_TYPE, null).singleNodeValue.innerHTML;").Result.Result.ToString(); }));
                }
                catch { }

            }
        }
    }
}
