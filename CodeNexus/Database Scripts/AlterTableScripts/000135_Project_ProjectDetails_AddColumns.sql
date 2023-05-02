USE [PMSNexus_Projects]

IF Not EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ProjectDetails' AND COLUMN_NAME = 'BillableTypeId' AND COLUMN_NAME = 'BillableFrequencyId'AND COLUMN_NAME = 'BillableValue')
BEGIN
  ALTER TABLE ProjectDetails Add BillableTypeId  int NULL
  ALTER TABLE ProjectDetails Add BillableFrequencyId  int NULL
  ALTER TABLE ProjectDetails Add BillableValue  int NULL
END
