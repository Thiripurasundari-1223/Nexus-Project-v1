USE [PMSNexus_ExitManagement]

BEGIN --Manager , PMO -> Manager
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ManagerCheckList' AND COLUMN_NAME = 'KnowledgeTransferId')
	BEGIN
	  ALTER TABLE ManagerCheckList Alter column [KnowledgeTransferId] varchar(10) 
	END
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ManagerCheckList' AND COLUMN_NAME = 'MailID')
	BEGIN
	  ALTER TABLE ManagerCheckList Alter column [MailID] varchar(10) 
	END
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ManagerCheckList' AND COLUMN_NAME = 'ProjectDocumentsReturnedId')
	BEGIN
	  ALTER TABLE ManagerCheckList Alter column [ProjectDocumentsReturnedId] varchar(10) 
	END
	IF EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ManagerCheckList' AND COLUMN_NAME = 'RecoverPayNoticeId')
	BEGIN
	  ALTER TABLE ManagerCheckList Alter column [RecoverPayNoticeId] varchar(10) 
	END

	IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ManagerCheckList' AND COLUMN_NAME = 'TimesheetsId')
	BEGIN
	  ALTER TABLE ManagerCheckList Add  [TimesheetsId] varchar(10) 
	END

	IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ManagerCheckList' AND COLUMN_NAME = 'TimesheetsRemark')
	BEGIN
	  ALTER TABLE ManagerCheckList Add  [TimesheetsRemark] nvarchar(4000) 
	END
END
GO

BEGIN --HR -> People Experience
	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='HRCheckList' AND COLUMN_NAME = 'NoticePayId')
	BEGIN
	  ALTER TABLE HRCheckList Alter column [NoticePayId] varchar(10) 
	END	
	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='HRCheckList' AND COLUMN_NAME = 'ELBalanceId')
	BEGIN
	  ALTER TABLE HRCheckList Alter column [ELBalanceId] varchar(10) 
	END
	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='HRCheckList' AND COLUMN_NAME = 'NoticePeriodWaiverRequestId')
	BEGIN
	  ALTER TABLE HRCheckList Alter column [NoticePeriodWaiverRequestId] varchar(10) 
	END
	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='HRCheckList' AND COLUMN_NAME = 'LeaveBalanceSummaryId')
	BEGIN
	  ALTER TABLE HRCheckList Alter column [LeaveBalanceSummaryId] varchar(10) 
	END
	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='HRCheckList' AND COLUMN_NAME = 'RehireEligibleId')
	BEGIN
	  ALTER TABLE HRCheckList Alter column [RehireEligibleId] varchar(10) 
	END

	
	
END
GO

BEGIN --Admin -> HR Partner

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AdminCheckList' AND COLUMN_NAME = 'IdentityCardId')
	BEGIN
	  ALTER TABLE AdminCheckList Alter column [IdentityCardId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AdminCheckList' AND COLUMN_NAME = 'CabinKeysID')
	BEGIN
	  ALTER TABLE AdminCheckList Alter column [CabinKeysID] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AdminCheckList' AND COLUMN_NAME = 'TravelCardId')
	BEGIN
	  ALTER TABLE AdminCheckList Alter column [TravelCardId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AdminCheckList' AND COLUMN_NAME = 'BusinessCardsId')
	BEGIN
	  ALTER TABLE AdminCheckList Alter column [BusinessCardsId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AdminCheckList' AND COLUMN_NAME = 'LibraryBooksId')
	BEGIN
	  ALTER TABLE AdminCheckList Alter column [LibraryBooksId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AdminCheckList' AND COLUMN_NAME = 'CompanyMobileId')
	BEGIN
	  ALTER TABLE AdminCheckList Alter column [CompanyMobileId] varchar(10) 
	END

	IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AdminCheckList' AND COLUMN_NAME = 'BiometricAccessTerminationId')
	BEGIN
	  ALTER TABLE AdminCheckList add  [BiometricAccessTerminationId] varchar(10) 
	END
	IF NOT EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='AdminCheckList' AND COLUMN_NAME = 'BiometricAccessTerminationRemark')
	BEGIN
	  ALTER TABLE AdminCheckList add  [BiometricAccessTerminationRemark] varchar(max) 
	END
END
GO

BEGIN --IT List

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ITCheckList' AND COLUMN_NAME = 'LoginDisabledId')
	BEGIN
	  ALTER TABLE ITCheckList Alter column [LoginDisabledId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ITCheckList' AND COLUMN_NAME = 'MailID')
	BEGIN
	  ALTER TABLE ITCheckList Alter column [MailID] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ITCheckList' AND COLUMN_NAME = 'BiometricAccessTerminationId')
	BEGIN
	  ALTER TABLE ITCheckList Alter column [BiometricAccessTerminationId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ITCheckList' AND COLUMN_NAME = 'SystemAssetsRecoveredId')
	BEGIN
	  ALTER TABLE ITCheckList Alter column [SystemAssetsRecoveredId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ITCheckList' AND COLUMN_NAME = 'DATAcardReturnedId')
	BEGIN
	  ALTER TABLE ITCheckList Alter column [DATAcardReturnedId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ITCheckList' AND COLUMN_NAME = 'DamageRecoveryId')
	BEGIN
	  ALTER TABLE ITCheckList Alter column [DamageRecoveryId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ITCheckList' AND COLUMN_NAME = 'MacAddressRemovalId')
	BEGIN
	  ALTER TABLE ITCheckList Alter column [MacAddressRemovalId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='ITCheckList' AND COLUMN_NAME = 'DataBackUpId')
	BEGIN
	  ALTER TABLE ITCheckList Alter column [DataBackUpId] varchar(10) 
	END
END
GO


BEGIN --Finance List

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='FinanceCheckList' AND COLUMN_NAME = 'ITProofsId')
	BEGIN
	  ALTER TABLE FinanceCheckList Alter column [ITProofsId] varchar(10) 
	END

	IF  EXISTS (SELECT 1 FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME='FinanceCheckList' AND COLUMN_NAME = 'GratuityEligibilityId')
	BEGIN
	  ALTER TABLE FinanceCheckList Alter column [GratuityEligibilityId] varchar(10) 
	END
END
GO