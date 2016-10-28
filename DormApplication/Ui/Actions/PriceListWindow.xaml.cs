using DormApp.Domain;
using DormApp.Domain.Interfaces;
using DormApplication.Ui.Dialogs;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Ninject;
using System;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

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

            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                gridPriceList.ItemsSource = unitOfWork.GetTariffs(AppSettings.DormId);
                comboRoomType.ItemsSource = unitOfWork.RoomTypes.GetRoomTypeNames().ToList();
            }
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            Close();
        }

        private void gridPriceList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            Tariff item = e.Row.Item as Tariff;
            if (item != null)
            {
                if (checkIsRelevant(item.DateStart, item.DateFinish) == true)
                {
                    e.Row.Background = Brushes.LightGreen;
                }
                else
                {
                    e.Row.Background = Brushes.White;
                }
            }
        }

        private void comboRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                _roomTypeId = unitOfWork.RoomTypes.GetRoomTypeId(((ComboBox)sender).SelectedItem.ToString());
            }
        }

        private void btnAddRoomType_Click(object sender, RoutedEventArgs e)
        {
            InputNameDialog window = new InputNameDialog("Введите название");
            if (window.ShowDialog().Value == true)
            {
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    unitOfWork.RoomTypes.Add(new DormApp.Entities.RoomType { name = window.Answer });
                    comboRoomType.ItemsSource = unitOfWork.RoomTypes.GetRoomTypeNames();
                    unitOfWork.Complete();
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

        private async void btnAddTariff_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Tariff tariff = (Tariff)TryFindResource("tariff");
                tariff.IsStudent = (checkStudent.IsChecked == true) ? true : false;
                tariff.OnBudget = (tariff.IsStudent == true) && checkBudget.IsChecked.HasValue ?
                    checkBudget.IsChecked.Value : false;
                string errMessage;
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    errMessage = unitOfWork.AddTariff(tariff, AppSettings.DormId, AppSettings.Admin);
                    unitOfWork.Complete();
                    gridPriceList.ItemsSource = unitOfWork.GetTariffs(AppSettings.DormId);
                }
                if (!errMessage.Equals(string.Empty))
                {
                    await this.ShowMessageAsync("", errMessage);
                }
            }
            catch (Exception ex)
            {
                await this.ShowMessageAsync("...ы", "Что-то пошло не так...");
                Debug.WriteLine(ex.ToString());
            }
        }

        private bool checkIsRelevant(DateTime DateStart, DateTime? DateFinish)
        {
            return (((DateFinish.HasValue == true) && (DateStart <= DateTime.Now.Date) && (DateFinish >= DateTime.Now.Date))
                    || ((DateFinish.HasValue == false) && (DateStart <= DateTime.Now.Date))) ? true : false;
        }
    }
}
