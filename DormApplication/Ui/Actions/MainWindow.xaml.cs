using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Imaging;
using DormApplication.Ui.Reports;
using MahApps.Metro.Controls;

namespace DormApplication.Ui.Actions
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        private string _header;
        private string _btn;

        public MainWindow()
        {
            InitializeComponent();

        }

        private void btn_MouseEnter(object sender, MouseEventArgs e)
        {
            string str;
            str = sender.GetType().GetProperties()[37].GetValue(sender).ToString();

            foreach (var i in InfoClass.Default())
            {
                if (i.BtnName.Equals(str))
                {
                    lblCategory.Content = i.Category;
                    _header = i.Category;
                    _btn = str;
                    lstView.Items.Clear();
                    foreach (string s in i.Lst)
                    {
                        ListBoxItem item = new ListViewItem();
                        item.Content = s;
                        lstView.Items.Add(item);
                    }
                    break;
                }
            }
        }

        private void MenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lblCategory.Content = "Здравствуйте, " + AppSettings.Admin + "!";
        }

        private void btn_Click(object sender, RoutedEventArgs e)
        {
            string btnName = ((Button)sender).Name.ToString();
            switch (btnName)
            {
                case "btnEnterPerson":
                    InsertPersonWindow insertPersonWindow = new InsertPersonWindow();
                    Close();
                    insertPersonWindow.Show();
                    break;
                case "btnQuit":
                    ChangeRoomWindow changeRoomWindow = new ChangeRoomWindow();
                    changeRoomWindow.Show();
                    Close();
                    break;
                case "btnReport":
                    GenerateReportWindow generateReportWindow = new GenerateReportWindow();
                    generateReportWindow.Show();
                    Close();
                    break;
                case "btnTarif":
                    PriceListWindow priceListWindow = new PriceListWindow();
                    priceListWindow.Show();
                    Close();
                    break;
                case "btnStartListView":
                    lstView.Items.Clear();
                    lblCategory.Content = "Здравствуйте, " + AppSettings.Admin + "!";
                    ListViewItem item = new ListViewItem();
                    Image img = new Image();
                    img.Source = new BitmapImage(new Uri("pack://application:,,,/DormApplication;component/Images/Dormitory.png"));
                    item.Style = (Style)FindResource("ImageDorm");
                    item.Resources.Add(0, img);
                    lstView.Items.Add(img);
                    lstView.Items.Add("> Справка");
                    lstView.Items.Add("> О программе");
                    break;
                case "btnPay":
                    PaymentWindow paymentWindow = new PaymentWindow();
                    paymentWindow.Show();
                    Close();
                    break;
                case "bntHistory":
                    HistoryWindow historyWindow = new HistoryWindow();
                    historyWindow.Show();
                    Close();
                    break;
                default:
                    break;
            }
        }
    }
}
