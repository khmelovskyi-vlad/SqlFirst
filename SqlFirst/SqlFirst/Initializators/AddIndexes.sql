DROP INDEX IX_Student_GroupId
ON [dbo].[Student]
DROP INDEX IX_Score_StudentId
ON [dbo].[Score]
DROP INDEX IX_Group_CourseId
ON [dbo].[Group]

CREATE INDEX IX_Student_GroupId
ON [dbo].[Student] (GroupId)
CREATE INDEX IX_Score_StudentId
ON [dbo].[Score] (StudentId)
INCLUDE ([Value], [SubjectId])
CREATE INDEX IX_Group_CourseId
ON [dbo].[Group] (CourseId)
INCLUDE (SpecialtyId)



DROP INDEX IX_Score_StudentId2
ON [dbo].[Score]
DROP INDEX IX_Student_GroupId2
ON [dbo].[Student]


CREATE INDEX IX_Score_StudentId2
ON [dbo].[Score] (StudentId)
INCLUDE ([Value], [SubjectId], [CourseId])
CREATE INDEX IX_Student_GroupId2
ON [dbo].[Student] (GroupId)
INCLUDE ([FirstName], [LastName])


DROP INDEX IX_Score_Value
ON [dbo].[Score]
CREATE INDEX IX_Score_Value
ON [dbo].[Score] ([Value])
INCLUDE (StudentId, [SubjectId], [CourseId])


DROP INDEX IX_Student_AverageScore
ON [dbo].[Student]
DROP INDEX IX_Student_AverageScore2
ON [dbo].[Student]

CREATE INDEX IX_Student_AverageScore
ON [dbo].[Student] (AverageScore)
INCLUDE ([FirstName], [LastName], GroupId)
CREATE INDEX IX_Student_AverageScore2
ON [dbo].[Student] (AverageScore)