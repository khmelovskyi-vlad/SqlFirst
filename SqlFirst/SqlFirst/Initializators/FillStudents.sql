DECLARE @firstId UNIQUEIDENTIFIER
SET @firstId = NEWID()
SELECT @firstId
DECLARE @secondId UNIQUEIDENTIFIER
SET @secondId = NEWID()
SELECT @secondId
DECLARE @thirdtId UNIQUEIDENTIFIER
SET @thirdtId = NEWID()
SELECT @thirdtId
DECLARE @fourthId UNIQUEIDENTIFIER
SET @fourthId = NEWID()
SELECT @fourthId
DECLARE @fifthId UNIQUEIDENTIFIER
SET @fifthId = NEWID()
SELECT @fifthId

CREATE TABLE #TempStudentes
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL,
    [GroupName] NVARCHAR(50) NOT NULL
)

INSERT INTO #TempStudentes
(Id, FirstName, LastName, GroupName)
VALUES
(@firstId, 'Vasia', 'Pupkin', 'YD-01'),
(@secondId, 'Vanya', 'Pupchelo', 'YD-02'),
(@thirdtId, 'Katya', 'Pupkina', 'ET-01'),
(@fourthId, 'Irka', 'Pupchelosna', 'ET-01'),
(@fifthId, 'Kartoshka', 'Pup', 'ET-01')

INSERT INTO [dbo].[Student]
(Id, FirstName, LastName, GroupId, AverageScore)
SELECT ts.Id, ts.FirstName, ts.LastName, g.Id, NULL
FROM #TempStudentes ts
JOIN [dbo].[Group] g ON g.Name = ts.GroupName


CREATE TABLE #TempSubject
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Count] int NOT NULL, 
    [SubjectName] NVARCHAR(50) NOT NULL,
    StudentId UNIQUEIDENTIFIER NOT NULL
)

INSERT INTO #TempSubject
(Id, [Count], [SubjectName], StudentId)
VALUES
(NEWID(), 4, '1', @firstId),
(NEWID(), 4, '2', @firstId),
(NEWID(), 5, '3', @firstId),
(NEWID(), 5, '4', @firstId),
(NEWID(), 4, '5', @firstId),

(NEWID(), 2, '1', @secondId),
(NEWID(), 5, '2', @secondId),
(NEWID(), 5, '3', @secondId),
(NEWID(), 5, '4', @secondId),
(NEWID(), 4, '5', @secondId),

(NEWID(), 3, '1', @thirdtId),
(NEWID(), 4, '2', @thirdtId),
(NEWID(), 4, '3', @thirdtId),
(NEWID(), 4, '4', @thirdtId),
(NEWID(), 3, '5', @thirdtId),

(NEWID(), 1, '1', @fourthId),
(NEWID(), 2, '2', @fourthId),
(NEWID(), 3, '3', @fourthId),
(NEWID(), 4, '4', @fourthId),
(NEWID(), 5, '5', @fourthId),

(NEWID(), 5, '1', @fifthId),
(NEWID(), 4, '2', @fifthId),
(NEWID(), 3, '3', @fifthId),
(NEWID(), 5, '4', @fifthId),
(NEWID(), 1, '5', @fifthId)

INSERT INTO [dbo].StudentScore
(Id, [Count], [SubjectId], StudentId)
SELECT ts.Id, ts.[Count], s.Id, st.Id
FROM #TempSubject ts
JOIN [dbo].[Subject] s ON s.Name = ts.SubjectName
JOIN [dbo].Student st ON st.Id = ts.StudentId