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
DECLARE @sixthId UNIQUEIDENTIFIER
SET @sixthId = NEWID()
SELECT @sixthId
DECLARE @seventhId UNIQUEIDENTIFIER
SET @seventhId = NEWID()
SELECT @seventhId

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
(@firstId, 'Vasia', 'Pupkin', 'Ec-01'),
(@secondId, 'Vanya', 'Pupchelo', 'Ec-01'),
(@thirdtId, 'Katya', 'Pupkina', 'Ps-01'),
(@fourthId, 'Irka', 'Pupchelosna', 'Jo-01'),
(@fifthId, 'Kartoshka', 'Pup', 'Jo-01'),
(@sixthId, 'Kuchka', 'Kuk', 'Ec-02'),
(@seventhId, 'Valka', 'Krom', 'Ec-02')

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
(NEWID(), 4, '6', @thirdtId),
(NEWID(), 4, '7', @thirdtId),
(NEWID(), 4, '8', @thirdtId),
(NEWID(), 4, '9', @thirdtId),
(NEWID(), 3, '10', @thirdtId),
			  
(NEWID(), 5, '1', @fourthId),
(NEWID(), 1, '11', @fourthId),
(NEWID(), 2, '12', @fourthId),
(NEWID(), 3, '13', @fourthId),
(NEWID(), 4, '14', @fourthId),
(NEWID(), 5, '15', @fourthId),

(NEWID(), 5, '1', @fifthId),
(NEWID(), 4, '11', @fifthId),
(NEWID(), 3, '12', @fifthId),
(NEWID(), 5, '13', @fifthId),
(NEWID(), 1, '14', @fifthId),
(NEWID(), 1, '15', @fifthId),

(NEWID(), 5, '1', @sixthId),
(NEWID(), 5, '3', @sixthId),
(NEWID(), 5, '4', @sixthId),

(NEWID(), 4, '1', @seventhId),
(NEWID(), 3, '3', @seventhId),
(NEWID(), 3, '4', @seventhId)

INSERT INTO [dbo].StudentScore
(Id, [Value], [SubjectId], StudentId, CourseId)
SELECT ts.Id, ts.[Count], s.Id, st.Id, g.CourseId
FROM #TempSubject ts
JOIN [dbo].[Subject] s ON s.[Name] = ts.SubjectName
JOIN [dbo].Student st ON st.Id = ts.StudentId
JOIN [dbo].[Group] g ON g.Id = st.GroupId

CREATE TABLE #TempSubject2
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Count] int NOT NULL, 
    [SubjectName] NVARCHAR(50) NOT NULL,
    StudentId UNIQUEIDENTIFIER NOT NULL,
    [CourseName] int NOT NULL
)
INSERT INTO #TempSubject2
(Id, [Count], [SubjectName], StudentId, [CourseName])
VALUES
(NEWID(), 4, '1', @sixthId, 1),
(NEWID(), 4, '2', @sixthId, 1),
(NEWID(), 5, '3', @sixthId, 1),
(NEWID(), 5, '4', @sixthId, 1),
(NEWID(), 4, '5', @sixthId, 1),

(NEWID(), 2, '1', @seventhId, 1),
(NEWID(), 4, '2', @seventhId, 1),
(NEWID(), 3, '3', @seventhId, 1),
(NEWID(), 5, '4', @seventhId, 1),
(NEWID(), 4, '5', @seventhId, 1)

INSERT INTO [dbo].StudentScore
(Id, [Value], [SubjectId], StudentId, CourseId)
SELECT ts.Id, ts.[Count], s.Id, st.Id, co.Id
FROM #TempSubject2 ts
JOIN [dbo].[Subject] s ON s.[Name] = ts.SubjectName
JOIN [dbo].Student st ON st.Id = ts.StudentId
JOIN [dbo].[Course] co ON co.[Name] = ts.CourseName