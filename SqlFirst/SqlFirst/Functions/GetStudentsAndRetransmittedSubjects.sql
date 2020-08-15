CREATE FUNCTION [dbo].[GetStudentsAndRetransmittedSubjects]
(
	@maxFoursCount int
)
RETURNS @returntable TABLE
(
	[StudentId] uniqueidentifier NOT NULL,
	[SubjectId] uniqueidentifier NOT NULL,
	[ScoreCount] nvarchar(50) NOT NULL
)
AS
BEGIN
	INSERT @returntable
	SELECT stud.Id, sub.Id, sc.[Value]
	FROM [dbo].StudentScore sc
	JOIN [dbo].Student stud ON stud.Id = sc.StudentId
	JOIN [dbo].[Subject] sub ON sub.Id = sc.SubjectId
	JOIN [dbo].[Group] g ON g.Id = stud.GroupId
	JOIN [dbo].[SubjectCourse] subCu ON subCu.SubjectId = sub.Id AND g.CourseId = subCu.CourseId
	JOIN [dbo].SubjectSpecialty subSpec ON subSpec.SubjectId = sub.Id AND g.SpecialtyId = subSpec.SpecialtyId
	WHERE sc.[Value] < 5  AND g.CourseId = sc.CourseId
	AND stud.Id NOT IN (SELECT s.StudentId
	FROM [dbo].GetCleverStudents(@maxFoursCount) s)
	RETURN
END
