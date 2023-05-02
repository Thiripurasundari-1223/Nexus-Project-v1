using ExcelDataReader;
using IAM.DAL.Repository;
using Newtonsoft.Json;
using SharedLibraries.Models.Employee;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employee;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.ExitManagement;
using SharedLibraries.ViewModels.Home;
using SharedLibraries.ViewModels.Leaves;
using SharedLibraries.ViewModels.Notifications;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using SharedLibraries.Common;
using SharedLibraries.Models.Accounts;
using SharedLibraries.ViewModels.PolicyManagement;
using SharedLibraries;
using Microsoft.IdentityModel.Tokens;

namespace IAM.DAL.Services
{
    public class EmployeeService
    {
        private readonly IEmployeeRepository _employeeRepository;
        private readonly IDepartmentRepository _departmentRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IEmployeesSkillsetRepository _employeesSkillsetRepository;
        private readonly ISkillsetRepository _skillsetRepository;
        private readonly IRolePermissionRepository _rolePermissionRepository;
        private readonly IEmployeeTypeRepository _employeeTypeRepository;
        private readonly IEmployeeCategoryRepository _employeeCategoryRepository;
        private readonly IEmployeeDependentRepository _employeeDependentRepository;
        private readonly IEmployeeShiftDetailsRepository _employeeShiftDetailsRepository;
        private readonly IProbationStatusRepository _probationStatusRepository;
        private readonly ISystemRoleRepository _systemRoleRepository;
        private readonly IEmployeeRelationshipRepository _employeeRelationshipRepository;
        private readonly IWorkHistoryRepository _workHistoryRepository;
        private readonly IEducationDetailrepository _educationDetailRepository;
        private readonly ICompensationDetailRepository _compensationDetailRepository;
        private readonly IEmployeeDocumentRepository _employeeDocumentRepository;
        private readonly IEmployeeAppConstantRepository _appConstantRepository;
        private readonly IEmployeesPersonalInfoRepository _employeesPersonalInfoRepository;
        private readonly ICountryRepository _countryRepository;
        private readonly IStateRepository _stateRepository;
        private readonly IDesignationRepository _designationRepository;
        private readonly ISkillsetHistoryRepository _skillsetHistoryRepository;
        private readonly IEmployeeRequestRepository _employeeRequestRepository;
        private readonly IEmployeeDesignationHistoryRepository _employeeDesignationHistoryRepository;
        private readonly IEmployeeNationalityRepository _employeeNationalityRepository;
        private readonly IAuditRepository _auditRepository;
        private readonly IEmployeeSpecialAbilityRepository _employeeSpecialAbilityRepository;
        private readonly IEmployeeRequestDetailsRepository _employeeRequestDetailsRepository;
        private readonly IEmployeeRequestDocumentRepository _employeeRequestDocumentRepository;
        private readonly IEmployeeLocationRepository _employeeLocationRepository;
        public EmployeeService(IEmployeeRepository employeeRepository, IDepartmentRepository departmentRepository,
            IRoleRepository roleRepository, IEmployeesSkillsetRepository employeesSkillsetRepository, ISkillsetRepository skillsetRepository,
            IEmployeeTypeRepository employeeTypeRepository,
            IRolePermissionRepository rolePermissionRepository, IEmployeeCategoryRepository employeeCategoryRepository,
            IEmployeeDependentRepository employeeDependentRepository, IEmployeeShiftDetailsRepository employeeShiftDetailsRepository, IProbationStatusRepository probationStatusRepository, ISystemRoleRepository systemRoleRepository, IEmployeeRelationshipRepository employeeRelationshipRepository,
            IWorkHistoryRepository workHistoryRepository, IEducationDetailrepository educationDetailrepository, ICompensationDetailRepository compensationDetailRepository, IEmployeeDocumentRepository documentRepository, IEmployeeAppConstantRepository appConstantRepository, IEmployeesPersonalInfoRepository employeesPersonalInfoRepository, IStateRepository stateRepository, ICountryRepository countryRepository,
            IDesignationRepository designationRepository, ISkillsetHistoryRepository skillsetHistoryRepository, IEmployeeRequestRepository employeeRequestRepository, IEmployeeDesignationHistoryRepository employeeDesignationHistoryRepository, IEmployeeNationalityRepository employeeNationalityRepository, IAuditRepository auditRepository, IEmployeeSpecialAbilityRepository employeeSpecialAbilityRepository,
            IEmployeeRequestDetailsRepository employeeRequestDetailsRepository, IEmployeeRequestDocumentRepository employeeRequestDocumentRepository, IEmployeeLocationRepository employeeLocationRepository)
        {
            _employeeRepository = employeeRepository;
            _departmentRepository = departmentRepository;
            _roleRepository = roleRepository;
            _employeesSkillsetRepository = employeesSkillsetRepository;
            _skillsetRepository = skillsetRepository;
            _rolePermissionRepository = rolePermissionRepository;
            _employeeTypeRepository = employeeTypeRepository;
            _employeeCategoryRepository = employeeCategoryRepository;
            _employeeDependentRepository = employeeDependentRepository;
            _employeeShiftDetailsRepository = employeeShiftDetailsRepository;
            _probationStatusRepository = probationStatusRepository;
            _systemRoleRepository = systemRoleRepository;
            _employeeRelationshipRepository = employeeRelationshipRepository;
            _workHistoryRepository = workHistoryRepository;
            _educationDetailRepository = educationDetailrepository;
            _compensationDetailRepository = compensationDetailRepository;
            _employeeDocumentRepository = documentRepository;
            _appConstantRepository = appConstantRepository;
            _employeesPersonalInfoRepository = employeesPersonalInfoRepository;
            _countryRepository = countryRepository;
            _stateRepository = stateRepository;
            _designationRepository = designationRepository;
            _skillsetHistoryRepository = skillsetHistoryRepository;
            _employeeRequestRepository = employeeRequestRepository;
            _employeeDesignationHistoryRepository = employeeDesignationHistoryRepository;
            _employeeNationalityRepository = employeeNationalityRepository;
            _auditRepository = auditRepository;
            _employeeSpecialAbilityRepository = employeeSpecialAbilityRepository;
            _employeeRequestDetailsRepository = employeeRequestDetailsRepository;
            _employeeRequestDocumentRepository = employeeRequestDocumentRepository;
            _employeeLocationRepository = employeeLocationRepository;
        }

        #region Insert Update Employee
        /// <summary>
        /// Insert Or Update Employee
        /// </summary>
        /// <param name="pEmployee"></param>
        /// <returns></returns>
        public async Task<EmployeeNotificationData> AddOrUpdateEmployee(EmployeesViewModel pEmployee)
        {
            int employeeId = 0;
            bool reportingManagerChanged = false;
            bool designationchanged = false;
            bool baseWorkLocationChanged = false;
            bool probationstatuschanged = false;
            string oldData = "";
            EmployeeNotificationData data = new EmployeeNotificationData();
            data.checkLeave = false;
            EmployeeManagerDetails oldmanagerDetails = new();
            EmployeeManagerDetails newmanagerdetails = new();
            EmployeeMasterEmailTemplate emailtemplatefordesignation = new();
            EmployeeMasterEmailTemplate emailtemplateforbaseworklocation = new();
            EmployeeMasterEmailTemplate emailtemplateforprobationstatus = new();
            EmployeeMasterEmailTemplate emailtemplateforreportingmanagerchanged = new();
            EmployeeManagerDetails managerdetailsforprobationstatus = new();

            EmployeeDesignationDetails oldemployeeDesignationDetails = new();
            EmployeeDesignationDetails newemployeeDesignationDetails = new();
            EmployeeBaseWorkLocationDetails oldbaseWorkLocationDetails = new();
            EmployeeBaseWorkLocationDetails newbaseWorkLocationDetails = new();
            EmployeeProbationStatusDetails oldprobationStatusDetails = new();
            EmployeeProbationStatusDetails newprobationStatusDetails = new();
            string employeeName = "";
            string employeeemailaddress = "";
            string employeeObj = "";
            try
            {
                EmployeeAuditDataView oldemployeeAuditData = new EmployeeAuditDataView();
                EmployeeAuditDataView newemployeeAuditData = new EmployeeAuditDataView();
                Employees employee = _employeeRepository.GetEmployeeByFormattedEmployeeId(pEmployee?.Employee?.FormattedEmployeeId, pEmployee.Employee.EmployeeID);
                if (employee != null)
                {
                    return new EmployeeNotificationData()
                    {
                        EmployeeId = -1,
                    }; // "Formatted employee id (" + pEmployee.Employee.FormattedEmployeeId + ") already exists!";
                }
                employee = _employeeRepository.CheckEmployeeByemailId(pEmployee?.Employee?.EmailAddress, pEmployee.Employee.EmployeeID);
                if (employee != null)
                {
                    return new EmployeeNotificationData()
                    {
                        EmployeeId = -2,
                    }; // "Email Id (" + pEmployee?.Employee?.EmailAddress + ") already exists!";
                }
                EmployeesViewModel employeeViewModel = new();
                employee = new();
                var profilePicturePath = "";
                if (pEmployee?.Employee?.EmployeeID != 0)
                {
                    employeeViewModel = _employeeRepository.GetEmployeeById(pEmployee.Employee.EmployeeID);
                    employee = employeeViewModel?.Employee;
                    oldemployeeAuditData.Employee = employeeViewModel?.Employee;
                }
                employeeObj = JsonConvert.SerializeObject(employee);
                EmployeesPersonalInfo personalInfo = new EmployeesPersonalInfo();
                if (pEmployee.EmployeesPersonalInfo.PersonalInfoId != 0)
                {
                    personalInfo = _employeesPersonalInfoRepository.GetEmployeePersonalInfoById(pEmployee.EmployeesPersonalInfo.PersonalInfoId);
                    oldemployeeAuditData.EmployeesPersonalInfoDetail = personalInfo;
                }

                if (employee != null)
                {
                    DateTime TodayDate = DateTime.Now;
                    if (employee?.EmployeeTypeId != pEmployee?.Employee?.EmployeeTypeId || pEmployee?.Employee?.DepartmentId != employee?.DepartmentId || pEmployee?.Employee?.SystemRoleId != employee?.SystemRoleId || employee?.Gender != pEmployee?.Employee?.Gender || employee?.Maritalstatus != pEmployee?.Employee?.Maritalstatus)
                    {
                        data.checkLeave = true;
                    }
                    employee.FirstName = pEmployee.Employee.FirstName;
                    employee.LastName = pEmployee.Employee.LastName;
                    employee.EmailAddress = pEmployee.Employee.EmailAddress;
                    employee.EmployeeTypeId = pEmployee.Employee.EmployeeTypeId;
                    employee.EmployeeName = pEmployee.Employee.EmployeeName == null ? pEmployee.Employee.FirstName + " " + pEmployee.Employee.LastName : pEmployee.Employee.EmployeeName;
                    employee.DepartmentId = pEmployee.Employee.DepartmentId;
                    employee.RoleId = pEmployee.Employee.RoleId;
                    employee.SystemRoleId = pEmployee.Employee.SystemRoleId;
                    employee.DateOfJoining = pEmployee.Employee.DateOfJoining;
                    employee.DateOfContract = pEmployee.Employee.DateOfContract;
                    employee.ContractEndDate = pEmployee.Employee.ContractEndDate;
                    employee.DateOfRelieving = pEmployee.Employee.DateOfRelieving;
                    //employee.ReportingManagerId = pEmployee.Employee.ReportingManagerId;
                    employeeName = pEmployee?.Employee?.EmployeeName;
                    employeeemailaddress = pEmployee?.Employee?.EmailAddress;
                    if (employee.ReportingManagerId != null && employee.ReportingManagerId != pEmployee?.Employee?.ReportingManagerId && pEmployee?.Employee?.ReportingManagerId != null)
                    {
                        oldmanagerDetails = await _employeeRepository.reportingManagerData((int)(employee.ReportingManagerId));
                        if (oldmanagerDetails != null)
                        {
                            newmanagerdetails = await _employeeRepository.reportingManagerData((int)(pEmployee?.Employee?.ReportingManagerId));
                            if (newmanagerdetails != null)
                            {
                                emailtemplateforreportingmanagerchanged = await _employeeRepository.getEmployeeEmailByName("ReportingManagerChange");
                                reportingManagerChanged = true;

                            }
                        }
                    }
                    employee.ReportingManagerId = pEmployee.Employee.ReportingManagerId;
                    if (employee.LocationId != null && employee.LocationId != pEmployee?.Employee?.LocationId && pEmployee?.Employee?.LocationId != null)
                    {
                        oldbaseWorkLocationDetails = await _employeeRepository.baseWorkLocationData((int)(employee.LocationId));
                        if (oldbaseWorkLocationDetails != null)
                        {
                            newbaseWorkLocationDetails = await _employeeRepository.baseWorkLocationData((int)(pEmployee?.Employee?.LocationId));
                            if (newbaseWorkLocationDetails != null)
                            {
                                emailtemplateforbaseworklocation = await _employeeRepository.getEmployeeEmailByName("BaseWorkLocationChange");
                                baseWorkLocationChanged = true;
                                data.checkLeave = true;
                            }
                        }
                    }
                    employee.LocationId = pEmployee.Employee.LocationId == null ? 0 : pEmployee.Employee.LocationId;
                    employee.CurrentWorkLocationId = pEmployee.Employee.CurrentWorkLocationId == null ? 0 : pEmployee.Employee.CurrentWorkLocationId;
                    employee.CurrentWorkPlaceId = pEmployee.Employee.CurrentWorkPlaceId == null ? 0 : pEmployee.Employee.CurrentWorkPlaceId;
                    employee.IsSpecialAbility = pEmployee.Employee.IsSpecialAbility;
                    employee.Gender = pEmployee.Employee.Gender;
                    if (employee.DesignationId != null && employee.DesignationId != pEmployee?.Employee?.DesignationId && pEmployee?.Employee?.DesignationId != null)
                    {
                        oldemployeeDesignationDetails = await _employeeRepository.designationData((int)(employee.DesignationId));
                        if (oldemployeeDesignationDetails != null)
                        {
                            newemployeeDesignationDetails = await _employeeRepository.designationData((int)(pEmployee?.Employee?.DesignationId));
                            if (newemployeeDesignationDetails != null)
                            {
                                emailtemplatefordesignation = await _employeeRepository.getEmployeeEmailByName("DesignationChange");
                                designationchanged = true;
                                data.checkLeave = true;
                            }
                        }
                    }
                    if (pEmployee?.Employee?.DesignationEffectiveFrom != null && pEmployee?.Employee?.DesignationEffectiveFrom.Value.Date <= DateTime.Now.Date)
                    {
                        employee.DesignationId = pEmployee.Employee.DesignationId;
                        employee.DesignationEffectiveFrom = pEmployee.Employee.DesignationEffectiveFrom;
                    }
                    employee.EmployeeCategoryId = pEmployee.Employee.EmployeeCategoryId;
                    employee.EmployeeGrade = pEmployee.Employee.EmployeeGrade;
                    employee.ProbationExtension = pEmployee.Employee.ProbationExtension;
                    employee.ActualConfirmationDate = pEmployee.Employee.ActualConfirmationDate;
                    employee.MergerDate = pEmployee.Employee.MergerDate;
                    //employee.SpecialAbilityId = pEmployee.Employee.SpecialAbilityId;
                    employee.SpecialAbilityRemark = pEmployee.Employee.SpecialAbilityRemark;
                    //employee.TotalExperience = pEmployee.Employee.TotalExperience;
                    employee.PreviousExperience = pEmployee.Employee.PreviousExperience;
                    employee.SourceOfHireId = pEmployee.Employee.SourceOfHireId;
                    //if (pEmployee?.Employee?.DateOfJoining != null && employee?.DateOfJoining?.Date <= DateTime.Now.Date)
                    //{
                    //    DateTime zeroTime = new(1, 1, 1);
                    //    DateTime doj = (DateTime)employee?.DateOfJoining;
                    //    DateTime curdate = DateTime.Now.ToLocalTime();
                    //    TimeSpan span = curdate - doj;
                    //    int years = (zeroTime + span).Year - 1;
                    //    int months = (zeroTime + span).Month - 1;
                    //    employee.TVSNextExperience = (decimal?)((years * 1.0) + (months * 0.1));
                    //}
                    //else
                    //{
                    //    employee.TVSNextExperience = 0;
                    //}
                    employee.OfficialMobileNumber = pEmployee.Employee.OfficialMobileNumber;
                    employee.BirthDate = pEmployee.Employee.BirthDate;
                    employee.WeddingAnniversary = pEmployee.Employee.WeddingAnniversary;
                    employee.Maritalstatus = pEmployee.Employee.Maritalstatus;
                    if (employee.ProbationStatusId != null && employee.ProbationStatusId != pEmployee?.Employee?.ProbationStatusId && pEmployee?.Employee?.ProbationStatusId != null)
                    {
                        if (employee.ReportingManagerId != null)
                        {
                            managerdetailsforprobationstatus = await _employeeRepository.reportingManagerData((int)(employee.ReportingManagerId));
                            if (managerdetailsforprobationstatus != null)
                            {
                                oldprobationStatusDetails.ProbationStatusId = (int)(employee.ProbationStatusId);
                                newprobationStatusDetails.ProbationStatusId = (int)(pEmployee?.Employee?.ProbationStatusId);
                                emailtemplateforprobationstatus = await _employeeRepository.getEmployeeEmailByName("ProbationStatusChange");
                                probationstatuschanged = true;
                                data.checkLeave = true;
                            }
                        }
                    }

                    employee.ProbationStatusId = pEmployee.Employee.ProbationStatusId;
                    employee.FormattedEmployeeId = pEmployee.Employee.FormattedEmployeeId;
                    employee.NoticeCategory = pEmployee.Employee.NoticeCategory;
                    employee.ProfilePicture = pEmployee.Employee.ProfilePicture;
                    employee.ProbationExtension = pEmployee.Employee.ProbationExtension;
                    employee.ContractEndDate = pEmployee.Employee.ContractEndDate;
                    employee.Entity = pEmployee.Employee.Entity;
                    employee.IsActive = pEmployee.Employee.IsActive == null ? true : pEmployee.Employee.IsActive;
                    employee.RetirementDate = pEmployee.Employee.RetirementDate;
                    if (pEmployee?.Employee?.EmployeeID > 0)
                    {
                        newemployeeAuditData.ActionType = "Update";
                        employee.ModifiedOn = DateTime.UtcNow;
                        employee.ModifiedBy = pEmployee.Employee.ModifiedBy;
                        newemployeeAuditData.CreatedBy = (int)pEmployee.Employee.ModifiedBy;
                        var dependentList = _employeeDependentRepository.GetEmployeeDependentByEmployeeId(employee.EmployeeID);
                        List<int> depList = dependentList.Select(x => (int)x.EmployeeDependentId).ToList();
                        List<int> newDepList = pEmployee.EmployeeDependent.Select(x => (int)x.EmployeeDependentId).ToList();
                        oldemployeeAuditData.EmployeeDependents = dependentList;
                        var exceptList = depList?.Where(p => !newDepList.Contains(p)).ToList();
                        dependentList = dependentList.Where(x => exceptList.Contains(x.EmployeeDependentId)).ToList();

                        if (dependentList?.Count > 0)
                        {
                            foreach (EmployeeDependent dependent in dependentList)
                            {
                                _employeeDependentRepository.Delete(dependent);
                            }
                            await _employeeDependentRepository.SaveChangesAsync();
                        }

                        var skillsetList = _employeesSkillsetRepository.GetEmployeesSkillsetByEmployeeId(pEmployee.Employee.EmployeeID);
                        oldemployeeAuditData.EmployeesSkillsets = skillsetList;
                        if (skillsetList?.Count > 0)
                        {
                            foreach (var skillset in skillsetList)
                            {
                                EmployeesSkillset empSkillset = await _employeesSkillsetRepository.GetEmployeesSkillsetById(skillset.EmployeesSkillsetId);
                                if (empSkillset?.EmployeesSkillsetId > 0)
                                {
                                    _employeesSkillsetRepository.Delete(empSkillset);
                                }
                            }
                            await _employeesSkillsetRepository.SaveChangesAsync();
                        }

                        var specialAbilityList = _employeeSpecialAbilityRepository.GetEmployeeSpecialAbilityByEmployeeId(pEmployee.Employee.EmployeeID);
                        oldemployeeAuditData.EmployeeSpecialAbility = specialAbilityList;
                        if (specialAbilityList?.Count > 0)
                        {
                            foreach (var specialAbility in specialAbilityList)
                            {
                                EmployeeSpecialAbility empSpecialAbility = await _employeeSpecialAbilityRepository.GetEmployeeSpecialAbilityById(specialAbility.EmployeeSpecialAbilityId);
                                if (empSpecialAbility?.EmployeeSpecialAbilityId > 0)
                                {
                                    _employeeSpecialAbilityRepository.Delete(empSpecialAbility);
                                    await _employeeSpecialAbilityRepository.SaveChangesAsync();

                                }
                            }
                        }

                        var shiftList = _employeeShiftDetailsRepository.GetEmployeeShiftByEmployeeId(pEmployee.Employee.EmployeeID);
                        List<int> oldshifList = shiftList?.Select(x => (int)x.EmployeeShiftDetailsId).ToList();
                        List<int> newshifList = pEmployee.EmployeeShiftDetails?.Select(x => (int)x.EmployeeShiftDetailsId).ToList();
                        oldemployeeAuditData.EmployeeShiftDetails = shiftList;
                        var exceptShifList = oldshifList.Where(p => !newshifList.Contains(p)).ToList();
                        shiftList = shiftList.Where(x => exceptShifList.Contains((int)x.EmployeeShiftDetailsId)).ToList();
                        if (shiftList?.Count > 0)
                        {
                            foreach (var shift in shiftList)
                            {
                                EmployeeShiftDetails empShift = await _employeeShiftDetailsRepository.GetShiftDetailsById(shift.EmployeeShiftDetailsId);
                                if (empShift?.EmployeeShiftDetailsId > 0)
                                {
                                    _employeeShiftDetailsRepository.Delete(empShift);
                                }
                            }
                            await _employeeShiftDetailsRepository.SaveChangesAsync();
                        }
                        oldemployeeAuditData.Employee = JsonConvert.DeserializeObject<Employees>(employeeObj);
                        oldData = JsonConvert.SerializeObject(oldemployeeAuditData);
                        _employeeRepository.Update(employee);

                    }
                    else
                    {
                        newemployeeAuditData.ActionType = "Create";
                        employee.IsActive = true;
                        employee.CreatedOn = DateTime.UtcNow;
                        employee.CreatedBy = pEmployee.Employee.CreatedBy;
                        newemployeeAuditData.CreatedBy = (int)pEmployee.Employee.CreatedBy;
                        oldemployeeAuditData.Employee = JsonConvert.DeserializeObject<Employees>(employeeObj);
                        oldData = JsonConvert.SerializeObject(oldemployeeAuditData);
                        await _employeeRepository.AddAsync(employee);
                    }
                    await _employeeRepository.SaveChangesAsync();
                    employeeId = employee.EmployeeID;
                    pEmployee.supportingDocumentsViews.SourceId = employeeId;
                    var result = await AddDesignationHistory(employeeId, pEmployee.Employee.DesignationId, pEmployee.Employee.DesignationEffectiveFrom, pEmployee.Employee.CreatedBy);
                    //Add Personal Information
                    if (personalInfo != null)
                    {
                        personalInfo.EmployeeId = employeeId;
                        personalInfo.HighestQualification = pEmployee.EmployeesPersonalInfo.HighestQualification;
                        personalInfo.PersonalMobileNumber = pEmployee.EmployeesPersonalInfo.PersonalMobileNumber;
                        personalInfo.OtherEmail = pEmployee.EmployeesPersonalInfo.OtherEmail;
                        personalInfo.BloodGroup = pEmployee.EmployeesPersonalInfo.BloodGroup;
                        personalInfo.PermanentAddressLine1 = pEmployee.EmployeesPersonalInfo.PermanentAddressLine1;
                        personalInfo.PermanentAddressLine2 = pEmployee.EmployeesPersonalInfo.PermanentAddressLine2;
                        personalInfo.PermanentCity = pEmployee.EmployeesPersonalInfo.PermanentCity;
                        personalInfo.PermanentState = pEmployee.EmployeesPersonalInfo.PermanentState;
                        personalInfo.PermanentCountry = pEmployee.EmployeesPersonalInfo.PermanentCountry;
                        personalInfo.PermanentAddressZip = pEmployee.EmployeesPersonalInfo.PermanentAddressZip;
                        personalInfo.SpouseName = pEmployee.EmployeesPersonalInfo.SpouseName;
                        personalInfo.FathersName = pEmployee.EmployeesPersonalInfo.FathersName;
                        personalInfo.EmergencyMobileNumber = pEmployee.EmployeesPersonalInfo.EmergencyMobileNumber;
                        personalInfo.Nationality = pEmployee.EmployeesPersonalInfo.Nationality;
                        personalInfo.CommunicationAddressLine1 = pEmployee.EmployeesPersonalInfo.CommunicationAddressLine1;
                        personalInfo.CommunicationAddressLine2 = pEmployee.EmployeesPersonalInfo.CommunicationAddressLine2;
                        personalInfo.CommunicationCity = pEmployee.EmployeesPersonalInfo.CommunicationCity;
                        personalInfo.CommunicationState = pEmployee.EmployeesPersonalInfo.CommunicationState;
                        personalInfo.CommunicationCountry = pEmployee.EmployeesPersonalInfo.CommunicationCountry;
                        personalInfo.CommunicationAddressZip = pEmployee.EmployeesPersonalInfo.CommunicationAddressZip;
                        personalInfo.PANNumber = pEmployee.EmployeesPersonalInfo.PANNumber;
                        personalInfo.UANNumber = pEmployee.EmployeesPersonalInfo.UANNumber;
                        personalInfo.DrivingLicense = pEmployee.EmployeesPersonalInfo.DrivingLicense;
                        personalInfo.AadhaarCardNumber = pEmployee.EmployeesPersonalInfo.AadhaarCardNumber;
                        personalInfo.PassportNumber = pEmployee.EmployeesPersonalInfo.PassportNumber;
                        personalInfo.EmergencyContactName = pEmployee.EmployeesPersonalInfo.EmergencyContactName;
                        personalInfo.EmergencyContactRelation = pEmployee.EmployeesPersonalInfo.EmergencyContactRelation;
                        personalInfo.EmergencyMobileNumber = pEmployee.EmployeesPersonalInfo.EmergencyMobileNumber;
                        personalInfo.ReferenceContactName = pEmployee.EmployeesPersonalInfo.ReferenceContactName;
                        personalInfo.ReferenceEmailId = pEmployee.EmployeesPersonalInfo.ReferenceEmailId;
                        personalInfo.ReferenceMobileNumber = pEmployee.EmployeesPersonalInfo.ReferenceMobileNumber;
                        personalInfo.IsJoiningBonus = pEmployee.EmployeesPersonalInfo?.IsJoiningBonus;
                        personalInfo.JoiningBonusAmmount = pEmployee.EmployeesPersonalInfo?.JoiningBonusAmmount;
                        personalInfo.JoiningBonusCondition = pEmployee.EmployeesPersonalInfo?.JoiningBonusCondition;
                        personalInfo.AccountHolderName = pEmployee.EmployeesPersonalInfo?.AccountHolderName;
                        personalInfo.BankName = pEmployee.EmployeesPersonalInfo?.BankName;
                        personalInfo.IFSCCode = pEmployee.EmployeesPersonalInfo?.IFSCCode;
                        personalInfo.AccountNumber = pEmployee.EmployeesPersonalInfo?.AccountNumber;
                    }

                    if (pEmployee.EmployeesPersonalInfo.PersonalInfoId != 0)
                    {
                        personalInfo.ModifiedOn = DateTime.UtcNow;
                        personalInfo.ModifiedBy = pEmployee.Employee.ModifiedBy;
                        _employeesPersonalInfoRepository.Update(personalInfo);
                    }
                    else
                    {
                        personalInfo.CreatedOn = DateTime.UtcNow;
                        personalInfo.CreatedBy = pEmployee.Employee.CreatedBy;
                        await _employeesPersonalInfoRepository.AddAsync(personalInfo);
                    }
                    await _employeesPersonalInfoRepository.SaveChangesAsync();
                    var baseDirectory = pEmployee.supportingDocumentsViews.BaseDirectory;
                    foreach (DocumentsToUpload documents in pEmployee.EmployeesPersonalInfo?.employeeProofDocument)
                    {
                        var docList = new List<DocumentsToUpload> { documents };
                        pEmployee.supportingDocumentsViews.DocumentType = documents.DocumentCategory;
                        pEmployee.supportingDocumentsViews.EmployeeDocumentList = docList;
                        pEmployee.supportingDocumentsViews.BaseDirectory = baseDirectory;
                        pEmployee.supportingDocumentsViews.EmployeeId = employeeId;
                        pEmployee.supportingDocumentsViews.BaseDirectory = GetDirectoryPath(pEmployee.supportingDocumentsViews);
                        //pEmployee.supportingDocumentsViews.SourceId = null;
                        await AddSupportingDocument(pEmployee.supportingDocumentsViews);
                    }

                    newemployeeAuditData.Employee = employee;
                    newemployeeAuditData.EmployeesPersonalInfoDetail = personalInfo;

                    if (pEmployee?.Skillset?.Count > 0)
                    {
                        List<EmployeesSkillset> newSkillSetList = new List<EmployeesSkillset>();
                        foreach (var skillset in pEmployee?.Skillset)
                        {
                            EmployeesSkillset empSkillset = new()
                            {
                                EmployeeId = employeeId,
                                CreatedOn = DateTime.UtcNow,
                                CreatedBy = pEmployee.Employee.CreatedBy
                            };
                            if (skillset?.SkillsetId == 0)
                            {
                                Skillsets existSkillset = await _skillsetRepository.GetSkillsetByName(skillset?.Skillset?.ToLower());
                                if (existSkillset?.SkillsetId > 0)
                                {
                                    empSkillset.SkillsetId = existSkillset.SkillsetId;
                                }
                                else
                                {
                                    Skillsets skillsets = new()
                                    {
                                        Skillset = skillset?.Skillset,
                                        CreatedOn = DateTime.UtcNow,
                                        CreatedBy = pEmployee.Employee.CreatedBy
                                    };
                                    await _skillsetRepository.AddAsync(skillsets);
                                    await _skillsetRepository.SaveChangesAsync();

                                    empSkillset.SkillsetId = skillsets.SkillsetId;
                                }
                            }
                            else
                            {
                                empSkillset.SkillsetId = skillset.SkillsetId;
                            }
                            await _employeesSkillsetRepository.AddAsync(empSkillset);
                            newSkillSetList.Add(empSkillset);
                        }
                        await _employeesSkillsetRepository.SaveChangesAsync();
                        newemployeeAuditData.EmployeesSkillsets = newSkillSetList;

                    }
                    if (pEmployee?.EmployeespecialAbilities?.Count > 0)
                    {
                        newemployeeAuditData.EmployeeSpecialAbility = new List<EmployeeSpecialAbility>();
                        foreach (EmployeeSpecialAbility employeeSpecialAbility in pEmployee?.EmployeespecialAbilities)
                        {
                            if (pEmployee?.EmployeespecialAbilities != null)
                            {
                                EmployeeSpecialAbility specialAbility = new()
                                {
                                    SpecialAbilityId = employeeSpecialAbility.SpecialAbilityId,
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedBy = pEmployee.Employee.CreatedBy,
                                    EmployeeId = employeeId
                                };
                                await _employeeSpecialAbilityRepository.AddAsync(specialAbility);
                                await _employeeSpecialAbilityRepository.SaveChangesAsync();
                                newemployeeAuditData?.EmployeeSpecialAbility.Add(specialAbility);
                            }
                        }
                    }
                    if (pEmployee?.EmployeeDependent?.Count > 0)
                    {
                        List<EmployeeDependent> employeeDependentsList = new List<EmployeeDependent>();
                        foreach (EmployeeDependentView employeeDependentView in pEmployee?.EmployeeDependent)
                        {
                            if (pEmployee?.EmployeeDependent != null)
                            {
                                if (employeeDependentView.EmployeeDependentId != 0)
                                {
                                    EmployeeDependent dependent = await _employeeDependentRepository.GetEmployeeDependentByDependentId(employeeDependentView.EmployeeDependentId);
                                    if (dependent != null)
                                    {
                                        dependent.EmployeeRelationName = employeeDependentView.EmployeeRelationName;
                                        dependent.EmployeeRelationshipId = employeeDependentView.EmployeeRelationshipId;
                                        dependent.EmployeeRelationDateOfBirth = employeeDependentView.EmployeeRelationDateOfBirth;
                                        dependent.ModifiedBy = pEmployee.Employee.ModifiedBy;
                                        dependent.ModifiedOn = DateTime.UtcNow;
                                        _employeeDependentRepository.Update(dependent);
                                        employeeDependentsList.Add(dependent);
                                        await _employeeDependentRepository.SaveChangesAsync();

                                    }
                                    if (employeeDependentView?.DependentDetailsProof != null)
                                    {
                                        var docList = new List<DocumentsToUpload> { employeeDependentView?.DependentDetailsProof };
                                        pEmployee.supportingDocumentsViews.DocumentType = "DependentProof";
                                        pEmployee.supportingDocumentsViews.SourceType = "Dependent Detail";
                                        pEmployee.supportingDocumentsViews.SourceId = dependent.EmployeeDependentId;
                                        pEmployee.supportingDocumentsViews.EmployeeDocumentList = docList;
                                        pEmployee.supportingDocumentsViews.EmployeeId = employeeId;
                                        pEmployee.supportingDocumentsViews.BaseDirectory = GetDirectoryPath(pEmployee.supportingDocumentsViews);
                                        await AddSupportingDocument(pEmployee.supportingDocumentsViews);
                                    }

                                }
                                else
                                {
                                    EmployeeDependent employeeDependent = new()
                                    {
                                        EmployeeRelationName = employeeDependentView.EmployeeRelationName,
                                        EmployeeRelationshipId = employeeDependentView.EmployeeRelationshipId,
                                        EmployeeRelationDateOfBirth = employeeDependentView.EmployeeRelationDateOfBirth,
                                        CreatedOn = DateTime.UtcNow,
                                        CreatedBy = pEmployee.Employee.CreatedBy,
                                        EmployeeID = employeeId
                                    };
                                    await _employeeDependentRepository.AddAsync(employeeDependent);
                                    employeeDependentsList.Add(employeeDependent);
                                    await _employeeDependentRepository.SaveChangesAsync();

                                    if (employeeDependentView?.DependentDetailsProof != null)
                                    {
                                        var docList = new List<DocumentsToUpload> { employeeDependentView?.DependentDetailsProof };
                                        pEmployee.supportingDocumentsViews.DocumentType = "DependentProof";
                                        pEmployee.supportingDocumentsViews.SourceType = "Dependent Detail";
                                        pEmployee.supportingDocumentsViews.SourceId = employeeDependent.EmployeeDependentId;
                                        pEmployee.supportingDocumentsViews.EmployeeDocumentList = docList;
                                        pEmployee.supportingDocumentsViews.EmployeeId = employeeId;
                                        pEmployee.supportingDocumentsViews.BaseDirectory = GetDirectoryPath(pEmployee.supportingDocumentsViews);
                                        await AddSupportingDocument(pEmployee.supportingDocumentsViews);
                                    }
                                }

                            }
                        }
                        newemployeeAuditData.EmployeeDependents = employeeDependentsList;
                    }

                    if (pEmployee?.EmployeeShiftDetails?.Count > 0)
                    {
                        newemployeeAuditData.EmployeeShiftDetails = new List<EmployeeShiftDetails>();
                        foreach (EmployeeShiftDetails employeeShiftDetailsView in pEmployee?.EmployeeShiftDetails)
                        {
                            EmployeeShiftDetails employeeShiftDetails = new();
                            //if (pEmployee?.Employee?.EmployeeID != 0 && employeeShiftDetailsView?.EmployeeShiftDetailsId == 0)
                            //{
                            //    EmployeeShiftDetails updateEmployeeShiftDetails = new();
                            //    updateEmployeeShiftDetails = _employeeShiftDetailsRepository.GetEmployeePreviousShiftDetailsByEmployeeId(pEmployee.Employee.EmployeeID);
                            //    if (updateEmployeeShiftDetails != null)
                            //    {
                            //        updateEmployeeShiftDetails.ShiftToDate = employeeShiftDetailsView?.ShiftFromDate?.AddDays(-1);
                            //        updateEmployeeShiftDetails.ModifiedOn = DateTime.UtcNow;
                            //        updateEmployeeShiftDetails.ModifiedBy = pEmployee.Employee.ModifiedBy;
                            //        _employeeShiftDetailsRepository.Update(updateEmployeeShiftDetails);
                            //        await _employeeShiftDetailsRepository.SaveChangesAsync();
                            //    }
                            employeeShiftDetails = await _employeeShiftDetailsRepository.GetShiftDetailsById(employeeShiftDetailsView.EmployeeShiftDetailsId == null ? 0 : (int)employeeShiftDetailsView.EmployeeShiftDetailsId);
                            if (employeeShiftDetails == null) employeeShiftDetails = new EmployeeShiftDetails();
                            if (employeeShiftDetailsView.EmployeeShiftDetailsId != 0)
                            {
                                employeeShiftDetails.ShiftFromDate = employeeShiftDetailsView.ShiftFromDate;
                                employeeShiftDetails.ShiftToDate = employeeShiftDetailsView.ShiftToDate;
                                employeeShiftDetails.ShiftDetailsId = employeeShiftDetailsView.ShiftDetailsId;
                                employeeShiftDetails.ModifiedOn = DateTime.UtcNow;
                                employeeShiftDetails.ModifiedBy = pEmployee.Employee.CreatedBy;
                                employeeShiftDetails.EmployeeID = employeeId;
                                _employeeShiftDetailsRepository.Update(employeeShiftDetails);
                                await _employeeShiftDetailsRepository.SaveChangesAsync();
                            }
                            else
                            {
                                employeeShiftDetails.ShiftDetailsId = employeeShiftDetailsView.ShiftDetailsId;
                                employeeShiftDetails.ShiftFromDate = employeeShiftDetailsView.ShiftFromDate;
                                employeeShiftDetails.ShiftToDate = employeeShiftDetailsView.ShiftToDate;

                                employeeShiftDetails.CreatedOn = DateTime.UtcNow;
                                employeeShiftDetails.CreatedBy = pEmployee.Employee.CreatedBy;
                                employeeShiftDetails.EmployeeID = employeeId;
                                await _employeeShiftDetailsRepository.AddAsync(employeeShiftDetails);
                                await _employeeShiftDetailsRepository.SaveChangesAsync();
                            }

                            newemployeeAuditData?.EmployeeShiftDetails?.Add(employeeShiftDetails);
                            //}
                            //else
                            //{
                            //    if (pEmployee?.Employee?.EmployeeID != 0 && employeeShiftDetailsView?.EmployeeShiftDetailsId != 0)
                            //    {
                            //        EmployeeShiftDetails shiftDetails = new();
                            //        shiftDetails = _employeeShiftDetailsRepository.GetShiftDetailsById(employeeShiftDetailsView.EmployeeShiftDetailsId);
                            //        if (shiftDetails != null)
                            //        {
                            //            shiftDetails.ShiftDetailsId = employeeShiftDetailsView.ShiftDetailsId;
                            //            shiftDetails.ShiftFromDate = employeeShiftDetailsView.ShiftFromDate;
                            //            shiftDetails.ShiftToDate = employeeShiftDetailsView.ShiftToDate;
                            //            shiftDetails.ModifiedOn = DateTime.UtcNow;
                            //            shiftDetails.ModifiedBy = pEmployee.Employee.ModifiedBy;
                            //            shiftDetails.EmployeeID = employeeId;
                            //            _employeeShiftDetailsRepository.Update(shiftDetails);
                            //            await _employeeShiftDetailsRepository.SaveChangesAsync();
                            //        }
                            //    }
                            //}
                        }
                    }
                    await AddAuditFile(newemployeeAuditData, oldData);

                }
            }
            catch (Exception e)
            {
                throw;
            }
            data.EmployeeId = employeeId;
            if (reportingManagerChanged == true)
            {
                data.EmployeeName = employeeName;
                data.EmployeeEmailAddress = employeeemailaddress;
                data.OldManagerDetails = new EmployeeManagerDetails()
                {
                    ManagerID = oldmanagerDetails.ManagerID,
                    ManagerName = oldmanagerDetails.ManagerName,
                    EmailAddress = oldmanagerDetails.EmailAddress,
                };
                data.NewManagerDetails = new EmployeeManagerDetails()
                {
                    ManagerID = newmanagerdetails.ManagerID,
                    ManagerName = newmanagerdetails.ManagerName,
                    EmailAddress = newmanagerdetails.EmailAddress,
                };
                data.EmployeeMasterEmailTemplateForReportingManagerChange = new EmployeeMasterEmailTemplate()
                {
                    TemplateId = emailtemplateforreportingmanagerchanged.TemplateId,
                    TemplateName = emailtemplateforreportingmanagerchanged.TemplateName,
                    Body = emailtemplateforreportingmanagerchanged.Body,
                    Subject = emailtemplateforreportingmanagerchanged.Subject,
                };

            }
            if (designationchanged == true)
            {
                data.EmployeeName = employeeName;
                data.EmployeeEmailAddress = employeeemailaddress;
                data.OldEmployeeDesignationDetails = new EmployeeDesignationDetails()
                {
                    DesignationId = oldemployeeDesignationDetails.DesignationId,
                    DesignationName = oldemployeeDesignationDetails.DesignationName,

                };
                data.NewEmployeeDesignationDetails = new EmployeeDesignationDetails()
                {
                    DesignationId = newemployeeDesignationDetails.DesignationId,
                    DesignationName = newemployeeDesignationDetails.DesignationName,

                };
                data.EmployeeMasterEmailTemplateForDesignation = new EmployeeMasterEmailTemplate()
                {
                    TemplateId = emailtemplatefordesignation.TemplateId,
                    TemplateName = emailtemplatefordesignation.TemplateName,
                    Body = emailtemplatefordesignation.Body,
                    Subject = emailtemplatefordesignation.Subject,
                };

            }
            if (baseWorkLocationChanged == true)
            {
                data.EmployeeName = employeeName;
                data.EmployeeEmailAddress = employeeemailaddress;
                data.OldEmployeeBaseWorkLocationDetails = new EmployeeBaseWorkLocationDetails()
                {
                    LocationId = oldbaseWorkLocationDetails.LocationId,
                    LocationName = oldbaseWorkLocationDetails.LocationName,

                };
                data.NewEmployeeBaseWorkLocationDetails = new EmployeeBaseWorkLocationDetails()
                {
                    LocationId = newbaseWorkLocationDetails.LocationId,
                    LocationName = newbaseWorkLocationDetails.LocationName,

                };
                data.EmployeeMasterEmailTemplateForBaseWorkLocation = new EmployeeMasterEmailTemplate()
                {
                    TemplateId = emailtemplateforbaseworklocation.TemplateId,
                    TemplateName = emailtemplateforbaseworklocation.TemplateName,
                    Body = emailtemplateforbaseworklocation.Body,
                    Subject = emailtemplateforbaseworklocation.Subject,
                };

            }
            if (probationstatuschanged == true)
            {
                data.EmployeeName = employeeName;
                data.EmployeeEmailAddress = employeeemailaddress;
                data.OldEmployeeProbationStatusDetails = new EmployeeProbationStatusDetails()
                {
                    ProbationStatusId = oldprobationStatusDetails.ProbationStatusId,
                    //ProbationStatusName=oldprobationStatusDetails.ProbationStatusName,

                };
                data.NewEmployeeProbationStatusDetails = new EmployeeProbationStatusDetails()
                {
                    ProbationStatusId = newprobationStatusDetails.ProbationStatusId,
                    //ProbationStatusName=newprobationStatusDetails.ProbationStatusName,

                };
                data.EmployeeMasterEmailTemplateForProbationStatus = new EmployeeMasterEmailTemplate()
                {
                    TemplateId = emailtemplateforprobationstatus.TemplateId,
                    TemplateName = emailtemplateforprobationstatus.TemplateName,
                    Body = emailtemplateforprobationstatus.Body,
                    Subject = emailtemplateforprobationstatus.Subject,
                };
                data.ManagerDetailsForProbationStatus = new EmployeeManagerDetails()
                {
                    ManagerID = managerdetailsforprobationstatus.ManagerID,
                    ManagerName = managerdetailsforprobationstatus.ManagerName,
                    EmailAddress = managerdetailsforprobationstatus.EmailAddress,
                };

            }
            return data;


        }
        #endregion

        #region Get employee by employee id
        public EmployeesViewModel GetEmployeeDetailsByEmployeeId(int employeeId)
        {
            EmployeesViewModel employeeDetails = new();
            if (employeeId > 0)
            {
                employeeDetails = _employeeRepository.GetEmployeeById(employeeId);
                employeeDetails.EmployeesPersonalInfo = GetEmployeesPersonalInformation(employeeId);
                employeeDetails.AddressProof = _employeeDocumentRepository.GetDocumentsDetail(employeeId);
                employeeDetails.designationHistory = _employeeDesignationHistoryRepository.GetEmployeeDesignationHistoeryByEmployeeId(employeeId);
                employeeDetails.employeeRequestDetails = _employeeRequestRepository.GetPendingRequestByEmployeeId(employeeId);
                employeeDetails.CommunicationAddressProof = _employeeDocumentRepository.GetAddressProof(employeeId, "communicationAddressProof");
                employeeDetails.PermanentAddressProof = _employeeDocumentRepository.GetAddressProof(employeeId, "permanentAddressProof");
                employeeDetails.MaritalStatusProof = _employeeDocumentRepository.GetAddressProof(employeeId, "maritalstatusProof");
            }
            return employeeDetails;
        }
        #endregion

        #region Get employee master data
        public EmployeeMasterData GetEmployeeMasterData(string employeeIdFormat)
        {
            EmployeeMasterData employeeMasterData = new()
            {
                EmployeeTypeList = _employeeRepository.GetEmployeeTypeList(),
                EmployeeDepartmentList = _employeeRepository.GetEmployeeDepartmentList(),
                SkillsetList = _employeeRepository.GetSkillsetList(),
                RolesList = _employeeRepository.GetRolesList(),
                //ReportingManagerList = _employeeRepository.GetEmployeeList(),
                EmployeeWorkPlaceList = _appConstantRepository.GetAppConstantByType("CurrentWorkPlace"),
                EmployeeLocationList = _employeeRepository.GetEmployeeLocationList(),
                DesignationList = _employeeRepository.GetDesignationList(),
                EmployeeCategoryList = _employeeRepository.GetEmployeeCategoryList(),
                EmployeeRelationshipList = _employeeRepository.GetEmployeeRelationshipList(),
                EmployeeProbationStatus = _probationStatusRepository.GetProbationStatusList(),
                nextFormattedEmployeeId = "",//_employeeRepository.GetNextFormattedEmployeeId(employeeIdFormat)
                SystemRolesList = _employeeRepository.GetSystemRolesList(),
                BloodGroupList = _appConstantRepository.GetAppConstantByType("BloodGroup"),
                ExitTypeList = _appConstantRepository.GetAppConstantByType("ExitType"),
                MaritalStatusList = _appConstantRepository.GetAppConstantByType("MaritalStatus"),
                QualificationLIst = _appConstantRepository.GetAppConstantByType("Qualification"),
                SourceOfHireList = _appConstantRepository.GetAppConstantByType("SourceOfHire"),
                SpecialAbilityList = _appConstantRepository.GetAppConstantByType("SpecialAbility"),
                Entity = _appConstantRepository.GetAppConstantByType("Entity"),
                CountryList = _countryRepository.GetAllCountry(),
                StateList = _stateRepository.GetAllState(),
                Board = _appConstantRepository.GetAppConstantByType("Boards"),
                ResignationStatus = _appConstantRepository.GetAppConstantByType("ResignationStatus"),
                NationalityList = _employeeNationalityRepository.GetAllNationality(),
                NoticePeriodCategyList = _employeeRepository.GetNoticeCategory()
            };
            return employeeMasterData;
        }
        #endregion

        #region Get employee master data
        public EmployeeMasterDataForOrgChart GetEmployeeMasterDataForOrgChart(string employeeIdFormat)
        {
            EmployeeMasterDataForOrgChart EmployeeMasterDataForOrgChart = new()
            {
                EmployeeLocationList = _employeeRepository.GetEmployeeLocationList(),
            };
            return EmployeeMasterDataForOrgChart;
        }
        #endregion

        #region Insert or update department
        /// <summary>
        /// Insert or update department
        /// </summary>
        /// <param name="pDepartment"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateDepartment(Department pDepartment)
        {
            int departmentId = 0;
            try
            {
                Department department = new();
                if (pDepartment != null)
                {
                    department.DepartmentName = pDepartment.DepartmentName;
                    department.DepartmentShortName = pDepartment.DepartmentShortName;
                    department.DepartmentDescription = pDepartment.DepartmentDescription;
                    department.CreatedOn = DateTime.UtcNow;
                    department.CreatedBy = pDepartment.CreatedBy;
                    await _departmentRepository.AddAsync(department);
                    await _departmentRepository.SaveChangesAsync();
                    departmentId = department.DepartmentId;
                }

            }
            catch (Exception)
            {
                throw;
            }
            return departmentId;
        }
        #endregion

        #region Insert And Update Role
        public async Task<int> InsertOrUpdateRole(Roles pRole)
        {
            int roleId = 0;
            try
            {
                Roles role = new();
                if (pRole != null)
                {
                    role.RoleName = pRole.RoleName;
                    role.RoleShortName = pRole.RoleShortName;
                    role.RoleDescription = pRole.RoleDescription;
                    role.CreatedOn = DateTime.UtcNow;
                    role.CreatedBy = pRole.CreatedBy;
                    await _roleRepository.AddAsync(role);
                    await _roleRepository.SaveChangesAsync();
                    roleId = role.RoleId;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return roleId;
        }
        #endregion

        #region Get employee list
        /// <summary>
        /// Get employee list
        /// </summary>
        /// <returns></returns>
        public List<EmployeeDetail> GetEmployeesList(string pRole = "")
        {
            List<EmployeeDetail> listOfEmployee = new();
            var employeeList = _employeeRepository.GetEmployeesList(pRole);
            if (employeeList?.Count > 0)
            {
                for (int i = 0; i < employeeList?.Count; i++)
                {
                    int reportingManagerId = (employeeList[i].ReportingManagerId == null ? 0 : (int)employeeList[i].ReportingManagerId);
                    List<EmployeeName> reportingManager = _employeeRepository.GetEmployeeNameById(new List<int> { reportingManagerId });
                    EmployeeDetail employee = new()
                    {
                        EmployeeID = employeeList[i].EmployeeID,
                        FirstName = employeeList[i].FirstName,
                        LastName = employeeList[i].LastName,
                        EmailAddress = employeeList[i].EmailAddress,
                        EmployeeType = _employeeRepository.GetEmployeeTypeById(employeeList[i].EmployeeTypeId),
                        Department = _employeeRepository.GetDepartmentById(employeeList[i].DepartmentId),
                        Role = _employeeRepository.GetRoleNameByRoleId(employeeList[i].RoleId),
                        DateOfJoining = employeeList[i].DateOfJoining,
                        DateOfContract = employeeList[i].DateOfContract,
                        DateOfRelieving = employeeList[i].DateOfRelieving,
                        ReportingManager = reportingManager.Count > 0 ? reportingManager[0].EmployeeFullName : "",
                        IsActive = employeeList[i].IsActive,
                        CreatedOn = employeeList[i].CreatedOn,
                        CreatedBy = employeeList[i].CreatedBy,
                        FormattedEmployeeID = employeeList[i].FormattedEmployeeId,
                        DesignationId = employeeList[i].DesignationId,
                        Designation = _employeeRepository.GetDesignationById(employeeList[i].DesignationId)
                    };
                    listOfEmployee.Add(employee);
                }
            }
            return listOfEmployee;
        }
        #endregion

        #region Get all employee list
        /// <summary>
        /// Get all employee list
        /// </summary>
        /// <returns></returns>
        public List<EmployeeDetail> GetAllEmployeesList()
        {
            List<EmployeeDetail> listOfEmployee = new();
            var employeeList = _employeeRepository.GetAllEmployeesList();
            string skillSet = "NA";
            string dependent = "";
            if (employeeList?.Count > 0)
            {
                for (int i = 0; i < employeeList?.Count; i++)
                {
                    skillSet = "NA";
                    dependent = "";
                    int reportingManagerId = (employeeList[i].ReportingManagerId == null ? 0 : (int)employeeList[i].ReportingManagerId);
                    List<EmployeeName> reportingManager = _employeeRepository.GetEmployeeNameById(new List<int> { reportingManagerId });
                    var skillSetList = _skillsetRepository.GetSkillsetsByEmployeeId(employeeList[i].EmployeeID);

                    if (skillSetList?.Count > 0)
                    {
                        skillSet = string.Join(",", skillSetList);
                    }
                    var dependentsList = _employeeDependentRepository.GetEmployeeDependentByEmployeeId(employeeList[i].EmployeeID);
                    dependentsList.ForEach(x =>
                     dependent = dependent + "Name: " + x.EmployeeRelationName + ", Relation: " + _employeeRelationshipRepository.GetEmployeeRelationshipById(x.EmployeeRelationshipId)?.EmployeeRelationshipName + ", DOB: " + x.EmployeeRelationDateOfBirth?.ToString("dd MMM yyyy") + "; "
                    );
                    EmployeeDetail employee = new()
                    {
                        EmployeeID = employeeList[i].EmployeeID,
                        FirstName = employeeList[i].FirstName,
                        LastName = employeeList[i].LastName,
                        EmailAddress = employeeList[i].EmailAddress,
                        EmployeeType = _employeeRepository.GetEmployeeTypeById(employeeList[i].EmployeeTypeId),
                        Department = _employeeRepository.GetDepartmentById(employeeList[i].DepartmentId),
                        Role = _employeeRepository.GetRoleNameByRoleId(employeeList[i].RoleId),
                        DateOfJoining = employeeList[i].DateOfJoining,
                        DateOfContract = employeeList[i].DateOfContract,
                        DateOfRelieving = employeeList[i].DateOfRelieving,
                        ReportingManager = reportingManager.Count > 0 ? reportingManager[0].EmployeeFullName : "",
                        IsActive = employeeList[i].IsActive,
                        CreatedOn = employeeList[i].CreatedOn,
                        CreatedBy = employeeList[i].CreatedBy,
                        FormattedEmployeeID = employeeList[i].FormattedEmployeeId,
                        DesignationId = employeeList[i].DesignationId,
                        Designation = _employeeRepository.GetDesignationById(employeeList[i].DesignationId),
                        Gender = employeeList[i].Gender,
                        Maritalstatus = employeeList[i].Maritalstatus,
                        SystemRole = _systemRoleRepository.GetSystemRoleNameByRoleId(employeeList[i].SystemRoleId),
                        ProbationStatus = _probationStatusRepository.GetProbationStatusNameById(employeeList[i].ProbationStatusId),
                        Location = _employeeRepository.GetLocationNameByLocationId((employeeList[i].LocationId)),
                        DateOfBirth = employeeList[i].BirthDate,
                        //PersonalMobileNumber = employeeList[i].PersonalMobileNumber,
                        WeddingAnniversary = employeeList[i].WeddingAnniversary,
                        Skillset = skillSet,
                        EmployeeDependent = dependent,
                        EmployeeShiftDetails = _employeeShiftDetailsRepository.GetEmployeeShiftByEmployeeId(employeeList[i].EmployeeID),
                        Employee = employeeList[i]
                    };
                    listOfEmployee.Add(employee);
                }
            }
            return listOfEmployee;
        }
        #endregion

        #region Delete Employee
        public async Task<bool> DeleteEmployeeById(int EmployeeId, int modifiedBy)
        {
            try
            {
                List<EmployeeDependent> employeeDependents = _employeeDependentRepository.GetEmployeeDependentByEmployeeId(EmployeeId);
                foreach (EmployeeDependent employeeDependent in employeeDependents)
                {
                    _employeeDependentRepository.Delete(employeeDependent);
                }
                await _employeeDependentRepository.SaveChangesAsync();
                EmployeesViewModel employeeViewModel = new();
                employeeViewModel = _employeeRepository.GetEmployeeById(EmployeeId);
                Employees employees = employeeViewModel?.Employee;
                if (employees != null)
                {
                    employees.IsActive = false;
                    employees.ModifiedOn = DateTime.UtcNow;
                    employees.ModifiedBy = modifiedBy;
                    _employeeRepository.Update(employees);
                    await _employeeRepository.SaveChangesAsync();
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #region Get Employee Name By Id
        public List<EmployeeName> GetEmployeeNameById(List<int> listEmployeeId)
        {
            try
            {
                return _employeeRepository.GetEmployeeNameById(listEmployeeId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get finance manager
        public int GetfinanceManagerId()
        {
            try
            {
                return _roleRepository.GetFinanceManagerId();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get reporting employee list
        public List<int> GetReportingEmployeeById(int? employeeId)
        {
            try
            {
                return _employeeRepository.GetReportingEmployeeById(employeeId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get employee list
        public List<EmployeeList> GetEmployeeList(string pRole = "")
        {
            try
            {
                return _employeeRepository.GetEmployeeList(pRole);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get role name by id
        public List<RoleName> GetRoleNameById(List<int> lstRoleId)
        {
            try
            {
                return _roleRepository.GetRoleNameById(lstRoleId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get team member
        public List<TeamMemberDetails> GetTeamMemberDetails(List<TeamMemberDetails> resources)
        {
            return _employeeRepository.GetTeamMemberDetails(resources);
        }
        #endregion

        #region Insert Or Update Role Permissions
        public async Task<bool> InsertOrUpdateRolePermissions(List<RolePermissions> rolePermissions)
        {
            if (rolePermissions?.Count > 0)
            {
                List<RolePermissions> lstRolePermissions = _rolePermissionRepository.GetRolePermissionsByRoleId((int)rolePermissions?[0].RoleId);
                foreach (RolePermissions permission in lstRolePermissions)
                {
                    _rolePermissionRepository.Delete(permission);
                }
                await _rolePermissionRepository.SaveChangesAsync();
            }
            foreach (RolePermissions permission in rolePermissions)
            {
                if (permission?.RolePermissionId == 0)
                {
                    RolePermissions rolePermission = new()
                    {
                        CreatedBy = permission.CreatedBy,
                        CreatedOn = DateTime.UtcNow,
                        FeatureId = permission.FeatureId,
                        IsEnabled = permission.IsEnabled,
                        ModuleId = permission.ModuleId,
                        RoleId = permission.RoleId
                    };
                    await _rolePermissionRepository.AddAsync(rolePermission);
                }
            }
            await _rolePermissionRepository.SaveChangesAsync();
            return true;
        }
        #endregion

        #region Get Role Permission's By Email or Role Id
        public List<RolesDetail> GetRolePermissionsByEmail(string email, int pRoleId)
        {
            return _rolePermissionRepository.GetRolePermissionsByEmail(email, pRoleId);
        }
        #endregion

        #region Get Module Wise Feature Details
        public List<ModuleWiseFeatureDetails> GetModuleWiseFeatureDetails()
        {
            return _rolePermissionRepository.GetModuleWiseFeatureDetails();
        }
        #endregion

        #region Check department name duplication
        public bool DepartmentNameDuplication(string departmentName)
        {
            return _departmentRepository.DepartmentNameDuplication(departmentName);
        }
        #endregion

        #region Check role name duplication
        public bool RoleNameDuplication(string roleName)
        {
            return _roleRepository.RoleNameDuplication(roleName);
        }
        #endregion

        #region Get all employees details
        public List<Employees> GetAllEmployeesDetails()
        {
            return _employeeRepository.GetAllEmployeesDetails();
        }
        #endregion

        #region Get Employee List For reporting manager
        public List<EmployeeList> GetEmployeeListForManager(int employeeId)
        {
            return _employeeRepository.GetEmployeeListForManager(employeeId);
        }
        #endregion

        #region Get employees master data for search  
        /// <summary>
        /// Get employees master data for search  
        /// </summary>
        /// <returns></returns>
        public List<SearchEmployeesMasterDataViewModel> GetEmployeesMasterDataForSearch()
        {
            return _employeeRepository.GetEmployeesMasterDataForSearch();
        }
        #endregion

        #region Get role name list
        public List<RoleName> GetRoleNameList()
        {
            return _roleRepository.GetSystemRoleNameList();

        }
        #endregion

        #region Get Designation List
        public List<Designation> GetDesignationList()
        {
            return _roleRepository.GetDesignationList();

        }
        #endregion

        #region Get BU Accountable For Projects
        public List<BUAccountableForProject> GetBUAccountableForProjects(int? departmentHeadId = 0)
        {
            return _roleRepository.GetBUAccountableForProjects(departmentHeadId);
        }
        #endregion

        #region Get active employee list   
        /// <summary>
        /// Get active employee list  
        /// </summary>
        /// <returns></returns>
        public List<int> GetActiveEmployeeIdList()
        {
            return _employeeRepository.GetActiveEmployeeIdList();
        }
        #endregion

        #region Get admin employee id   
        /// <summary>
        /// Get admin employee id  
        /// </summary>
        /// <returns></returns>
        public int GetAdminEmployeeId(string roleName)
        {
            return _employeeRepository.GetAdminEmployeeId(roleName);
        }
        #endregion

        #region Get all skillset
        public List<Skillsets> GetAllSkillset()
        {
            return _skillsetRepository.GetAllSkillset();
        }
        #endregion

        #region Get all employee skillset
        public List<EmployeesSkillset> GetAllEmployeesSkillset()
        {
            return _employeesSkillsetRepository.GetAllEmployeesSkillset();
        }
        #endregion

        #region Get Module Description List
        public List<Modules> GetModuleDescription()
        {
            return _rolePermissionRepository.GetModuleDescription();
        }
        #endregion

        #region Get Holiday Employee Master data
        public HolidayEmployeeMasterData GetHolidayEmployeeMasterData()
        {
            HolidayEmployeeMasterData holidayEmployeeMasterData = new()
            {
                DepartmentList = _departmentRepository.GetAllDepartmentName(),
                LocationList = _employeeRepository.GetAllLocationName()
            };
            return holidayEmployeeMasterData;
        }
        #endregion

        #region Get report details by employee id
        public List<Reports> GetReportDetailsByEmployeeId(int employeeId, int employeeCategoryId)
        {
            return _employeeRepository.GetReportsList(employeeId, employeeCategoryId);

        }
        #endregion

        #region Get employee type name by id
        public List<EmployeeTypeNames> GetEmployeeTypeNameById(List<int> employeesTypeId)
        {
            return _employeeTypeRepository.GetEmployeeTypeNameById(employeesTypeId);
        }
        #endregion

        #region Get Department list
        /// <summary>
        /// Get Department list
        /// </summary>
        /// <returns></returns>
        public List<Department> GetDepartmentDropDownList()
        {

            return _employeeRepository.GetEmployeeDepartmentList();
        }
        #endregion

        #region Get Location list
        /// <summary>
        /// Get Location list
        /// </summary>
        /// <returns></returns>
        public List<EmployeeLocation> GetLocationDropDownList()
        {

            return _employeeRepository.GetEmployeeLocationList();
        }
        #endregion

        #region Get Employee Leaves Master Data
        public LeavesMasterDataView GetEmployeeLeavesMasterData()
        {
            LeavesMasterDataView leavesMasterDataView = new()
            {
                EmployeeDepartmentList = _employeeRepository.GetEmployeeDepartmentList(),
                RoleNamesList = _roleRepository.GetSystemRoleNameList(),
                EmployeeTypeList = _employeeRepository.GetEmployeeTypeList(),
                EmployeeList = _employeeRepository.GetEmployeeDetailsList(),
                EmployeeLocationList = _employeeRepository.GetEmployeeLocationList(),
                EmployeeDesignationList = _employeeRepository.GetDesignationList(),
                ProbationStatusList = _employeeRepository.GetAllProbationStatusDetails()
            };
            //if (leavesMasterDataView?.EmployeeList != null) 
            //{ 
            //foreach(var item in leavesMasterDataView?.EmployeeList)
            //    {
            //        if(item?.FormattedEmployeeId != null)
            //        {
            //            item.EmployeeName = (item?.FormattedEmployeeId + " " + item?.EmployeeName);
            //        }
            //    }
            //}
            return leavesMasterDataView;
        }
        #endregion

        #region Get employee LeaveAdjustment
        public List<LeaveAdjustment> GetEmployeeLeaveAdjustment(EmployeeListView employee)
        {
            List<LeaveAdjustment> leaveAdjustment = new();
            var employeeList = _employeeRepository.GetEmployees(employee);
            if (employeeList?.Count > 0)
            {
                for (int i = 0; i < employeeList?.Count; i++)
                {
                    LeaveAdjustment leave = new()
                    {
                        EmployeeID = employeeList[i].EmployeeID,
                        EmployeeName = employeeList[i].FirstName + " " + employeeList[i].LastName,
                        Department = _employeeRepository.GetDepartmentById(employeeList[i].DepartmentId),
                        DepartmentId = employeeList[i].DepartmentId,
                        Designation = _employeeRepository.GetDesignationById(employeeList[i].DesignationId),
                        FormattedEmployeeId = employeeList[i].FormattedEmployeeId,
                        DOJ = employeeList[i].DateOfJoining,
                    };
                    leaveAdjustment.Add(leave);
                }
            }
            return leaveAdjustment;
        }
        #endregion

        #region Get employee LeaveAdjustment With Pagination
        public List<EmployeeLeaveAdjustmentView> GetEmployeeLeaveAdjustment(EmployeeLeaveAdjustmentFilterView employeeLeaveAdjustmentView)
        {
            //LeaveAdjustmentView leaveAdjustment = new()
            //{
            //    LeaveAdjustments = _employeeRepository.GetEmployeeLeaveAdjustmentListWithFilter(employeeLeaveAdjustmentView),                
            //};
            //if (employeeLeaveAdjustmentView.PageNumber == 0 && !employeeLeaveAdjustmentView.IsFilterApplied)
            //{
            //    leaveAdjustment.LeavesMasterData = new()
            //    {
            //        EmployeeDepartmentList = _employeeRepository.GetEmployeeDepartmentList(),
            //        EmployeeDesignationList = _employeeRepository.GetDesignationList(),
            //    };
            //}
            //return leaveAdjustment;
            return _employeeRepository.GetEmployeeLeaveAdjustmentListWithFilter(employeeLeaveAdjustmentView);
        }
        #endregion													
        #region Get employee Department by employee id
        public int GetEmployeeDepartmentIdByEmployeeId(int employeeId)
        {
            return _employeeRepository.GetEmployeeDepartmentIdByEmployeeId(employeeId);

        }
        #endregion

        #region Get employee Attendance Details
        public List<EmployeeAttendanceDetails> GetEmployeeAttendanceDetails(EmployeesAttendanceFilterView employeesAttendanceFilterView)
        {
            //EmployeeAttendanceDetailsView employeesAttendance = new()
            //{
            //    AttendanceDetailsList = _employeeRepository.GetAllEmployeesAttendanceDetails(employeesAttendanceFilterView),
            //};
            //if (employeesAttendanceFilterView.PageNumber == 0 && !employeesAttendanceFilterView.IsFilterApplied)
            //{
            //    employeesAttendance.DepartmentList = _employeeRepository.GetEmployeeDepartmentList();
            //    employeesAttendance.DesignationList = _employeeRepository.GetDesignationList();
            //    employeesAttendance.LocationList = _employeeRepository.GetEmployeeLocationList();
            //}
            //return employeesAttendance;
            return _employeeRepository.GetAllEmployeesAttendanceDetails(employeesAttendanceFilterView);
        }
        #endregion

        #region Get employee Department by employee id
        public List<EmployeeList> GetEmployeesForManagerId(int managerEmployeeID)
        {
            return _employeeRepository.GetEmployeesForManagerId(managerEmployeeID);

        }
        #endregion

        #region Get employee details
        public List<EmployeeDetails> GetEmployeeAvailabilityDetails(int employeeId)
        {
            return _employeeRepository.GetEmployeeAvailabilityDetails(employeeId);
        }
        #endregion

        #region Get employee detailsby manager id
        public List<EmployeeAssociates> GetEmployeeDetailByManagerId(int employeeId)
        {
            return _employeeRepository.GetEmployeeDetailByManagerId(employeeId);
        }
        #endregion

        #region Get associate home report
        public List<HomeReportData> GetAssociateHomeReport()
        {
            return _employeeRepository.GetAssociateHomeReport();
        }
        #endregion

        #region Sync User From AD
        public async Task<List<Employees>> SyncUserFromAD(List<ADUserList> userList, int userId, int? pShiftDetailsId = 0)
        {
            try
            {
                _employeeRepository.UpdateAllEmployeesAsInacive();
                List<Employees> employeeList = new List<Employees>();

                foreach (ADUserList list in userList)
                {
                    if (list.userPrincipalName == null || (list.userPrincipalName != null && !list.userPrincipalName.EndsWith("@tvsnext.io")))
                        continue;
                    if (list.mail != null && !list.mail.EndsWith("@tvsnext.io"))
                        continue;
                    Employees employee = new();
                    employee = _employeeRepository.GetEmployeeByEmailId(list.userPrincipalName);
                    string strLocatoin = "Chennai", strDepartmentName = "Engineering";
                    string strEmployeesType = "Permanent", strRoleName = "Dev Engineer", strEmployeeCategoryName = "Individual";
                    if (list.officeLocation == null || list.officeLocation == "TVSN-CHN" || list.officeLocation == "TVSN-HO") strLocatoin = "Chennai";
                    else if (list.officeLocation == "TVSN-BAN") strLocatoin = "Bangalore";
                    else if (list.officeLocation == "TVSN-USA") strLocatoin = "New Jersey";
                    if (list.userPrincipalName != null && list.userPrincipalName.Contains("-")) strEmployeesType = "Contract";
                    int intDepartmentId = 0, intLocationId = 0, intEmployeeTypeId = 0;
                    int intRoleId = 0, intEmployeeCategoryId = 0;
                    intLocationId = _employeeRepository.GetEmployeeLocationIdByName(strLocatoin);
                    intEmployeeTypeId = _employeeRepository.GetEmployeeTypeIdByType(strEmployeesType);
                    intDepartmentId = _employeeRepository.GetDepartmentByName(strDepartmentName);
                    intRoleId = _roleRepository.GetRoleByName(strRoleName);
                    intEmployeeCategoryId = _employeeCategoryRepository.GetEmployeeCategoryByNameAndDepartmentId(intDepartmentId, strEmployeeCategoryName);
                    bool blnEmployeeStatus = true;
                    if (list.surname == null && list.givenName == null) blnEmployeeStatus = false;
                    if (employee != null)
                    {
                        employee.FirstName = (employee.FirstName == null || employee.FirstName == "") ? list.surname : employee.FirstName;
                        employee.LastName = (employee.LastName == null || employee.LastName == "") ? list.givenName : employee.LastName;
                        employee.LocationId = (employee.LocationId == null || employee.LocationId == 0) ? intLocationId : employee.LocationId;
                        employee.EmployeeTypeId = (employee.EmployeeTypeId == null || employee.EmployeeTypeId == 0) ? intEmployeeTypeId : employee.EmployeeTypeId;
                        employee.ModifiedOn = DateTime.UtcNow;
                        employee.ModifiedBy = userId;
                        employee.IsActive = blnEmployeeStatus;
                        employee.Mobile = (employee.Mobile == null || employee.Mobile == "") ? list.mobilePhone : employee.Mobile;
                        employee.JobTitle = (employee.JobTitle == null || employee.JobTitle == "") ? list.jobTitle : employee.JobTitle;
                        employee.DepartmentId = (employee.DepartmentId == null || employee.DepartmentId == 0) ? intDepartmentId : employee.DepartmentId;
                        employee.RoleId = (employee.RoleId == null || employee.RoleId == 0) ? intRoleId : employee.RoleId;
                        employee.EmployeeCategoryId = (employee.EmployeeCategoryId == null || employee.EmployeeCategoryId == 0) ? intEmployeeCategoryId : employee.EmployeeCategoryId;
                        //employee.PersonalMobileNumber = (employee.PersonalMobileNumber == null || employee.PersonalMobileNumber == "") ? list.mobilePhone : employee.PersonalMobileNumber;
                        _employeeRepository.Update(employee);
                    }
                    else
                    {
                        DateTime? dtDateOfJoining = null, dtDateOfContract = null;
                        if (strEmployeesType == "Permanent") dtDateOfJoining = DateTime.UtcNow;
                        if (strEmployeesType == "Contract") dtDateOfContract = DateTime.UtcNow;
                        employee = new()
                        {
                            EmailAddress = list.userPrincipalName,
                            FirstName = list.surname,
                            LastName = list.givenName,
                            LocationId = intLocationId,
                            EmployeeTypeId = intEmployeeTypeId,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = userId,
                            IsActive = blnEmployeeStatus,
                            Mobile = list.mobilePhone,
                            JobTitle = list.jobTitle,
                            DateOfJoining = dtDateOfJoining,
                            DateOfContract = dtDateOfContract,
                            DepartmentId = intDepartmentId,
                            RoleId = intRoleId,
                            EmployeeCategoryId = intEmployeeCategoryId,
                            //PersonalMobileNumber = list.mobilePhone
                        };
                        await _employeeRepository.AddAsync(employee);
                        await _employeeRepository.SaveChangesAsync();
                        int intEmployeeID = employee.EmployeeID;
                        if (intEmployeeID > 0 && pShiftDetailsId > 0)
                            AddEmployeeShiftDetails(userId, intEmployeeID, pShiftDetailsId);

                        employeeList?.Add(employee);

                    }
                }
                await _employeeRepository.SaveChangesAsync();
                await _employeeShiftDetailsRepository.SaveChangesAsync();

                return employeeList;
            }
            catch
            {
                throw;
            }
        }
        #endregion

        #region Add Employee Shift Details
        private async void AddEmployeeShiftDetails(int pUserId, int pEmployeeID, int? pShiftDetailsId)
        {

            EmployeeShiftDetails employeeShiftDetails = new()
            {
                ShiftDetailsId = pShiftDetailsId,
                ShiftFromDate = DateTime.Now,
                CreatedOn = DateTime.UtcNow,
                CreatedBy = pUserId,
                EmployeeID = pEmployeeID
            };
            await _employeeShiftDetailsRepository.AddAsync(employeeShiftDetails);
        }
        #endregion

        #region Get Appraisal Dept Role Master Data
        public AppraisalMasterData GetAppraisalDeptRoleMasterData()
        {

            AppraisalMasterData appraisalMasterData = new()
            {
                Department = _employeeRepository.GetEmployeeDepartmentList(),
                Roles = _employeeRepository.GetRolesList(),
                EmployeesTypes = _employeeRepository.GetEmployeeTypeList(),
                ReportingManagerEmployeeList = _employeeRepository.GetManagerEmployeeList()
            };

            return appraisalMasterData;
        }
        #endregion

        #region Add or update EmployeeCategory
        public async Task<int> AddOrUpdateEmployeeCategory(EmployeeCategory pEmployeeCategory)
        {
            int employeeCategoryId = 0;
            try
            {
                EmployeeCategory employeeCategory = new();
                if (pEmployeeCategory != null)
                {
                    employeeCategory.EmployeeCategoryName = pEmployeeCategory.EmployeeCategoryName;
                    employeeCategory.DepartmentId = pEmployeeCategory.DepartmentId;
                    employeeCategory.Description = pEmployeeCategory.Description;
                    employeeCategory.CreatedOn = DateTime.UtcNow;
                    employeeCategory.CreatedBy = pEmployeeCategory.CreatedBy;
                    await _employeeCategoryRepository.AddAsync(employeeCategory);
                    await _employeeCategoryRepository.SaveChangesAsync();
                    employeeCategoryId = employeeCategory.EmployeeCategoryId;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return employeeCategoryId;
        }
        #endregion

        #region Get department location name by id
        public DepartmentLocationName GetDepartmentLocationNameById(DepartmentLocationName departmentLocation)
        {

            DepartmentLocationName depatmentLocation = new()
            {
                Department = _departmentRepository.GetDepartmentNameById(departmentLocation?.DepartmentId),
                Location = _employeeRepository.GetLocationNameById(departmentLocation?.LocationId)
            };
            return depatmentLocation;
        }
        #endregion

        #region Get All Employee List For reporting manager
        public List<EmployeeViewDetails> GetAllEmployeeListForManagerReport(int employeeId, bool isAll = false)
        {
            return _employeeRepository.GetAllEmployeeListForManagerReport(employeeId, isAll);
        }
        #endregion

        #region Get Employee Shift Details 
        public List<EmployeeShiftDetailsView> GetEmployeeShiftDetails(int employeeID)
        {
            return _employeeRepository.GetEmployeeShiftDetails(employeeID);
        }
        #endregion

        #region Get Employees By Permanent and Active New
        public AppraisalManagerEmployeeDetailsView AppraisalManagerEmployeeDetails(EmployeelistForAppraisalMaster details)
        {

            AppraisalManagerEmployeeDetailsView employeeDEtails = _employeeRepository.AppraisalManagerEmployeeDetails(details.ManagerID);
            if (employeeDEtails != null && details?.listEmployeeID?.Count > 0)
            {
                employeeDEtails.EmployeeDetails = _employeeRepository.GetEmployeeNameById(details?.listEmployeeID);
            }
            return employeeDEtails;
        }
        #endregion

        #region Get Employees By Permanent and Active
        public List<AppraisalManagerEmployeeDetailsView> GetEmployeesByPermanentandActive()
        {
            return _employeeRepository.GetEmployeesByPermanentandActive();
        }
        #endregion

        #region Get Employee and Manager By EmployeeID
        public EmployeeandManagerView GetEmployeeandManagerByEmployeeID(int employeeID)
        {
            return _employeeRepository.GetEmployeeandManagerByEmployeeID(employeeID);
        }
        #endregion

        #region Get Employee Category details by employee id
        public EmployeeCategoryView GetEmployeeCategoryDetailsByEmployeeId(int employeeId, int employeeCategoryId)
        {
            return _employeeRepository.GetEmployeeCategoryDetailsByEmployeeId(employeeId, employeeCategoryId);

        }
        #endregion

        #region Get Employees List By Department
        public List<EmployeeandManagerView> GetEmployeesListByDepartment(EmployeeListByDepartment employeeView)
        {
            return _employeeRepository.GetEmployeesListByDepartment(employeeView);
        }
        #endregion

        #region Get all employee type
        public List<EmployeesTypes> GetEmployeeTypeList()
        {
            return _employeeTypeRepository.GetEmployeeTypeList();
        }
        #endregion

        #region Get All Employee for Leave
        public List<EmployeeDetailsForLeaveView> GetAllEmployeeforLeave()
        {
            return _employeeTypeRepository.GetAllEmployeeforLeave();
        }

        #endregion

        #region Get Employee Department And Location
        public EmployeeDepartmentAndLocationView GetEmployeeDepartmentAndLocation(int employeeId)
        {
            EmployeeDepartmentAndLocationView employee = _employeeRepository.GetEmployeeDepartmentAndLocation(employeeId);
            if (employee == null)
            {
                employee = new();
            }
            employee.ReportingEmployeeList = _employeeRepository.GetEmployeesForManagerId(employeeId);
            if (employee?.ReportingEmployeeList != null)
            {
                foreach (var item in employee?.ReportingEmployeeList)
                {
                    if (item?.FormattedEmployeeId != null)
                    {
                        item.EmployeeName = item?.FormattedEmployeeId + " " + item?.EmployeeName;
                    }
                }
            }
            employee.EmployeeShiftDetails = _employeeRepository.GetShiftListByemployeeId(employeeId);
            employee.EmployeeDetails = _employeeRepository.GetEmployeeDetailsById(employeeId);
            return employee;
        }
        #endregion
        #region 
        public List<EmployeeList> GetEmployeeListByManagerId(int? employeeId)
        {
            return _employeeRepository.GetEmployeeListByManagerId(employeeId);
        }
        #endregion
        #region Get employee Details by employee id
        public Employees GetEmployeeByEmployeeId(int employeeId)
        {
            return _employeeRepository.GetEmployeeByEmployeeId(employeeId);
        }
        #endregion
        #region Get grant leave approver
        public GrantLeaveApproverView GetGrantLeaveApprover(int employeeId, string hrDepartmentName)
        {
            return _employeeRepository.GetGrantLeaveApprover(employeeId, hrDepartmentName);

        }
        #endregion
        #region Get employee EmployeeName Department Location details
        public EmployeeNameDepartmentAndLocationView EmployeeNameDepartmentLocation(EmployeeNameDepartmentLocation employeeDetails)
        {
            EmployeeNameDepartmentAndLocationView employee = new EmployeeNameDepartmentAndLocationView();
            employee.EmployeeDepartmentAndLocationDetails = _employeeRepository.GetEmployeeDepartmentAndLocation(employeeDetails.EmployeeId);
            employee.EmployeeName = _employeeRepository.GetEmployeeNameById(employeeDetails.EmployeeList);
            employee.EmployeeShiftDetails = _employeeRepository.GetShiftListByemployeeId(employeeDetails.EmployeeId);
            return employee;
        }
        #endregion
        #region Get leave employee details
        public EmployeeNameDepartmentAndLocationView LeaveEmployeeDetails(EmployeeNameDepartmentLocation employeeDetails)
        {
            EmployeeNameDepartmentAndLocationView employee = new EmployeeNameDepartmentAndLocationView();
            Employees empDetails = _employeeRepository.GetEmployeeByEmployeeId(employeeDetails.EmployeeId);
            employee.BirthDate = empDetails.BirthDate;
            employee.WeddingAnniversary = empDetails.WeddingAnniversary;
            employee.EmployeeName = _employeeRepository.GetEmployeeNameById(employeeDetails.EmployeeList);
            return employee;
        }
        #endregion
        #region Get employee shift details
        public List<EmployeeShiftDetailsView> GetEmployeeShiftDetailsById(int employeeId)
        {
            return _employeeRepository.GetShiftListByemployeeId(employeeId);

        }
        #endregion

        #region Get New Employee Details for Leave
        public EmployeeDetailsForLeaveView GetNewEmployeeDetailsbyID(int EmployeeID)
        {
            return _employeeTypeRepository.GetNewEmployeeDetailsbyID(EmployeeID);
        }
        #endregion 
        #region Get all active employee list
        public List<EmployeeDetail> GetAllActiveEmployeeDetails()
        {
            return _employeeRepository.GetAllActiveEmployeeDetails();
        }
        #endregion
        #region 
        public EmployeeShiftDetails GetShifByDate(int EmployeeID, DateTime date)
        {
            return _employeeRepository.GetShifByDate(EmployeeID, date);
        }
        #endregion
        #region 
        public EmployeeandManagerView GetEmployeeAndApproverDetails(int employeeId, int approverId)
        {
            return _employeeRepository.GetEmployeeAndApproverDetails(employeeId, approverId);
        }
        #endregion
        #region 
        public EmployeeManagerAndHeadDetailsView GetEmployeeManagerAndHeadDetails(int employeeId, int approverId)
        {
            return _employeeRepository.GetEmployeeManagerAndHeadDetails(employeeId, approverId);
        }
        #endregion
        public EmployeeName GetEmployeeNameByEmployeeId(int employeeId)
        {
            return _employeeRepository.GetEmployeeNameByEmployeeId(employeeId);
        }

        #region Get Employee Detail with Designation By Id
        public List<ResignedEmployeeView> GetEmployeeDetailsById(List<int> listEmployeeId)
        {
            try
            {
                return _employeeRepository.GetEmployeeDetailsById(listEmployeeId);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Insert And Update System Role
        public async Task<int> InsertOrUpdateSystemRole(SystemRoles pRole)
        {
            int roleId = 0;
            try
            {
                SystemRoles role = new();
                if (pRole != null)
                {
                    role.RoleName = pRole.RoleName;
                    role.RoleShortName = pRole.RoleShortName;
                    role.RoleDescription = pRole.RoleDescription;
                    role.CreatedOn = DateTime.UtcNow;
                    role.CreatedBy = pRole.CreatedBy;
                    await _systemRoleRepository.AddAsync(role);
                    await _systemRoleRepository.SaveChangesAsync();
                    roleId = role.RoleId;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return roleId;
        }
        #endregion

        #region Check system role name duplication
        public bool SystemRoleNameDuplication(string roleName)
        {
            return _systemRoleRepository.RoleNameDuplication(roleName);
        }
        #endregion

        #region Update Employee Status 
        public async Task<bool> UpdateEmployeeStatus(EmployeeStatusView employeeStatus)
        {
            try
            {
                bool isAudit = true;
                Employees employee = _employeeRepository.GetEmployeeDetailByEmployeeId(employeeStatus.EmployeeId);

                if (employee != null)
                {
                    if (employee.IsActive == employeeStatus.IsEnabled)
                    {
                        isAudit = false;
                    }
                    employee.IsActive = employeeStatus.IsEnabled;
                    employee.ModifiedOn = DateTime.UtcNow;
                    employee.ModifiedBy = employeeStatus.ModifiedBy;
                    _employeeRepository.Update(employee);
                    await _employeeRepository.SaveChangesAsync();

                    if (isAudit == true)
                    {
                        EmployeeAudit audit = new EmployeeAudit();
                        audit.EmployeeId = employeeStatus.EmployeeId;
                        audit.ChangeRequestID = Guid.NewGuid();
                        audit.CreatedOn = DateTime.UtcNow;
                        audit.CreatedBy = employeeStatus.ModifiedBy;
                        audit.Field = "Is Active";
                        audit.ActionType = "Update";
                        audit.OldValue = (!employeeStatus.IsEnabled).ToString();
                        audit.NewValue = (employeeStatus.IsEnabled).ToString();
                        _auditRepository.AddAsync(audit);
                        _auditRepository.SaveChangesAsync();
                    }

                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Get employee resignation details
        public async Task<List<EmployeeResignationDetailsView>> GetEmployeeResignationDetails(List<EmployeeResignationDetailsView> employeeDetails)
        {
            return _employeeRepository.GetEmployeeResignationDetails(employeeDetails);
        }
        #endregion 
        #region Get employee resignation approver
        public async Task<ResignationApproverView> GetResignationApprover(int employeeId, string hrDepartmentName)
        {
            return _employeeRepository.GetResignationApprover(employeeId, hrDepartmentName);
        }
        #endregion
        #region Update employee relieving date
        public async Task<bool> UpdateEmployeeRelievingDate(UpdateEmployeeRelievingDate employee)
        {
            Employees employeeModel = _employeeRepository.GetEmployeeByEmployeeId(employee.EmployeeId);
            if (employeeModel != null)
            {
                if (employee.IsRevertRelievingDate == false)
                {
                    employeeModel.DateOfRelieving = employee.RelievingDate;
                    employeeModel.ResignationDate = employee.ResignationDate;
                    employeeModel.ResignationReason = employee.ResignationReason;
                    employeeModel.ResignationStatus = employee.ResignationStatus;
                    employeeModel.ExitType = employeeModel.ExitType == null ? employee.ExitType : employeeModel.ExitType;
                    employeeModel.ModifiedOn = DateTime.UtcNow;
                    employeeModel.ModifiedBy = employee.ModifiedBy;
                    employeeModel.IsResign = true;
                }
                else
                {
                    employeeModel.DateOfRelieving = null;
                    employeeModel.ResignationDate = null;
                    employeeModel.ResignationReason = null;
                    employeeModel.ResignationStatus = null;
                    employeeModel.ExitType = null;
                    employeeModel.ModifiedOn = DateTime.UtcNow;
                    employeeModel.ModifiedBy = employee.ModifiedBy;
                }
                _employeeRepository.Update(employeeModel);
                await _employeeRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
        #region Update employee relieving date
        public async Task<bool> UpdateEmployeePersonalInfo(UpdateEmployeeRelievingDate employee)
        {
            EmployeesPersonalInfo employeeModel = _employeesPersonalInfoRepository.GetEmployeePersonalIdByEmployeeID(employee.EmployeeId);
            if (employeeModel != null)
            {
                employeeModel.PersonalMobileNumber = employee.PersonalMobileNumber;
                employeeModel.OtherEmail = employee.PersonalEmailId;
                employeeModel.ModifiedOn = DateTime.UtcNow;
                employeeModel.ModifiedBy = employee.ModifiedBy;
                _employeesPersonalInfoRepository.Update(employeeModel);
                await _employeesPersonalInfoRepository.SaveChangesAsync();
                return true;
            }
            return false;
        }
        #endregion
        #region Get employee resignation Master Data
        public List<ResignationEmployeeMasterView> GetResignationEmployeeMasterData(List<int> employeeId)
        {
            return _employeeRepository.GetResignationEmployeeMasterData(employeeId);
        }
        #endregion
        #region Get exit interview employee data
        public List<ResignationInterviewDetailView> GetEmployeeExitInterviewDetails(List<ResignationInterviewDetailView> employeeDetails)
        {
            return _employeeRepository.GetEmployeeExitInterviewDetails(employeeDetails);
        }
        #endregion 
        #region Get project customer employee list
        public ProjectCustomerEmployeeList GetEmployeeListForProjectAndCustomer(int employeeId, bool isAllEmployee)
        {
            return _employeeRepository.GetEmployeeListForProjectAndCustomer(employeeId, isAllEmployee);
        }
        #endregion

        #region Get Employee Resignation date
        public DateTime? GetEmployeeResignationDate(int employeeId)
        {
            return _employeeRepository.GetEmployeeResignationDate(employeeId);
        }
        #endregion

        #region Get employee Exit interview Master Data
        public List<ResignationEmployeeMasterView> GetExitEmployeeMaster(List<int> employeeId)
        {
            return _employeeRepository.GetExitEmployeeMaster(employeeId);
        }
        #endregion

        #region Get employee list by systemRole
        public List<int> GetEmployeeListBySystemRole(string sRole = "")
        {
            try
            {
                return _employeeRepository.GetEmployeesListBySystemRole(sRole);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get checklist employee role
        public List<string> GetExitCheckListRole(int employeeId, int loginUserId, bool isAllReportees)
        {
            return _employeeRepository.GetExitCheckListRole(employeeId, loginUserId, isAllReportees);
        }
        #endregion
        #region Get checklist employee list
        public List<EmployeeViewDetails> GetCheckListEmployeeList(int employeeId, bool isAll)
        {
            return _employeeRepository.GetCheckListEmployeeList(employeeId, isAll);
        }
        #endregion 
        #region Get checklist employee details
        public List<ChecklistEmployeeView> GetCheckListEmployeeDetails(List<ChecklistEmployeeView> employeeDetails)
        {
            return _employeeRepository.GetCheckListEmployeeDetails(employeeDetails);
        }
        #endregion
        #region Get reportees checklist employee details
        public ReporteesChecklistEmployeeView GetReporteesCheckListEmployee(int employeeId, bool isAll)
        {
            return _employeeRepository.GetReporteesCheckListEmployee(employeeId, isAll);
        }
        #endregion 
        #region Get employee list for checklist
        public List<ResignationEmployeeMasterView> GetEmployeeListForResignation(int employeeId)
        {
            return _employeeRepository.GetEmployeeListForResignation(employeeId);
        }
        #endregion 
        #region Get resignation employee list
        public List<EmployeeList> GetResignationEmployeeList(int employeeId)
        {
            return _employeeRepository.GetResignationEmployeeList(employeeId);
        }
        #endregion

        #region Get resignation employee Filter list
        public List<ResignationEmployeeMasterView> GetResignationEmployeeListByFilter(ResignationEmployeeFilterView resignationEmployeeFilter)
        {
            return _employeeRepository.GetResignationEmployeeListByFilter(resignationEmployeeFilter);
        }
        #endregion 
        #region Get employee list by system role
        public List<ResignationEmployeeMasterView> GetEmployeesDetailsBySystemRole(string sRole)
        {
            return _employeeRepository.GetEmployeesDetailsBySystemRole(sRole);
        }
        #endregion
        #region Deactivate Employee Status 
        public async Task<bool> DeactivateEmployeeStatus()
        {
            try
            {
                List<Employees> employeeList = _employeeRepository.GetEmployeeListByRelievingDate();
                foreach (Employees employee in employeeList)
                {
                    if (employee != null)
                    {
                        employee.IsActive = false;
                        employee.ModifiedOn = DateTime.UtcNow;
                        employee.ModifiedBy = 1;
                        _employeeRepository.Update(employee);
                        await _employeeRepository.SaveChangesAsync();
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Bulk Insert Employee
        public async Task<List<ImportDataStatus>> BulkInsertEmployee(ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            if (!string.IsNullOrEmpty(importExcelView.Base64Format))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                byte[] bytes = Convert.FromBase64String(importExcelView.Base64Format);
                MemoryStream stream = new MemoryStream(bytes);
                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    EmployeeAuditDataView newData = new EmployeeAuditDataView();
                    DataSet dataset = reader?.AsDataSet();
                    if (dataset?.Tables?.Count > 0)
                    {
                        importExcelView.EmployeeMaster = this.GetEmployeeMasterData("");
                        importExcelView.EmployeeMaster.ReportingManagerList = this.GetEmployeeDropDownList(true);
                        DataTable employeeTable = dataset?.Tables["Basic_info"];
                        if (employeeTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView employeeDetails = await AppendEmployeeToAddView(employeeTable, importExcelView);
                            output.AddRange(employeeDetails?.ImportDataStatuses);
                            foreach (var validRecord in employeeDetails?.Employees)
                            {
                                newData.Employee = validRecord;
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = employeeDetails.EmployeeDetailList.Find(x => x.FormattedEmployeeId == validRecord.FormattedEmployeeId);
                                importStatus.FormattedEmployeeId = validRecord.FormattedEmployeeId;
                                importStatus.EmployeeName = validRecord.EmployeeName;
                                importStatus.EmployeeEmailId = validRecord.EmailAddress;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    await _employeeRepository.AddAsync(validRecord);
                                    await _employeeRepository.SaveChangesAsync();
                                    var result = await AddDesignationHistory(validRecord.EmployeeID, validRecord.DesignationId, validRecord.DesignationEffectiveFrom, validRecord.CreatedBy);
                                    newData.ActionType = "Create";
                                    newData.CreatedBy = (int)validRecord.CreatedBy;
                                    AddAuditFile(newData, null);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                        }
                        DataTable personalInfoTable = dataset?.Tables["Personal_info"];
                        if (personalInfoTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView personalDetails = await AppendEmployeePersonalInfoToAddView(personalInfoTable, importExcelView);
                            output.AddRange(personalDetails?.ImportDataStatuses);
                            foreach (var validRecord in personalDetails?.EmployeePersonalInfo)
                            {
                                newData.EmployeesPersonalInfoDetail = validRecord;
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = personalDetails.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    await _employeesPersonalInfoRepository.AddAsync(validRecord);
                                    await _employeesPersonalInfoRepository.SaveChangesAsync();
                                    newData.ActionType = "Create";
                                    newData.CreatedBy = (int)validRecord.CreatedBy;
                                    AddAuditFile(newData, null);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                        }
                        DataTable workHistoryTable = dataset?.Tables["WorkHistory"];
                        if (workHistoryTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView workHistoryDetail = await AppendWorkHistoryToAddView(workHistoryTable, importExcelView);
                            output.AddRange(workHistoryDetail?.ImportDataStatuses);
                            foreach (var validRecord in workHistoryDetail?.WorkHistories)
                            {
                                newData.WorkHistory = new List<WorkHistory> { validRecord };
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = workHistoryDetail.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    await _workHistoryRepository.AddAsync(validRecord);
                                    await _workHistoryRepository.SaveChangesAsync();
                                    newData.ActionType = "Create";
                                    newData.CreatedBy = (int)validRecord.CreatedBy;
                                    AddAuditFile(newData, null);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();

                        }
                        DataTable educationalDetails = dataset?.Tables["EducationHistory"];
                        if (educationalDetails?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView educationDetails = await AppendEducationDetailToAddView(educationalDetails, importExcelView);
                            output.AddRange(educationDetails?.ImportDataStatuses);
                            foreach (var validRecord in educationDetails?.EducationDetails)
                            {
                                newData.EducationDetail = new List<EducationDetail> { validRecord };
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = educationDetails?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    await _educationDetailRepository.AddAsync(validRecord);
                                    await _educationDetailRepository.SaveChangesAsync();
                                    newData.ActionType = "Create";
                                    newData.CreatedBy = (int)validRecord.CreatedBy;
                                    AddAuditFile(newData, null);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                        }
                        DataTable compensationTable = dataset?.Tables["CompensationDetails"];
                        if (compensationTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView compensationDetails = await AppendCompensationDetailToAddView(compensationTable, importExcelView);
                            output.AddRange(compensationDetails?.ImportDataStatuses);
                            foreach (var validRecord in compensationDetails?.CompensationDetails)
                            {
                                newData.CompensationDetail = new List<CompensationDetail> { validRecord };
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = compensationDetails?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeID);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    await _compensationDetailRepository.AddAsync(validRecord);
                                    await _compensationDetailRepository.SaveChangesAsync();
                                    newData.ActionType = "Create";
                                    newData.CreatedBy = (int)validRecord.CreatedBy;
                                    AddAuditFile(newData, null);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                        }
                        DataTable skillsetTable = dataset?.Tables["Skillset"];
                        if (skillsetTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView skillsetDetails = await AppendSkillSetToAddView(skillsetTable, importExcelView);
                            output.AddRange(skillsetDetails?.ImportDataStatuses);
                            foreach (var validRecord in skillsetDetails?.SkillsetsEmployee)
                            {
                                ImportDataStatus importStatus = new ImportDataStatus();
                                newData.EmployeesSkillsets = new List<EmployeesSkillset> { validRecord };
                                EmployeeStatusDetail employeeStatus = skillsetDetails?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    validRecord.CreatedBy = importExcelView.UploadedBy;
                                    await _employeesSkillsetRepository.AddAsync(validRecord);
                                    await _employeesSkillsetRepository.SaveChangesAsync();
                                    newData.ActionType = "Create";
                                    newData.CreatedBy = (int)validRecord.CreatedBy;
                                    AddAuditFile(newData, null);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                        }
                        DataTable DependencyTable = dataset?.Tables["Dependent"];
                        if (DependencyTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView DependencyDetail = await AppendDependencyToUpdateView(DependencyTable, importExcelView);
                            output.AddRange(DependencyDetail?.ImportDataStatuses);
                            foreach (var validRecord in DependencyDetail?.EmployeeDependency)
                            {
                                newData.EmployeeDependents = new List<EmployeeDependent> { validRecord };
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = DependencyDetail?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeID);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    validRecord.CreatedBy = importExcelView.UploadedBy;
                                    await _employeeDependentRepository.AddAsync(validRecord);
                                    await _employeeDependentRepository.SaveChangesAsync();
                                    newData.ActionType = "Create";
                                    newData.CreatedBy = (int)validRecord.CreatedBy;
                                    AddAuditFile(newData, null);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                        }
                        DataTable specialAbilityTable = dataset?.Tables["SpecialAbility"];
                        if (specialAbilityTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView specialAbilityDetails = await AppendSpecialAbilityToAddView(specialAbilityTable, importExcelView);
                            output.AddRange(specialAbilityDetails?.ImportDataStatuses);
                            foreach (var validRecord in specialAbilityDetails?.SpecialAbility)
                            {
                                EmployeeAuditDataView oldData = new EmployeeAuditDataView();
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = specialAbilityDetails?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    EmployeeSpecialAbility employeeSpecialAbility = new EmployeeSpecialAbility();
                                    employeeSpecialAbility.SpecialAbilityId = (int)validRecord.SpecialAbilityId;
                                    employeeSpecialAbility.EmployeeId = (int)validRecord.EmployeeId;
                                    employeeSpecialAbility.CreatedBy = importExcelView.UploadedBy;
                                    employeeSpecialAbility.CreatedOn = DateTime.Now;
                                    oldData.EmployeeSpecialAbility = new List<EmployeeSpecialAbility>();
                                    Employees employees = await _employeeRepository.GetEmployeeForBUlkUploadByEmployeeId((int)validRecord.EmployeeId);
                                    oldData.Employee = employees;
                                    string oldDataString = JsonConvert.SerializeObject(oldData);
                                    employees.IsSpecialAbility = true;
                                    employees.SpecialAbilityRemark = validRecord.SpecialAbilityRemark != null ? validRecord.SpecialAbilityRemark : employees.SpecialAbilityRemark;
                                    _employeeRepository.Update(employees);
                                    await _employeeRepository.SaveChangesAsync();
                                    await _employeeSpecialAbilityRepository.AddAsync(employeeSpecialAbility);
                                    await _employeeSpecialAbilityRepository.SaveChangesAsync();
                                    newData.EmployeeSpecialAbility = new List<EmployeeSpecialAbility> { employeeSpecialAbility };
                                    newData.ActionType = "Create";
                                    newData.CreatedBy = (int)employeeSpecialAbility.CreatedBy;
                                    newData.Employee = employees;
                                    AddAuditFile(newData, oldDataString);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                        }
                    }
                }
            }
            return output;
        }
        #endregion

        #region Bulk Update  Employee
        public async Task<List<ImportDataStatus>> BulkUpdateEmployee(ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            EmployeeAuditDataView newData = new EmployeeAuditDataView();
            EmployeeAuditDataView oldData = new EmployeeAuditDataView();
            string oldDataString = "";
            if (!string.IsNullOrEmpty(importExcelView.Base64Format))
            {
                System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
                byte[] bytes = Convert.FromBase64String(importExcelView.Base64Format);
                MemoryStream stream = new MemoryStream(bytes);

                using (var reader = ExcelReaderFactory.CreateReader(stream))
                {
                    DataSet dataset = reader?.AsDataSet();
                    if (dataset?.Tables?.Count > 0)
                    {
                        importExcelView.EmployeeMaster = this.GetEmployeeMasterData("");
                        importExcelView.EmployeeMaster.ReportingManagerList = this.GetEmployeeDropDownList(true);
                        DataTable employeeTable = dataset?.Tables["Basic_info"];
                        if (employeeTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView employeeDetails = await AppendEmployeeToUpdateView(employeeTable, importExcelView);
                            output.AddRange(employeeDetails?.ImportDataStatuses);
                            foreach (var validRecord in employeeDetails?.Employees)
                            {
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = employeeDetails.EmployeeDetailList.Find(x => x.FormattedEmployeeId == validRecord.FormattedEmployeeId);
                                importStatus.FormattedEmployeeId = validRecord.FormattedEmployeeId;
                                importStatus.EmployeeName = validRecord.EmployeeName;
                                importStatus.EmployeeEmailId = validRecord.EmailAddress;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    oldData.Employee = _employeeRepository.GetEmployeeById(validRecord.EmployeeID).Employee;
                                    oldDataString = JsonConvert.SerializeObject(oldData);
                                    _employeeRepository.Update(validRecord);
                                    await _employeeRepository.SaveChangesAsync();
                                    newData.Employee = validRecord;
                                    newData.ActionType = "Update";
                                    var result = await AddDesignationHistory(validRecord.EmployeeID, validRecord.DesignationId, validRecord.DesignationEffectiveFrom, validRecord.CreatedBy);
                                    newData.CreatedBy = (int)validRecord.ModifiedBy;
                                    AddAuditFile(newData, oldDataString);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                            oldData = new EmployeeAuditDataView();
                        }
                        DataTable personalInfoTable = dataset?.Tables["Personal_info"];
                        if (personalInfoTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView personalDetails = await AppendEmployeePersonalInfoToUpdateView(personalInfoTable, importExcelView);
                            output.AddRange(personalDetails?.ImportDataStatuses);
                            foreach (var validRecord in personalDetails.EmployeePersonalInfo)
                            {
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = personalDetails.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    oldData.EmployeesPersonalInfoDetail = _employeesPersonalInfoRepository.GetEmployeePersonalInfoById(validRecord.PersonalInfoId);
                                    oldDataString = JsonConvert.SerializeObject(oldData);
                                    _employeesPersonalInfoRepository.Update(validRecord);
                                    await _employeesPersonalInfoRepository.SaveChangesAsync();
                                    newData.ActionType = "Update";
                                    newData.CreatedBy = (int)validRecord.ModifiedBy;
                                    newData.EmployeesPersonalInfoDetail = validRecord;
                                    AddAuditFile(newData, oldDataString);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                            oldData = new EmployeeAuditDataView();
                        }
                        DataTable workHistoryTable = dataset?.Tables["WorkHistory"];
                        if (workHistoryTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView workHistoryDetail = await AppendWorkHistoryToUpdateView(workHistoryTable, importExcelView);
                            output.AddRange(workHistoryDetail?.ImportDataStatuses);
                            foreach (var validRecord in workHistoryDetail?.WorkHistories)
                            {
                                newData.WorkHistory = new List<WorkHistory> { validRecord };
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = workHistoryDetail.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.Remarks = employeeStatus.Remarks;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                try
                                {
                                    oldData.WorkHistory = new List<WorkHistory> { _workHistoryRepository.GetWorkHistoryWorkHistoryId(validRecord.WorkHistoryId) };
                                    oldDataString = JsonConvert.SerializeObject(oldData);
                                    _workHistoryRepository.Update(validRecord);
                                    await _workHistoryRepository.SaveChangesAsync();
                                    newData.ActionType = "Update";
                                    newData.CreatedBy = (int)validRecord.ModifiedBy;
                                    AddAuditFile(newData, oldDataString);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                            oldData = new EmployeeAuditDataView();
                        }
                        DataTable educationalDetails = dataset?.Tables["EducationHistory"];
                        if (educationalDetails?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView educationDetails = await AppendEducationDetailToUpdateView(educationalDetails, importExcelView);
                            output.AddRange(educationDetails?.ImportDataStatuses);
                            foreach (var validRecord in educationDetails?.EducationDetails)
                            {
                                newData.EducationDetail = new List<EducationDetail> { validRecord };
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = educationDetails?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    oldData.EducationDetail = new List<EducationDetail> { _educationDetailRepository.GetEducationDetailEducationDetailId(validRecord.EducationDetailId) };
                                    oldDataString = JsonConvert.SerializeObject(oldData);
                                    _educationDetailRepository.Update(validRecord);
                                    await _educationDetailRepository.SaveChangesAsync();
                                    newData.ActionType = "Update";
                                    newData.CreatedBy = (int)validRecord.ModifiedBy;
                                    AddAuditFile(newData, oldDataString);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                            oldData = new EmployeeAuditDataView();
                        }
                        DataTable compensationTable = dataset?.Tables["CompensationDetails"];
                        if (compensationTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView compensationDetails = await AppendCompensationDetailToUpdateView(compensationTable, importExcelView);
                            output.AddRange(compensationDetails?.ImportDataStatuses);
                            foreach (var validRecord in compensationDetails?.CompensationDetails)
                            {
                                newData.CompensationDetail = new List<CompensationDetail> { validRecord };
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = compensationDetails?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeID);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                try
                                {
                                    oldData.CompensationDetail = new List<CompensationDetail> { _compensationDetailRepository.GetCompendationDetailByCTCId(validRecord.CTCId) };
                                    oldDataString = JsonConvert.SerializeObject(oldData);
                                    _compensationDetailRepository.Update(validRecord);
                                    await _compensationDetailRepository.SaveChangesAsync();
                                    newData.ActionType = "Update";
                                    newData.CreatedBy = (int)validRecord.ModifiedBy;
                                    AddAuditFile(newData, oldDataString);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                            oldData = new EmployeeAuditDataView();
                        }
                        DataTable skillsetTable = dataset?.Tables["Skillset"];
                        if (skillsetTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView skillsetDetails = await AppendSkillSetToAddView(skillsetTable, importExcelView);
                            output.AddRange(skillsetDetails?.ImportDataStatuses);
                            foreach (EmployeesSkillset validRecord in skillsetDetails?.SkillsetsEmployee)
                            {
                                newData.EmployeesSkillsets = new List<EmployeesSkillset> { validRecord };
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = skillsetDetails?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                //var skillsetList = _employeesSkillsetRepository.GetEmployeesSkillsetByEmployeeId(validRecord.EmployeeId);
                                //if (skillsetList?.Count > 0)
                                //{
                                //    try
                                //    {
                                //        foreach (var skillset in skillsetList)
                                //        {
                                //            EmployeesSkillset empSkillset = _employeesSkillsetRepository.GetEmployeesSkillsetById(skillset.EmployeesSkillsetId);
                                //            if (empSkillset?.EmployeesSkillsetId > 0)
                                //            {
                                //                _employeesSkillsetRepository.Delete(empSkillset);
                                //            }
                                //        }
                                //        await _employeesSkillsetRepository.SaveChangesAsync();
                                //    }
                                //    catch(Exception e)
                                //    {
                                //    }
                                //}
                                try
                                {
                                    validRecord.CreatedBy = importExcelView.UploadedBy;
                                    await _employeesSkillsetRepository.AddAsync(validRecord);
                                    await _employeesSkillsetRepository.SaveChangesAsync();
                                    newData.ActionType = "Update";
                                    newData.CreatedBy = (int)validRecord.CreatedBy;
                                    AddAuditFile(newData, null);
                                    importStatus.Status = "Success";

                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }

                            }
                            newData = new EmployeeAuditDataView();
                            oldData = new EmployeeAuditDataView();
                        }
                        DataTable DependencyTable = dataset?.Tables["Dependent"];
                        if (DependencyTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView DependencyDetail = await AppendDependencyToUpdateView(DependencyTable, importExcelView);
                            output.AddRange(DependencyDetail?.ImportDataStatuses);
                            foreach (var validRecord in DependencyDetail?.EmployeeDependency)
                            {
                                newData.EmployeeDependents = new List<EmployeeDependent> { validRecord };
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = DependencyDetail?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeID);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                // var dependentList = _employeeDependentRepository.GetEmployeeDependentByEmployeeId((int)validRecord.EmployeeID);
                                //if (dependentList?.Count > 0)
                                //{
                                //    try
                                //    {
                                //        foreach (EmployeeDependent dependent in dependentList)
                                //        {
                                //            _employeeDependentRepository.Delete(dependent);
                                //        }
                                //        await _employeeDependentRepository.SaveChangesAsync();
                                //    }
                                //    catch(Exception e)
                                //    {
                                //    }
                                //}
                                try
                                {
                                    validRecord.CreatedBy = importExcelView.UploadedBy;
                                    await _employeeDependentRepository.AddAsync(validRecord);
                                    await _employeeDependentRepository.SaveChangesAsync();
                                    newData.ActionType = "Update";
                                    newData.CreatedBy = (int)validRecord.CreatedBy;
                                    AddAuditFile(newData, null);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                            oldData = new EmployeeAuditDataView();
                        }
                        DataTable specialAbilityTable = dataset?.Tables["SpecialAbility"];
                        if (specialAbilityTable?.Rows?.Count > 1)
                        {
                            ImportEmployeeExcelView specialAbilityDetails = await AppendSpecialAbilityToAddView(specialAbilityTable, importExcelView);
                            output.AddRange(specialAbilityDetails?.ImportDataStatuses);
                            foreach (var validRecord in specialAbilityDetails?.SpecialAbility)
                            {
                                ImportDataStatus importStatus = new ImportDataStatus();
                                EmployeeStatusDetail employeeStatus = specialAbilityDetails?.EmployeeDetailList.Find(x => x.EmployeeId == validRecord.EmployeeId);
                                importStatus.FormattedEmployeeId = employeeStatus.FormattedEmployeeId;
                                importStatus.EmployeeName = employeeStatus.EmployeeName;
                                importStatus.EmployeeEmailId = employeeStatus.EmployeeEmailId;
                                importStatus.Remarks = employeeStatus.Remarks;
                                Employees employees = await _employeeRepository.GetEmployeeForBUlkUploadByEmployeeId((int)validRecord.EmployeeId);
                                try
                                {
                                    EmployeeSpecialAbility employeeSpecialAbility = new EmployeeSpecialAbility();
                                    employeeSpecialAbility.SpecialAbilityId = (int)validRecord.SpecialAbilityId;
                                    employeeSpecialAbility.EmployeeId = (int)validRecord.EmployeeId;
                                    employeeSpecialAbility.CreatedBy = importExcelView.UploadedBy;
                                    employeeSpecialAbility.CreatedOn = DateTime.Now;
                                    oldData.EmployeeSpecialAbility = new List<EmployeeSpecialAbility>();
                                    oldData.Employee = employees;
                                    oldDataString = JsonConvert.SerializeObject(oldData);
                                    employees.IsSpecialAbility = true;
                                    employees.SpecialAbilityRemark = validRecord.SpecialAbilityRemark != null ? validRecord.SpecialAbilityRemark : employees.SpecialAbilityRemark;
                                    _employeeRepository.Update(employees);
                                    await _employeeRepository.SaveChangesAsync();
                                    await _employeeSpecialAbilityRepository.AddAsync(employeeSpecialAbility);
                                    await _employeeSpecialAbilityRepository.SaveChangesAsync();
                                    newData.EmployeeSpecialAbility = new List<EmployeeSpecialAbility> { employeeSpecialAbility };
                                    newData.ActionType = "Create";
                                    newData.CreatedBy = (int)employeeSpecialAbility.CreatedBy;
                                    newData.Employee = employees;
                                    AddAuditFile(newData, oldDataString);
                                    importStatus.Status = "Success";
                                    output.Add(importStatus);
                                }
                                catch (Exception ex)
                                {
                                    importStatus.Status = "Failure";
                                    importStatus.Remarks = ex.Message;
                                    output.Add(importStatus);
                                }
                            }
                            newData = new EmployeeAuditDataView();
                            oldData = new EmployeeAuditDataView();
                        }

                    }
                }
            }
            return output;
        }
        #endregion

        #region Bulk Insert Employee Basic Info
        private async Task<ImportEmployeeExcelView> AppendEmployeeToAddView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            List<Employees> employeesList = new List<Employees>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                List<string> remarkList = new List<string>();
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    Employees employees = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? throw new Exception("Employee ID is mandatory") : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                    if (employees != null)
                    {
                        throw new Exception("Employee already available");
                    }
                    else
                    {
                        employees = new Employees();
                    }
                    List<string> mandatoryErrorList = CheckEmployeeMandatory(employeeTable, i);
                    if (mandatoryErrorList.Count > 0)
                    {
                        throw new Exception(string.Join(',', mandatoryErrorList) + " are mandatory");
                    }
                    employees.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employees.FirstName = employeeTable.Rows[i][1]?.ToString()?.Trim();
                    employees.LastName = employeeTable.Rows[i][2]?.ToString()?.Trim();
                    employees.EmployeeName = employeeTable.Rows[i][3]?.ToString()?.Trim();
                    employees.EmailAddress = employeeTable.Rows[i][4]?.ToString()?.Trim();
                    employees.Gender = employeeTable.Rows[i][5]?.ToString()?.Trim()?.ToLower();
                    employees.IsSpecialAbility = false;
                    //data = importExcelView?.EmployeeMaster?.SpecialAbilityList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][7].ToString()?.Trim()?.ToLower())?.Key;
                    ////employees.SpecialAbilityId = employees.IsSpecialAbility == true ? data == null ? 0 : data : 0;
                    //employees.SpecialAbilityRemark = employeeTable.Rows[i][8]?.ToString()?.Trim().ToLower();
                    data = importExcelView?.EmployeeMaster?.EmployeeDepartmentList?.Find(x => x.DepartmentName?.ToLower() == employeeTable.Rows[i][6].ToString()?.Trim()?.ToLower())?.DepartmentId;
                    if (data == null) remarkList.Add("Department is Not Available in Department Master");
                    employees.DepartmentId = data == null ? 0 : data;
                    data = importExcelView?.EmployeeMaster?.EmployeeLocationList?.Find(x => x.Location?.ToLower() == employeeTable.Rows[i][7].ToString()?.Trim()?.ToLower())?.LocationId;
                    if (data == null) remarkList.Add("Base Work Location is Not Available in Location Master");
                    employees.LocationId = data == null ? 0 : data;
                    data = importExcelView?.EmployeeMaster?.EmployeeLocationList?.Find(x => x.Location?.ToLower() == employeeTable.Rows[i][8].ToString()?.Trim()?.ToLower())?.LocationId;
                    if (data == null) remarkList.Add("Current Work Location is Not Available in Location Master");
                    employees.CurrentWorkLocationId = data == null ? 0 : data;
                    data = importExcelView?.EmployeeMaster?.EmployeeWorkPlaceList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][9].ToString()?.Trim()?.ToLower() || x.DisplayName?.ToLower() == employeeTable.Rows[i][9].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) remarkList.Add("Current Work Place is Not Available in Current Work Place Master");
                    employees.CurrentWorkPlaceId = data == null ? 0 : data;
                    data = importExcelView?.EmployeeMaster?.ReportingManagerList?.Find(x => x.FormattedEmployeeId?.ToLower() == employeeTable.Rows[i][10].ToString()?.Trim()?.ToLower())?.EmployeeId;
                    if (data == null) remarkList.Add("Reporting manager Id is Not Available ");
                    employees.ReportingManagerId = data == null ? 0 : data;
                    data = importExcelView?.EmployeeMaster?.EmployeeTypeList?.Find(x => x.EmployeesType?.ToLower() == employeeTable.Rows[i][11]?.ToString()?.Trim()?.ToLower())?.EmployeesTypeId;
                    if (data == null) remarkList.Add("Employement Type is Not Available in Employement Type Master");
                    employees.EmployeeTypeId = data == null ? 0 : data;
                    data = importExcelView?.EmployeeMaster?.EmployeeProbationStatus.Find(x => x.ProbationStatusName.ToLower() == employeeTable.Rows[i][12]?.ToString()?.Trim().ToLower())?.ProbationStatusId;
                    if (data == null) remarkList.Add("Probation Status is Not Available in Probation Master");
                    employees.ProbationStatusId = data == null ? 0 : data;
                    employees.PreviousExperience = employeeTable.Rows[i][13]?.ToString()?.Trim() != null && employeeTable.Rows[i][13]?.ToString()?.Trim() != "" ? Convert.ToDecimal(employeeTable.Rows[i][13]?.ToString()?.Trim()) : 0;
                    data = importExcelView?.EmployeeMaster?.RolesList?.Find(x => x.RoleName?.ToLower() == employeeTable.Rows[i][14].ToString()?.Trim()?.ToLower())?.RoleId;
                    if (data == null) remarkList.Add("Role is Not Available in Role Master");
                    employees.RoleId = data == null ? 0 : data;
                    data = importExcelView?.EmployeeMaster?.DesignationList?.Find(x => x.DesignationName?.ToLower() == employeeTable.Rows[i][15].ToString()?.Trim()?.ToLower())?.DesignationId;
                    if (data == null) remarkList.Add("Designation is Not Available in Designation Master");
                    employees.DesignationId = data == null ? 0 : data;
                    employees.DesignationEffectiveFrom = Convert.ToDateTime(employeeTable.Rows[i][16]?.ToString()?.Trim());
                    data = importExcelView?.EmployeeMaster?.SystemRolesList?.Find(x => x.RoleName?.ToLower() == employeeTable.Rows[i][17].ToString()?.Trim()?.ToLower())?.RoleId;
                    if (data == null) remarkList.Add("System Role is Not Available in System Role Master");
                    employees.SystemRoleId = data == null ? 0 : data;
                    employees.DateOfJoining = Convert.ToDateTime(employeeTable.Rows[i][18]?.ToString()?.Trim());
                    data = importExcelView?.EmployeeMaster?.SourceOfHireList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][19].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) remarkList.Add("Source Of Hire is Not Available in Source Of Hire Master");
                    employees.SourceOfHireId = data == null ? 0 : data;
                    employees.MergerDate = employeeTable.Rows[i][20]?.ToString()?.Trim() != null && employeeTable.Rows[i][20]?.ToString()?.Trim() != "" ? Convert.ToDateTime(employeeTable.Rows[i][20]?.ToString()?.Trim()) : null;
                    data = importExcelView?.EmployeeMaster?.EmployeeCategoryList?.Find(x => x.EmployeeCategoryName?.ToLower() == employeeTable.Rows[i][21]?.ToString()?.Trim()?.ToLower())?.EmployeeCategoryId;
                    if (data == null) remarkList.Add("Employee Category is Not Available in Employee Category Master");
                    employees.EmployeeCategoryId = data == null ? 0 : data;
                    employees.EmployeeGrade = employeeTable.Rows[i][22]?.ToString()?.Trim() != null && employeeTable.Rows[i][22]?.ToString()?.Trim() != "" ? Convert.ToInt32(employeeTable.Rows[i][22]?.ToString()?.Trim()) : 0;
                    employees.ProbationExtension = !string.IsNullOrEmpty(employeeTable.Rows[i][23]?.ToString()?.Trim()) ? Convert.ToDateTime(employeeTable.Rows[i][23]?.ToString()?.Trim()) : null;
                    employees.OfficialMobileNumber = employeeTable.Rows[i][24]?.ToString()?.Trim();
                    data = importExcelView?.EmployeeMaster?.Entity?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][25].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) remarkList.Add("Entity is Not Available in Entity Master");
                    employees.Entity = data == null ? 0 : data;
                    employees.NoticeCategory = employeeTable.Rows[i][26]?.ToString()?.Trim()?.ToLower()?.Replace(" ", "_");
                    employees.IsActive = employeeTable.Rows[i][27]?.ToString()?.Trim()?.ToLower() == "true" ? true : false;
                    employees.BirthDate = Convert.ToDateTime(employeeTable.Rows[i][28]?.ToString()?.Trim());
                    employees.WeddingAnniversary = employeeTable.Rows[i][29]?.ToString()?.Trim() != null && employeeTable.Rows[i][29]?.ToString()?.Trim() != "" ? Convert.ToDateTime(employeeTable.Rows[i][29]?.ToString()?.Trim()) : null;
                    employees.Maritalstatus = importExcelView?.EmployeeMaster?.MaritalStatusList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][30].ToString()?.Trim()?.ToLower())?.Value;
                    employees.CreatedBy = importExcelView.UploadedBy;
                    employees.CreatedOn = DateTime.UtcNow;
                    employeesList.Add(employees);
                    employeeStatus.FormattedEmployeeId = employees.FormattedEmployeeId;
                    employeeStatus.Remarks = string.Join(',', remarkList);
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employeeTable.Rows[i][3]?.ToString()?.Trim();
                    importStatus.EmployeeEmailId = employeeTable.Rows[i][4]?.ToString()?.Trim();
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }
            }
            importExcelView.Employees = employeesList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add Personal Info Data
        private async Task<ImportEmployeeExcelView> AppendEmployeePersonalInfoToAddView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<EmployeesPersonalInfo> employeesList = new List<EmployeesPersonalInfo>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                Employees employee = new Employees();
                List<string> remarkList = new List<string>();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                try
                {
                    EmployeesPersonalInfo personalInfo = new EmployeesPersonalInfo();
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("EmployeeId is Mandatory ");
                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    personalInfo = _employeesPersonalInfoRepository.GetEmployeePersonalIdByEmployeeID(employee.EmployeeID);
                    if (personalInfo != null)
                    {
                        throw new Exception("Employee Personal Information  already available");
                    }
                    else
                    {
                        personalInfo = new EmployeesPersonalInfo();
                    }
                    personalInfo.EmployeeId = employee.EmployeeID;
                    personalInfo.HighestQualification = employeeTable.Rows[i][1]?.ToString()?.Trim();
                    personalInfo.PersonalMobileNumber = string.IsNullOrEmpty(employeeTable.Rows[i][2]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][2]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][3]?.ToString()?.Trim()) ? null : importExcelView?.EmployeeMaster?.BloodGroupList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][3].ToString()?.Trim()?.ToLower() || x.DisplayName?.ToLower() == employeeTable.Rows[i][3].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) remarkList.Add("Blood Group is Not Available in the Master Data");
                    personalInfo.BloodGroup = data == null ? 0 : data;
                    personalInfo.OtherEmail = string.IsNullOrEmpty(employeeTable.Rows[i][4]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][4]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][5]?.ToString()?.Trim()) ? null : importExcelView?.EmployeeMaster?.NationalityList?.Find(x => x.NationalityName?.ToLower() == employeeTable.Rows[i][5].ToString()?.Trim()?.ToLower())?.NationalityId;
                    if (data == null) remarkList.Add("Nationality is Not Available in the Master Data");
                    personalInfo.Nationality = data.ToString();
                    personalInfo.SpouseName = employeeTable.Rows[i][6]?.ToString()?.Trim();
                    personalInfo.FathersName = employeeTable.Rows[i][7]?.ToString()?.Trim();
                    personalInfo.PermanentAddressLine1 = string.IsNullOrEmpty(employeeTable.Rows[i][8]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][8]?.ToString()?.Trim();
                    personalInfo.PermanentAddressLine2 = string.IsNullOrEmpty(employeeTable.Rows[i][9]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][9]?.ToString()?.Trim();
                    personalInfo.PermanentCity = string.IsNullOrEmpty(employeeTable.Rows[i][10]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][10]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][11]?.ToString()?.Trim()) ? null : importExcelView?.EmployeeMaster?.StateList?.Find(x => x.StateName?.ToLower() == employeeTable.Rows[i][11].ToString()?.Trim()?.ToLower())?.StateId;
                    if (data == null) remarkList.Add("Permanent State is Not Available in the Master Data");
                    personalInfo.PermanentState = data == null ? 0 : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][12]?.ToString()?.Trim()) ? null : importExcelView?.EmployeeMaster?.CountryList?.Find(x => x.CountryName?.ToLower() == employeeTable.Rows[i][12].ToString()?.Trim()?.ToLower())?.CountryId;
                    if (data == null) remarkList.Add("Permanent Country is Not Available in the Master Data");
                    personalInfo.PermanentCountry = data == null ? 0 : data;
                    personalInfo.PermanentAddressZip = string.IsNullOrEmpty(employeeTable.Rows[i][13]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][13]?.ToString()?.Trim();
                    personalInfo.CommunicationAddressLine1 = string.IsNullOrEmpty(employeeTable.Rows[i][14]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][14]?.ToString()?.Trim();
                    personalInfo.CommunicationAddressLine2 = string.IsNullOrEmpty(employeeTable.Rows[i][15]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][15]?.ToString()?.Trim();
                    personalInfo.CommunicationCity = string.IsNullOrEmpty(employeeTable.Rows[i][16]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][16]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][17]?.ToString()?.Trim()) ? null : importExcelView?.EmployeeMaster?.StateList?.Find(x => x.StateName?.ToLower() == employeeTable.Rows[i][17].ToString()?.Trim()?.ToLower())?.StateId;
                    if (data == null) remarkList.Add("Communication State is Not Available in the Master Data");
                    personalInfo.CommunicationState = data == null ? 0 : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][18]?.ToString()?.Trim()) ? null : importExcelView?.EmployeeMaster?.CountryList?.Find(x => x.CountryName?.ToLower() == employeeTable.Rows[i][18].ToString()?.Trim()?.ToLower())?.CountryId;
                    if (data == null) remarkList.Add("Communication Country is Not Available in the Master Data");
                    personalInfo.CommunicationCountry = data == null ? 0 : data;
                    personalInfo.CommunicationAddressZip = string.IsNullOrEmpty(employeeTable.Rows[i][19]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][19]?.ToString()?.Trim();
                    personalInfo.EmergencyContactName = string.IsNullOrEmpty(employeeTable.Rows[i][20]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][20]?.ToString()?.Trim();
                    personalInfo.EmergencyContactRelation = string.IsNullOrEmpty(employeeTable.Rows[i][21]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][21]?.ToString()?.Trim();
                    personalInfo.EmergencyMobileNumber = string.IsNullOrEmpty(employeeTable.Rows[i][22]?.ToString()?.Trim()) ? null : employeeTable.Rows[i][22]?.ToString()?.Trim();
                    //personalInfo.ReferenceContactName =  employeeTable.Rows[i][23]?.ToString()?.Trim();
                    //personalInfo.ReferenceEmailId =  employeeTable.Rows[i][24]?.ToString()?.Trim();
                    //personalInfo.ReferenceMobileNumber = employeeTable.Rows[i][25]?.ToString()?.Trim();
                    personalInfo.PANNumber = string.IsNullOrEmpty(employeeTable.Rows[i][23].ToString()?.Trim()) ? null : employeeTable.Rows[i][23]?.ToString()?.Trim();
                    personalInfo.UANNumber = employeeTable.Rows[i][24]?.ToString()?.Trim();
                    personalInfo.AadhaarCardNumber = string.IsNullOrEmpty(employeeTable.Rows[i][25].ToString()?.Trim()) ? null : employeeTable.Rows[i][25]?.ToString()?.Trim();
                    personalInfo.DrivingLicense = employeeTable.Rows[i][26]?.ToString()?.Trim();
                    personalInfo.PassportNumber = employeeTable.Rows[i][27]?.ToString()?.Trim();
                    personalInfo.IsJoiningBonus = employeeTable.Rows[i][28]?.ToString()?.Trim().ToLower() == "yes" ? true : false;
                    personalInfo.JoiningBonusAmmount = personalInfo.IsJoiningBonus == false ? null : string.IsNullOrEmpty(employeeTable.Rows[i][29]?.ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][29]?.ToString()?.Trim());
                    personalInfo.JoiningBonusCondition = personalInfo.IsJoiningBonus == false ? "" : employeeTable.Rows[i][30]?.ToString()?.Trim().ToLower();
                    personalInfo.BankName = string.IsNullOrEmpty(employeeTable.Rows[i][31].ToString()?.Trim()) ? null : employeeTable.Rows[i][31]?.ToString()?.Trim();
                    personalInfo.AccountHolderName = string.IsNullOrEmpty(employeeTable.Rows[i][32].ToString()?.Trim()) ? null : employeeTable.Rows[i][32]?.ToString()?.Trim();
                    personalInfo.IFSCCode = string.IsNullOrEmpty(employeeTable.Rows[i][33].ToString()?.Trim()) ? null : employeeTable.Rows[i][33]?.ToString()?.Trim();
                    personalInfo.AccountNumber = string.IsNullOrEmpty(employeeTable.Rows[i][34].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][34]?.ToString()?.Trim());
                    personalInfo.CreatedBy = importExcelView.UploadedBy;
                    personalInfo.CreatedOn = DateTime.UtcNow;
                    employeesList.Add(personalInfo);
                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    employeeStatus.Remarks = string.Join(",", remarkList);
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }


            }
            importExcelView.EmployeePersonalInfo = employeesList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add Work History for Bulk Upload
        private async Task<ImportEmployeeExcelView> AppendWorkHistoryToAddView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<WorkHistory> workHistoryList = new List<WorkHistory>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                Employees employee = new Employees();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    WorkHistory workHistory = new WorkHistory();
                    workHistory.EmployeeId = employee.EmployeeID;
                    workHistory.OrganizationName = employeeTable.Rows[i][1]?.ToString()?.Trim();
                    workHistory.Designation = employeeTable.Rows[i][2]?.ToString()?.Trim();
                    data = employeeTable.Rows[i][3].ToString()?.Trim() != null ? importExcelView?.EmployeeMaster?.EmployeeTypeList?.Find(x => x.EmployeesType?.ToLower() == employeeTable.Rows[i][3].ToString()?.Trim()?.ToLower())?.EmployeesTypeId : 0;
                    if (data == null) employeeStatus.Remarks = "Employee type is Not Available in the Master Data";
                    workHistory.EmployeeTypeId = data == null ? 0 : data;
                    workHistory.StartDate = string.IsNullOrEmpty(employeeTable.Rows[i][4].ToString()?.Trim()) ? null : Convert.ToDateTime(employeeTable.Rows[i][4]?.ToString()?.Trim());
                    workHistory.EndDate = string.IsNullOrEmpty(employeeTable.Rows[i][5].ToString()?.Trim()) ? null : Convert.ToDateTime(employeeTable.Rows[i][5]?.ToString()?.Trim());
                    workHistory.LastCTC = string.IsNullOrEmpty(employeeTable.Rows[i][6].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][6]?.ToString()?.Trim());
                    workHistory.LeavingReason = employeeTable.Rows[i][7]?.ToString()?.Trim();
                    workHistory.CreatedBy = importExcelView.UploadedBy;
                    workHistory.CreatedOn = DateTime.UtcNow;
                    workHistoryList.Add(workHistory);
                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }

            }
            importExcelView.WorkHistories = workHistoryList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add Education Detail for Bulk Upload
        private async Task<ImportEmployeeExcelView> AppendEducationDetailToAddView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<EducationDetail> educationDetailsList = new List<EducationDetail>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();

            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                List<string> remarkList = new List<string>();
                Employees employee = new Employees();
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    EducationDetail educationDetail = new EducationDetail();
                    educationDetail.EmployeeId = employee.EmployeeID;
                    data = !string.IsNullOrEmpty(employeeTable.Rows[i][1].ToString()?.Trim()) ? importExcelView?.EmployeeMaster?.QualificationLIst?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][1].ToString()?.Trim()?.ToLower())?.Key : 0;
                    if (data == null) remarkList.Add("Education Type is Not Available in the Master Data");
                    educationDetail.EducationTypeId = data == null ? 0 : data;
                    educationDetail.InstitutionName = employeeTable.Rows[i][2]?.ToString()?.Trim();
                    data = !string.IsNullOrEmpty(employeeTable.Rows[i][3].ToString()?.Trim()) ? importExcelView?.EmployeeMaster?.Board?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][3].ToString()?.Trim()?.ToLower())?.Key : 0;
                    if (data == null) employeeStatus.Remarks = "Board is Not Available in the Master Data";
                    educationDetail.BoardId = data == null ? 0 : data;
                    educationDetail.UniversityName = employeeTable.Rows[i][4]?.ToString()?.Trim();
                    educationDetail.Degree = employeeTable.Rows[i][5]?.ToString()?.Trim();
                    educationDetail.Specialization = employeeTable.Rows[i][6]?.ToString()?.Trim();
                    educationDetail.CertificateName = employeeTable.Rows[i][7]?.ToString()?.Trim();
                    educationDetail.YearOfCompletion = string.IsNullOrEmpty(employeeTable.Rows[i][8].ToString()?.Trim()) ? null : Convert.ToDateTime(employeeTable.Rows[i][8]?.ToString()?.Trim());
                    educationDetail.MarkPercentage = string.IsNullOrEmpty(employeeTable.Rows[i][9].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][9]?.ToString()?.Trim());
                    educationDetail.ExpiryYear = string.IsNullOrEmpty(employeeTable.Rows[i][10].ToString()?.Trim()) ? null : Convert.ToInt32(employeeTable.Rows[i][10]?.ToString()?.Trim());
                    educationDetail.CreatedBy = importExcelView.UploadedBy;
                    educationDetail.CreatedOn = DateTime.UtcNow;
                    educationDetailsList.Add(educationDetail);
                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    employeeStatus.Remarks = string.Join(",", remarkList);
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }

            }
            importExcelView.EducationDetails = educationDetailsList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add CompensationDetail Detail for Bulk Upload
        private async Task<ImportEmployeeExcelView> AppendCompensationDetailToAddView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<CompensationDetail> compensationDetailsList = new List<CompensationDetail>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                Employees employee = new Employees();
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    CompensationDetail compensationDetail = new CompensationDetail();
                    compensationDetail.EmployeeID = employee.EmployeeID;
                    compensationDetail.EffectiveFromDate = string.IsNullOrEmpty(employeeTable.Rows[i][1].ToString()?.Trim()) ? throw new Exception("Effective From Date is mandatory") : Convert.ToDateTime(employeeTable.Rows[i][1]?.ToString()?.Trim());
                    compensationDetail.MonthlyCurrentCTC = string.IsNullOrEmpty(employeeTable.Rows[i][2].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][2]?.ToString()?.Trim());
                    compensationDetail.MonthlyBasicPay = string.IsNullOrEmpty(employeeTable.Rows[i][3].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][3]?.ToString()?.Trim());
                    compensationDetail.MonthlyHRA = string.IsNullOrEmpty(employeeTable.Rows[i][4].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][4]?.ToString()?.Trim());
                    compensationDetail.MonthlySatutoryBonus = string.IsNullOrEmpty(employeeTable.Rows[i][5].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][5]?.ToString()?.Trim());
                    compensationDetail.MonthlyFBP = string.IsNullOrEmpty(employeeTable.Rows[i][6].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][6]?.ToString()?.Trim());
                    compensationDetail.MonthlyGrossPay = string.IsNullOrEmpty(employeeTable.Rows[i][7].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][7]?.ToString()?.Trim());
                    compensationDetail.MonthlyPF = string.IsNullOrEmpty(employeeTable.Rows[i][8].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][8]?.ToString()?.Trim());
                    compensationDetail.MonthlyESI = string.IsNullOrEmpty(employeeTable.Rows[i][9].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][9]?.ToString()?.Trim());
                    compensationDetail.MonthlyCBP = string.IsNullOrEmpty(employeeTable.Rows[i][10].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][10]?.ToString()?.Trim());
                    compensationDetail.MonthlyCBPPercentage = string.IsNullOrEmpty(employeeTable.Rows[i][11].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][11]?.ToString()?.Trim());
                    compensationDetail.AnnualCurrentCTC = string.IsNullOrEmpty(employeeTable.Rows[i][12].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][12]?.ToString()?.Trim());
                    compensationDetail.AnnualBasicPay = string.IsNullOrEmpty(employeeTable.Rows[i][13].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][13]?.ToString()?.Trim());
                    compensationDetail.AnnualHRA = string.IsNullOrEmpty(employeeTable.Rows[i][14].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][14]?.ToString()?.Trim());
                    compensationDetail.AnnualSatutoryBonus = string.IsNullOrEmpty(employeeTable.Rows[i][15].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][15]?.ToString()?.Trim());
                    compensationDetail.AnnualFBP = string.IsNullOrEmpty(employeeTable.Rows[i][16].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][16]?.ToString()?.Trim());
                    compensationDetail.AnnualGrossPay = string.IsNullOrEmpty(employeeTable.Rows[i][17].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][17]?.ToString()?.Trim());
                    compensationDetail.AnnualPF = string.IsNullOrEmpty(employeeTable.Rows[i][18].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][18]?.ToString()?.Trim());
                    compensationDetail.AnnualESI = string.IsNullOrEmpty(employeeTable.Rows[i][19].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][19]?.ToString()?.Trim());
                    compensationDetail.AnnualCBP = string.IsNullOrEmpty(employeeTable.Rows[i][20].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][20]?.ToString()?.Trim());
                    compensationDetail.AnnualCBPPercentage = string.IsNullOrEmpty(employeeTable.Rows[i][21].ToString()?.Trim()) ? null : Convert.ToDecimal(employeeTable.Rows[i][21]?.ToString()?.Trim());
                    compensationDetail.CreatedBy = importExcelView.UploadedBy;
                    compensationDetail.CreatedOn = DateTime.UtcNow;
                    compensationDetailsList.Add(compensationDetail);
                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }
            }
            importExcelView.CompensationDetails = compensationDetailsList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add Skill set Detail for Bulk Upload
        private async Task<ImportEmployeeExcelView> AppendSkillSetToAddView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<EmployeesSkillset> skillsetList = new List<EmployeesSkillset>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                Employees employee = new Employees();
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }

                    EmployeesSkillset employeesSkillset = new EmployeesSkillset();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][1].ToString()?.Trim()) ? throw new Exception("Employee is Not Available ") : importExcelView.EmployeeMaster?.SkillsetList.Find(x => x.Skillset.ToLower() == employeeTable.Rows[i][1].ToString()?.Trim()?.ToLower())?.SkillsetId;
                    if (data == null) throw new Exception("Skill Set is Not Available in Master");
                    else
                    {
                        employeesSkillset.SkillsetId = data == null ? 0 : (int)data;
                        employeesSkillset = await _employeesSkillsetRepository.GetAllEmployeesSkillsetByIds(employeesSkillset.SkillsetId, employee.EmployeeID);
                        if (employeesSkillset == null)
                        {
                            employeesSkillset = new EmployeesSkillset();
                            employeesSkillset.SkillsetId = data == null ? 0 : (int)data;
                            employeesSkillset.EmployeeId = employee.EmployeeID;
                            employeesSkillset.CreatedOn = DateTime.UtcNow;
                            skillsetList.Add(employeesSkillset);
                        }

                    }

                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }
            }
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.SkillsetsEmployee = skillsetList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add Special Ability Detail for Bulk Upload
        private async Task<ImportEmployeeExcelView> AppendSpecialAbilityToAddView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<EmployeeSpecialAbilityView> specialAbilities = new List<EmployeeSpecialAbilityView>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                Employees employee = new Employees();
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    //if (employee?.IsSpecialAbility == false)
                    //{
                    //    throw new Exception("Employee No Disability");
                    //}

                    EmployeeSpecialAbilityView employeeSpecialAbility = new EmployeeSpecialAbilityView();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][1].ToString()?.Trim()) ? throw new Exception("Special Ability is Not Available ") : importExcelView.EmployeeMaster.SpecialAbilityList.Find(x => x.Value.ToLower() == employeeTable.Rows[i][1].ToString()?.Trim()?.ToLower() || x.DisplayName.ToLower() == employeeTable.Rows[i][1].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) throw new Exception("Special Ability is Not Available ");
                    else
                    {
                        employeeSpecialAbility.SpecialAbilityId = (int)data;
                        EmployeeSpecialAbility specialAbility = await _employeeSpecialAbilityRepository.GetEmployeeSpecialAbilityByIdAndEmployeeId(employeeSpecialAbility.SpecialAbilityId == null ? 0 : (int)employeeSpecialAbility.SpecialAbilityId, employee.EmployeeID);
                        if (specialAbility == null)
                        {
                            employeeSpecialAbility = new EmployeeSpecialAbilityView();
                            employeeSpecialAbility.SpecialAbilityId = (int)data;
                            employeeSpecialAbility.SpecialAbilityRemark = data == 48 ? employeeTable.Rows[i][2].ToString()?.Trim() : null;
                            employeeSpecialAbility.EmployeeId = employee.EmployeeID;
                            specialAbilities.Add(employeeSpecialAbility);
                        }

                    }

                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }
            }
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.SpecialAbility = specialAbilities;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Update employee detail with bulk upload
        private async Task<ImportEmployeeExcelView> AppendEmployeeToUpdateView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            int? data = 0;
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            List<Employees> employeesList = new List<Employees>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                ImportDataStatus importStatus = new ImportDataStatus();
                List<string> remarkList = new List<string>();

                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                Employees employees = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                try
                {
                    if (employees == null)
                    {
                        throw new Exception("Employee Not Available");
                    }

                    // employees.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employees.FirstName = string.IsNullOrEmpty(employeeTable.Rows[i][1]?.ToString()?.Trim()) ? employees.FirstName : employeeTable.Rows[i][1]?.ToString()?.Trim();
                    employees.LastName = string.IsNullOrEmpty(employeeTable.Rows[i][2]?.ToString()?.Trim()) ? employees.LastName : employeeTable.Rows[i][2]?.ToString()?.Trim();
                    employees.EmployeeName = string.IsNullOrEmpty(employeeTable.Rows[i][3]?.ToString()?.Trim()) ? employees.EmployeeName : employeeTable.Rows[i][3]?.ToString()?.Trim();
                    employees.EmailAddress = string.IsNullOrEmpty(employeeTable.Rows[i][4]?.ToString()?.Trim()) ? employees.EmailAddress : employeeTable.Rows[i][4]?.ToString()?.Trim();
                    employees.Gender = string.IsNullOrEmpty(employeeTable.Rows[i][5]?.ToString()?.Trim()) ? employees.Gender : employeeTable.Rows[i][5]?.ToString()?.Trim()?.ToLower();
                    //employees.IsSpecialAbility = employeeTable.Rows[i][6]?.ToString()?.Trim().ToLower() == "yes" ? true : employeeTable.Rows[i][6]?.ToString()?.Trim().ToLower() == "false" ? false : employees.IsSpecialAbility;
                    //data = string.IsNullOrEmpty(employeeTable.Rows[i][7].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.SpecialAbilityList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][7].ToString()?.Trim()?.ToLower())?.Key;
                    //employees.SpecialAbilityId = employees.IsSpecialAbility == true ? data != null ? (data == -1) ? employees.SpecialAbilityId : data : 0 : 0;
                    //employees.SpecialAbilityRemark = string.IsNullOrEmpty(employeeTable.Rows[i][8].ToString()?.Trim()) ? employees.SpecialAbilityRemark : employeeTable.Rows[i][8]?.ToString()?.Trim().ToLower();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][6].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.EmployeeDepartmentList?.Find(x => x.DepartmentName?.ToLower() == employeeTable.Rows[i][6].ToString()?.Trim()?.ToLower())?.DepartmentId;
                    if (data == null) remarkList.Add("Department is Not Available in Department Master");
                    employees.DepartmentId = data == null ? 0 : data == -1 ? employees.DepartmentId : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][7].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.EmployeeLocationList?.Find(x => x.Location?.ToLower() == employeeTable.Rows[i][7].ToString()?.Trim()?.ToLower())?.LocationId;
                    if (data == null) remarkList.Add("Base Work Location is Not Available in Location Master");
                    employees.LocationId = data == null ? 0 : data == -1 ? employees.LocationId : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][8].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.EmployeeLocationList?.Find(x => x.Location?.ToLower() == employeeTable.Rows[i][8].ToString()?.Trim()?.ToLower())?.LocationId;
                    if (data == null) remarkList.Add("Current Work Location is Not Available in Location Master");
                    employees.CurrentWorkLocationId = data == null ? 0 : data == -1 ? employees.CurrentWorkLocationId : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][9].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.EmployeeWorkPlaceList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][9].ToString()?.Trim()?.ToLower() || x.DisplayName?.ToLower() == employeeTable.Rows[i][9].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) remarkList.Add("Current Work Place is Not Available in Current Work Place Master");
                    employees.CurrentWorkPlaceId = data == null ? 0 : data == -1 ? employees.CurrentWorkPlaceId : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][10].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.ReportingManagerList?.Find(x => x.FormattedEmployeeId?.ToLower() == employeeTable.Rows[i][10].ToString()?.Trim()?.ToLower())?.EmployeeId;
                    if (data == null) remarkList.Add("Reporting manager Id is Not Available ");
                    employees.ReportingManagerId = data == null ? 0 : data == -1 ? employees.ReportingManagerId : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][11].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.EmployeeTypeList?.Find(x => x.EmployeesType?.ToLower() == employeeTable.Rows[i][11]?.ToString()?.Trim()?.ToLower())?.EmployeesTypeId;
                    if (data == null) remarkList.Add("Employement Type is Not Available in Employement Type Master");
                    employees.EmployeeTypeId = data == null ? 0 : data == -1 ? employees.EmployeeTypeId : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][12].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.EmployeeProbationStatus.Find(x => x.ProbationStatusName.ToLower() == employeeTable.Rows[i][12]?.ToString()?.Trim().ToLower())?.ProbationStatusId;
                    if (data == null) remarkList.Add("Probation Status is Not Available in Probation Master");
                    employees.ProbationStatusId = data == null ? 0 : data == -1 ? employees.ProbationStatusId : data;
                    employees.PreviousExperience = string.IsNullOrEmpty(employeeTable.Rows[i][13].ToString()?.Trim()) ? employees.PreviousExperience : employeeTable.Rows[i][13]?.ToString()?.Trim() != null && employeeTable.Rows[i][13]?.ToString()?.Trim() != "" ? Convert.ToDecimal(employeeTable.Rows[i][13]?.ToString()?.Trim()) : 0;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][14].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.RolesList?.Find(x => x.RoleName?.ToLower() == employeeTable.Rows[i][14].ToString()?.Trim()?.ToLower())?.RoleId;
                    if (data == null) remarkList.Add("Role is Not Available in Role Master");
                    employees.RoleId = data == null ? 0 : data == -1 ? employees.RoleId : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][15].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.DesignationList?.Find(x => x.DesignationName?.ToLower() == employeeTable.Rows[i][15].ToString()?.Trim()?.ToLower())?.DesignationId;
                    if (data == null) remarkList.Add("Designation is Not Available in Designation Master");
                    employees.DesignationId = data == null ? 0 : data == -1 ? employees.DesignationId : data;
                    employees.DesignationEffectiveFrom = string.IsNullOrEmpty(employeeTable.Rows[i][16].ToString()?.Trim()) ? employees.DesignationEffectiveFrom : Convert.ToDateTime(employeeTable.Rows[i][16]?.ToString()?.Trim());
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][17].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.SystemRolesList?.Find(x => x.RoleName?.ToLower() == employeeTable.Rows[i][17].ToString()?.Trim()?.ToLower())?.RoleId;
                    if (data == null) remarkList.Add("System Role is Not Available in System Role Master");
                    employees.SystemRoleId = data == null ? 0 : data == -1 ? employees.SystemRoleId : data;
                    employees.DateOfJoining = string.IsNullOrEmpty(employeeTable.Rows[i][18].ToString()?.Trim()) ? employees.DateOfJoining : Convert.ToDateTime(employeeTable.Rows[i][18]?.ToString()?.Trim());
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][19].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.SourceOfHireList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][19].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) remarkList.Add("Source Of Hire is Not Available in Source Of Hire Master");
                    employees.SourceOfHireId = data == null ? 0 : data == -1 ? employees.SourceOfHireId : data;
                    employees.MergerDate = employeeTable.Rows[i][20]?.ToString()?.Trim() != null && employeeTable.Rows[i][20]?.ToString()?.Trim() != "" ? Convert.ToDateTime(employeeTable.Rows[i][20]?.ToString()?.Trim()) : employees.MergerDate;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][21].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.EmployeeCategoryList?.Find(x => x.EmployeeCategoryName?.ToLower() == employeeTable.Rows[i][21]?.ToString()?.Trim()?.ToLower())?.EmployeeCategoryId;
                    if (data == null) remarkList.Add("Employee Category is Not Available in Employee Category Master");
                    employees.EmployeeCategoryId = data == null ? 0 : data == -1 ? employees.EmployeeCategoryId : data;
                    employees.EmployeeGrade = employeeTable.Rows[i][22]?.ToString()?.Trim() != null && employeeTable.Rows[i][22]?.ToString()?.Trim() != "" ? Convert.ToInt32(employeeTable.Rows[i][22]?.ToString()?.Trim()) : employees.EmployeeGrade;
                    employees.ProbationExtension = employeeTable.Rows[i][23]?.ToString()?.Trim() != null && employeeTable.Rows[i][23]?.ToString()?.Trim() != "" ? Convert.ToDateTime(employeeTable.Rows[i][23]?.ToString()?.Trim()) : employees.ProbationExtension;
                    employees.OfficialMobileNumber = string.IsNullOrEmpty(employeeTable.Rows[i][24].ToString()?.Trim()) ? employees.OfficialMobileNumber : employeeTable.Rows[i][24]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][25].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.Entity?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][25].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) remarkList.Add("Entity is Not Available in Entity Master");
                    employees.Entity = data == null ? 0 : data == -1 ? employees.Entity : data;
                    employees.NoticeCategory = string.IsNullOrEmpty(employeeTable.Rows[i][26].ToString()?.Trim()) ? employees.NoticeCategory : employeeTable.Rows[i][26]?.ToString()?.Trim()?.ToLower()?.Replace(" ", "_");
                    employees.IsActive = string.IsNullOrEmpty(employeeTable.Rows[i][27].ToString()?.Trim()) ? employees.IsActive : employeeTable.Rows[i][27]?.ToString()?.Trim()?.ToLower() == "true" ? true : false;
                    employees.BirthDate = string.IsNullOrEmpty(employeeTable.Rows[i][28].ToString()?.Trim()) ? employees.BirthDate : Convert.ToDateTime(employeeTable.Rows[i][28]?.ToString()?.Trim());
                    employees.WeddingAnniversary = string.IsNullOrEmpty(employeeTable.Rows[i][29].ToString()?.Trim()) ? employees.WeddingAnniversary : employeeTable.Rows[i][29]?.ToString()?.Trim() != null && employeeTable.Rows[i][29]?.ToString()?.Trim() != "" ? Convert.ToDateTime(employeeTable.Rows[i][29]?.ToString()?.Trim()) : null;
                    employees.Maritalstatus = string.IsNullOrEmpty(employeeTable.Rows[i][30].ToString()?.Trim()) ? employees.Maritalstatus : importExcelView?.EmployeeMaster?.MaritalStatusList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][30].ToString()?.Trim()?.ToLower())?.Value;
                    employees.ModifiedBy = importExcelView.UploadedBy;
                    employees.ModifiedOn = DateTime.UtcNow;
                    employeesList.Add(employees);
                    employeeStatus.FormattedEmployeeId = employees.FormattedEmployeeId;
                    employeeStatus.Remarks = string.Join(',', remarkList);
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    importStatus.FormattedEmployeeId = employees?.FormattedEmployeeId;
                    importStatus.EmployeeName = employees?.EmployeeName;
                    importStatus.EmployeeEmailId = employees?.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }
            }
            importExcelView.Employees = employeesList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add Personal Info Data bulk update
        private async Task<ImportEmployeeExcelView> AppendEmployeePersonalInfoToUpdateView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<EmployeesPersonalInfo> employeesList = new List<EmployeesPersonalInfo>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                List<string> remarkList = new List<string>();
                Employees employee = new Employees();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    EmployeesPersonalInfo personalInfo = _employeesPersonalInfoRepository.GetEmployeePersonalIdByEmployeeID(employee.EmployeeID);
                    if (personalInfo == null)
                    {
                        throw new Exception("Employee Personal Information Not available");
                    }
                    personalInfo.EmployeeId = employee.EmployeeID;
                    personalInfo.HighestQualification = string.IsNullOrEmpty(employeeTable.Rows[i][1]?.ToString()?.Trim()) ? personalInfo.HighestQualification : employeeTable.Rows[i][1]?.ToString()?.Trim();
                    personalInfo.PersonalMobileNumber = string.IsNullOrEmpty(employeeTable.Rows[i][2]?.ToString()?.Trim()) ? personalInfo.PersonalMobileNumber : employeeTable.Rows[i][2]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][3]?.ToString()?.Trim()) ? personalInfo.BloodGroup : importExcelView?.EmployeeMaster?.BloodGroupList?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][3].ToString()?.Trim()?.ToLower() || x.DisplayName?.ToLower() == employeeTable.Rows[i][3].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) remarkList.Add("Blood Group is Not Available in the Master Data");
                    personalInfo.BloodGroup = data == null ? 0 : data;
                    personalInfo.OtherEmail = string.IsNullOrEmpty(employeeTable.Rows[i][4]?.ToString()?.Trim()) ? personalInfo.OtherEmail : employeeTable.Rows[i][4]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][5]?.ToString()?.Trim()) ? Convert.ToInt32(personalInfo.Nationality) : importExcelView?.EmployeeMaster?.NationalityList?.Find(x => x.NationalityName?.ToLower() == employeeTable.Rows[i][5].ToString()?.Trim()?.ToLower())?.NationalityId;
                    if (data == null) remarkList.Add("Nationality is Not Available in the Master Data");
                    personalInfo.Nationality = data.ToString();
                    personalInfo.SpouseName = string.IsNullOrEmpty(employeeTable.Rows[i][6].ToString()?.Trim()) ? personalInfo.SpouseName : employeeTable.Rows[i][6]?.ToString()?.Trim();
                    personalInfo.FathersName = string.IsNullOrEmpty(employeeTable.Rows[i][7].ToString()?.Trim()) ? personalInfo.FathersName : employeeTable.Rows[i][7]?.ToString()?.Trim();
                    personalInfo.PermanentAddressLine1 = string.IsNullOrEmpty(employeeTable.Rows[i][8]?.ToString()?.Trim()) ? personalInfo.PermanentAddressLine1 : employeeTable.Rows[i][8]?.ToString()?.Trim();
                    personalInfo.PermanentAddressLine2 = string.IsNullOrEmpty(employeeTable.Rows[i][9]?.ToString()?.Trim()) ? personalInfo.PermanentAddressLine2 : employeeTable.Rows[i][9]?.ToString()?.Trim();
                    personalInfo.PermanentCity = string.IsNullOrEmpty(employeeTable.Rows[i][10]?.ToString()?.Trim()) ? personalInfo.PermanentCity : employeeTable.Rows[i][10]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][11]?.ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.StateList?.Find(x => x.StateName?.ToLower() == employeeTable.Rows[i][11].ToString()?.Trim()?.ToLower())?.StateId;
                    if (data == null) remarkList.Add("Permanent State is Not Available in the Master Data");
                    personalInfo.PermanentState = data == null ? 0 : data == -1 ? personalInfo.PermanentState : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][12]?.ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.CountryList?.Find(x => x.CountryName?.ToLower() == employeeTable.Rows[i][12].ToString()?.Trim()?.ToLower())?.CountryId;
                    if (data == null) remarkList.Add("Permanent Country is Not Available in the Master Data");
                    personalInfo.PermanentCountry = data == null ? 0 : data == -1 ? personalInfo.PermanentCountry : data;
                    personalInfo.PermanentAddressZip = string.IsNullOrEmpty(employeeTable.Rows[i][13]?.ToString()?.Trim()) ? personalInfo.PermanentAddressZip : employeeTable.Rows[i][13]?.ToString()?.Trim();
                    personalInfo.CommunicationAddressLine1 = string.IsNullOrEmpty(employeeTable.Rows[i][14]?.ToString()?.Trim()) ? personalInfo.CommunicationAddressLine1 : employeeTable.Rows[i][14]?.ToString()?.Trim();
                    personalInfo.CommunicationAddressLine2 = string.IsNullOrEmpty(employeeTable.Rows[i][15]?.ToString()?.Trim()) ? personalInfo.CommunicationAddressLine2 : employeeTable.Rows[i][15]?.ToString()?.Trim();
                    personalInfo.CommunicationCity = string.IsNullOrEmpty(employeeTable.Rows[i][16]?.ToString()?.Trim()) ? personalInfo.CommunicationCity : employeeTable.Rows[i][16]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][17]?.ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.StateList?.Find(x => x.StateName?.ToLower() == employeeTable.Rows[i][17].ToString()?.Trim()?.ToLower())?.StateId;
                    if (data == null) remarkList.Add("Communication State is Not Available in the Master Data");
                    personalInfo.CommunicationState = data == null ? 0 : data == -1 ? personalInfo.CommunicationState : data;
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][18]?.ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.CountryList?.Find(x => x.CountryName?.ToLower() == employeeTable.Rows[i][18].ToString()?.Trim()?.ToLower())?.CountryId;
                    if (data == null) remarkList.Add("Communication Country is Not Available in the Master Data");
                    personalInfo.CommunicationCountry = data == null ? 0 : data == -1 ? personalInfo.CommunicationCountry : data;
                    personalInfo.CommunicationAddressZip = string.IsNullOrEmpty(employeeTable.Rows[i][19]?.ToString()?.Trim()) ? personalInfo.CommunicationAddressZip : employeeTable.Rows[i][19]?.ToString()?.Trim();
                    personalInfo.EmergencyContactName = string.IsNullOrEmpty(employeeTable.Rows[i][20]?.ToString()?.Trim()) ? personalInfo.EmergencyContactName : employeeTable.Rows[i][20]?.ToString()?.Trim();
                    personalInfo.EmergencyContactRelation = string.IsNullOrEmpty(employeeTable.Rows[i][21]?.ToString()?.Trim()) ? personalInfo.EmergencyContactRelation : employeeTable.Rows[i][21]?.ToString()?.Trim();
                    personalInfo.EmergencyMobileNumber = string.IsNullOrEmpty(employeeTable.Rows[i][22]?.ToString()?.Trim()) ? personalInfo.EmergencyMobileNumber : employeeTable.Rows[i][22]?.ToString()?.Trim();
                    //personalInfo.ReferenceContactName = string.IsNullOrEmpty(employeeTable.Rows[i][23]?.ToString()?.Trim()) ? personalInfo.ReferenceContactName : employeeTable.Rows[i][23]?.ToString()?.Trim();
                    //personalInfo.ReferenceEmailId = string.IsNullOrEmpty(employeeTable.Rows[i][24]?.ToString()?.Trim()) ? personalInfo.ReferenceEmailId :  employeeTable.Rows[i][24]?.ToString()?.Trim();
                    //personalInfo.ReferenceMobileNumber = string.IsNullOrEmpty(employeeTable.Rows[i][25].ToString()?.Trim()) ? personalInfo.ReferenceMobileNumber :employeeTable.Rows[i][25]?.ToString()?.Trim();
                    personalInfo.PANNumber = string.IsNullOrEmpty(employeeTable.Rows[i][23].ToString()?.Trim()) ? personalInfo.PANNumber : employeeTable.Rows[i][23]?.ToString()?.Trim();
                    personalInfo.UANNumber = string.IsNullOrEmpty(employeeTable.Rows[i][24].ToString()?.Trim()) ? personalInfo.UANNumber : employeeTable.Rows[i][24]?.ToString()?.Trim();
                    personalInfo.AadhaarCardNumber = string.IsNullOrEmpty(employeeTable.Rows[i][25].ToString()?.Trim()) ? personalInfo.AadhaarCardNumber : employeeTable.Rows[i][25]?.ToString()?.Trim();
                    personalInfo.DrivingLicense = string.IsNullOrEmpty(employeeTable.Rows[i][26].ToString()?.Trim()) ? personalInfo.DrivingLicense : employeeTable.Rows[i][26]?.ToString()?.Trim();
                    personalInfo.PassportNumber = string.IsNullOrEmpty(employeeTable.Rows[i][27].ToString()?.Trim()) ? personalInfo.PassportNumber : employeeTable.Rows[i][27]?.ToString()?.Trim();
                    personalInfo.IsJoiningBonus = string.IsNullOrEmpty(employeeTable.Rows[i][28].ToString()?.Trim()) ? personalInfo.IsJoiningBonus : employeeTable.Rows[i][28]?.ToString()?.Trim().ToLower() == "yes" ? true : false;
                    personalInfo.JoiningBonusAmmount = personalInfo.IsJoiningBonus == true ? !string.IsNullOrEmpty(employeeTable.Rows[i][29]?.ToString()?.Trim()) ? Convert.ToDecimal(employeeTable.Rows[i][29]?.ToString()?.Trim()) : personalInfo.JoiningBonusAmmount : null;
                    personalInfo.JoiningBonusCondition = personalInfo.IsJoiningBonus == true ? string.IsNullOrEmpty(employeeTable.Rows[i][30].ToString()?.Trim()) ? personalInfo.JoiningBonusCondition : employeeTable.Rows[i][30]?.ToString()?.Trim().ToLower() : null;
                    personalInfo.BankName = string.IsNullOrEmpty(employeeTable.Rows[i][31].ToString()?.Trim()) ? personalInfo.BankName : employeeTable.Rows[i][31]?.ToString()?.Trim();
                    personalInfo.AccountHolderName = string.IsNullOrEmpty(employeeTable.Rows[i][32].ToString()?.Trim()) ? personalInfo.AccountHolderName : employeeTable.Rows[i][32]?.ToString()?.Trim();
                    personalInfo.IFSCCode = string.IsNullOrEmpty(employeeTable.Rows[i][33].ToString()?.Trim()) ? personalInfo.IFSCCode : employeeTable.Rows[i][33]?.ToString()?.Trim();
                    personalInfo.AccountNumber = string.IsNullOrEmpty(employeeTable.Rows[i][34].ToString()?.Trim()) ? personalInfo.AccountNumber : Convert.ToDecimal(employeeTable.Rows[i][34]?.ToString()?.Trim());
                    personalInfo.ModifiedBy = importExcelView.UploadedBy;
                    personalInfo.ModifiedOn = DateTime.UtcNow;
                    employeesList.Add(personalInfo);
                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    employeeStatus.Remarks = string.Join(",", remarkList);
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }


            }
            importExcelView.EmployeePersonalInfo = employeesList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add Work History for Bulk Update
        private async Task<ImportEmployeeExcelView> AppendWorkHistoryToUpdateView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            EmployeeAuditDataView oldata;
            List<WorkHistory> workHistoryList = new List<WorkHistory>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                oldata = new EmployeeAuditDataView();
                Employees employee = new Employees();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    string organizationName = string.IsNullOrEmpty(employeeTable.Rows[i][1]?.ToString()?.Trim()) ? throw new Exception("Organization Name is required") : employeeTable.Rows[i][1]?.ToString()?.Trim();
                    WorkHistory workHistory = _workHistoryRepository.GetWorkHistoryWorkHistoryByOrganizationName(employee.EmployeeID, organizationName);
                    if (workHistory == null)
                    {
                        throw new Exception("WorkHistory Is Not Available");
                    }
                    employeeStatus.id = workHistory.WorkHistoryId;
                    workHistory.EmployeeId = employee.EmployeeID;
                    workHistory.OrganizationName = organizationName;
                    workHistory.Designation = string.IsNullOrEmpty(employeeTable.Rows[i][2].ToString()?.Trim()) ? workHistory.Designation : employeeTable.Rows[i][2]?.ToString()?.Trim();
                    data = !string.IsNullOrEmpty(employeeTable.Rows[i][3].ToString()?.Trim()) ? importExcelView?.EmployeeMaster?.EmployeeTypeList?.Find(x => x.EmployeesType?.ToLower() == employeeTable.Rows[i][3].ToString()?.Trim()?.ToLower())?.EmployeesTypeId : -1;
                    if (data == null) employeeStatus.Remarks = "Employee type is Not Available in the Master Data";
                    workHistory.EmployeeTypeId = data == null ? 0 : data == -1 ? workHistory.EmployeeTypeId : data;
                    workHistory.StartDate = string.IsNullOrEmpty(employeeTable.Rows[i][4].ToString()?.Trim()) ? workHistory.StartDate : Convert.ToDateTime(employeeTable.Rows[i][4]?.ToString()?.Trim());
                    workHistory.EndDate = string.IsNullOrEmpty(employeeTable.Rows[i][5].ToString()?.Trim()) ? workHistory.EndDate : Convert.ToDateTime(employeeTable.Rows[i][5]?.ToString()?.Trim());
                    workHistory.LastCTC = string.IsNullOrEmpty(employeeTable.Rows[i][6].ToString()?.Trim()) ? workHistory.LastCTC : Convert.ToDecimal(employeeTable.Rows[i][6]?.ToString()?.Trim());
                    workHistory.LeavingReason = string.IsNullOrEmpty(employeeTable.Rows[i][7].ToString()?.Trim()) ? workHistory.LeavingReason : employeeTable.Rows[i][7]?.ToString()?.Trim();
                    workHistory.ModifiedBy = importExcelView.UploadedBy;
                    workHistory.ModifiedOn = DateTime.UtcNow;
                    workHistoryList.Add(workHistory);
                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }

            }
            importExcelView.WorkHistories = workHistoryList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add Education Detail for Bulk Upload
        private async Task<ImportEmployeeExcelView> AppendEducationDetailToUpdateView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<EducationDetail> educationDetailsList = new List<EducationDetail>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            EmployeeAuditDataView oldata;
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                oldata = new EmployeeAuditDataView();
                Employees employee = new Employees();
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    int? educationType = string.IsNullOrEmpty(employeeTable.Rows[i][1].ToString()?.Trim()) ? throw new Exception("Educationtype is required") : importExcelView?.EmployeeMaster?.QualificationLIst?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][1].ToString()?.Trim()?.ToLower())?.Key;
                    EducationDetail educationDetail = _educationDetailRepository.GetEducationDetailByEducationType(employee.EmployeeID, (int)educationType);
                    if (educationDetail == null)
                    {
                        throw new Exception("Education Detail Is Not Available");
                    }
                    employeeStatus.id = educationDetail.EducationDetailId;
                    educationDetail.EmployeeId = employee.EmployeeID;
                    educationDetail.EducationTypeId = educationType;
                    educationDetail.InstitutionName = string.IsNullOrEmpty(employeeTable.Rows[i][2].ToString()?.Trim()) ? educationDetail.InstitutionName : employeeTable.Rows[i][2]?.ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][3].ToString()?.Trim()) ? -1 : importExcelView?.EmployeeMaster?.Board?.Find(x => x.Value?.ToLower() == employeeTable.Rows[i][3].ToString()?.Trim()?.ToLower())?.Key;
                    if (data == null) employeeStatus.Remarks = "Board is Not Available in the Master Data";
                    educationDetail.BoardId = data == null ? 0 : data == -1 ? educationDetail.BoardId : data;
                    educationDetail.UniversityName = string.IsNullOrEmpty(employeeTable.Rows[i][4].ToString()?.Trim()) ? educationDetail.UniversityName : employeeTable.Rows[i][4]?.ToString()?.Trim();
                    educationDetail.Degree = string.IsNullOrEmpty(employeeTable.Rows[i][5].ToString()?.Trim()) ? educationDetail.Degree : employeeTable.Rows[i][5]?.ToString()?.Trim();
                    educationDetail.Specialization = string.IsNullOrEmpty(employeeTable.Rows[i][6].ToString()?.Trim()) ? educationDetail.Specialization : employeeTable.Rows[i][6]?.ToString()?.Trim();
                    educationDetail.CertificateName = string.IsNullOrEmpty(employeeTable.Rows[i][7].ToString()?.Trim()) ? educationDetail.CertificateName : employeeTable.Rows[i][7]?.ToString()?.Trim();
                    educationDetail.YearOfCompletion = string.IsNullOrEmpty(employeeTable.Rows[i][8].ToString()?.Trim()) ? educationDetail.YearOfCompletion : Convert.ToDateTime(employeeTable.Rows[i][8]?.ToString()?.Trim());
                    educationDetail.MarkPercentage = string.IsNullOrEmpty(employeeTable.Rows[i][9].ToString()?.Trim()) ? educationDetail.MarkPercentage : Convert.ToDecimal(employeeTable.Rows[i][9]?.ToString()?.Trim());
                    educationDetail.ExpiryYear = string.IsNullOrEmpty(employeeTable.Rows[i][10].ToString()?.Trim()) ? educationDetail.ExpiryYear : Convert.ToInt32(employeeTable.Rows[i][10]?.ToString()?.Trim());
                    educationDetail.ModifiedBy = importExcelView.UploadedBy;
                    educationDetail.ModifiedOn = DateTime.UtcNow;
                    educationDetailsList.Add(educationDetail);
                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }

            }
            importExcelView.EducationDetails = educationDetailsList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add CompensationDetail Detail for Bulk Update
        private async Task<ImportEmployeeExcelView> AppendCompensationDetailToUpdateView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            EmployeeAuditDataView oldata;
            List<CompensationDetail> compensationDetailsList = new List<CompensationDetail>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                Employees employee = new Employees();
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                oldata = new EmployeeAuditDataView();
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    DateTime EffectiveFromDate = string.IsNullOrEmpty(employeeTable.Rows[i][1].ToString()?.Trim()) ? throw new Exception("Effective From Date is mandatory") : Convert.ToDateTime(employeeTable.Rows[i][1]?.ToString()?.Trim());
                    CompensationDetail compensationDetail = _compensationDetailRepository.GetCompensationDetailByEffectiveFromDate(employee.EmployeeID, EffectiveFromDate);
                    if (compensationDetail == null)
                    {
                        throw new Exception("CompensationDetail Is Not Available");
                    }
                    employeeStatus.id = compensationDetail.CTCId;
                    employeeStatus.oldData = JsonConvert.SerializeObject(oldata);
                    compensationDetail.EmployeeID = employee.EmployeeID;
                    compensationDetail.EffectiveFromDate = EffectiveFromDate;
                    compensationDetail.MonthlyCurrentCTC = string.IsNullOrEmpty(employeeTable.Rows[i][2].ToString()?.Trim()) ? compensationDetail.MonthlyCurrentCTC : Convert.ToDecimal(employeeTable.Rows[i][2]?.ToString()?.Trim());
                    compensationDetail.MonthlyBasicPay = string.IsNullOrEmpty(employeeTable.Rows[i][3].ToString()?.Trim()) ? compensationDetail.MonthlyBasicPay : Convert.ToDecimal(employeeTable.Rows[i][3]?.ToString()?.Trim());
                    compensationDetail.MonthlyHRA = string.IsNullOrEmpty(employeeTable.Rows[i][4].ToString()?.Trim()) ? compensationDetail.MonthlyHRA : Convert.ToDecimal(employeeTable.Rows[i][4]?.ToString()?.Trim());
                    compensationDetail.MonthlySatutoryBonus = string.IsNullOrEmpty(employeeTable.Rows[i][5].ToString()?.Trim()) ? compensationDetail.MonthlySatutoryBonus : Convert.ToDecimal(employeeTable.Rows[i][5]?.ToString()?.Trim());
                    compensationDetail.MonthlyFBP = string.IsNullOrEmpty(employeeTable.Rows[i][6].ToString()?.Trim()) ? compensationDetail.MonthlyFBP : Convert.ToDecimal(employeeTable.Rows[i][6]?.ToString()?.Trim());
                    compensationDetail.MonthlyGrossPay = string.IsNullOrEmpty(employeeTable.Rows[i][7].ToString()?.Trim()) ? compensationDetail.MonthlyGrossPay : Convert.ToDecimal(employeeTable.Rows[i][7]?.ToString()?.Trim());
                    compensationDetail.MonthlyPF = string.IsNullOrEmpty(employeeTable.Rows[i][8].ToString()?.Trim()) ? compensationDetail.MonthlyPF : Convert.ToDecimal(employeeTable.Rows[i][8]?.ToString()?.Trim());
                    compensationDetail.MonthlyESI = string.IsNullOrEmpty(employeeTable.Rows[i][9].ToString()?.Trim()) ? compensationDetail.MonthlyESI : Convert.ToDecimal(employeeTable.Rows[i][9]?.ToString()?.Trim());
                    compensationDetail.MonthlyCBP = string.IsNullOrEmpty(employeeTable.Rows[i][10].ToString()?.Trim()) ? compensationDetail.MonthlyCBP : Convert.ToDecimal(employeeTable.Rows[i][10]?.ToString()?.Trim());
                    compensationDetail.MonthlyCBPPercentage = string.IsNullOrEmpty(employeeTable.Rows[i][11].ToString()?.Trim()) ? compensationDetail.MonthlyCBPPercentage : Convert.ToDecimal(employeeTable.Rows[i][11]?.ToString()?.Trim());
                    compensationDetail.AnnualCurrentCTC = string.IsNullOrEmpty(employeeTable.Rows[i][12].ToString()?.Trim()) ? compensationDetail.AnnualCurrentCTC : Convert.ToDecimal(employeeTable.Rows[i][12]?.ToString()?.Trim());
                    compensationDetail.AnnualBasicPay = string.IsNullOrEmpty(employeeTable.Rows[i][13].ToString()?.Trim()) ? compensationDetail.AnnualBasicPay : Convert.ToDecimal(employeeTable.Rows[i][13]?.ToString()?.Trim());
                    compensationDetail.AnnualHRA = string.IsNullOrEmpty(employeeTable.Rows[i][14].ToString()?.Trim()) ? compensationDetail.AnnualHRA : Convert.ToDecimal(employeeTable.Rows[i][14]?.ToString()?.Trim());
                    compensationDetail.AnnualSatutoryBonus = string.IsNullOrEmpty(employeeTable.Rows[i][15].ToString()?.Trim()) ? compensationDetail.AnnualSatutoryBonus : Convert.ToDecimal(employeeTable.Rows[i][15]?.ToString()?.Trim());
                    compensationDetail.AnnualFBP = string.IsNullOrEmpty(employeeTable.Rows[i][16].ToString()?.Trim()) ? compensationDetail.AnnualFBP : Convert.ToDecimal(employeeTable.Rows[i][16]?.ToString()?.Trim());
                    compensationDetail.AnnualGrossPay = string.IsNullOrEmpty(employeeTable.Rows[i][17].ToString()?.Trim()) ? compensationDetail.AnnualGrossPay : Convert.ToDecimal(employeeTable.Rows[i][17]?.ToString()?.Trim());
                    compensationDetail.AnnualPF = string.IsNullOrEmpty(employeeTable.Rows[i][18].ToString()?.Trim()) ? compensationDetail.AnnualPF : Convert.ToDecimal(employeeTable.Rows[i][18]?.ToString()?.Trim());
                    compensationDetail.AnnualESI = string.IsNullOrEmpty(employeeTable.Rows[i][19].ToString()?.Trim()) ? compensationDetail.AnnualESI : Convert.ToDecimal(employeeTable.Rows[i][19]?.ToString()?.Trim());
                    compensationDetail.AnnualCBP = string.IsNullOrEmpty(employeeTable.Rows[i][20].ToString()?.Trim()) ? compensationDetail.AnnualCBP : Convert.ToDecimal(employeeTable.Rows[i][20]?.ToString()?.Trim());
                    compensationDetail.AnnualCBPPercentage = string.IsNullOrEmpty(employeeTable.Rows[i][21].ToString()?.Trim()) ? compensationDetail.AnnualCBPPercentage : Convert.ToDecimal(employeeTable.Rows[i][21]?.ToString()?.Trim());
                    compensationDetail.ModifiedBy = importExcelView.UploadedBy;
                    compensationDetail.ModifiedOn = DateTime.UtcNow;
                    compensationDetailsList.Add(compensationDetail);
                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }
            }
            importExcelView.CompensationDetails = compensationDetailsList;
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region Add Dependency Detail for Bulk Update
        private async Task<ImportEmployeeExcelView> AppendDependencyToUpdateView(DataTable employeeTable, ImportEmployeeExcelView importExcelView)
        {
            List<ImportDataStatus> output = new List<ImportDataStatus>();
            int? data = 0;
            List<EmployeeDependent> DependencyList = new List<EmployeeDependent>();
            List<EmployeeStatusDetail> statusDetailsList = new List<EmployeeStatusDetail>();
            for (int i = 1; i < employeeTable?.Rows?.Count; i++)
            {
                Employees employee = new Employees();
                EmployeeStatusDetail employeeStatus = new EmployeeStatusDetail();
                employee = string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()) ? null : await _employeeRepository.GetEmployeeByFormattedEmployeeIdForBulkUpload(employeeTable.Rows[i][0]?.ToString()?.Trim());
                try
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    if (string.IsNullOrEmpty(employeeTable.Rows[i][0]?.ToString()?.Trim()))
                    {
                        throw new Exception("Employee Id  is Mandatory ");

                    }
                    if (employee == null)
                    {
                        throw new Exception("Employee is Not Available ");
                    }
                    EmployeeDependent employeeDependency = new EmployeeDependent();
                    employeeDependency.EmployeeID = employee.EmployeeID;
                    employeeDependency.EmployeeRelationName = employeeTable.Rows[i][1].ToString()?.Trim();
                    data = string.IsNullOrEmpty(employeeTable.Rows[i][2].ToString()?.Trim()) ? throw new Exception("Employee is Not Available ") : importExcelView.EmployeeMaster.EmployeeRelationshipList.Find(x => x.EmployeeRelationshipName.ToLower() == employeeTable.Rows[i][2].ToString()?.Trim()?.ToLower())?.EmployeeRelationshipId;
                    if (data == null) employeeStatus.Remarks = "Employee Relationship is Not Available in Master";
                    employeeDependency.EmployeeRelationshipId = data == null ? 0 : (int)data;
                    employeeDependency.EmployeeRelationDateOfBirth = string.IsNullOrEmpty(employeeTable.Rows[i][3].ToString()?.Trim()) ? null : Convert.ToDateTime(employeeTable.Rows[i][3]?.ToString()?.Trim());
                    employeeDependency.CreatedBy = 1;
                    employeeDependency.CreatedOn = DateTime.UtcNow;
                    DependencyList.Add(employeeDependency);
                    employeeStatus.EmployeeId = employee == null ? 0 : employee.EmployeeID;
                    employeeStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    employeeStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    employeeStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    statusDetailsList.Add(employeeStatus);
                }
                catch (Exception e)
                {
                    ImportDataStatus importStatus = new ImportDataStatus();
                    importStatus.FormattedEmployeeId = employeeTable.Rows[i][0]?.ToString()?.Trim();
                    importStatus.EmployeeName = employee == null ? "" : employee.EmployeeName;
                    importStatus.EmployeeEmailId = employee == null ? "" : employee.EmailAddress;
                    importStatus.Status = "Failure";
                    importStatus.Remarks = e.Message;
                    output.Add(importStatus);
                }
            }
            importExcelView.EmployeeDetailList = statusDetailsList;
            importExcelView.EmployeeDependency = DependencyList;
            importExcelView.ImportDataStatuses = output;
            return importExcelView;
        }
        #endregion

        #region check Employee Mandatory filed
        private List<string> CheckEmployeeMandatory(DataTable employeeTable, int index)
        {
            List<string> mandatoryFieldList = new List<string>();
            if (string.IsNullOrEmpty(employeeTable.Rows[index][1]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee First Name");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][2]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Last Name");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][3]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Full Name");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][4]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Email Id");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][5]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Gender");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][6]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Department");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][7]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Base Work Location");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][8]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Current Work Location");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][10]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Reporting Manager");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][11]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Type");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][15]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Designation");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][16]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Designation Effective from date");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][17]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("System Role");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][18]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Date of Joining");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][21]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Category");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][26]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Notice Category");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][27]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Status");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[index][28]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Birthday");
            }

            return mandatoryFieldList;
        }
        #endregion

        #region Check PersonalInfo Mandatory field
        private List<string> CheckPersonalInfoMandatory(DataTable employeeTable, int i)
        {
            List<string> mandatoryFieldList = new List<string>();
            if (string.IsNullOrEmpty(employeeTable.Rows[i][2]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Personal Mobile Number");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][3]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Blood Group");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][4]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Personal Email Id");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][5]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Nationality");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][8]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Permanent Address Line 1");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][9]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Permanent Address Line 2 ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][10]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Permanent city ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][11]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Permanent state ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][12]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Permanent country ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][13]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Permanent Address Zip ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][14]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Communication Address Line 1 ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][15]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Communication Address Line 2 ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][16]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Communication City ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][17]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Communication State ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][18]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Communication Country ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][19]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Communication Address Zip ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][20]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Emergency Contact Name ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][21]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Emergency Contact Person Relation ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][22]?.ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Emergency Contact Person Number ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][26].ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee PAN Number ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][28].ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Aadhar Number ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][34].ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Bank Name  ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][35].ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Account Holder Name ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][36].ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee IFSC Code ");
            }
            if (string.IsNullOrEmpty(employeeTable.Rows[i][37].ToString()?.Trim()))
            {
                mandatoryFieldList.Add("Employee Account Number ");
            }
            return mandatoryFieldList;
        }
        #endregion

        #region Add or Update employee Work History
        public async Task<bool> AddOrUpdateEmployeeWorkHistory(EmployeeWorkAndEducationDetailView workHistoryDetail)
        {
            try
            {

                string oldDataString = "";
                EmployeeAuditDataView newData = new EmployeeAuditDataView();
                EmployeeAuditDataView oldData = new EmployeeAuditDataView();
                newData.WorkHistory = new List<WorkHistory>();
                int employeeId = workHistoryDetail?.WorkHistoriesList[0]?.EmployeeId == null ? 0 : (int)workHistoryDetail?.WorkHistoriesList[0]?.EmployeeId;
                //List<WorkHistory> previousWorkHistories = _workHistoryRepository.GetWorkHistoryByEmployeeId(employeeId);
                //if (previousWorkHistories?.Count > 0)
                //{
                //    foreach (WorkHistory item in previousWorkHistories)
                //    {
                //        _workHistoryRepository.Delete(item);
                //        await _workHistoryRepository.SaveChangesAsync();
                //    }
                //}
                List<WorkHistory> workHistoryList = _workHistoryRepository.GetWorkHistoryByEmployeeId(employeeId);
                oldData.WorkHistory = workHistoryList;
                oldDataString = JsonConvert.SerializeObject(oldData);
                foreach (WorkHistoryView workHistory in workHistoryDetail.WorkHistoriesList)
                {
                    WorkHistory employeeWorkHistory = new WorkHistory();
                    if (workHistory.WorkHistoryId != 0)
                    {
                        employeeWorkHistory = _workHistoryRepository.GetWorkHistoryWorkHistoryId(workHistory.WorkHistoryId);

                    }
                    employeeWorkHistory.EmployeeId = workHistory?.EmployeeId != null ? workHistory?.EmployeeId : workHistoryDetail.EmployeeId;
                    employeeWorkHistory.Designation = workHistory?.Designation;
                    employeeWorkHistory.OrganizationName = workHistory?.OrganizationName;
                    employeeWorkHistory.EmployeeTypeId = workHistory?.EmployeeTypeId;
                    employeeWorkHistory.LastCTC = workHistory?.LastCTC;
                    employeeWorkHistory.StartDate = workHistory?.StartDate;
                    employeeWorkHistory.EndDate = workHistory?.ReleivingDate;
                    employeeWorkHistory.LeavingReason = workHistory?.LeavingReason;
                    if (workHistory.WorkHistoryId == 0)
                    {
                        employeeWorkHistory.CreatedBy = workHistory?.CreatedBy;
                        employeeWorkHistory.CreatedOn = workHistory?.CreatedOn;
                        newData.ActionType = "Create";
                        newData.CreatedBy = workHistory?.CreatedBy == null ? 0 : (int)workHistory?.CreatedBy;
                        await _workHistoryRepository.AddAsync(employeeWorkHistory);
                    }
                    else
                    {
                        employeeWorkHistory.ModifiedOn = workHistory?.ModifiedOn;
                        employeeWorkHistory.ModifiedBy = workHistory?.ModifiedBy;
                        newData.ActionType = "Update";
                        newData.CreatedBy = workHistory?.ModifiedBy == null ? 0 : (int)workHistory?.ModifiedBy;
                        _workHistoryRepository.Update(employeeWorkHistory);
                    }
                    await _workHistoryRepository.SaveChangesAsync();
                    newData.WorkHistory.Add(employeeWorkHistory);
                    List<DocumentsToUpload> listOfDocuments = new List<DocumentsToUpload>();
                    listOfDocuments.Add(workHistory.serviceLetter);
                    listOfDocuments.Add(workHistory.OfferLetter);
                    listOfDocuments.Add(workHistory.paySlip);
                    if (workHistoryDetail?.SupportingDocumentsViews != null)
                    {
                        //workHistoryDetail.SupportingDocumentsViews.SourceType = "Employee";
                        workHistoryDetail.SupportingDocumentsViews.DocumentType = "WorkHistory";
                        workHistoryDetail.SupportingDocumentsViews.SourceId = employeeWorkHistory.WorkHistoryId;
                        workHistoryDetail.SupportingDocumentsViews.EmployeeDocumentList = listOfDocuments;
                        workHistoryDetail.SupportingDocumentsViews.EmployeeId = employeeWorkHistory.EmployeeId == null ? 0 : (int)employeeWorkHistory.EmployeeId;
                        string directoryPath = GetDirectoryPath(workHistoryDetail?.SupportingDocumentsViews);
                        workHistoryDetail.SupportingDocumentsViews.SourceType = "WorkHistory";
                        workHistoryDetail.SupportingDocumentsViews.BaseDirectory = directoryPath;
                        workHistoryDetail.SupportingDocumentsViews.CreatedBy = (int)employeeWorkHistory?.CreatedBy;
                        await AddSupportingDocument(workHistoryDetail?.SupportingDocumentsViews);
                    }
                }
                AddAuditFile(newData, oldDataString);
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region Add or Update employee Education Detail 
        public async Task<bool> AddOrUpdateEducationDetails(EmployeeWorkAndEducationDetailView educationDetail)
        {
            try
            {
                EmployeeAuditDataView newData = new EmployeeAuditDataView();
                EmployeeAuditDataView oldData = new EmployeeAuditDataView();
                string oldDataString = "";
                newData.EducationDetail = new List<EducationDetail>();
                int employeeId = educationDetail?.EducationDetailsList[0]?.EmployeeId == null ? 0 : (int)educationDetail?.EducationDetailsList[0]?.EmployeeId;
                oldData.EducationDetail = _educationDetailRepository.GetEducationDetailByEmployeeId(employeeId);
                oldDataString = JsonConvert.SerializeObject(oldData);
                foreach (EducationDetailView item in educationDetail?.EducationDetailsList)
                {
                    EducationDetail education = new EducationDetail();
                    if (item?.EducationDetailId != 0)
                    {
                        education = _educationDetailRepository.GetEducationDetailEducationDetailId(item.EducationDetailId);
                    }
                    education.EmployeeId = item?.EmployeeId;
                    education.InstitutionName = item?.InstitutionName;
                    education.UniversityName = item?.UniversityName;
                    education.EducationTypeId = item?.EducationTypeId;
                    education.MarkPercentage = item?.MarkPercentage;
                    education.YearOfCompletion = item?.YearOfCompletion;
                    education.BoardId = item?.BoardId;
                    education.Specialization = item?.Specialization;
                    education.Degree = item?.Degree;
                    education.ExpiryYear = item?.ExpiryYear;
                    education.CertificateName = item?.CertificateName;
                    if (item.EducationDetailId == 0)
                    {
                        education.CreatedBy = item?.CreatedBy;
                        education.CreatedOn = item?.CreatedOn;
                        newData.ActionType = "Create";
                        newData.CreatedBy = item?.CreatedBy == null ? 0 : (int)item?.CreatedBy;
                        await _educationDetailRepository.AddAsync(education);
                    }
                    else
                    {
                        education.ModifiedBy = item?.ModifiedBy;
                        education.ModifiedOn = item?.ModifiedOn;
                        newData.ActionType = "Update";
                        newData.CreatedBy = item?.ModifiedBy == null ? 0 : (int)item?.ModifiedBy;
                        _educationDetailRepository.Update(education);
                    }
                    await _educationDetailRepository.SaveChangesAsync();
                    newData.EducationDetail.Add(education);
                    List<DocumentsToUpload> listOfDocuments = new List<DocumentsToUpload>();
                    if (item.Marksheet != null) { listOfDocuments.Add(item.Marksheet); }
                    if (item.Certificate != null) { listOfDocuments.Add(item.Certificate); }
                    if (educationDetail?.SupportingDocumentsViews != null && education?.EducationDetailId != 0)
                    {
                        educationDetail.SupportingDocumentsViews.DocumentType = "EducationDetail";
                        educationDetail.SupportingDocumentsViews.SourceId = education.EducationDetailId;
                        educationDetail.SupportingDocumentsViews.EmployeeId = education.EmployeeId == null ? 0 : (int)education.EmployeeId;
                        string directoryPath = GetDirectoryPath(educationDetail?.SupportingDocumentsViews);
                        educationDetail.SupportingDocumentsViews.EmployeeDocumentList = listOfDocuments;
                        educationDetail.SupportingDocumentsViews.SourceType = "EducationDetail";
                        educationDetail.SupportingDocumentsViews.BaseDirectory = directoryPath;
                        educationDetail.SupportingDocumentsViews.CreatedBy = education?.CreatedBy == null ? 0 : (int)education?.CreatedBy;
                        await AddSupportingDocument(educationDetail?.SupportingDocumentsViews);
                    }
                }
                AddAuditFile(newData, oldDataString);
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region Add or Update employee Compensation Detail 
        public async Task<bool> AddOrUpdateEmployeeCompensationDetails(List<CompensationDetailView> CompensationDetailViewList)
        {
            try
            {

                string oldDataString = "";
                EmployeeAuditDataView newData = new EmployeeAuditDataView();
                EmployeeAuditDataView oldData = new EmployeeAuditDataView();
                newData.CompensationDetail = new List<CompensationDetail>();
                int employeeId = CompensationDetailViewList[0]?.EmployeeID == null ? 0 : (int)CompensationDetailViewList[0]?.EmployeeID;
                oldData.CompensationDetail = _compensationDetailRepository.GetCompensationDetailByEmployeeId(employeeId);
                foreach (CompensationDetailView compensationDetail in CompensationDetailViewList)
                {
                    CompensationDetail compensation = _compensationDetailRepository.GetCompendationDetailByCTCId(compensationDetail.CTCId);
                    if (compensation == null)
                    {
                        compensation = new CompensationDetail();
                    }
                    oldDataString = JsonConvert.SerializeObject(oldData);
                    compensation.EffectiveFromDate = compensationDetail?.EffectiveFromDate;
                    compensation.EmployeeID = compensationDetail?.EmployeeID;
                    compensation.MonthlyCurrentCTC = compensationDetail?.MonthlyCurrentCTC;
                    compensation.MonthlyBasicPay = compensationDetail?.MonthlyBasicPay;
                    compensation.MonthlyHRA = compensationDetail?.MonthlyHRA;
                    compensation.MonthlySatutoryBonus = compensationDetail?.MonthlySatutoryBonus;
                    compensation.MonthlyFBP = compensationDetail?.MonthlyFBP;
                    compensation.MonthlyGrossPay = compensationDetail?.MonthlyGrossPay;
                    compensation.MonthlyPF = compensationDetail?.MonthlyPF;
                    compensation.MonthlyESI = compensationDetail?.MonthlyESI;
                    compensation.MonthlyCBP = compensationDetail?.MonthlyCBP;
                    compensation.MonthlyCBPPercentage = compensationDetail?.MonthlyCBPPercentage;
                    compensation.AnnualCurrentCTC = compensationDetail?.AnnualCurrentCTC;
                    compensation.AnnualBasicPay = compensationDetail?.AnnualBasicPay;
                    compensation.AnnualHRA = compensationDetail?.AnnualHRA;
                    compensation.AnnualSatutoryBonus = compensationDetail?.AnnualSatutoryBonus;
                    compensation.AnnualFBP = compensationDetail?.AnnualFBP;
                    compensation.AnnualGrossPay = compensationDetail?.AnnualGrossPay;
                    compensation.AnnualPF = compensationDetail?.AnnualPF;
                    compensation.AnnualESI = compensationDetail?.AnnualESI;
                    compensation.AnnualCBP = compensationDetail?.AnnualCBP;
                    compensation.AnnualCBPPercentage = compensationDetail?.AnnualCBPPercentage;
                    if (compensation?.CTCId == 0)
                    {
                        compensation.CreatedOn = compensationDetail?.CreatedOn;
                        compensation.CreatedBy = compensationDetail?.CreatedBy;
                        newData.ActionType = "Create";
                        newData.CreatedBy = compensationDetail?.CreatedBy == null ? 0 : (int)compensationDetail?.CreatedBy;
                        await _compensationDetailRepository.AddAsync(compensation);
                        await _compensationDetailRepository.SaveChangesAsync();
                    }
                    else
                    {
                        compensation.ModifiedOn = compensationDetail?.ModifiedOn;
                        compensation.ModifiedBy = compensationDetail?.ModifiedBy;
                        newData.ActionType = "Update";
                        newData.CreatedBy = compensationDetail?.ModifiedBy == null ? 0 : (int)compensationDetail?.ModifiedBy;
                        _compensationDetailRepository.Update(compensation);
                        await _compensationDetailRepository.SaveChangesAsync();
                    }
                    newData.CompensationDetail.Add(compensation);

                }
                AddAuditFile(newData, oldDataString);

                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Create the directory path for document
        private string GetDirectoryPath(SupportingDocumentsView supportingDocuments)
        {
            //Add supporting documents
            if (!string.IsNullOrEmpty(supportingDocuments.BaseDirectory))
            {
                //Create base directory
                if (!Directory.Exists(supportingDocuments.BaseDirectory))
                {
                    Directory.CreateDirectory(supportingDocuments.BaseDirectory);
                }
                //Create source type directory
                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType)))
                {
                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType));
                }
                //Create Document type directory
                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType)))
                {
                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType));
                }
                //Create accountId directory
                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType, supportingDocuments.SourceId.ToString())))
                {
                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType, supportingDocuments.SourceId.ToString()));
                }
            }
            return (Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType, supportingDocuments.SourceId.ToString()));
        }
        #endregion

        #region
        private async Task<bool> AddSupportingDocument(SupportingDocumentsView supportingDocuments)
        {
            try
            {
                List<EmployeeDocument> supDocument = new List<EmployeeDocument>();
                if (supportingDocuments.SourceType == "Employee")
                {
                    supDocument = await _employeeDocumentRepository.GetEmployeeDocumentDetailBySourceIdAndDocType(supportingDocuments.SourceId, supportingDocuments.DocumentType);
                }
                else
                {
                    supDocument = await _employeeDocumentRepository.GetEmployeeDocumentDetailBySourceIdSourceType(supportingDocuments.SourceId, supportingDocuments.SourceType);
                }
                List<EmployeeDocument> newDocument = new List<EmployeeDocument>();
                foreach (EmployeeDocument document in supDocument)
                {
                    if (document?.EmployeeDocumentId > 0)
                    {
                        _employeeDocumentRepository.Delete(document);
                        await _employeeDocumentRepository.SaveChangesAsync();

                        //Delete file from physical directory
                        if (File.Exists(document.DocumentPath))
                        {
                            File.Delete(document.DocumentPath);
                        }
                    }
                }

                foreach (DocumentsToUpload item in supportingDocuments?.EmployeeDocumentList)
                {
                    if (item?.DocumentName == null) continue;
                    string documentPath = Path.Combine(supportingDocuments?.BaseDirectory, item.DocumentName);
                    if (!System.IO.File.Exists(item?.DocumentName) && item?.DocumentSize > 0)
                    {
                        if (item.DocumentAsBase64.Contains(","))
                        {
                            item.DocumentAsBase64 = item?.DocumentAsBase64.Substring(item.DocumentAsBase64.IndexOf(",") + 1);
                        }
                        item.DocumentAsByteArray = Convert.FromBase64String(item.DocumentAsBase64);
                        using (Stream fileStream = new FileStream(documentPath, FileMode.Create))
                        {
                            fileStream.Write(item.DocumentAsByteArray, 0, item.DocumentAsByteArray.Length);
                        }
                        EmployeeDocument employeeDocument = new EmployeeDocument();
                        employeeDocument.EmployeeID = supportingDocuments?.EmployeeId;
                        employeeDocument.SourceId = supportingDocuments?.SourceId;
                        employeeDocument.SourceType = supportingDocuments?.SourceType;
                        employeeDocument.DocumentType = item.DocumentCategory;
                        employeeDocument.DocumentPath = documentPath;
                        employeeDocument.DocumentName = item.DocumentName;
                        employeeDocument.CreatedOn = DateTime.UtcNow;
                        employeeDocument.CreatedBy = supportingDocuments?.CreatedBy == null ? 0 : (int)supportingDocuments?.CreatedBy;
                        await _employeeDocumentRepository.AddAsync(employeeDocument);
                        await _employeeDocumentRepository.SaveChangesAsync();
                        newDocument.Add(employeeDocument);
                    }
                    bool result = await AddDocumentAudit(supDocument, newDocument);
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        private async Task<bool> AddDocumentAudit(List<EmployeeDocument> oldDocuments, List<EmployeeDocument> newDocuments)
        {
            foreach (EmployeeDocument document in newDocuments)
            {
                EmployeeDocument oldDocument = oldDocuments?.Where(x => x.SourceId == document.SourceId && x.DocumentType == document.DocumentType).FirstOrDefault();
                if (oldDocument == null) oldDocument = new EmployeeDocument();
                if (document?.DocumentName != oldDocument?.DocumentName)
                {
                    try
                    {
                        EmployeeAudit audit = new EmployeeAudit();
                        audit.ChangeRequestID = Guid.NewGuid();
                        audit.EmployeeId = document.EmployeeID;
                        audit.CreatedOn = DateTime.UtcNow;
                        audit.CreatedBy = document.CreatedBy;
                        audit.Field = document.DocumentType;
                        audit.OldValue = oldDocument.DocumentName;
                        audit.NewValue = document.DocumentName;
                        audit.ActionType = oldDocument?.DocumentName == null ? "Create" : "Update";
                        await _auditRepository.AddAsync(audit);
                        await _auditRepository.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        throw e;
                    }

                }
            }
            return true;
        }
        #region Add Employee Request
        public async Task<ChangeRequestEmailView> AddEmployeeRequest(EmployeesViewModel employeesViewModel)
        {
            ChangeRequestEmailView changeRequestEmail = new ChangeRequestEmailView();
            List<EmployeeRequestView> changesRequestList = new List<EmployeeRequestView>();

            System.Type sourceType;
            System.Type destinationType;
            int employeeId = employeesViewModel.Employee != null ? employeesViewModel.Employee.EmployeeID : (int)employeesViewModel?.EmployeeWorkAndEducationDetail?.EmployeeId;
            List<EmployeeRequestDetail> previousRequest = _employeeRequestRepository.GetPendingRequestByEmployeeIdForFieldCheck(employeeId);
            SupportingDocumentsView supportingDocuments = employeesViewModel.supportingDocumentsViews;
            EmployeeMasterData employeeMaster = GetEmployeeMasterData("");

            if (employeesViewModel.Employee != null)
            {
                EmployeesViewModel oldData = _employeeRepository.GetEmployeeById(employeesViewModel.Employee.EmployeeID);
                Employees newEmployee = employeesViewModel?.Employee;
                Employees oldEmployee = oldData.Employee;
                sourceType = oldEmployee?.GetType();
                destinationType = newEmployee?.GetType();
                if (sourceType == destinationType)
                {
                    // PropertyInfo[] sourceProperties = sourceTypeInfo?.GetProperties();
                    string changeRequest = "";
                    string previousChangeRequest = "";
                    Guid CRIdd = Guid.NewGuid();
                    ArrayList arraylst = new ArrayList();
                    EmployeeRequestView employeeRequest = null;
                    List<EmployeeRequestView> lstEmployeeRequest = new List<EmployeeRequestView>();
                    changeRequest = "Photo";

                    if (Convert.ToString(sourceType.GetProperty("ProfilePicture")?.GetValue(oldEmployee, null)) != Convert.ToString(destinationType.GetProperty("ProfilePicture")?.GetValue(newEmployee, null)) && (previousRequest.Count == 0 || previousRequest?.Find(x => x.Field == "ProfilePicture") == null))
                    {
                        CRIdd = Guid.NewGuid();
                        employeeRequest = new EmployeeRequestView();
                        employeeRequest.EmployeeId = employeesViewModel.Employee.EmployeeID;
                        employeeRequest.RequestCategory = changeRequest;
                        employeeRequest.ChangeRequestId = CRIdd;
                        employeeRequest.Status = "Pending";
                        employeeRequest.CreatedOn = DateTime.UtcNow;
                        employeeRequest.CreatedBy = newEmployee.ModifiedBy;
                        employeeRequest.Remarks = "";
                        employeeRequest.ApprovedBy = 0;
                        if (employeeRequest.EmployeeRequestDetailslst == null)
                            employeeRequest.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();
                        lstEmployeeRequest.Add(employeeRequest);
                        changesRequestList.Add(employeeRequest);
                        EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                        employeeRequestDetails.ChangeRequestId = CRIdd;
                        employeeRequestDetails.Field = "ProfilePicture";
                        employeeRequestDetails.OldValue = sourceType.GetProperty("ProfilePicture").GetValue(oldEmployee, null)?.ToString();
                        employeeRequestDetails.NewValue = destinationType.GetProperty("ProfilePicture").GetValue(newEmployee, null)?.ToString();
                        employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                        employeeRequestDetails.CreatedBy = newEmployee.ModifiedBy;
                        employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                        //  lstEmployeeRequest.Add(employeeRequest);

                    }
                    changeRequest = "Marital Status";
                    if (Convert.ToString(sourceType.GetProperty("Maritalstatus")?.GetValue(oldEmployee, null)) != Convert.ToString(destinationType.GetProperty("Maritalstatus")?.GetValue(newEmployee, null)) && (previousRequest.Count == 0 || previousRequest?.Find(x => x.Field == "Maritalstatus") == null))
                    {
                        CRIdd = Guid.NewGuid();
                        employeeRequest = new EmployeeRequestView();
                        employeeRequest.EmployeeId = employeesViewModel.Employee.EmployeeID;
                        employeeRequest.RequestCategory = changeRequest;
                        employeeRequest.ChangeRequestId = CRIdd;
                        employeeRequest.Status = "Pending";
                        employeeRequest.CreatedOn = DateTime.UtcNow;
                        employeeRequest.CreatedBy = newEmployee.ModifiedBy;
                        employeeRequest.Remarks = "";
                        employeeRequest.ApprovedBy = 0;
                        if (employeeRequest.EmployeeRequestDetailslst == null)
                            employeeRequest.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();
                        lstEmployeeRequest.Add(employeeRequest);
                        changesRequestList.Add(employeeRequest);
                        EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                        employeeRequestDetails.ChangeRequestId = CRIdd;
                        employeeRequestDetails.Field = "Maritalstatus";
                        employeeRequestDetails.OldValue = sourceType.GetProperty("Maritalstatus").GetValue(oldEmployee, null)?.ToString();
                        employeeRequestDetails.NewValue = destinationType.GetProperty("Maritalstatus").GetValue(newEmployee, null)?.ToString();
                        employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                        employeeRequestDetails.CreatedBy = newEmployee.ModifiedBy;
                        employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                        //  lstEmployeeRequest.Add(employeeRequest);
                        if (employeesViewModel.RequestProofDocument.Count > 0)
                        {
                            var document = employeesViewModel.RequestProofDocument.Find(x => x.DocumentCategory == "maritalstatusProof");
                            employeesViewModel.supportingDocumentsViews.proofDocumentId = CRIdd;
                            employeesViewModel.supportingDocumentsViews.DocumentType = changeRequest;
                            employeesViewModel.supportingDocumentsViews.SourceId = employeeId;
                            var getPath = GetDirectoryPathForRequestDoc(employeesViewModel.supportingDocumentsViews);
                            employeeRequest.DocumentPath = getPath;
                            employeeRequest.RequestProof = document;
                        }
                    }
                    arraylst.Clear();

                    await AddRequest(lstEmployeeRequest);

                }
            }

            if ((employeesViewModel.EmployeesPersonalInfo) != null)
            {
                EmployeePersonalInfoView newPersonalInfo = employeesViewModel.EmployeesPersonalInfo;
                EmployeePersonalInfoView oldPersonalInfo = GetEmployeesPersonalInformation(employeesViewModel.Employee.EmployeeID);
                EmployeesPersonalInfo employeesPersonalInfo = _employeesPersonalInfoRepository.GetEmployeePersonalInfoById(oldPersonalInfo?.PersonalInfoId);
                if (employeesPersonalInfo == null) employeesPersonalInfo = new EmployeesPersonalInfo();
                System.Type sourceTypeInfo = oldPersonalInfo?.GetType();
                System.Type destinationTypeInfo = newPersonalInfo?.GetType();
                if (sourceTypeInfo == destinationTypeInfo)
                {
                    PropertyInfo[] sourceProperties = sourceTypeInfo?.GetProperties();
                    string changeRequest = "";
                    string previousChangeRequest = "";
                    Guid CRIdd = Guid.NewGuid();
                    Dictionary<string, Guid> headerColumns = new Dictionary<string, Guid>();

                    // headerColumns.Add("PERSONAL INFO", Guid.NewGuid());
                    headerColumns.Add("Permanent Address", Guid.NewGuid());
                    headerColumns.Add("Communication Address", Guid.NewGuid());
                    headerColumns.Add("EMERGENCY CONTACT INFORMATION", Guid.NewGuid());
                    // headerColumns.Add("REFERENCE DETAIL", Guid.NewGuid());
                    //headerColumns.Add("JOINING BONUS", Guid.NewGuid());
                    headerColumns.Add("BANK DETAIL", Guid.NewGuid());
                    ArrayList arraylst = new ArrayList();
                    EmployeeRequestView employeeRequest = null;
                    List<EmployeeRequestView> lstEmployeeRequest = new List<EmployeeRequestView>();

                    foreach (PropertyInfo property in sourceProperties)
                    {

                        changeRequest = GetRequestchange(property.Name);
                        if (previousRequest == null || previousRequest?.Find(x => x.Field == property.Name) == null)
                        {
                            if (Convert.ToString(sourceTypeInfo.GetProperty(property.Name).GetValue(oldPersonalInfo, null)) != Convert.ToString(destinationTypeInfo.GetProperty(property.Name).GetValue(newPersonalInfo, null)))
                            {

                                if (!string.IsNullOrEmpty(changeRequest))
                                {
                                    if (!arraylst.Contains(changeRequest))
                                    {
                                        CRIdd = headerColumns[changeRequest];
                                        employeeRequest = new EmployeeRequestView();
                                        employeeRequest.EmployeeId = employeesViewModel.Employee.EmployeeID;
                                        employeeRequest.RequestCategory = changeRequest;
                                        employeeRequest.ChangeRequestId = CRIdd;
                                        employeeRequest.Status = "Pending";
                                        employeeRequest.CreatedOn = DateTime.UtcNow;
                                        employeeRequest.Remarks = "";
                                        employeeRequest.ApprovedBy = 0;
                                        employeeRequest.CreatedBy = newPersonalInfo.ModifiedBy;
                                        if (employeeRequest.EmployeeRequestDetailslst == null)
                                            employeeRequest.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();
                                        if (changeRequest != "EMERGENCY CONTACT INFORMATION")
                                        {
                                            lstEmployeeRequest.Add(employeeRequest);
                                        }

                                        changesRequestList.Add(employeeRequest);
                                        arraylst.Add(changeRequest);
                                        if (employeesViewModel.RequestProofDocument.Count > 0 && (changeRequest == "Permanent Address" || changeRequest == "Communication Address"))
                                        {
                                            string docType = (changeRequest == "Permanent Address") ? "permanentAddressProof" : "communicationAddressProof";
                                            var document = employeesViewModel.RequestProofDocument.Find(x => x.DocumentCategory == docType);
                                            employeesViewModel.supportingDocumentsViews.proofDocumentId = CRIdd;
                                            employeesViewModel.supportingDocumentsViews.DocumentType = docType;
                                            employeesViewModel.supportingDocumentsViews.SourceId = employeeId;
                                            var getPath = GetDirectoryPathForRequestDoc(employeesViewModel.supportingDocumentsViews);
                                            employeeRequest.DocumentPath = getPath;
                                            employeeRequest.RequestProof = document;
                                        }
                                    }
                                    //else
                                    //{
                                    //    CRIdd = Guid.NewGuid();

                                    //    employeeRequest = new EmployeeRequestView();
                                    //    employeeRequest.EmployeeId = employeesViewModel.Employee.EmployeeID;
                                    //    employeeRequest.RequestCategory = changeRequest;
                                    //    employeeRequest.ChangeRequestId = CRIdd;
                                    //    employeeRequest.Status = "Pending";
                                    //    employeeRequest.CreatedOn = DateTime.UtcNow;
                                    //    employeeRequest.CreatedBy = newPersonalInfo.CreatedBy;
                                    //    employeeRequest.Remarks = "";
                                    //    employeeRequest.ApprovedBy = 0;
                                    //    if (employeeRequest.EmployeeRequestDetailslst == null)
                                    //        employeeRequest.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();
                                    //    lstEmployeeRequest.Add(employeeRequest);
                                    //}

                                    if (((property.PropertyType == (typeof(int?))) || (property.PropertyType == (typeof(int)))))
                                    {
                                        EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                                        employeeRequestDetails.ChangeRequestId = CRIdd;
                                        int NewId = destinationTypeInfo.GetProperty(property.Name).GetValue(newPersonalInfo, null) == null ? 0 : (int)destinationTypeInfo.GetProperty(property.Name).GetValue(newPersonalInfo, null);
                                        int oldId = sourceTypeInfo.GetProperty(property.Name).GetValue(oldPersonalInfo, null) == null ? 0 : (int)sourceTypeInfo.GetProperty(property.Name).GetValue(oldPersonalInfo, null);
                                        switch (property.Name)
                                        {

                                            case "PermanentState":
                                                employeeRequestDetails.NewValue = employeeMaster?.StateList.Find(x => x.StateId == NewId)?.StateName;
                                                employeeRequestDetails.OldValue = employeeMaster?.StateList.Find(x => x.StateId == oldId)?.StateName;
                                                break;
                                            case "PermanentCountry":
                                                employeeRequestDetails.NewValue = employeeMaster?.CountryList.Find(x => x.CountryId == NewId)?.CountryName;
                                                employeeRequestDetails.OldValue = employeeMaster?.CountryList.Find(x => x.CountryId == oldId)?.CountryName;
                                                break;
                                            case "CommunicationState":
                                                employeeRequestDetails.NewValue = employeeMaster?.StateList?.Find(x => x.StateId == NewId)?.StateName;
                                                employeeRequestDetails.OldValue = employeeMaster?.StateList?.Find(x => x.StateId == oldId)?.StateName;
                                                break;
                                            case "CommunicationCountry":
                                                employeeRequestDetails.NewValue = employeeMaster.CountryList?.Find(x => x.CountryId == NewId)?.CountryName;
                                                employeeRequestDetails.OldValue = employeeMaster.CountryList?.Find(x => x.CountryId == oldId)?.CountryName;
                                                break;
                                            default:
                                                break;
                                        }
                                        employeeRequestDetails.Field = property.Name;
                                        employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                                        employeeRequestDetails.CreatedBy = newPersonalInfo.CreatedBy;
                                        employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                                    }
                                    else if (((property.PropertyType == (typeof(decimal?)))))
                                    {

                                        decimal NewId = destinationTypeInfo.GetProperty(property.Name)?.GetValue(newPersonalInfo, null)?.ToString() == null ? 0 : Convert.ToDecimal(destinationTypeInfo.GetProperty(property.Name)?.GetValue(newPersonalInfo, null)?.ToString());
                                        decimal oldId = sourceTypeInfo.GetProperty(property.Name)?.GetValue(oldPersonalInfo, null)?.ToString() == null ? 0 : Convert.ToDecimal(sourceTypeInfo.GetProperty(property.Name)?.GetValue(oldPersonalInfo, null)?.ToString());
                                        if (NewId.CompareTo(oldId) != 0)
                                        {
                                            EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                                            employeeRequestDetails.ChangeRequestId = CRIdd;
                                            employeeRequestDetails.Field = property.Name;
                                            employeeRequestDetails.OldValue = Math.Round(oldId).ToString();
                                            employeeRequestDetails.NewValue = Math.Round(NewId).ToString();
                                            employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                                            employeeRequestDetails.CreatedBy = newPersonalInfo.CreatedBy;
                                            employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                                        }
                                        else if (employeeRequest?.EmployeeRequestDetailslst?.Count == 0)
                                        {
                                            lstEmployeeRequest.Remove(employeeRequest);
                                        }
                                    }
                                    else
                                    {
                                        EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                                        employeeRequestDetails.ChangeRequestId = CRIdd;
                                        employeeRequestDetails.Field = property.Name;
                                        employeeRequestDetails.OldValue = sourceTypeInfo.GetProperty(property.Name).GetValue(oldPersonalInfo, null)?.ToString();
                                        employeeRequestDetails.NewValue = destinationTypeInfo.GetProperty(property.Name).GetValue(newPersonalInfo, null)?.ToString();
                                        employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                                        employeeRequestDetails.CreatedBy = newPersonalInfo.CreatedBy;
                                        employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                                    }

                                    if (changeRequest == "EMERGENCY CONTACT INFORMATION")
                                    {
                                        var data = employeesPersonalInfo.GetType().GetProperty(property.Name);
                                        data.SetValue(employeesPersonalInfo, destinationTypeInfo.GetProperty(property.Name).GetValue(newPersonalInfo, null)?.ToString());
                                    }
                                }


                                //  lstEmployeeRequest.Add(employeeRequest);

                            }

                            /*if (previousChangeRequest != "" && previousChangeRequest != changeRequest)
                            {
                                CRIdd = Guid.NewGuid();
                            }

                            previousChangeRequest = changeRequest;*/
                        }
                    }
                    _employeesPersonalInfoRepository.Update(employeesPersonalInfo);
                    await _employeesPersonalInfoRepository.SaveChangesAsync();
                    arraylst.Clear();
                    await AddRequest(lstEmployeeRequest);
                    var emergencyContactChange = changesRequestList.Find(x => x.RequestCategory == "EMERGENCY CONTACT INFORMATION");
                    if (emergencyContactChange != null)
                    {
                        await AddEmployeeChangesToAuditWithoutRequest(new AuditView(), emergencyContactChange);
                    }

                }
            }

            if (employeesViewModel?.EmployeeWorkAndEducationDetail?.EducationDetailsList?.Count > 0)
            {
                EmployeeRequestView employeeRequest = null;
                List<EmployeeRequestView> lstEmployeeRequest = new List<EmployeeRequestView>();
                foreach (EducationDetailView neweducationDetail in employeesViewModel?.EmployeeWorkAndEducationDetail?.EducationDetailsList)
                {
                    if (neweducationDetail.EducationDetailId == 0)
                    {
                        string changeRequest = "";
                        changeRequest = "Certification & Skills";
                        Guid CRId = Guid.NewGuid();
                        destinationType = neweducationDetail?.GetType();
                        employeeRequest = new EmployeeRequestView();
                        employeeRequest.EmployeeId = (int)employeesViewModel?.EmployeeWorkAndEducationDetail.EmployeeId;
                        employeeRequest.RequestCategory = changeRequest;
                        employeeRequest.ChangeRequestId = CRId;
                        employeeRequest.Status = "Pending";
                        employeeRequest.CreatedOn = DateTime.UtcNow;
                        employeeRequest.Remarks = "";
                        employeeRequest.ApprovedBy = 0;
                        employeeRequest.CreatedBy = neweducationDetail.CreatedBy;
                        if (employeeRequest.EmployeeRequestDetailslst == null)
                            employeeRequest.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();
                        lstEmployeeRequest.Add(employeeRequest);
                        changesRequestList.Add(employeeRequest);
                        // var document = employeesViewModel.RequestProofDocument.Find(x => x.DocumentCategory == changeRequest);
                        employeesViewModel.supportingDocumentsViews.proofDocumentId = CRId;
                        employeesViewModel.supportingDocumentsViews.DocumentType = changeRequest;
                        var getPath = GetDirectoryPathForRequestDoc(employeesViewModel.supportingDocumentsViews);
                        employeeRequest.DocumentPath = getPath;
                        employeeRequest.RequestProof = neweducationDetail.Certificate;
                        PropertyInfo[] sourceProperties = destinationType?.GetProperties();
                        foreach (PropertyInfo pi in sourceProperties)
                        {
                            if ((pi.Name == "CertificateName" || pi.Name == "EducationTypeId" || pi.Name == "ExpiryYear" || pi.Name == "MarkPercentage" || pi.Name == "YearOfCompletion"))
                            {
                                EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                                employeeRequestDetails.ChangeRequestId = CRId;
                                employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                                employeeRequestDetails.CreatedBy = neweducationDetail.CreatedBy;
                                if (((pi.PropertyType == (typeof(int?))) || (pi.PropertyType == (typeof(int)))))
                                {
                                    int NewId = destinationType.GetProperty(pi.Name)?.GetValue(neweducationDetail, null) == null ? 0 : (int)destinationType.GetProperty(pi.Name)?.GetValue(neweducationDetail, null);

                                    switch (pi.Name)
                                    {
                                        case "EducationTypeId":
                                            employeeRequestDetails.NewValue = employeeMaster?.QualificationLIst?.Find(x => x.Key == NewId)?.Value;
                                            employeeRequestDetails.OldValue = null;
                                            break;

                                        case "BoardId":
                                            employeeRequestDetails.NewValue = employeeMaster?.Board?.Find(x => x.Key == NewId)?.Value;
                                            employeeRequestDetails.OldValue = null;
                                            break;

                                        default:
                                            employeeRequestDetails.NewValue = NewId.ToString();
                                            employeeRequestDetails.OldValue = null;
                                            break;

                                    }
                                    employeeRequestDetails.Field = destinationType.GetProperty(pi.Name).ToString().Substring(destinationType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                }
                                else if (((pi.PropertyType == (typeof(decimal?)))))
                                {
                                    decimal NewId = destinationType.GetProperty(pi.Name)?.GetValue(neweducationDetail, null) == null ? 0 : (decimal)destinationType.GetProperty(pi.Name)?.GetValue(neweducationDetail, null);
                                    decimal oldId = 0;
                                    if (NewId.CompareTo(oldId) != 0)
                                    {
                                        employeeRequestDetails.NewValue = NewId.ToString();
                                        employeeRequestDetails.OldValue = "0";
                                        employeeRequestDetails.Field = destinationType.GetProperty(pi.Name).ToString().Substring(destinationType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                    }
                                    else
                                    {
                                        employeeRequestDetails = null;
                                    }
                                }
                                else
                                {
                                    employeeRequestDetails.NewValue = destinationType.GetProperty(pi.Name).GetValue(neweducationDetail, null)?.ToString();
                                    employeeRequestDetails.OldValue = null;
                                    employeeRequestDetails.Field = destinationType.GetProperty(pi.Name).ToString().Substring(destinationType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                }
                                if (employeeRequestDetails != null)
                                {
                                    employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                                }
                            }
                        }
                    }

                }
                if (lstEmployeeRequest?.Count > 0)
                {
                    await AddRequest(lstEmployeeRequest);
                }

            }

            if (employeesViewModel?.EmployeeDependent != null)
            {
                EmployeeRequestView employeeRequest = null;
                List<EmployeeRequestView> lstEmployeeRequest = new List<EmployeeRequestView>();
                // List<EmployeeDependentView> addedDependent = employeesViewModel?.EmployeeDependent.Where(x => x.EmployeeDependentId == 0).ToList();
                foreach (EmployeeDependentView employeeDependentView in employeesViewModel.EmployeeDependent)
                {
                    Guid CRId = Guid.NewGuid();
                    employeeRequest = new EmployeeRequestView();
                    employeeRequest.EmployeeId = employeesViewModel.Employee.EmployeeID;
                    employeeRequest.RequestCategory = "Dependent Details";
                    employeeRequest.ChangeRequestId = CRId;
                    employeeRequest.Status = "Pending";
                    employeeRequest.SourceId = employeeDependentView?.EmployeeDependentId;
                    employeeRequest.ChangeType = employeeDependentView?.EmployeeDependentId == 0 ? "Added" : "Updated";
                    employeeRequest.CreatedOn = DateTime.UtcNow;
                    employeeRequest.Remarks = "";
                    employeeRequest.ApprovedBy = 0;
                    employeeRequest.CreatedBy = employeeDependentView?.CreatedBy;
                    if (employeeRequest.EmployeeRequestDetailslst == null)
                        employeeRequest.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();
                    lstEmployeeRequest.Add(employeeRequest);
                    changesRequestList.Add(employeeRequest);
                    employeesViewModel.supportingDocumentsViews = supportingDocuments;
                    employeesViewModel.supportingDocumentsViews.proofDocumentId = CRId;
                    employeesViewModel.supportingDocumentsViews.DocumentType = "Dependent Details";
                    var getPath = GetDirectoryPathForRequestDoc(employeesViewModel?.supportingDocumentsViews);
                    employeeRequest.DocumentPath = getPath;
                    employeeRequest.RequestProof = employeeDependentView?.DependentDetailsProof;
                    EmployeeDependentView sourceDependent = employeeDependentView?.EmployeeDependentId == 0 ? new EmployeeDependentView() : _employeeDependentRepository.GetEmployeeDependentDetailById(employeeDependentView.EmployeeDependentId);
                    if (sourceDependent == null) sourceDependent = new EmployeeDependentView();
                    destinationType = employeeDependentView?.GetType();
                    sourceType = sourceDependent?.GetType();
                    PropertyInfo[] destinationProperties = destinationType?.GetProperties();
                    if (sourceType == destinationType)
                    {
                        foreach (PropertyInfo pi in destinationProperties)
                        {
                            if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "EmployeeDependentId" && pi.Name != "EmployeeID" && pi.Name != "DependentDetailsProof" && pi.Name != "EmployeeRelationship" && Convert.ToString(sourceType?.GetProperty(pi.Name).GetValue(sourceDependent, null)) != Convert.ToString(destinationType.GetProperty(pi.Name).GetValue(employeeDependentView, null))))
                            {
                                EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                                employeeRequestDetails.ChangeRequestId = CRId;
                                employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                                employeeRequestDetails.CreatedBy = employeeDependentView.CreatedBy;
                                employeeRequestDetails.Field = destinationType.GetProperty(pi.Name).ToString().Substring(destinationType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                if (pi.Name == "EmployeeRelationshipId")
                                {
                                    int NewId = destinationType.GetProperty(pi.Name)?.GetValue(employeeDependentView, null) == null ? 0 : (int)destinationType.GetProperty(pi.Name)?.GetValue(employeeDependentView, null);
                                    int OldId = sourceType.GetProperty(pi.Name)?.GetValue(sourceDependent, null) == null ? 0 : (int)sourceType.GetProperty(pi.Name)?.GetValue(sourceDependent, null);
                                    employeeRequestDetails.NewValue = employeeMaster?.EmployeeRelationshipList?.Find(x => x.EmployeeRelationshipId == NewId)?.EmployeeRelationshipName;
                                    employeeRequestDetails.OldValue = employeeMaster?.EmployeeRelationshipList?.Find(x => x.EmployeeRelationshipId == OldId)?.EmployeeRelationshipName;
                                }

                                else
                                {
                                    employeeRequestDetails.NewValue = destinationType.GetProperty(pi.Name).GetValue(employeeDependentView, null)?.ToString();
                                    employeeRequestDetails.OldValue = sourceType.GetProperty(pi.Name).GetValue(sourceDependent, null)?.ToString(); ;
                                }
                                employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                            }
                        }
                    }
                    if (employeeRequest?.EmployeeRequestDetailslst?.Count == 0)
                    {
                        lstEmployeeRequest.Remove(employeeRequest);
                        changesRequestList.Remove(employeeRequest);
                    }

                }
                var oldDependent = _employeeDependentRepository.GetEmployeeDependentByEmployeeId(employeeId);
                List<int> newDependent = employeesViewModel.EmployeeDependent.Select(x => x.EmployeeDependentId).ToList();
                List<EmployeeDependent> removedDependent = oldDependent?.Where(x => !newDependent.Contains(x.EmployeeDependentId)).ToList();
                foreach (EmployeeDependent employeeDependentView in removedDependent)
                {
                    Guid CRId = Guid.NewGuid();
                    employeeRequest = new EmployeeRequestView();
                    employeeRequest.EmployeeId = employeesViewModel.Employee.EmployeeID;
                    employeeRequest.RequestCategory = "Dependent Details";
                    employeeRequest.ChangeRequestId = CRId;
                    employeeRequest.Status = "Pending";
                    employeeRequest.SourceId = employeeDependentView.EmployeeDependentId;
                    employeeRequest.ChangeType = "Removed";
                    employeeRequest.CreatedOn = DateTime.UtcNow;
                    employeeRequest.Remarks = "";
                    employeeRequest.ApprovedBy = 0;
                    employeeRequest.CreatedBy = employeeDependentView.CreatedBy;
                    if (employeeRequest.EmployeeRequestDetailslst == null)
                        employeeRequest.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();
                    lstEmployeeRequest.Add(employeeRequest);
                    changesRequestList.Add(employeeRequest);
                    destinationType = employeeDependentView?.GetType();
                    PropertyInfo[] destinationProperties = destinationType?.GetProperties();
                    foreach (PropertyInfo pi in destinationProperties)
                    {
                        if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "EmployeeDependentId" && pi.Name != "EmployeeID"))
                        {
                            EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                            employeeRequestDetails.ChangeRequestId = CRId;
                            employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                            employeeRequestDetails.CreatedBy = employeeDependentView.CreatedBy;
                            employeeRequestDetails.Field = destinationType.GetProperty(pi.Name).ToString().Substring(destinationType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                            if (pi.Name == "EmployeeRelationshipId")
                            {
                                int NewId = destinationType.GetProperty(pi.Name)?.GetValue(employeeDependentView, null) == null ? 0 : (int)destinationType.GetProperty(pi.Name)?.GetValue(employeeDependentView, null);
                                employeeRequestDetails.NewValue = null;
                                employeeRequestDetails.OldValue = employeeMaster?.EmployeeRelationshipList?.Find(x => x.EmployeeRelationshipId == NewId)?.EmployeeRelationshipName;
                            }
                            else
                            {
                                employeeRequestDetails.NewValue = null;
                                employeeRequestDetails.OldValue = destinationType.GetProperty(pi.Name).GetValue(employeeDependentView, null)?.ToString(); ;
                            }
                            employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                        }


                    }
                    if (employeeRequest?.EmployeeRequestDetailslst?.Count == 0)
                    {
                        lstEmployeeRequest.Remove(employeeRequest);
                        changesRequestList.Remove(employeeRequest);
                    }
                }
                if (lstEmployeeRequest?.Count > 0)
                {
                    await AddRequest(lstEmployeeRequest);
                }

            }

            if (employeesViewModel?.Skillset != null)
            {
                EmployeeRequestView employeeRequest = null;
                List<int> oldSkillSetId = _employeesSkillsetRepository.GetEmployeeSkillsIdByEmployeeId(employeeId);
                List<int> requestChangeSkills = employeesViewModel?.Skillset?.OrderBy(x => x.SkillsetId).Select(x => x.SkillsetId).ToList();
                List<int> removedSkills = oldSkillSetId.Where(x => !requestChangeSkills.Contains(x)).ToList();
                List<int> AddedSkills = requestChangeSkills.Where(x => !oldSkillSetId.Contains(x)).ToList();
                string removedSkillName = "";
                string addedSkillName = "";

                if (AddedSkills.Count != 0 || removedSkills.Count != 0)
                {
                    List<EmployeeRequestView> lstEmployeeRequest = new List<EmployeeRequestView>();
                    List<EmployeesSkillset> employeeSkillsets = _employeesSkillsetRepository.GetEmployeesSkillsetByEmployeeId(employeeId);
                    Guid CRId = Guid.NewGuid();
                    employeeRequest = new EmployeeRequestView();
                    employeeRequest.EmployeeId = employeesViewModel.Employee.EmployeeID;
                    employeeRequest.RequestCategory = "Skills";
                    employeeRequest.ChangeRequestId = CRId;
                    employeeRequest.Status = "Pending";
                    employeeRequest.CreatedOn = DateTime.UtcNow;
                    employeeRequest.Remarks = "";
                    employeeRequest.ApprovedBy = 0;
                    employeeRequest.CreatedBy = employeesViewModel.CreatedBy;
                    if (employeeRequest.EmployeeRequestDetailslst == null)
                        employeeRequest.EmployeeRequestDetailslst = new List<EmployeeRequestDetailsView>();
                    //  lstEmployeeRequest.Add(employeeRequest);
                    changesRequestList.Add(employeeRequest);
                    //foreach (Skillsets skillset in employeesViewModel?.Skillset)
                    //{
                    //    EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                    //    employeeRequestDetails.ChangeRequestId = CRId;
                    //    employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                    //    employeeRequestDetails.CreatedBy = employeesViewModel.Employee.ModifiedBy;
                    //    employeeRequestDetails.NewValue = skillset.Skillset;
                    //    employeeRequestDetails.OldValue = null;
                    //    employeeRequestDetails.Field = "Skills";
                    //    employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                    //}
                    foreach (int skillid in removedSkills)
                    {
                        EmployeesSkillset employeesSkillset = _employeesSkillsetRepository.GetEmployeesSkillsetBySkillsetId(skillid, employeeId);
                        _employeesSkillsetRepository.Delete(employeesSkillset);
                        _employeesSkillsetRepository.SaveChangesAsync();
                        removedSkillName = employeeMaster.SkillsetList.Find(x => x.SkillsetId == skillid).Skillset;
                        EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                        employeeRequestDetails.ChangeRequestId = CRId;
                        employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                        employeeRequestDetails.CreatedBy = employeesViewModel.Employee.ModifiedBy;
                        employeeRequestDetails.NewValue = null;
                        employeeRequestDetails.OldValue = removedSkillName;
                        employeeRequestDetails.Field = "Skill Removed";
                        employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                        AuditView audit = new AuditView();
                        audit.OldValue = removedSkillName;
                        audit.NewValue = null;
                        audit.Field = "Skill Removed";
                        AddEmployeeChangesToAuditWithoutRequest(audit, employeeRequest);
                    }
                    foreach (int skillid in AddedSkills)
                    {
                        EmployeesSkillset employeesskillSet = new EmployeesSkillset();
                        addedSkillName = employeeMaster.SkillsetList.Find(x => x.SkillsetId == skillid).Skillset;
                        employeesskillSet.EmployeeId = employeeId;
                        employeesskillSet.SkillsetId = skillid;
                        employeesskillSet.CreatedBy = employeeRequest.CreatedBy;
                        employeesskillSet.CreatedOn = DateTime.UtcNow;
                        _employeesSkillsetRepository.AddAsync(employeesskillSet);
                        _employeesSkillsetRepository.SaveChangesAsync();
                        EmployeeRequestDetailsView employeeRequestDetails = new EmployeeRequestDetailsView();
                        employeeRequestDetails.ChangeRequestId = CRId;
                        employeeRequestDetails.CreatedOn = DateTime.UtcNow;
                        employeeRequestDetails.CreatedBy = employeesViewModel.Employee.ModifiedBy;
                        employeeRequestDetails.NewValue = addedSkillName;
                        employeeRequestDetails.OldValue = null;
                        employeeRequestDetails.Field = "Skill Added";
                        employeeRequest.EmployeeRequestDetailslst.Add(employeeRequestDetails);
                        AuditView audit = new AuditView();
                        audit.OldValue = null;
                        audit.NewValue = addedSkillName;
                        audit.Field = "Skill Added";
                        AddEmployeeChangesToAuditWithoutRequest(audit, employeeRequest);
                    }
                    //await AddRequest(lstEmployeeRequest);


                }

            }
            changeRequestEmail.employeeRequests = changesRequestList;
            changeRequestEmail.employeeMasterEmail = await _employeeRepository.getEmployeeEmailByName("ChangeRequest");
            changeRequestEmail.EmployeeDetail = _employeeRepository.GetEmployeeNameByEmployeeId(employeeId);
            return changeRequestEmail;
        }
        #endregion

        public string GetRequestchange(string property)
        {
            string changeRequest = "";
            // if(property== "PermanentAddressLine1" || property== "PermanentAddressLine2" || property == "PermanentCity" || property == "PermanentStateName" || property == "PermanentCountryName" || property == "PermanentAddressZip")
            if (property == "PermanentAddressLine1" || property == "PermanentAddressLine2" || property == "PermanentCity" || property == "PermanentState" || property == "PermanentCountry" || property == "PermanentAddressZip")
            {

                changeRequest = "Permanent Address";
                //  headerColumnscolumn.Add(Guid.NewGuid(), "Permanent Address");
            }
            else if (property == "CommunicationAddressLine1" || property == "CommunicationAddressLine2" || property == "CommunicationCity" || property == "CommunicationState" || property == "CommunicationCountry" || property == "CommunicationAddressZip")
            {
                changeRequest = "Communication Address";
            }

            else if (property == "AccountHolderName" || property == "BankName" || property == "IFSCCode" || property == "AccountNumber")
            {
                changeRequest = "BANK DETAIL";
            }

            //else if (property == "IsJoiningBonus" || property == "JoiningBonusAmmount" || property == "JoiningBonusCondition")
            //{
            //    changeRequest = "JOINING BONUS";
            //}

            else if (property == "EmergencyContactName" || property == "EmergencyContactRelation" || property == "EmergencyMobileNumber")
            {
                changeRequest = "EMERGENCY CONTACT INFORMATION";
            }
            //else if (property == "ReferenceContactName" || property == "ReferenceEmailId" || property == "ReferenceMobileNumber")
            //{
            //    changeRequest = "REFERENCE DETAIL";
            //}
            else if (property == "Maritalstatus")
            {
                changeRequest = "Marital Status";
            }
            else if (property == "ProfilePicture")
            {
                changeRequest = "Photo";
            }

            return changeRequest;
        }

        #region  Add Request

        private async Task<bool> AddRequest(List<EmployeeRequestView> lstEmployeeRequest)
        {
            foreach (EmployeeRequestView employeeRequest in lstEmployeeRequest)
            {
                EmployeeRequest employeeRequest1 = new EmployeeRequest();

                employeeRequest1.EmployeeId = employeeRequest.EmployeeId;
                employeeRequest1.RequestCategory = employeeRequest.RequestCategory;
                employeeRequest1.ChangeRequestId = employeeRequest.ChangeRequestId;
                employeeRequest1.Status = "Pending";
                employeeRequest1.CreatedOn = DateTime.UtcNow;
                employeeRequest1.CreatedBy = employeeRequest.CreatedBy;
                employeeRequest1.Remark = "";
                employeeRequest1.ApprovedBy = 0;
                employeeRequest1.ChangeType = employeeRequest.ChangeType;
                employeeRequest1.SourceId = employeeRequest.SourceId;
                await _employeeRequestRepository.AddAsync(employeeRequest1);
                await _employeeRequestRepository.SaveChangesAsync();
                foreach (EmployeeRequestDetailsView employeeRequestDetails in employeeRequest.EmployeeRequestDetailslst)
                {
                    EmployeeRequestDetail employeeRequestDetails1 = new EmployeeRequestDetail();


                    employeeRequestDetails1.ChangeRequestId = employeeRequestDetails.ChangeRequestId;
                    employeeRequestDetails1.Field = employeeRequestDetails.Field;
                    employeeRequestDetails1.OldValue = employeeRequestDetails.OldValue;
                    employeeRequestDetails1.NewValue = employeeRequestDetails.NewValue; employeeRequestDetails1.CreatedOn = employeeRequestDetails.CreatedOn;
                    employeeRequestDetails1.CreatedBy = employeeRequestDetails.CreatedBy;

                    await _employeeRequestDetailsRepository.AddAsync(employeeRequestDetails1);
                    await _employeeRequestDetailsRepository.SaveChangesAsync();
                }
                AddProofDocument(employeeRequest);

            }

            return true;

        }

        #endregion

        #region Adding Audit History
        private async Task<bool> AddAuditFile(EmployeeAuditDataView newData, string oldData)
        {
            List<AuditView> auditDataList = new List<AuditView>();
            Guid ChangeRequestID = Guid.NewGuid();
            EmployeeMasterData employeeMaster = GetEmployeeMasterData("");
            employeeMaster.ReportingManagerList = this.GetEmployeeDropDownList(true);
            EmployeeAuditDataView oldValue = new EmployeeAuditDataView();
            List<EmployeeAudit> auditsList = new List<EmployeeAudit>();
            if (oldData != null)
            {
                oldValue = JsonConvert.DeserializeObject<EmployeeAuditDataView>(oldData);
            }
            System.Type sourceType;
            System.Type destinationType;
            //Employee Audit
            try
            {
                if (newData.Employee != null)
                {
                    Employees newEmployee = newData.Employee;
                    Employees oldEmployee = new Employees();
                    if (oldValue.Employee != null) oldEmployee = oldValue.Employee;

                    sourceType = oldEmployee?.GetType();
                    destinationType = newEmployee?.GetType();
                    if (sourceType == destinationType && newEmployee != null)
                    {
                        PropertyInfo[] sourceProperties = sourceType?.GetProperties();
                        foreach (PropertyInfo pi in sourceProperties)
                        {
                            try
                            {
                                if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "EmployeeID") && (sourceType.GetProperty(pi.Name).GetValue(oldEmployee, null)?.ToString()?.Trim() != destinationType.GetProperty(pi.Name).GetValue(newEmployee, null)?.ToString()?.Trim()))
                                {
                                    EmployeeAudit audit = new EmployeeAudit();
                                    audit.EmployeeId = newEmployee.EmployeeID;
                                    audit.ChangeRequestID = ChangeRequestID;
                                    audit.CreatedOn = DateTime.UtcNow;
                                    audit.CreatedBy = newData.CreatedBy;
                                    audit.ActionType = newData.ActionType;
                                    if (((pi.PropertyType == (typeof(int?))) || (pi.PropertyType == (typeof(int)))))
                                    {
                                        int NewId = destinationType.GetProperty(pi.Name)?.GetValue(newEmployee, null) == null ? 0 : (int)destinationType.GetProperty(pi.Name)?.GetValue(newEmployee, null);
                                        int oldId = sourceType.GetProperty(pi.Name).GetValue(oldEmployee, null) == null ? 0 : (int)sourceType.GetProperty(pi.Name).GetValue(oldEmployee, null);
                                        switch (pi.Name)
                                        {
                                            case "DepartmentId":
                                                audit.NewValue = employeeMaster?.EmployeeDepartmentList?.Find(x => x.DepartmentId == NewId)?.DepartmentName;
                                                audit.OldValue = employeeMaster?.EmployeeDepartmentList?.Find(x => x.DepartmentId == oldId)?.DepartmentName;
                                                audit.Field = "Department";
                                                break;
                                            case "DesignationId":
                                                audit.NewValue = employeeMaster?.DesignationList?.Find(x => x.DesignationId == NewId)?.DesignationName;
                                                audit.OldValue = employeeMaster?.DesignationList?.Find(x => x.DesignationId == oldId)?.DesignationName;
                                                audit.Field = "Designation";
                                                break;
                                            case "RoleId":
                                                audit.NewValue = employeeMaster?.RolesList?.Find(x => x.RoleId == NewId)?.RoleName;
                                                audit.OldValue = employeeMaster?.RolesList?.Find(x => x.RoleId == oldId)?.RoleName;
                                                audit.Field = "OKR Role";
                                                break;
                                            case "SystemRoleId":
                                                audit.NewValue = employeeMaster?.SystemRolesList?.Find(x => x.RoleId == NewId)?.RoleName;
                                                audit.OldValue = employeeMaster?.SystemRolesList?.Find(x => x.RoleId == oldId)?.RoleName;
                                                audit.Field = "System Role";
                                                break;
                                            case "LocationId":
                                                audit.NewValue = employeeMaster?.EmployeeLocationList?.Find(x => x.LocationId == NewId)?.Location;
                                                audit.OldValue = employeeMaster?.EmployeeLocationList?.Find(x => x.LocationId == oldId)?.Location;
                                                audit.Field = "Location";
                                                break;
                                            case "CurrentWorkLocationId":
                                                audit.NewValue = employeeMaster?.EmployeeLocationList?.Find(x => x.LocationId == NewId)?.Location;
                                                audit.OldValue = employeeMaster?.EmployeeLocationList?.Find(x => x.LocationId == oldId)?.Location;
                                                audit.Field = "Current Work Location";
                                                break;
                                            case "CurrentWorkPlaceId":
                                                audit.NewValue = employeeMaster?.EmployeeWorkPlaceList?.Find(x => x.Key == NewId)?.Value;
                                                audit.OldValue = employeeMaster?.EmployeeWorkPlaceList?.Find(x => x.Key == oldId)?.Value;
                                                audit.Field = "Current Work Place";
                                                break;
                                            case "ReportingManagerId":
                                                audit.NewValue = employeeMaster?.ReportingManagerList?.Find(x => x.EmployeeId == NewId)?.EmployeeFullName;
                                                audit.OldValue = employeeMaster?.ReportingManagerList?.Find(x => x.EmployeeId == oldId)?.EmployeeFullName;
                                                audit.Field = "Reporting Manager";
                                                break;
                                            case "EmployeeCategoryId":
                                                audit.NewValue = employeeMaster?.EmployeeCategoryList?.Find(x => x.EmployeeCategoryId == NewId)?.EmployeeCategoryName;
                                                audit.OldValue = employeeMaster?.EmployeeCategoryList?.Find(x => x.EmployeeCategoryId == oldId)?.EmployeeCategoryName;
                                                audit.Field = "Employee Category";
                                                break;
                                            case "EmployeeTypeId":
                                                audit.NewValue = employeeMaster?.EmployeeTypeList?.Find(x => x.EmployeesTypeId == NewId)?.EmployeesType;
                                                audit.OldValue = employeeMaster?.EmployeeTypeList?.Find(x => x.EmployeesTypeId == oldId)?.EmployeesType;
                                                audit.Field = "Employee Type";
                                                break;
                                            case "ProbationStatusId":
                                                audit.NewValue = employeeMaster?.EmployeeProbationStatus?.Find(x => x.ProbationStatusId == NewId)?.ProbationStatusName;
                                                audit.OldValue = employeeMaster?.EmployeeProbationStatus?.Find(x => x.ProbationStatusId == oldId)?.ProbationStatusName;
                                                audit.Field = "Probation Status";
                                                break;
                                            case "Entity":
                                                audit.NewValue = employeeMaster?.Entity?.Find(x => x.Key == NewId)?.Value;
                                                audit.OldValue = employeeMaster?.Entity?.Find(x => x.Key == oldId)?.Value;
                                                audit.Field = "Entity";
                                                break;
                                            case "SourceOfHireId":
                                                audit.NewValue = employeeMaster?.SourceOfHireList?.Find(x => x.Key == NewId)?.Value;
                                                audit.OldValue = employeeMaster?.SourceOfHireList?.Find(x => x.Key == oldId)?.Value;
                                                audit.Field = "Source Of Hire";
                                                break;
                                            default:
                                                audit.NewValue = NewId.ToString();
                                                audit.OldValue = oldId.ToString();
                                                audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                                break;
                                        }
                                    }
                                    else if (((pi.PropertyType == (typeof(decimal?)))))
                                    {
                                        decimal NewId = destinationType.GetProperty(pi.Name)?.GetValue(newEmployee, null) == null ? 0 : (decimal)destinationType.GetProperty(pi.Name)?.GetValue(newEmployee, null);
                                        decimal oldId = sourceType.GetProperty(pi.Name)?.GetValue(oldEmployee, null) == null ? 0 : (decimal)sourceType.GetProperty(pi.Name)?.GetValue(oldEmployee, null);
                                        if (NewId.CompareTo(oldId) != 0)
                                        {
                                            audit.NewValue = NewId.ToString();
                                            audit.OldValue = oldId.ToString();
                                            audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                        }
                                        else
                                        {
                                            audit = null;
                                        }
                                    }
                                    else
                                    {
                                        audit.NewValue = destinationType.GetProperty(pi.Name)?.GetValue(newEmployee, null)?.ToString();
                                        audit.OldValue = sourceType.GetProperty(pi.Name)?.GetValue(oldEmployee, null)?.ToString();
                                        audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                    }
                                    if (audit != null && (audit?.OldValue != audit.NewValue))
                                    {
                                        await _auditRepository.AddAsync(audit);
                                    }

                                }
                            }
                            catch (Exception e)
                            {
                                LoggerManager.LoggingErrorTrack(e, "Employee Service", "Basic info Audit");
                            }

                        }
                        _auditRepository.SaveChangesAsync();
                    }

                }

                if (newData.EmployeesPersonalInfoDetail != null)
                {
                    EmployeesPersonalInfo newEmployeePersonalInfo = newData.EmployeesPersonalInfoDetail;
                    EmployeesPersonalInfo oldemployeesPersonalInfo = new EmployeesPersonalInfo();
                    if (oldValue.EmployeesPersonalInfoDetail != null) oldemployeesPersonalInfo = oldValue.EmployeesPersonalInfoDetail;

                    sourceType = oldemployeesPersonalInfo?.GetType();
                    destinationType = newEmployeePersonalInfo?.GetType();
                    //Personal Info Audit
                    if (sourceType == destinationType && newEmployeePersonalInfo != null)
                    {
                        PropertyInfo[] sourceProperties = sourceType?.GetProperties();
                        foreach (PropertyInfo pi in sourceProperties)
                        {
                            try
                            {
                                if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "PersonalInfoId" && pi.Name != "EmployeeId") && sourceType.GetProperty(pi.Name).GetValue(oldemployeesPersonalInfo, null)?.ToString()?.Trim() != destinationType.GetProperty(pi.Name).GetValue(newEmployeePersonalInfo, null)?.ToString()?.Trim())
                                {
                                    EmployeeAudit audit = new EmployeeAudit();
                                    audit.EmployeeId = newEmployeePersonalInfo.EmployeeId;
                                    audit.ChangeRequestID = ChangeRequestID;
                                    audit.CreatedOn = DateTime.UtcNow;
                                    audit.ActionType = newData.ActionType;
                                    audit.CreatedBy = newData.CreatedBy;
                                    if (((pi.PropertyType == (typeof(int?))) || (pi.PropertyType == (typeof(int)))))
                                    {

                                        int NewId = destinationType.GetProperty(pi.Name)?.GetValue(newEmployeePersonalInfo, null) == null ? 0 : (int)destinationType.GetProperty(pi.Name)?.GetValue(newEmployeePersonalInfo, null);
                                        int oldId = sourceType.GetProperty(pi.Name).GetValue(oldemployeesPersonalInfo, null) == null ? 0 : (int)sourceType.GetProperty(pi.Name).GetValue(oldemployeesPersonalInfo, null);
                                        switch (pi.Name)
                                        {
                                            case "BloodGroup":
                                                audit.NewValue = employeeMaster?.BloodGroupList?.Find(x => x.Key == NewId)?.Value;
                                                audit.OldValue = employeeMaster?.BloodGroupList?.Find(x => x.Key == oldId)?.Value;
                                                audit.Field = "Blood Group";
                                                break;
                                            case "PermanentState":
                                                audit.NewValue = employeeMaster?.StateList.Find(x => x.StateId == NewId)?.StateName;
                                                audit.OldValue = employeeMaster?.StateList.Find(x => x.StateId == oldId)?.StateName;
                                                audit.Field = "Permanent State";
                                                break;
                                            case "PermanentCountry":
                                                audit.NewValue = employeeMaster?.CountryList.Find(x => x.CountryId == NewId)?.CountryName;
                                                audit.OldValue = employeeMaster?.CountryList.Find(x => x.CountryId == oldId)?.CountryName;
                                                audit.Field = "Permanent Country";
                                                break;
                                            case "CommunicationState":
                                                audit.NewValue = employeeMaster?.StateList?.Find(x => x.StateId == NewId)?.StateName;
                                                audit.OldValue = employeeMaster?.StateList?.Find(x => x.StateId == oldId)?.StateName;
                                                audit.Field = "Communication State";
                                                break;
                                            case "CommunicationCountry":
                                                audit.NewValue = employeeMaster.CountryList?.Find(x => x.CountryId == NewId)?.CountryName;
                                                audit.OldValue = employeeMaster.CountryList?.Find(x => x.CountryId == oldId)?.CountryName;
                                                audit.Field = "Communication Country";
                                                break;
                                            default:
                                                audit.NewValue = NewId.ToString();
                                                audit.OldValue = oldId.ToString();
                                                audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                                break;
                                        }
                                        audit.CreatedOn = DateTime.UtcNow;
                                        audit.CreatedBy = newEmployeePersonalInfo.CreatedBy;

                                    }
                                    else if (((pi.PropertyType == (typeof(decimal?)))))
                                    {

                                        decimal NewId = destinationType.GetProperty(pi.Name)?.GetValue(newEmployeePersonalInfo, null) == null ? 0 : (decimal)destinationType.GetProperty(pi.Name)?.GetValue(newEmployeePersonalInfo, null);
                                        decimal oldId = sourceType.GetProperty(pi.Name)?.GetValue(oldemployeesPersonalInfo, null) == null ? 0 : (decimal)sourceType.GetProperty(pi.Name)?.GetValue(oldemployeesPersonalInfo, null);
                                        if (NewId.CompareTo(oldId) != 0)
                                        {
                                            audit.NewValue = NewId.ToString();
                                            audit.OldValue = oldId.ToString();
                                            audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                        }
                                        else
                                        {
                                            audit = null;
                                        }
                                    }
                                    else
                                    {
                                        if (pi.Name == "Nationality")
                                        {
                                            int NewId = destinationType.GetProperty(pi.Name)?.GetValue(newEmployeePersonalInfo, null) == null || destinationType.GetProperty(pi.Name)?.GetValue(newEmployeePersonalInfo, null) == "" ? 0 : (int)Convert.ToInt32(destinationType.GetProperty(pi.Name)?.GetValue(newEmployeePersonalInfo, null));
                                            int oldId = sourceType.GetProperty(pi.Name).GetValue(oldemployeesPersonalInfo, null) == null || sourceType.GetProperty(pi.Name).GetValue(oldemployeesPersonalInfo, null) == "" ? 0 : (int)Convert.ToInt32(sourceType.GetProperty(pi.Name).GetValue(oldemployeesPersonalInfo, null));
                                            audit.NewValue = employeeMaster?.NationalityList?.Find(x => x.NationalityId == NewId)?.NationalityName;
                                            audit.OldValue = employeeMaster?.NationalityList?.Find(x => x.NationalityId == oldId)?.NationalityName;
                                            audit.Field = "Nationality";
                                        }

                                        else
                                        {
                                            audit.NewValue = destinationType.GetProperty(pi.Name).GetValue(newEmployeePersonalInfo, null)?.ToString();
                                            audit.OldValue = sourceType.GetProperty(pi.Name).GetValue(oldemployeesPersonalInfo, null)?.ToString();
                                            audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                        }
                                    }

                                    if (audit != null && (audit?.OldValue != audit.NewValue))
                                    {
                                        await _auditRepository.AddAsync(audit);
                                    }
                                }
                            }
                            catch (Exception e)
                            {
                                LoggerManager.LoggingErrorTrack(e, "Employee Service", "personal info Audit");
                            }
                        }
                        _auditRepository.SaveChangesAsync();
                    }

                }

                if (newData?.WorkHistory?.Count > 0)
                {
                    foreach (WorkHistory newWorkHistory in newData.WorkHistory)
                    {

                        WorkHistory oldWorkHistory = oldValue?.WorkHistory?.Find(x => x.WorkHistoryId == newWorkHistory.WorkHistoryId);
                        if (oldWorkHistory == null) oldWorkHistory = new WorkHistory();
                        sourceType = oldWorkHistory?.GetType();
                        destinationType = newWorkHistory?.GetType();
                        if (sourceType == destinationType && newWorkHistory != null)
                        {
                            PropertyInfo[] sourceProperties = sourceType?.GetProperties();
                            foreach (PropertyInfo pi in sourceProperties)
                            {
                                try
                                {
                                    if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "WorkHistoryId" && pi.Name != "EmployeeId") && ((sourceType.GetProperty(pi.Name).GetValue(oldWorkHistory, null)?.ToString()?.Trim() != destinationType.GetProperty(pi.Name).GetValue(newWorkHistory, null)?.ToString()?.Trim())))
                                    {
                                        EmployeeAudit audit = new EmployeeAudit();
                                        audit.EmployeeId = newWorkHistory.EmployeeId;
                                        audit.ChangeRequestID = ChangeRequestID;
                                        audit.CreatedOn = DateTime.UtcNow;
                                        audit.ActionType = newData.ActionType;
                                        audit.CreatedBy = newData.CreatedBy;
                                        if (((pi.PropertyType == (typeof(int?))) || (pi.PropertyType == (typeof(int)))))
                                        {

                                            int NewId = destinationType.GetProperty(pi.Name)?.GetValue(newWorkHistory, null) == null ? 0 : (int)destinationType.GetProperty(pi.Name)?.GetValue(newWorkHistory, null);
                                            int oldId = sourceType.GetProperty(pi.Name).GetValue(oldWorkHistory, null) == null ? 0 : (int)sourceType.GetProperty(pi.Name).GetValue(oldWorkHistory, null);
                                            switch (pi.Name)
                                            {
                                                case "EmployeeTypeId":
                                                    audit.NewValue = employeeMaster?.EmployeeTypeList?.Find(x => x.EmployeesTypeId == NewId)?.EmployeesType;
                                                    audit.OldValue = employeeMaster?.EmployeeTypeList?.Find(x => x.EmployeesTypeId == oldId)?.EmployeesType;
                                                    audit.Field = "Employement Type";
                                                    break;
                                                default:
                                                    audit.NewValue = NewId.ToString();
                                                    audit.OldValue = oldId.ToString();
                                                    audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                                    break;

                                            }
                                        }
                                        else if (((pi.PropertyType == (typeof(decimal?)))))
                                        {
                                            decimal NewId = destinationType.GetProperty(pi.Name)?.GetValue(newWorkHistory, null) == null ? 0 : (decimal)destinationType.GetProperty(pi.Name)?.GetValue(newWorkHistory, null);
                                            decimal oldId = sourceType.GetProperty(pi.Name).GetValue(oldWorkHistory, null) == null ? 0 : (decimal)sourceType.GetProperty(pi.Name).GetValue(oldWorkHistory, null);
                                            if (NewId.CompareTo(oldId) != 0)
                                            {
                                                audit.NewValue = NewId.ToString();
                                                audit.OldValue = oldId.ToString();
                                                audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                            }
                                            else
                                            {
                                                audit = null;
                                            }
                                        }
                                        else
                                        {
                                            audit.NewValue = destinationType.GetProperty(pi.Name).GetValue(newWorkHistory, null)?.ToString();
                                            audit.OldValue = sourceType.GetProperty(pi.Name).GetValue(oldWorkHistory, null)?.ToString();
                                            audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);

                                        }
                                        if (audit != null && (audit?.OldValue != audit.NewValue))
                                        {
                                            await _auditRepository.AddAsync(audit);
                                        }
                                    }

                                }
                                catch (Exception e)
                                {
                                    LoggerManager.LoggingErrorTrack(e, "Employee Service", "Work info Audit");
                                }
                            }
                            _auditRepository.SaveChangesAsync();
                        }
                    }

                }

                if (newData?.EducationDetail?.Count > 0)
                {
                    foreach (EducationDetail neweducationDetail in newData.EducationDetail)
                    {
                        EducationDetail oldEducationDetail = oldValue?.EducationDetail?.Find(x => x.EducationDetailId == neweducationDetail.EducationDetailId);
                        if (oldEducationDetail == null) oldEducationDetail = new EducationDetail();
                        sourceType = oldEducationDetail?.GetType();
                        destinationType = neweducationDetail?.GetType();
                        if (sourceType == destinationType && neweducationDetail != null)
                        {
                            PropertyInfo[] sourceProperties = sourceType?.GetProperties();
                            foreach (PropertyInfo pi in sourceProperties)
                            {
                                try
                                {
                                    if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "EducationDetailId" && pi.Name != "EmployeeId") && ((sourceType.GetProperty(pi.Name).GetValue(oldEducationDetail, null)?.ToString()?.Trim() != destinationType.GetProperty(pi.Name).GetValue(neweducationDetail, null)?.ToString()?.Trim())) && destinationType.GetProperty(pi.Name).GetValue(neweducationDetail, null)?.ToString()?.Trim() != "")
                                    {
                                        EmployeeAudit audit = new EmployeeAudit();

                                        audit.EmployeeId = neweducationDetail.EmployeeId;
                                        audit.ChangeRequestID = ChangeRequestID;
                                        audit.CreatedOn = DateTime.UtcNow;
                                        audit.CreatedBy = newData.CreatedBy;
                                        audit.ActionType = newData.ActionType;

                                        if (((pi.PropertyType == (typeof(int?))) || (pi.PropertyType == (typeof(int)))))
                                        {
                                            int NewId = destinationType.GetProperty(pi.Name)?.GetValue(neweducationDetail, null) == null ? 0 : (int)destinationType.GetProperty(pi.Name)?.GetValue(neweducationDetail, null);
                                            int oldId = sourceType.GetProperty(pi.Name).GetValue(oldEducationDetail, null) == null ? 0 : (int)sourceType.GetProperty(pi.Name).GetValue(oldEducationDetail, null);

                                            switch (pi.Name)
                                            {
                                                case "EducationTypeId":
                                                    audit.NewValue = employeeMaster?.QualificationLIst?.Find(x => x.Key == NewId)?.Value;
                                                    audit.OldValue = employeeMaster?.QualificationLIst?.Find(x => x.Key == oldId)?.Value;
                                                    audit.Field = "Education Type";
                                                    break;
                                                case "BoardId":
                                                    audit.NewValue = employeeMaster?.Board?.Find(x => x.Key == NewId)?.Value;
                                                    audit.OldValue = employeeMaster?.Board?.Find(x => x.Key == oldId)?.Value;
                                                    audit.Field = "Board";
                                                    break;
                                                default:
                                                    audit.NewValue = NewId.ToString();
                                                    audit.OldValue = oldId.ToString();
                                                    audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                                    break;

                                            }
                                        }
                                        else if (((pi.PropertyType == (typeof(decimal?)))))
                                        {
                                            decimal NewId = destinationType.GetProperty(pi.Name)?.GetValue(neweducationDetail, null) == null ? 0 : (decimal)destinationType.GetProperty(pi.Name)?.GetValue(neweducationDetail, null);
                                            decimal oldId = sourceType.GetProperty(pi.Name).GetValue(oldEducationDetail, null) == null ? 0 : (decimal)sourceType.GetProperty(pi.Name).GetValue(oldEducationDetail, null);
                                            if (NewId.CompareTo(oldId) != 0)
                                            {
                                                audit.NewValue = NewId.ToString();
                                                audit.OldValue = oldId.ToString();
                                                audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                            }
                                            else
                                            {
                                                audit = null;
                                            }
                                        }
                                        else
                                        {
                                            audit.NewValue = destinationType.GetProperty(pi.Name).GetValue(neweducationDetail, null)?.ToString();
                                            audit.OldValue = sourceType.GetProperty(pi.Name).GetValue(oldEducationDetail, null)?.ToString();
                                            audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                            auditsList.Add(audit);
                                        }
                                        if (audit != null && (audit?.OldValue != audit.NewValue))
                                        {
                                            await _auditRepository.AddAsync(audit);
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    LoggerManager.LoggingErrorTrack(e, "Employee Service", "Education detail Audit");
                                }
                            }
                            _auditRepository.SaveChangesAsync();
                        }
                    }

                }

                if (newData?.CompensationDetail?.Count > 0)
                {
                    foreach (CompensationDetail newcompensationDetail in newData.CompensationDetail)
                    {
                        CompensationDetail oldCompensationDetail = oldValue?.CompensationDetail?.Find(x => x.CTCId == newcompensationDetail.CTCId);
                        if (oldCompensationDetail == null) oldCompensationDetail = new CompensationDetail();
                        sourceType = oldCompensationDetail?.GetType();
                        destinationType = newcompensationDetail?.GetType();
                        if (sourceType == destinationType && newcompensationDetail != null)
                        {
                            PropertyInfo[] sourceProperties = sourceType?.GetProperties();
                            foreach (PropertyInfo pi in sourceProperties)
                            {
                                try
                                {
                                    if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "CTCId" && pi.Name != "EmployeeID") && ((sourceType.GetProperty(pi.Name).GetValue(oldCompensationDetail, null)?.ToString()?.Trim() != destinationType.GetProperty(pi.Name).GetValue(newcompensationDetail, null)?.ToString()?.Trim())))
                                    {
                                        EmployeeAudit audit = new EmployeeAudit();
                                        audit.EmployeeId = newcompensationDetail.EmployeeID;
                                        audit.ActionType = newData.ActionType;
                                        audit.ChangeRequestID = ChangeRequestID;
                                        audit.CreatedOn = DateTime.UtcNow;
                                        audit.CreatedBy = newData.CreatedBy;
                                        if (((pi.PropertyType == (typeof(decimal?)))))
                                        {
                                            decimal NewId = destinationType.GetProperty(pi.Name)?.GetValue(newcompensationDetail, null) == null ? 0 : (decimal)destinationType.GetProperty(pi.Name)?.GetValue(newcompensationDetail, null);
                                            decimal oldId = sourceType.GetProperty(pi.Name).GetValue(oldCompensationDetail, null) == null ? 0 : (decimal)sourceType.GetProperty(pi.Name).GetValue(oldCompensationDetail, null);
                                            if (NewId.CompareTo(oldId) != 0)
                                            {
                                                audit.NewValue = NewId.ToString();
                                                audit.OldValue = oldId.ToString();
                                                audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                            }
                                            else
                                            {
                                                audit = null;
                                            }
                                        }
                                        else
                                        {
                                            audit.NewValue = destinationType.GetProperty(pi.Name).GetValue(newcompensationDetail, null)?.ToString();
                                            audit.OldValue = sourceType.GetProperty(pi.Name).GetValue(oldCompensationDetail, null)?.ToString();
                                            audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);

                                        }

                                        if (audit != null && (audit?.OldValue != audit.NewValue))
                                        {
                                            await _auditRepository.AddAsync(audit);
                                        }

                                    }
                                }
                                catch (Exception e)
                                {
                                    LoggerManager.LoggingErrorTrack(e, "Employee Service", "Compensation detail Audit");
                                }
                            }
                            _auditRepository.SaveChangesAsync();
                        }
                    }
                }
                if (newData?.EmployeesSkillsets?.Count > 0)
                {
                    var oldSkillsetList = oldValue?.EmployeesSkillsets?.Select(x => x.SkillsetId).ToList();
                    if (oldSkillsetList == null) oldSkillsetList = new List<int>();
                    var newSkillsetList = newData?.EmployeesSkillsets?.Select(x => x.SkillsetId).ToList();
                    if (newSkillsetList == null) newSkillsetList = new List<int>();
                    var removedSkills = oldSkillsetList?.Where(x => !newSkillsetList.Contains(x)).ToList();
                    if (removedSkills?.Count > 0)
                    {
                        foreach (int skills in removedSkills)
                        {
                            try
                            {
                                EmployeeAudit audit = new EmployeeAudit();
                                audit.EmployeeId = newData.EmployeesSkillsets[0].EmployeeId;
                                audit.ChangeRequestID = ChangeRequestID;
                                audit.ActionType = newData.ActionType;
                                audit.NewValue = null;
                                audit.OldValue = employeeMaster.SkillsetList?.Find(x => x.SkillsetId == skills)?.Skillset;
                                audit.Field = "Skill Removed";
                                audit.CreatedOn = DateTime.UtcNow;
                                audit.CreatedBy = newData.CreatedBy;
                                await _auditRepository.AddAsync(audit);
                                await _auditRepository.SaveChangesAsync();
                            }
                            catch (Exception e)
                            {
                                LoggerManager.LoggingErrorTrack(e, "Employee Service", "Remove Skill Audit");
                            }
                        }

                    }
                    var addedSkills = newSkillsetList.Where(x => !oldSkillsetList.Contains(x)).ToList();
                    if (addedSkills.Count > 0)
                    {
                        foreach (int skills in addedSkills)
                        {
                            try
                            {
                                EmployeeAudit audit = new EmployeeAudit();
                                audit.EmployeeId = newData.EmployeesSkillsets[0].EmployeeId;
                                audit.ChangeRequestID = ChangeRequestID;
                                audit.ActionType = newData.ActionType;
                                audit.NewValue = employeeMaster.SkillsetList.Find(x => x.SkillsetId == skills)?.Skillset; ;
                                audit.OldValue = null;
                                audit.Field = "Skill Added";
                                audit.CreatedOn = DateTime.UtcNow;
                                audit.CreatedBy = newData.CreatedBy;
                                await _auditRepository.AddAsync(audit);
                                await _auditRepository.SaveChangesAsync();
                            }
                            catch (Exception e)
                            {
                                LoggerManager.LoggingErrorTrack(e, "Employee Service", "Add Skill Audit");
                            }
                        }
                    }
                }
                if (newData?.EmployeeDependents?.Count > 0)
                {
                    List<EmployeeDependent> removedDependent = new List<EmployeeDependent>();
                    if (oldValue?.EmployeeDependents?.Count > 0)
                    {
                        List<int> depList = oldValue?.EmployeeDependents?.Select(x => (int)x.EmployeeDependentId).ToList();
                        if (depList == null) depList = new List<int>();
                        List<int> newDepList = newData?.EmployeeDependents?.Select(x => (int)x.EmployeeDependentId).ToList();
                        var exceptList = depList?.Where(p => !newDepList.Contains(p)).ToList();
                        removedDependent = oldValue?.EmployeeDependents?.Where(x => exceptList.Contains(x.EmployeeDependentId)).ToList();
                    }
                    if (removedDependent?.Count > 0)
                    {

                        foreach (EmployeeDependent dependent in removedDependent)
                        {
                            sourceType = dependent?.GetType();
                            PropertyInfo[] sourceProperties = sourceType?.GetProperties();
                            foreach (PropertyInfo pi in sourceProperties)
                            {
                                try
                                {
                                    if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "EmployeeDependentId" && pi.Name != "EmployeeID"))
                                    {
                                        EmployeeAudit audit = new EmployeeAudit();
                                        audit.EmployeeId = dependent.EmployeeID;
                                        audit.ChangeRequestID = ChangeRequestID;
                                        audit.CreatedOn = DateTime.UtcNow;
                                        audit.ActionType = newData.ActionType;
                                        audit.CreatedBy = newData.CreatedBy;
                                        if (pi.Name == "EmployeeRelationshipId")
                                        {
                                            int oldId = (int)sourceType.GetProperty(pi.Name)?.GetValue(dependent, null);
                                            audit.NewValue = null;
                                            audit.OldValue = employeeMaster.EmployeeRelationshipList.Find(x => x.EmployeeRelationshipId == oldId)?.EmployeeRelationshipName;
                                            audit.Field = "Employee Relationship";
                                        }

                                        else
                                        {
                                            audit.NewValue = null;
                                            audit.OldValue = sourceType.GetProperty(pi.Name).GetValue(dependent, null)?.ToString();
                                            audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                        }
                                        await _auditRepository.AddAsync(audit);
                                        await _auditRepository.SaveChangesAsync();
                                    }
                                }
                                catch (Exception e)
                                {
                                    LoggerManager.LoggingErrorTrack(e, "Employee Service", "Remove Dependent Audit");
                                }
                            }
                        }
                    }
                    List<EmployeeDependent> modifiedDependent = newData?.EmployeeDependents.Where(x => !removedDependent.Contains(x)).ToList();
                    foreach (EmployeeDependent newDependent in modifiedDependent)
                    {
                        EmployeeDependent oldemployeeDependent = oldValue?.EmployeeDependents?.Find(x => newDependent.EmployeeDependentId == x.EmployeeDependentId);
                        if (oldemployeeDependent == null) oldemployeeDependent = new EmployeeDependent();
                        sourceType = oldemployeeDependent?.GetType();
                        destinationType = newDependent?.GetType();
                        if (sourceType == destinationType && newDependent != null)
                        {
                            PropertyInfo[] sourceProperties = sourceType?.GetProperties();
                            foreach (PropertyInfo pi in sourceProperties)
                            {
                                try
                                {
                                    if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "EmployeeID" && pi.Name != "EmployeeDependentId") && ((sourceType.GetProperty(pi.Name).GetValue(oldemployeeDependent, null)?.ToString()?.Trim() != destinationType.GetProperty(pi.Name).GetValue(newDependent, null)?.ToString()?.Trim())))
                                    {
                                        EmployeeAudit audit = new EmployeeAudit();
                                        audit.EmployeeId = newDependent.EmployeeID;
                                        audit.ChangeRequestID = ChangeRequestID;
                                        audit.CreatedOn = DateTime.UtcNow;
                                        audit.CreatedBy = newData.CreatedBy;
                                        audit.ActionType = newData.ActionType;

                                        if (((pi.PropertyType == (typeof(int?))) || (pi.PropertyType == (typeof(int)))))
                                        {
                                            int NewId = destinationType.GetProperty(pi.Name)?.GetValue(newDependent, null) == null ? 0 : (int)destinationType.GetProperty(pi.Name)?.GetValue(newDependent, null);
                                            int oldId = sourceType.GetProperty(pi.Name).GetValue(oldemployeeDependent, null) == null ? 0 : (int)sourceType.GetProperty(pi.Name).GetValue(oldemployeeDependent, null);

                                            switch (pi.Name)
                                            {
                                                case "EmployeeRelationshipId":
                                                    audit.NewValue = employeeMaster.EmployeeRelationshipList.Find(x => x.EmployeeRelationshipId == NewId)?.EmployeeRelationshipName;
                                                    audit.OldValue = employeeMaster.EmployeeRelationshipList.Find(x => x.EmployeeRelationshipId == oldId)?.EmployeeRelationshipName;
                                                    audit.Field = "Employee Relationship";
                                                    break;
                                                default:
                                                    audit.NewValue = NewId.ToString();
                                                    audit.OldValue = oldId.ToString();
                                                    audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                                    break;
                                            }
                                        }
                                        else
                                        {
                                            audit.NewValue = destinationType.GetProperty(pi.Name).GetValue(newDependent, null)?.ToString();
                                            audit.OldValue = sourceType.GetProperty(pi.Name).GetValue(oldemployeeDependent, null)?.ToString();
                                            audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                        }
                                        await _auditRepository.AddAsync(audit);
                                        await _auditRepository.SaveChangesAsync();
                                    }
                                }
                                catch (Exception e)
                                {
                                    LoggerManager.LoggingErrorTrack(e, "Employee Service", "Added Dependent Audit");
                                }
                            }
                        }
                    }
                }
                if (newData?.EmployeeShiftDetails?.Count > 0)
                {
                    List<EmployeeShiftDetails> removedDependent = new List<EmployeeShiftDetails>();
                    if (oldValue?.EmployeeDependents?.Count > 0)
                    {
                        List<int> depList = oldValue?.EmployeeShiftDetails.Select(x => (x.EmployeeShiftDetailsId == null ? 0 : (int)x.EmployeeShiftDetailsId)).ToList();
                        if (depList == null) depList = new List<int>();
                        List<int> newDepList = newData?.EmployeeShiftDetails.Select(x => x.EmployeeShiftDetailsId == null ? 0 : (int)x.EmployeeShiftDetailsId).ToList();
                        var exceptList = depList?.Where(p => !newDepList.Contains(p)).ToList();
                        removedDependent = oldValue?.EmployeeShiftDetails?.Where(x => exceptList.Contains(x.EmployeeShiftDetailsId == null ? 0 : (int)x.EmployeeShiftDetailsId)).ToList();
                    }
                    if (removedDependent?.Count > 0)
                    {

                        foreach (EmployeeShiftDetails shift in removedDependent)
                        {
                            sourceType = shift?.GetType();
                            PropertyInfo[] sourceProperties = sourceType?.GetProperties();
                            foreach (PropertyInfo pi in sourceProperties)
                            {
                                try
                                {
                                    if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "EmployeeShiftDetailsId" && pi.Name != "EmployeeID"))
                                    {
                                        EmployeeAudit audit = new EmployeeAudit();
                                        audit.EmployeeId = shift.EmployeeID;
                                        audit.ChangeRequestID = ChangeRequestID;
                                        audit.CreatedOn = DateTime.UtcNow;
                                        audit.ActionType = newData.ActionType;
                                        audit.CreatedBy = newData.CreatedBy;
                                        audit.NewValue = null;
                                        audit.OldValue = sourceType.GetProperty(pi.Name).GetValue(shift, null)?.ToString();
                                        audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                        await _auditRepository.AddAsync(audit);
                                        await _auditRepository.SaveChangesAsync();
                                    }
                                }
                                catch (Exception e)
                                {
                                    LoggerManager.LoggingErrorTrack(e, "Employee Service", "Remove shift Audit");
                                }
                            }
                        }
                    }
                    List<EmployeeShiftDetails> modifiedShifts = newData?.EmployeeShiftDetails.Where(x => !removedDependent.Contains(x)).ToList();
                    foreach (EmployeeShiftDetails newshiftDetail in modifiedShifts)
                    {
                        EmployeeShiftDetails oldemployeeShift = oldValue.EmployeeShiftDetails.Find(x => newshiftDetail.EmployeeShiftDetailsId == x.EmployeeShiftDetailsId);
                        if (oldemployeeShift == null) oldemployeeShift = new EmployeeShiftDetails();

                        sourceType = oldemployeeShift?.GetType();
                        destinationType = newshiftDetail?.GetType();
                        if (sourceType == destinationType && newshiftDetail != null)
                        {
                            PropertyInfo[] sourceProperties = sourceType?.GetProperties();
                            foreach (PropertyInfo pi in sourceProperties)
                            {
                                try
                                {
                                    if ((pi.Name != "ModifiedOn" && pi.Name != "ModifiedBy" && pi.Name != "CreatedOn" && pi.Name != "CreatedBy" && pi.Name != "EmployeeID" && pi.Name != "EmployeeShiftDetailsId") && ((sourceType.GetProperty(pi.Name).GetValue(oldemployeeShift, null)?.ToString()?.Trim() != destinationType.GetProperty(pi.Name).GetValue(newshiftDetail, null)?.ToString()?.Trim())))
                                    {
                                        EmployeeAudit audit = new EmployeeAudit();
                                        audit.EmployeeId = newshiftDetail.EmployeeID;
                                        audit.ChangeRequestID = ChangeRequestID;
                                        audit.CreatedOn = DateTime.UtcNow;
                                        audit.CreatedBy = newData.CreatedBy;
                                        audit.ActionType = newData.ActionType;
                                        audit.NewValue = destinationType.GetProperty(pi.Name).GetValue(newshiftDetail, null)?.ToString();
                                        audit.OldValue = sourceType.GetProperty(pi.Name).GetValue(oldemployeeShift, null)?.ToString();
                                        audit.Field = sourceType.GetProperty(pi.Name).ToString().Substring(sourceType.GetProperty(pi.Name).ToString().IndexOf(" ") + 1);
                                        await _auditRepository.AddAsync(audit);
                                        await _auditRepository.SaveChangesAsync();
                                    }
                                }
                                catch (Exception e)
                                {
                                    LoggerManager.LoggingErrorTrack(e, "Employee Service", "Add shift Audit");
                                }
                            }
                        }
                    }
                }
                if (newData?.EmployeeSpecialAbility?.Count > 0)
                {
                    var oldSpecialAbilityList = oldValue?.EmployeeSpecialAbility.Select(x => x.SpecialAbilityId).ToList();
                    var newSpecialAbilityList = newData?.EmployeeSpecialAbility.Select(x => x.SpecialAbilityId).ToList();
                    var removedSpecialAbility = oldSpecialAbilityList?.Where(x => !newSpecialAbilityList.Contains(x)).ToList();
                    if (removedSpecialAbility?.Count > 0)
                    {
                        foreach (int specialAbility in removedSpecialAbility)
                        {
                            try
                            {
                                EmployeeAudit audit = new EmployeeAudit();
                                audit.EmployeeId = newData.EmployeeSpecialAbility[0].EmployeeId;
                                audit.ChangeRequestID = ChangeRequestID;
                                audit.ActionType = newData.ActionType;
                                audit.NewValue = null;
                                audit.OldValue = employeeMaster.SpecialAbilityList.Find(x => x.Key == specialAbility)?.Value;
                                audit.Field = "Special Ability Removed";
                                audit.CreatedOn = DateTime.UtcNow;
                                audit.CreatedBy = newData.CreatedBy;
                                await _auditRepository.AddAsync(audit);
                                await _auditRepository.SaveChangesAsync();
                            }
                            catch (Exception e)
                            {
                                LoggerManager.LoggingErrorTrack(e, "Employee Service", "Remove special ability Audit");
                            }
                        }

                    }
                    var addedSpecialAbility = newSpecialAbilityList.Where(x => !oldSpecialAbilityList.Contains(x)).ToList();
                    if (addedSpecialAbility?.Count > 0)
                    {
                        foreach (int specialAbility in addedSpecialAbility)
                        {
                            try
                            {
                                EmployeeAudit audit = new EmployeeAudit();
                                audit.EmployeeId = newData.EmployeeSpecialAbility[0].EmployeeId;
                                audit.ChangeRequestID = ChangeRequestID;
                                audit.ActionType = newData.ActionType;
                                audit.NewValue = employeeMaster.SpecialAbilityList.Find(x => x.Key == specialAbility)?.Value;
                                audit.OldValue = null;
                                audit.Field = "Special Ability Added";
                                audit.CreatedOn = DateTime.UtcNow;
                                audit.CreatedBy = newData.CreatedBy;
                                await _auditRepository.AddAsync(audit);
                                await _auditRepository.SaveChangesAsync();
                            }
                            catch (Exception e)
                            {
                                LoggerManager.LoggingErrorTrack(e, "Employee Service", "Add special ability Audit");
                            }
                        }
                    }
                }

            }
            catch (Exception e)
            {
                LoggerManager.LoggingErrorTrack(e, "Employee Service", "Audit ");
            }
            return true;
        }
        #endregion

        #region Get employee list by Compensation Detail 
        public List<CompensationDetail> GetEmployeeCompensationDetail(int employeeId)
        {
            return _compensationDetailRepository.GetCompensationDetailByEmployeeId(employeeId);
        }
        #endregion

        #region
        public EmployeeWorkAndEducationDetailView GetWorkHistoryViewDetails(int employeeId)
        {
            Byte[] bytes;
            EmployeeWorkAndEducationDetailView workInfo = new EmployeeWorkAndEducationDetailView();
            workInfo.WorkHistoriesList = _workHistoryRepository?.GetWorkHistoryViewByEmployeeId(employeeId);
            foreach (WorkHistoryView workHistory in workInfo.WorkHistoriesList)
            {
                if (workHistory.paySlip != null)
                {
                    bytes = File.Exists(workHistory.paySlip.Path) ? File.ReadAllBytes(workHistory.paySlip.Path) : null;
                    workHistory.paySlip.DocumentAsBase64 = bytes != null ? Convert.ToBase64String(bytes) : "";
                    workHistory.paySlip.DocumentSize = bytes != null ? bytes.Length : 0;
                }
                if (workHistory.serviceLetter != null)
                {
                    bytes = File.Exists(workHistory.serviceLetter.Path) ? File.ReadAllBytes(workHistory.serviceLetter.Path) : null;
                    workHistory.serviceLetter.DocumentAsBase64 = bytes != null ? Convert.ToBase64String(bytes) : "";
                    workHistory.serviceLetter.DocumentSize = bytes != null ? bytes.Length : 0;
                }
                if (workHistory.OfferLetter != null)
                {
                    bytes = File.Exists(workHistory.OfferLetter.Path) ? File.ReadAllBytes(workHistory.OfferLetter.Path) : null;
                    workHistory.OfferLetter.DocumentAsBase64 = bytes != null ? Convert.ToBase64String(bytes) : "";
                    workHistory.OfferLetter.DocumentSize = bytes != null ? bytes.Length : 0;
                }
            }
            return workInfo;
        }
        #endregion

        #region
        public EmployeeWorkAndEducationDetailView GetEmployeeEducationDetail(int employeeId)
        {
            Byte[] bytes;
            EmployeeWorkAndEducationDetailView educationDetail = new EmployeeWorkAndEducationDetailView();
            educationDetail.EducationDetailsList = _educationDetailRepository?.GetEducationDetailViewByEmployeeId(employeeId);
            educationDetail.employeeRequestDetails = _employeeRequestRepository.GetPendingRequestByEmployeeIdAndRequestCat(employeeId, "Certification & Skills");
            foreach (EducationDetailView education in educationDetail.EducationDetailsList)
            {
                if (education.Marksheet != null)
                {
                    bytes = File.Exists(education.Marksheet.Path) ? File.ReadAllBytes(education.Marksheet.Path) : null;
                    education.Marksheet.DocumentAsBase64 = bytes != null ? Convert.ToBase64String(bytes) : "";
                    education.Marksheet.DocumentSize = bytes != null ? bytes.Length : 0;
                }
                if (education.Certificate != null)
                {
                    bytes = File.Exists(education.Certificate.Path) ? File.ReadAllBytes(education.Certificate.Path) : null;
                    education.Certificate.DocumentAsBase64 = bytes != null ? Convert.ToBase64String(bytes) : "";
                    education.Certificate.DocumentSize = bytes != null ? bytes.Length : 0;
                }
            }
            return educationDetail;
        }
        #endregion


        #region Get employee Personal information
        public EmployeePersonalInfoView GetEmployeesPersonalInformation(int employeeId)
        {
            EmployeePersonalInfoView personalInfo = new EmployeePersonalInfoView();
            EmployeesPersonalInfo employeesPersonal = _employeesPersonalInfoRepository.GetEmployeePersonalIdByEmployeeID(employeeId);
            if (employeesPersonal != null)
            {
                personalInfo.PersonalInfoId = employeesPersonal.PersonalInfoId;
                personalInfo.EmployeeId = employeeId;
                personalInfo.HighestQualification = employeesPersonal.HighestQualification;
                personalInfo.PersonalMobileNumber = employeesPersonal.PersonalMobileNumber;
                personalInfo.OtherEmail = employeesPersonal.OtherEmail;
                personalInfo.BloodGroup = employeesPersonal.BloodGroup;
                personalInfo.PermanentAddressLine1 = employeesPersonal.PermanentAddressLine1;
                personalInfo.PermanentAddressLine2 = employeesPersonal.PermanentAddressLine2;
                personalInfo.PermanentCity = employeesPersonal.PermanentCity;
                personalInfo.PermanentState = employeesPersonal.PermanentState;
                personalInfo.PermanentCountry = employeesPersonal.PermanentCountry;
                personalInfo.PermanentAddressZip = employeesPersonal.PermanentAddressZip;
                personalInfo.SpouseName = employeesPersonal.SpouseName;
                personalInfo.FathersName = employeesPersonal.FathersName;
                personalInfo.EmergencyMobileNumber = employeesPersonal.EmergencyMobileNumber;
                personalInfo.Nationality = employeesPersonal.Nationality;
                personalInfo.CommunicationAddressLine1 = employeesPersonal.CommunicationAddressLine1;
                personalInfo.CommunicationAddressLine2 = employeesPersonal.CommunicationAddressLine2;
                personalInfo.CommunicationCity = employeesPersonal.CommunicationCity;
                personalInfo.CommunicationState = employeesPersonal.CommunicationState;
                personalInfo.CommunicationCountry = employeesPersonal.CommunicationCountry;
                personalInfo.CommunicationAddressZip = employeesPersonal.CommunicationAddressZip;
                personalInfo.PANNumber = employeesPersonal.PANNumber;
                personalInfo.UANNumber = employeesPersonal.UANNumber;
                personalInfo.DrivingLicense = employeesPersonal.DrivingLicense;
                personalInfo.AadhaarCardNumber = employeesPersonal.AadhaarCardNumber;
                personalInfo.PassportNumber = employeesPersonal.PassportNumber;
                personalInfo.EmergencyContactName = employeesPersonal.EmergencyContactName;
                personalInfo.EmergencyContactRelation = employeesPersonal.EmergencyContactRelation;
                personalInfo.EmergencyMobileNumber = employeesPersonal.EmergencyMobileNumber;
                personalInfo.ReferenceContactName = employeesPersonal.ReferenceContactName;
                personalInfo.ReferenceEmailId = employeesPersonal.ReferenceEmailId;
                personalInfo.ReferenceMobileNumber = employeesPersonal.ReferenceMobileNumber;
                personalInfo.IsJoiningBonus = employeesPersonal.IsJoiningBonus;
                personalInfo.JoiningBonusAmmount = employeesPersonal.JoiningBonusAmmount;
                personalInfo.JoiningBonusCondition = employeesPersonal.JoiningBonusCondition;
                personalInfo.IFSCCode = employeesPersonal.IFSCCode;
                personalInfo.BankName = employeesPersonal.BankName;
                personalInfo.AccountHolderName = employeesPersonal.AccountHolderName;
                personalInfo.AccountNumber = employeesPersonal.AccountNumber;
                personalInfo.CreatedBy = employeesPersonal.CreatedBy;
                personalInfo.CreatedOn = employeesPersonal.CreatedOn;
                personalInfo.ModifiedOn = employeesPersonal.ModifiedOn;
                personalInfo.ModifiedBy = employeesPersonal.ModifiedBy;
            }
            return personalInfo;
        }
        #endregion
        #region Get all employee list for view
        /// <summary>
        /// Get all employee list
        /// </summary>
        /// <returns></returns>
        public EmployeeListDetails GetEmployeesListForGrid(PaginationView pagination)
        {
            EmployeeListDetails employeeList = new EmployeeListDetails();
            employeeList.EmployeeDetailLists = _employeeRepository.GetEmployeeListForGrid(pagination);
            return employeeList;
        }
        #endregion

        public List<EmployeeDetailListView> GetEmployeeListForRequestedDocumentGrid(List<int> employeeIds)
        {
            return _employeeRepository.GetEmployeeListForRequestedDocumentGrid(employeeIds);
        }

        #region Get all employee list for orgchart
        /// <summary>
        /// Get all employee list
        /// </summary>
        /// <returns></returns>
        public EmployeeListDetailsForOrgChart GetEmployeesListForOrgChart(PaginationView pagination)
        {
            EmployeeListDetailsForOrgChart employeeList = new EmployeeListDetailsForOrgChart();
            employeeList.EmployeeDetailLists = _employeeRepository.GetEmployeesListForOrgChart(pagination);
            return employeeList;
        }
        #endregion
        #region Get Employee Basic Information By Id
        /// <summary>
        /// Get Employee Basic Information By Id
        /// </summary>
        /// <returns></returns>
        public async Task<EmployeeBasicInfoView> GetEmployeeBasicInformationById(int employeeId)
        {
            EmployeeBasicInfoView employeeDetails = new();
            employeeDetails = _employeeRepository.GetEmployeeBasicInfoById(employeeId);
            employeeDetails.EmployeesPersonalInfo = GetEmployeesPersonalInformation(employeeId);
            employeeDetails.EmployeesPersonalInfo.CommunicationStateName = _stateRepository.GetStateNameById(employeeDetails?.EmployeesPersonalInfo?.CommunicationState == null ? 0 : (int)employeeDetails?.EmployeesPersonalInfo?.CommunicationState);
            employeeDetails.EmployeesPersonalInfo.CommunicationCountryName = _countryRepository.GetCountryNameById(employeeDetails?.EmployeesPersonalInfo?.CommunicationCountry == null ? 0 : (int)employeeDetails?.EmployeesPersonalInfo?.CommunicationCountry);
            employeeDetails.EmployeesPersonalInfo.PermanentStateName = _stateRepository.GetStateNameById(employeeDetails?.EmployeesPersonalInfo?.PermanentState == null ? 0 : (int)employeeDetails?.EmployeesPersonalInfo?.PermanentState);
            employeeDetails.EmployeesPersonalInfo.PermanentCountryName = _countryRepository.GetCountryNameById(employeeDetails?.EmployeesPersonalInfo?.PermanentCountry == null ? 0 : (int)employeeDetails?.EmployeesPersonalInfo?.PermanentCountry);
            //employeeDetails.EmployeesPersonalInfo.NationalityName = _countryRepository.GetCountryNameById(string.IsNullOrEmpty(employeeDetails?.EmployeesPersonalInfo?.Nationality) ? 0 : (Convert.ToInt32(employeeDetails?.EmployeesPersonalInfo?.Nationality)));
            employeeDetails.EmployeesPersonalInfo.NationalityName = _employeeNationalityRepository.GetNationalityNameById(string.IsNullOrEmpty(employeeDetails?.EmployeesPersonalInfo?.Nationality) ? 0 : (Convert.ToInt32(employeeDetails?.EmployeesPersonalInfo?.Nationality)));
            employeeDetails.EmployeesPersonalInfo.BloodGroupName = _appConstantRepository.GetAppconstantValueById(employeeDetails.EmployeesPersonalInfo?.BloodGroup == null ? 0 : employeeDetails.EmployeesPersonalInfo.BloodGroup);
            employeeDetails.AddressProof = await _employeeDocumentRepository.GetEmployeeDocumentDetailBySourceId(employeeId);
            employeeDetails.CommunicationAddressProof = _employeeDocumentRepository.GetAddressProof(employeeId, "communicationAddressProof");
            employeeDetails.PermanentAddressProof = _employeeDocumentRepository.GetAddressProof(employeeId, "permanentAddressProof");
            employeeDetails.MaritalStatusProof = _employeeDocumentRepository.GetAddressProof(employeeId, "maritalstatusProof");
            return employeeDetails;
        }
        #endregion

        # region Get Employee Compensation Detail For View
        /// <summary>
        /// Get Employee Compensation Detail For View
        /// </summary>
        /// <returns></returns>
        public List<CompensationDetailView> GetEmployeeCompensationDetailForView(EmployeeCompensationCompareView compensationCompareView)
        {
            List<CompensationDetailView> compensationDetails = new List<CompensationDetailView>();
            compensationDetails = _compensationDetailRepository.GetCompensationDetailViewsByEmployeeId(compensationCompareView);
            return compensationDetails;
        }
        #endregion

        #region Get all employee list for Birthday
        /// <summary>
        ///Get all employee list for Birthday
        /// </summary>
        /// <returns></returns>
        public List<EmployeeDetailListView> GetListOfEmployeeBirthDay(DateTime fromDate, DateTime todate)
        {
            return _employeeRepository.GetListOfEmployeeBirthDay(fromDate, todate);
        }
        #endregion


        #region InsertOrUpdate Designation
        public async Task<int> InsertOrUpdateDesignation(Designation pDesignation)
        {
            int designationId = 0;
            try
            {
                Designation designation = new();
                if (pDesignation.DesignationId != 0)
                {
                    designation = _designationRepository.GetDesignationDetailByDesignationId(pDesignation.DesignationId);
                }
                if (designation == null)
                {
                    return designationId = -1;
                }
                designation.DesignationName = pDesignation.DesignationName;
                designation.DesignationShortName = pDesignation.DesignationShortName;
                designation.DesignationDescription = pDesignation.DesignationDescription;


                if (designation.DesignationId != 0)
                {
                    designation.ModifiedBy = pDesignation.ModifiedBy;
                    designation.ModifiedOn = DateTime.UtcNow;
                    _designationRepository.Update(designation);
                }
                else
                {
                    designation.CreatedBy = pDesignation.CreatedBy;
                    designation.CreatedOn = DateTime.UtcNow;
                    await _designationRepository.AddAsync(designation);
                }
                await _designationRepository.SaveChangesAsync();
                designationId = designation.DesignationId;
            }
            catch (Exception)
            {
                throw;
            }
            return designationId;
        }
        #endregion


        #region Get Designation List and count
        public List<DesignationDetail> GetDesignationListAndCount()
        {
            return _designationRepository.GetDesignationListAndCount();
        }
        #endregion
        #region Get employee List by Designation id
        public List<DesignationEmployeeDetails> GetEmployeeListByDesignationId(int designationID)
        {
            return _designationRepository.GetEmployeeListByDesignationId(designationID);

        }
        #endregion

        //#region Get Department List and count
        //public List<DepartmentDetailList> GetDepartmentListAndCount()
        //{
        //    return _departmentRepository.GetDepartmentListAndCount();
        //}
        //#endregion

        #region Get employee List by Department id
        public List<DepartmentEmployeeList> GetEmployeeListByDepartmentId(int departmentID)
        {
            return _departmentRepository.GetEmployeeListByDepartmentId(departmentID);
        }
        #endregion

        #region InsertOrUpdate Department
        public async Task<int> InsertOrUpdateDepartment(Department pDepartment)
        {
            int departmentId = 0;
            try
            {
                Department department = new();
                if (pDepartment.DepartmentId != 0)
                {
                    department = _departmentRepository.GetDepartmentById(pDepartment.DepartmentId);
                }
                if (department == null)
                {
                    return departmentId = -1;
                }
                department.DepartmentName = pDepartment.DepartmentName;
                department.DepartmentShortName = pDepartment.DepartmentShortName;
                department.DepartmentDescription = pDepartment.DepartmentDescription;
                department.IsEnableBUAccountable = pDepartment.IsEnableBUAccountable;
                department.ParentDepartmentId = pDepartment.ParentDepartmentId;
                department.DepartmentHeadEmployeeId = pDepartment.DepartmentHeadEmployeeId;

                if (department.DepartmentId != 0)
                {
                    department.ModifiedBy = pDepartment.ModifiedBy;
                    department.ModifiedOn = DateTime.UtcNow;
                    _departmentRepository.Update(department);
                }
                else
                {
                    department.CreatedBy = pDepartment.CreatedBy;
                    department.CreatedOn = DateTime.UtcNow;
                    await _departmentRepository.AddAsync(department);
                }
                await _departmentRepository.SaveChangesAsync();
                departmentId = department.DepartmentId;
            }
            catch (Exception)
            {
                throw;
            }
            return departmentId;
        }
        #endregion

        #region Get department master data
        public DepartmentMasterData GetDepartmentMasterData(string employeeIdFormat)
        {
            DepartmentMasterData departmentMasterData = new()
            {
                // EmployeeCategoryList = _employeeCategoryRepository.GetEmployeeCategoryList(),

                DepartmentDetailLists = _departmentRepository.GetDepartmentListAndCount(),
                ParentDepartmentId = _appConstantRepository.GetAppConstantByType("ParentDepartmentId"),
                DepartmentHeadEmployee = _departmentRepository.GetDepartmentHeadEmployees(),
            };
            return departmentMasterData;
        }
        #endregion

        #region
        public List<EmployeeDetailListView> employeeFilterData(PaginationView paginationView)
        {
            return _employeeRepository.employeeFilterData(paginationView);
        }
        #endregion

        #region Get employee List by Skillset id
        public List<SkillsetEmployeeDetails> GetEmployeeListBySkillsetId(EmployeeSkillsetCategoryInput skillsetInput)
        {
            return _skillsetRepository.GetEmployeeListBySkillsetId(skillsetInput);

        }
        #endregion

        #region InsertOrUpdate Skillset
        public async Task<int> InsertOrUpdateSkillset(Skillsets pSkillsets)
        {
            int skillsetId = 0;
            try
            {
                string oldSkillValue = "";
                string newSkillValue = "";
                int? oldCategoryValue;
                int? newCategoryValue;

                Skillsets skillset = new();
                if (pSkillsets.SkillsetId != 0)
                {
                    skillset = _skillsetRepository.GetSkillsetById(pSkillsets.SkillsetId);
                }
                if (skillset == null)
                {
                    return skillsetId = -1;
                }
                oldSkillValue = skillset.Skillset;
                oldCategoryValue = skillset.SkillsetCategoryId;
                newSkillValue = pSkillsets.Skillset;
                newCategoryValue = pSkillsets.SkillsetCategoryId;
                skillset.Skillset = pSkillsets.Skillset;
                skillset.SkillsetCategoryId = pSkillsets.SkillsetCategoryId;

                if (skillset.SkillsetId != 0)
                {
                    skillset.ModifiedBy = pSkillsets.ModifiedBy;
                    skillset.ModifiedOn = DateTime.UtcNow;
                    _skillsetRepository.Update(skillset);
                }
                else
                {
                    oldSkillValue = "NA";
                    oldCategoryValue = 0;
                    skillset.CreatedBy = pSkillsets.CreatedBy;
                    skillset.CreatedOn = DateTime.UtcNow;
                    await _skillsetRepository.AddAsync(skillset);
                }
                await _skillsetRepository.SaveChangesAsync();
                skillsetId = skillset.SkillsetId;
                if (oldSkillValue != newSkillValue)
                {
                    var result = await AddSkillsetHistory(skillsetId, oldSkillValue, newSkillValue, "Skill Change", pSkillsets.SkillsetId == 0 ? pSkillsets.CreatedBy : null, pSkillsets.SkillsetId > 0 ? pSkillsets.ModifiedBy : null);
                }
                if (oldCategoryValue != newCategoryValue)
                {

                    var result = await AddSkillsetHistory(skillsetId, _skillsetRepository.GetSkillsetCategoryNameById(oldCategoryValue),
                        _skillsetRepository.GetSkillsetCategoryNameById(newCategoryValue), "Category Change", pSkillsets.SkillsetId == 0 ? pSkillsets.CreatedBy : null, pSkillsets.SkillsetId > 0 ? pSkillsets.ModifiedBy : null);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return skillsetId;
        }
        #endregion

        public async Task<bool> AddSkillsetHistory(int skillsetId, string oldValue, string newValue, string category, int? createdUserId, int? modifiedUserId)
        {
            SkillsetHistory skillsethistory = new();
            skillsethistory.SkillsetId = skillsetId;
            skillsethistory.OldValue = oldValue == null ? "NA" : oldValue;
            skillsethistory.NewValue = newValue == null ? "NA" : newValue;
            skillsethistory.Category = category;
            //skillsethistory.ModifiedBy = null;
            //skillsethistory.ModifiedOn =null ;
            skillsethistory.CreatedBy = createdUserId == null ? modifiedUserId : createdUserId;
            skillsethistory.CreatedOn = DateTime.UtcNow;
            await _skillsetHistoryRepository.AddAsync(skillsethistory);
            await _skillsetHistoryRepository.SaveChangesAsync();
            return true;
        }

        #region Get SkillsetDetails
        public SkillsetDetails GetSkillsetDetails()
        {
            SkillsetDetails skillsetDetails = new()
            {
                GetSkillsetCategory = _skillsetRepository.GetSkillsetCategoryNames(),
                SkillsetCategoryName = _skillsetRepository.GetSkillsetNames(),

            };
            return skillsetDetails;
        }
        #endregion

        #region Get SkillsetHistory By SkillsetId
        public SkillsetHistoryView GetSkillsetHistoryBySkillsetId(int skillsetId)
        {
            return _skillsetHistoryRepository.GetSkillsetHistoryBySkillsetId(skillsetId);
        }
        #endregion

        //#region Get SkillsetHistory By SkillsetId
        //public SkillsetHistoryView GetSkillsetHistoryBySkillsetId(int skillsetId)
        //{
        //    return _skillsetHistoryRepository.GetSkillsetHistoryBySkillsetId(skillsetId);
        //}
        //#endregion

        #region Get all employee list for WorkAnniversaries
        public List<EmployeeWorkAnniversaries> GetListOfEmployeeWorkAnniversaries(DateTime fromDate, DateTime todate)
        {
            return _employeeRepository.GetListOfEmployeeWorkAnniversaries(fromDate, todate);
        }
        #endregion

        #region Get Absent Notification Employee List
        public List<EmployeeList> GetAbsentNotificationEmployeeList()
        {
            return _employeeRepository.GetAbsentNotificationEmployeeList();
        }
        #endregion

        #region Get employee List by Skillset id
        public List<EmployeeDetails> GetEmployeeListBySkillsetIdForDownload(EmployeeSkillsetCategoryInput skillsetInput)
        {
            return _skillsetRepository.GetEmployeeListBySkillsetIdForDownload(skillsetInput);

        }
        #endregion

        #region Orgnization Chart
        public EmployeeorganizationcharView GetorganizationchartDetails(int employeeId)
        {
            return _employeeRepository.GetorganizationchartDetails(employeeId);
        }
        #endregion

        #region  
        public int GetEmployeesListCount(PaginationView paginationView)
        {
            if (paginationView.EmployeeFilter == null)
            {
                return _employeeRepository.GetTotalRecordCount();
            }
            else
            {
                return _employeeRepository.employeeFilterDataCount(paginationView);
            }
        }
        #endregion

        #region Add Designation History 
        public async Task<bool> AddDesignationHistory(int? employeeId, int? designationId, DateTime? fromDate, int? createdUserId)
        {
            EmployeesDesignationHistory employeesDesignationHistory = new();
            EmployeesDesignationHistory designationHistory = _employeeDesignationHistoryRepository.CheckDesignationHistoryByEmployeeId(employeeId, designationId, fromDate);
            if (designationHistory == null)
            {
                employeesDesignationHistory.EmployeeId = employeeId;
                employeesDesignationHistory.DesignationId = designationId;
                employeesDesignationHistory.EffiectiveFromDate = fromDate;
                employeesDesignationHistory.CreatedBy = createdUserId;
                employeesDesignationHistory.CreatedOn = DateTime.UtcNow;
                await _employeeDesignationHistoryRepository.AddAsync(employeesDesignationHistory);
                await _employeeDesignationHistoryRepository.SaveChangesAsync();

                if (employeesDesignationHistory?.DesignationHistoryId > 0)
                {
                    EmployeesDesignationHistory previousDesignationHistory = _employeeDesignationHistoryRepository.GetPreviousDesignationHistoryByEmployeeId(employeeId, employeesDesignationHistory?.DesignationHistoryId);
                    if (previousDesignationHistory != null)
                    {
                        previousDesignationHistory.EffiectiveToDate = fromDate?.AddDays(-1);
                        _employeeDesignationHistoryRepository.Update(previousDesignationHistory);
                        await _employeeDesignationHistoryRepository.SaveChangesAsync();
                    }
                }
            }

            return true;
        }
        #endregion

        #region
        public EmployeeDownloadData EmployeeListDownload(PaginationView paginationView)
        {
            return _employeeRepository.EmployeeListDownload(paginationView);
        }
        #endregion

        #region 
        public List<AuditDetailView> GetAuditListByEmployeeId(int employeeId)
        {
            return _auditRepository.GetAuditListByEmployeeId(employeeId);
        }
        #endregion

        #region Approve Or Reject EmployeeRequest
        public async Task<ChangeRequestEmailView> ApproveOrRejectEmployeeRequest(ApproveOrRejectEmpRequestListView approveOrRejectEmpRequestList)
        {
            ChangeRequestEmailView changeRequestEmail = new ChangeRequestEmailView();
            EmployeeMasterData employeeMaster = GetEmployeeMasterData("");
            List<EmployeeRequest> updatedRequest = new List<EmployeeRequest>();
            int employeeId = 0;
            foreach (ApproveOrRejectEmpRequestView ApproveOrRejectEmployeeRequestlst in approveOrRejectEmpRequestList.ApproveOrRejectEmployeeRequestlst)
            {
                try
                {
                    EmployeeRequest employeeRequest = new EmployeeRequest();
                    employeeRequest = await _employeeRequestRepository.GetEmployeeRequestById(ApproveOrRejectEmployeeRequestlst.EmployeeRequestId);
                    employeeId = employeeRequest.EmployeeId;
                    if (employeeRequest != null)
                    {
                        //employeeRequest.EmployeeRequestId = approveOrRejectEmpRequestList.ApproveOrRejectEmployeeRequestlst.EmployeeRequestId;
                        //employeeRequest.EmployeeId = ApproveOrRejectEmployeeRequestlst.EmployeeId;
                        //employeeRequest.RequestCategory = ApproveOrRejectEmployeeRequestlst.RequestCategory;
                        //employeeRequest.ChangeRequestId = ApproveOrRejectEmployeeRequestlst.ChangeRequestId;
                        employeeRequest.Status = ApproveOrRejectEmployeeRequestlst.Status;
                        employeeRequest.ModifiedOn = DateTime.UtcNow; ;
                        employeeRequest.ModifiedBy = ApproveOrRejectEmployeeRequestlst.ApprovedBy;
                        employeeRequest.ApprovedBy = ApproveOrRejectEmployeeRequestlst.ApprovedBy;
                        employeeRequest.ApprovedOn = DateTime.UtcNow;
                        employeeRequest.Remark = ApproveOrRejectEmployeeRequestlst.Remarks;
                        _employeeRequestRepository.Update(employeeRequest);
                        await _employeeRequestRepository.SaveChangesAsync();
                        if (employeeRequest.Status == "Approved")
                        {
                            AddApprovedChangesToEmployeeTable(employeeRequest, employeeMaster);
                        }
                        AddEmployeeChangesToAudit(employeeRequest);
                        updatedRequest.Add(employeeRequest);
                    }

                }
                catch (Exception e)
                {
                    throw e;
                }

            }
            changeRequestEmail.ApprovedData = updatedRequest;
            if (approveOrRejectEmpRequestList?.isEmployee == true)
            {
                changeRequestEmail.employeeMasterEmail = await _appConstantRepository.GetEmployeeEmailByName("RequestWithdrawn");
            }
            else
            {
                changeRequestEmail.employeeMasterEmail = await _appConstantRepository.GetEmployeeEmailByName("ApproveOrReject");
            }
            changeRequestEmail.EmployeeDetail = _employeeRepository.GetEmployeeNameByEmployeeId(employeeId);
            return changeRequestEmail;
        }
        #endregion

        #region Get Employee Request
        public List<EmployeeRequestListView> GetEmployeeRequest(int employeeId)
        {
            return _employeeRequestRepository.GetEmployeeRequest(employeeId);
        }

        public EmployeesRequestList GetEmployeeApproval(int employeeID)
        {
            return _employeeRequestRepository.GetEmployeeApproval(employeeID);
        }
        #endregion

        #region
        public async Task<bool> AddEmployeeChangesToAudit(EmployeeRequest employeeRequest)
        {
            List<EmployeeRequestDetail> employeeRequestsDetailList = _employeeRequestDetailsRepository.GetEmployeeRequestDetailByCRId(employeeRequest.ChangeRequestId);
            foreach (EmployeeRequestDetail employeeRequestDetail in employeeRequestsDetailList)
            {
                if (employeeRequestDetail.Field != "Skills")
                {
                    try
                    {
                        EmployeeAudit employeeAudit = new EmployeeAudit();
                        employeeAudit.ChangeRequestID = employeeRequest.ChangeRequestId;
                        employeeAudit.Field = employeeRequestDetail.Field;
                        employeeAudit.EmployeeId = employeeRequest.EmployeeId;
                        employeeAudit.OldValue = employeeRequestDetail.OldValue;
                        employeeAudit.NewValue = employeeRequestDetail.NewValue;
                        employeeAudit.ActionType = "Request";
                        employeeAudit.ApprovedById = employeeRequest.ApprovedBy;
                        employeeAudit.CreatedOn = employeeRequestDetail.CreatedOn;
                        employeeAudit.CreatedBy = employeeRequestDetail.CreatedBy;
                        employeeAudit.Status = employeeRequest.Status;
                        employeeAudit.Remark = employeeRequest.Remark;
                        employeeAudit.ApprovedOn = employeeRequest.ApprovedOn;
                        await _auditRepository.AddAsync(employeeAudit);
                        await _auditRepository.SaveChangesAsync();

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }


            }

            return true;
        }
        #endregion

        public async Task<bool> AddApprovedChangesToEmployeeTable(EmployeeRequest employeeRequest, EmployeeMasterData employeeMaster)
        {
            List<EmployeeRequestDetail> employeeRequestsDetailList = _employeeRequestDetailsRepository.GetEmployeeRequestDetailByCRId(employeeRequest.ChangeRequestId);
            EmployeesPersonalInfo employeePersonalDetails = _employeesPersonalInfoRepository.GetEmployeePersonalIdByEmployeeID(employeeRequest.EmployeeId);
            Employees employee = await _employeeRepository.GetEmployeeForBUlkUploadByEmployeeId(employeeRequest.EmployeeId);
            EmployeeDependent dependent = new EmployeeDependent();
            EducationDetail educationDetail = new EducationDetail();
            List<int> skillList = new List<int>();
            try
            {
                foreach (EmployeeRequestDetail employeeRequestDetail in employeeRequestsDetailList)
                {
                    var field = employeeRequestDetail.Field;
                    if (employeeRequest.RequestCategory == "Photo" || employeeRequest.RequestCategory == "Marital Status")
                    {
                        var data = employee.GetType().GetProperty(field);
                        data.SetValue(employee, employeeRequestDetail.NewValue);
                        _employeeRepository.Update(employee);
                        await _employeeRepository.SaveChangesAsync();
                    }
                    else if (employeeRequest.RequestCategory == "Dependent Details")
                    {
                        if (employeeRequest.ChangeType == "Removed")
                        {
                            dependent = await _employeeDependentRepository.GetEmployeeDependentByDependentId(employeeRequest.SourceId == null ? 0 : (int)employeeRequest.SourceId);
                        }
                        else
                        {
                            if (employeeRequest.ChangeType == "Updated")
                            {
                                dependent = await _employeeDependentRepository.GetEmployeeDependentByDependentId(employeeRequest.SourceId == null ? 0 : (int)employeeRequest.SourceId);
                            }

                            var data = dependent.GetType().GetProperty(field);
                            if (field == "EmployeeRelationshipId")
                            {
                                var NewValue = employeeMaster?.EmployeeRelationshipList.Find(x => x.EmployeeRelationshipName == employeeRequestDetail.NewValue)?.EmployeeRelationshipId;
                                data.SetValue(dependent, NewValue);
                            }
                            else if (field == "EmployeeRelationDateOfBirth")
                            {
                                data.SetValue(dependent, Convert.ToDateTime(employeeRequestDetail.NewValue));
                            }
                            else
                            {
                                data.SetValue(dependent, employeeRequestDetail.NewValue);
                            }
                        }

                    }
                    else if (employeeRequest.RequestCategory == "Certification & Skills")
                    {
                        //if(field == "Skills")
                        //{
                        //    int skillId =(int) employeeMaster?.SkillsetList.Find(x => x.Skillset == employeeRequestDetail.NewValue)?.SkillsetId;
                        //    skillList.Add(skillId);
                        //}
                        var data = educationDetail?.GetType()?.GetProperty(field);
                        if (field == "BoardId")
                        {
                            var NewValue = employeeMaster?.Board.Find(x => x.Value == employeeRequestDetail.NewValue)?.Key;
                            data.SetValue(educationDetail, NewValue);
                        }
                        else if (field == "EducationTypeId")
                        {
                            var NewValue = employeeMaster?.QualificationLIst.Find(x => x.Value == employeeRequestDetail.NewValue)?.Key;
                            data.SetValue(educationDetail, NewValue);
                        }
                        else if (field == "MarkPercentage")
                        {
                            decimal dataValue = employeeRequestDetail.NewValue == null ? 0 : Convert.ToDecimal(employeeRequestDetail.NewValue);
                            data.SetValue(educationDetail, dataValue);
                        }
                        else if (field == "YearOfCompletion")
                        {
                            data.SetValue(educationDetail, Convert.ToDateTime(employeeRequestDetail?.NewValue));
                        }
                        else if (field == "ExpiryYear")
                        {
                            data.SetValue(educationDetail, (int?)Convert.ToInt64(employeeRequestDetail?.NewValue));
                        }
                        else
                        {
                            data.SetValue(educationDetail, employeeRequestDetail.NewValue);
                        }
                    }
                    else
                    {
                        var data = employeePersonalDetails?.GetType()?.GetProperty(field);
                        int? NewValue = 0;
                        if (field == "AccountNumber")
                        {
                            decimal dataValue = employeeRequestDetail.NewValue == null ? 0 : Convert.ToDecimal(employeeRequestDetail.NewValue);
                            data.SetValue(employeePersonalDetails, dataValue);
                        }
                        else
                        {
                            switch (field)
                            {
                                case "PermanentState":
                                    NewValue = employeeMaster?.StateList.Find(x => x.StateName == employeeRequestDetail.NewValue)?.StateId;
                                    data.SetValue(employeePersonalDetails, NewValue);
                                    break;
                                case "PermanentCountry":
                                    NewValue = employeeMaster?.CountryList.Find(x => x.CountryName == employeeRequestDetail.NewValue)?.CountryId;
                                    data.SetValue(employeePersonalDetails, NewValue);
                                    break;
                                case "CommunicationState":
                                    NewValue = employeeMaster?.StateList.Find(x => x.StateName == employeeRequestDetail.NewValue)?.StateId;
                                    data.SetValue(employeePersonalDetails, NewValue);
                                    break;
                                case "CommunicationCountry":
                                    NewValue = employeeMaster?.CountryList.Find(x => x.CountryName == employeeRequestDetail.NewValue)?.CountryId;
                                    data.SetValue(employeePersonalDetails, NewValue);
                                    break;
                                default:
                                    data.SetValue(employeePersonalDetails, employeeRequestDetail.NewValue);
                                    break;
                            }
                        }

                        _employeesPersonalInfoRepository.Update(employeePersonalDetails);
                        await _employeesPersonalInfoRepository.SaveChangesAsync();

                    }
                }
                if (employeeRequest.RequestCategory == "Communication Address" || employeeRequest.RequestCategory == "Permanent Address" || employeeRequest.RequestCategory == "Marital Status")
                {
                    EmployeeRequestDocument requestDocument = _employeeRequestDocumentRepository.GetEmployeeProofDocumentByCRId(employeeRequest.ChangeRequestId);
                    if (requestDocument != null && employeePersonalDetails != null && employeeRequest.RequestCategory != "Marital Status")
                    {
                        EmployeeDocument employeeDocument = _employeeDocumentRepository.GetDocumentDetailBySourceIdAndType((int)employeeRequest?.EmployeeId, employeeRequest.RequestCategory == "Communication Address" ? "communicationAddressProof" : "permanentAddressProof");
                        if (employeeDocument == null) employeeDocument = new EmployeeDocument();
                        string oldName = employeeDocument.DocumentName;
                        employeeDocument.DocumentName = requestDocument.DocumentName;
                        employeeDocument.DocumentPath = requestDocument.DocumentPath;
                        employeeDocument.EmployeeID = employeeRequest.EmployeeId;
                        AuditView audit = new AuditView();
                        audit.OldValue = oldName;
                        audit.NewValue = requestDocument.DocumentName;
                        audit.Field = employeeRequest.RequestCategory + " Document";
                        employeeDocument.DocumentType = employeeRequest.RequestCategory == "Communication Address" ? "communicationAddressProof" : "permanentAddressProof";
                        employeeDocument.SourceId = employeeRequest.EmployeeId;
                        employeeDocument.SourceType = "Employee";
                        employeeDocument.EmployeeID = requestDocument.EmployeeID;
                        employeeDocument.CreatedOn = DateTime.Now;
                        employeeDocument.CreatedBy = employeeRequest.CreatedBy == null ? (int)employeeRequest.CreatedBy : 0;
                        if (employeeDocument.EmployeeDocumentId != 0)
                        {
                            _employeeDocumentRepository.Update(employeeDocument);
                        }
                        else
                        {
                            await _employeeDocumentRepository.AddAsync(employeeDocument);
                        }
                        await _employeeDocumentRepository.SaveChangesAsync();
                        AddToAudit(audit, employeeRequest);
                    }
                    else
                    {
                        EmployeeDocument employeeDocument = new EmployeeDocument();
                        employeeDocument = _employeeDocumentRepository.GetDocumentDetailBySourceIdAndType(employeeRequest.EmployeeId, "maritalstatusProof");
                        if (employeeDocument == null)
                        {
                            employeeDocument = new EmployeeDocument();
                            //employeeDocument.EmployeeDocumentId = 0;
                            employeeDocument.CreatedOn = DateTime.UtcNow;
                            employeeDocument.CreatedBy = requestDocument.CreatedBy == null ? (int)employeeRequest.CreatedBy : requestDocument.CreatedBy;

                        }
                        string oldName = employeeDocument.DocumentName;
                        employeeDocument.DocumentName = requestDocument.DocumentName;
                        employeeDocument.DocumentPath = requestDocument.DocumentPath;
                        employeeDocument.EmployeeID = requestDocument.EmployeeID;
                        employeeDocument.DocumentType = "maritalstatusProof";
                        employeeDocument.SourceType = "Employee";
                        employeeDocument.SourceId = employeeRequest.EmployeeId;
                        employeeDocument.CreatedOn = DateTime.Now;
                        employeeDocument.CreatedBy = employeeRequest.CreatedBy == null ? (int)employeeRequest.CreatedBy : 0;
                        AuditView audit = new AuditView();
                        audit.OldValue = oldName;
                        audit.NewValue = requestDocument.DocumentName;
                        audit.Field = employeeRequest.RequestCategory + " Document";
                        if (employeeDocument.EmployeeDocumentId != 0)
                        {
                            _employeeDocumentRepository.Update(employeeDocument);
                        }
                        else
                        {
                            await _employeeDocumentRepository.AddAsync(employeeDocument);
                        }
                        await _employeeDocumentRepository.SaveChangesAsync();
                        AddToAudit(audit, employeeRequest);
                    }

                }
                else if (employeeRequest.RequestCategory == "Dependent Details")
                {
                    if (employeeRequest.ChangeType == "Removed")
                    {
                        _employeeDependentRepository.Delete(dependent);
                        await _employeeDependentRepository.SaveChangesAsync();
                    }
                    else
                    {
                        dependent.CreatedOn = DateTime.UtcNow;
                        if (dependent.EmployeeDependentId == 0)
                        {
                            dependent.EmployeeID = employeeRequest.EmployeeId;
                            dependent.CreatedBy = employeeRequest.CreatedBy;
                            await _employeeDependentRepository.AddAsync(dependent);
                            await _employeeDependentRepository.SaveChangesAsync();
                        }
                        else
                        {
                            dependent.ModifiedBy = employeeRequest.ApprovedBy;
                            _employeeDependentRepository.Update(dependent);
                            await _employeeDependentRepository.SaveChangesAsync();
                        }
                        EmployeeRequestDocument requestDocument = _employeeRequestDocumentRepository.GetEmployeeProofDocumentByCRId(employeeRequest.ChangeRequestId);
                        if (requestDocument != null)
                        {
                            EmployeeDocument employeeDocument = new EmployeeDocument();
                            employeeDocument = _employeeDocumentRepository.GetDocumentDetailBySourceIdAndType((int)dependent?.EmployeeDependentId, "dependentDetailsProof");
                            if (employeeDocument == null)
                            {
                                employeeDocument = new EmployeeDocument();
                                employeeDocument.EmployeeID = employeeRequest?.EmployeeId;
                                employeeDocument.CreatedOn = DateTime.UtcNow;
                                employeeDocument.CreatedBy = requestDocument?.CreatedBy == null ? 0 : (int)requestDocument?.CreatedBy;

                            }
                            string oldName = employeeDocument.DocumentName;
                            employeeDocument.SourceId = dependent?.EmployeeDependentId;
                            employeeDocument.SourceType = "Dependent Detail";
                            employeeDocument.DocumentType = "dependentDetailsProof";
                            employeeDocument.DocumentPath = requestDocument.DocumentPath;
                            employeeDocument.DocumentName = requestDocument.DocumentName;
                            AuditView audit = new AuditView();
                            audit.OldValue = oldName;
                            audit.NewValue = requestDocument.DocumentName;
                            audit.Field = employeeRequest.RequestCategory + " Document";
                            if (employeeDocument.EmployeeDocumentId != 0)
                            {
                                _employeeDocumentRepository.Update(employeeDocument);
                            }
                            else
                            {
                                await _employeeDocumentRepository.AddAsync(employeeDocument);
                            }
                            await _employeeDocumentRepository.SaveChangesAsync();
                            AddToAudit(audit, employeeRequest);
                        }
                    }

                }
                //else if(skillList?.Count > 0)
                //{
                //    int employeeId = employeeRequest?.EmployeeId == null ? 0 : (int)employeeRequest?.EmployeeId;
                //    List<int> oldSkillSetId = _employeesSkillsetRepository.GetEmployeeSkillsIdByEmployeeId(employeeId);
                //    List<int> removedSkills = oldSkillSetId.Where(x => !skillList.Contains(x)).ToList();
                //    foreach(int skillid in removedSkills)
                //    {
                //        EmployeesSkillset employeesSkillset = _employeesSkillsetRepository.GetEmployeesSkillsetBySkillsetId(skillid, employeeId);
                //        _employeesSkillsetRepository.Delete(employeesSkillset);
                //        _employeesSkillsetRepository.SaveChangesAsync();
                //        AuditView audit = new AuditView();
                //        audit.OldValue = employeeMaster.SkillsetList.Find(x => x.SkillsetId == skillid).Skillset;
                //        audit.NewValue = null;
                //        audit.Field = "Skill Removed";
                //        AddToAudit(audit,employeeRequest);
                //    }
                //    List<int> AddedSkills = skillList.Where(x => !oldSkillSetId.Contains(x)).ToList();
                //    foreach (int skillid in AddedSkills)
                //    {
                //        EmployeesSkillset employeesskillSet = new EmployeesSkillset();
                //        employeesskillSet.EmployeeId = employeeId;
                //        employeesskillSet.SkillsetId = skillid;
                //        employeesskillSet.CreatedBy = employeeRequest.CreatedBy;
                //        employeesskillSet.CreatedOn = DateTime.UtcNow;
                //        _employeesSkillsetRepository.AddAsync(employeesskillSet);
                //        _employeesSkillsetRepository.SaveChangesAsync();
                //        AuditView audit = new AuditView();
                //        audit.OldValue = null;
                //        audit.NewValue = employeeMaster.SkillsetList.Find(x => x.SkillsetId == skillid).Skillset; ;
                //        audit.Field = "Skill Added";
                //        AddToAudit(audit, employeeRequest);
                //    }
                //}
                else if (employeeRequest.RequestCategory == "Certification & Skills" && skillList?.Count == 0)
                {
                    educationDetail.CreatedOn = DateTime.UtcNow;
                    educationDetail.EmployeeId = employeeRequest?.EmployeeId;
                    await _educationDetailRepository.AddAsync(educationDetail);
                    await _educationDetailRepository.SaveChangesAsync();

                    EmployeeRequestDocument requestDocument = _employeeRequestDocumentRepository.GetEmployeeProofDocumentByCRId(employeeRequest.ChangeRequestId);
                    if (requestDocument != null)
                    {
                        EmployeeDocument employeeDocument = new EmployeeDocument();
                        employeeDocument.EmployeeID = requestDocument?.EmployeeID;
                        employeeDocument.SourceId = educationDetail.EducationDetailId;
                        employeeDocument.SourceType = "EducationDetail";
                        employeeDocument.DocumentType = "certificate";
                        employeeDocument.DocumentPath = requestDocument.DocumentPath;
                        employeeDocument.DocumentName = requestDocument.DocumentName;
                        employeeDocument.CreatedOn = DateTime.UtcNow;
                        employeeDocument.CreatedBy = requestDocument?.CreatedBy == null ? 0 : (int)requestDocument?.CreatedBy;
                        await _employeeDocumentRepository.AddAsync(employeeDocument);
                        await _employeeDocumentRepository.SaveChangesAsync();
                        AuditView audit = new AuditView();
                        audit.OldValue = null;
                        audit.NewValue = employeeDocument.DocumentName;
                        audit.Field = "Certificate Added";
                        AddToAudit(audit, employeeRequest);
                    }
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return true;
        }

        #region Create the directory path for document
        private string GetDirectoryPathForRequestDoc(SupportingDocumentsView supportingDocuments)
        {
            //Add supporting documents
            if (!string.IsNullOrEmpty(supportingDocuments.BaseDirectory))
            {
                //Create base directory
                if (!Directory.Exists(supportingDocuments.BaseDirectory))
                {
                    Directory.CreateDirectory(supportingDocuments.BaseDirectory);
                }
                //Create source type directory
                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType)))
                {
                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType));
                }
                //Create Document type directory
                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType)))
                {
                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType));
                }
                //Create accountId directory
                if (!Directory.Exists(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType, supportingDocuments.proofDocumentId.ToString())))
                {
                    Directory.CreateDirectory(Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType, supportingDocuments.proofDocumentId.ToString()));
                }
            }
            return (Path.Combine(supportingDocuments.BaseDirectory, supportingDocuments.SourceType, supportingDocuments.DocumentType, supportingDocuments.proofDocumentId.ToString()));
        }
        #endregion

        #region Add Document Proof 
        private async Task<bool> AddProofDocument(EmployeeRequestView employeeRequest)
        {
            try
            {
                //foreach (DocumentsToUpload item in employeeRequest?.EmployeeDocumentList)
                //{
                if (employeeRequest.RequestProof?.DocumentName != null)
                {
                    string documentPath = Path.Combine(employeeRequest?.DocumentPath, employeeRequest.RequestProof?.DocumentName);
                    if (!System.IO.File.Exists(employeeRequest.RequestProof?.DocumentName) && employeeRequest.RequestProof?.DocumentSize > 0)
                    {
                        if (employeeRequest.RequestProof.DocumentAsBase64.Contains(","))
                        {
                            employeeRequest.RequestProof.DocumentAsBase64 = employeeRequest.RequestProof.DocumentAsBase64.Substring(employeeRequest.RequestProof.DocumentAsBase64.IndexOf(",") + 1);
                        }
                        employeeRequest.RequestProof.DocumentAsByteArray = Convert.FromBase64String(employeeRequest.RequestProof.DocumentAsBase64);
                        using (Stream fileStream = new FileStream(documentPath, FileMode.Create))
                        {
                            fileStream.Write(employeeRequest.RequestProof.DocumentAsByteArray, 0, employeeRequest.RequestProof.DocumentAsByteArray.Length);
                        }
                        EmployeeRequestDocument employeeDocument = new EmployeeRequestDocument();
                        employeeDocument.EmployeeID = employeeRequest?.EmployeeId;
                        employeeDocument.ChangeRequestId = employeeRequest?.ChangeRequestId;
                        employeeDocument.DocumentType = employeeRequest.RequestCategory;
                        employeeDocument.DocumentPath = documentPath;
                        employeeDocument.DocumentName = employeeRequest.RequestProof.DocumentName;
                        employeeDocument.CreatedOn = DateTime.UtcNow;
                        employeeDocument.CreatedBy = employeeRequest?.CreatedBy == null ? 0 : (int)employeeRequest?.CreatedBy;
                        await _employeeRequestDocumentRepository.AddAsync(employeeDocument);
                        await _employeeRequestDocumentRepository.SaveChangesAsync();
                    }
                }
                //}
                return true;
            }
            catch (Exception e)
            {
                throw;
            }
        }
        #endregion

        #region 
        private async Task<bool> AddToAudit(AuditView AuditView, EmployeeRequest employeeRequest)
        {
            EmployeeAudit audit = new EmployeeAudit();
            audit.ChangeRequestID = employeeRequest.ChangeRequestId;
            audit.EmployeeId = employeeRequest.CreatedBy;
            audit.ApprovedById = employeeRequest.ApprovedBy;
            audit.Field = AuditView.Field;
            audit.NewValue = AuditView.NewValue;
            audit.OldValue = AuditView.OldValue;
            audit.ActionType = "Request";
            audit.ApprovedOn = employeeRequest.ApprovedOn;
            audit.Status = employeeRequest.Status;
            audit.Remark = employeeRequest.Remark;
            audit.CreatedOn = DateTime.UtcNow;
            _auditRepository.AddAsync(audit);
            _auditRepository.SaveChangesAsync();
            return true;
        }
        #endregion

        #region
        public async Task<bool> AddEmployeeChangesToAuditWithoutRequest(AuditView AuditView, EmployeeRequestView employeeRequest)
        {
            foreach (EmployeeRequestDetailsView employeeRequestDetail in employeeRequest?.EmployeeRequestDetailslst)
            {
                if (employeeRequestDetail.Field != "Skills")
                {
                    try
                    {
                        EmployeeAudit employeeAudit = new EmployeeAudit();
                        employeeAudit.ChangeRequestID = employeeRequest.ChangeRequestId;
                        employeeAudit.Field = employeeRequestDetail.Field;
                        employeeAudit.EmployeeId = employeeRequest.EmployeeId;
                        employeeAudit.OldValue = employeeRequestDetail.OldValue;
                        employeeAudit.NewValue = employeeRequestDetail.NewValue;
                        employeeAudit.ActionType = "Update";
                        employeeAudit.CreatedOn = employeeRequestDetail.CreatedOn;
                        employeeAudit.CreatedBy = employeeRequestDetail.CreatedBy;
                        await _auditRepository.AddAsync(employeeAudit);
                        await _auditRepository.SaveChangesAsync();

                    }
                    catch (Exception e)
                    {
                        throw e;
                    }
                }
                else
                {
                    EmployeeAudit audit = new EmployeeAudit();
                    audit.ChangeRequestID = employeeRequest.ChangeRequestId;
                    audit.EmployeeId = employeeRequest.CreatedBy;
                    audit.ApprovedById = employeeRequest.ApprovedBy;
                    audit.Field = AuditView.Field;
                    audit.NewValue = AuditView.NewValue;
                    audit.ActionType = "Update";
                    audit.OldValue = AuditView.OldValue;
                    audit.ApprovedOn = employeeRequest.ApprovedOn;
                    audit.Status = employeeRequest.Status;
                    audit.Remark = employeeRequest.Remarks;
                    audit.CreatedOn = DateTime.UtcNow;
                    _auditRepository.AddAsync(audit);
                    _auditRepository.SaveChangesAsync();
                }


            }

            return true;
        }
        #endregion
        #region Get employee list
        public List<EmployeeDetailListView> GetMyApprovalEmployeeList(PaginationView pagination)
        {
            return _employeeRequestRepository.GetMyApprovalEmployeeList(pagination);
        }
        #endregion 
        #region Get employee count
        public int GetMyApprovalEmployeeCount(PaginationView pagination)
        {
            return _employeeRequestRepository.GetMyApprovalEmployeeCount(pagination);
        }
        #endregion

        #region Contract end date notification
        public async Task<ChangeRequestEmailView> ContractEndDateNotification()
        {
            ChangeRequestEmailView changeRequestEmail = new ChangeRequestEmailView();
            changeRequestEmail.employeeMasterEmail = await _employeeRepository.getEmployeeEmailByName("ContractClosureNotification");
            changeRequestEmail.NotifiedEmployee = await _employeeRepository.ContractEndDateNotification();
            return changeRequestEmail;
        }
        #endregion
        #region Update Employee Designation 
        public async Task<bool> UpdateEmployeeDesignation()
        {
            try
            {
                List<EmployeesDesignationHistory> designationList = _employeeDesignationHistoryRepository.GetEmployeeDesignationHistoryByEffectiveDate(DateTime.Now.Date);
                if (designationList?.Count > 0)
                {
                    foreach (EmployeesDesignationHistory designation in designationList)
                    {
                        try
                        {
                            Employees employee = _employeeRepository.GetEmployeeDetailByEmployeeId(designation?.EmployeeId == null ? 0 : (int)designation.EmployeeId);
                            int? oldDesignation = employee.DesignationId;
                            if (employee != null)
                            {
                                employee.DesignationId = designation.DesignationId;
                                employee.DesignationEffectiveFrom = designation.EffiectiveFromDate;
                                employee.ModifiedOn = DateTime.UtcNow;
                                employee.ModifiedBy = designation.CreatedBy;
                                _employeeRepository.Update(employee);
                                await _employeeRepository.SaveChangesAsync();

                                EmployeeAudit audit = new EmployeeAudit();
                                audit.EmployeeId = designation.EmployeeId;
                                audit.ChangeRequestID = Guid.NewGuid();
                                audit.CreatedOn = DateTime.UtcNow;
                                audit.CreatedBy = designation.CreatedBy;
                                audit.Field = "DesignationId";
                                audit.OldValue = oldDesignation?.ToString();
                                audit.NewValue = designation?.DesignationId?.ToString();
                                await _auditRepository.AddAsync(audit);
                                await _auditRepository.SaveChangesAsync();
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.LoggingErrorTrack(ex, "Employee", "Employee/UpdateEmployeeDesignation");
                        }
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
            //return false;
        }
        #endregion
        #region Download document
        public DocumentsToUpload DownloadDocumentById(int documentId)
        {
            DocumentsToUpload documentDetails = new DocumentsToUpload();
            try
            {
                if (documentId > 0)
                {
                    documentDetails = _employeeRequestDocumentRepository.GetDocumentByDocumentId(documentId);
                }
            }
            catch (Exception)
            {
                throw;
            }
            return documentDetails;
        }
        #endregion
        #region Get employee data for dropdown
        public List<EmployeeDataForDropDown> GetEmployeeDropDownList(bool isAll)
        {
            return _employeeRepository.GetEmployeeDropDownList(isAll);
        }
        #endregion

        #region Insert or update Location
        /// <summary>
        /// Insert or update Location
        /// </summary>
        /// <param name="pLocation"></param>
        /// <returns></returns>
        public async Task<int> AddOrUpdateLocation(EmployeeLocationView pLocation)
        {
            EmployeeLocation Location = new EmployeeLocation();
            try
            {
                Location = _employeeLocationRepository.GetLocationById(pLocation.LocationId);
                if (Location == null) Location = new EmployeeLocation();
                Location.Location = pLocation.Location;
                if (pLocation.LocationId == 0)
                {
                    Location.CreatedBy = pLocation.CreatedBy;
                    Location.CreatedOn = DateTime.Now;
                    await _employeeLocationRepository.AddAsync(Location);
                }
                else
                {
                    Location.ModifiedBy = pLocation.ModifiedBy;
                    Location.ModifiedOn = DateTime.Now;
                    _employeeLocationRepository.Update(Location);
                }
                await _employeeLocationRepository.SaveChangesAsync();

            }
            catch (Exception)
            {
                throw;
            }
            return Location.LocationId;
        }
        #endregion

        public bool LocationNameDuplicate(string locationName)
        {
            return _employeeLocationRepository.LocationNameDuplicate(locationName);
        }
        #region Get Employee PersonalInfo By EmployeeID
        public EmployeeBasicInfoView GetEmployeeBasicInfoByEmployeeID(int employeeId)
        {
            EmployeeBasicInfoView employeeDetails = new();
            employeeDetails = _employeeRepository.GetEmployeeBasicInfoByEmployeeID(employeeId);
            employeeDetails.EmployeesPersonalInfo = GetEmployeesPersonalInformation(employeeId);
            employeeDetails.EmployeesPersonalInfo.CommunicationStateName = _stateRepository.GetStateNameById(employeeDetails?.EmployeesPersonalInfo?.CommunicationState == null ? 0 : (int)employeeDetails?.EmployeesPersonalInfo?.CommunicationState);
            employeeDetails.EmployeesPersonalInfo.CommunicationCountryName = _countryRepository.GetCountryNameById(employeeDetails?.EmployeesPersonalInfo?.CommunicationCountry == null ? 0 : (int)employeeDetails?.EmployeesPersonalInfo?.CommunicationCountry);
            employeeDetails.EmployeesPersonalInfo.PermanentStateName = _stateRepository.GetStateNameById(employeeDetails?.EmployeesPersonalInfo?.PermanentState == null ? 0 : (int)employeeDetails?.EmployeesPersonalInfo?.PermanentState);
            employeeDetails.EmployeesPersonalInfo.PermanentCountryName = _countryRepository.GetCountryNameById(employeeDetails?.EmployeesPersonalInfo?.PermanentCountry == null ? 0 : (int)employeeDetails?.EmployeesPersonalInfo?.PermanentCountry);
            return employeeDetails;
        }
        #endregion

        #region Get employee list for new resignation
        public List<EmployeeDataForDropDown> GetEmployeeListForNewResignation()
        {
            return _employeeRepository.GetEmployeeListForNewResignation();
        }
        #endregion

        #region Get employee Attendance Details
        public int GetEmployeeAttendanceDetailsCount(EmployeesAttendanceFilterView employeesAttendanceFilterView)
        {
            return _employeeRepository.GetEmployeeAttendanceDetailsCount(employeesAttendanceFilterView);
        }
        #endregion

        public EmployeeDetailsForLeave EmployeeDetailsForLeave(int employeeId)
        {
            return _employeeRepository.GetEmployeeByEmployeeIdForLeaves(employeeId);
        }
        public List<Roles> GetRolesList()
        {
            return _employeeRepository.GetRolesList();
        }

        public EmployeePlaceHolderValueView GetEmployeePersonalInfo(int employeeID)
        {
            EmployeePlaceHolderValueView employeePlaceHolderValue = new();
            EmployeesPersonalInfo employeesPersonalInfo = _employeesPersonalInfoRepository.GetEmployeePersonalIdByEmployeeID(employeeID);
            Employees employee = _employeeRepository.Get(employeeID);
            if (employee != null)
            {
                employeePlaceHolderValue.EmployeeID = employeeID;
                employeePlaceHolderValue.FullName = employee.FirstName + " " + employee.LastName;
                employeePlaceHolderValue.DateOfJoining = employee.DateOfJoining;
                employeePlaceHolderValue.Entity = employee.Entity > 0 ? _employeeRepository.GetEntityName((int)employee.Entity) : "";
                employeePlaceHolderValue.Designation = employee.DesignationId > 0 ? _designationRepository.Get((int)employee.DesignationId).DesignationName : "";
                employeePlaceHolderValue.Department = employee.DepartmentId > 0 ? _departmentRepository.Get((int)employee.DepartmentId).DepartmentName : "";
                employeePlaceHolderValue.FormattedEmployeeID = employee.FormattedEmployeeId;
                employeePlaceHolderValue.OtherEmail = employeesPersonalInfo?.OtherEmail;
                employeePlaceHolderValue.PersonalMobileNumber = employeesPersonalInfo?.PersonalMobileNumber;
                employeePlaceHolderValue.BaseWorkLocation = employee.LocationId > 0 ? _employeeLocationRepository.Get((int)employee.LocationId).Location : "";
                employeePlaceHolderValue.CurrentWorkLocation = employee.CurrentWorkLocationId > 0 ? _employeeLocationRepository.Get((int)employee.CurrentWorkLocationId).Location : "";
                string CommunicationAddress = "", PermanentAddress = "";
                if (!string.IsNullOrEmpty(employeesPersonalInfo?.CommunicationAddressLine1))
                    CommunicationAddress += employeesPersonalInfo?.CommunicationAddressLine1 + " ";
                if (!string.IsNullOrEmpty(employeesPersonalInfo?.CommunicationAddressLine2))
                    CommunicationAddress += employeesPersonalInfo?.CommunicationAddressLine2 + " ";
                if (!string.IsNullOrEmpty(employeesPersonalInfo?.CommunicationCity))
                    CommunicationAddress += employeesPersonalInfo?.CommunicationCity + " ";
                if (employeesPersonalInfo?.CommunicationState > 0)
                    CommunicationAddress += _stateRepository.Get((int)employeesPersonalInfo?.CommunicationState)?.StateName + " ";
                if (employeesPersonalInfo?.CommunicationCountry > 0)
                    CommunicationAddress += _countryRepository.Get((int)employeesPersonalInfo?.CommunicationCountry)?.CountryName + " ";
                if (!string.IsNullOrEmpty(employeesPersonalInfo?.CommunicationAddressZip))
                    CommunicationAddress += employeesPersonalInfo?.CommunicationAddressZip;
                if (!string.IsNullOrEmpty(employeesPersonalInfo?.PermanentAddressLine1))
                    PermanentAddress += employeesPersonalInfo?.PermanentAddressLine1 + " ";
                if (!string.IsNullOrEmpty(employeesPersonalInfo?.PermanentAddressLine2))
                    PermanentAddress += employeesPersonalInfo?.PermanentAddressLine2 + " ";
                if (!string.IsNullOrEmpty(employeesPersonalInfo?.PermanentCity))
                    PermanentAddress += employeesPersonalInfo?.PermanentCity + " ";
                if (employeesPersonalInfo?.PermanentState > 0)
                    PermanentAddress += _stateRepository.Get((int)employeesPersonalInfo?.PermanentState)?.StateName + " ";
                if (employeesPersonalInfo?.PermanentCountry > 0)
                    PermanentAddress += _countryRepository.Get((int)employeesPersonalInfo?.PermanentCountry)?.CountryName + " ";
                if (!string.IsNullOrEmpty(employeesPersonalInfo?.PermanentAddressZip))
                    PermanentAddress += employeesPersonalInfo?.PermanentAddressZip;
                employeePlaceHolderValue.CommunicationAddress = CommunicationAddress;
                employeePlaceHolderValue.PermanentAddress = PermanentAddress;
            }
            return employeePlaceHolderValue;
        }

        public List<int> GetEmployeeListForAcknowledgement(PolicyEmployeeAcknowledgementListView policyEmployeeAcknowledgement)
        {
            List<int> employeeIds = new();
            List<Employees> employees = new();
            employees = _employeeRepository.GetAll().Where(x => x.IsActive == true).ToList();
            if (policyEmployeeAcknowledgement.ToShareWithAll == false)
            {
                if (policyEmployeeAcknowledgement.DepartmentIds?.Count > 0)
                    employees = employees.Where(e => policyEmployeeAcknowledgement.DepartmentIds.Contains((int)e.DepartmentId)).ToList();
                if (policyEmployeeAcknowledgement.RoleIds?.Count > 0)
                    employees = employees.Where(e => policyEmployeeAcknowledgement.RoleIds.Contains((int)e.RoleId)).ToList();
                if (policyEmployeeAcknowledgement.LocationIds?.Count > 0)
                    employees = employees.Where(e => policyEmployeeAcknowledgement.LocationIds.Contains((int)e.LocationId)).ToList();
            }
            if (employees.Count > 0)
                employeeIds = employees.Select(x => x.EmployeeID).ToList();
            return employeeIds;
        }
        #region Get finance manager
        public FinanceManagerDetails GetfinanceManagerDetails(AccountDetails accountDetails)
        {
            try
            {
                return _roleRepository.GetfinanceManagerDetails(accountDetails);
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region
        public List<EmployeeList> GetEmployeeDetailForGrantLeaveById(EmployeeListByDepartment employeeList)
        {
            return _employeeRepository.GetEmployeeDetailForGrantLeaveById(employeeList);
        }
        #endregion
    }
}