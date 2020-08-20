DECLARE @coursesCount int = (SELECT COUNT(Id)
FROM [dbo].[Course])
DECLARE @specialtiesCount int = (SELECT COUNT(Id)
FROM [dbo].[Specialty])

DECLARE @subjectsCount INT = 1000;
CREATE TABLE #TempSubjectsCourses
(
	SubjectId UNIQUEIDENTIFIER NOT NULL, 
	CourseId UNIQUEIDENTIFIER NOT NULL
)
CREATE TABLE #TempSubjectsSpecialties
(
	SubjectId UNIQUEIDENTIFIER NOT NULL, 
	SpecialtyId UNIQUEIDENTIFIER NOT NULL
)
CREATE TABLE #TempSubjects
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[Name] NVARCHAR(50) NOT NULL
)
WHILE @subjectsCount > 0
BEGIN
	DECLARE @randomString NVARCHAR(50)
	DECLARE @Id UNIQUEIDENTIFIER = NEWID()
	EXEC PickRandomStringg 1, 50, 'abcdefghijklmnopqrstuvwxyz', @randomString OUTPUT;

	INSERT INTO #TempSubjects
	(Id, [Name])
	VALUES
	(@Id, @randomString)
	   	

	DECLARE @rowNumber int = [dbo].RandIntBetween(1, @coursesCount, RAND())
	WHILE @rowNumber != @coursesCount+1
	BEGIN
	INSERT INTO #TempSubjectsCourses
	(SubjectId, CourseId)
	VALUES(@Id, (SELECT cor.Id 
	FROM (
	  SELECT
	    ROW_NUMBER() OVER (ORDER BY c.Id ASC) AS rowNumber,
	    c.Id
	  FROM [dbo].[Course] c
	) AS cor
	WHERE rowNumber = @rowNumber))
	SET @rowNumber = [dbo].RandIntBetween(@rowNumber + 1,@coursesCount + 1, RAND())
	END
	
	SET @rowNumber = [dbo].RandIntBetween(1, @specialtiesCount-1, RAND())
	WHILE @rowNumber != @specialtiesCount+1
	BEGIN
	INSERT INTO #TempSubjectsSpecialties
	(SubjectId, SpecialtyId)
	VALUES(@Id, (SELECT spec.Id 
	FROM (
	  SELECT
	    ROW_NUMBER() OVER (ORDER BY s.Id ASC) AS rowNumber,
	    s.Id
	  FROM [dbo].[Specialty] s
	) AS spec
	WHERE rowNumber = @rowNumber))
	SET @rowNumber = [dbo].RandIntBetween(@rowNumber + 1,@specialtiesCount + 1, RAND())
	END

	
	SET @subjectsCount = @subjectsCount - 1;
END

INSERT INTO [dbo].[Subject]
SELECT *
FROM #TempSubjects

INSERT INTO [dbo].[SubjectCourse]
SELECT *
FROM #TempSubjectsCourses

INSERT INTO [dbo].[SubjectSpecialty]
SELECT *
FROM #TempSubjectsSpecialties