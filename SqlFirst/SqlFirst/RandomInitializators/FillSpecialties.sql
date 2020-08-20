DECLARE @specialtiesCount INT = 80;
CREATE TABLE #TempSpecialties
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
	[Name] NVARCHAR(50) NOT NULL
)
WHILE @specialtiesCount > 0
BEGIN
	DECLARE @randomString NVARCHAR(50)
	EXEC PickRandomStringg 1, 50, 'abcdefghijklmnopqrstuvwxyz', @randomString OUTPUT;
	INSERT INTO #TempSpecialties
	(Id, [Name])
	VALUES
	(NEWID(), @randomString)
	SET @specialtiesCount = @specialtiesCount - 1;
END
INSERT INTO [dbo].[Specialty]
SELECT *
FROM #TempSpecialties