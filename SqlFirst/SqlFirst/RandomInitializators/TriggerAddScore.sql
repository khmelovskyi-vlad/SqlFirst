CREATE TRIGGER [TriggerAddScore]
ON [dbo].[Student]
FOR INSERT
AS EXTERNAL NAME INSERT #TempScores
		(Id, [Value], SubjectId, StudentId, CourseId)

--SomeAssembly.SomeType.SomeMethod
