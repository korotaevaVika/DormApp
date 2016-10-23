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
using DormApp.Domain;
using MahApps.Metro.Controls;

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
            this.Close();
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
                    unitOfWork.Dispose();
                }
                //_lstRooms = Data.GetLivingRooms(floor).ToList<int>();
                ((ComboBox)FindName(comboboxName)).ItemsSource = _lstRooms;
            }
            catch (Exception)
            {
                //MessageBox.Show("txtFloor_TextChanged " + ex.ToString());
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
                    //Data.GetPeopleLivingInRoom(_room).ToList<string>();
                    unitOfWork.Dispose();
                }
                ((ComboBox)FindName(comboboxPeopleName)).ItemsSource = _lstPeople;
            }
            catch (Exception)
            {// MessageBox.Show("comboRoom_SelectionChanged" + ex.ToString()); 
            }
        }

        private void comboPeople_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string[] fio;
            long? passport = new long();
            string contract = String.Empty;
            string room_type = String.Empty;
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


                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    unitOfWork.GetInformation(_person, out passport, out contract, out room_type, out sum);
                    unitOfWork.Dispose();
                }

                txtContract.Text = contract;
                txtDebt.Text = sum.ToString() + "p.";
                txtRoom.Text = _room.ToString();
                txtName.Text = _name;
                txtSurname.Text = _surname;
                txtRoomType.Text = room_type;
                txtPassport.Text = passport.ToString();
            }
            catch (Exception)
            {
                //MessageBox.Show(ex.ToString()); 
            }
        }

        private void btnOut_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    unitOfWork.MovePersonOut(
                        _person,
                        AppSettings.Admin,
                        AppSettings.DormId
                        );
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }
                lblProgress.Content = _surname + " " + _name + " " + "выселен(а)";
            }
            catch { lblProgress.Content = "Ошибка при совершении операции"; }
        }

        private void btnChange_Click(object sender, RoutedEventArgs e)
        {
            int new_floor, new_room, new_room_id;
            try
            {
                new_floor = Int32.Parse(txtNewFloor.Text);
                new_room = Int32.Parse(txtNewRoom.Text);
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
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
                    unitOfWork.Dispose();
                }
                MessageBox.Show(_surname + " " + _name + " " + "переехал(-а) из " + _room + " в " + new_room);
            }
            catch (Exception ex)
            { MessageBox.Show(ex.ToString()); }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                comboNewRoomType.ItemsSource = unitOfWork.RoomTypes.GetRoomTypeNames();
                unitOfWork.Dispose();
            }
        }
    }
}