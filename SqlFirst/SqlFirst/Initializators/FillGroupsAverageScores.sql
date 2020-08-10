UPDATE [dbo].[Group]
SET AverageScore = ( 
SELECT AVG(s.[AverageScore])
FROM [dbo].[Student] s
WHERE s.GroupId = [dbo].[Group].Id
)