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
    /// Логика взаимодействия для StudentWindow.xaml
    /// </summary>
    public partial class StudentWindow : Window
    {
        private Windows.StudentsWindow _studentsWindow;
        private Models.Student _student;
        public StudentWindow(Windows.StudentsWindow studentsWindow, Models.Student student)
        {
            InitializeComponent();
            this._student = student;
            this._studentsWindow = studentsWindow;
            DgReportCard.ItemsSource = Models.JournalDBEntities.GetContext().StudentInReportCard.ToList().Where(s => s.Student == student);
            this.Title = this._student.FullName + " " + this._student.Group.GroupNumber;
        }

        private void BnBack_Click(object sender, RoutedEventArgs e)
        {
            _studentsWindow.Show();
            this.Hide();
        }
    }
}
