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
        public MainWindow()
        {
            InitializeComponent();
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
            Windows.AddingReportCard addingReportCard = new Windows.AddingReportCard(this);
            addingReportCard.Show();
            this.Hide();
        }

        private void BnAuth_Click(object sender, RoutedEventArgs e)
        {
            foreach (Models.Users item in Models.JournalDBEntities.GetContext().Users.ToList())
            {
                if (item.Log == TbLog.Text && item.Password == TbPassword.Text && item.IsAdmin == true)
                {
                    AuthGrid.Visibility = Visibility.Hidden;
                    MainGrid.Visibility = Visibility.Visible;
                    return;
                }
                else if (item.Log == TbLog.Text && item.Password == TbPassword.Text && item.IsAdmin == false)
                {
                    Windows.AddingReportCard addingReportCard = new Windows.AddingReportCard(this, item);
                    addingReportCard.Show();
                    this.Hide();
                    return;
                }
            }
            MessageBox.Show("Логин и пароль введены неверно");
        }
    }
}
