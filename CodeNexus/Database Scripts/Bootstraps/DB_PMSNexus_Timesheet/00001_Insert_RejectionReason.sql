TRUNCATE TABLE RejectionReason
GO
INSERT INTO RejectionReason(ReasonForRejection, CreatedOn, CreatedBy) VALUES('Incorrect No. of Hours', GETDATE(), 1)
INSERT INTO RejectionReason(ReasonForRejection, CreatedOn, CreatedBy) VALUES('Entry During Leaves/Holidays', GETDATE(), 1)
INSERT INTO RejectionReason(ReasonForRejection, CreatedOn, CreatedBy) VALUES('Others', GETDATE(), 1)
GO