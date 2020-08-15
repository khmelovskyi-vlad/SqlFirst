UPDATE [dbo].[Student]
SET AverageScore = ( 
SELECT AVG(s.[Value])
FROM [dbo].[StudentScore] s
WHERE s.StudentId = [dbo].[Student].Id
)