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
	SELECT stud.Id, sub.Id, sc.[Count]
	FROM [dbo].StudentScore sc
	JOIN [dbo].Student stud ON stud.Id = sc.StudentId
	JOIN [dbo].[Subject] sub ON sub.Id = sc.SubjectId
	WHERE sc.[Count] < 5 
	AND stud.Id NOT IN (SELECT s.StudentId
	FROM [dbo].GetCleverStudents(@maxFoursCount) s)
	RETURN
END
