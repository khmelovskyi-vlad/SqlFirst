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
	SELECT st.Id, MIN(score.[Count])
	FROM [dbo].[StudentScore] score
	JOIN [dbo].[Student] st ON st.Id = score.StudentId
	JOIN [dbo].[Group] g ON g.Id = st.GroupId
	JOIN [dbo].[Subject] sub ON sub.Id = score.SubjectId
	WHERE g.CourseId = sub.CourseId AND g.SpecialtyId = g.SpecialtyId
	GROUP BY st.ID
	RETURN
END
