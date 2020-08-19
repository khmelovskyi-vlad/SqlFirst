DECLARE @courseCount INT = 6;
CREATE TABLE #TempCourses
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[Name] INT NOT NULL
)
WHILE @courseCount > 0
BEGIN
	INSERT INTO #TempCourses
	(Id, [Name])
	VALUES
	(NEWID(), @courseCount)
	SET @courseCount = @courseCount - 1;
END
INSERT INTO [dbo].[Course]
SELECT *
FROM #TempCourses