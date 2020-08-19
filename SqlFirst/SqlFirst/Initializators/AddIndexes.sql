
--DROP INDEX ScoreIndex
--ON [dbo].[Score]
--DROP INDEX ScoreSubIndex
--ON [dbo].[Score]
--DROP INDEX ScoreIndex2
--ON [dbo].[Score]
CREATE INDEX ScoreIndex
ON [dbo].[Score] (StudentId)
INCLUDE ([Value], [SubjectId], [CourseId])
CREATE INDEX ScoreIndex2
ON [dbo].[Score] (StudentId)
INCLUDE ([Value], [SubjectId])
CREATE INDEX ScoreSubIndex
ON [dbo].[Score] (SubjectId)
INCLUDE ([Value], [StudentId], [CourseId])

DROP INDEX StudentIndex
ON [dbo].[Student]
DROP  INDEX StudentGrIndex
ON [dbo].[Student]
CREATE NONCLUSTERED INDEX StudentIndex
ON [dbo].[Student] (GroupId)
INCLUDE (FirstName, LastName, AverageScore)
CREATE NONCLUSTERED INDEX StudentGrIndex
ON [dbo].[Student] (GroupId)

--DROP INDEX GroupIndex
--ON [dbo].[Group]
--DROP INDEX GroupIndexSpecialty
--ON [dbo].[Group]
CREATE NONCLUSTERED INDEX GroupIndex
ON [dbo].[Group] (CourseId)
INCLUDE (SpecialtyId, [Name])
CREATE NONCLUSTERED INDEX GroupIndexSpecialty
ON [dbo].[Group] (SpecialtyId)
INCLUDE (CourseId, [Name])