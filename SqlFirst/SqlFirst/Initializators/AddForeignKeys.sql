ALTER TABLE [dbo].[Student]
ADD FOREIGN KEY ([GroupId]) REFERENCES [dbo].[Group](Id)

ALTER TABLE [dbo].[Group]
ADD FOREIGN KEY (CourseId) REFERENCES [dbo].[Course](Id)
ALTER TABLE [dbo].[Group]
ADD FOREIGN KEY (SpecialtyId) REFERENCES [dbo].[Specialty](Id)

ALTER TABLE [dbo].[Score]
ADD FOREIGN KEY (SubjectId) REFERENCES [dbo].[Subject](Id)
ALTER TABLE [dbo].[Score]
ADD FOREIGN KEY (StudentId) REFERENCES [dbo].[Student](Id)
ALTER TABLE [dbo].[Score]
ADD FOREIGN KEY (CourseId) REFERENCES [dbo].[Course](Id)

--ALTER TABLE [dbo].[SubjectCourse]
--ADD FOREIGN KEY (SubjectId) REFERENCES [dbo].[Subject](Id)
--ALTER TABLE [dbo].[SubjectCourse]
--ADD FOREIGN KEY (CourseId) REFERENCES [dbo].[Course](Id)

--ALTER TABLE [dbo].[SubjectSpecialty]
--ADD FOREIGN KEY (SubjectId) REFERENCES [dbo].[Subject](Id)
--ALTER TABLE [dbo].[SubjectSpecialty]
--ADD FOREIGN KEY (SpecialtyId) REFERENCES [dbo].[Specialty](Id)