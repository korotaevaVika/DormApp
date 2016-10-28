using DormApp.Domain;
using DormApp.Domain.Interfaces;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Ninject;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace DormApplication.Ui.Actions
{
    /// <summary>
    /// Логика взаимодействия для PaymentWindow.xaml
    /// </summary>
    public partial class PaymentWindow : MetroWindow
    {
        private AccountData accountData;
        private int _roomTypeId, _personId, _floor, _roomNum;
        private int _floorForDebit;

        public PaymentWindow()
        {
            InitializeComponent();
            lblHeader.Content += AppSettings.DormName;
            dateOfPayment.SelectedDate = DateTime.Today;
            _roomTypeId = _floor = _roomNum = _floorForDebit = 0;
            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                comboFloors.ItemsSource = comboFloorsForDebit.ItemsSource = unitOfWork.Dormitories.GetLivingFloors(AppSettings.DormId);
                comboRoomType.ItemsSource = unitOfWork.RoomTypes.GetRoomTypeNames().ToList();
            }
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            Close();
        }

        private void comboRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    _roomTypeId = unitOfWork.RoomTypes.GetRoomTypeId((comboRoomType.SelectedItem.ToString()));
                }
                UpdatePeopleCombobox();
            }
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
        }

        private void comboFloors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                comboRooms.ItemsSource = new List<string>();
                _floor = (int)((ComboBox)sender).SelectedItem;
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    comboRooms.ItemsSource = unitOfWork.GetLivingRooms(_floor, AppSettings.DormId).ToList();
                }
            }
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
        }

        private void comboRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int.TryParse(comboRooms.SelectedItem.ToString(), out _roomNum);
                UpdatePeopleCombobox();
            }
            catch (Exception ex) { Debug.WriteLine(ex.ToString()); }
        }

        private async void btnPay_Click(object sender, RoutedEventArgs e)
        {
            decimal sum;
            if ((_personId == 0) || !(decimal.TryParse(txtSumOfPayment.Text, out sum)))
            {
                await this.ShowMessageAsync("...", "Ошибка ввода данных");
            }
            else
            {
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    await this.ShowMessageAsync("", unitOfWork.AddPayment(_personId, sum, dateOfPayment.SelectedDate.Value.Date,
                        AppSettings.Admin, AppSettings.DormId));
                    ClearFields();
                    unitOfWork.Complete();
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearFields();
        }


        private void comboPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                string[] fio = comboPeople.SelectedItem.ToString().Split(' ');
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    _personId = unitOfWork.GetPersonId(new PersonData
                    {
                        DormID = AppSettings.DormId,
                        SurName = fio[0],
                        Name = fio[1],
                        Floor = _floor,
                        Room = _roomNum,
                        RoomType = unitOfWork.RoomTypes.GetRoomTypeId(comboRooms.SelectedItem.ToString())
                    });
                    accountData = unitOfWork.GetSuitableTariff(_personId, AppSettings.DormId);
                    FillFields(accountData);
                }
            }
            catch { }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                gridHistoryPayment.ItemsSource = unitOfWork.History.GetDebitAccountHistory(AppSettings.DormId).ToList();
            }
        }

        private void comboFloorsForDebit_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                btnDebitAccount.IsEnabled = true;
                _floorForDebit = (int)comboFloorsForDebit.SelectedItem;
            }
            catch { }
        }

        private async void btnDebitAccount_Click(object sender, RoutedEventArgs e)
        {
            if (_floorForDebit != 0)
            {
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    if (unitOfWork.DebitAccount(AppSettings.DormId, _floorForDebit, AppSettings.Admin))
                    {
                        unitOfWork.Complete();
                        gridHistoryPayment.ItemsSource = unitOfWork.History.GetDebitAccountHistory(AppSettings.DormId).ToList();
                        MessageBox.Show("Ok");
                    }
                    else
                    {
                        await this.ShowMessageAsync("...", "Что-то пошло не так...");
                    }
                }
                _floorForDebit = 0;
                comboFloorsForDebit.SelectedIndex = -1;
                btnDebitAccount.IsEnabled = false;
            }
        }

        private void UpdatePeopleCombobox()
        {
            bool floorAndRoomIsEmpty = ((_floor == 0) || (_roomNum == 0)) ? true : false;
            bool CorrectFloorRoom = (_roomNum.ToString().IndexOfAny(_floor.ToString().ToArray<char>()) == 0) ? true : false;

            if (!floorAndRoomIsEmpty && CorrectFloorRoom && (_roomTypeId != 0))
            {
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    comboPeople.ItemsSource = unitOfWork.GetPeopleNamesLivingInRoom(AppSettings.DormId, _roomNum, _roomTypeId).ToList();
                }
            }
            else if (!floorAndRoomIsEmpty && CorrectFloorRoom && (_roomTypeId == 0))
            {
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    comboPeople.ItemsSource = unitOfWork.GetPeopleNamesLivingInRoom(AppSettings.DormId, _roomNum).ToList();
                }
            }
        }

        private void FillFields(AccountData accountData)
        {
            checkBudget.IsChecked = accountData.IsBudget;
            checkStudent.IsChecked = accountData.IsStudent;
            txtAccountAmount.Text = accountData.AccountAmount.ToString();
            txtMonthPayment.Text = accountData.PricePerMonth.ToString();
        }

        private void ClearFields()
        {
            txtAccountAmount.Text = string.Empty;
            txtMonthPayment.Text = string.Empty;
            txtSumOfPayment.Text = string.Empty;
            comboFloors.SelectedIndex = -1;
            comboRooms.ItemsSource = new List<string>();
            comboPeople.ItemsSource = new List<string>();
            _floor = _roomNum = _roomTypeId = _personId = 0;
            comboRoomType.SelectedIndex = -1;
        }
    }
}
