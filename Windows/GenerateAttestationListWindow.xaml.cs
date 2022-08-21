using Microsoft.Win32;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Excel = Microsoft.Office.Interop.Excel;

namespace AccountingQualityAcademicWork.Windows
{
    /// <summary>
    /// Логика взаимодействия для GenerateAttestationListWindow.xaml
    /// </summary>
    public partial class GenerateAttestationListWindow : Window
    {
        private MainWindow _mainWindow;
        private Models.Group _group;
        private List<Models.Student> _students;
        private List<Models.StudentInReportCard> _studentInReportCards;
        private ObservableCollection<PartialClasses.GenerateAttestationListItem> _items;
        public GenerateAttestationListWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _students = new List<Models.Student>();
            _studentInReportCards = new List<Models.StudentInReportCard>();
            _items = new ObservableCollection<PartialClasses.GenerateAttestationListItem>();
            CbGroups.ItemsSource = Models.JournalDBEntities.GetContext().Group.ToList().OrderBy(g => g.GroupNumber);
        }

        private void BnGenerate_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog saveFileDialog = new SaveFileDialog();
            if (!(saveFileDialog.ShowDialog() == true))
                return;

            var app = new Excel.Application();
            app.SheetsInNewWorkbook = 1;
            Excel.Workbook workbook = app.Workbooks.Add(Type.Missing);

            int startRowIndex = 1;

            Excel.Worksheet worksheet = app.Worksheets.Item[1];
            worksheet.Name = _group.GroupNumber.ToString();

            worksheet.Cells[1][1] = "ФИО студента";
            for (int j = 2; j < _items[0].Disciplines.Count() + 2; j++)
            {
                worksheet.Cells[j][1] = _items[0].Disciplines[j - 2];
            }

            startRowIndex++;

            foreach (PartialClasses.GenerateAttestationListItem item in _items)
            {
                worksheet.Cells[1][startRowIndex] = item.Name;
                for (int i = 0; i < item.Scores.Count(); i++)
                {
                    worksheet.Cells[i + 2][startRowIndex] = item.Scores[i];
                }
                startRowIndex++;
            }


            Excel.Range rangeBorders = worksheet.Range[worksheet.Cells[1][1], worksheet.Cells[_items[0].Scores.Count() + 1][startRowIndex - 1]];

            rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeBottom].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeLeft].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeTop].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlEdgeRight].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideHorizontal].LineStyle =
                rangeBorders.Borders[Excel.XlBordersIndex.xlInsideVertical].LineStyle = Excel.XlLineStyle.xlDouble;

            worksheet.Columns.AutoFit();

            workbook.SaveAs(saveFileDialog.FileName + ".xlsx");
            app.Visible = true;
        }

        private void Window_Closed(object sender, EventArgs e)
        {
            this._mainWindow.Show();
            this.Hide();
        }

        private void CbGroups_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DgAttestationList.Columns.Clear();
            this._items.Clear();
            this._students.Clear();
            this._studentInReportCards.Clear();

            this._group = CbGroups.SelectedItem as Models.Group;
            this.Title = "Успеваемость группы " + this._group.GroupNumber;
            this._students = Models.JournalDBEntities.GetContext().Student.Where(s => s.Group.Id == _group.Id).ToList();

            foreach (var inReportCard in Models.JournalDBEntities.GetContext().StudentInReportCard)
            {
                foreach (var student in _students)
                {
                    if (inReportCard.Student.Id == student.Id)
                    {
                        _studentInReportCards.Add(inReportCard);
                    }
                }
            }

            var elements = (from r in _studentInReportCards
                            select new
                            {
                                FullName = r.Student.FullName,
                                Discipline = r.ReportCard.NameDiscipline,
                                Score = r.Scores
                            }).ToList().GroupBy(el => el.Discipline).ToList();

            DgAttestationList.Columns.Add(new DataGridTextColumn() { Header = "Ф.И.О.", Binding = new Binding("Name") });

            for (int i = 0; i < elements.Count(); i++)
            {
                DgAttestationList.Columns.Add(new DataGridTextColumn() { Header = elements[i].Key, Binding = new Binding("Scores[" + i + "]") });
            }

            foreach (var student in _students)
            {
                _items.Add(new PartialClasses.GenerateAttestationListItem() { Name = student.FullName });
                foreach (var element in elements)
                {
                    foreach (var item in element)
                    {
                        if (item.FullName == student.FullName)
                        {
                            _items[_items.Count - 1].Scores.Add(item.Score);
                        }
                    }
                }
            }

            for (int i = 0; i < _items.Count(); i++)
            {
                for (int j = 0; j < elements.Count(); j++)
                {
                    _items[i].Disciplines.Add(elements[j].Key);
                }
            }

            DgAttestationList.ItemsSource = _items;
        }
    }
}