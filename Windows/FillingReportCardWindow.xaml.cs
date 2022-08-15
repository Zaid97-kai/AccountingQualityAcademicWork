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

namespace AccountingQualityAcademicWork.Windows
{
    /// <summary>
    /// Логика взаимодействия для FillingReportCardWindow.xaml
    /// </summary>
    public partial class FillingReportCardWindow : Window
    {
        public FillingReportCardWindow(Models.ReportCard o)
        {
            InitializeComponent();
            DgStatement.ItemsSource = Models.JournalDBEntities.GetContext().StudentInReportCard.ToList().Where(s => s.IdReportCard == o.Id).ToList();
        }

        private void BnBack_Click(object sender, RoutedEventArgs e)
        {

        }

        private void BnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Models.JournalDBEntities.GetContext().SaveChanges();
        }

        private void BnSaveWord_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
