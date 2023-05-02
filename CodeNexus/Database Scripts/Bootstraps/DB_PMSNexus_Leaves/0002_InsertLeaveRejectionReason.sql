TRUNCATE TABLE LeaveRejectionReason
GO
INSERT INTO LeaveRejectionReason(LeaveRejectionReasons, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy) VALUES('Data Mismatch', GETDATE(), 1, GETDATE(), 1)
INSERT INTO LeaveRejectionReason(LeaveRejectionReasons, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy) VALUES('Proof not submitted', GETDATE(), 1, GETDATE(), 1)
INSERT INTO LeaveRejectionReason(LeaveRejectionReasons, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy) VALUES('Invalid Proof', GETDATE(), 1, GETDATE(), 1)
INSERT INTO LeaveRejectionReason(LeaveRejectionReasons, CreatedOn, CreatedBy, ModifiedOn, ModifiedBy) VALUES('Others', GETDATE(), 1, GETDATE(), 1)
GO