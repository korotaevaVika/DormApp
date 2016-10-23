using CrystalDecisions.CrystalReports.Engine;
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
using DormApplication.Ui.Actions;
using DormApp.Domain;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;

namespace DormApplication.Ui.Reports
{
    /// <summary>
    /// Логика взаимодействия для InputValuesForDebtReportWindow.xaml
    /// </summary>
    public partial class InputValuesForDebtReportWindow : MetroWindow
    {
        public InputValuesForDebtReportWindow()
        {
            InitializeComponent();

            lblDormName.Content = AppSettings.DormName;

            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                comboFloor.ItemsSource = unitOfWork.Dormitories.GetLivingFloors(AppSettings.DormId);
                unitOfWork.Dispose();
            }
        }

        private void btnGenerate_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("\n\nСгенерировать еще один отчет такого типа?",
                "Повторная генерация", MessageBoxButton.YesNo);

            switch (result)
            {
                case MessageBoxResult.Yes:
                    break;
                case MessageBoxResult.No:
                    GenerateReportWindow grw = new GenerateReportWindow();
                    grw.Show();
                    this.Close();
                    break;
            }
            try
            {
                ReportsWindow w = new ReportsWindow((int)comboFloor.SelectedItem);
                w.Show();
            }
            catch { }
        }

        private async void btnBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GenerateReportWindow generateWindow = new GenerateReportWindow();
                generateWindow.Show();
                this.Close();
            }
            catch (Exception ex) { await this.ShowMessageAsync("Ошибка", ex.ToString()); }
        }
    }
}
