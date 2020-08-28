CREATE TRIGGER [AddStudentAverageScoresTrigger]
ON [dbo].[Score]  
AFTER INSERT, DELETE, UPDATE
AS 
BEGIN
	UPDATE [dbo].[Student]
	SET AverageScore = (
	SELECT AVG(sc.[Value])
	FROM inserted i
	JOIN [dbo].[Score] sc ON sc.StudentId = i.StudentId
	WHERE sc.StudentId = [dbo].[Student].Id)
END