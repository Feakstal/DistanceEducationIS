//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DistanceEducation.DataBase
{
    using System;
    using System.Collections.Generic;
    
    public partial class Report
    {
        public int ReportID { get; set; }
        public int PupilID { get; set; }
        public Nullable<int> RatingID { get; set; }
        public int TeacherID { get; set; }
        public int TimetableID { get; set; }
        public int LessonTopicID { get; set; }
        public int LessonTypeID { get; set; }
        public bool IsAttend { get; set; }
    
        public virtual LessonTopic LessonTopic { get; set; }
        public virtual LessonType LessonType { get; set; }
        public virtual Pupil Pupil { get; set; }
        public virtual Rating Rating { get; set; }
        public virtual Teacher Teacher { get; set; }
        public virtual Timetable Timetable { get; set; }
    }
}