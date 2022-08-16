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
    /// Логика взаимодействия для StudentsWindow.xaml
    /// </summary>
    public partial class StudentsWindow : Window
    {
        private MainWindow _mainWindow;
        private List<Models.Student> _students;
        public StudentsWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this._students = Models.JournalDBEntities.GetContext().Student.ToList();
            DgStudents.ItemsSource = this._students;
            CbNumberGroup.ItemsSource = Models.JournalDBEntities.GetContext().Group.ToList();
            _mainWindow = mainWindow;
        }

        private void BnBack_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.Show();
            this.Hide();
        }

        private void BtnStudentInfo_Click(object sender, RoutedEventArgs e)
        {
            StudentWindow studentWindow = new StudentWindow(this, (sender as Button).DataContext as Models.Student);
            studentWindow.Show();
            this.Hide();
        }

        private void RefreshList()
        {
            this._students = Models.JournalDBEntities.GetContext().Student.ToList();

            if (CbNumberGroup.SelectedItem != null)
            {
                _students = (from s in _students
                          where s.Group.GroupNumber == (CbNumberGroup.SelectedItem as Models.Group).GroupNumber
                          select s).ToList();
            }

            if (TbNameStudent.Text != "")
            {
                _students = _students.OrderBy(s => s.FullName).Where(s => s.FullName.Contains(TbNameStudent.Text)).ToList();
            }

            DgStudents.ItemsSource = _students;
        }

        private void TbNameStudent_TextChanged(object sender, TextChangedEventArgs e)
        {
            RefreshList();
        }

        private void CbNumberGroup_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefreshList();
        }
    }
}
