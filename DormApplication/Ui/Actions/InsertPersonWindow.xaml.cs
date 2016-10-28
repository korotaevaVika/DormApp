using DormApp.Domain.Interfaces;
using DormApp.Domain;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using Ninject;
using System.Diagnostics;

namespace DormApplication.Ui.Actions
{
    /// <summary>
    /// Логика взаимодействия для InsertPersonWindow.xaml
    /// </summary>
    public partial class InsertPersonWindow : MetroWindow
    {
        private List<KeyValuePair<string, string>> _text;

        public InsertPersonWindow()
        {
            InitializeComponent();
            _text = new List<KeyValuePair<string, string>>();
            dateStart.SelectedDate = DateTime.Today;
            dateFinish.SelectedDate = DateTime.Today;

            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                comboCountry.ItemsSource = unitOfWork.Countries.GetCountryNames();
                comboRoomType.ItemsSource = unitOfWork.RoomTypes.GetRoomTypeNames();
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
            foreach (KeyValuePair<string, string> pair in _text)
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

            //if (checkProperties(out count_mistakes) == 0)
            {
                checkProperties();
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
                        Floor = int.Parse(txtFloor.Text),
                        Room = int.Parse(txtRoom.Text),
                        RoomTypeName = comboRoomType.SelectedItem.ToString(),
                        DateStart = dateStart.SelectedDate.Value,
                        DateFinish = dateFinish.SelectedDate.Value
                    };

                    using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                    {
                        status = unitOfWork.InsertNewPerson(person, AppSettings.Admin, AppSettings.DormId);
                        unitOfWork.Complete();
                    }
                    lblProgress.Content = "Выполнено. " + status;
                }
                catch (Exception ex)
                {
                    lblProgress.Content = "Произошла ошибка...";
                    Debug.WriteLine(ex.ToString());
                }
            }
        }

        private async void checkProperties()
        {
            string msg = "";
            int count = 0;
            long res;
            int res_int;

            if (string.IsNullOrWhiteSpace(txtSurname.Text))
            {
                count++;
                msg += "Незаполнена фамилия \n";
            }
            if (string.IsNullOrWhiteSpace(txtName.Text))
            {
                count++;
                msg += "Незаполнено имя \n";
            }
            if (string.IsNullOrWhiteSpace(txtPassport.Text))
            {
                count++;
                msg += "Не введены паспортные данные \n";
            }
            if (!(long.TryParse(txtPassport.Text, out res)))
            {
                count++;
                msg += "Неверно введены паспортные данные \n";
            }

            if (string.IsNullOrWhiteSpace(txtFloor.Text))
            {
                count++;
                msg += "Не введен этаж \n";
            }
            else if (!(int.TryParse(txtFloor.Text, out res_int)))
            {
                count++;
                msg += "В номере этажа содержаться недопустимые символы \n";
            }

            if (string.IsNullOrWhiteSpace(txtRoom.Text))
            {
                count++;
                msg += "Не введен номер комнаты \n";
            }
            else if (!(int.TryParse(txtRoom.Text, out res_int)))
            {
                count++;
                msg += "В номере комнаты содержаться недопустимые символы \n";
            }

            int max_floor;
            using (IUnitOfWork unitOfWork = App.kernel.Get<UnitOfWork>())
            {
                max_floor = unitOfWork.Dormitories.GetMaximumFloor(AppSettings.DormId);
            }

            if ((int.TryParse(txtFloor.Text, out res_int)) && (Int32.Parse(txtFloor.Text) > max_floor))
            {
                count++;
                msg += "Такого этажа нет в данном корпусе \n";
            }

            if (count != 0)
            {
                await this.ShowMessageAsync((count + " ошибок"), msg);
            }
            //return count;
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
            Close();
        }
    }
}