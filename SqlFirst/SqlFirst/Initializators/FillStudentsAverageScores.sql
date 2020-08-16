UPDATE [dbo].[Student]
SET AverageScore = ( 
SELECT AVG(s.[Value])
FROM [dbo].[Score] s
WHERE s.StudentId = [dbo].[Student].Id
)