﻿CREATE TABLE [dbo].[Group]
(
	[Id] UNIQUEIDENTIFIER NOT NULL PRIMARY KEY, 
    [Name] NVARCHAR(60) NOT NULL, 
    [AverageScore] INT NULL, 
    [CourseId] UNIQUEIDENTIFIER NOT NULL, 
    [SpecialtyId] UNIQUEIDENTIFIER NOT NULL
)
