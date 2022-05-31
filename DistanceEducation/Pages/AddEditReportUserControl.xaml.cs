using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using DistanceEducation.Classes;
using DistanceEducation.DataBase;

namespace DistanceEducation.Pages
{
    /// <summary>
    /// Логика взаимодействия для AddEditReportUserControl.xaml
    /// </summary>
    public partial class AddEditReportUserControl : UserControl
    {
        Entities Entities = new Entities();

        private bool CheckEdit = false;
        private Report EditReport = new Report();

        List<Timetable> listTimetable = new List<Timetable>();

        public AddEditReportUserControl()
        {
            InitializeComponent();

            cboxFIOPupil.ItemsSource = Entities.Pupil.Select(i => i.Surname + " " + i.Name + " " + i.FatherName).ToList();
            cboxRating.ItemsSource = Entities.Rating.Select(i => i.RatingNumber).ToList();
            cboxIsAttend.ItemsSource = Entities.IsAttend.Select(i => i.IsAttendName).ToList();
            cboxFIOEmployee.ItemsSource = Entities.Employee.Select(i => i.Surname + " " + i.Name + " " + i.FatherName).ToList();
            LbTimetable.ItemsSource = Entities.Timetable.ToList();
            cboxLessonTopic.ItemsSource = Entities.LessonTopic.Select(i => i.LessonTopicName).ToList();
            cboxLessonType.ItemsSource = Entities.LessonType.Select(i => i.LessonTypeName).ToList();

            tblockTitle.Text = "Добавление";
        }

        public AddEditReportUserControl(Report GetReport)
        {
            InitializeComponent();

            cboxFIOPupil.ItemsSource = Entities.Pupil.Select(i => i.Surname + " " + i.Name + " " + i.FatherName).ToList();
            cboxRating.ItemsSource = Entities.Rating.Select(i => i.RatingNumber).ToList();
            cboxIsAttend.ItemsSource = Entities.IsAttend.Select(i => i.IsAttendName).ToList();
            cboxFIOEmployee.ItemsSource = Entities.Employee.Select(i => i.Surname + " " + i.Name + " " + i.FatherName).ToList();
            LbTimetable.ItemsSource = Entities.Timetable.ToList();
            cboxLessonTopic.ItemsSource = Entities.LessonTopic.Select(i => i.LessonTopicName).ToList();
            cboxLessonType.ItemsSource = Entities.LessonType.Select(i => i.LessonTypeName).ToList();

            if (GetReport != null)
            {
                CheckEdit = true;
                EditReport = GetReport;

                cboxFIOPupil.SelectedItem = Entities.Pupil.Where(i => i.PupilID == GetReport.PupilID).Select(i => i.Surname + " " + i.Name + " " + i.FatherName).FirstOrDefault();
                cboxRating.SelectedItem = Entities.Rating.Where(i => i.RatingID == GetReport.RatingID).Select(i => i.RatingNumber).FirstOrDefault();
                cboxIsAttend.SelectedItem = Entities.IsAttend.Where(i => i.IsAttendID == GetReport.IsAttendID).Select(i => i.IsAttendName).FirstOrDefault();
                cboxFIOEmployee.SelectedItem = Entities.Employee.Where(i => i.EmployeeID == GetReport.EmployeeID).Select(i => i.Surname + " " + i.Name + " " + i.FatherName).FirstOrDefault();
                LbTimetable.SelectedItem = Entities.Timetable.Where(i => i.TimetableID == GetReport.TimetableID).Select(i => i.DateEnd).FirstOrDefault();
                cboxLessonTopic.SelectedItem = Entities.LessonTopic.Where(i => i.LessonTopicID == GetReport.LessonTopicID).Select(i => i.LessonTopicName).FirstOrDefault();
                cboxLessonType.SelectedItem = Entities.LessonType.Where(i => i.LessonTypeID == GetReport.LessonTypeID).Select(i => i.LessonTypeName).FirstOrDefault();
            }

            tblockTitle.Text = "Редактирование";
        }

