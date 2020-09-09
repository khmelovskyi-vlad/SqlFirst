--CREATE TRIGGER [AddGroupAverageScoresTrigger]
--ON [dbo].[Score]  
--AFTER INSERT, DELETE, UPDATE
--AS 
--BEGIN
--	DECLARE @groupIds TABLE
--	(
--		Id UNIQUEIDENTIFIER
--	)
--	INSERT INTO @groupIds
--	SELECT st.GroupId
--	FROM inserted i
--	JOIN [dbo].[Student] st ON st.Id = i.StudentId

--	UPDATE [dbo].[Group]
--	SET AverageScore = (
--	SELECT AVG(st.[AverageScore])
--	FROM [dbo].[Student] st 
--	WHERE st.GroupId = @groupIds)
--	WHERE [dbo].[Group].Id = @groupIds
--END
CREATE TRIGGER [AddGroupAverageScoresTrigger]
                          ON [dbo].[Score]
                          AFTER INSERT, DELETE, UPDATE
                          AS
                          BEGIN
                               DECLARE @groupIds TABLE
                               (
                                   Id UNIQUEIDENTIFIER
                               )
                               INSERT INTO @groupIds
                               SELECT st.GroupId
                               FROM inserted i
                               JOIN [dbo].[Student] st ON st.Id = i.StudentId

                               UPDATE [dbo].[Group]
                               SET AverageScore = (

                               SELECT AVG(st.[AverageScore])
                               FROM [dbo].[Student] st
							   JOIN @groupIds g ON g.Id = [dbo].[Group].Id 
                               WHERE st.GroupId = [dbo].[Group].Id)

							   FROM [dbo].[Group] gr
							   JOIN [dbo].[Student] st ON st.GroupId = gr.Id
                               JOIN inserted i ON i.StudentId = st.Id
                          END