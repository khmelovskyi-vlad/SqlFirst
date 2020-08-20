DECLARE @coursesCount int = (SELECT COUNT(Id)
FROM [dbo].[Course])
DECLARE @specialtiesCount int = (SELECT COUNT(Id)
FROM [dbo].[Specialty])
DECLARE @specialtiesCountDontChange int = @specialtiesCount
CREATE TABLE #TempGroups
(
	Id UNIQUEIDENTIFIER NOT NULL, 
	GroupName nvarchar(60) NOT NULL,
	CourseId UNIQUEIDENTIFIER NOT NULL, 
	SpecialtyId UNIQUEIDENTIFIER NOT NULL
)
WHILE @coursesCount > 0
BEGIN
	DECLARE @courseId UNIQUEIDENTIFIER = (SELECT cor.Id FROM (
		  SELECT
		    ROW_NUMBER() OVER (ORDER BY c.Id ASC) AS rowNumber,
		    c.Id
		  FROM [dbo].[Course] c
		) AS cor
		WHERE rowNumber = @coursesCount
		)
		DECLARE @courseName nvarchar(50) = (SELECT [Name]
		FROM [dbo].[Course]
		WHERE Id = @courseId)

	WHILE @specialtiesCount > 0
	BEGIN
		DECLARE @specialtyId UNIQUEIDENTIFIER = (SELECT spec.Id FROM (
			  SELECT
			    ROW_NUMBER() OVER (ORDER BY s.Id ASC) AS rowNumber,
			    s.Id
			  FROM [dbo].[Specialty] s
			) AS spec
			WHERE rowNumber = @specialtiesCount
			)
		DECLARE @specialtyName nvarchar(50) = (SELECT [Name]
		FROM [dbo].[Specialty]
		WHERE Id = @specialtyId)



		INSERT INTO #TempGroups
		(Id, GroupName, CourseId, SpecialtyId)
		VALUES
		(NEWID(), @specialtyName + '-' + @courseName, @courseId, @specialtyId)

		SET @specialtiesCount = @specialtiesCount - 1;
	END
	SET @specialtiesCount = @specialtiesCountDontChange
	SET @coursesCount = @coursesCount - 1;
END
INSERT INTO [dbo].[Group]
SELECT tg.Id, tg.GroupName, NULL, tg.CourseId, tg.SpecialtyId
FROM #TempGroups tg