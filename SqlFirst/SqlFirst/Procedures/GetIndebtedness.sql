CREATE PROCEDURE [dbo].[GetIndebtedness]
AS
DECLARE @Temp TABLE
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [Count] int NOT NULL
)
INSERT @Temp
SELECT *
FROM [dbo].GetCleverStudents(4)

DECLARE @Temp2 TABLE
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [SubjectName] nvarchar(50) NOT NULL,
    [Count] int NOT NULL
)

INSERT @Temp2
SELECT stud.Id, sub.[Name], COUNT(stud.Id)
FROM [dbo].StudentScore sc
JOIN [dbo].Student stud ON stud.Id = sc.StudentId
JOIN [dbo].[Subject] sub ON sub.Id = sc.SubjectId
WHERE sc.[Value] < 5
GROUP BY stud.Id, sub.[Name]
HAVING stud.Id NOT IN (SELECT t.Id
FROM @Temp t)

DECLARE @Temp3 TABLE
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [FirstName] nvarchar(50) NOT NULL,
    [LastName] nvarchar(50) NOT NULL,
    [SubjectName] nvarchar(50) NOT NULL,
    [Count] int NOT NULL
)
DECLARE @Temp4 TABLE
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [Count] int NOT NULL
)

INSERT @Temp4
SELECT t.Id, SUM(t.[Count])
FROM @Temp2 t
GROUP BY t.ID

INSERT @Temp3
SELECT stud.Id, stud.FirstName, stud.LastName, t.SubjectName, t4.[Count]
FROM @Temp2 t
JOIN [dbo].Student stud ON stud.Id = t.Id
JOIN @Temp4 t4 ON t4.Id = t.Id

SELECT *
FROM @Temp3