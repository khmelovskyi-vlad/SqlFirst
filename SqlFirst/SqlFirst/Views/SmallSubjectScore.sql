CREATE VIEW [dbo].[SmallSubjectScore]
	AS SELECT sub.Id, sub.[Name], st.FirstName, st.LastName, score.[Count]
FROM [dbo].StudentScore score 
JOIN [dbo].[Subject] sub ON sub.Id = score.SubjectId
JOIN [dbo].Student st ON st.Id = score.StudentId
WHERE score.[Count] <
(SELECT AVG(sc.[Count])
FROM [dbo].StudentScore sc
WHERE score.SubjectId = sc.SubjectId)
GROUP BY sub.[Name], sub.Id, st.FirstName, st.LastName, score.[Count]