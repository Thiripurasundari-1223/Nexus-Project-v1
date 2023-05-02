TRUNCATE TABLE AccountRelatedIssue
GO
INSERT INTO AccountRelatedIssue(AccountRelatedIssueReason, CreatedOn, CreatedBy) VALUES('Payment Tenure', GETDATE(), 1)
INSERT INTO AccountRelatedIssue(AccountRelatedIssueReason, CreatedOn, CreatedBy) VALUES('Reimbursement of Expenses', GETDATE(), 1)
INSERT INTO AccountRelatedIssue(AccountRelatedIssueReason, CreatedOn, CreatedBy) VALUES('Ramp Up Time', GETDATE(), 1)
INSERT INTO AccountRelatedIssue(AccountRelatedIssueReason, CreatedOn, CreatedBy) VALUES('Other', GETDATE(), 1)
GO