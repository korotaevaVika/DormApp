using DormApp.Domain;
using DormApplication.Ui.Actions;
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

namespace DormApplication.Ui.Reports
{
    /// <summary>
    /// Логика взаимодействия для PopulationWindow.xaml
    /// </summary>
    public partial class PopulationWindow : MetroWindow
    {
        private List<Population> _data;
        public PopulationWindow()
        {
            InitializeComponent();
            _data = new List<Population>();
            lblHeader.Content += AppSettings.DormName;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                comboFloors.ItemsSource = unitOfWork.Dormitories.GetLivingFloors(AppSettings.DormId).ToList();
                unitOfWork.Dispose();
            }
            comboFloors.SelectedIndex = -1;
        }

        private void btnShowGrid_Click(object sender, RoutedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            this.Close();
        }

        private void comboFloors_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateDataGrid();
        }

        private void UpdateDataGrid()
        {
            int _floor;
            if (checkChooseAll.IsChecked == true)
            {
                try
                {
                    using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                    {
                        _data = unitOfWork.GetPopulation(AppSettings.DormId).ToList();
                        gridPopulation.ItemsSource = _data;
                        unitOfWork.Dispose();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
            else if (comboFloors.SelectedIndex != -1)
            {
                _floor = (int)comboFloors.SelectedItem;
                try
                {
                    using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
                    {
                        _data = unitOfWork.GetPopulation(AppSettings.DormId, _floor).ToList();
                        gridPopulation.ItemsSource = _data;
                        unitOfWork.Dispose();
                    }
                }
                catch (Exception ex) { MessageBox.Show(ex.ToString()); }
            }
        }
    }
}
