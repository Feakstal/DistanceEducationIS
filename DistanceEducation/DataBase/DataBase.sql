USE [RemoteSchoolManagementDB]
GO
CREATE TABLE [dbo].[Discipline]
(
	[DisciplineID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[DisciplineName] [nvarchar](120) NOT NULL,
	[HoursCount] [smallint] NULL
)
CREATE TABLE [dbo].[Employee]
(
	[EmployeeID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Surname] [nvarchar](40) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[FatherName] [nvarchar](40) NULL,
	[JobExperience] [tinyint] NULL,
	[DisciplineID] [int] REFERENCES Discipline (DisciplineID) NULL,
	[PostID] [int] NOT NULL
)
CREATE TABLE [dbo].[GradeProfile](
	[GradeProfileID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[GradeProfileName] [nvarchar](80) NOT NULL,
)
CREATE TABLE [dbo].[Grade](
	[GradeID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[GradeName] [nvarchar](5) NOT NULL,
	[PupilCount] [tinyint] NOT NULL,
	[GradeProfileID] [int] REFERENCES GradeProfile (GradeProfileID) NOT NULL,
)
CREATE TABLE [dbo].[IsAttend](
	[IsAttendID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[IsAttendName] [nvarchar](50) NOT NULL,
)
CREATE TABLE [dbo].[Lesson](
	[LessonID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[LessonName] [nvarchar](100) NOT NULL,
	[DisciplineID] [int] REFERENCES Discipline (DisciplineID) NOT NULL,
	[HourCount] [int] NOT NULL,
)
CREATE TABLE [dbo].[LessonTopic](
	[LessonTopicID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[LessonTopicName] [nvarchar](150) NOT NULL
)
CREATE TABLE [dbo].[LessonType](
	[LessonTypeID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[LessonTypeName] [nvarchar](50) NOT NULL,
)
CREATE TABLE [dbo].[Post](
	[PostID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[PostName] [nvarchar](50) NOT NULL,
)
CREATE TABLE [dbo].[Pupil](
	[PupilID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Surname] [nvarchar](40) NOT NULL,
	[Name] [nvarchar](40) NOT NULL,
	[FatherName] [nvarchar](40) NULL,
	[GradeID] [int] REFERENCES Grade (GradeID) NOT NULL,
	[PupilStatusDesc] [nvarchar](100) NULL,
)
CREATE TABLE [dbo].[Rating](
	[RatingID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[RatingNumber] [tinyint] NOT NULL,
)
CREATE TABLE [dbo].[Timetable](
	[TimetableID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[GradeID] [int] REFERENCES Grade (GradeID) NOT NULL,
	[LessonID] [int] REFERENCES Lesson (LessonID) NOT NULL,
	[LessonNumber] [tinyint] NOT NULL,
	[DateStart] [datetime] NOT NULL,
	[DateEnd] [datetime] NOT NULL,
)
CREATE TABLE [dbo].[Report](
	[ReportID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[PupilID] [int] REFERENCES Pupil (PupilID) NOT NULL,
	[RatingID] [int] REFERENCES Rating (RatingID) NULL,
	[EmployeeID] [int] REFERENCES Employee (EmployeeID) NOT NULL,
	[TimetableID] [int] REFERENCES Timetable (TimetableID) NOT NULL,
	[LessonTopicID] [int] REFERENCES LessonTopic (LessonTopicID) NOT NULL,
	[LessonTypeID] [int] REFERENCES LessonType (LessonTypeID) NOT NULL,
	[IsAttendID] [int] REFERENCES IsAttend (IsAttendID) NOT NULL,
)
CREATE TABLE [dbo].[User](
	[UserID] [int] PRIMARY KEY IDENTITY(1,1) NOT NULL,
	[Login] [nvarchar](30) NOT NULL,
	[Password] [nvarchar](30) NOT NULL,
	[PostID] [int] REFERENCES Post (PostID) NOT NULL,
	[EmployeeID] [int] REFERENCES Employee (EmployeeID) NULL,
)

INSERT INTO Post (PostName)
VALUES ('Администратор'), ('Директор'), ('Учитель')

INSERT INTO Discipline (DisciplineName, HoursCount)
VALUES ('Без категории', NULL)

INSERT INTO Employee (Surname, [Name], FatherName, JobExperience, DisciplineID, PostID)
VALUES ('Новиков', 'Дмитрий', NULL, NULL, 1, 1)

INSERT INTO [User] (Login, Password, PostID, EmployeeID)
VALUES ('admin', 'admin', 1, 1)