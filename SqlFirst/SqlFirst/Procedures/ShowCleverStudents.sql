CREATE PROCEDURE [dbo].[ShowCleverStudents]
  @minLength INT,
  @maxLength INT,
  @chars VARCHAR(200),
  @randomString VARCHAR(MAX) = NULL OUTPUT
AS
  DECLARE @stringLength INT = [dbo].[RandIntBetween](@minLength, @maxLength, RAND());
  SET @randomString = ''
  WHILE LEN(@randomString) < @stringLength
  BEGIN
    SET @randomString = @randomString + [dbo].[PickRandomChar](@chars, RAND());
  END
RETURN 0
