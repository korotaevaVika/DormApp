using DormApp.Domain;
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
    /// Логика взаимодействия для InsertPersonWindow.xaml
    /// </summary>
    public partial class InsertPersonWindow : MetroWindow
    {
        private List<KeyValuePair<String, String>> _text;

        public InsertPersonWindow()
        {
            InitializeComponent();
            _text = new List<KeyValuePair<string, string>>();
            dateStart.SelectedDate = DateTime.Today;
            dateFinish.SelectedDate = DateTime.Today;

            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                comboCountry.ItemsSource = unitOfWork.Countries.GetCountryNames();
                comboRoomType.ItemsSource = unitOfWork.RoomTypes.GetRoomTypeNames();
                unitOfWork.Dispose();
            }

            txtDorm.Text += "\t" + AppSettings.DormName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _text.Add(new KeyValuePair<string, string>(txtSurname.Name, txtSurname.Text));
            _text.Add(new KeyValuePair<string, string>(txtName.Name, txtName.Text));
            _text.Add(new KeyValuePair<string, string>(txtSecondName.Name, txtSecondName.Text));
            _text.Add(new KeyValuePair<string, string>(txtPassport.Name, txtPassport.Text));
        }

        private void txt_GotFocus(object sender, RoutedEventArgs e)
        {
            string name = ((TextBox)sender).Name;
            foreach (KeyValuePair<String, String> pair in _text)
            {
                if (pair.Key.Equals(name) && (((TextBox)sender).Text.Equals(pair.Value)))
                {
                    ((TextBox)sender).Text = "";
                    break;
                }
            }
        }

        private void txt_LostFocus(object sender, RoutedEventArgs e)
        {
            string name = ((TextBox)sender).Name;

            foreach (KeyValuePair<String, String> pair in _text)
            {
                if (pair.Key.Equals(name) && (((TextBox)sender).Text.Equals("")))
                {
                    ((TextBox)sender).Text = pair.Value;
                    break;
                }
            }
        }

        private void gender_Checked(object sender, RoutedEventArgs e)
        {
            if (((CheckBox)sender).Name.EndsWith("Male"))
            {
                checkFemale.IsChecked = false;
            }
            else if (((CheckBox)sender).Name.EndsWith("Female"))
            {
                checkMale.IsChecked = false;
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

        private void btnInsert_Click(object sender, RoutedEventArgs e)
        {
            stackPanel.Visibility = Visibility.Visible;

            if (checkProperties() == 0)
            {
                string status;

                bool is_male = false;
                if (checkMale.IsChecked == true)
                {
                    is_male = true;
                }

                bool is_student = (checkStudent.IsChecked == true) ? true : false;
                bool on_budget = (is_student == true) && checkBudget.IsChecked.HasValue ?
                    checkBudget.IsChecked.Value : false;

                try
                {
                    NewPerson person = new NewPerson
                    {
                        SurName = txtSurname.Text,
                        Name = txtName.Text,
                        LastName = txtSecondName.Text,
                        Passport = Convert.ToInt64(txtPassport.Text),
                        IsMale = is_male,
                        IsStudent = is_student,
                        IsBudget = on_budget,
                        Country = comboCountry.SelectedItem.ToString(),
                        Contract = txtContract.Text,
                        Floor = Int32.Parse(txtFloor.Text),
                        Room = Int32.Parse(txtRoom.Text),
                        RoomTypeName = comboRoomType.SelectedItem.ToString(),
                        DateStart = dateStart.SelectedDate.Value,
                        DateFinish = dateFinish.SelectedDate.Value
                    };

                    using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                    {
                        status = unitOfWork.InsertNewPerson(person, AppSettings.Admin, AppSettings.DormId);
                        unitOfWork.Complete();
                        unitOfWork.Dispose();
                    }

                    lblProgress.Content = "Выполнено. " + status;
                }
                catch (Exception ex)
                {
                    lblProgress.Content = ex.ToString();
                }
            }
        }

        private int checkProperties()
        {
            string msg = "";
            int count = 0;
            long res;
            int res_int;

            if (String.IsNullOrWhiteSpace(txtSurname.Text))
            {
                count++;
                msg += "Незаполнена фамилия \n";
            }
            if (String.IsNullOrWhiteSpace(txtName.Text))
            {
                count++;
                msg += "Незаполнено имя \n";
            }
            if (String.IsNullOrWhiteSpace(txtPassport.Text))
            {
                count++;
                msg += "Не введены паспортные данные \n";
            }
            if (!(Int64.TryParse(txtPassport.Text, out res)))
            {
                count++;
                msg += "Неверно введены паспортные данные \n";
            }

            if (String.IsNullOrWhiteSpace(txtFloor.Text))
            {
                count++;
                msg += "Не введен этаж \n";
            }
            else if (!(Int32.TryParse(txtFloor.Text, out res_int)))
            {
                count++;
                msg += "В номере этажа содержаться недопустимые символы \n";
            }

            if (String.IsNullOrWhiteSpace(txtRoom.Text))
            {
                count++;
                msg += "Не введен номер комнаты \n";
            }
            else if (!(Int32.TryParse(txtRoom.Text, out res_int)))
            {
                count++;
                msg += "В номере комнаты содержаться недопустимые символы \n";
            }

            int max_floor;
            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                max_floor = unitOfWork.Dormitories.GetMaximumFloor(AppSettings.DormId);
                unitOfWork.Dispose();
            }

            if ((Int32.TryParse(txtFloor.Text, out res_int)) && (Int32.Parse(txtFloor.Text) > max_floor))
            {
                count++;
                msg += "Такого этажа нет в данном корпусе \n";
            }

            if (count != 0)
            {
                MessageBox.Show("Количество, найденных ошибок: " + count + "\n \n" + msg);
            }
            return count;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            txtSurname.Text = "";
            txtName.Text = "";
            txtSecondName.Text = "";
            txtPassport.Text = "Паспорт";
            txtContract.Text = "";
            txtFloor.Text = "";
            txtRoom.Text = "";
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            this.Close();
        }
    }
}