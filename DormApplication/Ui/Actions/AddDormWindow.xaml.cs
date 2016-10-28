using System.Windows;
using MahApps.Metro.Controls;
using DormApp.Domain.Interfaces;
using Ninject;

namespace DormApplication.Ui.Actions
{
    /// <summary>
    /// Логика взаимодействия для AddDormWindow.xaml
    /// </summary>
    public partial class AddDormWindow : MetroWindow
    {
        private EntranceWindow _caller;

        public AddDormWindow(EntranceWindow caller)
        {
            _caller = caller;
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    unitOfWork.Dormitories.Add(new DormApp.Entities.DormType
                    {
                        name = txtDormName.Text,
                        address = txtDormAddress.Text,
                        floor_number = int.Parse(txtDormFloorNum.Text)
                    });
                    unitOfWork.Complete();
                }
                Close();
                _caller.Show();
                using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                {
                    _caller.comboDorm.ItemsSource = unitOfWork.Dormitories.GetDormNames();
                }
            }
            catch
            {
                Close();
                _caller.Show();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _caller.Show();
        }
    }
}
