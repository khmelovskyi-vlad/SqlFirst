CREATE TABLE #TempGroups
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [CourseName] int NOT NULL,
    [SpecialtyName] NVARCHAR(50) NOT NULL
)
INSERT INTO #TempGroups
(Id, [Name], [CourseName], [SpecialtyName])
VALUES
(NEWID(), 'Ec-01', 1, 'Economy'),
(NEWID(), 'Ec-02', 2, 'Economy'),
(NEWID(), 'Ps-01', 1, 'Psychology'),
(NEWID(), 'Jo-01', 1, 'Journalism')

INSERT INTO [dbo].[Group]
(Id, [Name], AverageScore, CourseId, SpecialtyId)
SELECT tg.Id, tg.[Name], NULL, c.Id, s.Id
FROM #TempGroups tg
JOIN [dbo].[Course] c ON c.Name = tg.CourseName
JOIN [dbo].Specialty s ON s.Name = tg.SpecialtyName