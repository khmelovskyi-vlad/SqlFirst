CREATE PROCEDURE [dbo].[ShowSomeStudents]
	@studentsCount INT
AS
	SELECT TOP (@studentsCount) *
	FROM [dbo].[Student]
RETURN 0
