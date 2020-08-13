CREATE FUNCTION [dbo].[GetCleverStudents]
(
	@maxFoursCount int
)
RETURNS @returntable TABLE
(
	[StudentId] uniqueidentifier NOT NULL,
	[FirstName] nvarchar(50) NOT NULL,
	[LastName] nvarchar(50) NOT NULL,
	[FoursCount] int NOT NULL
)
AS
BEGIN
	INSERT @returntable
	SELECT st.Id, st.FirstName, st.LastName, ts.[Count]
	FROM (SELECT st.Id, score.[Count] AS [Score], COUNT(score.[Count]) AS [Count]
	FROM [dbo].[StudentScore] score
	JOIN [dbo].[Student] st ON st.Id = score.StudentId
	JOIN [dbo].[Group] g ON g.Id = st.GroupId
	JOIN [dbo].[Subject] sub ON sub.Id = score.SubjectId
	WHERE g.CourseId = sub.CourseId AND g.SpecialtyId = g.SpecialtyId
	GROUP BY st.Id, score.[Count]) ts
	JOIN [dbo].Student st ON st.Id = ts.Id
	JOIN [dbo].GetStudentsWithMinimalScores() stMin ON stMin.StudentID = st.Id
	WHERE stMin.MinimalScore > 3 
	GROUP BY st.Id, st.FirstName, st.LastName,ts.[Count]
	HAVING 
	CAST(1 AS BIT) LIKE CAST( CASE WHEN MIN(ts.[Score]) = 4 AND ts.[Count] <= @maxFoursCount THEN 1 ELSE 0 END AS BIT)
	RETURN;
END
