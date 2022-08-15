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

namespace AccountingQualityAcademicWork.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddingReportCard.xaml
    /// </summary>
    public partial class AddingReportCard : Window
    {
        private MainWindow _mainWindow;
        private Models.Users _users;
        public AddingReportCard(MainWindow mainWindow, Models.Users users = null)
        {
            InitializeComponent();
            CbGroup.ItemsSource = Models.JournalDBEntities.GetContext().Group.ToList();
            CbSpecialization.ItemsSource = Models.JournalDBEntities.GetContext().Specialization.ToList();
            CbTeacher.ItemsSource = Models.JournalDBEntities.GetContext().Users.ToList();
            GridAddingForm.Visibility = Visibility.Hidden;
            this._mainWindow = mainWindow;
            this._users = users;
            UpdateReportCardsTable();
        }

        private void UpdateReportCardsTable()
        {
            if (!_users.IsAdmin)
            {
                DgReportCards.ItemsSource = Models.JournalDBEntities.GetContext().ReportCard.ToList().Where(r => r.Users.Id == _users.Id).ToList();
                for (int i = 0; i < Models.JournalDBEntities.GetContext().Users.ToList().Count; i++)
                {
                    if (Models.JournalDBEntities.GetContext().Users.ToList()[i].Id == _users.Id)
                    {
                        CbTeacher.SelectedIndex = i;
                    }
                }
                CbTeacher.IsEnabled = false;
            }
            else
            {
                DgReportCards.ItemsSource = Models.JournalDBEntities.GetContext().ReportCard.ToList();
            }
        }

        private void BnAddingReportCard_Click(object sender, RoutedEventArgs e)
        {
            if (GridAddingForm.Visibility == Visibility.Hidden)
            {
                GridAddingForm.Visibility = Visibility.Visible;
                DgReportCards.Visibility = Visibility.Hidden;
            }
            else
            {
                try
                {
                    Models.ReportCard reportCard = new Models.ReportCard()
                    {
                        Users = CbTeacher.SelectedItem as Models.Users,
                        DateFilling = DateTime.Now,
                        Group = CbGroup.SelectedItem as Models.Group,
                        NameDiscipline = TbName.Text,
                        NumberLabs = Convert.ToInt32(TbLabsCount.Text),
                        NumberLectures = Convert.ToInt32(TbLectionCount.Text),
                        NumberPractical = Convert.ToInt32(TbPracticalCount.Text),
                        Specialization = CbSpecialization.SelectedItem as Models.Specialization
                    };
                    Models.JournalDBEntities.GetContext().ReportCard.Add(reportCard);
                    Models.JournalDBEntities.GetContext().SaveChanges();

                    foreach (var item in Models.JournalDBEntities.GetContext().Student.ToList())
                    {
                        if(item.Group == CbGroup.SelectedItem as Models.Group)
                        {
                            Models.JournalDBEntities.GetContext().StudentInReportCard.Add(new Models.StudentInReportCard() { Student = item, Scores = 0, ReportCard = reportCard });
                        }
                    }
                    Models.JournalDBEntities.GetContext().SaveChanges();

                    GridAddingForm.Visibility = Visibility.Hidden;
                    DgReportCards.Visibility = Visibility.Visible;
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    MessageBox.Show("Табель успешно добавлена");
                    UpdateReportCardsTable();
                }
            }
        }

        private void BtnEditInfo_Click(object sender, RoutedEventArgs e)
        {
            Windows.FillingReportCardWindow fillingReportCardWindow = new Windows.FillingReportCardWindow((sender as Button).DataContext as Models.ReportCard, this);
            fillingReportCardWindow.ShowDialog();
        }
    }
}
