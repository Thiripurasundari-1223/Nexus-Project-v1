USE [PMSNexus_Employees]
GO
TRUNCATE TABLE [dbo].[EmployeeAppConstants]
GO
SET IDENTITY_INSERT [dbo].[EmployeeAppConstants] ON 
Go
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 1,N'ExitType', N'Voluntary', N'Voluntary', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 2,N'ExitType', N'Involuntary', N'Involuntary', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 3,N'Qualification', N'10th', N'10th', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 4,N'Qualification', N'12th', N'12th', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 5,N'Qualification', N'UG', N'UG', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 6,N'Qualification', N'PG', N'PG', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 7,N'BloodGroup', N'A +ve', N'A+ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 8,N'BloodGroup', N'A -ve', N'A-ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 9,N'BloodGroup', N'B +ve', N'B+ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 10,N'BloodGroup', N'B -ve', N'B-ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 11,N'BloodGroup', N'AB +ve', N'AB+ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 12,N'BloodGroup', N'AB -ve', N'AB-ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 13,N'BloodGroup', N'O +ve', N'O+ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 14,N'BloodGroup', N'O -ve', N'O-ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 15,N'MaritalStatus', N'Single', N'Single', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 16,N'MaritalStatus', N'Married', N'Married', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 17,N'MaritalStatus', N'Divorced', N'Divorced', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 18,N'MaritalStatus', N'Widowed', N'Widowed', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 19,N'MaritalStatus', N'Other', N'Other', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES (20 ,N'SourceOfHire', N'Direct', N'Direct', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 21,N'SourceOfHire', N'Social Media', N'Social Media', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 22,N'SourceOfHire', N'Advertisement', N'Advertisement', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 23,N'SourceOfHire', N'External Referral', N'External Referral', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 24,N'SourceOfHire', N'Job Portal', N'Job Portal', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 25,N'SourceOfHire', N'Employee Referral', N'Employee Referral', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 26,N'SourceOfHire', N'Others', N'Others', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 27,N'SpecialAbility', N'Blindness', N'Blindness', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 28,N'SpecialAbility', N'Low-vision', N'Low-vision', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 29,N'SpecialAbility', N'Leprosy Cured persons', N'Leprosy Cured persons', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 30,N'SpecialAbility', N'Hearing Impairment (deaf and hard of hearing)', N'Hearing Impairment (deaf and hard of hearing)', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 31,N'SpecialAbility', N'Locomotor Disability', N'Locomotor Disability', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 32,N'SpecialAbility', N'Dwarfism', N'Dwarfism', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 33,N'SpecialAbility', N'Intellectual Disability', N'Intellectual Disability', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 34,N'SpecialAbility', N'Mental Illness', N'Mental Illness', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 35,N'SpecialAbility', N'Autism Spectrum Disorder', N'Autism Spectrum Disorder', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 36,N'SpecialAbility', N'Cerebral Palsy', N'Cerebral Palsy', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 37,N'SpecialAbility', N'Muscular Dystrophy', N'Muscular Dystrophy', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 38,N'SpecialAbility', N'Chronic Neurological conditions', N'Chronic Neurological conditions', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 39,N'SpecialAbility', N'Specific Learning Disabilities', N'Specific Learning Disabilities', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 40,N'SpecialAbility', N'Multiple Sclerosis', N'Multiple Sclerosis', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 41,N'SpecialAbility', N'Speech and Language disability', N'Speech and Language disability', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 42,N'SpecialAbility', N'Thalassemia', N'Thalassemia', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 43,N'SpecialAbility', N'Hemophilia', N'Hemophilia', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 44,N'SpecialAbility', N'Sickle Cell disease', N'Sickle Cell disease', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 45,N'SpecialAbility', N'Multiple Disabilities including deafblindness', N'Multiple Disabilities including deafblindness', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 46,N'SpecialAbility', N'Acid Attack victim', N'Acid Attack victim', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 47,N'SpecialAbility', N'Parkinson disease', N'Parkinson disease', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 48,N'SpecialAbility', N'Others', N'Others', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 49,N'Entity', N'TVS Next Limited', N'TVS Next Limited', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 50,N'Entity', N'TVS Next Inc', N'TVS Next Inc', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 51,N'Boards', N'State Board', N'State Board', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 52,N'Boards', N'CBSE', N'CBSE', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 53,N'Boards', N'ICSE', N'ICSE', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 54,N'Qualification', N'Diploma', N'Diploma', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 55,N'Qualification', N'Certification', N'Certification', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 56,N'ParentDepartmentId', N'Corporate', N'Corporate', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 57,N'ParentDepartmentId', N'Consulting', N'Consulting', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 58,N'ResignationStatus', N'Approved', N'Approved', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 59,N'ResignationStatus', N'Withdrawal Approved', N'Withdrawal Approved', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 60,N'ResignationStatus', N'Rejected', N'Rejected', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 61,N'ResignationStatus', N'Withdrawal Rejected', N'Withdrawal Rejected', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 62,N'ResignationStatus', N'Pending', N'Pending', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 63,N'ResignationStatus', N'Withdrawal Pending', N'Withdrawal Pending', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 64,N'ResignationStatus', N'Cancelled', N'Cancelled', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 65,N'CurrentWorkPlace', N'Office', N'Office', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 66,N'CurrentWorkPlace', N'Client Office', N'ClientOffice', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 67,N'CurrentWorkPlace', N'Remote', N'Remote', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 68,N'BloodGroup', N'A1 +ve', N'A1+ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 69,N'BloodGroup', N'A2 +ve', N'A2+ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 70,N'BloodGroup', N'A1B +ve', N'A1B+ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 71,N'BloodGroup', N'A2B +ve', N'A2B+ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 72,N'BloodGroup', N'A1 -ve', N'A1-ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 73,N'BloodGroup', N'A2 -ve', N'A2-ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 74,N'BloodGroup', N'A1B -ve', N'A1B-ve', 1, getdate())
GO
INSERT [dbo].[EmployeeAppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 75,N'BloodGroup', N'A2B -ve', N'A2B-ve', 1, getdate())
GO
SET IDENTITY_INSERT [dbo].[EmployeeAppConstants] OFF