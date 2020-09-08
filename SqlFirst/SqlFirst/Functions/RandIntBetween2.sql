CREATE FUNCTION [dbo].[RandIntBetween2]
(
  @lower  INT,
  @upper  INT,
  @rand   FLOAT
)
RETURNS @returntable TABLE
(
	RandomInt int
)
AS
BEGIN
	INSERT @returntable
	VALUES (FLOOR(@rand * (@upper - @lower + 1) + @lower))
	RETURN
END

