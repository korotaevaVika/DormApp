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
