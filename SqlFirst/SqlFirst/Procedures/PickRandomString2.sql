CREATE PROCEDURE [dbo].[PickRandomString2]
  @minLength INT,
  @maxLength INT,
  @chars VARCHAR(200),
  @randomString VARCHAR(MAX) = NULL OUTPUT
AS
  DECLARE @stringLength INT =
  (SELECT * FROM [dbo].[RandIntBetween2](@minLength, @maxLength, RAND()))
  SET @randomString = ''
  WHILE LEN(@randomString) < @stringLength
  BEGIN
    SET @randomString = @randomString + (SELECT * FROM [dbo].[PickRandomChar2](@chars, RAND()))
  END
RETURN 0
