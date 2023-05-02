USE [PMSNexus_ExitManagement]
GO
TRUNCATE TABLE ResignationReason
GO
INSERT INTO ResignationReason(ResignationReasonName,  CreatedBy, CreatedOn) VALUES('CareerGrowth', 1, GETDATE())
INSERT INTO ResignationReason(ResignationReasonName,  CreatedBy, CreatedOn) VALUES('Better Prospects', 1, GETDATE())
INSERT INTO ResignationReason(ResignationReasonName,  CreatedBy, CreatedOn) VALUES('Personal', 1, GETDATE())
INSERT INTO ResignationReason(ResignationReasonName,  CreatedBy, CreatedOn) VALUES('Health Issues', 1, GETDATE())
INSERT INTO ResignationReason(ResignationReasonName,  CreatedBy, CreatedOn) VALUES('Abscond', 1, GETDATE())
INSERT INTO ResignationReason(ResignationReasonName,  CreatedBy, CreatedOn) VALUES('Performance Issue', 1, GETDATE())
INSERT INTO ResignationReason(ResignationReasonName,  CreatedBy, CreatedOn) VALUES('Gross misconduct', 1, GETDATE())
INSERT INTO ResignationReason(ResignationReasonName,  CreatedBy, CreatedOn) VALUES('Others', 1, GETDATE())
GO
