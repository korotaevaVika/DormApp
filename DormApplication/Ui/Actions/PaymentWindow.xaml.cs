using DormApp.Domain;
using MahApps.Metro.Controls;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                comboFloors.ItemsSource = comboFloorsForDebit.ItemsSource = unitOfWork.Dormitories.GetLivingFloors(AppSettings.DormId);
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

        private void comboRoomType_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try {
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    _roomTypeId = unitOfWork.RoomTypes.GetRoomTypeId((comboRoomType.SelectedItem.ToString()));
                    unitOfWork.Dispose();
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
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    comboRooms.ItemsSource = unitOfWork.GetLivingRooms(_floor, AppSettings.DormId).ToList();
                    unitOfWork.Dispose();
                }
            }
            catch(Exception ex) { Debug.WriteLine(ex.ToString()); }
        }

        private void comboRooms_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                int.TryParse(comboRooms.SelectedItem.ToString(), out _roomNum);
                UpdatePeopleCombobox();
            }
            catch (Exception) { }
        }

        private void btnPay_Click(object sender, RoutedEventArgs e)
        {
            decimal sum;
            if ((_personId == 0) || !(Decimal.TryParse(txtSumOfPayment.Text, out sum)))
            {
                MessageBox.Show("Ошибка ввода данных");
            }
            else
            {
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    MessageBox.Show(unitOfWork.AddPayment(_personId, sum, dateOfPayment.SelectedDate.Value.Date, 
                        AppSettings.Admin, AppSettings.DormId));
                    ClearFields();
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
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
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
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
                    unitOfWork.Dispose();
                }  
            }
            catch{}
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                gridHistoryPayment.ItemsSource = unitOfWork.History.GetDebitAccountHistory(AppSettings.DormId).ToList();
                unitOfWork.Dispose();
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

        private void btnDebitAccount_Click(object sender, RoutedEventArgs e)
        {
            if (_floorForDebit != 0)
            {
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    if (unitOfWork.DebitAccount(AppSettings.DormId, _floorForDebit, AppSettings.Admin))
                    {
                        unitOfWork.Complete();
                        gridHistoryPayment.ItemsSource = unitOfWork.History.GetDebitAccountHistory(AppSettings.DormId).ToList();
                        MessageBox.Show("Ok");
                    }
                    else {
                        MessageBox.Show("Something went wrong...");
                    }
                    unitOfWork.Dispose();
                }
                _floorForDebit = 0;
                comboFloorsForDebit.SelectedIndex = -1;
                btnDebitAccount.IsEnabled = false;
            }
        }

        private void UpdatePeopleCombobox()
        {
            bool floorAndRoomIsEmpty = ((_floor == 0) || (_roomNum == 0)) ? true: false;
            bool CorrectFloorRoom = (_roomNum.ToString().IndexOfAny(_floor.ToString().ToArray<char>()) == 0) ? true : false;
            
            if ( !floorAndRoomIsEmpty && CorrectFloorRoom && (_roomTypeId != 0))
            {
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    comboPeople.ItemsSource = unitOfWork.GetPeopleNamesLivingInRoom(AppSettings.DormId, _roomNum, _roomTypeId).ToList();
                    unitOfWork.Dispose();
                }
            }
            else if (!floorAndRoomIsEmpty && CorrectFloorRoom && (_roomTypeId == 0))
            {
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    comboPeople.ItemsSource = unitOfWork.GetPeopleNamesLivingInRoom(AppSettings.DormId, _roomNum).ToList();
                    unitOfWork.Dispose();
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
