--set statistics io on
--set statistics time on
--DECLARE @studentsCount int = 100
--DECLARE @groupsCount int = (SELECT COUNT(Id)
--FROM [dbo].[Group])

--CREATE TABLE #TempStudents
--(
--	Id UNIQUEIDENTIFIER NOT NULL, 
--	FirstName nvarchar(50) NOT NULL,
--	LastName nvarchar(50) NOT NULL, 
--	GroupId UNIQUEIDENTIFIER NOT NULL
--)
--CREATE TABLE #TempScores
--(
--	Id UNIQUEIDENTIFIER NOT NULL, 
--	[Value] int NOT NULL,
--	SubjectId UNIQUEIDENTIFIER NOT NULL,
--	StudentId UNIQUEIDENTIFIER NOT NULL,
--	CourseId UNIQUEIDENTIFIER NOT NULL
--)
--WHILE @studentsCount > 0
--BEGIN
--	DECLARE @randomFirstName NVARCHAR(50)
--	EXEC PickRandomStringg 1, 50, 'abcdefghijklmnopqrstuvwxyz', @randomFirstName OUTPUT;
--	DECLARE @randomLastName NVARCHAR(50)
--	EXEC PickRandomStringg 1, 50, 'abcdefghijklmnopqrstuvwxyz', @randomLastName OUTPUT;
--	DECLARE @studentId UNIQUEIDENTIFIER = NEWID()

--	DECLARE @group TABLE
--	(
--		Id UNIQUEIDENTIFIER NOT NULL, 
--		CourseId UNIQUEIDENTIFIER NOT NULL,
--		SpecialtyId UNIQUEIDENTIFIER NOT NULL
--	)
--	INSERT @group
--	SELECT gro.Id, gro.CourseId, gro.SpecialtyId FROM (
--	  SELECT
--	    ROW_NUMBER() OVER (ORDER BY g.Id ASC) AS rowNumber,
--	    g.Id,
--		g.CourseId,
--		g.SpecialtyId
--	  FROM [dbo].[Group] g
--	) AS gro
--	WHERE rowNumber = [dbo].RandIntBetween(1, @groupsCount, RAND())

--	DECLARE @scoreCount INT = (SELECT COUNT(sub.Id)
--	FROM [dbo].[Subject] sub
--	JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = sub.Id AND subCo.CourseId = (SELECT g.CourseId FROM @group g)
--	JOIN [dbo].[SubjectSpecialty] subSpec ON subSpec.SubjectId = sub.Id AND subSpec.SpecialtyId = (SELECT g.SpecialtyId FROM @group g))

--	INSERT INTO #TempStudents
--	(Id, FirstName, LastName, GroupId)
--	VALUES
--	(@studentId, @randomFirstName, @randomLastName, (SELECT g.Id FROM @group g))

--	WHILE @scoreCount > 0
--		BEGIN
--			DECLARE @subjectId UNIQUEIDENTIFIER = (SELECT sub.Id FROM (
--			  SELECT
--			    ROW_NUMBER() OVER (ORDER BY s.Id ASC) AS rowNumber,
--			    s.Id
--			  FROM [dbo].[Subject] s
--				JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = s.Id AND subCo.CourseId = (SELECT g.CourseId FROM @group g)
--				JOIN [dbo].[SubjectSpecialty] subSpec ON subSpec.SubjectId = s.Id AND subSpec.SpecialtyId = (SELECT g.SpecialtyId FROM @group g)
--			) AS sub
--			WHERE rowNumber = @scoreCount)
--			DECLARE @scoreValue INT = [dbo].RandIntBetween(1, 5, RAND())
			
--			INSERT INTO #TempScores
--			(Id, [Value], SubjectId, StudentId, CourseId)
--			VALUES
--			(NEWID(), @scoreValue, @subjectId, @studentId, (SELECT g.CourseId FROM @group g))
--			SET @scoreCount = @scoreCount - 1
--		END
		
--	DELETE @group
--	SET @studentsCount = @studentsCount - 1;
--END
--INSERT INTO [dbo].[Student]
--SELECT ts.Id, ts.FirstName, ts.LastName, ts.GroupId, NULL
--FROM #TempStudents ts

--INSERT INTO [dbo].[Score]
--SELECT *
--FROM #TempScores ts









DECLARE @group TABLE
(
	Id UNIQUEIDENTIFIER NOT NULL, 
	CourseId UNIQUEIDENTIFIER NOT NULL,
	SpecialtyId UNIQUEIDENTIFIER NOT NULL
)

INSERT @group
SELECT TOP 1 g.Id, g.CourseId, g.SpecialtyId
FROM [dbo].[Group] g
ORDER BY NEWID()

DECLARE @subjectId UNIQUEIDENTIFIER = (SELECT TOP 1 s.Id
FROM [dbo].[Subject] s
JOIN [dbo].[SubjectCourse] subCo ON subCo.SubjectId = s.Id AND subCo.CourseId = (SELECT g.CourseId FROM @group g)
JOIN [dbo].[SubjectSpecialty] subSpec ON subSpec.SubjectId = s.Id AND subSpec.SpecialtyId = (SELECT g.SpecialtyId FROM @group g)
ORDER BY NEWID())


DECLARE @studentsCount int = 10000000
CREATE TABLE #TempStudents
(
	Id UNIQUEIDENTIFIER NOT NULL, 
	FirstName nvarchar(50) NOT NULL,
	LastName nvarchar(50) NOT NULL, 
	GroupId UNIQUEIDENTIFIER NOT NULL
)
CREATE TABLE #TempScores
(
	Id UNIQUEIDENTIFIER NOT NULL, 
	[Value] int NOT NULL,
	SubjectId UNIQUEIDENTIFIER NOT NULL,
	StudentId UNIQUEIDENTIFIER NOT NULL,
	CourseId UNIQUEIDENTIFIER NOT NULL
)
WHILE @studentsCount > 0
	BEGIN
		DECLARE @firstName NVARCHAR(50) = 'first'
		DECLARE @lastName NVARCHAR(50) = 'last'
		DECLARE @studentId uniqueidentifier = NEWID()

		INSERT #TempStudents
		(Id, FirstName, LastName, GroupId)
		VALUES
		(@studentId, @firstName, @lastName, (SELECT g.Id FROM @group g))

		
		INSERT #TempScores
		(Id, [Value], SubjectId, StudentId, CourseId)
		VALUES
		(NEWID(), 5, @subjectId, @studentId, (SELECT g.CourseId FROM @group g))

		SET @studentsCount = @studentsCount - 1
	END
	
INSERT INTO [dbo].[Student]
SELECT ts.Id, ts.FirstName, ts.LastName, ts.GroupId, NULL
FROM #TempStudents ts

INSERT INTO [dbo].[Score]
SELECT *
FROM #TempScores ts