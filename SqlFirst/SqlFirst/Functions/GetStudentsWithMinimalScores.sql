CREATE FUNCTION [dbo].[GetStudentsWithMinimalScores]
(
)
RETURNS @returntable TABLE
(
	[StudentID] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
	[MinimalScore] int
)
AS
BEGIN
	INSERT @returntable
	SELECT st.Id, MIN(score.[Value])
	FROM [dbo].[StudentScore] score
	JOIN [dbo].[Student] st ON st.Id = score.StudentId
	JOIN [dbo].[Group] g ON g.Id = st.GroupId
	JOIN [dbo].[Subject] sub ON sub.Id = score.SubjectId
	JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = sub.Id
	JOIN [dbo].[SubjectSpecialty] subSpec ON subSpec.SubjectId = sub.Id
	WHERE g.CourseId = subCo.CourseId AND g.SpecialtyId = subSpec.SpecialtyId
	GROUP BY st.ID
	RETURN
END