        private void UpdateReport()
        {
            Report report = Entities.Report.Find(EditReport.ReportID);

            if (String.IsNullOrWhiteSpace(cboxRating.Text))
            {
                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) != null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) != null)
                {
                    report.PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault();
                    report.RatingID = null;
                    report.EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault();
                    report.TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault();
                    report.LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == cboxLessonTopic.SelectedItem.ToString()).Select(i => i.LessonTopicID).FirstOrDefault();
                    report.LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == cboxLessonType.SelectedItem.ToString()).Select(i => i.LessonTypeID).FirstOrDefault();
                    report.IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault();
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) == null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) == null)
                {
                    LessonType NewLessonType = new LessonType
                    {
                        LessonTypeName = cboxLessonType.Text
                    };

                    LessonTopic NewLessonTopic = new LessonTopic
                    {
                        LessonTopicName = cboxLessonTopic.Text
                    };

                    Entities.LessonType.Add(NewLessonType);
                    Entities.LessonTopic.Add(NewLessonTopic);
                    Entities.SaveChanges();

                    report.PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault();
                    report.RatingID = null;
                    report.EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault();
                    report.TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault();
                    report.LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == NewLessonTopic.LessonTopicName).Select(i => i.LessonTopicID).FirstOrDefault();
                    report.LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == NewLessonType.LessonTypeName).Select(i => i.LessonTypeID).FirstOrDefault();
                    report.IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault();
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) != null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) == null)
                {

                    LessonType NewLessonType = new LessonType
                    {
                        LessonTypeName = cboxLessonType.Text
                    };

                    Entities.LessonType.Add(NewLessonType);
                    Entities.SaveChanges();

                    report.PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault();
                    report.RatingID = null;
                    report.EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault();
                    report.TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault();
                    report.LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == NewLessonType.LessonTypeName).Select(i => i.LessonTopicID).FirstOrDefault();
                    report.LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == cboxLessonType.SelectedItem.ToString()).Select(i => i.LessonTypeID).FirstOrDefault();
                    report.IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault();
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) == null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) != null)
                {
                    LessonTopic NewLessonTopic = new LessonTopic
                    {
                        LessonTopicName = cboxLessonTopic.Text
                    };

                    Entities.LessonTopic.Add(NewLessonTopic);
                    Entities.SaveChanges();

                    report.PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault();
                    report.RatingID = null;
                    report.EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault();
                    report.TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault();
                    report.LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == NewLessonTopic.LessonTopicName).Select(i => i.LessonTopicID).FirstOrDefault();
                    report.LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == cboxLessonType.SelectedItem.ToString()).Select(i => i.LessonTypeID).FirstOrDefault();
                    report.IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault();
                }
            }
            else
            {
                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) != null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) != null)
                {
                    report.PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault();
                    report.RatingID = Entities.Rating.Where(i => i.RatingNumber.ToString() == cboxRating.SelectedItem.ToString()).Select(i => i.RatingID).FirstOrDefault();
                    report.EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault();
                    report.TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault();
                    report.LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == cboxLessonTopic.SelectedItem.ToString()).Select(i => i.LessonTopicID).FirstOrDefault();
                    report.LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == cboxLessonType.SelectedItem.ToString()).Select(i => i.LessonTypeID).FirstOrDefault();
                    report.IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault();
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) == null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) == null)
                {
                    LessonType NewLessonType = new LessonType
                    {
                        LessonTypeName = cboxLessonType.Text
                    };

                    LessonTopic NewLessonTopic = new LessonTopic
                    {
                        LessonTopicName = cboxLessonTopic.Text
                    };

                    Entities.LessonType.Add(NewLessonType);
                    Entities.LessonTopic.Add(NewLessonTopic);
                    Entities.SaveChanges();

                    report.PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault();
                    report.RatingID = Entities.Rating.Where(i => i.RatingNumber.ToString() == cboxRating.SelectedItem.ToString()).Select(i => i.RatingID).FirstOrDefault();
                    report.EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault();
                    report.TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault();
                    report.LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == NewLessonTopic.LessonTopicName).Select(i => i.LessonTopicID).FirstOrDefault();
                    report.LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == NewLessonType.LessonTypeName).Select(i => i.LessonTypeID).FirstOrDefault();
                    report.IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault();
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) != null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) == null)
                {

                    LessonType NewLessonType = new LessonType
                    {
                        LessonTypeName = cboxLessonType.Text
                    };

                    Entities.LessonType.Add(NewLessonType);
                    Entities.SaveChanges();

                    report.PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault();
                    report.RatingID = Entities.Rating.Where(i => i.RatingNumber.ToString() == cboxRating.SelectedItem.ToString()).Select(i => i.RatingID).FirstOrDefault();
                    report.EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault();
                    report.TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault();
                    report.LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == cboxLessonTopic.SelectedItem.ToString()).Select(i => i.LessonTopicID).FirstOrDefault();
                    report.LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == NewLessonType.LessonTypeName).Select(i => i.LessonTypeID).FirstOrDefault();
                    report.IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault();
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) == null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) != null)
                {
                    LessonTopic NewLessonTopic = new LessonTopic
                    {
                        LessonTopicName = cboxLessonTopic.Text
                    };

                    Entities.LessonTopic.Add(NewLessonTopic);
                    Entities.SaveChanges();

                    report.PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault();
                    report.RatingID = Entities.Rating.Where(i => i.RatingNumber.ToString() == cboxRating.SelectedItem.ToString()).Select(i => i.RatingID).FirstOrDefault();
                    report.EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault();
                    report.TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault();
                    report.LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == NewLessonTopic.LessonTopicName).Select(i => i.LessonTopicID).FirstOrDefault();
                    report.LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == cboxLessonType.SelectedItem.ToString()).Select(i => i.LessonTypeID).FirstOrDefault();
                    report.IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault();
                }
            }

            Entities.SaveChanges();

            MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void AddReport()
        {
            if (String.IsNullOrWhiteSpace(cboxRating.Text))
            {
                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) != null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) != null)
                {
                    Report NewReport = new Report
                    {
                        PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault(),
                        RatingID = null,
                        EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault(),
                        TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault(),
                        LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == cboxLessonTopic.SelectedItem.ToString()).Select(i => i.LessonTopicID).FirstOrDefault(),
                        LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == cboxLessonType.SelectedItem.ToString()).Select(i => i.LessonTypeID).FirstOrDefault(),
                        IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault()
                    };

                    Entities.Report.Add(NewReport);
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) == null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) == null)
                {
                    LessonType NewLessonType = new LessonType
                    {
                        LessonTypeName = cboxLessonType.Text
                    };

                    LessonTopic NewLessonTopic = new LessonTopic
                    {
                        LessonTopicName = cboxLessonTopic.Text
                    };

                    Entities.LessonType.Add(NewLessonType);
                    Entities.LessonTopic.Add(NewLessonTopic);
                    Entities.SaveChanges();

                    Report NewReport = new Report
                    {
                        PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault(),
                        RatingID = null,
                        EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault(),
                        TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault(),
                        LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == NewLessonTopic.LessonTopicName).Select(i => i.LessonTopicID).FirstOrDefault(),
                        LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == NewLessonType.LessonTypeName).Select(i => i.LessonTypeID).FirstOrDefault(),
                        IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault()
                    };

                    Entities.Report.Add(NewReport);
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) != null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) == null)
                {
                    LessonType NewLessonType = new LessonType
                    {
                        LessonTypeName = cboxLessonType.Text
                    };

                    Entities.LessonType.Add(NewLessonType);
                    Entities.SaveChanges();

                    Report NewReport = new Report
                    {
                        PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault(),
                        RatingID = null,
                        EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault(),
                        TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault(),
                        LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == cboxLessonTopic.SelectedItem.ToString()).Select(i => i.LessonTopicID).FirstOrDefault(),
                        LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == NewLessonType.LessonTypeName).Select(i => i.LessonTypeID).FirstOrDefault(),
                        IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault()
                    };

                    Entities.Report.Add(NewReport);
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) == null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) != null)
                {
                    LessonTopic NewLessonTopic = new LessonTopic
                    {
                        LessonTopicName = cboxLessonTopic.Text
                    };

                    Entities.LessonTopic.Add(NewLessonTopic);
                    Entities.SaveChanges();

                    Report NewReport = new Report
                    {
                        PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault(),
                        RatingID = null,
                        EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault(),
                        TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault(),
                        LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == NewLessonTopic.LessonTopicName).Select(i => i.LessonTopicID).FirstOrDefault(),
                        LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == cboxLessonType.SelectedItem.ToString()).Select(i => i.LessonTypeID).FirstOrDefault(),
                        IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault()
                    };

                    Entities.Report.Add(NewReport);
                }
            }
            else
            {
                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) != null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) != null)
                {
                    Report NewReport = new Report
                    {
                        PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault(),
                        RatingID = Entities.Rating.Where(i => i.RatingNumber.ToString() == cboxRating.SelectedItem.ToString()).Select(i => i.RatingID).FirstOrDefault(),
                        EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault(),
                        TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault(),
                        LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == cboxLessonTopic.SelectedItem.ToString()).Select(i => i.LessonTopicID).FirstOrDefault(),
                        LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == cboxLessonType.SelectedItem.ToString()).Select(i => i.LessonTypeID).FirstOrDefault(),
                        IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault()
                    };

                    Entities.Report.Add(NewReport);
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) == null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) == null)
                {
                    LessonType NewLessonType = new LessonType
                    {
                        LessonTypeName = cboxLessonType.Text
                    };

                    LessonTopic NewLessonTopic = new LessonTopic
                    {
                        LessonTopicName = cboxLessonTopic.Text
                    };

                    Entities.LessonType.Add(NewLessonType);
                    Entities.LessonTopic.Add(NewLessonTopic);
                    Entities.SaveChanges();

                    Report NewReport = new Report
                    {
                        PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault(),
                        RatingID = Entities.Rating.Where(i => i.RatingNumber.ToString() == cboxRating.SelectedItem.ToString()).Select(i => i.RatingID).FirstOrDefault(),
                        EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault(),
                        TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault(),
                        LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == NewLessonTopic.LessonTopicName).Select(i => i.LessonTopicID).FirstOrDefault(),
                        LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == NewLessonType.LessonTypeName).Select(i => i.LessonTypeID).FirstOrDefault(),
                        IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault()
                    };

                    Entities.Report.Add(NewReport);
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) != null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) == null)
                {
                    LessonType NewLessonType = new LessonType
                    {
                        LessonTypeName = cboxLessonType.Text
                    };

                    Entities.LessonType.Add(NewLessonType);
                    Entities.SaveChanges();

                    Report NewReport = new Report
                    {
                        PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault(),
                        RatingID = Entities.Rating.Where(i => i.RatingNumber.ToString() == cboxRating.SelectedItem.ToString()).Select(i => i.RatingID).FirstOrDefault(),
                        EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault(),
                        TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault(),
                        LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == cboxLessonTopic.SelectedItem.ToString()).Select(i => i.LessonTopicID).FirstOrDefault(),
                        LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == NewLessonType.LessonTypeName).Select(i => i.LessonTypeID).FirstOrDefault(),
                        IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault()
                    };

                    Entities.Report.Add(NewReport);
                }

                if (Entities.LessonTopic.FirstOrDefault(i => i.LessonTopicName == cboxLessonTopic.Text) == null && Entities.LessonType.FirstOrDefault(i => i.LessonTypeName == cboxLessonType.Text) != null)
                {
                    LessonTopic NewLessonTopic = new LessonTopic
                    {
                        LessonTopicName = cboxLessonTopic.Text
                    };

                    Entities.LessonTopic.Add(NewLessonTopic);
                    Entities.SaveChanges();

                    Report NewReport = new Report
                    {
                        PupilID = Entities.Pupil.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOPupil.SelectedItem.ToString()).Select(i => i.PupilID).FirstOrDefault(),
                        RatingID = Entities.Rating.Where(i => i.RatingNumber.ToString() == cboxRating.SelectedItem.ToString()).Select(i => i.RatingID).FirstOrDefault(),
                        EmployeeID = Entities.Employee.Where(i => i.Surname + " " + i.Name + " " + i.FatherName == cboxFIOEmployee.SelectedItem.ToString()).Select(i => i.EmployeeID).FirstOrDefault(),
                        TimetableID = Entities.Timetable.Where(i => i.DateEnd == ((Timetable)LbTimetable.SelectedItem).DateEnd).Select(i => i.TimetableID).FirstOrDefault(),
                        LessonTopicID = Entities.LessonTopic.Where(i => i.LessonTopicName == NewLessonTopic.LessonTopicName).Select(i => i.LessonTopicID).FirstOrDefault(),
                        LessonTypeID = Entities.LessonType.Where(i => i.LessonTypeName == cboxLessonType.SelectedItem.ToString()).Select(i => i.LessonTypeID).FirstOrDefault(),
                        IsAttendID = Entities.IsAttend.Where(i => i.IsAttendName == cboxIsAttend.SelectedItem.ToString()).Select(i => i.IsAttendID).FirstOrDefault()
                    };

                    Entities.Report.Add(NewReport);
                }
            }

            Entities.SaveChanges();

            MessageBox.Show("Данные успешно обновлены.", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            if (String.IsNullOrWhiteSpace(cboxFIOPupil.Text) || String.IsNullOrWhiteSpace(cboxFIOEmployee.Text) 
                || LbTimetable.SelectedItem == null || String.IsNullOrWhiteSpace(cboxLessonTopic.Text) 
                || String.IsNullOrWhiteSpace(cboxLessonType.Text) || String.IsNullOrWhiteSpace(cboxIsAttend.Text))
            {
                MessageBox.Show("Некоторые поля были не заполнены.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (cboxLessonTopic.Text.Length >= 150 || cboxFIOPupil.Text.Length >= 122 || cboxFIOEmployee.Text.Length >= 122)
            {
                MessageBox.Show("Вы вышли за диапазон допустимой длины строки.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите обновить запись?.", "Редактирование", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (question == MessageBoxResult.Yes)
                {
                    UpdateReport();
                }
            }
            else if (!CheckEdit)
            {
                var question = MessageBox.Show("Вы действительно хотите добавить запись?.", "Добавление", MessageBoxButton.YesNo, MessageBoxImage.Question);

                if (question == MessageBoxResult.Yes)
                {
                    AddReport();
                }
            }
        }

        private void Search()
        {
            listTimetable = Entities.Timetable.ToList();

            listTimetable = listTimetable.Where(i => i.DateEnd.ToString().Contains(tboxSearch.Text)).ToList();
            LbTimetable.ItemsSource = listTimetable;
        }

        private void tboxSearch_SelectionChanged(object sender, RoutedEventArgs e)
        {
            Search();
        }

        private void cboxFIO_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void cboxDate_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlForDate(sender, e);
        }

        private void cboxLessonTopic_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void cboxRating_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlLetters(sender, e);
        }

        private void cboxLessonType_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            ValidatorExtensions.PreviewTextInput_ControlNumbers(sender, e);
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            grdAddEditReport.Children.Clear();
            grdAddEditReport.Children.Add(new ReportsUserControl());
        }
    }
}
