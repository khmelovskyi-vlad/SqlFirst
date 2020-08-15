CREATE VIEW [dbo].[SmallSubjectScoreWithGroupBy]
	AS SELECT sub.Id, sub.[Name], st.FirstName, st.LastName, score.[Value]
FROM(SELECT sub.Id, AVG(score.[Value]) AS Average
FROM [dbo].StudentScore score
JOIN [dbo].[Subject] sub ON sub.Id = score.SubjectId
GROUP BY sub.Id) subAVG
JOIN [dbo].[Subject] sub ON sub.Id = subAVG.Id
JOIN [dbo].StudentScore score ON score.SubjectId = sub.Id
JOIN [dbo].[Student] st ON st.Id = score.StudentId
WHERE score.[Value] < subAVG.Average