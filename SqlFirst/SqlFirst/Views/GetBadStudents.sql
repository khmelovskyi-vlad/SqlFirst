--CREATE VIEW [dbo].[GetBadStudents]
--	AS SELECT s.Id, s.FirstName, s.LastName, g.[Name]
--FROM [dbo].Student s
--JOIN [dbo].[Group] g ON g.Id = s.GroupId
--WHERE s.AverageScore <
--(SELECT AVG(st.AverageScore)
--FROM [dbo].Student st)
CREATE VIEW [dbo].Viev_StudentScoresCount
	AS SELECT s.Id, s.FirstName, s.LastName, COUNT(s.Id) AS ScoreCount
FROM [dbo].Student s
JOIN [dbo].[Score] score ON score.StudentId = s.Id
GROUP BY s.Id, s.FirstName, s.LastName