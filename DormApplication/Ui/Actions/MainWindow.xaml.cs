using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using DormApplication.Ui.Reports;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

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
            String str;
            str = sender.GetType().GetProperties()[37].GetValue(sender).ToString();

            foreach (var i in InfoClass.Default())
            {
                if (i.BtnName.Equals(str))
                {
                    lblCategory.Content = i.Category;
                    _header = i.Category;
                    _btn = str;
                    this.lstView.Items.Clear();
                    foreach (string s in i.Lst)
                    {
                        ListBoxItem item = new ListViewItem();
                        item.Content = s;
                        this.lstView.Items.Add(item);
                    }
                    break;
                }
            }
        }

        private void MenuWindow_Loaded(object sender, RoutedEventArgs e)
        {
            lblCategory.Content = "Здравствуйте, " + AppSettings.Admin + "!";
        }

        private async void btn_Click(object sender, RoutedEventArgs e)
        {
            string btnName = ((Button)sender).Name.ToString();

            switch (btnName)
            {
                case "btnEnterPerson":
                    InsertPersonWindow insertPersonWindow = new InsertPersonWindow();
                    this.Close();
                    insertPersonWindow.Show();
                    break;
                case "btnQuit":
                    try
                    {
                        ChangeRoomWindow changeRoomWindow = new ChangeRoomWindow();
                        changeRoomWindow.Show();
                        this.Close();
                    }
                    catch (Exception)
                    {
                        await this.ShowMessageAsync("Ошибка", "Что-то пошло не так");
                    }
                    break;
                case "btnReport":
                    GenerateReportWindow generateReportWindow = new GenerateReportWindow();
                    generateReportWindow.Show();
                    this.Close();
                    break;
                case "btnTarif":
                    PriceListWindow priceListWindow = new PriceListWindow();
                    priceListWindow.Show();
                    this.Close();
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
                    this.Close();
                    break;
                case "bntHistory":
                    HistoryWindow historyWindow = new HistoryWindow();
                    historyWindow.Show();
                    this.Close();
                    break;
                default:
                    break;
            }
        }
    }
}
