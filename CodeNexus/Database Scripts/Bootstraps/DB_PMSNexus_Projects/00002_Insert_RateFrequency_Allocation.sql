
TRUNCATE TABLE Allocation
GO

INSERT INTO Allocation(AllocationDescription, CreatedOn, CreatedBy) VALUES('10%', GETDATE(), 1)
INSERT INTO Allocation(AllocationDescription, CreatedOn, CreatedBy) VALUES('25%', GETDATE(), 1)
INSERT INTO Allocation(AllocationDescription, CreatedOn, CreatedBy) VALUES('50%', GETDATE(), 1)
INSERT INTO Allocation(AllocationDescription, CreatedOn, CreatedBy) VALUES('75%', GETDATE(), 1)
INSERT INTO Allocation(AllocationDescription, CreatedOn, CreatedBy) VALUES('100%', GETDATE(), 1)
GO

TRUNCATE TABLE RateFrequency
GO

INSERT INTO RateFrequency(RateFrequencyDescription, CreatedOn, CreatedBy) VALUES('Hourly', GETDATE(), 1)
INSERT INTO RateFrequency(RateFrequencyDescription, CreatedOn, CreatedBy) VALUES('Daily', GETDATE(), 1)
INSERT INTO RateFrequency(RateFrequencyDescription, CreatedOn, CreatedBy) VALUES('Monthly', GETDATE(), 1)
GO