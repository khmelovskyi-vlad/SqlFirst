CREATE TABLE [dbo].[SubjectCourse]
(
	[SubjectId] UNIQUEIDENTIFIER NOT NULL , 
    [CourseId] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY ([SubjectId], [CourseId])
)
