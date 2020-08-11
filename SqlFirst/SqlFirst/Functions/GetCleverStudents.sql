CREATE FUNCTION [dbo].[GetCleverStudents]
(
	@param1 int
)
RETURNS @returntable TABLE
(
	c1 uniqueidentifier,
	c2 nvarchar(50),
	c3 nvarchar(50),
	c4 int,
	c5 int
)
AS
BEGIN

DECLARE @Temp TABLE
(
	[Id] UNIQUEIDENTIFIER NOT NULL,
    [Score] int NOT NULL,
    [Count] int NOT NULL
)
INSERT @Temp
SELECT st.Id, score.[Count], COUNT(score.[Count])
FROM [dbo].[StudentScore] score
JOIN [dbo].[Student] st ON st.Id = score.StudentId
GROUP BY st.Id, score.[Count]

	INSERT @returntable
	SELECT st.Id, st.FirstName, st.LastName, MIN(ts.[Score]), ts.[Count]
	FROM @Temp ts
	JOIN [dbo].Student st ON st.Id = ts.Id
	GROUP BY st.Id, st.FirstName, st.LastName,ts.[Count]
	HAVING MIN(ts.[Score]) > 3 AND 
	CAST(1 AS BIT) LIKE CASE WHEN MIN(ts.[Score]) = 4 AND ts.[Count] < @param1 THEN CAST(1 AS BIT) ELSE CAST(0 AS bit) END
	RETURN;
END
