﻿CREATE TABLE #TempSubjectSpecialty
(
	[SubjectName] NVARCHAR(50) NOT NULL, 
    [SpecialtyName] NVARCHAR(50) NOT NULL
)

INSERT INTO #TempSubjectSpecialty
([SubjectName], [SpecialtyName])
VALUES
('1', 'Economy'),
('1', 'Psychology'),
('1', 'Journalism'),
('2', 'Economy'),
('3', 'Economy'),
('4', 'Economy'),
('5', 'Economy'),
('6', 'Psychology'),
('7', 'Psychology'),
('8', 'Psychology'),
('9', 'Psychology'),
('10', 'Psychology'),
('11', 'Journalism'),
('12', 'Journalism'),
('13', 'Journalism'),
('14', 'Journalism'),
('15', 'Journalism'),
('16', 'Economy'),
('17', 'Economy'),
('18', 'Economy'),
('19', 'Economy'),
('20', 'Economy'),
('21', 'Psychology'),
('22', 'Psychology'),
('23', 'Psychology'),
('24', 'Psychology'),
('25', 'Psychology'),
('26', 'Journalism'),
('27', 'Journalism'),
('28', 'Journalism'),
('29', 'Journalism'),
('30', 'Journalism'),
('31', 'Economy'),
('32', 'Economy'),
('33', 'Economy'),
('34', 'Economy'),
('35', 'Economy'),
('36', 'Psychology'),
('37', 'Psychology'),
('38', 'Psychology'),
('39', 'Psychology'),
('40', 'Psychology'),
('41', 'Journalism'),
('42', 'Journalism'),
('43', 'Journalism'),
('44', 'Journalism'),
('45', 'Journalism'),
('46', 'Economy'),
('47', 'Economy'),
('48', 'Economy'),
('49', 'Economy'),
('50', 'Economy'),
('51', 'Psychology'),
('52', 'Psychology'),
('53', 'Psychology'),
('54', 'Psychology'),
('55', 'Psychology'),
('56', 'Journalism'),
('57', 'Journalism'),
('58', 'Journalism'),
('59', 'Journalism'),
('60', 'Journalism'),
('61', 'Economy'),
('62', 'Economy'),
('63', 'Economy'),
('64', 'Economy'),
('65', 'Economy'),
('66', 'Psychology'),
('67', 'Psychology'),
('68', 'Psychology'),
('69', 'Psychology'),
('70', 'Psychology'),
('71', 'Journalism'),
('72', 'Journalism'),
('73', 'Journalism'),
('74', 'Journalism'),
('75', 'Journalism'),
('76', 'Economy'),
('77', 'Economy'),
('78', 'Economy'),
('79', 'Economy'),
('80', 'Economy'),
('81', 'Psychology'),
('82', 'Psychology'),
('83', 'Psychology'),
('84', 'Psychology'),
('85', 'Psychology'),
('86', 'Journalism'),
('87', 'Journalism'),
('88', 'Journalism'),
('89', 'Journalism'),
('90', 'Journalism')

INSERT INTO [dbo].[SubjectSpecialty]
(SubjectId, SpecialtyId)
SELECT sub.Id, spec.Id
FROM #TempSubjectSpecialty ts
JOIN [dbo].[Subject] sub ON sub.[Name] = ts.SubjectName
JOIN [dbo].[Specialty] spec ON spec.[Name] = ts.SpecialtyName