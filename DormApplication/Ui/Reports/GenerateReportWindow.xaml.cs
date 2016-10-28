using System.Windows;
using System.Windows.Controls;
using DormApplication.Ui.Actions;
using MahApps.Metro.Controls;

namespace DormApplication.Ui.Reports
{
    /// <summary>
    /// Логика взаимодействия для GenerateReportWindow.xaml
    /// </summary>
    /// 
    public partial class GenerateReportWindow : MetroWindow
    {
        public GenerateReportWindow()
        {
            InitializeComponent();
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            Close();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            string nameOfItem = ((ListViewItem)sender).Name.ToString();
            switch (nameOfItem)
            {
                case "DebtItem":
                    InputValuesForDebtReportWindow w = new InputValuesForDebtReportWindow();
                    w.Show();
                    Close();
                    break;
                case "PopulationItem":
                    PopulationWindow populationWindow = new PopulationWindow();
                    populationWindow.Show();
                    Close();
                    break;
                default:
                    break;
            }
        }
    }
}
