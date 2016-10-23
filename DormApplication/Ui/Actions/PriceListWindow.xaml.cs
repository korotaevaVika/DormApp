using DormApp.Domain;
using DormApp.Entities;
using DormApplication.Ui.Dialogs;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace DormApplication.Ui.Actions
{
    /// <summary>
    /// Логика взаимодействия для PriceListWindow.xaml
    /// </summary>
    public partial class PriceListWindow : MetroWindow
    {
        private int _roomTypeId;

        public PriceListWindow()
        {
            InitializeComponent();
            lblHeader.Content += AppSettings.DormName;
            dateStart.SelectedDate = DateTime.Today;

            using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
            {
                gridPriceList.ItemsSource = unitOfWork.GetTariffs(AppSettings.DormId);
                comboRoomType.ItemsSource = unitOfWork.RoomTypes.GetRoomTypeNames().ToList();
                unitOfWork.Dispose();
            }
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            this.Close();
        }

        private void gridPriceList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Tariff item = e.Row.Item as Tariff;
            if (item != null)
            {
                if (checkIsRelevant(item.DateStart, item.DateFinish) == true)
                {
                    e.Row.Background = System.Windows.Media.Brushes.LightGreen;
                }
                else
                {
                    e.Row.Background = System.Windows.Media.Brushes.White;
                }
            }
        }

        private void comboRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                _roomTypeId = unitOfWork.RoomTypes.GetRoomTypeId(((ComboBox)sender).SelectedItem.ToString());
                unitOfWork.Dispose();
            }
        }

        private void btnAddRoomType_Click(object sender, RoutedEventArgs e)
        {
            InputNameDialog window = new InputNameDialog("Введите название");
            if (window.ShowDialog().Value == true)
            {
                using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
                {
                    unitOfWork.RoomTypes.Add(new RoomType { name = window.Answer });
                    comboRoomType.ItemsSource = unitOfWork.RoomTypes.GetRoomTypeNames();
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
            }
        }

        private void Student_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).IsChecked == true)
            {
                checkBudget.IsEnabled = true;
            }
            else
            {
                checkBudget.IsEnabled = false;
            }
        }

        private void btnAddTariff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tariff tariff = (Tariff)this.TryFindResource("tariff");
                tariff.IsStudent = (checkStudent.IsChecked == true) ? true : false;
                tariff.OnBudget = (tariff.IsStudent == true) && checkBudget.IsChecked.HasValue ?
                    checkBudget.IsChecked.Value : false;
                string errMessage;
                using (var unitOfWork = new UnitOfWork(new Dormitory_Entities()))
                {
                    errMessage = unitOfWork.AddTariff(tariff, AppSettings.DormId, AppSettings.Admin);
                    unitOfWork.Complete();
                    gridPriceList.ItemsSource = unitOfWork.GetTariffs(AppSettings.DormId);
                    unitOfWork.Dispose();
                }
                if (!errMessage.Equals(string.Empty)) { MessageBox.Show(errMessage); }
            }
            catch (Exception ex) { MessageBox.Show(ex.ToString()); }
        }

        private bool checkIsRelevant(DateTime DateStart, DateTime? DateFinish)
        {
            return (((DateFinish.HasValue == true) && (DateStart <= DateTime.Now.Date) && (DateFinish >= DateTime.Now.Date))
                    || ((DateFinish.HasValue == false) && (DateStart <= DateTime.Now.Date))) ? true : false;
        }
    }
}
