using System;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.Controls.Dialogs;
using DormApp.Domain.Interfaces;
using Ninject;
using System.Diagnostics;

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

            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                comboFloor.ItemsSource = unitOfWork.Dormitories.GetLivingFloors(AppSettings.DormId);
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
                    Close();
                    break;
            }
            try
            {
                ReportsWindow w = new ReportsWindow((int)comboFloor.SelectedItem);
                w.Show();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
            }
        }

        private async void btnBack_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GenerateReportWindow generateWindow = new GenerateReportWindow();
                generateWindow.Show();
                Close();
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.ToString());
                await this.ShowMessageAsync("Ошибка", "Что-то произошло...");
            }
        }
    }
}
