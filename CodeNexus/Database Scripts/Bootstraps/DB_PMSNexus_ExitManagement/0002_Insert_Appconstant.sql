USE [PMSNexus_ExitManagement]
GO
TRUNCATE TABLE [dbo].[AppConstants]
GO
SET IDENTITY_INSERT [dbo].[AppConstants] ON 
Go
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 1,N'ResignationApproval', N'Reporting To', N'ReportingTo', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 2,N'ResignationApproval', N'Department BUHead', N'DepartmentBUHead', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 3,N'ResignationApproval', N'HR BUHead', N'HRBUHead', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 4,N'ResignationApproval', N'Others', N'Others', 1, getdate())
GO

INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 5,N'ResignationInterview', N'Excellent', N'Excellent', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 6,N'ResignationInterview', N'Good', N'Good', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 7,N'ResignationInterview', N'Average', N'Average', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 8,N'ResignationInterview', N'Poor', N'Poor', 1, getdate())
GO

INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 9,N'ReasonRelievingPosition', N'Move/Relocation', N'MoveRelocation', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 10,N'ReasonRelievingPosition', N'Family and Personal Reasons', N'FamilyPersonal', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 11,N'ReasonRelievingPosition', N'Limited career advancement opportunities', N'LimitedCareerOpportunities', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 12,N'ReasonRelievingPosition', N'Unhappy with job duties', N'UnhappyDuties', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 13,N'ReasonRelievingPosition', N'Inadequate benefits', N'InadequateBenefits', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 14,N'ReasonRelievingPosition', N'Unhappy with supervision', N'UnhappySupervision', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 15,N'ReasonRelievingPosition', N'Not Satisifed', N'NotSatisifed', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 16,N'ReasonRelievingPosition', N'Unhappy with working conditions', N'UnhappyWorkingConditions', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 17,N'ReasonRelievingPosition', N'Lack of recognition', N'LackOfRecognition', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 18,N'ReasonRelievingPosition', N'Career Opportunity', N'CareerOpportunity', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 19,N'ReasonRelievingPosition', N'Other', N'Other', 1, getdate())
GO

INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 20,N'ExitCheckListLetter', N'
   <p>Subsequent to my resignation, I am aware that the following declaration is part of the relieving process and I hereby declare the following: </p>
    <p class="m-0">1. I have surrendered all properties belonging to the company.</p>
    <p  class="m-0">2. I have returned all information including the documents pertaining to company that I was dealing with.</p>
    <p class="m-0">3. I will not contact my colleagues on official matters after I cease to be a TVS Next employee.</p>
    <p class="m-0">4. I will not call my colleagues and ask information on projects, customers or team members,</p>
    <p class="m-0">5. I will not try to recruit my colleagues of TVS Next Pvt Ltd. </p>
    <p class="m-0">6. I will not disclose the names of the customers / projects or names of my colleagues to my new employer</p>
    <p class="m-0">7. I will not contact TVS Next customers to my prospective employer(s).</p>
    <p class="m-0">8. I will not hand over copies of processes, policies and other templates followed at TVS Next to my prospective employer(s).</p>
    <p class="m-0">9. I will not inform customers of my status unless approved by supervisor/HR</p>
    <p class="m-0">10. I further hereby undertake to the following:</p>
   <p>Non-solicit / Non- compete </p>
  <p> 1. Non-compete: During the term and for a period of 2 years thereafter (“Non-Compete Period”), the Employee I shall not, directly or indirectly, enter into or in any manner take part in any business, either individually or as an officer, director, executive, agent, consultant, partner, investor principal or otherwise, which is in competition with the business of the Company or any of its affiliates in any business segment or field in which the Company or its affiliates is engaged in any jurisdiction in which the Company or its affiliates is so engaged or the Company’s board of directors has approved a plan for the Company to become so engaged. Furthermore, I acknowledge that I have been privy to Confidential Information including process, trade secrets, business strategies, technologies, unique practices which are specific to the Company’s Customers and the nature of services rendered there. Therefore, I shall not, during the Non-Compete Period, engage in any manner of work / services for the Company’s Customers whether independently or as an employee of any other company/ organization. The Employee I will further not engage in any other activities that conflict with my obligations to the Company.</p>
  <p>2. I will the Employee is required to promptly disclose to the Company all outside activities or interests, including ownership or participation in the development of prior inventions, that conflict or may conflict with the best interests of the Company.</p>
  <p>3. Non-Solicit: During the Term and for 2 years thereafter, the Employee I shall not, directly, or indirectly: </p>
  <p class="mb-0">3.1. directly or indirectly, solicit or attempt to solicit, hire any of Company’s (or any of its affiliate’s) customers or clients for my Employee’s personal benefit, induce, advise or encourage any supplier, customer, client, employee or any other person, firm, partnership, association, trust, venture, corporation, or business organization, entity or enterprise having business dealings with Company or any subsidiary or affiliate of Company, to withdraw, curtail or cancel such business dealings. </p>
  <p class="mt-1">3.2. Initiate the act of solicitation for the purpose of recruitment of any person who is in the employment of the Company or assist another in this act or otherwise interfere with Company’s (or any affiliate of Company’s) lawful employment relationship with any of its employees.</p>
  <p>4. Reasonableness of Restrictions. I The Employee agrees and acknowledges that the covenants set forth in this Clause do not impose unreasonable restrictions or work hardship on me the Employee and are essential to the willingness of the Company to employ me the Employee, are necessary and fundamental to the protection of the business conducted by the Company and are reasonable as to scope, duration, and territory. I The Employee further waives any right to assert inadequacy of consideration as a defense to enforcement of the covenants set forth under this Undertaking Agreement.</p>
  <p>5. If any breach or violation of the provisions of this Clause occurs, I the Employee agrees and acknowledges that damages alone are not likely to be sufficient compensation, and that injunctive relief (in addition to any other remedies afforded by a court of equity) is reasonable and is likely to be essential to safeguard the interests of the Company, which may (subject to the discretion of the courts) be obtained. </p>
  <p>6. I have been trained on specific Projects/Services/Products at TVS Next and I am acquainted with Confidential Information including process, trade secrets, business strategies, technologies, unique practices of TVS Next and its Customers during my employment at TVS Next, which gives uniqueness that distinguishes TVS Next from its competitors. I am aware and acknowledge that my joining any competitor of TVS Next will mean would lead to disclosure of Confidential Information of TVS Next and its Customers, which will be breach of this Undertaking / Declaration. In event of such breach by me, I shall be liable to indemnify for the loss or damage caused to TVS Next including for loss of business. </p>
  <p>6.a) “Confidential Information” shall mean and include all information of TVS Next and its Customers, which I learnt of in connection with my employment with TVS Next. Confidential Information shall include, without limitation: (1) TVS Next’s business policies, finances, and business plans; (2) financial projections, including but not limited to, annual sales forecasts and targets and any computation(s) of the market share of Customers and/or Customer Prospects; (3) sales information relating to TVS Next’s product/services roll-outs; (4) customized software, marketing tools, and/or supplies that I had access; (5) the identity of the TVS Next Customers, Customer Prospects, and/or Vendors any list(s) (6) the account terms and pricing upon which the TVS Next obtains products and services from its Vendors; (7) the account terms and pricing of sales and pricing contracts between TVS Next and its Customers (8) the names and addresses of the TVS Next’s employees and other business contacts of TVS Next; (9) the techniques, methods, and strategies by which the TVS Next develops, services, markets, and/or sells any of the services/products and (10) all inventions, discoveries, developments, methods, processes, compositions, works, data (including information relating to the generation and updating thereof), concepts, and ideas (whether or not patentable or copyrightable) conceived, made, developed, created, or reduced to practice (whether at the request or suggestion of TVS Next or otherwise, whether alone or in conjunction with others, and whether during regular hours of work or otherwise) during my your employment, which may be directly or indirectly useful in, or related to, the Business of TVS Next or any business, Service or Products contemplated by TVS Next while I was are an employee at TVS Next. </p>
  <p>7. I acknowledge that I have carefully read and understand declaration and understand that I have the right to seek independent advice at my expense or to propose modifications prior to signing this declaration. I represent I have signed this declaration voluntarily and after consulting professional advisors. </p>
  <p>Arbitration. Any dispute relating to this Undertaking shall be referred to Arbitration under Annexure – A (attached) </p>
  <p>I understand that the covenants under this Undertaking will survive mythe resignation, exit or retirement from employment at TVS Next and that failure to keep confidential information is grounds for appropriate legal action against me for breach. </p>
  <p>By my signature I acknowledge that I have read and will abide by this Undertaking - Confidentiality, Non – Compete and Non – Solicitation.</p>
', N'Exit checklist letter', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 21,N'ExitCheckList', N'Yes', N'Yes', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 22,N'ExitCheckList', N'No', N'No', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 23,N'ExitCheckList', N'NA', N'NA', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 24,N'ManagerMailData', N'Disabled', N'Disabled', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 25,N'ManagerMailData', N'Routed To', N'RoutedTo', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 26,N'ITMailData', N'Disabled', N'Disabled', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 27,N'ITMailData', N'Routed', N'Routed', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 28,N'ELPayData', N'Payable', N'Payable', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 29,N'ELPayData', N'Recoverable', N'Recoverable', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 30,N'ELPayData', N'Not Eligible', N'Not Eligible', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 31,N'CheckListSubmission', N'Submitted', N'Submitted', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 32,N'CheckListSubmission', N'Not Submitted', N'Not Submitted', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 33,N'CheckListSubmission', N'NA', N'NA', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 34,N'NoticePayData', N'Payable', N'Payable', 1, getdate())
GO
INSERT [dbo].[AppConstants] ([AppConstantId], [AppConstantType], [DisplayName], [AppConstantValue], [CreatedBy], [CreatedOn]) VALUES ( 35,N'NoticePayData', N'Recoverable', N'Recoverable', 1, getdate())
GO
SET IDENTITY_INSERT [dbo].[AppConstants] OFF