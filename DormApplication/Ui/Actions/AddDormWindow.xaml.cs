using DormApp.Domain;
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
using MahApps.Metro.Controls;

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
            this._caller = caller;
            InitializeComponent();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    unitOfWork.Dormitories.Add(new DormApp.Entities.DormType
                    {
                        name = txtDormName.Text,
                        address = txtDormAddress.Text,
                        floor_number = Int32.Parse(txtDormFloorNum.Text)
                    });
                    unitOfWork.Complete();
                    unitOfWork.Dispose();
                }

                this.Close();
                _caller.Show();
                using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                {
                    _caller.comboDorm.ItemsSource = unitOfWork.Dormitories.GetDormNames();
                    unitOfWork.Dispose();
                }
            }
            catch
            {
                this.Close();
                _caller.Show();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _caller.Show();
        }
    }
}
