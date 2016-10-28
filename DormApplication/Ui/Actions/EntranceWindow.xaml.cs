using System.Windows;
using System.Windows.Controls;
using DormApp.Domain.Interfaces;
using System.Text.RegularExpressions;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using Ninject;

namespace DormApplication.Ui.Actions
{
    /// <summary>
    /// Логика взаимодействия для EntranceWindow.xaml
    /// </summary>
    public partial class EntranceWindow : MetroWindow
    {
        private string _text = "Введите имя";

        public EntranceWindow()
        {
            InitializeComponent();
            txtName.Focus();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                comboDorm.ItemsSource = unitOfWork.Dormitories.GetDormNames();
            }
        }

        private void txtName_GotFocus(object sender, RoutedEventArgs e)
        {
            if (txtName.Text.Equals(_text))
            {
                txtName.Text = "";
            }
        }

        private void txtName_LostFocus(object sender, RoutedEventArgs e)
        {
            string name = ((TextBox)sender).Text;
            if (name.Equals(""))
            {
                txtName.AppendText(_text);
            }
            else if (!name.Equals(_text))
            {
                string new_name = string.Empty;
                string[] words = name.Split(' ');
                for (int i = 0; i < words.Length; i++)
                {
                    if (words[i].Length > 0)
                    {
                        words[i] = words[i].Substring(0, 1).ToUpper() + words[i].Remove(0, 1);
                    }
                }
                new_name = string.Join(" ", words);

                ((TextBox)sender).Select(((TextBox)sender).Text.Length, 0);
                ((TextBox)sender).Text = new_name;
            }
        }

        //Кнопка Войти
        private async void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            if (comboDorm.SelectedIndex == -1)
            {
                await this.ShowMessageAsync("Ошибка", "Выберите общежитие");
                //MessageBox.Show("Выберите общежитие");
            }
            else if (txtName.Text.Equals("") || (txtName.Text.Equals(_text)))
            {
                //MessageBox.Show("Введите имя");
                await this.ShowMessageAsync("Ошибка", "Введите имя");
            }
            else if (!Regex.IsMatch(txtName.Text, @"^[a-zA-Zа-яА-Я\\s]+"))
            {
                await this.ShowMessageAsync("Ошибка", "Вводите только буквы в имени");
                txtName.Focus();
            }
            else
            {
                AppSettings.Admin = txtName.Text.ToString();
                AppSettings.DormName = comboDorm.SelectedItem.ToString();
                MainWindow window = new MainWindow();
                window.Show();
                this.Close();
            }
        }

        //Кнопка Добавить
        private void btnAddDorm_Click(object sender, RoutedEventArgs e)
        {
            this.Hide();
            Window window = new AddDormWindow(this);
            window.Show();
        }
    }
}
