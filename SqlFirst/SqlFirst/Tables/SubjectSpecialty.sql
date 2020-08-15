CREATE TABLE [dbo].[SubjectSpecialty]
(
	[SubjectId] UNIQUEIDENTIFIER NOT NULL , 
    [SpecialtyId] UNIQUEIDENTIFIER NOT NULL, 
    PRIMARY KEY ([SubjectId], [SpecialtyId])
)
