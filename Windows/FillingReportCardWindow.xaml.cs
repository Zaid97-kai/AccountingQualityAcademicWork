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
using Word = Microsoft.Office.Interop.Word;

namespace AccountingQualityAcademicWork.Windows
{
    /// <summary>
    /// Логика взаимодействия для FillingReportCardWindow.xaml
    /// </summary>
    public partial class FillingReportCardWindow : Window
    {
        Windows.AddingReportCard addingReport;
        Models.ReportCard reportCard;
        List<Models.StudentInReportCard> studentInReportsCard;
        public FillingReportCardWindow(Models.ReportCard o, Windows.AddingReportCard addingReport)
        {
            InitializeComponent();
            this.reportCard = o;
            this.studentInReportsCard = Models.JournalDBEntities.GetContext().StudentInReportCard.ToList().Where(s => s.IdReportCard == o.Id).ToList();
            DgStatement.ItemsSource = this.studentInReportsCard;
            this.addingReport = addingReport;
        }

        private void BnBack_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Сохранить ведомость перед выходом", "Сообщение", MessageBoxButton.YesNo) == MessageBoxResult.Yes)
            {
                Models.JournalDBEntities.GetContext().SaveChanges();
            }
            addingReport.Show();
            this.Hide();
        }

        private void BnSaveChanges_Click(object sender, RoutedEventArgs e)
        {
            Models.JournalDBEntities.GetContext().SaveChanges();
        }
        /// <summary>
        /// Генерация WORD документа
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void BnSaveWord_Click(object sender, RoutedEventArgs e)
        {
            var app = new Word.Application();
            Word.Document document = app.Documents.Add();

            Word.Paragraph paragraph = document.Paragraphs.Add();
            Word.Range range = paragraph.Range;
            range.Font.Size = 16;
            range.Font.Name = "Times New Roman";
            range.Text = "ОТДЕЛЕНИЕ СПО ИКТЗИ";
            range.Bold = 1;
            range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            range = paragraph.Range;
            range.Font.Size = 16;
            range.Font.Name = "Times New Roman";
            range.Text = "ВЕДОМОСТЬ УЧЕТА КАЧЕСТВА УЧЕБНОЙ РАБОТЫ";
            range.Bold = 1;
            range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
            range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            range = paragraph.Range;
            range.Font.Size = 14;
            range.Font.Name = "Times New Roman";
            range.Text = "Учебная дисциплина: " + reportCard.NameDiscipline;
            range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            range = paragraph.Range;
            range.Font.Size = 14;
            range.Font.Name = "Times New Roman";
            range.Text = "Дата заполнения: " + reportCard.DateFilling.ToString();
            range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            range = paragraph.Range;
            range.Font.Size = 14;
            range.Font.Name = "Times New Roman";
            range.Text = "Преподаватель: " + reportCard.Users.FullName;
            range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            range.InsertParagraphAfter();

            paragraph = document.Paragraphs.Add();
            range = paragraph.Range;
            range.Font.Size = 14;
            range.Font.Name = "Times New Roman";
            range.Text = "Группа (специальность): " + reportCard.Group.GroupNumber.ToString() + " (" + reportCard.Specialization.Code + ")";
            range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphLeft;
            range.InsertParagraphAfter();


            Word.Paragraph tableParagraph = document.Paragraphs.Add();
            Word.Range tableRange = tableParagraph.Range;
            Word.Table studentsTable = document.Tables.Add(tableRange, this.studentInReportsCard.Count() + 1, 6);
            studentsTable.Borders.InsideLineStyle =
                studentsTable.Borders.OutsideLineStyle = Word.WdLineStyle.wdLineStyleSingle;
            studentsTable.Range.Cells.VerticalAlignment =
                Word.WdCellVerticalAlignment.wdCellAlignVerticalCenter;

            Word.Range cellRange;
            cellRange = studentsTable.Cell(1, 1).Range;
            cellRange.Font.Size = 11;
            cellRange.Font.Name = "Times New Roman";
            cellRange.Text = "№ пп";
            cellRange = studentsTable.Cell(1, 2).Range;
            cellRange.Font.Size = 11;
            cellRange.Font.Name = "Times New Roman";
            cellRange.Text = "Ф.И.О. студента";
            cellRange = studentsTable.Cell(1, 3).Range;
            cellRange.Font.Size = 11;
            cellRange.Font.Name = "Times New Roman";
            cellRange.Text = "1 аттестация (max 50 баллов)";
            cellRange = studentsTable.Cell(1, 4).Range;
            cellRange.Font.Size = 11;
            cellRange.Font.Name = "Times New Roman";
            cellRange.Text = "Кол-во пропусков лекционных занятий (сколько было/сколько пропущено)";
            cellRange = studentsTable.Cell(1, 5).Range;
            cellRange.Font.Size = 11;
            cellRange.Font.Name = "Times New Roman";
            cellRange.Text = "Кол-во пропусков практических занятий (сколько было/сколько пропущено)";
            cellRange = studentsTable.Cell(1, 6).Range;
            cellRange.Font.Size = 11;
            cellRange.Font.Name = "Times New Roman";
            cellRange.Text = "Кол-во пропусков лабораторных занятий (сколько было/сколько пропущено)";
            studentsTable.Rows[1].Range.Bold = 1;
            studentsTable.Rows[1].Range.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

            int i = 1;
            foreach (var item in this.studentInReportsCard)
            {
                cellRange = studentsTable.Cell(i + 1, 1).Range;
                cellRange.Font.Size = 11;
                cellRange.Font.Name = "Times New Roman";
                cellRange.Text = i.ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                cellRange = studentsTable.Cell(i + 1, 2).Range;
                cellRange.Font.Size = 11;
                cellRange.Font.Name = "Times New Roman";
                cellRange.Text = item.Student.FullName;
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;

                cellRange = studentsTable.Cell(i + 1, 3).Range;
                cellRange.Font.Size = 11;
                cellRange.Font.Name = "Times New Roman";
                cellRange.Text = item.Scores.ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                
                cellRange = studentsTable.Cell(i + 1, 4).Range;
                cellRange.Font.Size = 11;
                cellRange.Font.Name = "Times New Roman";
                cellRange.Text = item.NumberMissedLectures.ToString() + "/" + this.reportCard.NumberLectures.ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                
                cellRange = studentsTable.Cell(i + 1, 5).Range;
                cellRange.Font.Size = 11;
                cellRange.Font.Name = "Times New Roman";
                cellRange.Text = item.NumberMissedPractical.ToString() + "/" + this.reportCard.NumberPractical.ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;               
                
                cellRange = studentsTable.Cell(i + 1, 6).Range;
                cellRange.Font.Size = 11;
                cellRange.Font.Name = "Times New Roman";
                cellRange.Text = item.NumberMissedLabs.ToString() + "/" + this.reportCard.NumberLabs.ToString();
                cellRange.ParagraphFormat.Alignment = Word.WdParagraphAlignment.wdAlignParagraphCenter;
                i++;
            }

            document.Words.Last.InsertBreak(Word.WdBreakType.wdPageBreak);

            app.Visible = true;

            document.SaveAs2(@"D:\outputFileWord.docx");
            document.SaveAs2(@"D:\outputFilePdf.pdf", Word.WdExportFormat.wdExportFormatPDF);
        }
    }
}
