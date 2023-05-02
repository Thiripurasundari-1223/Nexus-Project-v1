TRUNCATE TABLE ChangeRequestType
GO

INSERT INTO ChangeRequestType(ChangeRequestTypeDescription, CreatedOn, CreatedBy) VALUES('Change Request Type 1', GETDATE(), 1)
INSERT INTO ChangeRequestType(ChangeRequestTypeDescription, CreatedOn, CreatedBy) VALUES('Change Request Type 2', GETDATE(), 1)
INSERT INTO ChangeRequestType(ChangeRequestTypeDescription, CreatedOn, CreatedBy) VALUES('Change Request Type 3', GETDATE(), 1)
GO
