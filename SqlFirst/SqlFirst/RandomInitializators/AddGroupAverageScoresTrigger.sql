CREATE TRIGGER [AddGroupAverageScoresTrigger]
ON [dbo].[Score]  
AFTER INSERT, DELETE, UPDATE
AS 
BEGIN
	DECLARE @groupId UNIQUEIDENTIFIER = 
	(SELECT st.GroupId
	FROM inserted i
	JOIN [dbo].[Student] st ON st.Id = i.StudentId)

	UPDATE [dbo].[Group]
	SET AverageScore = (
	SELECT AVG(st.[AverageScore])
	FROM [dbo].[Student] st 
	WHERE st.GroupId = @groupId)
	WHERE [dbo].[Group].Id = @groupId
END