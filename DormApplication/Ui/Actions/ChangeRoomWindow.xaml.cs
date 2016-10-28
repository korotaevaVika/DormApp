using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using DormApp.Domain;
using MahApps.Metro.Controls;
using System.Diagnostics;
using DormApp.Domain.Interfaces;
using Ninject;
using MahApps.Metro.Controls.Dialogs;

namespace DormApplication.Ui.Actions
{
    /// <summary>
    /// Логика взаимодействия для ChangeRoomWindow.xaml
    /// </summary>
    public partial class ChangeRoomWindow : MetroWindow
    {
        private List<int> _lstRooms;
        private List<string> _lstPeople;
        private int _room;
        private string _surname;
        private string _name;
        private PersonData _person;

        public ChangeRoomWindow()
        {
            InitializeComponent();
            lblHeader.Content += AppSettings.DormName;
            _lstRooms = new List<int>();
            _lstPeople = new List<string>();
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            Close();
        }

        private void txtFloor_TextChanged(object sender, TextChangedEventArgs e)
        {
            string name = ((TextBox)sender).Name.ToString();
            string tab = name.Substring(name.IndexOf('_'));
            string comboboxName = "comboRoom" + tab;

            try
            {
                int floor = int.Parse(((TextBox)FindName(name)).Text);

                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    _lstRooms = unitOfWork.GetLivingRooms(floor, AppSettings.DormId).ToList();
                }
                ((ComboBox)FindName(comboboxName)).ItemsSource = _lstRooms;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void comboRoom_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string name = ((ComboBox)sender).Name.ToString();
            string tab = name.Substring(name.IndexOf('_'));
            string comboboxPeopleName = "comboPeople" + tab;
            try
            {
                _room = int.Parse(((ComboBox)FindName(name)).SelectedItem.ToString());
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    _lstPeople = unitOfWork.GetPeopleNamesLivingInRoom(AppSettings.DormId, _room).ToList();
                }
                ((ComboBox)FindName(comboboxPeopleName)).ItemsSource = _lstPeople;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void comboPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] fio;
            long? passport = new long();
            string contract = string.Empty;
            string room_type = string.Empty;
            decimal sum = new decimal();

            try
            {
                fio = ((ComboBox)sender).SelectedItem.ToString().Split(' ');
                _surname = fio[0];
                _name = fio[1];
                _person = new PersonData
                {
                    Name = _name,
                    SurName = _surname,
                    Room = _room,
                    DormID = AppSettings.DormId
                };
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    unitOfWork.GetInformation(_person, out passport, out contract, out room_type, out sum);
                }
                txtContract.Text = contract;
                txtDebt.Text = sum.ToString() + "p.";
                txtRoom.Text = _room.ToString();
                txtName.Text = _name;
                txtSurname.Text = _surname;
                txtRoomType.Text = room_type;
                txtPassport.Text = passport.ToString();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    unitOfWork.MovePersonOut(
                        _person,
                        AppSettings.Admin,
                        AppSettings.DormId
                        );
                    unitOfWork.Complete();
                }
                lblProgress.Content = _surname + " " + _name + " " + "выселен(а)";
            }
            catch (Exception ex)
            {
                lblProgress.Content = "Ошибка при совершении операции";
                Debug.WriteLine(ex.ToString());
            }
        }

        private async void btnChange_Click(object sender, RoutedEventArgs e)
        {
            int new_floor, new_room, new_room_id;
            try
            {
                new_floor = int.Parse(txtNewFloor.Text);
                new_room = int.Parse(txtNewRoom.Text);
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    new_room_id = unitOfWork.RoomTypes.GetRoomTypeId(
                        comboNewRoomType.SelectedItem.ToString()
                        );
                    NewPlace place = new NewPlace
                    {
                        NewFloor = new_floor,
                        NewRoom = new_room,
                        NewRoomTypeId = new_room_id
                    };
                    unitOfWork.ChangeRoom(
                        _person,
                        place,
                        AppSettings.Admin,
                        AppSettings.DormId
                        );
                    unitOfWork.Complete();
                }
                await this.ShowMessageAsync("OK", _surname + " " + _name + " " + "переехал(-а) из " + _room + " в " + new_room);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                comboNewRoomType.ItemsSource = unitOfWork.RoomTypes.GetRoomTypeNames();
            }
        }
    }
}