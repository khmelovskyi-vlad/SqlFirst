CREATE FUNCTION [dbo].[GetStudentsWithMinimalScores]
(
)
RETURNS TABLE AS RETURN
(
	SELECT st.Id AS [StudentID], MIN(score.[Value]) As [MinimalScore]
	FROM [dbo].[Score] score
	JOIN [dbo].[Student] st ON st.Id = score.StudentId
	JOIN [dbo].[Group] g ON g.Id = st.GroupId
	JOIN [dbo].[Subject] sub ON sub.Id = score.SubjectId
	JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = sub.Id AND subCo.CourseId = g.CourseId
	JOIN [dbo].[SubjectSpecialty] subSpec ON subSpec.SubjectId = sub.Id AND subSpec.SpecialtyId = g.SpecialtyId
	GROUP BY st.ID
)