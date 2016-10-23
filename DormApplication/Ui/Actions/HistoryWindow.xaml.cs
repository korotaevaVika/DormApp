using DormApp.Domain;
using DormApp.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using Xceed.Wpf.Toolkit;
using System.Data;
using MahApps.Metro.Controls;

namespace DormApplication.Ui.Actions
{
    /// <summary>
    /// Логика взаимодействия для HistoryWindow.xaml
    /// </summary>
    public partial class HistoryWindow : MetroWindow
    {
        public HistoryWindow()
        {
            InitializeComponent();
            lblHeader.Content += AppSettings.DormName;
            colorInserted.SelectedColor = Color.FromRgb(180, 225, 133);
            colorChanged.SelectedColor = Color.FromRgb(255, 250, 150);
            colorAddPrice.SelectedColor = Color.FromRgb(150, 200, 250);
            colorAddPayment.SelectedColor = Color.FromRgb(230, 210, 240);
            colorMoveOut.SelectedColor = Color.FromRgb(247, 179, 179);
            colorDebitAccount.SelectedColor = Color.FromRgb(255, 140, 215);
        }

        private void btnBackToMenu_Click(object sender, RoutedEventArgs e)
        {
            MainWindow w = new MainWindow();
            w.Show();
            this.Close();
        }

        private void gridHistoryPayment_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            History item = e.Row.Item as History;
            if (item != null)
            {
                if (item.addPriceFlag == true)
                {
                    e.Row.Background = new SolidColorBrush(colorAddPrice.SelectedColor.Value);
                }
                else if (item.addPaymentFlag == true)
                {
                    e.Row.Background = new SolidColorBrush(colorAddPayment.SelectedColor.Value);
                }
                else if (item.insertFlag == true)
                {
                    e.Row.Background = new SolidColorBrush(colorInserted.SelectedColor.Value);
                }
                else if (item.changeRoomFlag == true)
                {
                    e.Row.Background = new SolidColorBrush(colorChanged.SelectedColor.Value);
                }
                else if (item.moveOutFlag == true)
                {
                    e.Row.Background = new SolidColorBrush(colorMoveOut.SelectedColor.Value);
                }
                else if (item.debitAccountFlag == true)
                {
                    e.Row.Background = new SolidColorBrush(colorDebitAccount.SelectedColor.Value);
                }
                else e.Row.Background = Brushes.Chocolate;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (var unitOfWork = new UnitOfWork(new DormApp.Entities.Dormitory_Entities()))
            {
                gridHistory.ItemsSource = unitOfWork.History.GetHistory(AppSettings.DormId);
                unitOfWork.Dispose();
            }
        }

        private void SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            //string colorPickerName = ((ColorPicker)sender).Name;
            //Dictionary<string, string> dictColorPickerBindingFlag = new Dictionary<string, string>();
            //dictColorPickerBindingFlag.Add("colorInserted", "insertedFlag");
            //dictColorPickerBindingFlag.Add("colorChange", "changeRoomFlag");
            //dictColorPickerBindingFlag.Add("colorMoveOut", "moveOutFlag");
            //dictColorPickerBindingFlag.Add("colorAddPayment", "addPaymentFlag");
            //dictColorPickerBindingFlag.Add("colorAddPrice", "addPriceFlag");
            //dictColorPickerBindingFlag.Add("colorDebitAccount", "debitAccountFlag");
            //string flagNeeded;
            //dictColorPickerBindingFlag.TryGetValue(colorPickerName, out flagNeeded);
            
            try
            {
                foreach (DataGridRow Row in GetDataGridRows(gridHistory))
                {
                    gridHistoryPayment_LoadingRow(sender, new DataGridRowEventArgs(Row));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\n\nHistoryWindow \n SelectedColorChanged \n" + ex.ToString() + "\n\n");
            }
        }
        
        private IEnumerable<DataGridRow> GetDataGridRows(DataGrid grid)
        {
            var itemsSource = grid.ItemsSource as System.Collections.IEnumerable;
            if (null == itemsSource) yield return null;
            foreach (var item in itemsSource)
            {
                var row = grid.ItemContainerGenerator.ContainerFromItem(item) as DataGridRow;
                if (null != row) yield return row;
            }
        }
    }
}
