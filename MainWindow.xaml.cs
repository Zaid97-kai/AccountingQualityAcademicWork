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
    }
}
