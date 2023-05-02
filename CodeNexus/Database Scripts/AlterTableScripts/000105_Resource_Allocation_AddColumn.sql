USE [PMSNexus_Projects]
IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ResourceAllocation' AND COLUMN_NAME = 'IsAdditionalResource')
BEGIN
   ALTER TABLE ResourceAllocation Add IsAdditionalResource bit NULL;
END