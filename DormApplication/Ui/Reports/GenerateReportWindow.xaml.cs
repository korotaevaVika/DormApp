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
            this.Close();
        }

        private void ListViewItem_Selected(object sender, RoutedEventArgs e)
        {
            string nameOfItem = ((ListViewItem)sender).Name.ToString();
            switch (nameOfItem)
            {
                case "DebtItem":
                    InputValuesForDebtReportWindow w = new InputValuesForDebtReportWindow();
                    w.Show();
                    this.Close();
                    break;
                case "PopulationItem":
                    PopulationWindow populationWindow = new PopulationWindow();
                    populationWindow.Show();
                    this.Close();
                    break;
                default:
                    break;
            }
        }
    }
}
