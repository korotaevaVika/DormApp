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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using DormApp.Domain;
using DormApp.Reports;
using MahApps.Metro.Controls;

namespace DormApplication.Ui.Reports
{
    /// <summary>
    /// Логика взаимодействия для ReportsWindow.xaml
    /// ReportsWindow.xaml - это окно для вывода должников по оплате за общежитие
    /// </summary>
    public partial class ReportsWindow : MetroWindow
    {
        private DebtReport _rp;
        public ReportsWindow()
        {
            InitializeComponent();
            _rp = new DebtReport();
            RpViewer.ViewerCore.ReportSource = _rp;
        }

        public ReportsWindow(int floor)
        {
            InitializeComponent();
            _rp = new DebtReport();

            _rp.SetParameterValue(
                _rp.Parameter_dorm_id.ParameterFieldName, AppSettings.DormId
                );
            _rp.SetParameterValue(_rp.Parameter_floor.ParameterFieldName, floor);
            RpViewer.ViewerCore.ReportSource = _rp;
        }
    }
}
