CREATE TABLE #TempStudentes
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL,
    [GroupName] NVARCHAR(50) NOT NULL
)

INSERT INTO #TempStudentes
(ID, FirstName, LastName, GroupName)
VALUES
(NEWID(), 'Vasia', 'Pupkin', 'YD-01'),
(NEWID(), 'Vanya', 'Pupchelo', 'YD-02'),
(NEWID(), 'Katya', 'Pupkina', 'ET-01'),
(NEWID(), 'Irka', 'Pupchelosna', 'ET-01'),
(NEWID(), 'Kartoshka', 'Pup', 'ET-02')

INSERT INTO [dbo].[Student]
(ID, FirstName, LastName, GroupId)
SELECT ts.Id, ts.FirstName, ts.LastName, g.Id
FROM #TempStudentes ts
JOIN [dbo].[Group] g ON g.Name = ts.GroupName 