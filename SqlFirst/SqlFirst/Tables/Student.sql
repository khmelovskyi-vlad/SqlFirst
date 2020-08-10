﻿CREATE TABLE [dbo].[Student]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [FirstName] NVARCHAR(50) NOT NULL, 
    [LastName] NVARCHAR(50) NOT NULL, 
    [GroupId] UNIQUEIDENTIFIER NOT NULL, 
    [AverageScore] INT NULL 
)
