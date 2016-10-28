using DormApp.Domain;
using DormApp.Domain.Interfaces;
using DormApplication.Ui.Actions;
using MahApps.Metro.Controls;
using Ninject;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

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
            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                comboFloors.ItemsSource = unitOfWork.Dormitories.GetLivingFloors(AppSettings.DormId).ToList();
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
            Close();
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
                    using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                    {
                        _data = unitOfWork.GetPopulation(AppSettings.DormId).ToList();
                        gridPopulation.ItemsSource = _data;
                    }
                }
                catch { }
            }
            else if (comboFloors.SelectedIndex != -1)
            {
                _floor = (int)comboFloors.SelectedItem;
                try
                {
                    using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
                    {
                        _data = unitOfWork.GetPopulation(AppSettings.DormId, _floor).ToList();
                        gridPopulation.ItemsSource = _data;
                    }
                }
                catch { }
            }
        }
    }
}
