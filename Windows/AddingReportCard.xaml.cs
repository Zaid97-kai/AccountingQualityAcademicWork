using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
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
using Excel = Microsoft.Office.Interop.Excel;

namespace AccountingQualityAcademicWork.Windows
{
    /// <summary>
    /// Логика взаимодействия для AddingReportCard.xaml
    /// </summary>
    public partial class AddingReportCard : Window
    {
        private MainWindow _mainWindow;
        private Models.Users _users;
        private OpenFileDialog _openFileDialog;
        private Models.ReportCard _reportCard;
        private int flag;
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
                    if (TbLabsCount.Text != "" && TbLectionCount.Text != "" && TbName.Text != "" && TbPracticalCount.Text != "" && CbGroup.SelectedItem != null && CbSpecialization.SelectedItem != null && CbTeacher.SelectedItem != null)
                    {
                        AddingNewReportCard();
                    }
                    else
                    {
                        MessageBox.Show("Не все поля заполнены");
                    }
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
        /// <summary>
        /// Добавление новой ведомости
        /// </summary>
        private void AddingNewReportCard()
        {
            _reportCard = new Models.ReportCard()
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
            Models.JournalDBEntities.GetContext().ReportCard.Add(_reportCard);
            Models.JournalDBEntities.GetContext().SaveChanges();

            foreach (var item in Models.JournalDBEntities.GetContext().Student.ToList())
            {
                if (item.Group == CbGroup.SelectedItem as Models.Group)
                {
                    Models.JournalDBEntities.GetContext().StudentInReportCard.Add(new Models.StudentInReportCard() { Student = item, Scores = 0, ReportCard = _reportCard });
                }
            }
            Models.JournalDBEntities.GetContext().SaveChanges();
        }

        private void BtnEditInfo_Click(object sender, RoutedEventArgs e)
        {
            Windows.FillingReportCardWindow fillingReportCardWindow = new Windows.FillingReportCardWindow((sender as Button).DataContext as Models.ReportCard, this);
            fillingReportCardWindow.ShowDialog();
        }

        private async void BnOpenReportCard_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog()
            {
                DefaultExt = "*.json",
                Filter = "файл JSON (File.json)|*.json",
                Title = "Выберите JSON файл"
            };
            if (!(ofd.ShowDialog() == true))
                return;

            using (FileStream fs = new FileStream(ofd.FileName, FileMode.Open))
            {
                var options = new JsonSerializerOptions
                {
                    ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve
                };

                Models.ReportCard reportCard = Models.JournalDBEntities.GetContext().ReportCard.Find((await JsonSerializer.DeserializeAsync<Models.ReportCard>(fs, options)).Id);

                Windows.FillingReportCardWindow fillingReportCardWindow = new Windows.FillingReportCardWindow(reportCard, this);
                fillingReportCardWindow.Show();
            }
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            _mainWindow.Show();
            this.Hide();
        }

        private void BtnDelete_Click(object sender, RoutedEventArgs e)
        {
            foreach (var item in Models.JournalDBEntities.GetContext().StudentInReportCard)
            {
                if (item.ReportCard == (sender as Button).DataContext as Models.ReportCard)
                {
                    Models.JournalDBEntities.GetContext().StudentInReportCard.Remove(item);
                }
            }
            Models.JournalDBEntities.GetContext().ReportCard.Remove((sender as Button).DataContext as Models.ReportCard);
            Models.JournalDBEntities.GetContext().SaveChanges();
            DgReportCards.ItemsSource = Models.JournalDBEntities.GetContext().ReportCard.ToList();
        }

        private void BnDeleteReportCard_Click(object sender, RoutedEventArgs e)
        {

        }
        private void BnOpenExcelReportCard_Click(object sender, RoutedEventArgs e)
        {
            if (flag == 0)
            {
                GridAddingForm.Visibility = Visibility.Visible;
                DgReportCards.Visibility = Visibility.Hidden;
                flag++;
                return;
            }
            if (flag == 1)
            {
                try
                {
                    _openFileDialog = new OpenFileDialog()
                    {
                        DefaultExt = "*.xls;*.xlsx",
                        Filter = "файл Excel (Spisok.xlsx)|*.xlsx",
                        Title = "Выберите файл базы данных"
                    };
                    if (!(_openFileDialog.ShowDialog() == true))
                        return;
                }
                catch
                {
                    BnOpenExcelReportCard.Background = Brushes.Red;
                }

                try
                {
                    string[,] list;
                    Excel.Application ObjWorkExcel = new Excel.Application();
                    Excel.Workbook ObjWorkBook = ObjWorkExcel.Workbooks.Open(_openFileDialog.FileName);
                    Excel.Worksheet ObjWorkSheet = (Excel.Worksheet)ObjWorkBook.Sheets[1];
                    var lastCell = ObjWorkSheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell);

                    int _columns = (int)lastCell.Column;
                    int _rows = (int)lastCell.Row;

                    list = new string[_rows, _columns];

                    for (int j = 0; j < _columns; j++)
                        for (int i = 0; i < _rows; i++)
                            list[i, j] = ObjWorkSheet.Cells[i + 1, j + 1].Text;
                    ObjWorkBook.Close(false, Type.Missing, Type.Missing);
                    ObjWorkExcel.Quit();
                    GC.Collect();

                    AddingNewReportCard();

                    for (int i = 1; i < _rows; i++)
                    {
                        Models.JournalDBEntities.GetContext().StudentInReportCard.Add(new Models.StudentInReportCard() { ReportCard = _reportCard, NumberMissedLabs = Convert.ToInt32(list[i, 5]), NumberMissedLectures = Convert.ToInt32(list[i, 3]), NumberMissedPractical = Convert.ToInt32(list[i, 4]), Scores = Convert.ToInt32(list[i, 2]), Student = Models.JournalDBEntities.GetContext().Student.AsEnumerable().Where(s => (s.Surname + " " + s.Name + " " + s.Patronymic) == list[i, 1]).FirstOrDefault() });
                    }
                    Models.JournalDBEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                finally
                {
                    GridAddingForm.Visibility = Visibility.Hidden;
                    DgReportCards.Visibility = Visibility.Visible;
                    flag--;
                    this._mainWindow.Show();
                    this.Hide();
                }
            }
        }

        private void BnBack_Click(object sender, RoutedEventArgs e)
        {
            GridAddingForm.Visibility = Visibility.Hidden;
            DgReportCards.Visibility = Visibility.Visible;
        }
    }
}
