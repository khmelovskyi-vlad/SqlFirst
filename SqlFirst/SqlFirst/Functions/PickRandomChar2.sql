CREATE FUNCTION [dbo].[PickRandomChar2]
(
  @chars NVARCHAR(50),
  @rand  FLOAT
)
RETURNS @returntable TABLE
(
	RandomChar char(1)
)
AS
BEGIN
	INSERT @returntable
	VALUES (SUBSTRING(@chars, (SELECT *
	FROM [dbo].[RandIntBetween2](1, LEN(@chars), @rand)), 1))
	RETURN
END
