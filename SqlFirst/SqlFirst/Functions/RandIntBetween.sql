CREATE FUNCTION [dbo].[RandIntBetween]
(
  @lower  INT,
  @upper  INT,
  @rand   FLOAT
)
RETURNS INT
AS
BEGIN
  DECLARE @result INT;
  DECLARE @range INT = @upper - @lower + 1;
  SET @result = FLOOR(@rand * @range + @lower);
  RETURN @result;
END
