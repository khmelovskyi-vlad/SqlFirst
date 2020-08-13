CREATE PROCEDURE [dbo].[ShowDebts]
	@maxFoursCount int,
	@maxCountRetakes int
AS
SELECT stud.Id, stud.FirstName, stud.LastName, sub.[Name], 
CASE WHEN t4.FoursCount <= @maxFoursCount THEN t4.[Count] - t4.FoursCount ELSE t4.[Count] - @maxFoursCount END AS [Count],
CASE WHEN t4.FoursCount > @maxFoursCount AND t.ScoreCount = 4 AND @maxFoursCount != 0 THEN 'At choice' ELSE 'Need' END AS [NeedRetake]

FROM GetStudentsAndRetransmittedSubjects(@maxFoursCount) t
JOIN [dbo].Student stud ON stud.Id = t.StudentId
JOIN [dbo].[Subject] sub ON sub.Id = t.SubjectId

JOIN (SELECT t.StudentId, COUNT(CASE WHEN t.ScoreCount = 4 THEN 1 END) AS [FoursCount], COUNT(t.StudentId) AS [Count]
FROM GetStudentsAndRetransmittedSubjects(@maxFoursCount) t
GROUP BY t.StudentId) t4 ON t4.StudentId = t.StudentId

WHERE (t4.[Count] <= @maxCountRetakes AND
	CAST(1 AS BIT) LIKE CAST( CASE WHEN  t.ScoreCount != 4 THEN 1 ELSE 0 END AS BIT))
	OR (t4.[Count] <= @maxCountRetakes AND
	CAST(1 AS BIT) LIKE CAST( CASE WHEN  t.ScoreCount = 4 AND t4.[FoursCount] > @maxFoursCount THEN 1 ELSE 0 END AS BIT))
ORDER BY stud.FirstName
RETURN 0
