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
    /// Логика взаимодействия для AddingTeacherWindow.xaml
    /// </summary>
    public partial class AddingTeacherWindow : Window
    {

        private MainWindow mainWindow;
        public AddingTeacherWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            this.mainWindow = mainWindow;
        }

        private void BnAddingTeacher_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Models.JournalDBEntities.GetContext().Users.Add(new Models.Users() { Name = TbName.Text, Surname = TbSurname.Text, Patronymic = TbPatronymic.Text, Log = TbLog.Text, Password = TbPass.Text });
                Models.JournalDBEntities.GetContext().SaveChanges();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            finally
            {
                mainWindow.Show();
                this.Hide();
                MessageBox.Show("Преподаватель добавлен");
            }
        }
    }
}
