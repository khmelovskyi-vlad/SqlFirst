﻿CREATE FUNCTION [dbo].[PickRandomChar]
(
  @chars NVARCHAR(50),
  @rand  FLOAT
)
RETURNS CHAR(1)
AS
BEGIN
  DECLARE @result CHAR(1) = NULL;
  DECLARE @resultIndex INT = NULL;
  IF @chars IS NULL
    SET @result = NULL;
  ELSE IF LEN(@chars) = 0
    SET @result = NULL
  ELSE
  BEGIN
    SET @resultIndex = [dbo].[RandIntBetween](1, LEN(@chars), @rand);
    SET @result = SUBSTRING(@chars, @resultIndex, 1);
  END

  RETURN @result;
END
