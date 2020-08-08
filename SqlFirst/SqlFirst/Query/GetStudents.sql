SELECT s.Id, s.FirstName, s.LastName, g.Name
FROM [dbo].Student s
JOIN [dbo].[Group] g ON g.Id = s.GroupId