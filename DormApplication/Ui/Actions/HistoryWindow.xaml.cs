using DormApp.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using MahApps.Metro.Controls;
using DormApp.Domain.Interfaces;
using Ninject;

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
            Close();
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
                else e.Row.Background = Brushes.White;
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            using (IUnitOfWork unitOfWork = App.kernel.Get<IUnitOfWork>())
            {
                try
                {
                    gridHistory.ItemsSource = unitOfWork.History.GetHistory(AppSettings.DormId).ToList();
                    unitOfWork.Dispose();
                }
                catch (Exception ex)
                {
                    Debug.WriteLine(ex.ToString());
                }
            }
        }

        private void SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            try
            {
                foreach (DataGridRow Row in GetDataGridRows(gridHistory))
                {
                    gridHistoryPayment_LoadingRow(sender, new DataGridRowEventArgs(Row));
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nHistoryWindow -> SelectedColorChanged -> exception\n" + ex.ToString());
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
