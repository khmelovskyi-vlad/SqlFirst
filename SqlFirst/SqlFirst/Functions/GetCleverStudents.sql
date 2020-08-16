CREATE FUNCTION [dbo].[GetCleverStudents]
(
	@maxFoursCount int
)
RETURNS TABLE AS RETURN
(	
	SELECT DISTINCT st.Id AS StudentId, stud.FirstName, stud.LastName, g.[Name]
	FROM(
	SELECT stt.Id, MIN(scoree.[Value]) AS MinimalScore
	FROM [dbo].[Score] scoree
	JOIN [dbo].[Student] stt ON stt.Id = scoree.StudentId
	JOIN [dbo].[Group] gg ON gg.Id = stt.GroupId
	JOIN [dbo].[Subject] subb ON subb.Id = scoree.SubjectId
	JOIN [dbo].[SubjectCourse] subCoo ON subCoo.SubjectId = subb.Id AND gg.CourseId = subCoo.CourseId
	JOIN [dbo].[SubjectSpecialty] subSpecc ON subSpecc.SubjectId = subb.Id AND gg.SpecialtyId = subSpecc.SpecialtyId
	WHERE scoree.CourseId = gg.CourseId
	GROUP BY stt.ID) st
	JOIN (SELECT st.Id, score.[Value] AS [Score], COUNT(score.[Id]) AS [Count]
	FROM [dbo].[Score] score
	JOIN [dbo].[Student] st ON st.Id = score.StudentId
	JOIN [dbo].[Group] g ON g.Id = st.GroupId
	JOIN [dbo].[Subject] sub ON sub.Id = score.SubjectId
	JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = sub.Id AND g.CourseId = subCo.CourseId
	JOIN [dbo].[SubjectSpecialty] subSpec ON subSpec.SubjectId = sub.Id AND g.SpecialtyId = subSpec.SpecialtyId
	WHERE score.CourseId = g.CourseId
	GROUP BY st.Id, score.[Value]) ts ON ts.Id = st.Id
	JOIN [dbo].Student stud ON stud.Id = ts.Id
	JOIN [dbo].[Group] g ON g.Id = stud.GroupId
	WHERE CAST(1 AS BIT) LIKE CAST( CASE WHEN ts.[Count] <= @maxFoursCount AND st.MinimalScore = 4 THEN 1 ELSE 0 END AS BIT) 
	OR st.MinimalScore = 5
)
