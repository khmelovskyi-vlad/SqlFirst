UPDATE [dbo].[Student]
SET AverageScore = ( 
SELECT AVG(s.[Count])
FROM [dbo].[StudentScore] s
WHERE s.StudentId = [dbo].[Student].Id
)