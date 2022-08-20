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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AccountingQualityAcademicWork
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Models.Users _users;
        public MainWindow()
        {
            InitializeComponent();
            TbLog.Text = "user01";
            TbPassword.Text = "user01";
            MainGrid.Visibility = Visibility.Hidden;
        }

        private void BnEnter_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddingStudentWindow addingStudentWindow = new Windows.AddingStudentWindow(this);
            addingStudentWindow.Show();
            this.Hide();
        }

        private void BnEnterMenu_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddingReportCard addingReportCard = new Windows.AddingReportCard(this, _users);
            addingReportCard.Show();
            this.Hide();
        }
        /// <summary>
        /// Авторизация пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BnAuth_Click(object sender, RoutedEventArgs e)
        {
            foreach (Models.Users item in Models.JournalDBEntities.GetContext().Users.ToList())
            {
                if (item.Log == TbLog.Text && item.Password == TbPassword.Text && item.IsAdmin == true)
                {
                    _users = item;
                    AuthGrid.Visibility = Visibility.Hidden;
                    MainGrid.Visibility = Visibility.Visible;
                    return;
                }
                else if (item.Log == TbLog.Text && item.Password == TbPassword.Text && item.IsAdmin == false)
                {
                    Windows.AddingReportCard addingReportCard = new Windows.AddingReportCard(this, item);
                    TbLog.Text = "";
                    TbPassword.Text = "";
                    addingReportCard.Show();
                    this.Hide();
                    return;
                }
            }
            MessageBox.Show("Логин и пароль введены неверно");
        }
        /// <summary>
        /// Выход из учетной записи администратора
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BnExit_Click(object sender, RoutedEventArgs e)
        {
            TbLog.Text = "";
            TbPassword.Text = "";
            AuthGrid.Visibility = Visibility.Visible;
            MainGrid.Visibility = Visibility.Hidden;
        }

        private void BnEnterExcel_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddingStudentFromExcelWindow addingStudentFromExcelWindow = new Windows.AddingStudentFromExcelWindow(this);
            addingStudentFromExcelWindow.Show();
            this.Hide();
        }

        private void BnOpenStudentsList_Click(object sender, RoutedEventArgs e)
        {
            Windows.StudentsWindow studentsWindow = new Windows.StudentsWindow(this);
            studentsWindow.Show();
            this.Hide();
        }

        private void BnOpenAddingGroupWindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.AddingGroupWindow addingGroupWindow = new Windows.AddingGroupWindow(this);
            addingGroupWindow.Show();
            this.Hide();
        }

        private void BnGenerateAttestationListWindow_Click(object sender, RoutedEventArgs e)
        {
            Windows.GenerateAttestationListWindow generateAttestationListWindow = new Windows.GenerateAttestationListWindow(this);
            generateAttestationListWindow.Show();
            this.Hide();
        }
    }
}
