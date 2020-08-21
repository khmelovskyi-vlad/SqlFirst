--DECLARE @randomString NVARCHAR(50)
--EXEC PickRandomStringg 0, 50, 'abcdefghijklmnopqrstuvwxyz', @randomString OUTPUT;
--PRINT @randomString

--DECLARE @subjectCount INT = 1000;
--CREATE TABLE #TempSubjectsCourses
--(
--	SubjectId UNIQUEIDENTIFIER NOT NULL, 
--	CourseId UNIQUEIDENTIFIER NOT NULL
--)
--CREATE TABLE #TempSubjectsSpecialties
--(
--	SubjectId UNIQUEIDENTIFIER NOT NULL, 
--	SpecialtyId UNIQUEIDENTIFIER NOT NULL
--)
--WHILE @subjectCount > 0
--BEGIN

--	INSERT INTO #TempSubjectsCourses
--	(SubjectId, CourseId)
--	VALUES
--	(NEWID(), @randomString)

	
--	INSERT INTO #TempSubjectsSpecialties
--	(SubjectId, SpecialtyId)
--	VALUES
--	(NEWID(), @randomString)



--	SET @subjectCount = @subjectCount - 1;
--END
--INSERT INTO [dbo].[SubjectCourse]
--SELECT *
--FROM #TempSubjectsCourses


DECLARE @count INT = 1000000
DECLARE @chars NVARCHAR(50) = 'abcdefghijklmnopqrstuvwxyz'
WHILE @count > 0
BEGIN
	DECLARE @rangeStringLength INT = 5 - 1 + 1
	DECLARE @stringLength INT = FLOOR(RAND() * @rangeStringLength + 1)
	DECLARE @randomString NVARCHAR(50) = ''
	WHILE LEN(@randomString) < @stringLength
	BEGIN
		DECLARE @range INT = LEN(@chars) - 1 + 1
		DECLARE @resultIndex INT = FLOOR(RAND() * @range + 1)
		DECLARE @result CHAR(1) = SUBSTRING(@chars, @resultIndex, 1)
		SET @randomString = @randomString + @result
	END
	SET @count = @count - 1
END
 



--DECLARE @coursesCount int = (SELECT COUNT(Id)
--FROM [dbo].[Course])
--DECLARE @rowNumber int = [dbo].RandIntBetween(1, @coursesCount-1, RAND())
--DECLARE @res UNIQUEIDENTIFIER

--WHILE @rowNumber != @coursesCount+1
--BEGIN


--SET @res = (SELECT cor.Id FROM (
--  SELECT
--    ROW_NUMBER() OVER (ORDER BY c.Id ASC) AS rowNumber,
--    c.Id
--  FROM [dbo].[Course] c
--) AS cor
--WHERE rowNumber = @rowNumber
--)
--print(@res)
--SET @rowNumber = [dbo].RandIntBetween(@rowNumber + 1,@coursesCount + 1, RAND())
--END

--DECLARE @rand int = [dbo].RandIntBetween(1,@coursesCount, RAND())
--SELECT RAND(@rand), RAND(), RAND()

--EXEC ShowDebts 5, 50

--set statistics io on
--set statistics time on
--DECLARE @i INT = 100
--WHILE @i > 0
--	BEGIN
--		SELECT TOP 100 g.Id 
--		FROM [dbo].[Group] g
--		ORDER BY NEWID()
--		--SELECT gro.Id, gro.CourseId, gro.SpecialtyId FROM (
--		--SELECT
--		--    ROW_NUMBER() OVER (ORDER BY g.Id ASC) AS rowNumber,
--		--    g.Id,
--		--	g.CourseId,
--		--	g.SpecialtyId
--		--  FROM [dbo].[Group] g
--		--) AS gro
--		--WHERE rowNumber = [dbo].RandIntBetween(1, 50, RAND())
--		SET @i = @i - 1
--	END
	