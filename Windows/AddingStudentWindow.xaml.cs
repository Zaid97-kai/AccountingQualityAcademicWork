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

        private void BnAddingStudent_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.JournalDBEntities.GetContext().Student.Add(new Models.Student() { Name = TbName.Text, Surname = TbSurname.Text, Patronymic = TbPatronymic.Text, Group = CbGroup.SelectedItem as Models.Group });
                Models.JournalDBEntities.GetContext().SaveChanges();
            }
            catch(Exception ex)
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
