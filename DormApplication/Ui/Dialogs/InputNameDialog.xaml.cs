using MahApps.Metro.Controls;
using System;
using System.Windows;

namespace DormApplication.Ui.Dialogs
{
    /// <summary>
    /// Логика взаимодействия для InputRoomTypeName.xaml
    /// </summary>
    public partial class InputNameDialog : MetroWindow
    {
        public InputNameDialog(string question)
        {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = "";
        }

        public InputNameDialog(string question, string answer)
        {
            InitializeComponent();
            lblQuestion.Content = question;
            txtAnswer.Text = answer;
        }

        private void btnDialogOk_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void Window_ContentRendered(object sender, EventArgs e)
        {
            txtAnswer.SelectAll();
            txtAnswer.Focus();
        }

        public string Answer
        {
            get { return txtAnswer.Text; }
        }
    }
}
