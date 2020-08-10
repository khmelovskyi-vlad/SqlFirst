SELECT s.Id, s.FirstName, s.LastName
FROM [dbo].Student s
WHERE s.AverageScore <
(SELECT AVG(st.AverageScore)
FROM [dbo].Student st)