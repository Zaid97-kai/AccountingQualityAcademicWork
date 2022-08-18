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
    /// Логика взаимодействия для AddingStudentWindow.xaml
    /// </summary>
    public partial class AddingStudentWindow : Window
    {
        private MainWindow mainWindow;
        public AddingStudentWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            CbGroup.ItemsSource = Models.JournalDBEntities.GetContext().Group.ToList();
            this.mainWindow = mainWindow;
        }
        /// <summary>
        /// Добавление студента
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BnAddingStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Models.JournalDBEntities.GetContext().Student.ToList().Where(s => (s.Surname == TbSurname.Text) && (s.Name == TbName.Text) && (s.Patronymic == TbPatronymic.Text) && s.Group == CbGroup.SelectedItem as Models.Group).Count() == 0)
                {
                    Models.Student student = new Models.Student() { Name = TbName.Text, Surname = TbSurname.Text, Patronymic = TbPatronymic.Text, Group = CbGroup.SelectedItem as Models.Group };
                    Models.JournalDBEntities.GetContext().Student.Add(student);
                    Models.JournalDBEntities.GetContext().SaveChanges();

                    foreach (var item in Models.JournalDBEntities.GetContext().ReportCard.ToList())
                    {
                        if (item.Group.GroupNumber == (CbGroup.SelectedItem as Models.Group).GroupNumber)
                        {
                            Models.JournalDBEntities.GetContext().StudentInReportCard.Add(new Models.StudentInReportCard() { ReportCard = item, NumberMissedLabs = 0, NumberMissedLectures = 0, NumberMissedPractical = 0, Scores = 0, Student = student });
                        }
                    }
                    Models.JournalDBEntities.GetContext().SaveChanges();
                }
                else
                {
                    MessageBox.Show("Такой студент уже существует в базе данных");
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mainWindow.Show();
                this.Hide();
                MessageBox.Show("Студент добавлен");
            }
        

        }
    }
}
