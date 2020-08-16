CREATE FUNCTION [dbo].[GetStudentsAndRetransmittedSubjects]
(
	@maxFoursCount int
)
RETURNS TABLE AS RETURN
(
	SELECT stud.Id AS [StudentId], sub.Id AS [SubjectId], sc.[Value] AS [ScoreCount]
	FROM [dbo].[Score] sc
	JOIN [dbo].Student stud ON stud.Id = sc.StudentId
	JOIN [dbo].[Subject] sub ON sub.Id = sc.SubjectId
	JOIN [dbo].[Group] g ON g.Id = stud.GroupId
	JOIN [dbo].[SubjectCourse] subCu ON subCu.SubjectId = sub.Id AND g.CourseId = subCu.CourseId
	JOIN [dbo].SubjectSpecialty subSpec ON subSpec.SubjectId = sub.Id AND g.SpecialtyId = subSpec.SpecialtyId
	WHERE sc.[Value] < 5  AND g.CourseId = sc.CourseId
	AND stud.Id NOT IN (SELECT s.StudentId
	FROM [dbo].GetCleverStudents(@maxFoursCount) s)
)