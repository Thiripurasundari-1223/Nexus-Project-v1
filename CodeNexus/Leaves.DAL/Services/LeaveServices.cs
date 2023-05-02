using Leaves.DAL.Repository;
using Newtonsoft.Json;
using SharedLibraries.Common;
using SharedLibraries.Models.Leaves;
using SharedLibraries.Models.Notifications;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Appraisal;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using static SharedLibraries.ViewModels.Leaves.WeeklyOverviewReportView;

namespace Leaves.DAL.Services
{
    public class LeaveServices
    {
        private readonly IHolidayRepository _holidayRepository;
        public readonly ILeaveRepository _leaveRepository;
        public readonly ILeaveApplicableRepository _leaveApplicableRepository;
        public readonly ILeaveRestrictionsRepository _leaveRestrictionsRepository;
        public readonly ILeaveEntitlementRepository _leaveEntitlementRepository;
        public readonly ILeaveApplyRepository _LeaveApplyRepository;
        public readonly IAppliedLeaveDetailsRepository _AppliedLeaveDetailsRepository;
        public readonly ILeaveRejectionReasonRepository _LeaveRejectionReasonRepository;
        private readonly IHolidayShiftRepository _holidayShiftRepository;
        private readonly IHolidayLocationRepository _holidayLocationRepository;
        private readonly IHolidayDepartmentRepository _holidayDepartmentRepository;
        private readonly ILeaveDepartmentRepository _leaveDepartmentRepository;
        private readonly ILeaveDesignationRepository _leaveDesignationRepository;
        private readonly ILeaveLocationRepository _leaveLocationRepository;
        private readonly ILeaveRoleRepository _leaveRoleRepository;
        private readonly IEmployeeApplicableLeaveRepository _employeeApplicableLeaveRepository;
        private readonly ILeaveDurationRepository _leaveDurationRepository;
        private readonly ILeaveTypeRepository _leaveTypeRepository;
        private readonly IEmployeeLeaveDetailsRepository _employeeLeaveDetailsRepository;
        private readonly ILeaveTakenTogetherRepository _leaveTakenTogetherRepository;
        private readonly IProRateMonthDetailsRepository _proRateMonthDetailsRepository;
        private readonly IAppConstantsRepository _appConstantsRepository;
        private readonly ISpecificEmployeeDetailLeaveRepository _specificEmployeeDetailLeaveRepository;
        private readonly ILeaveCarryForwardRepository _leaveCarryForwardRepository;
        private readonly ILeaveGrantRequestDetailsRepository _leaveGrantRequestDetailsRepository;
        private readonly ILeaveGrantDocumentDetailsRepository _leaveGrantDocumentDetailsRepository;
        private readonly IGrantLeaveApprovalRepository _grantLeaveApprovalRepository;
        private readonly IEmployeeGrantLeaveApprovalRepository _employeeGrantLeaveApprovalRepository;
        private readonly ILeaveAdjustmentDetailsRepository _leaveAdjustmentDetailsRepository;
        private readonly ILeaveEmployeeTypeRepository _leaveEmployeeTypeRepository;
        private readonly ILeaveProbationStatuRepository _leaveProbationStatuRepository;

        #region Constructor
        public LeaveServices(IHolidayRepository holidayRepository, ILeaveRepository LeaveRepository, ILeaveApplicableRepository leaveApplicableRepository, ILeaveRestrictionsRepository LeaveRestrictionsRepository,
                ILeaveEntitlementRepository LeaveEntitlementRepository, ILeaveApplyRepository LeaveApplyRepository, IAppliedLeaveDetailsRepository AppliedLeaveDetailsRepository, ILeaveRejectionReasonRepository LeaveRejectionReasonRepository,
                IHolidayShiftRepository holidayShiftRepository, IHolidayLocationRepository holidayLocationRepository, IHolidayDepartmentRepository holidayDepartmentRepository, ILeaveDepartmentRepository leaveDepartmentRepository,
                ILeaveDesignationRepository leaveDesignationRepository, ILeaveLocationRepository leaveLocationRepository, ILeaveRoleRepository leaveRoleRepository, IEmployeeApplicableLeaveRepository employeeApplicableLeaveRepository,
                ILeaveDurationRepository leaveDurationRepository, ILeaveTakenTogetherRepository leaveTakenTogetherRepository, ILeaveTypeRepository leaveTypeRepository,
                IEmployeeLeaveDetailsRepository employeeLeaveDetailsRepository, IProRateMonthDetailsRepository proRateMonthDetailsRepository, IAppConstantsRepository appConstantsRepository,
                ISpecificEmployeeDetailLeaveRepository specificEmployeeDetailLeaveRepository, ILeaveCarryForwardRepository leaveCarryForwardRepository, ILeaveGrantRequestDetailsRepository leaveGrantRequestDetailsRepository, ILeaveGrantDocumentDetailsRepository leaveGrantDocumentDetailsRepository,
                IGrantLeaveApprovalRepository grantLeaveApprovalRepository, IEmployeeGrantLeaveApprovalRepository employeeGrantLeaveApprovalRepository,
                ILeaveAdjustmentDetailsRepository leaveAdjustmentDetailsRepository, ILeaveEmployeeTypeRepository leaveEmployeeTypeRepository, ILeaveProbationStatuRepository leaveProbationStatuRepository)
        {
            _holidayRepository = holidayRepository;
            _leaveRepository = LeaveRepository;
            _leaveApplicableRepository = leaveApplicableRepository;
            _leaveRestrictionsRepository = LeaveRestrictionsRepository;
            _leaveEntitlementRepository = LeaveEntitlementRepository;
            _LeaveApplyRepository = LeaveApplyRepository;
            _AppliedLeaveDetailsRepository = AppliedLeaveDetailsRepository;
            _LeaveRejectionReasonRepository = LeaveRejectionReasonRepository;
            _holidayShiftRepository = holidayShiftRepository;
            _holidayLocationRepository = holidayLocationRepository;
            _holidayDepartmentRepository = holidayDepartmentRepository;
            _leaveDepartmentRepository = leaveDepartmentRepository;
            _leaveDesignationRepository = leaveDesignationRepository;
            _leaveLocationRepository = leaveLocationRepository;
            _leaveRoleRepository = leaveRoleRepository;
            _employeeApplicableLeaveRepository = employeeApplicableLeaveRepository;
            _leaveDurationRepository = leaveDurationRepository;
            _leaveTakenTogetherRepository = leaveTakenTogetherRepository;
            _leaveTypeRepository = leaveTypeRepository;
            _employeeLeaveDetailsRepository = employeeLeaveDetailsRepository;
            _proRateMonthDetailsRepository = proRateMonthDetailsRepository;
            _appConstantsRepository = appConstantsRepository;
            _specificEmployeeDetailLeaveRepository = specificEmployeeDetailLeaveRepository;
            _leaveCarryForwardRepository = leaveCarryForwardRepository;
            _leaveGrantRequestDetailsRepository = leaveGrantRequestDetailsRepository;
            _leaveGrantDocumentDetailsRepository = leaveGrantDocumentDetailsRepository;
            _grantLeaveApprovalRepository = grantLeaveApprovalRepository;
            _employeeGrantLeaveApprovalRepository = employeeGrantLeaveApprovalRepository;
            _leaveAdjustmentDetailsRepository = leaveAdjustmentDetailsRepository;
            _leaveEmployeeTypeRepository = leaveEmployeeTypeRepository;
            _leaveProbationStatuRepository = leaveProbationStatuRepository;
        }
        #endregion

        #region Add or update Holiday       
        public async Task<int> AddOrUpdateHoliday(HolidayDetailView pHoliday)
        {
            try
            {
                Holiday holiday = new();
                if (pHoliday?.HolidayID != 0) holiday = _holidayRepository.GetByID(pHoliday.HolidayID);
                if (holiday != null)
                {
                    holiday.Year = pHoliday.Year;
                    holiday.HolidayName = pHoliday.HolidayName;
                    holiday.HolidayCode = pHoliday.HolidayCode;
                    holiday.HolidayDescription = pHoliday.HolidayDescription;
                    holiday.HolidayDate = pHoliday.HolidayDate;
                    holiday.IsRestrictHoliday = pHoliday.IsRestrictHoliday;
                }
                if (pHoliday?.HolidayID == 0)
                {
                    holiday.IsActive = false;
                    holiday.CreatedOn = DateTime.UtcNow;
                    holiday.CreatedBy = pHoliday.CreatedBy;
                    await _holidayRepository.AddAsync(holiday);
                    await _holidayRepository.SaveChangesAsync();
                }
                else
                {
                    holiday.ModifiedOn = DateTime.UtcNow;
                    holiday.ModifiedBy = pHoliday.ModifiedBy;
                    _holidayRepository.Update(holiday);
                    await _holidayRepository.SaveChangesAsync();
                    //Delete Holiday Shift
                    var shidtDetailsList = _holidayShiftRepository.GetHolidayShiftDetailsByHolidayId(pHoliday.HolidayID);
                    if (shidtDetailsList?.Count > 0)
                    {
                        foreach (HolidayShift shiftDetails in shidtDetailsList)
                        {
                            _holidayShiftRepository.Delete(shiftDetails);
                        }
                        await _holidayShiftRepository.SaveChangesAsync();
                    }
                    //Delete Holiday Location
                    var locationList = _holidayLocationRepository.GetHolidayLocationDetailsByHolidayId(pHoliday.HolidayID);
                    if (locationList?.Count > 0)
                    {
                        foreach (HolidayLocation location in locationList)
                        {
                            _holidayLocationRepository.Delete(location);
                        }
                        await _holidayLocationRepository.SaveChangesAsync();
                    }
                    //Delete Holiday Department
                    var departmentList = _holidayDepartmentRepository.GetHolidayDepartmentDetailsByHolidayId(pHoliday.HolidayID);
                    if (departmentList?.Count > 0)
                    {
                        foreach (HolidayDepartment department in departmentList)
                        {
                            _holidayDepartmentRepository.Delete(department);
                        }
                        await _holidayDepartmentRepository.SaveChangesAsync();
                    }
                }
                //Add Holiday Shift
                if (pHoliday?.ShiftDetailId?.Count > 0)
                {
                    foreach (int shiftDetailsId in pHoliday?.ShiftDetailId)
                    {
                        HolidayShift holidayShift = new();
                        holidayShift.HolidayId = holiday?.HolidayID;
                        holidayShift.ShiftDetailsId = shiftDetailsId;
                        holidayShift.CreatedOn = DateTime.UtcNow;
                        holidayShift.CreatedBy = pHoliday.CreatedBy;
                        await _holidayShiftRepository.AddAsync(holidayShift);
                    }
                    await _holidayShiftRepository.SaveChangesAsync();
                }
                //Add Holiday Location
                if (pHoliday?.LocationId?.Count > 0)
                {
                    foreach (int locationId in pHoliday?.LocationId)
                    {
                        HolidayLocation holidayLocation = new();
                        holidayLocation.HolidayId = holiday?.HolidayID;
                        holidayLocation.LocationId = locationId;
                        holidayLocation.CreatedOn = DateTime.UtcNow;
                        holidayLocation.CreatedBy = pHoliday.CreatedBy;
                        await _holidayLocationRepository.AddAsync(holidayLocation);
                    }
                    await _holidayLocationRepository.SaveChangesAsync();
                }
                //Add Holiday Document
                if (pHoliday?.DepartmentId?.Count > 0)
                {
                    foreach (int departmentId in pHoliday?.DepartmentId)
                    {
                        HolidayDepartment holidayDepartment = new();
                        holidayDepartment.HolidayId = holiday?.HolidayID;
                        holidayDepartment.DepartmentId = departmentId;
                        holidayDepartment.CreatedOn = DateTime.UtcNow;
                        holidayDepartment.CreatedBy = pHoliday.CreatedBy;
                        await _holidayDepartmentRepository.AddAsync(holidayDepartment);
                    }
                    await _holidayDepartmentRepository.SaveChangesAsync();
                }
                return holiday.HolidayID;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Delete Holidayapprove
        public async Task<bool> DeleteHoliday(int pHolidayID)
        {
            try
            {
                Holiday holiday = _holidayRepository.GetByID(pHolidayID);
                if (holiday != null && holiday?.HolidayID > 0)
                {
                    _holidayRepository.Delete(holiday);
                    await _holidayRepository.SaveChangesAsync();
                    //Delete Holiday Shift
                    var shidtDetailsList = _holidayShiftRepository.GetHolidayShiftDetailsByHolidayId(pHolidayID);
                    if (shidtDetailsList?.Count > 0)
                    {
                        foreach (HolidayShift shiftDetails in shidtDetailsList)
                        {
                            _holidayShiftRepository.Delete(shiftDetails);
                        }
                        await _holidayShiftRepository.SaveChangesAsync();
                    }
                    //Delete Holiday Location
                    var locationList = _holidayLocationRepository.GetHolidayLocationDetailsByHolidayId(pHolidayID);
                    if (locationList?.Count > 0)
                    {
                        foreach (HolidayLocation location in locationList)
                        {
                            _holidayLocationRepository.Delete(location);
                        }
                        await _holidayLocationRepository.SaveChangesAsync();
                    }
                    //Delete Holiday Department
                    var departmentList = _holidayDepartmentRepository.GetHolidayDepartmentDetailsByHolidayId(pHolidayID);
                    if (departmentList?.Count > 0)
                    {
                        foreach (HolidayDepartment department in departmentList)
                        {
                            _holidayDepartmentRepository.Delete(department);
                        }
                        await _holidayDepartmentRepository.SaveChangesAsync();
                    }
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

        #region Get All Holidays
        public List<HolidayView> GetAllHolidays()
        {
            return _holidayRepository.GetAllHolidays();
        }
        #endregion

        #region Get Holiday Detail By Id
        public HolidayDetailView GetByHolidayID(int pHolidayID)
        {
            HolidayDetailView holidayDetail = _holidayRepository.GetByHolidayID(pHolidayID);
            holidayDetail.ShiftDetailId = _holidayShiftRepository.GetHolidayShiftByHolidayId(pHolidayID);
            holidayDetail.LocationId = _holidayLocationRepository.GetHolidayLocationByHolidayId(pHolidayID);
            holidayDetail.DepartmentId = _holidayDepartmentRepository.GetHolidayDepartmentByHolidayId(pHolidayID);
            return holidayDetail;
        }
        #endregion

        #region Get All Upcoming Holidays
        public HolidayDetailsView GetUpcomingHolidays(WeekMonthAttendanceView employeeDetails)
        {
            return _holidayRepository.GetUpcomingHolidays(employeeDetails.DepartmentId, employeeDetails.LocationId, employeeDetails.DOJ, employeeDetails.FromDate, employeeDetails.ToDate.AddYears(1));
        }
        #endregion

        #region Add or Update Leave
        public async Task<LeaveTypesDetailView> AddorUpdateLeave(LeaveDetailsView leaveDetailsView)
        {
            try
            {
                int leaveDetailId = 0;
                LeaveTypesDetailView previousLeaveDetails = new LeaveTypesDetailView();
                if (_leaveRepository.GetByleaveType(leaveDetailsView.LeaveType, leaveDetailsView.LeaveTypeId))
                {
                    previousLeaveDetails.LeaveTypeId = -1;
                    return previousLeaveDetails;
                }
                LeaveTypes leaveDetails = new();
                if (leaveDetailsView?.LeaveTypeId != 0)
                {
                    leaveDetails = _leaveRepository.GetByleaveId(leaveDetailsView.LeaveTypeId);
                    previousLeaveDetails.LeaveTypeId = leaveDetailsView.LeaveTypeId;
                    previousLeaveDetails.LeaveAccruedNoOfDays = leaveDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays;
                    previousLeaveDetails.EffectiveFromDate = leaveDetails?.EffectiveFromDate;
                    previousLeaveDetails.EffectiveToDate = leaveDetails?.EffectiveToDate;
                }

                leaveDetails.LeaveType = leaveDetailsView.LeaveType;
                leaveDetails.LeaveCode = leaveDetailsView.LeaveCode;
                leaveDetails.LeaveDescription = leaveDetailsView.LeaveDescription;
                leaveDetails.LeaveAccruedDay = leaveDetailsView.LeaveAccruedDay;
                leaveDetails.LeaveAccruedType = leaveDetailsView.LeaveAccruedType;
                leaveDetails.LeaveAccruedNoOfDays = leaveDetailsView.LeaveAccruedNoOfDays;
                //leaveDetails.IsActive = leaveDetailsView.IsActive;
                leaveDetails.LeaveTypesId = leaveDetailsView.LeaveTypesId;
                leaveDetails.ProRate = leaveDetailsView.ProRate;
                leaveDetails.EffectiveFromDate = leaveDetailsView.EffectiveFromDate;
                if (leaveDetails?.LeaveTypeId > 0 && leaveDetailsView.EffectiveToDate.HasValue && leaveDetails?.EffectiveToDate != leaveDetailsView?.EffectiveToDate)
                {
                    bool IsSuccess = await DeleteLeavesIfToDateIsConfigured(leaveDetailsView.EffectiveToDate.Value, leaveDetails.LeaveTypeId, leaveDetailsView.CreatedBy);
                }

                leaveDetails.EffectiveToDate = leaveDetailsView.EffectiveToDate;
                leaveDetails.AllowTimesheet = leaveDetailsView.AllowTimesheet;
                leaveDetails.BalanceBasedOn = leaveDetailsView.BalanceBasedOn;
                if (leaveDetails?.LeaveTypeId == 0)
                {
                    leaveDetails.IsActive = false;
                    leaveDetails.CreatedOn = DateTime.UtcNow;
                    leaveDetails.CreatedBy = leaveDetailsView.CreatedBy;
                    leaveDetails.IsActive = true;
                    await _leaveRepository.AddAsync(leaveDetails);
                    await _leaveRepository.SaveChangesAsync();
                    leaveDetailId = leaveDetails.LeaveTypeId;
                }
                else
                {
                    leaveDetails.ModifiedOn = DateTime.UtcNow;
                    leaveDetails.ModifiedBy = leaveDetailsView.CreatedBy;
                    _leaveRepository.Update(leaveDetails);
                    leaveDetailId = leaveDetails.LeaveTypeId;
                    await _leaveRepository.SaveChangesAsync();
                }
                previousLeaveDetails.LeaveTypeId = leaveDetailId;
                if (leaveDetailsView?.ProRateMonthList != null && leaveDetailsView?.ProRateMonthList?.Count > 0 && leaveDetailId != 0)
                {
                    var proRateList = _proRateMonthDetailsRepository.GetByID(leaveDetailsView.LeaveTypeId);
                    if (proRateList?.Count > 0)
                    {
                        foreach (ProRateMonthDetails proRateMonth in proRateList)
                        {
                            _proRateMonthDetailsRepository.Delete(proRateMonth);
                            await _proRateMonthDetailsRepository.SaveChangesAsync();
                        }
                    }

                    foreach (ProRateMonthList proRateMonth in leaveDetailsView?.ProRateMonthList)
                    {
                        ProRateMonthDetails proRateMonthDetails = new();
                        proRateMonthDetails.LeaveTypeId = leaveDetailId;
                        proRateMonthDetails.Fromday = proRateMonth.Fromday;
                        proRateMonthDetails.Today = proRateMonth.Today;
                        proRateMonthDetails.Count = proRateMonth.Count;
                        proRateMonthDetails.CreatedOn = DateTime.UtcNow;
                        proRateMonthDetails.CreatedBy = leaveDetailsView.CreatedBy;
                        await _proRateMonthDetailsRepository.AddAsync(proRateMonthDetails);
                        await _proRateMonthDetailsRepository.SaveChangesAsync();
                    }

                }
                if (leaveDetailsView.LeaveEntitlement != null)
                {
                    LeaveEntitlement Entitlement = new();
                    if (leaveDetailsView?.LeaveTypeId != 0) Entitlement = _leaveEntitlementRepository.GetEntitlementByLeaveId(leaveDetailsView.LeaveTypeId);
                    if (Entitlement != null)
                    {
                        Entitlement.MaxLeaveAvailedYearId = leaveDetailsView.LeaveEntitlement.MaxLeaveAvailedYearId;
                        Entitlement.MaxLeaveAvailedDays = leaveDetailsView.LeaveEntitlement.MaxLeaveAvailedDays;
                        Entitlement.CarryForwardId = leaveDetailsView.LeaveEntitlement.CarryForwardId;
                        Entitlement.MaximumCarryForwardDays = leaveDetailsView.LeaveEntitlement.MaximumCarryForwardDays;
                        Entitlement.ReimbursementId = leaveDetailsView.LeaveEntitlement.ReimbursementId;
                        Entitlement.MaximumReimbursementDays = leaveDetailsView.LeaveEntitlement.MaximumReimbursementDays;
                        Entitlement.ResetYear = leaveDetailsView.LeaveEntitlement.ResetYear;
                        Entitlement.ResetMonth = leaveDetailsView.LeaveEntitlement.ResetMonth;
                        Entitlement.ResetDay = leaveDetailsView.LeaveEntitlement.ResetDay;
                        if (leaveDetailsView?.LeaveTypeId == 0)
                        {
                            Entitlement.CreatedOn = DateTime.UtcNow;
                            Entitlement.CreatedBy = leaveDetailsView.CreatedBy;
                            Entitlement.LeaveTypeId = leaveDetailId;
                            await _leaveEntitlementRepository.AddAsync(Entitlement);
                        }
                        else
                        {
                            Entitlement.ModifiedOn = DateTime.UtcNow;
                            Entitlement.ModifiedBy = leaveDetailsView.CreatedBy;
                            Entitlement.LeaveTypeId = leaveDetailId;
                            _leaveEntitlementRepository.Update(Entitlement);
                        }
                        await _leaveEntitlementRepository.SaveChangesAsync();
                    }
                }
                if (leaveDetailsView.LeaveApplicable != null)
                {
                    LeaveApplicable Addleaveapplicable = new();
                    if (leaveDetailsView?.LeaveTypeId != 0) Addleaveapplicable = _leaveApplicableRepository.GetleaveApplicableByLeaveId(leaveDetailsView.LeaveTypeId);
                    if (Addleaveapplicable != null)
                    {
                        Addleaveapplicable.Gender_Male = leaveDetailsView.LeaveApplicable.Gender_Male;
                        Addleaveapplicable.Gender_Female = leaveDetailsView.LeaveApplicable.Gender_Female;
                        Addleaveapplicable.Gender_Others = leaveDetailsView.LeaveApplicable.Gender_Others;
                        Addleaveapplicable.MaritalStatus_Single = leaveDetailsView.LeaveApplicable.MaritalStatus_Single;
                        Addleaveapplicable.MaritalStatus_Married = leaveDetailsView.LeaveApplicable.MaritalStatus_Married;
                        Addleaveapplicable.EmployeeTypeId = leaveDetailsView.LeaveApplicable.EmployeeTypeId;
                        Addleaveapplicable.ProbationStatus = leaveDetailsView.LeaveApplicable.ProbationStatus;
                        Addleaveapplicable.Type = "LeaveApplicable";
                        if (leaveDetailsView?.LeaveTypeId == 0)
                        {
                            Addleaveapplicable.CreatedOn = DateTime.UtcNow;
                            Addleaveapplicable.CreatedBy = leaveDetailsView.CreatedBy;
                            Addleaveapplicable.LeaveTypeId = leaveDetailId;
                            await _leaveApplicableRepository.AddAsync(Addleaveapplicable);
                        }
                        else
                        {
                            Addleaveapplicable.ModifiedOn = DateTime.UtcNow;
                            Addleaveapplicable.ModifiedBy = leaveDetailsView.CreatedBy;
                            Addleaveapplicable.LeaveTypeId = leaveDetailId;
                            _leaveApplicableRepository.Update(Addleaveapplicable);
                            await _leaveApplicableRepository.SaveChangesAsync();
                            // Delete Employee
                            var employeeList = _employeeApplicableLeaveRepository.GetEmployeeApplicableLeaveDetailsByLeaveTypeId(leaveDetailsView.LeaveTypeId);
                            if (employeeList?.Count > 0)
                            {
                                foreach (EmployeeApplicableLeave employee in employeeList)
                                {
                                    _employeeApplicableLeaveRepository.Delete(employee);
                                    await _employeeApplicableLeaveRepository.SaveChangesAsync();
                                }
                            }
                            // delete Leave Department
                            var departmentList = _leaveDepartmentRepository.GetLeaveApplicableDepartmentDetailsByLeaveTypeId(leaveDetailsView.LeaveTypeId);
                            if (departmentList?.Count > 0)
                            {
                                foreach (LeaveDepartment department in departmentList)
                                {
                                    _leaveDepartmentRepository.Delete(department);
                                    await _leaveDepartmentRepository.SaveChangesAsync();
                                }
                            }
                            // delete Leave Designation
                            var designationList = _leaveDesignationRepository.GetLeaveApplicableDesignationDetailsByLeaveTypeId(leaveDetailsView.LeaveTypeId);
                            if (designationList?.Count > 0)
                            {
                                foreach (LeaveDesignation designation in designationList)
                                {
                                    _leaveDesignationRepository.Delete(designation);
                                    await _leaveDesignationRepository.SaveChangesAsync();
                                }
                            }
                            // delete Leave Location
                            var locationList = _leaveLocationRepository.GetLeaveApplicableLocationDetailsByLeaveTypeId(leaveDetailsView.LeaveTypeId);
                            if (locationList?.Count > 0)
                            {
                                foreach (LeaveLocation location in locationList)
                                {
                                    _leaveLocationRepository.Delete(location);
                                    await _leaveDesignationRepository.SaveChangesAsync();
                                }
                            }
                            // delete Leave Role
                            var roleList = _leaveRoleRepository.GetLeaveApplicableRoleDetailsByLeaveTypeId(leaveDetailsView.LeaveTypeId);
                            if (roleList?.Count > 0)
                            {
                                foreach (LeaveRole role in roleList)
                                {
                                    _leaveRoleRepository.Delete(role);
                                    await _leaveRoleRepository.SaveChangesAsync();
                                }
                            }
                            // delete Leave Employee Type
                            var employeeTypeList = _leaveEmployeeTypeRepository.GetLeaveEmployeeTypeDetailsByLeaveTypeId(leaveDetailsView.LeaveTypeId);
                            if (employeeTypeList?.Count > 0)
                            {
                                foreach (LeaveEmployeeType employeeType in employeeTypeList)
                                {
                                    _leaveEmployeeTypeRepository.Delete(employeeType);
                                    await _leaveEmployeeTypeRepository.SaveChangesAsync();
                                }
                            }
                            // delete Leave Probation Status
                            var probationStatusList = _leaveProbationStatuRepository.GetLeaveProbationStatusDetailsByLeaveTypeId(leaveDetailsView.LeaveTypeId);
                            if (probationStatusList?.Count > 0)
                            {
                                foreach (LeaveProbationStatus probationStatus in probationStatusList)
                                {
                                    _leaveProbationStatuRepository.Delete(probationStatus);
                                    await _leaveProbationStatuRepository.SaveChangesAsync();
                                }
                            }
                        }
                        // Add Leave Exception
                        if (leaveDetailsView?.LeaveException != null)
                        {
                            LeaveApplicable leaveException = new();
                            if (leaveDetailsView?.LeaveTypeId != 0) leaveException = _leaveApplicableRepository.GetleaveExceptionByLeaveId(leaveDetailsView.LeaveTypeId);
                            if (leaveException != null)
                            {
                                leaveException.LeaveTypeId = leaveDetailId;
                                leaveException.Gender_Male_Exception = leaveDetailsView.LeaveException.Gender_Male_Exception;
                                leaveException.Gender_Female_Exception = leaveDetailsView.LeaveException.Gender_Female_Exception;
                                leaveException.Gender_Others_Exception = leaveDetailsView.LeaveException.Gender_Others_Exception;
                                leaveException.MaritalStatus_Single_Exception = leaveDetailsView.LeaveException.MaritalStatus_Single_Exception;
                                leaveException.MaritalStatus_Married_Exception = leaveDetailsView.LeaveException.MaritalStatus_Married_Exception;
                                leaveException.Type = "LeaveException";
                                if (leaveDetailsView?.LeaveTypeId == 0)
                                {
                                    leaveException.CreatedOn = DateTime.UtcNow;
                                    leaveException.CreatedBy = leaveDetailsView.CreatedBy;
                                    leaveException.LeaveTypeId = leaveDetailId;
                                    await _leaveApplicableRepository.AddAsync(leaveException);
                                }
                                else
                                {
                                    leaveException.ModifiedOn = DateTime.UtcNow;
                                    leaveException.ModifiedBy = leaveDetailsView.CreatedBy;
                                    leaveException.LeaveTypeId = leaveDetailId;
                                    _leaveApplicableRepository.Update(leaveException);
                                }
                                await _leaveApplicableRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Applicable Department
                        if (leaveDetailsView?.LeaveApplicableDepartmentId?.Count > 0)
                        {
                            foreach (int leaveApplicableDepartmentId in leaveDetailsView?.LeaveApplicableDepartmentId)
                            {
                                LeaveDepartment applicableDepartment = new();
                                applicableDepartment.LeaveTypeId = leaveDetailId;
                                applicableDepartment.LeaveApplicableDepartmentId = leaveApplicableDepartmentId;
                                applicableDepartment.LeaveExceptionDepartmentId = 0;
                                applicableDepartment.CreatedOn = DateTime.UtcNow;
                                applicableDepartment.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveDepartmentRepository.AddAsync(applicableDepartment);
                                await _leaveDepartmentRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Exception Department
                        if (leaveDetailsView?.LeaveExceptionDepartmentId?.Count > 0)
                        {
                            foreach (int leaveExceptionDepartmentId in leaveDetailsView?.LeaveExceptionDepartmentId)
                            {
                                LeaveDepartment exceptionDepartment = new();
                                exceptionDepartment.LeaveTypeId = leaveDetailId;
                                exceptionDepartment.LeaveExceptionDepartmentId = leaveExceptionDepartmentId;
                                exceptionDepartment.LeaveApplicableDepartmentId = 0;
                                exceptionDepartment.CreatedOn = DateTime.UtcNow;
                                exceptionDepartment.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveDepartmentRepository.AddAsync(exceptionDepartment);
                                await _leaveDepartmentRepository.SaveChangesAsync();
                            }
                        }
                        //Add Leave Applicable Designation
                        if (leaveDetailsView?.LeaveApplicableDesginationId?.Count > 0)
                        {
                            foreach (int leaveApplicableDesignationId in leaveDetailsView?.LeaveApplicableDesginationId)
                            {
                                LeaveDesignation applicableDesignation = new();
                                applicableDesignation.LeaveTypeId = leaveDetailId;
                                applicableDesignation.LeaveApplicableDesignationId = leaveApplicableDesignationId;
                                applicableDesignation.LeaveExceptionDesignationId = 0;
                                applicableDesignation.CreatedOn = DateTime.UtcNow;
                                applicableDesignation.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveDesignationRepository.AddAsync(applicableDesignation);
                                await _leaveDesignationRepository.SaveChangesAsync();
                            }
                        }
                        //Add Leave Exception Designation
                        if (leaveDetailsView?.LeaveExceptionDesginationId?.Count > 0)
                        {
                            foreach (int leaveExecptionDesignationId in leaveDetailsView?.LeaveExceptionDesginationId)
                            {
                                LeaveDesignation exceptionDesignation = new();
                                exceptionDesignation.LeaveTypeId = leaveDetailId;
                                exceptionDesignation.LeaveExceptionDesignationId = leaveExecptionDesignationId;
                                exceptionDesignation.LeaveApplicableDesignationId = 0;
                                exceptionDesignation.CreatedOn = DateTime.UtcNow;
                                exceptionDesignation.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveDesignationRepository.AddAsync(exceptionDesignation);
                                await _leaveDesignationRepository.SaveChangesAsync();
                            }
                        }
                        //Add Leave Applicable Location
                        if (leaveDetailsView?.LeaveApplicableLocationId?.Count > 0)
                        {
                            foreach (int leaveApplicableLocationId in leaveDetailsView?.LeaveApplicableLocationId)
                            {
                                LeaveLocation applicableLocation = new();
                                applicableLocation.LeaveTypeId = leaveDetailId;
                                applicableLocation.LeaveApplicableLocationId = leaveApplicableLocationId;
                                applicableLocation.LeaveExceptionLocationId = 0;
                                applicableLocation.CreatedOn = DateTime.UtcNow;
                                applicableLocation.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveLocationRepository.AddAsync(applicableLocation);
                                await _leaveLocationRepository.SaveChangesAsync();
                            }
                        }
                        //Add Leave Exception Location
                        if (leaveDetailsView?.LeaveExceptionLocationId?.Count > 0)
                        {
                            foreach (int leaveExceptionLocationId in leaveDetailsView?.LeaveExceptionLocationId)
                            {
                                LeaveLocation exceptionLocation = new();
                                exceptionLocation.LeaveTypeId = leaveDetailId;
                                exceptionLocation.LeaveExceptionLocationId = leaveExceptionLocationId;
                                exceptionLocation.LeaveApplicableLocationId = 0;
                                exceptionLocation.CreatedOn = DateTime.UtcNow;
                                exceptionLocation.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveLocationRepository.AddAsync(exceptionLocation);
                                await _leaveLocationRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Applicable Role
                        if (leaveDetailsView?.LeaveApplicableRoleId?.Count > 0)
                        {
                            foreach (int leaveApplicableRoleId in leaveDetailsView?.LeaveApplicableRoleId)
                            {
                                LeaveRole applicableRole = new();
                                applicableRole.LeaveTypeId = leaveDetailId;
                                applicableRole.LeaveApplicableRoleId = leaveApplicableRoleId;
                                applicableRole.LeaveExceptionRoleId = 0;
                                applicableRole.CreatedOn = DateTime.UtcNow;
                                applicableRole.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveRoleRepository.AddAsync(applicableRole);
                                await _leaveRoleRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Exception Role
                        if (leaveDetailsView?.LeaveExceptionRoleId?.Count > 0)
                        {
                            foreach (int leaveExceptionRoleId in leaveDetailsView?.LeaveExceptionRoleId)
                            {
                                LeaveRole exceptionRole = new();
                                exceptionRole.LeaveTypeId = leaveDetailId;
                                exceptionRole.LeaveExceptionRoleId = leaveExceptionRoleId;
                                exceptionRole.LeaveApplicableRoleId = 0;
                                exceptionRole.CreatedOn = DateTime.UtcNow;
                                exceptionRole.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveRoleRepository.AddAsync(exceptionRole);
                                await _leaveRoleRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Applicable Employee
                        if (leaveDetailsView?.LeaveApplicableEmployeeId?.Count > 0)
                        {
                            foreach (int leaveApplicableEmployeeId in leaveDetailsView?.LeaveApplicableEmployeeId)
                            {
                                EmployeeApplicableLeave employeeApplicable = new();
                                employeeApplicable.LeaveTypeId = leaveDetailId;
                                employeeApplicable.EmployeeId = leaveApplicableEmployeeId;
                                employeeApplicable.LeaveExceptionEmployeeId = 0;
                                employeeApplicable.CreatedOn = DateTime.UtcNow;
                                employeeApplicable.CreatedBy = leaveDetailsView.CreatedBy;
                                await _employeeApplicableLeaveRepository.AddAsync(employeeApplicable);
                                await _leaveDepartmentRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Exception Employee 
                        if (leaveDetailsView?.LeaveExceptionEmployeeId?.Count > 0)
                        {
                            foreach (int leaveExceptionEmployeeId in leaveDetailsView?.LeaveExceptionEmployeeId)
                            {
                                EmployeeApplicableLeave exceptionEmployee = new();
                                exceptionEmployee.LeaveTypeId = leaveDetailId;
                                exceptionEmployee.LeaveExceptionEmployeeId = leaveExceptionEmployeeId;
                                exceptionEmployee.EmployeeId = 0;
                                exceptionEmployee.CreatedOn = DateTime.UtcNow;
                                exceptionEmployee.CreatedBy = leaveDetailsView.CreatedBy;
                                await _employeeApplicableLeaveRepository.AddAsync(exceptionEmployee);
                                await _employeeApplicableLeaveRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Applicable Employee Type
                        if (leaveDetailsView?.LeaveEmployeeTypeId?.Count > 0)
                        {
                            foreach (int leaveEmployeeTypeId in leaveDetailsView?.LeaveEmployeeTypeId)
                            {
                                LeaveEmployeeType employeeType = new();
                                employeeType.LeaveTypeId = leaveDetailId;
                                employeeType.LeaveApplicableEmployeeTypeId = leaveEmployeeTypeId;
                                employeeType.LeaveExceptionEmployeeTypeId = 0;
                                employeeType.CreatedOn = DateTime.UtcNow;
                                employeeType.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveEmployeeTypeRepository.AddAsync(employeeType);
                                await _leaveEmployeeTypeRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Exception Employee Type
                        if (leaveDetailsView?.LeaveExceptionEmployeeTypeId?.Count > 0)
                        {
                            foreach (int leaveExceptionEmployeeTypeId in leaveDetailsView?.LeaveExceptionEmployeeTypeId)
                            {
                                LeaveEmployeeType exceptionEmployeeType = new();
                                exceptionEmployeeType.LeaveTypeId = leaveDetailId;
                                exceptionEmployeeType.LeaveExceptionEmployeeTypeId = leaveExceptionEmployeeTypeId;
                                exceptionEmployeeType.LeaveApplicableEmployeeTypeId = 0;
                                exceptionEmployeeType.CreatedOn = DateTime.UtcNow;
                                exceptionEmployeeType.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveEmployeeTypeRepository.AddAsync(exceptionEmployeeType);
                                await _leaveEmployeeTypeRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Probation Status
                        if (leaveDetailsView?.LeaveProbationStatusId?.Count > 0)
                        {
                            foreach (int leaveProbationStatusId in leaveDetailsView?.LeaveProbationStatusId)
                            {
                                LeaveProbationStatus probationStatus = new();
                                probationStatus.LeaveTypeId = leaveDetailId;
                                probationStatus.LeaveApplicableProbationStatus = leaveProbationStatusId;
                                probationStatus.LeaveExceptionProbationStatus = 0;
                                probationStatus.CreatedOn = DateTime.UtcNow;
                                probationStatus.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveProbationStatuRepository.AddAsync(probationStatus);
                                await _leaveProbationStatuRepository.SaveChangesAsync();
                            }
                        }
                        // Add Leave Exception Probation Status
                        if (leaveDetailsView?.LeaveExceptionProbationStatusId?.Count > 0)
                        {
                            foreach (int leaveExceptionProbationStatus in leaveDetailsView?.LeaveExceptionProbationStatusId)
                            {
                                LeaveProbationStatus exceptionProbationStatus = new();
                                exceptionProbationStatus.LeaveTypeId = leaveDetailId;
                                exceptionProbationStatus.LeaveExceptionProbationStatus = leaveExceptionProbationStatus;
                                exceptionProbationStatus.LeaveApplicableProbationStatus = 0;
                                exceptionProbationStatus.CreatedOn = DateTime.UtcNow;
                                exceptionProbationStatus.CreatedBy = leaveDetailsView.CreatedBy;
                                await _leaveProbationStatuRepository.AddAsync(exceptionProbationStatus);
                                await _leaveProbationStatuRepository.SaveChangesAsync();
                            }
                        }
                    }
                }
                if (leaveDetailsView.LeaveRestrictions != null)
                {
                    LeaveRestrictions AddLeaveRestrictions = new();
                    if (leaveDetailsView.LeaveTypeId != 0) AddLeaveRestrictions = _leaveRestrictionsRepository.GetRestrictionsByLeaveId(leaveDetailsView.LeaveTypeId);
                    if (AddLeaveRestrictions != null)
                    {
                        AddLeaveRestrictions.Weekendsbetweenleaveperiod = leaveDetailsView.LeaveRestrictions.Weekendsbetweenleaveperiod;
                        AddLeaveRestrictions.Holidaybetweenleaveperiod = leaveDetailsView.LeaveRestrictions.Holidaybetweenleaveperiod;
                        AddLeaveRestrictions.ExceedLeaveBalance = leaveDetailsView.LeaveRestrictions.ExceedLeaveBalance;
                        AddLeaveRestrictions.AllowUsersViewId = leaveDetailsView.LeaveRestrictions.AllowUsersViewId;
                        AddLeaveRestrictions.BalanceDisplayedId = leaveDetailsView.LeaveRestrictions.BalanceDisplayedId;
                        AddLeaveRestrictions.DaysInAdvance = leaveDetailsView.LeaveRestrictions.DaysInAdvance;
                        AddLeaveRestrictions.AllowRequestDates = leaveDetailsView.LeaveRestrictions.AllowRequestDates;
                        AddLeaveRestrictions.AllowRequestNextDays = leaveDetailsView.LeaveRestrictions.AllowRequestNextDays;
                        AddLeaveRestrictions.DatesAppliedAdvance = leaveDetailsView.LeaveRestrictions.DatesAppliedAdvance;
                        AddLeaveRestrictions.MaximumLeavePerApplication = leaveDetailsView.LeaveRestrictions.MaximumLeavePerApplication;
                        AddLeaveRestrictions.MinimumGapTwoApplication = leaveDetailsView.LeaveRestrictions.MinimumGapTwoApplication;
                        AddLeaveRestrictions.MaximumConsecutiveDays = leaveDetailsView.LeaveRestrictions.MaximumConsecutiveDays;
                        AddLeaveRestrictions.EnableFileUpload = leaveDetailsView.LeaveRestrictions.EnableFileUpload;
                        AddLeaveRestrictions.MinimumNoOfApplicationsPeriod = leaveDetailsView.LeaveRestrictions.MinimumNoOfApplicationsPeriod;
                        AddLeaveRestrictions.AllowRequestPeriodId = leaveDetailsView.LeaveRestrictions.AllowRequestPeriodId;
                        AddLeaveRestrictions.MaximumLeave = leaveDetailsView.LeaveRestrictions.MaximumLeave;
                        AddLeaveRestrictions.MinimumGap = leaveDetailsView.LeaveRestrictions.MinimumGap;
                        AddLeaveRestrictions.MaximumConsecutive = leaveDetailsView.LeaveRestrictions.MaximumConsecutive;
                        AddLeaveRestrictions.EnableFile = leaveDetailsView.LeaveRestrictions.EnableFile;
                        AddLeaveRestrictions.CannotBeTakenTogether = leaveDetailsView.LeaveRestrictions.CannotBeTakenTogether;
                        AddLeaveRestrictions.AllowPastDates = leaveDetailsView.LeaveRestrictions.AllowPastDates;
                        AddLeaveRestrictions.AllowFutureDates = leaveDetailsView.LeaveRestrictions.AllowFutureDates;
                        AddLeaveRestrictions.IsAllowRequestNextDays = leaveDetailsView.LeaveRestrictions.IsAllowRequestNextDays;
                        AddLeaveRestrictions.IsToBeApplied = leaveDetailsView.LeaveRestrictions.IsToBeApplied;
                        AddLeaveRestrictions.GrantMinimumNoOfRequestDay = leaveDetailsView.LeaveRestrictions.GrantMinimumNoOfRequestDay;
                        AddLeaveRestrictions.GrantMaximumNoOfRequestDay = leaveDetailsView.LeaveRestrictions.GrantMaximumNoOfRequestDay;
                        AddLeaveRestrictions.GrantMaximumNoOfPeriod = leaveDetailsView.LeaveRestrictions.GrantMaximumNoOfPeriod;
                        AddLeaveRestrictions.GrantMaximumNoOfDay = leaveDetailsView.LeaveRestrictions.GrantMaximumNoOfDay;
                        AddLeaveRestrictions.GrantMinimumGapTwoApplicationDay = leaveDetailsView.LeaveRestrictions.GrantMinimumGapTwoApplicationDay;
                        AddLeaveRestrictions.GrantUploadDocumentSpecificPeriodDay = leaveDetailsView.LeaveRestrictions.GrantUploadDocumentSpecificPeriodDay;
                        AddLeaveRestrictions.IsGrantRequestPastDay = leaveDetailsView.LeaveRestrictions.IsGrantRequestPastDay;
                        AddLeaveRestrictions.GrantRequestPastDay = leaveDetailsView.LeaveRestrictions.GrantRequestPastDay;
                        AddLeaveRestrictions.IsGrantRequestFutureDay = leaveDetailsView.LeaveRestrictions.IsGrantRequestFutureDay;
                        AddLeaveRestrictions.GrantRequestFutureDay = leaveDetailsView.LeaveRestrictions.GrantRequestFutureDay;
                        AddLeaveRestrictions.GrantResetLeaveAfterDays = leaveDetailsView.LeaveRestrictions.GrantResetLeaveAfterDays;
                        AddLeaveRestrictions.ToBeAdvanced = leaveDetailsView.LeaveRestrictions.ToBeAdvanced;
                        if (leaveDetailsView?.LeaveTypeId == 0)
                        {
                            AddLeaveRestrictions.CreatedOn = DateTime.UtcNow;
                            AddLeaveRestrictions.CreatedBy = leaveDetailsView.CreatedBy;
                            AddLeaveRestrictions.LeaveTypeId = leaveDetailId;
                            await _leaveRestrictionsRepository.AddAsync(AddLeaveRestrictions);
                        }
                        else
                        {
                            AddLeaveRestrictions.ModifiedOn = DateTime.UtcNow;
                            AddLeaveRestrictions.ModifiedBy = leaveDetailsView.CreatedBy;
                            AddLeaveRestrictions.LeaveTypeId = leaveDetailId;
                            _leaveRestrictionsRepository.Update(AddLeaveRestrictions);
                        }
                        await _leaveRestrictionsRepository.SaveChangesAsync();
                        if (leaveDetailsView?.LeaveRestrictions?.DurationsAllowedId?.Count > 0)
                        {
                            // delete Leave Duration
                            var leaveDurationList = _leaveDurationRepository.GetByID(leaveDetailsView.LeaveTypeId);
                            if (leaveDurationList?.Count > 0)
                            {
                                foreach (LeaveDuration duration in leaveDurationList)
                                {
                                    _leaveDurationRepository.Delete(duration);
                                    await _leaveDurationRepository.SaveChangesAsync();
                                }
                            }
                            List<DurationAllowed> durationAllowed = new();
                            durationAllowed = leaveDetailsView?.LeaveRestrictions?.DurationsAllowedId;
                            foreach (var durationid in durationAllowed)
                            {
                                LeaveDuration leaveDuration = new();
                                leaveDuration.DurationId = durationid.DurationId;
                                leaveDuration.LeaveTypeId = leaveDetailId;
                                leaveDuration.CreatedOn = DateTime.UtcNow;
                                leaveDuration.CreatedBy = leaveDetailsView.CreatedBy;
                                leaveDuration.ModifiedOn = null;
                                leaveDuration.ModifiedBy = null;
                                await _leaveDurationRepository.AddAsync(leaveDuration);
                                await _leaveDurationRepository.SaveChangesAsync();
                            }
                        }
                        var activeLeaveAndHolidayList = _leaveTakenTogetherRepository.GetByID(leaveDetailsView.LeaveTypeId);
                        if (activeLeaveAndHolidayList?.Count > 0)
                        {
                            foreach (LeaveTakenTogether leaveTakenList in activeLeaveAndHolidayList)
                            {
                                _leaveTakenTogetherRepository.Delete(leaveTakenList);
                                await _leaveTakenTogetherRepository.SaveChangesAsync();
                            }
                        }
                        var leaveAndHolidayList = _leaveTakenTogetherRepository.GetByLeaveType(leaveDetailsView.LeaveTypeId);
                        if (leaveAndHolidayList?.Count > 0)
                        {
                            foreach (LeaveTakenTogether leaveHolidayList in leaveAndHolidayList)
                            {
                                _leaveTakenTogetherRepository.Delete(leaveHolidayList);
                                await _leaveTakenTogetherRepository.SaveChangesAsync();
                            }
                        }
                        if (leaveDetailsView.LeaveRestrictions.activeLeaveType.Count > 0)
                        {
                            List<ActiveLeaveList> activeLeaveList = new();
                            activeLeaveList = leaveDetailsView?.LeaveRestrictions?.activeLeaveType;
                            foreach (var leaveList in activeLeaveList)
                            {
                                LeaveTakenTogether leaveTakenTogether = new();
                                leaveTakenTogether.LeaveTypeId = leaveDetailId;
                                leaveTakenTogether.LeaveOrHolidayId = leaveList?.leaveTypeId;
                                leaveTakenTogether.LeaveTakenType = "Leave";
                                leaveTakenTogether.CreatedOn = DateTime.UtcNow;
                                leaveTakenTogether.CreatedBy = leaveDetailsView.CreatedBy;
                                leaveTakenTogether.ModifiedOn = null;
                                leaveTakenTogether.ModifiedBy = null;
                                await _leaveTakenTogetherRepository.AddAsync(leaveTakenTogether);
                                await _leaveTakenTogetherRepository.SaveChangesAsync();
                            }
                        }
                        if (leaveDetailsView.LeaveRestrictions.activeLeaveType.Count > 0)
                        {
                            List<ActiveLeaveList> activeLeaveList = new();
                            activeLeaveList = leaveDetailsView?.LeaveRestrictions?.activeLeaveType;
                            foreach (var leaveList in activeLeaveList)
                            {
                                LeaveTakenTogether leaveTakenTogether = new();
                                leaveTakenTogether.LeaveTypeId = leaveList?.leaveTypeId == null ? 0 : (int)leaveList?.leaveTypeId;
                                leaveTakenTogether.LeaveOrHolidayId = leaveDetailId;
                                leaveTakenTogether.LeaveTakenType = "Leave";
                                leaveTakenTogether.CreatedOn = DateTime.UtcNow;
                                leaveTakenTogether.CreatedBy = leaveDetailsView.CreatedBy;
                                leaveTakenTogether.ModifiedOn = null;
                                leaveTakenTogether.ModifiedBy = null;
                                await _leaveTakenTogetherRepository.AddAsync(leaveTakenTogether);
                                await _leaveTakenTogetherRepository.SaveChangesAsync();
                            }
                        }
                        if (leaveDetailsView?.LeaveRestrictions?.activeHoliday?.Count > 0)
                        {
                            List<ActiveHolidayList> activeHolidayList = new();
                            activeHolidayList = leaveDetailsView?.LeaveRestrictions?.activeHoliday;
                            foreach (var leaveList in activeHolidayList)
                            {
                                LeaveTakenTogether leaveTakenTogether = new()
                                {
                                    LeaveTypeId = leaveDetailId,
                                    LeaveOrHolidayId = leaveList?.holidayID,
                                    LeaveTakenType = "Holiday",
                                    CreatedOn = DateTime.UtcNow,
                                    CreatedBy = leaveDetailsView?.CreatedBy,
                                    ModifiedOn = null,
                                    ModifiedBy = null
                                };
                                await _leaveTakenTogetherRepository.AddAsync(leaveTakenTogether);
                                await _leaveTakenTogetherRepository.SaveChangesAsync();
                            }
                        }
                        var specificEmployeeLeaveDetails = _specificEmployeeDetailLeaveRepository.GetByID(leaveDetailsView.LeaveTypeId);
                        if (specificEmployeeLeaveDetails?.Count > 0)
                        {
                            foreach (SpecificEmployeeDetailLeave specificEmployeeDetailLeaveList in specificEmployeeLeaveDetails)
                            {
                                _specificEmployeeDetailLeaveRepository.Delete(specificEmployeeDetailLeaveList);
                                await _specificEmployeeDetailLeaveRepository.SaveChangesAsync();
                            }
                        }
                        if (leaveDetailsView.LeaveRestrictions.SpecificEmployeeDetailLeaveList?.Count > 0)
                        {
                            List<SpecificEmployeeDetailLeaveList> specificEmployeeDetailList = new();
                            specificEmployeeDetailList = leaveDetailsView?.LeaveRestrictions?.SpecificEmployeeDetailLeaveList;
                            foreach (var specificemployeeleaveList in specificEmployeeDetailList)
                            {
                                SpecificEmployeeDetailLeave specificEmployeeDetailLeave = new();
                                specificEmployeeDetailLeave.LeaveTypeId = leaveDetailId;
                                specificEmployeeDetailLeave.EmployeeDetailLeaveId = specificemployeeleaveList?.EmployeeDetailLeaveId;
                                specificEmployeeDetailLeave.CreatedOn = DateTime.UtcNow;
                                specificEmployeeDetailLeave.CreatedBy = leaveDetailsView?.CreatedBy;
                                specificEmployeeDetailLeave.ModifiedOn = null;
                                specificEmployeeDetailLeave.ModifiedBy = null;
                                await _specificEmployeeDetailLeaveRepository.AddAsync(specificEmployeeDetailLeave);
                                await _specificEmployeeDetailLeaveRepository.SaveChangesAsync();
                            }
                        }
                    }
                }
                //Grand Leave approval 
                if (leaveDetailsView?.LeaveTypeId > 0)
                {
                    List<GrantLeaveApproval> grantLeaveList = _grantLeaveApprovalRepository.GetGrantLeaveApprovalByLeaveTypeId(leaveDetailId);
                    if (grantLeaveList?.Count > 0)
                    {
                        foreach (GrantLeaveApproval grantLeave in grantLeaveList)
                        {
                            _grantLeaveApprovalRepository.Delete(grantLeave);
                            await _grantLeaveApprovalRepository.SaveChangesAsync();
                        }
                    }
                }
                if (leaveDetailsView?.GrantLeaveApprovalList?.Count > 0)
                {
                    foreach (GrantLeaveApprovalView grantLeave in leaveDetailsView?.GrantLeaveApprovalList)
                    {
                        if (leaveDetailId > 0 && grantLeave?.LevelId > 0 && grantLeave?.LevelApprovalId > 0)
                        {
                            GrantLeaveApproval grantLeaveDetails = new();
                            grantLeaveDetails.LeaveTypeId = leaveDetailId;
                            grantLeaveDetails.LevelId = grantLeave?.LevelId;
                            grantLeaveDetails.LevelApprovalId = grantLeave?.LevelApprovalId;
                            grantLeaveDetails.LevelApprovalEmployeeId = grantLeave?.LevelApprovalEmployeeId;
                            grantLeaveDetails.CreatedOn = DateTime.UtcNow;
                            grantLeaveDetails.CreatedBy = leaveDetailsView?.CreatedBy;
                            await _grantLeaveApprovalRepository.AddAsync(grantLeaveDetails);
                            await _grantLeaveApprovalRepository.SaveChangesAsync();
                        }
                    }

                }
                //}
                return previousLeaveDetails;
                //}
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> DeleteLeavesIfToDateIsConfigured(DateTime effectiveToDate, int leaveTypeId, int? doneBy)
        {
            try
            {
                List<ApplyLeaves> appliedLeaves = _LeaveApplyRepository.GetAppliedleaveByTypeId(leaveTypeId);

                if (appliedLeaves == null || appliedLeaves?.Count == 0)
                    return true;

                List<ApplyLeaves> deletableAppliedLeaves = appliedLeaves?.Where(e => e.ToDate > effectiveToDate).ToList();
                if (deletableAppliedLeaves == null || deletableAppliedLeaves?.Count == 0)
                    return true;

                foreach (var leave in deletableAppliedLeaves)
                {
                    leave.IsActive = false;
                    leave.ModifiedBy = doneBy;
                    leave.ModifiedOn = DateTime.Now;
                    _LeaveApplyRepository.Update(leave);
                    await _LeaveApplyRepository.SaveChangesAsync();
                }
            }
            catch (Exception)
            {
                throw;
            }
            return true;
        }
        #endregion

        #region Get Leave Details By Leave Id
        public LeaveDetailsView GetLeaveDetailsByLeaveId(int leaveId)
        {
            try
            {
                LeaveDetailsView LeaveDetailsView = _leaveRepository.GetLeaveDetailsViewById(leaveId);
                LeaveDetailsView.LeaveEntitlement = _leaveRepository.GetLeaveEntitlementById(leaveId);
                LeaveDetailsView.LeaveApplicable = _leaveRepository.GetLeaveApplicableById(leaveId);
                LeaveDetailsView.LeaveRestrictions = _leaveRepository.GetLeaveRestrictionsById(leaveId);
                LeaveDetailsView.LeaveApplicableDepartmentId = _leaveDepartmentRepository.GetLeaveApplicableDepartmentByLeaveId(leaveId);
                LeaveDetailsView.LeaveExceptionDepartmentId = _leaveDepartmentRepository.GetLeaveExceptionDepartmentByLeaveId(leaveId);
                LeaveDetailsView.LeaveApplicableDesginationId = _leaveDesignationRepository.GetLeaveApplicableDesignationByLeaveId(leaveId);
                LeaveDetailsView.LeaveExceptionDesginationId = _leaveDesignationRepository.GetLeaveExceptionDesignationByLeaveId(leaveId);
                LeaveDetailsView.LeaveApplicableLocationId = _leaveLocationRepository.GetLeaveApplicableLocationByleaveId(leaveId);
                LeaveDetailsView.LeaveExceptionLocationId = _leaveLocationRepository.GetLeaveExceptionLocationByleaveId(leaveId);
                LeaveDetailsView.LeaveApplicableRoleId = _leaveRoleRepository.GetLeaveApplicableRoleByLeaveId(leaveId);
                LeaveDetailsView.LeaveExceptionRoleId = _leaveRoleRepository.GetLeaveExceptionRoleByLeaveId(leaveId);
                LeaveDetailsView.LeaveApplicableEmployeeId = _employeeApplicableLeaveRepository.GetEmployeeApplicableLeaveByLeaveId(leaveId);
                LeaveDetailsView.LeaveEmployeeTypeId = _leaveEmployeeTypeRepository.GetLeaveApplicableEmployeeTypeByLeaveId(leaveId);
                LeaveDetailsView.LeaveProbationStatusId = _leaveProbationStatuRepository.GetLeaveApplicableProbationStatusByLeaveId(leaveId);
                LeaveDetailsView.LeaveExceptionEmployeeTypeId = _leaveEmployeeTypeRepository.GetLeaveExceptionEmployeeTypeByLeaveId(leaveId);
                LeaveDetailsView.LeaveExceptionProbationStatusId = _leaveProbationStatuRepository.GetLeaveExceptionProbationStatusByLeaveId(leaveId);
                LeaveDetailsView.LeaveExceptionEmployeeId = _employeeApplicableLeaveRepository.GetLeaveExceptionEmployeeIdByLeaveId(leaveId);
                LeaveDetailsView.LeaveException = _leaveRepository.GetLeaveExceptionById(leaveId);
                List<GrantLeaveApproval> grantLeaveApproval = _grantLeaveApprovalRepository.GetGrantLeaveApprovalByLeaveTypeId(leaveId);
                LeaveDetailsView.GrantLeaveApprovalList = new List<GrantLeaveApprovalView>();
                if (grantLeaveApproval?.Count > 0)
                {
                    LeaveDetailsView.GrantLeaveApprovalList = grantLeaveApproval?.Select(x => new GrantLeaveApprovalView
                    {
                        LevelId = x.LevelId,
                        LevelApprovalId = x.LevelApprovalId,
                        LevelApprovalEmployeeId = x.LevelApprovalEmployeeId
                    }).OrderBy(x => x.LevelId).ToList();
                }
                return LeaveDetailsView;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region List All Leaves Details
        public List<LeaveMaxLimitAction> GetMaxlimitAction()
        {
            return _leaveRepository.GetMaxlimitAction();
        }
        #endregion

        #region Delete Leave
        public async Task<DocumnentsPathView> DeleteLeave(int pLeaveTypeId)
        {
            try
            {
                if (pLeaveTypeId > 0)
                {
                    DocumnentsPathView documnentsPath = new();
                    documnentsPath.requestDetails = new();
                    documnentsPath.LeaveId = new();
                    LeaveTypes leaveDetails = _leaveRepository.GetByID(pLeaveTypeId);
                    if (leaveDetails != null && leaveDetails?.LeaveTypeId > 0)
                    {
                        _leaveRepository.Delete(leaveDetails);
                        await _leaveRepository.SaveChangesAsync();
                    }
                    LeaveEntitlement leaveEntitlement = _leaveEntitlementRepository.GetByID(pLeaveTypeId);
                    if (leaveEntitlement != null && leaveEntitlement?.LeaveTypeId > 0)
                    {
                        _leaveEntitlementRepository.Delete(leaveEntitlement);
                        await _leaveEntitlementRepository.SaveChangesAsync();
                    }
                    LeaveApplicable leaveApplicable = _leaveApplicableRepository.GetByID(pLeaveTypeId);
                    if (leaveApplicable != null && leaveApplicable?.LeaveTypeId > 0)
                    {
                        _leaveApplicableRepository.Delete(leaveApplicable);
                        await _leaveApplicableRepository.SaveChangesAsync();
                        //Delete Leave Department
                        var departmentList = _leaveDepartmentRepository.GetLeaveApplicableDepartmentDetailsByLeaveTypeId(pLeaveTypeId);
                        if (departmentList?.Count > 0)
                        {
                            foreach (LeaveDepartment department in departmentList)
                            {
                                _leaveDepartmentRepository.Delete(department);
                            }
                            await _leaveDepartmentRepository.SaveChangesAsync();
                        }
                        //Delete Leave Designation
                        var designationList = _leaveDesignationRepository.GetLeaveApplicableDesignationDetailsByLeaveTypeId(pLeaveTypeId);
                        if (designationList?.Count > 0)
                        {
                            foreach (LeaveDesignation designation in designationList)
                            {
                                _leaveDesignationRepository.Delete(designation);
                            }
                            await _leaveDesignationRepository.SaveChangesAsync();
                        }
                        //Delete Leave Location
                        var locationList = _leaveLocationRepository.GetLeaveApplicableLocationDetailsByLeaveTypeId(pLeaveTypeId);
                        if (locationList?.Count > 0)
                        {
                            foreach (LeaveLocation location in locationList)
                            {
                                _leaveLocationRepository.Delete(location);
                            }
                            await _leaveLocationRepository.SaveChangesAsync();
                        }
                        //Delete Leave Role
                        var roleList = _leaveRoleRepository.GetLeaveApplicableRoleDetailsByLeaveTypeId(pLeaveTypeId);
                        if (roleList?.Count > 0)
                        {
                            foreach (LeaveRole role in roleList)
                            {
                                _leaveRoleRepository.Delete(role);
                            }
                            await _leaveRoleRepository.SaveChangesAsync();
                        }
                        //Delete employee 
                        var employeeList = _employeeApplicableLeaveRepository.GetEmployeeApplicableLeaveDetailsByLeaveTypeId(pLeaveTypeId);
                        if (employeeList?.Count > 0)
                        {
                            foreach (EmployeeApplicableLeave employee in employeeList)
                            {
                                _employeeApplicableLeaveRepository.Delete(employee);
                            }
                            await _employeeApplicableLeaveRepository.SaveChangesAsync();
                        }

                        LeaveRestrictions leaveRestrictions = _leaveRestrictionsRepository.GetByID(pLeaveTypeId);
                        if (leaveRestrictions != null && leaveRestrictions?.LeaveTypeId > 0)
                        {
                            _leaveRestrictionsRepository.Delete(leaveRestrictions);
                            await _leaveRestrictionsRepository.SaveChangesAsync();
                        }
                        //delete Leave Employee Type
                        var employeeTypeList = _leaveEmployeeTypeRepository.GetLeaveEmployeeTypeDetailsByLeaveTypeId(pLeaveTypeId);
                        if (employeeTypeList?.Count > 0)
                        {
                            foreach (LeaveEmployeeType employeeType in employeeTypeList)
                            {
                                _leaveEmployeeTypeRepository.Delete(employeeType);
                            }
                            await _leaveEmployeeTypeRepository.SaveChangesAsync();
                        }
                        // delete Leave Probation Status
                        var probationStatusList = _leaveProbationStatuRepository.GetLeaveProbationStatusDetailsByLeaveTypeId(pLeaveTypeId);
                        if (probationStatusList?.Count > 0)
                        {
                            foreach (LeaveProbationStatus probationStatus in probationStatusList)
                            {
                                _leaveProbationStatuRepository.Delete(probationStatus);
                            }
                            await _leaveProbationStatuRepository.SaveChangesAsync();
                        }
                    }
                    //Delete applied leave
                    List<EmployeeLeaveDetails> empLeaveDetail = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByLeaveID(pLeaveTypeId);
                    if (empLeaveDetail?.Count > 0)
                    {
                        foreach (EmployeeLeaveDetails empLeave in empLeaveDetail)
                        {
                            List<ApplyLeaves> leavelist = _LeaveApplyRepository.GetAppliedleaveByEmployeeId(empLeave.EmployeeID == null ? 0 : (int)empLeave.EmployeeID, pLeaveTypeId);
                            if (leavelist?.Count > 0)
                            {
                                foreach (ApplyLeaves appliedLeaves in leavelist)
                                {
                                    if (appliedLeaves?.LeaveId > 0)
                                    {
                                        List<AppliedLeaveDetails> appliedLeaveDetails = _AppliedLeaveDetailsRepository.GetByID(appliedLeaves.LeaveId);
                                        foreach (AppliedLeaveDetails appliedLeave in appliedLeaveDetails)
                                        {
                                            _AppliedLeaveDetailsRepository.Delete(appliedLeave);
                                            await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                        }
                                        ApplyLeaves ApplyLeaves = _LeaveApplyRepository.GetAppliedleaveByIdToDelete(appliedLeaves.LeaveId);
                                        if (ApplyLeaves != null && ApplyLeaves?.LeaveId > 0)
                                        {
                                            documnentsPath.LeaveId.Add(appliedLeaves.LeaveId);
                                            _LeaveApplyRepository.Delete(ApplyLeaves);
                                            await _LeaveApplyRepository.SaveChangesAsync();
                                        }
                                    }
                                }
                            }
                            _employeeLeaveDetailsRepository.Delete(empLeave);
                            await _employeeLeaveDetailsRepository.SaveChangesAsync();
                        }

                    }
                    //Grant details
                    List<GrantLeaveApproval> grantLeaves = _grantLeaveApprovalRepository.GetGrantLeaveApprovalByLeaveTypeId(pLeaveTypeId);
                    if (grantLeaves?.Count > 0)
                    {
                        foreach (GrantLeaveApproval grantLeave in grantLeaves)
                        {
                            _grantLeaveApprovalRepository.Delete(grantLeave);
                            await _grantLeaveApprovalRepository.SaveChangesAsync();
                        }
                    }
                    List<LeaveGrantRequestDetails> grantLeaveApprovals = _leaveGrantRequestDetailsRepository.GetByLeaveTypeID(pLeaveTypeId);
                    if (grantLeaveApprovals?.Count > 0)
                    {
                        foreach (LeaveGrantRequestDetails leaveGrantRequestDetails in grantLeaveApprovals)
                        {
                            documnentsPath.requestDetails.Add(leaveGrantRequestDetails);
                            List<EmployeeGrantLeaveApproval> employeeGrantLeaveApprovals = _employeeGrantLeaveApprovalRepository.GetEmployeeGrantLeaveApprovalByLeaveGrantDetailId(leaveGrantRequestDetails.LeaveGrantDetailId);
                            if (employeeGrantLeaveApprovals?.Count > 0)
                            {
                                foreach (EmployeeGrantLeaveApproval employeeGrantLeave in employeeGrantLeaveApprovals)
                                {
                                    if (employeeGrantLeave?.LeaveGrantDetailId > 0)
                                    {
                                        List<LeaveGrantDocumentDetails> documentDetails = _leaveGrantDocumentDetailsRepository.GetByID(employeeGrantLeave.LeaveGrantDetailId == null ? 0 : (int)employeeGrantLeave.LeaveGrantDetailId);
                                        foreach (LeaveGrantDocumentDetails details in documentDetails)
                                        {
                                            _leaveGrantDocumentDetailsRepository.Delete(details);
                                            await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                        }
                                    }
                                    _employeeGrantLeaveApprovalRepository.Delete(employeeGrantLeave);
                                    await _employeeGrantLeaveApprovalRepository.SaveChangesAsync();
                                }
                            }
                            _leaveGrantRequestDetailsRepository.Delete(leaveGrantRequestDetails);
                            await _leaveGrantRequestDetailsRepository.SaveChangesAsync();
                        }
                    }
                    //LeaveType delete(ActiveLeaveTypeList)
                    var activeLeaveAndHolidayList = _leaveTakenTogetherRepository.GetByID(pLeaveTypeId);
                    if (activeLeaveAndHolidayList?.Count > 0)
                    {
                        foreach (LeaveTakenTogether leaveTakenList in activeLeaveAndHolidayList)
                        {
                            _leaveTakenTogetherRepository.Delete(leaveTakenList);
                        }
                        await _leaveTakenTogetherRepository.SaveChangesAsync();
                    }
                    var leaveAndHolidayList = _leaveTakenTogetherRepository.GetByLeaveType(pLeaveTypeId);
                    if (leaveAndHolidayList?.Count > 0)
                    {
                        foreach (LeaveTakenTogether leaveHolidayList in leaveAndHolidayList)
                        {
                            _leaveTakenTogetherRepository.Delete(leaveHolidayList);
                        }
                        await _leaveTakenTogetherRepository.SaveChangesAsync();
                    }
                    return documnentsPath;
                }
            }
            catch (Exception)
            {
                throw;
            }
            return new DocumnentsPathView();
        }
        #endregion

        #region Get All Leaves
        public List<LeaveView> GetAllLeaves()
        {
            return _leaveRepository.GetAllLeaves();
        }
        #endregion

        #region List All Leaves Details
        public LeavesMasterDataView GetLeaveEmployeeMasterData()
        {
            LeavesMasterDataView LeavesMasterDataView = new()
            {
                LeaveMaxLimitActionList = _leaveRepository.GetMaxlimitAction(),
                //ProbationStatusList = _leaveRepository.GetProbationStatus(),
                MonthList = _leaveRepository.GetMonthsList(),
                DaysList = _leaveRepository.GetDaysList(),
                AllowUserList = _leaveRepository.GetAllowUserToViewList(),
                BalanceToBeDisplayList = _leaveRepository.GetBalanceToBeDisplay(),
                LeaveDurationList = _LeaveApplyRepository.GetAppconstantList("LeaveDuration"),
                ReportConfigurationList = _LeaveApplyRepository.GetAppconstantList("ReportConfiguration"),
                CurrentFinancialYearHolidayList = _LeaveApplyRepository.GetCurrentFinancialYearHolidayList(),
                ActiveLeaveTypeList = _LeaveApplyRepository.GetActiveLeaveTypeList(),
                LeaveTypeList = _LeaveApplyRepository.GetAppconstantList("LeaveType"),
                LeaveAccuredList = _LeaveApplyRepository.GetAppconstantList("LeaveAccured"),
                AllowRequestPeriod = _LeaveApplyRepository.GetAppconstantList("AllowRequestPeriod"),
                SpecificEmployeeLeaveList = _LeaveApplyRepository.GetAppconstantList("SpecificEmployeeLeave"),
                CarryForwardList = _LeaveApplyRepository.GetAppconstantList("CarryForwardLeave"),
                ReimbursementList = _LeaveApplyRepository.GetAppconstantList("ReimbursementLeave"),
                BalanceBasedOn = _LeaveApplyRepository.GetAppconstantList("BalanceBasedOn"),
                GrantLeaveRequestPeriod = _LeaveApplyRepository.GetAppconstantList("GrantLeaveRequestPeriod"),
                GrantLeaveApproval = _LeaveApplyRepository.GetAppconstantList("GrantLeaveApproval")
            };
            return LeavesMasterDataView;
        }
        #endregion

        #region Add or Update Leave
        public async Task<int> AddorUpdateApplyLeave(ApplyLeavesView ApplyLeavesView, bool isEdit = false)
        {
            try
            {
                int leaveId = 0;
                ApplyLeaves leaves = new();
                DateTime editFromDate = default;
                DateTime editToDate = default;
                bool isGrantLeave = _leaveTypeRepository.CheckIsGrantLeaveByLeaveTypeId(ApplyLeavesView.LeaveTypeId);
                LeaveCarryForward leaveCarryForwardDetails = new();
                LeaveCarryForwardListView carryForwardDetails = new(); 
                if (isGrantLeave==false)
                {
                    leaveCarryForwardDetails = _leaveCarryForwardRepository.GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId);
                    carryForwardDetails = _leaveEntitlementRepository.GetLeaveTypeCarryForwardDetails(ApplyLeavesView.LeaveTypeId);
                }                
                if (ApplyLeavesView?.LeaveId != 0)
                {
                    leaves = _LeaveApplyRepository.GetApplyleaveById(ApplyLeavesView.LeaveId);
                    editFromDate = leaves.FromDate;
                    editToDate = leaves.ToDate;
                }
                if (leaves != null)
                {
                    leaves.EmployeeId = ApplyLeavesView.EmployeeId;
                    leaves.LeaveTypeId = ApplyLeavesView.LeaveTypeId;
                    leaves.FromDate = (DateTime)ApplyLeavesView.FromDate;
                    leaves.ToDate = ApplyLeavesView.ToDate;
                    leaves.NoOfDays = ApplyLeavesView.NoOfDays;
                    leaves.Reason = ApplyLeavesView.Reason;
                    leaves.Feedback = ApplyLeavesView.Feedback;
                    leaves.LeaveRejectionReasonId = ApplyLeavesView.LeaveRejectionReasonId;
                    leaves.IsActive = ApplyLeavesView.IsActive;
                    leaves.ManagerId = ApplyLeavesView?.ApproverManagerId;
                    if (ApplyLeavesView?.LeaveId == 0)
                    {
                        leaves.CreatedOn = DateTime.UtcNow;
                        leaves.CreatedBy = ApplyLeavesView.EmployeeId;
                        leaves.Status = "Pending";
                        await _LeaveApplyRepository.AddAsync(leaves);
                        await _LeaveApplyRepository.SaveChangesAsync();
                        leaveId = leaves.LeaveId;
                    }
                    else
                    {
                        leaves.ModifiedOn = DateTime.UtcNow;
                        leaves.ModifiedBy = ApplyLeavesView.EmployeeId;
                        _LeaveApplyRepository.Update(leaves);
                        leaveId = ApplyLeavesView.LeaveId;
                        await _LeaveApplyRepository.SaveChangesAsync();
                    }
                    if (ApplyLeavesView?.AppliedLeaveDetails?.Count > 0)
                    {

                        //decimal editLeaveTotal = 0;
                        //decimal newLeaveTotal = 0;
                        decimal editGrantLeaveTotal = 0;
                        decimal newGrantLeaveTotal = 0;
                        List<AppliedLeaveDetailsView> editGrantLeaveDetails = new List<AppliedLeaveDetailsView>();
                        if (isEdit)
                        {
                            List<AppliedLeaveDetails> appliedLeaveDetails = _AppliedLeaveDetailsRepository.GetByID(leaveId);
                            if (appliedLeaveDetails != null && appliedLeaveDetails?.Count > 0)
                            {
                                foreach (AppliedLeaveDetails appliedLeave in appliedLeaveDetails)
                                {
                                    AppliedLeaveDetailsView leave = new AppliedLeaveDetailsView();
                                    leave.Date = appliedLeave.Date;
                                    leave.IsFullDay = appliedLeave.IsFullDay;
                                    leave.IsFirstHalf = appliedLeave.IsFirstHalf;
                                    leave.IsSecondHalf = appliedLeave.IsSecondHalf;
                                    leave.LeaveId = appliedLeave.LeaveId;
                                    editGrantLeaveDetails.Add(leave);
                                    decimal editLeave = 0;
                                    if (appliedLeave.Date.Date <= DateTime.Now.Date)
                                    {
                                        //if (appliedLeave.IsFullDay)
                                        //{
                                        //    editLeaveTotal += 1;
                                        //}
                                        //if (appliedLeave.IsFirstHalf || appliedLeave.IsSecondHalf)
                                        //{
                                        //    editLeaveTotal += (decimal)0.5;
                                        //}
                                        if (appliedLeave.IsFullDay)
                                        {
                                            editLeave = 1;
                                        }
                                        if (appliedLeave.IsFirstHalf || appliedLeave.IsSecondHalf)
                                        {
                                            editLeave = (decimal)0.5;
                                        }
                                        bool isValidEmpLeave = true;
                                        EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId);
                                        if (employeeLeaveDetails != null && leaveCarryForwardDetails != null &&
                                            leaveCarryForwardDetails.ResetDate>= appliedLeave.Date)
                                        {
                                            isValidEmpLeave = false;
                                           LeaveCarryForward updateCarry = _leaveCarryForwardRepository.GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId( ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId);
                                            updateCarry.BalanceLeave = updateCarry.BalanceLeave + editLeave;
                                            if(updateCarry.AdjustmentEffectiveFromDate !=null && updateCarry.AdjustmentEffectiveFromDate<= appliedLeave.Date)
                                            {
                                                updateCarry.AdjustmentBalanceLeave = updateCarry.AdjustmentBalanceLeave==null? editLeave : (updateCarry.AdjustmentBalanceLeave + editLeave);
                                            }
                                            if(updateCarry.CarryForwardLeaves !=null && updateCarry.CarryForwardLeaves<carryForwardDetails.MaximumCarryForwardDays)
                                            {
                                                isValidEmpLeave = true;
                                                updateCarry.CarryForwardLeaves = updateCarry.CarryForwardLeaves == null ? editLeave : (updateCarry.CarryForwardLeaves + editLeave);
                                            }
                                            _leaveCarryForwardRepository.Update(updateCarry);
                                            await _LeaveApplyRepository.SaveChangesAsync();
                                        }
                                        if(employeeLeaveDetails != null && isValidEmpLeave==true)
                                        {
                                            employeeLeaveDetails.BalanceLeave = employeeLeaveDetails?.BalanceLeave==null? editLeave : (decimal)employeeLeaveDetails?.BalanceLeave + editLeave;
                                            if (employeeLeaveDetails?.AdjustmentEffectiveFromDate != null)
                                            {                                                
                                                if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= appliedLeave.Date)
                                                {
                                                    employeeLeaveDetails.AdjustmentBalanceLeave = employeeLeaveDetails.AdjustmentBalanceLeave==null? editLeave : (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave + editLeave;
                                                }
                                            }
                                            employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                            employeeLeaveDetails.ModifiedBy = ApplyLeavesView?.EmployeeId;
                                            _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                            await _LeaveApplyRepository.SaveChangesAsync();
                                        }
                                    }
                                    //Grant leave count
                                    if (appliedLeave.IsFullDay)
                                    {
                                        editGrantLeaveTotal += 1;
                                    }
                                    if (appliedLeave.IsFirstHalf || appliedLeave.IsSecondHalf)
                                    {
                                        editGrantLeaveTotal += (decimal)0.5;
                                    }
                                    _AppliedLeaveDetailsRepository.Delete(appliedLeave);
                                    await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                }
                            }
                        }
                        foreach (AppliedLeaveDetailsView appliedLeaveDetailsView in ApplyLeavesView?.AppliedLeaveDetails)
                        {
                            if (appliedLeaveDetailsView.Date.Date <= DateTime.Now.Date)
                            {
                                //if (appliedLeaveDetailsView.IsFullDay)
                                //{
                                //    newLeaveTotal += (decimal)1;
                                //}
                                //if (appliedLeaveDetailsView.IsFirstHalf || appliedLeaveDetailsView.IsSecondHalf)
                                //{
                                //    newLeaveTotal += (decimal)0.5;
                                //}
                                decimal leaveTotal = 0;
                                if (appliedLeaveDetailsView.IsFullDay)
                                {
                                    leaveTotal = 1;
                                }
                                if (appliedLeaveDetailsView.IsFirstHalf || appliedLeaveDetailsView.IsSecondHalf)
                                {
                                    leaveTotal = (decimal)0.5;
                                }
                                bool isValidEmpLeave = true;
                                EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId);
                                if (employeeLeaveDetails != null && leaveCarryForwardDetails != null &&
                                    leaveCarryForwardDetails.ResetDate >= appliedLeaveDetailsView.Date.Date)
                                {
                                    isValidEmpLeave = false;
                                    LeaveCarryForward updateCarry = _leaveCarryForwardRepository.GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId);
                                    updateCarry.BalanceLeave = updateCarry.BalanceLeave - leaveTotal;
                                    if (updateCarry.AdjustmentEffectiveFromDate != null && updateCarry.AdjustmentEffectiveFromDate <= appliedLeaveDetailsView.Date.Date)
                                    {
                                        updateCarry.AdjustmentBalanceLeave = updateCarry.AdjustmentBalanceLeave == null ? -(leaveTotal) : (updateCarry.AdjustmentBalanceLeave - leaveTotal);
                                    }
                                    if ( updateCarry?.BalanceLeave < carryForwardDetails.MaximumCarryForwardDays)
                                    {
                                        isValidEmpLeave = true;
                                        updateCarry.CarryForwardLeaves = updateCarry.CarryForwardLeaves == null ? -(leaveTotal) : (updateCarry.CarryForwardLeaves - leaveTotal);
                                    }
                                    _leaveCarryForwardRepository.Update(updateCarry);
                                    await _LeaveApplyRepository.SaveChangesAsync();
                                }
                                if (employeeLeaveDetails != null && isValidEmpLeave == true)
                                {
                                    employeeLeaveDetails.BalanceLeave = employeeLeaveDetails?.BalanceLeave == null ? -(leaveTotal) : (decimal)employeeLeaveDetails?.BalanceLeave - leaveTotal;
                                    if (employeeLeaveDetails?.AdjustmentEffectiveFromDate != null)
                                    {
                                        if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= appliedLeaveDetailsView.Date.Date)
                                        {
                                            employeeLeaveDetails.AdjustmentBalanceLeave = employeeLeaveDetails.AdjustmentBalanceLeave==null? -(leaveTotal):(decimal)employeeLeaveDetails?.AdjustmentBalanceLeave - leaveTotal;
                                        }
                                    }
                                    employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                    employeeLeaveDetails.ModifiedBy = ApplyLeavesView?.EmployeeId;
                                    _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                    await _LeaveApplyRepository.SaveChangesAsync();
                                }
                            }
                            //Grant leave count
                            if (appliedLeaveDetailsView.IsFullDay)
                            {
                                newGrantLeaveTotal += 1;
                            }
                            if (appliedLeaveDetailsView.IsFirstHalf || appliedLeaveDetailsView.IsSecondHalf)
                            {
                                newGrantLeaveTotal += (decimal)0.5;
                            }

                            AppliedLeaveDetails appliedLeaveDetails = new();
                            appliedLeaveDetails.Date = appliedLeaveDetailsView.Date;
                            appliedLeaveDetails.IsFullDay = appliedLeaveDetailsView.IsFullDay;
                            appliedLeaveDetails.IsFirstHalf = appliedLeaveDetailsView.IsFirstHalf;
                            appliedLeaveDetails.IsSecondHalf = appliedLeaveDetailsView.IsSecondHalf;
                            appliedLeaveDetails.CompensatoryOffId = appliedLeaveDetailsView.CompensatoryOffId;
                            appliedLeaveDetails.CreatedOn = DateTime.UtcNow;
                            appliedLeaveDetails.CreatedBy = ApplyLeavesView?.EmployeeId;
                            appliedLeaveDetails.LeaveId = leaveId;
                            await _AppliedLeaveDetailsRepository.AddAsync(appliedLeaveDetails);
                            await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                        }
                        //decimal totalLeave = 0;
                        
                        if (isGrantLeave == true)
                        {
                            if (editGrantLeaveTotal > 0)
                            {
                                bool IsSucsses = await RevertGrantLeaveRequestBalance(editGrantLeaveTotal, ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId, editFromDate, editToDate, editGrantLeaveDetails);
                            }
                            //totalLeave = newGrantLeaveTotal - editGrantLeaveTotal;                            
                            if (newGrantLeaveTotal > 0)
                            {
                                bool IsSucsses = await UpdateGrantLeaveRequestBalance(newGrantLeaveTotal, ApplyLeavesView.EmployeeId,
                                     ApplyLeavesView.LeaveTypeId, ApplyLeavesView.FromDate, ApplyLeavesView.ToDate, ApplyLeavesView.AppliedLeaveDetails);
                            }
                        }
                        //else
                        //{
                        //    totalLeave = newLeaveTotal - editLeaveTotal;
                        //    EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId);
                        //    if (employeeLeaveDetails != null)
                        //    {
                        //        decimal totalAvailableLeave = totalLeave;
                        //        if (employeeLeaveDetails?.AdjustmentEffectiveFromDate != null)
                        //        {
                        //            if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= ApplyLeavesView?.FromDate)
                        //            {
                        //                totalAvailableLeave = (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave - totalLeave;
                        //                employeeLeaveDetails.AdjustmentBalanceLeave = totalAvailableLeave;
                        //            }
                        //            else
                        //            {
                        //                totalAvailableLeave = (decimal)employeeLeaveDetails?.BalanceLeave - totalLeave;
                        //                employeeLeaveDetails.BalanceLeave = totalAvailableLeave;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            totalAvailableLeave = (decimal)employeeLeaveDetails?.BalanceLeave - totalLeave;
                        //            employeeLeaveDetails.BalanceLeave = totalAvailableLeave;
                        //        }
                        //        employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                        //        employeeLeaveDetails.ModifiedBy = ApplyLeavesView?.EmployeeId;
                        //        _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                        //        await _LeaveApplyRepository.SaveChangesAsync();
                        //    }
                        //}


                    }
                }
                return leaveId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region update grant leave balance
        public async Task<bool> UpdateGrantLeaveRequestBalance(decimal newLeave, int employeeId, int leaveTypeId, DateTime? fromDate, DateTime? toDate, List<AppliedLeaveDetailsView> appliedLeaveDetails)
        {
            //decimal daysDiff = newLeave;
            List<LeaveGrantRequestDetails> leaveDetails = _leaveGrantRequestDetailsRepository.GetGrantLeaveRequestBalance(employeeId, leaveTypeId, fromDate, toDate, appliedLeaveDetails);
            if (newLeave > 0 && leaveDetails?.Count > 0)
            {
                foreach (AppliedLeaveDetailsView item in appliedLeaveDetails)
                {
                    decimal days = 0;
                    if (item?.IsFullDay == true)
                    {
                        days = 1;
                    }
                    else if (item?.IsFirstHalf == true || item?.IsSecondHalf == true)
                    {
                        days = (decimal)0.5;
                    }
                    foreach (LeaveGrantRequestDetails grantRequest in leaveDetails)
                    {

                        if (days > 0 && grantRequest?.BalanceDay > 0 && grantRequest.EffectiveFromDate <= item.Date &&
                            (grantRequest.EffectiveToDate >= item.Date || grantRequest.EffectiveToDate == null))
                        {
                            if (grantRequest?.BalanceDay >= days)
                            {
                                grantRequest.BalanceDay = grantRequest?.BalanceDay - days;
                                days = 0;
                            }
                            else if (grantRequest?.BalanceDay < days)
                            {
                                decimal? addBalance = grantRequest.NumberOfDay - grantRequest.BalanceDay;
                                if (addBalance != null && addBalance > 0)
                                {
                                    grantRequest.BalanceDay = grantRequest?.BalanceDay - addBalance;
                                    days = days - (addBalance == null ? 0 : (decimal)addBalance);
                                }
                            }
                            _leaveGrantRequestDetailsRepository.Update(grantRequest);
                            await _leaveGrantRequestDetailsRepository.SaveChangesAsync();
                        }
                    }
                }


            }
            return true;
        }
        #endregion
        #region revert grant leave balance
        public async Task<bool> RevertGrantLeaveRequestBalance(decimal revertLeave, int employeeId, int leaveTypeId, DateTime? fromDate, DateTime? toDate, List<AppliedLeaveDetailsView> appliedLeave)
        {
            //decimal revertDays = revertLeave;
            List<LeaveGrantRequestDetails> leaveDetails = _leaveGrantRequestDetailsRepository.GetGrantLeaveRequestByDate(employeeId, leaveTypeId, fromDate, toDate, appliedLeave);
            if (revertLeave > 0 && leaveDetails?.Count > 0)
            {
                //leaveDetails = leaveDetails.Select(x => x).OrderByDescending(x => x.EffectiveFromDate).ToList();
                foreach (AppliedLeaveDetailsView item in appliedLeave)
                {
                    decimal days = 0;
                    if (item?.IsFullDay == true)
                    {
                        days = 1;
                    }
                    else if (item?.IsFirstHalf == true || item?.IsSecondHalf == true)
                    {
                        days = (decimal)0.5;
                    }
                    foreach (LeaveGrantRequestDetails grantRequest in leaveDetails)
                    {
                        if (days > 0 && grantRequest?.NumberOfDay > grantRequest?.BalanceDay && grantRequest.EffectiveFromDate <= item.Date &&
                            (grantRequest.EffectiveToDate >= item.Date || grantRequest.EffectiveToDate == null))
                        {
                            decimal? diffDay = grantRequest.NumberOfDay - grantRequest.BalanceDay;
                            if (diffDay >= days)
                            {
                                grantRequest.BalanceDay = grantRequest?.BalanceDay + days;
                                days = 0;
                            }
                            else
                            {
                                grantRequest.BalanceDay = grantRequest?.BalanceDay + diffDay;
                                days = days - (diffDay == null ? 0 : (decimal)diffDay);
                            }
                            _leaveGrantRequestDetailsRepository.Update(grantRequest);
                            await _leaveGrantRequestDetailsRepository.SaveChangesAsync();
                        }
                    }
                }

            }
            return true;
        }
        #endregion

        #region Get Applied Leave Detail By Employee Id and Leave Id
        public AppliedLeaveEditView GetAppliedLeaveToEdit(int leaveId)
        {
            return _LeaveApplyRepository.GetAppliedLeaveToEdit(leaveId);
        }
        #endregion

        #region Delete Applied Leave By Leave Id
        public async Task<bool> DeleteAppliedLeaveByLeaveId(int leaveId)
        {
            try
            {
                if (leaveId > 0)
                {                    
                    decimal grantLeaveTotal = 0;
                    List<AppliedLeaveDetails> appliedLeaveDetails = _AppliedLeaveDetailsRepository.GetByID(leaveId);
                    ApplyLeaves ApplyLeaves = _LeaveApplyRepository.GetAppliedleaveByIdToDelete(leaveId);
                    List<AppliedLeaveDetailsView> appliedLeaveDetailsView = new List<AppliedLeaveDetailsView>();
                    bool isGrantLeave = _leaveTypeRepository.CheckIsGrantLeaveByLeaveTypeId(ApplyLeaves.LeaveTypeId);
                    LeaveCarryForward leaveCarryForwardDetails = new();
                    LeaveCarryForwardListView carryForwardDetails = new();
                    if (isGrantLeave == false)
                    {
                        leaveCarryForwardDetails = _leaveCarryForwardRepository.GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(ApplyLeaves.EmployeeId, ApplyLeaves.LeaveTypeId);
                        carryForwardDetails = _leaveEntitlementRepository.GetLeaveTypeCarryForwardDetails(ApplyLeaves.LeaveTypeId);
                    }
                    foreach (AppliedLeaveDetails appliedLeave in appliedLeaveDetails)
                    {
                        if (appliedLeave.Date.Date <= DateTime.Now.Date && appliedLeave?.AppliedLeaveStatus != false)
                        {
                            decimal DeleteLeaveTotal = 0;
                            if (appliedLeave.IsFullDay)
                            {
                                DeleteLeaveTotal = 1;
                            }
                            if (appliedLeave.IsFirstHalf || appliedLeave.IsSecondHalf)
                            {
                                DeleteLeaveTotal = (decimal)0.5;
                            }
                            bool isValidEmpLeave = true;
                            EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(ApplyLeaves.EmployeeId, ApplyLeaves.LeaveTypeId);
                            if (employeeLeaveDetails != null && leaveCarryForwardDetails != null &&
                                leaveCarryForwardDetails.ResetDate >= appliedLeave?.Date.Date)
                            {
                                isValidEmpLeave = false;
                                LeaveCarryForward updateCarry = _leaveCarryForwardRepository.GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(ApplyLeaves.EmployeeId, ApplyLeaves.LeaveTypeId);
                                updateCarry.BalanceLeave = updateCarry.BalanceLeave + DeleteLeaveTotal;
                                if (updateCarry.AdjustmentEffectiveFromDate != null && updateCarry.AdjustmentEffectiveFromDate <= appliedLeave?.Date.Date)
                                {
                                    updateCarry.AdjustmentBalanceLeave = updateCarry.AdjustmentBalanceLeave == null ? DeleteLeaveTotal : (updateCarry.AdjustmentBalanceLeave + DeleteLeaveTotal);
                                }
                                if (updateCarry.CarryForwardLeaves != null && updateCarry.CarryForwardLeaves < carryForwardDetails.MaximumCarryForwardDays)
                                {
                                    isValidEmpLeave = true;
                                    updateCarry.CarryForwardLeaves = updateCarry.CarryForwardLeaves == null ? DeleteLeaveTotal : (updateCarry.CarryForwardLeaves + DeleteLeaveTotal);
                                }
                                _leaveCarryForwardRepository.Update(updateCarry);
                                await _LeaveApplyRepository.SaveChangesAsync();
                            }
                            if (employeeLeaveDetails != null && isValidEmpLeave == true)
                            {
                                employeeLeaveDetails.BalanceLeave = employeeLeaveDetails?.BalanceLeave == null ? DeleteLeaveTotal : (decimal)employeeLeaveDetails?.BalanceLeave + DeleteLeaveTotal;
                                if (employeeLeaveDetails?.AdjustmentEffectiveFromDate != null)
                                {
                                    if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= appliedLeave?.Date.Date)
                                    {
                                        employeeLeaveDetails.AdjustmentBalanceLeave = employeeLeaveDetails.AdjustmentBalanceLeave==null? DeleteLeaveTotal : (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave + DeleteLeaveTotal;
                                    }
                                }
                                employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                employeeLeaveDetails.ModifiedBy = ApplyLeaves.EmployeeId;
                                _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                await _LeaveApplyRepository.SaveChangesAsync();
                            }
                        }
                        if (appliedLeave?.AppliedLeaveStatus != false)
                        {
                            AppliedLeaveDetailsView leave = new AppliedLeaveDetailsView();
                            leave.Date = appliedLeave.Date;
                            leave.IsFullDay = appliedLeave.IsFullDay;
                            leave.IsFirstHalf = appliedLeave.IsFirstHalf;
                            leave.IsSecondHalf = appliedLeave.IsSecondHalf;
                            leave.LeaveId = appliedLeave.LeaveId;
                            appliedLeaveDetailsView.Add(leave);

                            if (appliedLeave.IsFullDay)
                            {
                                grantLeaveTotal += 1;
                            }
                            if (appliedLeave.IsFirstHalf || appliedLeave.IsSecondHalf)
                            {
                                grantLeaveTotal += (decimal)0.5;
                            }
                        }
                        _AppliedLeaveDetailsRepository.Delete(appliedLeave);
                        await _AppliedLeaveDetailsRepository.SaveChangesAsync();


                    }
                    
                    if (ApplyLeaves != null && ApplyLeaves?.LeaveId > 0)
                    {
                        //bool isGrantLeave = _leaveTypeRepository.CheckIsGrantLeaveByLeaveId(leaveId);
                        _LeaveApplyRepository.Delete(ApplyLeaves);
                        await _LeaveApplyRepository.SaveChangesAsync();
                        //DeleteLeaveTotal += ApplyLeaves.NoOfDays;
                        //if (DeleteLeaveTotal > 0 || grantLeaveTotal > 0)
                        //{
                        //    EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(ApplyLeaves.EmployeeId, ApplyLeaves.LeaveTypeId);

                        //    if (employeeLeaveDetails != null)
                        //    {
                        //        decimal totalAvailableLeave = isGrantLeave == true ? 0 : DeleteLeaveTotal;
                        //        if (employeeLeaveDetails.AdjustmentEffectiveFromDate != null)
                        //        {
                        //            if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= DateTime.UtcNow.Date)
                        //            {
                        //                totalAvailableLeave += (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave;
                        //                employeeLeaveDetails.AdjustmentBalanceLeave = totalAvailableLeave;
                        //            }
                        //            else
                        //            {
                        //                totalAvailableLeave += (decimal)employeeLeaveDetails?.BalanceLeave;
                        //                employeeLeaveDetails.BalanceLeave = totalAvailableLeave;
                        //            }
                        //        }
                        //        else
                        //        {
                        //            totalAvailableLeave += (decimal)employeeLeaveDetails?.BalanceLeave;
                        //            employeeLeaveDetails.BalanceLeave = totalAvailableLeave;
                        //        }
                        //        employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                        //        employeeLeaveDetails.ModifiedBy = ApplyLeaves?.EmployeeId;
                        //        _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                        //        await _LeaveApplyRepository.SaveChangesAsync();
                        //    }
                        //}
                        if (isGrantLeave && grantLeaveTotal > 0)
                        {
                            bool IsSucsses = await RevertGrantLeaveRequestBalance(grantLeaveTotal, ApplyLeaves.EmployeeId,
                                ApplyLeaves.LeaveTypeId, ApplyLeaves.FromDate, ApplyLeaves.ToDate, appliedLeaveDetailsView);
                        }
                        return true;
                    }

                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion

        #region Get Team Leaves
        public List<TeamLeaveView> GetTeamLeave(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            return _leaveRepository.GetTeamLeave(managerTeamLeaveView);
        }
        #endregion

        #region Approve or Reject Leave
        public async Task<StatusandApproverDetails> ApproveOrRejectLeave(ApproveOrRejectLeave pApproveOrRejectLeave)
        {
            //string status = "";
            StatusandApproverDetails statusandApprover = new();
            try
            {
                //decimal RejectLeaveTotal = 0;

                if (pApproveOrRejectLeave?.IsGrantLeaveRequest == true)
                {
                    List<EmployeeGrantLeaveApproval> Details = _employeeGrantLeaveApprovalRepository.GetEmployeeGrantLeaveApprovalByLeaveGrantDetailId(
                            pApproveOrRejectLeave.LeaveGrantDetailId == null ? 0 : (int)pApproveOrRejectLeave.LeaveGrantDetailId);
                    EmployeeGrantLeaveApproval approverDetails = _employeeGrantLeaveApprovalRepository.GetEmployeeGrantLeaveApprovalByApprover(
                            pApproveOrRejectLeave.LeaveGrantDetailId, pApproveOrRejectLeave.LevelId == null ? 0 : (int)pApproveOrRejectLeave.LevelId);
                    if (approverDetails != null)
                    {
                        statusandApprover.ApproverManagerId = approverDetails?.ApproverEmployeeId;
                        approverDetails.Status = pApproveOrRejectLeave?.Status;
                        approverDetails.Comments = pApproveOrRejectLeave?.Feedback;
                        approverDetails.ModifiedOn = DateTime.UtcNow;
                        approverDetails.ModifiedBy = pApproveOrRejectLeave?.ModifiedBy;
                        _employeeGrantLeaveApprovalRepository.Update(approverDetails);
                        await _employeeGrantLeaveApprovalRepository.SaveChangesAsync();
                    }
                    if (pApproveOrRejectLeave?.Status == "Approved")
                    {
                        EmployeeGrantLeaveApproval nextApprover = _employeeGrantLeaveApprovalRepository.GetEmployeeGrantLeaveApprovalByLevelId(
                            pApproveOrRejectLeave.LeaveGrantDetailId, (pApproveOrRejectLeave.LevelId == null ? 0 : (int)pApproveOrRejectLeave.LevelId) + 1);
                        if (nextApprover != null)
                        {
                            statusandApprover.ApproverID = nextApprover.ApproverEmployeeId == null ? 0 : (int)nextApprover.ApproverEmployeeId;
                            statusandApprover.LevelId = nextApprover.LevelId == null ? 0 : (int)nextApprover.LevelId;
                            nextApprover.Status = "Pending";
                            nextApprover.ModifiedOn = DateTime.UtcNow;
                            nextApprover.ModifiedBy = pApproveOrRejectLeave?.ModifiedBy;
                            _employeeGrantLeaveApprovalRepository.Update(nextApprover);
                            await _employeeGrantLeaveApprovalRepository.SaveChangesAsync();
                        }

                        else
                        {
                            LeaveGrantRequestDetails leaveGrantRequestDetails = _leaveGrantRequestDetailsRepository.GetByID(pApproveOrRejectLeave.LeaveGrantDetailId == null ? 0 : (int)pApproveOrRejectLeave.LeaveGrantDetailId);
                            if (leaveGrantRequestDetails != null)
                            {
                                leaveGrantRequestDetails.Status = "Approved";
                                leaveGrantRequestDetails.ModifiedOn = DateTime.UtcNow;
                                leaveGrantRequestDetails.ModifiedBy = pApproveOrRejectLeave?.ModifiedBy;
                                _leaveGrantRequestDetailsRepository.Update(leaveGrantRequestDetails);
                                await _leaveGrantRequestDetailsRepository.SaveChangesAsync();

                                EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(leaveGrantRequestDetails.EmployeeID, pApproveOrRejectLeave.LeaveTypeId);
                                if (employeeLeaveDetails == null)
                                {
                                    employeeLeaveDetails = new()
                                    {
                                        EmployeeID = leaveGrantRequestDetails?.EmployeeID,
                                        LeaveTypeID = pApproveOrRejectLeave?.LeaveTypeId,
                                        BalanceLeave = 0, // leaveGrantRequestDetails?.NumberOfDay,
                                        CreatedOn = DateTime.UtcNow,
                                        CreatedBy = pApproveOrRejectLeave?.ModifiedBy
                                    };
                                    await _employeeLeaveDetailsRepository.AddAsync(employeeLeaveDetails);
                                    await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                }
                                //if (employeeLeaveDetails != null)
                                //{
                                //    employeeLeaveDetails.BalanceLeave = 0;//employeeLeaveDetails.BalanceLeave == null ? leaveGrantRequestDetails.NumberOfDay : (employeeLeaveDetails.BalanceLeave) + (leaveGrantRequestDetails.NumberOfDay == null ? 0 : leaveGrantRequestDetails.NumberOfDay);
                                //    employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                //    employeeLeaveDetails.ModifiedBy = pApproveOrRejectLeave?.ModifiedBy;
                                //    _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                //    await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                //}
                                //else
                                //{
                                //    employeeLeaveDetails = new()
                                //    {
                                //        EmployeeID = leaveGrantRequestDetails?.EmployeeID,
                                //        LeaveTypeID = pApproveOrRejectLeave?.LeaveTypeId,
                                //        BalanceLeave = 0, // leaveGrantRequestDetails?.NumberOfDay,
                                //        CreatedOn = DateTime.UtcNow,
                                //        CreatedBy = pApproveOrRejectLeave?.ModifiedBy
                                //    };
                                //    await _employeeLeaveDetailsRepository.AddAsync(employeeLeaveDetails);
                                //    await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                //}
                            }
                        }
                    }
                    else
                    {
                        LeaveGrantRequestDetails leaveGrantRequestDetails = _leaveGrantRequestDetailsRepository.GetByID(pApproveOrRejectLeave.LeaveGrantDetailId == null ? 0 : (int)pApproveOrRejectLeave.LeaveGrantDetailId);
                        if (leaveGrantRequestDetails != null)
                        {
                            leaveGrantRequestDetails.Status = pApproveOrRejectLeave?.Status;
                            leaveGrantRequestDetails.ModifiedOn = DateTime.UtcNow;
                            leaveGrantRequestDetails.ModifiedBy = pApproveOrRejectLeave?.ModifiedBy;
                            _leaveGrantRequestDetailsRepository.Update(leaveGrantRequestDetails);
                            await _leaveGrantRequestDetailsRepository.SaveChangesAsync();
                        }
                    }
                    if (pApproveOrRejectLeave?.Status == "Cancelled")
                    {
                        string status = Details.Select(x => x.Status).LastOrDefault();
                        if (status?.ToLower() == "approved")
                        {
                            EmployeeLeaveDetails employeeLeave = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(pApproveOrRejectLeave.EmployeeId, pApproveOrRejectLeave.LeaveTypeId);
                            if (employeeLeave != null)
                            {
                                if (employeeLeave.BalanceLeave != null && employeeLeave.BalanceLeave > 0)
                                {
                                    employeeLeave.BalanceLeave = employeeLeave.BalanceLeave - pApproveOrRejectLeave.NoOfDays;
                                }
                                employeeLeave.ModifiedOn = DateTime.UtcNow;
                                employeeLeave.ModifiedBy = pApproveOrRejectLeave?.ModifiedBy;
                                _employeeLeaveDetailsRepository.Update(employeeLeave);
                                await _employeeLeaveDetailsRepository.SaveChangesAsync();
                            }
                        }
                        foreach (var item in Details)
                        {
                            item.Status = pApproveOrRejectLeave?.Status;
                            item.ModifiedOn = DateTime.UtcNow;
                            item.ModifiedBy = pApproveOrRejectLeave?.ModifiedBy;
                            _employeeGrantLeaveApprovalRepository.Update(item);
                            await _employeeGrantLeaveApprovalRepository.SaveChangesAsync();
                        }
                    }
                    if (pApproveOrRejectLeave?.Status == "Approved")
                    {
                        statusandApprover.Status = "Approved";
                    }
                    else if (pApproveOrRejectLeave?.Status == "Rejected")
                    {
                        statusandApprover.Status = "Rejected";
                    }
                    else
                    {
                        statusandApprover.Status = "Cancelled";
                    }
                }
                else
                {
                    List<AppliedLeaveDetailsView> appliedLeaveDetailsView = new List<AppliedLeaveDetailsView>();

                    bool isGrantLeave = _leaveTypeRepository.CheckIsGrantLeaveByLeaveTypeId(pApproveOrRejectLeave.LeaveTypeId);
                    LeaveCarryForward leaveCarryForwardDetails = new();
                    LeaveCarryForwardListView carryForwardDetails = new();
                    if (isGrantLeave == false)
                    {
                        leaveCarryForwardDetails = _leaveCarryForwardRepository.GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(pApproveOrRejectLeave.EmployeeId, pApproveOrRejectLeave.LeaveTypeId);
                        carryForwardDetails = _leaveEntitlementRepository.GetLeaveTypeCarryForwardDetails(pApproveOrRejectLeave.LeaveTypeId);
                    }
                    ApplyLeaves applyLeaves = new();
                    if (pApproveOrRejectLeave.LeaveId != 0) applyLeaves = _LeaveApplyRepository.GetTeamLeaveByID(pApproveOrRejectLeave.LeaveId);
                    if (applyLeaves != null)
                    {
                        DateTime leaveFromDate = applyLeaves.FromDate;
                        DateTime leaveToDate = applyLeaves.ToDate;
                        applyLeaves.Status = pApproveOrRejectLeave?.Status;
                        applyLeaves.Feedback = pApproveOrRejectLeave?.Feedback;
                        applyLeaves.LeaveRejectionReasonId = pApproveOrRejectLeave?.LeaveRejectionReasonId;
                        applyLeaves.ApproveRejectBy = pApproveOrRejectLeave.ModifiedBy;
                        applyLeaves.ApproveRejectOn = DateTime.UtcNow;
                        applyLeaves.ApproveRejectName = pApproveOrRejectLeave.ApproveRejectName;
                        //if (pApproveOrRejectLeave?.AppliedLeaveApproveOrReject?.Count() > 0 && applyLeaves?.Status != "Cancelled")
                        //{
                        //    List<AppliedLeaveApproveOrReject> fullDayCount = new();
                        //    fullDayCount = pApproveOrRejectLeave?.AppliedLeaveApproveOrReject?.Where(x => x.AppliedLeaveStatus == false && x.IsFullDay).Select(x => x).ToList();
                        //    foreach (var view in fullDayCount)
                        //    {
                        //        view.NoOfDays = 1;
                        //    }
                        //    List<AppliedLeaveApproveOrReject> halfDayCount = new();
                        //    halfDayCount = pApproveOrRejectLeave?.AppliedLeaveApproveOrReject?.Where(x => x.AppliedLeaveStatus == false && (x.IsFirstHalf || x.IsSecondHalf)).Select(x => x).ToList();
                        //    foreach (var view in halfDayCount)
                        //    {
                        //        view.NoOfDays = 0.5m;
                        //    }
                        //    decimal? totalCount = fullDayCount.Sum(x => x.NoOfDays) + halfDayCount.Sum(x => x.NoOfDays);
                        //    RejectLeaveTotal += totalCount == null ? 0 : (decimal)totalCount;
                        //    //applyLeaves.NoOfDays = (decimal)(applyLeaves.NoOfDays - totalCount);
                        //}
                        //decimal leaveTotal = 0;
                        decimal grantLeaveTotal = 0;
                        if (applyLeaves.Status == "Cancelled")
                        {
                            //RejectLeaveTotal += applyLeaves.NoOfDays;
                            applyLeaves.IsActive = false;
                            foreach (var item in pApproveOrRejectLeave?.AppliedLeaveApproveOrReject)
                            {
                                var appliedLeave = _AppliedLeaveDetailsRepository.GetAppliedLeaveByID(item.AppliedLeaveDetailsID);
                                decimal leaveTotal = 0;
                                if (appliedLeave != null)
                                {
                                    if (appliedLeave?.Date.Date <= DateTime.Now.Date && appliedLeave?.AppliedLeaveStatus != false)
                                    {
                                        if (item.IsFullDay)
                                        {
                                            leaveTotal = 1;
                                        }
                                        if (item.IsFirstHalf || item.IsSecondHalf)
                                        {
                                            leaveTotal = (decimal)0.5;
                                        }

                                        bool isValidEmpLeave = true;
                                        EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(pApproveOrRejectLeave.EmployeeId, pApproveOrRejectLeave.LeaveTypeId);
                                        if (employeeLeaveDetails != null && leaveCarryForwardDetails != null &&
                                            leaveCarryForwardDetails.ResetDate >= appliedLeave?.Date.Date)
                                        {
                                            isValidEmpLeave = false;
                                            LeaveCarryForward updateCarry = _leaveCarryForwardRepository.GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(pApproveOrRejectLeave.EmployeeId, pApproveOrRejectLeave.LeaveTypeId);
                                            updateCarry.BalanceLeave = updateCarry.BalanceLeave + leaveTotal;
                                            if (updateCarry.AdjustmentEffectiveFromDate != null && updateCarry.AdjustmentEffectiveFromDate <= appliedLeave?.Date.Date)
                                            {
                                                updateCarry.AdjustmentBalanceLeave = updateCarry.AdjustmentBalanceLeave == null ? leaveTotal : (updateCarry.AdjustmentBalanceLeave + leaveTotal);
                                            }
                                            if (updateCarry.CarryForwardLeaves != null && updateCarry.CarryForwardLeaves < carryForwardDetails.MaximumCarryForwardDays)
                                            {
                                                isValidEmpLeave = true;
                                                updateCarry.CarryForwardLeaves = updateCarry.CarryForwardLeaves == null ? leaveTotal : (updateCarry.CarryForwardLeaves + leaveTotal);
                                            }
                                            _leaveCarryForwardRepository.Update(updateCarry);
                                            await _LeaveApplyRepository.SaveChangesAsync();
                                        }
                                        if (employeeLeaveDetails != null && isValidEmpLeave == true)
                                        {
                                            employeeLeaveDetails.BalanceLeave = employeeLeaveDetails?.BalanceLeave == null ? leaveTotal : (decimal)employeeLeaveDetails?.BalanceLeave + leaveTotal;
                                            if (employeeLeaveDetails?.AdjustmentEffectiveFromDate != null)
                                            {
                                                if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= appliedLeave?.Date.Date)
                                                {
                                                    employeeLeaveDetails.AdjustmentBalanceLeave = employeeLeaveDetails.AdjustmentBalanceLeave==null? leaveTotal : (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave + leaveTotal;
                                                }
                                            }
                                            employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                            employeeLeaveDetails.ModifiedBy = pApproveOrRejectLeave.EmployeeId;
                                            _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                            await _LeaveApplyRepository.SaveChangesAsync();
                                        }

                                    }
                                    if (appliedLeave?.AppliedLeaveStatus != false)
                                    {
                                        if (item.IsFullDay)
                                        {
                                            grantLeaveTotal += 1;
                                        }
                                        if (item.IsFirstHalf || item.IsSecondHalf)
                                        {
                                            grantLeaveTotal += (decimal)0.5;
                                        }
                                        AppliedLeaveDetailsView leave = new AppliedLeaveDetailsView();
                                        leave.Date = appliedLeave.Date;
                                        leave.IsFullDay = item.IsFullDay;
                                        leave.IsFirstHalf = item.IsFirstHalf;
                                        leave.IsSecondHalf = item.IsSecondHalf;
                                        appliedLeaveDetailsView.Add(leave);
                                    }
                                    appliedLeave.AppliedLeaveStatus = false;
                                    appliedLeave.ModifiedOn = DateTime.UtcNow;
                                    appliedLeave.ModifiedBy = item?.ModifiedBy;
                                    _AppliedLeaveDetailsRepository.Update(appliedLeave);
                                    await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                }
                            }

                        }
                        applyLeaves.ModifiedOn = DateTime.UtcNow;
                        applyLeaves.ModifiedBy = pApproveOrRejectLeave?.ModifiedBy;
                        //applyLeaves.ManagerId = pApproveOrRejectLeave?.ApproverManagerId;
                        _LeaveApplyRepository.Update(applyLeaves);
                        await _LeaveApplyRepository.SaveChangesAsync();
                        if (applyLeaves?.Status == "Approved")
                        {
                            foreach (var item in pApproveOrRejectLeave?.AppliedLeaveApproveOrReject)
                            {
                                var appliedLeave = _AppliedLeaveDetailsRepository.GetAppliedLeaveByID(item.AppliedLeaveDetailsID);
                                if (appliedLeave != null)
                                {
                                    appliedLeave.AppliedLeaveStatus = item?.AppliedLeaveStatus;
                                    appliedLeave.ModifiedOn = DateTime.UtcNow;
                                    appliedLeave.ModifiedBy = item?.ModifiedBy;
                                    _AppliedLeaveDetailsRepository.Update(appliedLeave);
                                    await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                }
                            }
                        }
                        else if (applyLeaves?.Status == "Rejected")
                        {
                            foreach (var item in pApproveOrRejectLeave?.AppliedLeaveApproveOrReject)
                            {

                                var appliedLeave = _AppliedLeaveDetailsRepository.GetAppliedLeaveByID(item.AppliedLeaveDetailsID);
                                if (appliedLeave != null)
                                {
                                    if (appliedLeave?.Date.Date <= DateTime.Now.Date && item?.AppliedLeaveStatus == false)
                                    {
                                        decimal leaveTotal = 0;
                                        if (item.IsFullDay)
                                        {
                                            leaveTotal = 1;
                                        }
                                        if (item.IsFirstHalf || item.IsSecondHalf)
                                        {
                                            leaveTotal = (decimal)0.5;
                                        }
                                        bool isValidEmpLeave = true;
                                        EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(pApproveOrRejectLeave.EmployeeId, pApproveOrRejectLeave.LeaveTypeId);
                                        if (employeeLeaveDetails != null && leaveCarryForwardDetails != null &&
                                            leaveCarryForwardDetails.ResetDate >= appliedLeave?.Date.Date)
                                        {
                                            isValidEmpLeave = false;
                                            LeaveCarryForward updateCarry = _leaveCarryForwardRepository.GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(pApproveOrRejectLeave.EmployeeId, pApproveOrRejectLeave.LeaveTypeId);
                                            updateCarry.BalanceLeave = updateCarry.BalanceLeave + leaveTotal;
                                            if (updateCarry.AdjustmentEffectiveFromDate != null && updateCarry.AdjustmentEffectiveFromDate <= appliedLeave?.Date.Date)
                                            {
                                                updateCarry.AdjustmentBalanceLeave = updateCarry.AdjustmentBalanceLeave == null ? leaveTotal : (updateCarry.AdjustmentBalanceLeave + leaveTotal);
                                            }
                                            if (updateCarry.CarryForwardLeaves != null && updateCarry.CarryForwardLeaves < carryForwardDetails.MaximumCarryForwardDays)
                                            {
                                                isValidEmpLeave = true;
                                                updateCarry.CarryForwardLeaves = updateCarry.CarryForwardLeaves == null ? leaveTotal : (updateCarry.CarryForwardLeaves + leaveTotal);
                                            }
                                            _leaveCarryForwardRepository.Update(updateCarry);
                                            await _LeaveApplyRepository.SaveChangesAsync();
                                        }
                                        if (employeeLeaveDetails != null && isValidEmpLeave == true)
                                        {
                                            employeeLeaveDetails.BalanceLeave = employeeLeaveDetails?.BalanceLeave == null ? leaveTotal : (decimal)employeeLeaveDetails?.BalanceLeave + leaveTotal;
                                            if (employeeLeaveDetails?.AdjustmentEffectiveFromDate != null)
                                            {
                                                if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= appliedLeave?.Date.Date)
                                                {
                                                    employeeLeaveDetails.AdjustmentBalanceLeave = employeeLeaveDetails.AdjustmentBalanceLeave==null? leaveTotal : (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave + leaveTotal;
                                                }
                                            }
                                            employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                            employeeLeaveDetails.ModifiedBy = pApproveOrRejectLeave.EmployeeId;
                                            _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                            await _LeaveApplyRepository.SaveChangesAsync();
                                        }
                                    }
                                    if (item?.AppliedLeaveStatus == false)
                                    {
                                        if (item.IsFullDay)
                                        {
                                            grantLeaveTotal += 1;
                                        }
                                        if (item.IsFirstHalf || item.IsSecondHalf)
                                        {
                                            grantLeaveTotal += (decimal)0.5;
                                        }
                                        AppliedLeaveDetailsView leave = new AppliedLeaveDetailsView();
                                        leave.Date = appliedLeave.Date;
                                        leave.IsFullDay = item.IsFullDay;
                                        leave.IsFirstHalf = item.IsFirstHalf;
                                        leave.IsSecondHalf = item.IsSecondHalf;
                                        appliedLeaveDetailsView.Add(leave);
                                    }
                                    appliedLeave.AppliedLeaveStatus = item?.AppliedLeaveStatus;
                                    appliedLeave.ModifiedOn = DateTime.UtcNow;
                                    appliedLeave.ModifiedBy = item?.ModifiedBy;
                                    _AppliedLeaveDetailsRepository.Update(appliedLeave);
                                    await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                }
                            }
                        }
                        else if (applyLeaves?.Status == "Pending")
                        {
                            foreach (var item in pApproveOrRejectLeave?.AppliedLeaveApproveOrReject)
                            {

                                var appliedLeave = _AppliedLeaveDetailsRepository.GetAppliedLeaveByID(item.AppliedLeaveDetailsID);
                                if (appliedLeave != null)
                                {
                                    if (appliedLeave?.Date.Date <= DateTime.Now.Date && item?.AppliedLeaveStatus == false)
                                    {
                                        decimal leaveTotal = 0;
                                        if (item.IsFullDay)
                                        {
                                            leaveTotal = 1;
                                        }
                                        if (item.IsFirstHalf || item.IsSecondHalf)
                                        {
                                            leaveTotal = (decimal)0.5;
                                        }
                                        bool isValidEmpLeave = true;
                                        EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(pApproveOrRejectLeave.EmployeeId, pApproveOrRejectLeave.LeaveTypeId);
                                        if (employeeLeaveDetails != null && leaveCarryForwardDetails != null &&
                                            leaveCarryForwardDetails.ResetDate >= appliedLeave?.Date.Date)
                                        {
                                            isValidEmpLeave = false;
                                            LeaveCarryForward updateCarry = _leaveCarryForwardRepository.GetLeaveCarryForwardByEmployeeIdAndLeaveTypeId(pApproveOrRejectLeave.EmployeeId, pApproveOrRejectLeave.LeaveTypeId);
                                            updateCarry.BalanceLeave = updateCarry.BalanceLeave + leaveTotal;
                                            if (updateCarry.AdjustmentEffectiveFromDate != null && updateCarry.AdjustmentEffectiveFromDate <= appliedLeave?.Date.Date)
                                            {
                                                updateCarry.AdjustmentBalanceLeave = updateCarry.AdjustmentBalanceLeave == null ? leaveTotal : (updateCarry.AdjustmentBalanceLeave + leaveTotal);
                                            }
                                            if (updateCarry.CarryForwardLeaves != null && updateCarry.CarryForwardLeaves < carryForwardDetails.MaximumCarryForwardDays)
                                            {
                                                isValidEmpLeave = true;
                                                updateCarry.CarryForwardLeaves = updateCarry.CarryForwardLeaves == null ? leaveTotal : (updateCarry.CarryForwardLeaves + leaveTotal);
                                            }
                                            _leaveCarryForwardRepository.Update(updateCarry);
                                            await _LeaveApplyRepository.SaveChangesAsync();
                                        }
                                        if (employeeLeaveDetails != null && isValidEmpLeave == true)
                                        {
                                            employeeLeaveDetails.BalanceLeave = employeeLeaveDetails?.BalanceLeave == null ? leaveTotal : (decimal)employeeLeaveDetails?.BalanceLeave + leaveTotal;
                                            if (employeeLeaveDetails?.AdjustmentEffectiveFromDate != null)
                                            {
                                                if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= appliedLeave?.Date.Date)
                                                {
                                                    employeeLeaveDetails.AdjustmentBalanceLeave = employeeLeaveDetails.AdjustmentBalanceLeave == null ? leaveTotal : (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave + leaveTotal;
                                                }
                                            }
                                            employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                            employeeLeaveDetails.ModifiedBy = pApproveOrRejectLeave.EmployeeId;
                                            _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                            await _LeaveApplyRepository.SaveChangesAsync();
                                        }
                                    }
                                    if (item?.AppliedLeaveStatus == false)
                                    {
                                        if (item.IsFullDay)
                                        {
                                            grantLeaveTotal += 1;
                                        }
                                        if (item.IsFirstHalf || item.IsSecondHalf)
                                        {
                                            grantLeaveTotal += (decimal)0.5;
                                        }
                                        AppliedLeaveDetailsView leave = new AppliedLeaveDetailsView();
                                        leave.Date = appliedLeave.Date;
                                        leave.IsFullDay = item.IsFullDay;
                                        leave.IsFirstHalf = item.IsFirstHalf;
                                        leave.IsSecondHalf = item.IsSecondHalf;
                                        appliedLeaveDetailsView.Add(leave);
                                    }
                                    appliedLeave.AppliedLeaveStatus = item?.AppliedLeaveStatus;
                                    appliedLeave.ModifiedOn = DateTime.UtcNow;
                                    appliedLeave.ModifiedBy = item?.ModifiedBy;
                                    _AppliedLeaveDetailsRepository.Update(appliedLeave);
                                    await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                }
                            }
                        }
                        if (isGrantLeave == true && grantLeaveTotal > 0)
                        {
                            bool IsSucsses = await RevertGrantLeaveRequestBalance(grantLeaveTotal, pApproveOrRejectLeave.EmployeeId,
                                pApproveOrRejectLeave.LeaveTypeId, leaveFromDate, leaveToDate, appliedLeaveDetailsView);
                        }
                    }

                    if (applyLeaves?.Status == "Approved")
                    {
                        statusandApprover.Status = "Approved";
                    }
                    else if (applyLeaves?.Status == "Rejected")
                    {
                        statusandApprover.Status = "Rejected";
                    }
                    else
                    {
                        statusandApprover.Status = "Cancelled";
                    }
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return statusandApprover;
        }
        #endregion

        //#region Get Available Leave Details By EmployeeId
        //public List<EmployeeAvailableLeaveDetails> GetAvailableLeaveDetailsByEmployeeId(int employeeId, DateTime FromDate, DateTime ToDate)
        //{
        //    return _leaveRestrictionsRepository.GetEmployeeLeaveAndRestrictionDetails(employeeId, FromDate, ToDate);
        //}
        //#endregion

        #region Get Leave Types
        public List<LeaveTypesView> GetLeaveTypes(int departmentId)
        {
            return _leaveRepository.GetLeaveTypes(departmentId);
        }
        #endregion

        #region Get Employee LeaveAdjustment Details
        public List<EmployeeLeaveAdjustment> GetEmployeeLeaveAdjustmentDetails(int employeeId, DateTime fromDate, DateTime toDate)
        {
            List<EmployeeLeaveAdjustment> LeaveDetails = _leaveRepository.GetEmployeeLeaveAdjustmentDetails(employeeId, fromDate, toDate);
            foreach (EmployeeLeaveAdjustment item in LeaveDetails)
            {
                if (item.LeaveBalance != null)
                {
                    item.LeaveBalance = GetRoundOffValues((decimal)item?.LeaveBalance);
                }
                if (item.ActualBalanceLeave != null)
                {
                    item.ActualBalanceLeave = GetRoundOffValues((decimal)item?.ActualBalanceLeave);
                }
                if (item.AdjustmentLeaveBalance != null)
                {
                    item.AdjustmentLeaveBalance = GetRoundOffValues((decimal)item?.AdjustmentLeaveBalance);
                }
                if (!string.IsNullOrEmpty(item.ResetYearName) && !string.IsNullOrEmpty(item.ResetDay))
                {
                    item.LeaveResetOn = GetLeaveResetDate(item?.ResetYearName, item?.ResetDay, item.ResetMonth == null ? 0 : (int)item.ResetMonth);
                    item.AdjustmentDays = GetLeaveAdjustmentDays(item?.ResetYearName, item?.LeaveResetOn, item?.LeaveAdjustmentDetails);
                }
                if (item?.LeaveAdjustmentDetails?.Count > 0)
                {
                    var leaveAdjustmentList = (from le in item.LeaveAdjustmentDetails
                                               group le by le.EffectiveFromDate into ad
                                               select ad).ToList();
                    List<LeaveAdjustmentDetails> adjustment = new List<LeaveAdjustmentDetails>();
                    foreach (var detail in leaveAdjustmentList)
                    {
                        LeaveAdjustmentDetails data = detail.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                        adjustment.Add(data);
                    }
                    item.LeaveAdjustmentDetails = adjustment;
                }
                //Calculate grant leave balance
                if (item.BalanceBasedOn?.BalanceBasedOnValue == "LeaveGrant")
                {
                    decimal balance = GetGrantLeaveBalance(employeeId, item.LeaveTypeId == null ? 0 : (int)item.LeaveTypeId, DateTime.Now.Date, 0);
                    item.ActualBalanceLeave = balance;
                    item.LeaveBalance = balance;
                    item.AdjustmentLeaveBalance = 0;
                    item.AdjustmentEffectiveFromDate = null;
                }
            }
            return LeaveDetails;
        }
        #endregion 

        #region Get Weekly Leave Detail By Employee Id
        public WeeklyLeaveHolidayOverview GetWeeklyLeavesHolidayByEmployeeId(int employeeId)
        {
            WeeklyLeaveHolidayOverview WeeklyOverviewReport = new()
            {
                LeaveData = _LeaveApplyRepository.GetWeeklyLeavesByEmployeeId(employeeId),
                HolidaysData = _holidayRepository.GetWeeklyReportHolidays(employeeId)
            };
            return WeeklyOverviewReport;
        }
        #endregion

        #region Get Attendace Leave Detail By Employee Id
        public AttendanceDaysAndHoursDetailsView GetAttendanceLeaveDetailsByEmployeeId(int employeeId, int departmentId)
        {
            return _LeaveApplyRepository.GetAttendanceLeaveDetailsByEmployeeId(employeeId, departmentId);
        }
        #endregion

        #region Get Leave By Employee Id
        public LeaveHolidayView GetLeaveHolidayByEmployeeId(WeekMonthAttendanceView weekMonthAttendanceView)
        {
            LeaveHolidayView leaveHoliday = new();
            //leaveHoliday.ApplyLeaves = _leaveRepository.GetLeaveByEmployeeId(weekMonthAttendanceView.EmployeeId, weekMonthAttendanceView.FromDate, weekMonthAttendanceView.ToDate);
            leaveHoliday.Holiday = _holidayRepository.GetHolidayByDate(weekMonthAttendanceView.DepartmentId, weekMonthAttendanceView.FromDate, weekMonthAttendanceView.ToDate, weekMonthAttendanceView.LocationId, (int)weekMonthAttendanceView.ShiftDetailsId);
            leaveHoliday.appliedLeaveDetails = _leaveRepository.GetEmployeeLeaveDetails(weekMonthAttendanceView.EmployeeId, weekMonthAttendanceView.FromDate, weekMonthAttendanceView.ToDate);
            leaveHoliday.holidayDetails = _holidayRepository.GetHolidayDetails(weekMonthAttendanceView.DepartmentId, weekMonthAttendanceView.FromDate, weekMonthAttendanceView.ToDate, weekMonthAttendanceView.LocationId, weekMonthAttendanceView.DOJ);
            return leaveHoliday;
        }
        #endregion

        #region Get Leave Rejection Reason
        public List<LeaveRejectionReason> GetLeaveRejectionReason()
        {
            return _LeaveRejectionReasonRepository.GetLeaveRejectionReason();
        }
        #endregion

        #region Applied Dates Duplication
        public bool ApplyLeaveDatesDupilication(ApplyLeavesView ApplyLeavesView)
        {
            return _AppliedLeaveDetailsRepository.ApplyLeaveDatesDupilication(ApplyLeavesView);
        }
        #endregion

        #region Get Available Leave Detail By Employee 
        public List<AvailableLeaveDetailsView> GetAvailableLeaveDetailsByEmployee(int employeeId)
        {
            return _LeaveApplyRepository.GetAvailableLeaveDetailsByEmployee(employeeId);
        }
        #endregion

        #region Accrued Employee Leaves
        public async Task<bool> AccruedEmployeeLeaves(tempparameter temp)
        {
            List<EmployeeDetailsForLeaveView> EmployeeList = temp?.EmployeeDetail;
            DateTime todayDate = temp.executedate == null ? DateTime.Now.Date : (DateTime)temp.executedate;
            var creditdates = default(decimal);
            try
            {
                LeaveTypes leaveDetails = new();
                List<LeaveTypes> leavetypeslist = new();
                LeaveApplicable leaveApplicable = new();
                List<ProRateMonthDetailsView> proRateMonthDetails = new();
                AppConstants appConstants = new();

                if (EmployeeList.Count != 0)
                {
                    leavetypeslist = _leaveTypeRepository?.GetAllLeaveType(todayDate);
                    //leavetypeslist = leavetypeslist.Where(x => x.LeaveTypeId == 12).Select(x => x).ToList();
                    foreach (LeaveTypes leaveTypesList in leavetypeslist)
                    {
                        try
                        {
                            DateTime Executeddate = default;
                            var Quarter = GetFinancialYearQuarter(todayDate);
                            var HalfYearly = GetFinancialYearHalfYearly(todayDate);
                            var Yearly = GetFinancialYearly(todayDate);

                            appConstants = _appConstantsRepository.GetAppconstantByID(leaveTypesList.LeaveAccruedType);
                            if (appConstants != null)
                            {
                                bool IsSameMonth = false;
                                if (appConstants?.DisplayName?.ToLower() == "Monthly".ToLower())
                                {
                                    if (leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower())
                                    {
                                        Executeddate = new DateTime(todayDate.Year, todayDate.Month, 1);
                                    }
                                    else if (leaveTypesList?.LeaveAccruedDay?.ToLower() == "LastDays".ToLower())
                                    {
                                        Executeddate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));
                                    }
                                    else
                                    {
                                        Executeddate = new DateTime(todayDate.Year, todayDate.Month, Convert.ToInt32(leaveTypesList.LeaveAccruedDay));
                                    }
                                    if (leaveTypesList?.EffectiveFromDate.Value.Date <= Executeddate.Date)
                                    {
                                        IsSameMonth = true;
                                    }
                                }
                                else if (appConstants?.DisplayName?.ToLower() == "Quarterly".ToLower())
                                {
                                    Executeddate = leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower() ? Quarter.FromDate : leaveTypesList.LeaveAccruedDay.ToLower() == "LastDays".ToLower() ? Quarter.ToDate : default;

                                    if (leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower())
                                    {
                                        if (Quarter?.FromDate >= leaveTypesList?.EffectiveFromDate)
                                        {
                                            IsSameMonth = true;
                                        }
                                    }
                                    else
                                    {
                                        IsSameMonth = true;
                                    }
                                }
                                else if (appConstants?.DisplayName?.ToLower() == "HalfYearly".ToLower())
                                {
                                    Executeddate = leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower() ? HalfYearly.FromDate : leaveTypesList.LeaveAccruedDay.ToLower() == "LastDays".ToLower() ? HalfYearly.ToDate : default;
                                    if (leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower())
                                    {
                                        if (HalfYearly?.FromDate >= leaveTypesList?.EffectiveFromDate)
                                        {
                                            IsSameMonth = true;
                                        }
                                    }
                                    else
                                    {
                                        IsSameMonth = true;
                                    }
                                }
                                else if (appConstants?.DisplayName?.ToLower() == "Yearly".ToLower())
                                {
                                    Executeddate = leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower() ? Yearly.FromDate : leaveTypesList.LeaveAccruedDay.ToLower() == "LastDays".ToLower() ? Yearly.ToDate : default;
                                    if (leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower())
                                    {
                                        if (Yearly?.FromDate >= leaveTypesList?.EffectiveFromDate)
                                        {
                                            IsSameMonth = true;
                                        }
                                    }
                                    else
                                    {
                                        IsSameMonth = true;
                                    }
                                }

                                if (todayDate == Executeddate && leaveTypesList?.EffectiveFromDate <= todayDate && IsSameMonth)
                                {
                                    leaveDetails = leaveTypesList;
                                }
                                else
                                {
                                    leaveDetails = null;
                                }
                            }
                            var Basedon = _appConstantsRepository.GetAppconstantByID(leaveTypesList?.BalanceBasedOn);
                            if (leaveDetails != null && (Basedon == null || Basedon?.AppConstantValue?.ToLower() == "FixedEntitlement".ToLower()))
                            {
                                leaveApplicable = _leaveApplicableRepository.GetleaveApplicableByLeaveId(leaveDetails.LeaveTypeId);
                                //EmployeeList = EmployeeList.Where(x => x.EmployeeID == 122).Select(x => x).ToList();
                                foreach (EmployeeDetailsForLeaveView item in EmployeeList)
                                {
                                    try
                                    {


                                        switch (item?.Gender?.ToLower())
                                        {
                                            case "male":
                                                item.GenderMale = true;
                                                item.GenderFemale = null;
                                                item.GenderOther = null;

                                                break;
                                            case "female":
                                                item.GenderMale = null;
                                                item.GenderFemale = true;
                                                item.GenderOther = null;
                                                break;
                                            case "other":
                                                item.GenderMale = null;
                                                item.GenderFemale = null;
                                                item.GenderOther = true;
                                                break;
                                            default:
                                                item.GenderMale = null;
                                                item.GenderFemale = null;
                                                item.GenderOther = null;
                                                break;
                                        }
                                        switch (item?.MaritalStatus?.ToLower())
                                        {
                                            case "single":
                                                item.MaritalStatusSingle = true;
                                                item.MaritalStatusMarried = null;
                                                break;
                                            case "married":
                                                item.MaritalStatusSingle = null;
                                                item.MaritalStatusMarried = true;
                                                break;
                                            default:
                                                item.MaritalStatusSingle = null;
                                                item.MaritalStatusMarried = null;
                                                break;
                                        }
                                        bool leaveapplicable = false;
                                        bool isEligible = true;
                                        bool isApplicable = false;
                                        var prob = item?.ProbationStatusID == 0 ? null : item?.ProbationStatusID == null ? null : item?.ProbationStatusID;


                                        if ((leaveApplicable?.Gender_Male == true && item?.GenderMale == true) || (leaveApplicable?.Gender_Female == true && item?.GenderFemale == true) || (leaveApplicable?.Gender_Others == true && item?.GenderOther == true))
                                        {
                                            leaveapplicable = true;
                                            isApplicable = true;
                                        }
                                        else if (leaveApplicable?.Gender_Male != true && leaveApplicable?.Gender_Female != true && leaveApplicable?.Gender_Others != true)
                                        {
                                            leaveapplicable = true;
                                        }
                                        else
                                        {
                                            isEligible = false;
                                        }
                                        if ((leaveApplicable?.MaritalStatus_Single == true && item?.MaritalStatusSingle == true) || (leaveApplicable?.MaritalStatus_Married == true && item?.MaritalStatusMarried == true))
                                        {
                                            leaveapplicable = true;
                                            isApplicable = true;
                                        }
                                        else if (leaveApplicable?.MaritalStatus_Single != true && leaveApplicable?.MaritalStatus_Married != true)
                                        {
                                            leaveapplicable = true;
                                        }
                                        else
                                        {
                                            isEligible = false;
                                        }
                                        //if (leaveApplicable?.EmployeeTypeId != 0 && leaveApplicable?.EmployeeTypeId != item?.EmployeeTypeID)
                                        //{
                                        //    isEligible = false;                                            
                                        //}
                                        //else if (leaveApplicable?.EmployeeTypeId != 0 && leaveApplicable?.EmployeeTypeId == item?.EmployeeTypeID)
                                        //{
                                        //    isApplicable = true;
                                        //}
                                        //else
                                        //{
                                        //    leaveapplicable = true;
                                        //}
                                        //if (leaveApplicable?.ProbationStatus != 0 && leaveApplicable?.ProbationStatus != prob)
                                        //{
                                        //    isEligible = false;
                                        //}
                                        //else if (leaveApplicable?.ProbationStatus != 0 && leaveApplicable?.ProbationStatus == prob)
                                        //{
                                        //    isApplicable = true;
                                        //}
                                        //else
                                        //{
                                        //    leaveapplicable = true;
                                        //}



                                        //if (leaveApplicable?.Gender_Male == item?.GenderMale ||
                                        //    leaveApplicable?.Gender_Female == item?.GenderFemale ||
                                        //    leaveApplicable?.Gender_Others == item.GenderOther ||
                                        //    leaveApplicable?.MaritalStatus_Single == item.MaritalStatusSingle ||
                                        //    leaveApplicable?.MaritalStatus_Married == item.MaritalStatusMarried ||
                                        //    leaveApplicable?.EmployeeTypeId == item.EmployeeTypeID ||
                                        //    leaveApplicable?.ProbationStatus == prob
                                        //    )
                                        //{
                                        //    leaveapplicable = true;
                                        //}

                                        EmployeeLeaveApplicableView applicable = GetLeaveApplicable(leaveDetails, item);
                                        var exceptions = GetLeaveExceptions(leaveDetails, item);
                                        bool employeeApplicable = false;
                                        List<EmployeeApplicableLeave> employeeApplicableLeaves = _employeeApplicableLeaveRepository.GetEmployeeApplicableLeaveDetailsByLeaveTypeId(leaveDetails.LeaveTypeId);
                                        if (employeeApplicableLeaves.Any(x => x.EmployeeId != 0) && employeeApplicableLeaves.Any(m => m.EmployeeId == item.EmployeeID))
                                        {
                                            employeeApplicable = true;
                                        }
                                        // exception
                                        bool leaveexception = false;
                                        bool isEligibleException = false;
                                        bool isException = false;
                                        LeaveApplicable leaveException = new();
                                        leaveException = _leaveApplicableRepository.GetleaveExceptionByLeaveId(leaveDetails.LeaveTypeId);

                                        if ((leaveException?.Gender_Male_Exception == true && item?.GenderMale == true) || (leaveException?.Gender_Female_Exception == true && item?.GenderFemale == true) || (leaveException?.Gender_Others_Exception == true && item?.GenderOther == true))
                                        {
                                            leaveexception = true;
                                            isException = true;
                                        }
                                        else if (leaveException?.Gender_Male_Exception != true && leaveException?.Gender_Female_Exception != true && leaveException?.Gender_Others_Exception != true)
                                        {
                                            leaveexception = false;
                                        }
                                        else
                                        {
                                            isEligibleException = false;
                                        }
                                        if ((leaveException?.MaritalStatus_Single_Exception == true && item?.MaritalStatusSingle == true) || (leaveException?.MaritalStatus_Married_Exception == true && item?.MaritalStatusMarried == true))
                                        {
                                            leaveexception = true;
                                            isException = true;
                                        }
                                        else if (leaveException?.MaritalStatus_Single_Exception != true && leaveException?.MaritalStatus_Married_Exception != true)
                                        {
                                            leaveexception = false;
                                        }
                                        else
                                        {
                                            isEligibleException = false;
                                        }
                                        //employee exception
                                        bool employeeException = false;
                                        List<EmployeeApplicableLeave> employeeExceptionLeaves = _employeeApplicableLeaveRepository.GetEmployeeExceptionLeaveDetailsByLeaveTypeId(leaveDetails.LeaveTypeId);
                                        if (employeeExceptionLeaves.Any(x => x.LeaveExceptionEmployeeId != 0) && employeeExceptionLeaves.Any(m => m.LeaveExceptionEmployeeId == item.EmployeeID))
                                        {
                                            employeeException = true;
                                        }

                                        EmployeeLeaveDetails employeeLeaveDetails = new();
                                        if ((isEligibleException == false && leaveexception == false && exceptions == false && isException == false) && (employeeException == false))
                                        {
                                            if ((isEligible && leaveapplicable && applicable.IsApplicable && (isApplicable || applicable.IsCreteriaMached)) || (employeeApplicable))
                                            {
                                                var quarterly = GetFinancialYearQuarter(todayDate);
                                                var halfyearly = GetFinancialYearHalfYearly(todayDate);
                                                var yearly = GetFinancialYearly(todayDate);
                                                //decimal proratevalue = default;
                                                DateTime FromDate = (item.DateOfJoining != null && leaveDetails?.EffectiveFromDate <= item?.DateOfJoining) ? (DateTime)item.DateOfJoining : (DateTime)leaveDetails.EffectiveFromDate;
                                                DateTime ToDate = (leaveDetails.EffectiveToDate != null && leaveDetails?.EffectiveToDate.Value.Date <= DateTime.Now.Date) ? leaveDetails.EffectiveToDate.Value.Date : DateTime.Now.Date;
                                                //Prorate
                                                if (appConstants?.DisplayName?.ToLower() == "Monthly".ToLower())
                                                {
                                                    if (leaveTypesList?.ProRate == true)
                                                    {
                                                        proRateMonthDetails = _proRateMonthDetailsRepository.GetProRateMonthDetailsByID(leaveDetails.LeaveTypeId);
                                                        DateTime monthStartDate = new DateTime(FromDate.Year, FromDate.Month, 1);
                                                        DateTime monthToDate = monthStartDate.AddMonths(1).AddDays(-1);
                                                        if (FromDate.Date <= todayDate.Date && monthToDate.Date >= todayDate.Date)
                                                        {
                                                            string ProrateFromday = "";
                                                            string ProrateToday = "";
                                                            foreach (ProRateMonthDetailsView ProRateitem in proRateMonthDetails)
                                                            {
                                                                if (ProRateitem?.Fromday.ToLower() == "firstdays".ToLower())
                                                                {
                                                                    ProrateFromday = "1";
                                                                }
                                                                else if (ProRateitem?.Fromday.ToLower() == "lastdays".ToLower())
                                                                {
                                                                    ProrateFromday = Convert.ToString(FromDate.Day);
                                                                }
                                                                else
                                                                {
                                                                    ProrateFromday = ProRateitem.Fromday;
                                                                }
                                                                if (ProRateitem?.Today.ToLower() == "firstdays".ToLower())
                                                                {
                                                                    ProrateToday = "1";
                                                                }
                                                                else if (ProRateitem?.Today.ToLower() == "lastdays".ToLower())
                                                                {
                                                                    ProrateToday = Convert.ToString(FromDate.Day);
                                                                }
                                                                else
                                                                {
                                                                    ProrateToday = ProRateitem?.Today;
                                                                }
                                                                //fromdate                                                
                                                                var fromDate = new DateTime(FromDate.Year, FromDate.Month, Convert.ToInt32(ProrateFromday));
                                                                //todate
                                                                var toDate = new DateTime(FromDate.Year, FromDate.Month, Convert.ToInt32(ProrateToday));
                                                                if ((fromDate <= FromDate.Date && FromDate.Date <= toDate))
                                                                {
                                                                    creditdates = (decimal)ProRateitem?.Count;
                                                                    break;
                                                                }
                                                                //proratevalue = (decimal)ProRateitem.Count;
                                                            }
                                                        }
                                                        else if (todayDate >= FromDate)
                                                        {
                                                            creditdates = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays;
                                                        }
                                                    }
                                                    else
                                                    {
                                                        creditdates = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays;
                                                    }
                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "Quarterly".ToLower())
                                                {
                                                    if (leaveTypesList.ProRate == true)
                                                    {
                                                        FinancialYearDateView duration = GetFinancialYearQuarter(todayDate);
                                                        int totalMonths = 0;
                                                        DateTime toDate = DateTime.Now;
                                                        FromDate = (FromDate.Date <= duration.FromDate.Date) ? (DateTime)duration.FromDate.Date : FromDate.Date;
                                                        toDate = (leaveDetails.EffectiveToDate != null && leaveDetails?.EffectiveToDate.Value.Date <= duration.ToDate.Date) ? leaveDetails.EffectiveToDate.Value.Date : duration.ToDate.Date;
                                                        totalMonths = ((toDate.Year * 12) + toDate.Month) - ((FromDate.Year * 12) + FromDate.Month) + 1;
                                                        decimal leaveAccrued = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays / 3;
                                                        creditdates = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;
                                                    }
                                                    else
                                                    {
                                                        creditdates = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays;
                                                    }
                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "HalfYearly".ToLower())
                                                {
                                                    if (leaveTypesList.ProRate == true)
                                                    {
                                                        FinancialYearDateView duration = GetFinancialYearHalfYearly(todayDate);
                                                        int totalMonths = 0;
                                                        DateTime toDate = DateTime.Now;
                                                        FromDate = (FromDate.Date <= duration.FromDate.Date) ? (DateTime)duration.FromDate.Date : FromDate.Date;
                                                        toDate = (leaveDetails.EffectiveToDate != null && leaveDetails?.EffectiveToDate.Value.Date <= duration.ToDate.Date) ? leaveDetails.EffectiveToDate.Value.Date : duration.ToDate.Date;
                                                        totalMonths = ((toDate.Year * 12) + toDate.Month) - ((FromDate.Year * 12) + FromDate.Month) + 1;
                                                        decimal leaveAccrued = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays / 6;
                                                        creditdates = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;
                                                    }
                                                    else
                                                    {
                                                        creditdates = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays;
                                                    }
                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "Yearly".ToLower())
                                                {
                                                    if (leaveTypesList.ProRate == true)
                                                    {
                                                        var duration = GetFinancialYearly(todayDate);
                                                        int totalMonths = 0;
                                                        DateTime toDate = DateTime.Now;
                                                        FromDate = (FromDate.Date <= duration.FromDate.Date) ? (DateTime)duration.FromDate.Date : FromDate.Date;
                                                        toDate = (leaveDetails.EffectiveToDate != null && leaveDetails.EffectiveToDate.Value.Date <= duration.ToDate.Date) ? leaveDetails.EffectiveToDate.Value.Date : duration.ToDate.Date;
                                                        totalMonths = ((toDate.Year * 12) + toDate.Month) - ((FromDate.Year * 12) + FromDate.Month) + 1;
                                                        decimal leaveAccrued = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays / 12;
                                                        creditdates = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;
                                                    }
                                                    else
                                                    {
                                                        creditdates = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays;
                                                    }
                                                }
                                                else
                                                {
                                                    creditdates = 0;
                                                }

                                                employeeLeaveDetails = _employeeLeaveDetailsRepository?.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(item.EmployeeID, leaveDetails.LeaveTypeId);

                                                if (employeeLeaveDetails == null)
                                                {
                                                    employeeLeaveDetails = new()
                                                    {
                                                        EmployeeID = item.EmployeeID,
                                                        LeaveTypeID = leaveDetails?.LeaveTypeId,
                                                        BalanceLeave = creditdates,
                                                        CreatedOn = DateTime.UtcNow
                                                    };
                                                    await _employeeLeaveDetailsRepository.AddAsync(employeeLeaveDetails);
                                                    await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                                }
                                                else
                                                {

                                                    employeeLeaveDetails.BalanceLeave += creditdates;
                                                    employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                                    if (employeeLeaveDetails.AdjustmentEffectiveFromDate != null)
                                                    {
                                                        employeeLeaveDetails.AdjustmentBalanceLeave = employeeLeaveDetails.AdjustmentBalanceLeave == null ? null : employeeLeaveDetails.AdjustmentBalanceLeave + creditdates;
                                                    }
                                                    _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                                    await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                                }
                                            }
                                        }
                                        //else
                                        //{
                                        //    List<ApplyLeaves> leavelist = _LeaveApplyRepository.GetAppliedleaveByEmployeeId(item.EmployeeID, leaveDetails.LeaveTypeId);
                                        //    if (leavelist?.Count > 0)
                                        //    {
                                        //        foreach (ApplyLeaves appliedLeaves in leavelist)
                                        //        {
                                        //            if (appliedLeaves.LeaveId > 0)
                                        //            {
                                        //                List<AppliedLeaveDetails> appliedLeaveDetails = _AppliedLeaveDetailsRepository.GetByID(appliedLeaves.LeaveId);
                                        //                foreach (AppliedLeaveDetails appliedLeave in appliedLeaveDetails)
                                        //                {
                                        //                    _AppliedLeaveDetailsRepository.Delete(appliedLeave);
                                        //                    await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                        //                }
                                        //                ApplyLeaves ApplyLeaves = _LeaveApplyRepository.GetAppliedleaveByIdToDelete(appliedLeaves.LeaveId);
                                        //                if (ApplyLeaves != null && ApplyLeaves?.LeaveId > 0)
                                        //                {
                                        //                    _LeaveApplyRepository.Delete(ApplyLeaves);
                                        //                    await _LeaveApplyRepository.SaveChangesAsync();
                                        //                }

                                        //            }
                                        //        }
                                        //    }
                                        //    EmployeeLeaveDetails empLeaveDetail = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(item.EmployeeID, leaveDetails.LeaveTypeId);
                                        //    if (empLeaveDetail != null)
                                        //    {
                                        //        _employeeLeaveDetailsRepository.Delete(empLeaveDetail);
                                        //        await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                        //    }

                                        //}

                                    }
                                    catch (Exception ex)
                                    {
                                        LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AccruedEmployeeLeaves Employee Id - ", Convert.ToString(item.EmployeeID));
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AccruedEmployeeLeaves LeaveTypeId - ", Convert.ToString(leaveTypesList.LeaveTypeId));
                        }

                        try
                        {
                            foreach (EmployeeDetailsForLeaveView item in EmployeeList)
                            {
                                try
                                {
                                    LeaveAdjustmentDetails adjustmentDetails = _leaveAdjustmentDetailsRepository.GetByDateAndEmployeeId(item.EmployeeID,
                    todayDate.Date, leaveTypesList.LeaveTypeId);
                                    if (adjustmentDetails != null)
                                    {
                                        EmployeeLeaveDetails employeeLeaveDetails = new();
                                        employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(item.EmployeeID, leaveTypesList.LeaveTypeId);
                                        employeeLeaveDetails.AdjustmentBalanceLeave = adjustmentDetails?.AdjustmentBalance;
                                        employeeLeaveDetails.AdjustmentEffectiveFromDate = adjustmentDetails?.EffectiveFromDate;
                                        employeeLeaveDetails.AdjustmentDays = employeeLeaveDetails.AdjustmentDays == null ? adjustmentDetails.NoOfDays :
                                            employeeLeaveDetails.AdjustmentDays == null ? 0 : employeeLeaveDetails.AdjustmentDays + adjustmentDetails.NoOfDays == null ? 0 : adjustmentDetails.NoOfDays;
                                        _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                        await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                    }

                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AccruedEmployeeLeaves Employee Id - ", Convert.ToString(item.EmployeeID));
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AccruedEmployeeLeaves LeaveTypeId - ", Convert.ToString(leaveTypesList.LeaveTypeId));
                        }

                    }
                }

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveEmployeeMasterData");
            }
            return true;
        }
        #endregion

        #region Get Employee Holiday By Department
        public LeaveHolidayView GetEmployeeHolidayByDepartment(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            LeaveHolidayView holidayView = new();
            holidayView.Holiday = _holidayRepository.GetHolidayByDate(employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate, employeeLeaveandRestriction.LocationId, employeeLeaveandRestriction.ShiftId);
            holidayView.appliedLeaveDetails = _leaveRepository.GetEmployeeLeaves(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate);
            holidayView.leaveBalance = _LeaveApplyRepository.GetEmployeeLeaveBalance(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate);
            return holidayView;
        }
        #endregion

        #region Get Team Leaves and Rejection
        public TeamLeaveAndRejectionListView GetTeamLeaveAndRejection(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            TeamLeaveAndRejectionListView teamLeaveRejection = new();
            teamLeaveRejection.TeamLeaveView = _leaveRepository.GetTeamLeave(managerTeamLeaveView);
            teamLeaveRejection.LeaveRejectionReason = _LeaveRejectionReasonRepository.GetLeaveRejectionReason();
            if ((managerTeamLeaveView.PageNumber == 0 && !managerTeamLeaveView.IsFilterApplied))//(managerTeamLeaveView.ToDate - managerTeamLeaveView.FromDate).Days >= 364) 
            {
                teamLeaveRejection.LeaveTypesView = (from leave in _leaveTypeRepository.GetAllLeaveType()
                    select new LeaveTypesView
                    {
                        LeaveTypeId = leave.LeaveTypeId,
                        LeaveType = leave.LeaveType
                    }).ToList(); 
                teamLeaveRejection.LeaveStatus = _leaveRepository.GetLeaveStatusToDisplay();
            }
            return teamLeaveRejection;
        }
        #endregion

        #region Get Available Leave and Duration Detail By Employee Id
        public AvailableLeaveAndDurationDetailsView GetAvailableLeaveAndDurationDetailsByEmployeeId(int employeeId, int departmentId)
        {
            AvailableLeaveAndDurationDetailsView availableLeaveAndDuration = new();
            availableLeaveAndDuration.AvailableLeaveDetailsView = _LeaveApplyRepository.GetAvailableLeaveDetailsByEmployeeId(employeeId, departmentId);
            availableLeaveAndDuration.LeaveDurationListView = _LeaveApplyRepository.GetAvailableLeaveDurationList();
            return availableLeaveAndDuration;
        }
        #endregion

        #region Get Employee Leave and Restriction Details By Employee Id
        public async Task<IndividualLeaveList> GetEmployeeLeaveAndRestrictionDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            individualLeaveList.EmployeeAvailableLeaveDetails = _leaveRestrictionsRepository.GetEmployeeLeaveAndRestrictionDetails(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate, employeeLeaveandRestriction.DateOfJoining);
            //individualLeaveList.HolidayList = _holidayRepository.GetCurrentFinancialYearHolidayList(employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate, employeeLeaveandRestriction.LocationId, employeeLeaveandRestriction.ShiftId, employeeLeaveandRestriction.DateOfJoining);
            if (individualLeaveList?.EmployeeAvailableLeaveDetails?.Count > 0)
            {
                int currrentFinanceYear = DateTime.Now.Year;
                List<EmployeeAvailableLeaveDetails> EmployeeAvailableLeaveListDetails = new List<EmployeeAvailableLeaveDetails>();
                if (DateTime.Now.Month > 3)
                    currrentFinanceYear = DateTime.Now.Year;
                else
                    currrentFinanceYear = DateTime.Now.Year - 1;

                foreach (EmployeeAvailableLeaveDetails item in individualLeaveList?.EmployeeAvailableLeaveDetails)
                {
                    if (!string.IsNullOrEmpty(item.ResetPeriod) && !string.IsNullOrEmpty(item.ResetDay))
                    {
                        item.LeaveResetOn = GetLeaveResetDate(item.ResetPeriod, item.ResetDay, item.ResetMonth == null ? 0 : (int)item.ResetMonth);
                    }

                    if (item?.LeaveAdjustmentDetails?.Count > 0)
                    {
                        var leaveAdjustmentList = (from le in item.LeaveAdjustmentDetails
                                                   group le by le.EffectiveFromDate into ad
                                                   select ad).ToList();
                        List<LeaveAdjustmentDetails> adjustment = new List<LeaveAdjustmentDetails>();
                        foreach (var detail in leaveAdjustmentList)
                        {
                            LeaveAdjustmentDetails data = detail.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                            adjustment.Add(data);
                        }
                        item.LeaveAdjustmentDetails = adjustment;
                    }
                    //Calculate grant leave balance
                    if (item.BalanceBasedOn.Select(x => x.BalanceBasedOnValue).FirstOrDefault() == "LeaveGrant")
                    {
                        decimal balance = 0;
                        balance = GetGrantLeaveCardBalance(employeeLeaveandRestriction.EmployeeId, item.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, employeeLeaveandRestriction.FromDate, DateTime.Now.Date, item.LeaveGrantRequestDetails, item.AppliedLeaveDates);
                        if (employeeLeaveandRestriction.FromDate.Year != currrentFinanceYear)
                        {
                            item.DisplayBalanceLeave = _leaveGrantRequestDetailsRepository.GetGrantRequestCardBalanceByDate(employeeLeaveandRestriction.EmployeeId, item.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, employeeLeaveandRestriction.ToDate);
                        }
                        else
                        {
                            item.DisplayBalanceLeave = balance;
                        }

                        //decimal balance = GetGrantLeaveBalance(employeeLeaveandRestriction.EmployeeId,item.LeaveTypeID==null?0:(int)item.LeaveTypeID, DateTime.Now.Date,0);
                        item.ActualBalanceLeave = balance;
                        item.BalanceLeave = balance;
                        item.AdjustmentBalanceLeave = 0;
                        item.AdjustmentEffectiveFromDate = null;
                    }
                    else
                    {

                        //AppConstants appConstantBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);
                        if (employeeLeaveandRestriction.FromDate.Year != currrentFinanceYear)
                        {

                            DateTime fromDate = DateTime.Now.Date;
                            DateTime toDate = employeeLeaveandRestriction.ToDate;
                            if (employeeLeaveandRestriction.DateOfJoining != null && employeeLeaveandRestriction.DateOfJoining > fromDate)
                            {
                                fromDate = (DateTime)employeeLeaveandRestriction.DateOfJoining;
                            }
                            if (item.EffectiveFromDate != null && item.EffectiveFromDate > fromDate)
                            {
                                fromDate = (DateTime)item.EffectiveFromDate;
                            }
                            if (item.EffectiveToDate != null && item.EffectiveToDate < toDate)
                            {
                                fromDate = (DateTime)item.EffectiveToDate;
                            }
                            if (employeeLeaveandRestriction.DateOfJoining != null && employeeLeaveandRestriction.DateOfJoining > toDate)
                            {
                                toDate = (DateTime)employeeLeaveandRestriction.DateOfJoining;
                            }
                            List<AppliedLeaveTypeDetails> appliedLeaveDetails = _leaveRepository.GetAppliedLeaveByLeaveTypeAndDate(item?.EmployeeID == null ? 0 : (int)item.EmployeeID, fromDate, toDate, item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID);
                            List<LeaveAdjustmentDetails> leaveAdjustmentDetails = _leaveAdjustmentDetailsRepository.GetLeaveAdjustmentDetailsByDate(item?.EmployeeID == null ? 0 : (int)item.EmployeeID, item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, fromDate, toDate);
                            LeaveCarryForwardListView leaveCarryForwardDetails = _leaveEntitlementRepository.GetLeaveTypeCarryForwardDetails(item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID);
                            item.DisplayBalanceLeave = await GetFinancialYearLeaveBalance(item?.ActualBalanceLeave == null ? 0 : (decimal)item.ActualBalanceLeave, fromDate, toDate, employeeLeaveandRestriction.EmployeeId, item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, appliedLeaveDetails, leaveAdjustmentDetails, leaveCarryForwardDetails);
                        }

                    }
                    if (item.MaximumCarryForwardDays != null)
                    {
                        item.MaximumCarryForwardDays = GetRoundOff((decimal)item.MaximumCarryForwardDays);
                    }
                    if (item.ActualBalanceLeave != null)
                    {
                        item.ActualBalanceLeave = GetRoundOffValues((decimal)item.ActualBalanceLeave);
                    }
                    if (item.AdjustmentBalanceLeave != null)
                    {
                        item.AdjustmentBalanceLeave = GetRoundOffValues((decimal)item.AdjustmentBalanceLeave);
                    }
                    if (item.MaximumConsecutiveDays != null)
                    {
                        item.MaximumConsecutiveDays = GetHalfDayValues((decimal)item.MaximumConsecutiveDays);
                    }
                    if (item.MaximumLeavePerApplication != null)
                    {
                        item.MaximumLeavePerApplication = GetHalfDayValues((decimal)item.MaximumLeavePerApplication);
                    }
                    if (item.MinimumGapTwoApplication != null)
                    {
                        item.MinimumGapTwoApplication = GetHalfDayValues((decimal)item.MinimumGapTwoApplication);
                    }
                    if (item.MinimumNoOfApplicationsPeriod != null)
                    {
                        item.MinimumNoOfApplicationsPeriod = GetRoundOff((decimal)item.MinimumNoOfApplicationsPeriod);
                    }
                    if (item.EnableFileUpload != null)
                    {
                        item.EnableFileUpload = GetHalfDayValues((decimal)item.EnableFileUpload);
                    }
                    if (item.BalanceLeave != null)
                    {
                        item.BalanceLeave = GetRoundOff((decimal)item.BalanceLeave);
                    }
                    if (item.GrantMinimumNoOfRequestDay != null)
                    {
                        item.GrantMinimumNoOfRequestDay = GetHalfDayValues((decimal)item.GrantMinimumNoOfRequestDay);
                    }
                    if (item.GrantMaximumNoOfRequestDay != null)
                    {
                        item.GrantMaximumNoOfRequestDay = GetHalfDayValues((decimal)item.GrantMaximumNoOfRequestDay);
                    }
                    if (item.GrantUploadDocumentSpecificPeriodDay != null)
                    {
                        item.GrantUploadDocumentSpecificPeriodDay = GetHalfDayValues((decimal)item.GrantUploadDocumentSpecificPeriodDay);
                    }
                    if (item.DisplayBalanceLeave != null)
                    {
                        item.DisplayBalanceLeave = GetRoundOffValues((decimal)item.DisplayBalanceLeave);
                    }
                }
            }
            //individualLeaveList.AppliedLeaveList = _leaveRepository.GetAppliedLeaveByEmployeeId(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate);
            //individualLeaveList.HolidayDetails = _holidayRepository.GetHolidayDetails(employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate, employeeLeaveandRestriction.LocationId, employeeLeaveandRestriction.DateOfJoining);
            return individualLeaveList;
        }
        #endregion

        public async Task<decimal> GetFinancialYearLeaveBalance(decimal balance, DateTime fromDate, DateTime toDate, int employeeId, int leaveTypeId, List<AppliedLeaveTypeDetails> appliedLeaveDetails, List<LeaveAdjustmentDetails> leaveAdjustmentDetails, LeaveCarryForwardListView leaveCarryForwardDetails)
        {
            //List<AppliedLeaveTypeDetails> appliedLeaveDetails = _leaveRepository.GetAppliedLeaveByLeaveTypeAndDate(employeeId, fromDate, toDate, leaveTypeId);
            //List<LeaveAdjustmentDetails> leaveAdjustmentDetails = _leaveAdjustmentDetailsRepository.GetLeaveAdjustmentDetailsByDate(employeeId, leaveTypeId,fromDate, toDate);
            //LeaveCarryForwardListView leaveCarryForwardDetails = _leaveEntitlementRepository.GetLeaveTypeCarryForwardDetails(leaveTypeId);
            decimal balanceLeave = balance;
            
            LeaveTypes leaveTypeDetails = _leaveTypeRepository?.GetByID(leaveTypeId);
            if (toDate.Date <= DateTime.Now.Date)
            {
                DateTime resetDate = GetResetDate(toDate, leaveTypeId, leaveCarryForwardDetails);
                for (DateTime day = DateTime.Now.Date; toDate.Date <= day; day = day.AddDays(-1))
                {
                    //Check leave accured

                    FinancialYearDateView periodDate = new FinancialYearDateView();

                    //Prorate 
                    AppConstants leaveAccuredType = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.LeaveAccruedType);
                    if (leaveAccuredType?.AppConstantValue?.ToLower() == "Monthly".ToLower())
                    {
                        string leaveDay = "";
                        if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays")
                        {
                            leaveDay = "1";
                        }
                        else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays")
                        {

                            leaveDay = Convert.ToString(DateTime.DaysInMonth(day.Year, day.Month));
                        }
                        else
                        {
                            leaveDay = leaveTypeDetails?.LeaveAccruedDay;
                        }
                        if (Convert.ToInt32(leaveDay) == day.Date.Day)
                        {
                            balanceLeave = balanceLeave - (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                        }
                    }
                    else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower())
                    {
                        periodDate = GetFinancialYearQuarter(day);
                    }
                    else if (leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower())
                    {
                        periodDate = GetFinancialYearHalfYearly(day);
                    }
                    else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                    {
                        periodDate = GetFinancialYearly(day);
                    }
                    if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower() ||
                        leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower() ||
                        leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                    {
                        if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                        {
                            if (periodDate.FromDate.Date == day.Date)
                            {
                                balanceLeave = balanceLeave - (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                            }
                        }
                        else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                        {
                            if (periodDate.ToDate.Date == day.Date)
                            {
                                balanceLeave = balanceLeave - (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                            }
                        }
                    }

                    //Check Applied leave dates
                    List<AppliedLeaveTypeDetails> appliedLeave = appliedLeaveDetails.Where(x => x.Date == day.Date).Select(x => x).ToList();
                    if (appliedLeave?.Count > 0)
                    {
                        foreach (var item in appliedLeave)
                        {
                            if (item.IsFullDay)
                            {
                                balanceLeave = balanceLeave + 1;
                            }
                            else
                            {
                                balanceLeave = balanceLeave + (decimal)0.5;
                            }
                        }
                    }

                    //Check leave Adjustment
                    LeaveAdjustmentDetails adjustment = leaveAdjustmentDetails.Where(x => x.EffectiveFromDate.Value.AddDays(-1).Date == day.Date).OrderByDescending(x => x.CreatedOn).Select(x => x).FirstOrDefault();
                    if (adjustment != null)
                    {
                        decimal val = (adjustment?.NoOfDays == null ? 0 : (decimal)adjustment.NoOfDays);
                        balanceLeave = balanceLeave - val;

                    }
                    LeaveAdjustmentDetails adjustmentDetail = leaveAdjustmentDetails.Where(x => x.EffectiveFromDate.Value.Date == day.Date).OrderByDescending(x => x.CreatedOn).Select(x => x).FirstOrDefault();
                    if (adjustmentDetail != null)
                    {
                        balanceLeave = adjustmentDetail?.AdjustmentBalance == null ? 0 : (decimal)adjustmentDetail.AdjustmentBalance;
                    }


                    //Check Carryforward details
                    if (day.Date == resetDate.Date)
                    {
                        LeaveCarryForward carryForward = _leaveCarryForwardRepository.GetLeaveCarryForwardByLeaveTypeId(employeeId, leaveTypeId, resetDate.Date);
                        if (carryForward != null)
                        {
                            if (carryForward.AdjustmentEffectiveFromDate != null)
                            {
                                balanceLeave = carryForward?.AdjustmentBalanceLeave == null ? 0 : (int)carryForward.AdjustmentBalanceLeave;
                            }
                            else
                            {
                                balanceLeave = carryForward.BalanceLeave;
                            }

                        }

                    }
                }
            }
            else
            {
                DateTime resetDate = GetResetDate(fromDate, leaveTypeId, leaveCarryForwardDetails);
                for (DateTime day = DateTime.Now.Date.AddDays(1); toDate.Date >= day; day = day.AddDays(1))
                {
                    //Check leave accured
                    //AppConstants appConstantBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);

                    FinancialYearDateView periodDate = new FinancialYearDateView();

                    //Check Carryforward details
                    if (day.Date == resetDate.AddDays(1).Date)
                    {
                        if (leaveCarryForwardDetails?.CarryForwardName?.ToLower() == "NoOfDays".ToLower())
                        {
                            balanceLeave = balanceLeave >= leaveCarryForwardDetails?.MaximumCarryForwardDays ? (leaveCarryForwardDetails?.MaximumCarryForwardDays == null ? 0 : (decimal)leaveCarryForwardDetails.MaximumCarryForwardDays) : balanceLeave;
                        }
                        else if (leaveCarryForwardDetails?.CarryForwardName?.ToLower() == "None".ToLower())
                        {
                            balanceLeave = 0;
                        }

                        if (leaveCarryForwardDetails?.Period?.ToLower() == "monthly".ToLower())
                        {
                            resetDate = resetDate.AddMonths(1);
                        }
                        else if (leaveCarryForwardDetails?.Period?.ToLower() == "quarterly".ToLower())
                        {
                            resetDate = resetDate.AddMonths(3);
                        }
                        else if (leaveCarryForwardDetails?.Period?.ToLower() == "halfyearly".ToLower())
                        {
                            resetDate = resetDate.AddMonths(6);
                        }
                        else if (leaveCarryForwardDetails?.Period?.ToLower() == "yearly".ToLower())
                        {
                            resetDate = resetDate.AddMonths(12);
                        }
                    }

                    //Prorate 
                    AppConstants leaveAccuredType = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.LeaveAccruedType);
                    if (leaveAccuredType?.AppConstantValue?.ToLower() == "Monthly".ToLower())
                    {
                        string leaveDay = "";
                        if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays")
                        {
                            leaveDay = "1";
                        }
                        else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays")
                        {

                            leaveDay = Convert.ToString(DateTime.DaysInMonth(day.Year, day.Month));
                        }
                        else
                        {
                            leaveDay = leaveTypeDetails?.LeaveAccruedDay;
                        }
                        if (Convert.ToInt32(leaveDay) == day.Date.Day)
                        {
                            balanceLeave = balanceLeave + (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                            //LoggerManager.LoggingMessage("Balance - " + balanceLeave + " Date - " + day.ToString() + " Leave id-" + leaveTypeDetails.LeaveTypeId);
                        }
                    }
                    else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower())
                    {
                        periodDate = GetFinancialYearQuarter(day);
                    }
                    else if (leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower())
                    {
                        periodDate = GetFinancialYearHalfYearly(day);
                    }
                    else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                    {
                        periodDate = GetFinancialYearly(day);
                    }
                    if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower() ||
                        leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower() ||
                        leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                    {
                        if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                        {
                            if (periodDate.FromDate.Date == day.Date)
                            {
                                balanceLeave = balanceLeave + (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                            }
                        }
                        else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                        {
                            if (periodDate.ToDate.Date == day.Date)
                            {
                                balanceLeave = balanceLeave + (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                            }
                        }
                    }



                    //Check Applied leave dates
                    List<AppliedLeaveTypeDetails> appliedLeave = appliedLeaveDetails.Where(x => x.Date == day.Date).Select(x => x).ToList();
                    if (appliedLeave?.Count > 0)
                    {
                        foreach (var item in appliedLeave)
                        {
                            if (item.IsFullDay)
                            {
                                balanceLeave = balanceLeave - 1;
                            }
                            else
                            {
                                balanceLeave = balanceLeave - (decimal)0.5;
                            }
                        }
                    }

                    //Check leave Adjustment
                    LeaveAdjustmentDetails adjustmentDetail = leaveAdjustmentDetails.Where(x => x.EffectiveFromDate.Value.Date == day.Date).OrderByDescending(x => x.CreatedOn).Select(x => x).FirstOrDefault();
                    if (adjustmentDetail != null)
                    {
                        balanceLeave = adjustmentDetail?.AdjustmentBalance == null ? 0 : (decimal)adjustmentDetail.AdjustmentBalance;
                    }
                }
            }
            return balanceLeave;
        }

        #region Get Employee Applied leave Details
        public IndividualLeaveList GetEmployeeAppliedLeaveDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            individualLeaveList.AppliedLeaveList = _leaveRepository.GetAppliedLeaveByEmployeeId(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate);
            individualLeaveList.HolidayDetails = _holidayRepository.GetHolidayDetails(employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate.AddYears(1), employeeLeaveandRestriction.LocationId, employeeLeaveandRestriction.DateOfJoining);
            //individualLeaveList.AppliedLeaveDetails= _leaveRepository.GetEmployeeLeaves(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate);
            return individualLeaveList;
        }
        #endregion
        #region Get Grant leave balance by date
        public decimal GetGrantLeaveBalance(int employeeId, int leaveTypeId, DateTime date, int grantRequestId)
        {
            decimal balanceLeave = 0;
            List<LeaveGrantRequestDetails> requestDetails = _leaveGrantRequestDetailsRepository.GetGrantRequestDetailsByLeaveTypeId(leaveTypeId, employeeId, date, grantRequestId);
            if (requestDetails?.Count > 0)
            {
                foreach (LeaveGrantRequestDetails request in requestDetails)
                {
                    if (request.BalanceDay != null && request.BalanceDay != 0 && (request.EffectiveToDate == null || request.EffectiveToDate >= date))
                    {
                        balanceLeave = balanceLeave + (decimal)request.BalanceDay;
                    }
                }
            }
            return balanceLeave;
        }
        #endregion

        #region Get Employee Applied leave Details
        public IndividualLeaveList GetAppliedLeaveDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            individualLeaveList.AppliedLeaveDetails = _leaveRepository.GetEmployeeLeaves(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate);
            individualLeaveList.HolidayDetails = _holidayRepository.GetHolidayDetails(employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate.AddYears(1), employeeLeaveandRestriction.LocationId, employeeLeaveandRestriction.DateOfJoining);
            return individualLeaveList;
        }
        #endregion

        #region Get Appconstant Type List
        public List<AppConstantsView> GetAppconstantList(string AppConstantType)
        {
            return _LeaveApplyRepository.GetAppconstantList(AppConstantType);
        }
        #endregion

        #region Get Appconstant By ID
        public AppConstants GetAppconstantByID(int appConstantID)
        {
            return _appConstantsRepository.GetAppconstantByID(appConstantID);
        }
        #endregion

        #region Get Leave Applicable
        public EmployeeLeaveApplicableView GetLeaveApplicable(LeaveTypes leavetype, EmployeeDetailsForLeaveView employeedetails)
        {
            EmployeeLeaveApplicableView applicable = new EmployeeLeaveApplicableView();
            applicable.IsCreteriaMached = true;
            try
            {
                List<LeaveDepartment> leaveDepartments = new();
                List<LeaveDesignation> leaveDesignations = new();
                List<LeaveLocation> leavelocation = new();
                List<LeaveRole> leaverole = new();
                List<LeaveEmployeeType> leaveEmployeeType = new();
                List<LeaveProbationStatus> leaveProbationStatuse = new();
                //List<EmployeeApplicableLeave> employeeApplicableLeaves = new();

                leaveDepartments = _leaveDepartmentRepository.GetLeaveApplicableDepartmentDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leaveDesignations = _leaveDesignationRepository.GetLeaveApplicableDesignationDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leavelocation = _leaveLocationRepository.GetLeaveApplicableLocationDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leaverole = _leaveRoleRepository.GetLeaveApplicableRoleDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leaveEmployeeType = _leaveEmployeeTypeRepository.GetLeaveEmployeeTypeDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leaveProbationStatuse = _leaveProbationStatuRepository.GetLeaveProbationStatusDetailsByLeaveTypeId(leavetype.LeaveTypeId);

                if (leaveDepartments.Any(x => x.LeaveApplicableDepartmentId != 0) && !leaveDepartments.Any(m => employeedetails.DepartmentID == m.LeaveApplicableDepartmentId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (leaveDesignations.Any(x => x.LeaveApplicableDesignationId != 0) && !leaveDesignations.Any(m => employeedetails.DesignationID == m.LeaveApplicableDesignationId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (leavelocation.Any(x => x.LeaveApplicableLocationId != 0) && !leavelocation.Any(m => employeedetails.LocationID == m.LeaveApplicableLocationId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (leaverole.Any(x => x.LeaveApplicableRoleId != 0) && !leaverole.Any(m => employeedetails.RoleID == m.LeaveApplicableRoleId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (leaveEmployeeType.Any(x => x.LeaveApplicableEmployeeTypeId != 0) && !leaveEmployeeType.Any(m => employeedetails.EmployeeTypeID == m.LeaveApplicableEmployeeTypeId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (leaveProbationStatuse.Any(x => x.LeaveApplicableProbationStatus != 0) && !leaveProbationStatuse.Any(m => employeedetails.ProbationStatusID == m.LeaveApplicableProbationStatus))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (leaverole?.Count == 0 && leavelocation?.Count == 0 && leaveDesignations?.Count == 0 && leaveDepartments?.Count == 0 && leaveEmployeeType?.Count == 0 && leaveProbationStatuse?.Count == 0)
                {
                    applicable.IsCreteriaMached = false;
                }
                applicable.IsApplicable = true;
                return applicable;


            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/GetLeaveApplicable");
            }
            applicable.IsApplicable = false;
            applicable.IsCreteriaMached = false;
            return applicable;
        }
        #endregion

        #region Get Leave Exceptions
        public bool GetLeaveExceptions(LeaveTypes leavetype, EmployeeDetailsForLeaveView employeedetails)
        {
            try
            {

                List<LeaveDepartment> leaveDepartments = new();
                List<LeaveDesignation> leaveDesignations = new();
                List<LeaveLocation> leavelocation = new();
                List<LeaveRole> leaverole = new();
                List<LeaveEmployeeType> leaveEmployeeType = new();
                List<LeaveProbationStatus> leaveProbationStatuse = new();

                leaveDepartments = _leaveDepartmentRepository.GetLeaveApplicableDepartmentDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leaveDesignations = _leaveDesignationRepository.GetLeaveApplicableDesignationDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leavelocation = _leaveLocationRepository.GetLeaveApplicableLocationDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leaverole = _leaveRoleRepository.GetLeaveApplicableRoleDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leaveEmployeeType = _leaveEmployeeTypeRepository.GetLeaveEmployeeTypeDetailsByLeaveTypeId(leavetype.LeaveTypeId);
                leaveProbationStatuse = _leaveProbationStatuRepository.GetLeaveProbationStatusDetailsByLeaveTypeId(leavetype.LeaveTypeId);

                var Exceleavdept = leaveDepartments.Any(m => employeedetails?.DepartmentID == m.LeaveExceptionDepartmentId);
                var Exceleavedesi = leaveDesignations.Any(m => employeedetails?.DesignationID == m.LeaveExceptionDesignationId);
                var Exceleveloc = leavelocation.Any(m => employeedetails?.LocationID == m.LeaveExceptionLocationId);
                var Exceleaverol = leaverole.Any(m => employeedetails?.RoleID == m.LeaveExceptionRoleId);
                var Execleavemptype = leaveEmployeeType.Any(m => employeedetails?.EmployeeTypeID == m.LeaveExceptionEmployeeTypeId);
                var Execleavprostatus = leaveProbationStatuse.Any(m => employeedetails?.ProbationStatusID == m.LeaveExceptionProbationStatus);
                bool exceptioncheck = false;

                if (leaveDepartments.Any(x => x.LeaveExceptionDepartmentId != 0) && !leaveDepartments.Any(m => employeedetails?.DepartmentID == m.LeaveExceptionDepartmentId))
                {
                    return false;
                }
                if (leaveDesignations.Any(x => x.LeaveExceptionDesignationId != 0) && !leaveDesignations.Any(m => employeedetails?.DesignationID == m.LeaveExceptionDesignationId))
                {
                    return false;
                }
                if (leavelocation.Any(x => x.LeaveExceptionLocationId != 0) && !leavelocation.Any(m => employeedetails?.LocationID == m.LeaveExceptionLocationId))
                {
                    return false;
                }
                if (leaverole.Any(x => x.LeaveExceptionRoleId != 0) && !leaverole.Any(m => employeedetails?.RoleID == m.LeaveExceptionRoleId))
                {
                    return false;
                }
                if (leaveEmployeeType.Any(x => x.LeaveExceptionEmployeeTypeId != 0) && !leaveEmployeeType.Any(m => employeedetails?.EmployeeTypeID == m.LeaveExceptionEmployeeTypeId))
                {
                    return false;
                }
                if (leaveProbationStatuse.Any(x => x.LeaveExceptionProbationStatus != 0) && !leaveProbationStatuse.Any(m => employeedetails?.ProbationStatusID == m.LeaveExceptionProbationStatus))
                {
                    return false;
                }


                if (leaveDepartments.Any(x => x.LeaveExceptionDepartmentId != 0) && leaveDepartments.Any(m => employeedetails?.DepartmentID == m.LeaveExceptionDepartmentId))
                {
                    exceptioncheck = true;
                }
                if (leaveDesignations.Any(x => x.LeaveExceptionDesignationId != 0) && leaveDesignations.Any(m => employeedetails?.DesignationID == m.LeaveExceptionDesignationId))
                {
                    exceptioncheck = true;
                }
                else if (leaveDesignations.Any(x => x.LeaveExceptionDesignationId != 0) && !leaveDesignations.Any(m => employeedetails?.DesignationID == m.LeaveExceptionDesignationId))
                {
                    return false;
                }
                if (leavelocation.Any(x => x.LeaveExceptionLocationId != 0) && leavelocation.Any(m => employeedetails?.LocationID == m.LeaveExceptionLocationId))
                {
                    exceptioncheck = true;
                }
                else if (leavelocation.Any(x => x.LeaveExceptionLocationId != 0) && !leavelocation.Any(m => employeedetails?.LocationID == m.LeaveExceptionLocationId))
                {
                    return false;
                }
                if (leaverole.Any(x => x.LeaveExceptionRoleId != 0) && leaverole.Any(m => employeedetails?.RoleID == m.LeaveExceptionRoleId))
                {
                    exceptioncheck = true;
                }
                else if (leaverole.Any(x => x.LeaveExceptionRoleId != 0) && !leaverole.Any(m => employeedetails?.RoleID == m.LeaveExceptionRoleId))
                {
                    return false;
                }
                if (leaveEmployeeType.Any(x => x.LeaveExceptionEmployeeTypeId != 0) && leaveEmployeeType.Any(m => employeedetails?.EmployeeTypeID == m.LeaveExceptionEmployeeTypeId))
                {
                    exceptioncheck = true;
                }
                else if (leaveEmployeeType.Any(x => x.LeaveExceptionEmployeeTypeId != 0) && !leaveEmployeeType.Any(m => employeedetails?.EmployeeTypeID == m.LeaveExceptionEmployeeTypeId))
                {
                    return false;
                }
                if (leaveProbationStatuse.Any(x => x.LeaveExceptionProbationStatus != 0) && leaveProbationStatuse.Any(m => employeedetails?.ProbationStatusID == m.LeaveExceptionProbationStatus))
                {
                    exceptioncheck = true;
                }
                else if (leaveProbationStatuse.Any(x => x.LeaveExceptionProbationStatus != 0) && !leaveProbationStatuse.Any(m => employeedetails?.ProbationStatusID == m.LeaveExceptionProbationStatus))
                {
                    return false;
                }

                return exceptioncheck;

                //if (Exceleavdept && Exceleavedesi && Exceleveloc && Exceleaverol)
                //{
                //    return true;
                //}
                //else
                //{
                //    return false;
                // }
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Insert One Time Leave
        public async Task<string> InsertOneTimeLeave(OneTimeEmployeeLeaveView EmployeeList)
        {
            try
            {
                AppConstants appConstantsBasedOn = new();
                LeaveTypes leaveTypeDetails = new();
                leaveTypeDetails = _leaveTypeRepository?.GetByID(EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);

                appConstantsBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);
                // Doubt if edit means ?
                //if (EmployeeList.IsNewLeave == true)
                //{
                //LeaveTypes leaveTypeDetails = new();
                List<LeaveTypes> leavetypeslist = new();
                LeaveApplicable leaveApplicable = new();
                List<ProRateMonthDetailsView> proRateMonthDetails = new();
                AppConstants appConstants = new();
                AppConstants appConstantsRes = new();
                List<EmployeeDetailsForLeaveView> EmployeeLists = EmployeeList?.EmployeeDetails;

                decimal afterroundoff = default;
                if (EmployeeList?.EmployeeDetails?.Count != 0)
                {
                    //leaveTypeDetails = _leaveTypeRepository?.GetByID(EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);
                    appConstants = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.LeaveAccruedType);
                    if (appConstants == null)
                    {
                        appConstants = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.LeaveTypesId);
                        if (appConstants?.AppConstantValue?.ToLower() == "RestrictedHoliday".ToLower())
                        {
                            appConstants = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.LeaveTypesId);
                        }
                    }
                    leaveApplicable = _leaveApplicableRepository.GetleaveApplicableByLeaveId(EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);

                    if (appConstants != null)
                    {
                        //EmployeeLists = EmployeeLists.Where(x => x.EmployeeID == 121).Select(x => x).ToList();
                        // Get All Employee List.
                        foreach (EmployeeDetailsForLeaveView item in EmployeeLists)
                        {
                            try
                            {

                                switch (item?.Gender?.ToLower())
                                {
                                    case "male":
                                        item.GenderMale = true;
                                        item.GenderFemale = null;
                                        item.GenderOther = null;

                                        break;
                                    case "female":
                                        item.GenderMale = null;
                                        item.GenderFemale = true;
                                        item.GenderOther = null;
                                        break;
                                    case "other":
                                        item.GenderMale = null;
                                        item.GenderFemale = null;
                                        item.GenderOther = true;
                                        break;
                                    default:
                                        item.GenderMale = null;
                                        item.GenderFemale = null;
                                        item.GenderOther = null;
                                        break;
                                }
                                switch (item?.MaritalStatus?.ToLower())
                                {
                                    case "single":
                                        item.MaritalStatusSingle = true;
                                        item.MaritalStatusMarried = null;
                                        break;
                                    case "married":
                                        item.MaritalStatusSingle = null;
                                        item.MaritalStatusMarried = true;
                                        break;
                                    default:
                                        item.MaritalStatusSingle = null;
                                        item.MaritalStatusMarried = null;
                                        break;
                                }
                                bool leaveapplicable = false;
                                bool isEligible = true;
                                bool isApplicable = false;
                                var prob = item.ProbationStatusID == 0 ? null : item.ProbationStatusID == null ? null : item.ProbationStatusID;
                                if ((leaveApplicable?.Gender_Male == true && item?.GenderMale == true) || (leaveApplicable?.Gender_Female == true && item?.GenderFemale == true) || (leaveApplicable?.Gender_Others == true && item?.GenderOther == true))
                                {
                                    leaveapplicable = true;
                                    isApplicable = true;
                                }
                                else if (leaveApplicable?.Gender_Male != true && leaveApplicable?.Gender_Female != true && leaveApplicable?.Gender_Others != true)
                                {
                                    leaveapplicable = true;
                                }
                                else
                                {
                                    isEligible = false;
                                }
                                if ((leaveApplicable?.MaritalStatus_Single == true && item?.MaritalStatusSingle == true) || (leaveApplicable?.MaritalStatus_Married == true && item?.MaritalStatusMarried == true))
                                {
                                    leaveapplicable = true;
                                    isApplicable = true;
                                }
                                else if (leaveApplicable?.MaritalStatus_Single != true && leaveApplicable?.MaritalStatus_Married != true)
                                {
                                    leaveapplicable = true;
                                }
                                else
                                {
                                    isEligible = false;
                                }
                                //if (leaveApplicable?.EmployeeTypeId != 0 && leaveApplicable?.EmployeeTypeId != item?.EmployeeTypeID)
                                //{
                                //    isEligible = false;
                                //}
                                //else if(leaveApplicable?.EmployeeTypeId !=0 && leaveApplicable?.EmployeeTypeId == item?.EmployeeTypeID)
                                //{
                                //    isApplicable = true;
                                //}
                                //else
                                //{
                                //    leaveapplicable = true;

                                //}
                                //if (leaveApplicable?.ProbationStatus != 0 && leaveApplicable?.ProbationStatus != prob)
                                //{
                                //    isEligible = false;
                                //}
                                //else if (leaveApplicable?.ProbationStatus != 0 && leaveApplicable?.ProbationStatus == prob)
                                //{
                                //    isApplicable = true;
                                //}
                                //else
                                //{
                                //    leaveapplicable = true;
                                //}

                                EmployeeLeaveApplicableView applicable = GetLeaveApplicable(leaveTypeDetails, item);
                                var exceptions = GetLeaveExceptions(leaveTypeDetails, item);

                                bool employeeApplicable = false;
                                List<EmployeeApplicableLeave> employeeApplicableLeaves = _employeeApplicableLeaveRepository.GetEmployeeApplicableLeaveDetailsByLeaveTypeId(EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);
                                if (employeeApplicableLeaves.Any(x => x.EmployeeId != 0) && employeeApplicableLeaves.Any(m => m.EmployeeId == item.EmployeeID))
                                {
                                    employeeApplicable = true;
                                }
                                bool leaveexception = false;
                                bool isEligibleException = false;
                                bool isException = false;
                                LeaveApplicable leaveException = new();
                                leaveException = _leaveApplicableRepository.GetleaveExceptionByLeaveId(EmployeeList.LeaveTypeDetails.LeaveTypeId);

                                if ((leaveException?.Gender_Male_Exception == true && item?.GenderMale == true) || (leaveException?.Gender_Female_Exception == true && item?.GenderFemale == true) || (leaveException?.Gender_Others_Exception == true && item?.GenderOther == true))
                                {
                                    leaveexception = true;
                                    isException = true;
                                }
                                else if (leaveException?.Gender_Male_Exception != true && leaveException?.Gender_Female_Exception != true && leaveException?.Gender_Others_Exception != true)
                                {
                                    leaveexception = false;
                                }
                                else
                                {
                                    isEligibleException = false;
                                }
                                if ((leaveException?.MaritalStatus_Single_Exception == true && item?.MaritalStatusSingle == true) || (leaveException?.MaritalStatus_Married_Exception == true && item?.MaritalStatusMarried == true))
                                {
                                    leaveexception = true;
                                    isException = true;
                                }
                                else if (leaveException?.MaritalStatus_Single_Exception != true && leaveException?.MaritalStatus_Married_Exception != true)
                                {
                                    leaveexception = false;
                                }
                                else
                                {
                                    isEligibleException = false;
                                }
                                //employee exception
                                bool employeeException = false;
                                List<EmployeeApplicableLeave> employeeExceptionLeaves = _employeeApplicableLeaveRepository.GetEmployeeExceptionLeaveDetailsByLeaveTypeId(EmployeeList.LeaveTypeDetails.LeaveTypeId);
                                if (employeeExceptionLeaves.Any(x => x.LeaveExceptionEmployeeId != 0) && employeeExceptionLeaves.Any(m => m.LeaveExceptionEmployeeId == item.EmployeeID))
                                {
                                    employeeException = true;
                                }
                                EmployeeLeaveDetails employeeLeaveDetails = new();
                                //if ((leaveapplicable == true || applicable) && exceptions == false)
                                bool deleteExisting = false;
                                if ((isEligibleException == false && leaveexception == false && exceptions == false && isException == false) && (employeeException == false))
                                {
                                    if ((isEligible && leaveapplicable && applicable.IsApplicable && (isApplicable || applicable.IsCreteriaMached)) || (employeeApplicable))
                                    {

                                        // leave credit only for  Balance Based On 
                                        AppConstants appConstantBasedOn = new();
                                        bool isBalanceChange = true;
                                        appConstantBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);
                                        if (appConstantBasedOn?.AppConstantValue?.ToLower() != "LeaveGrant".ToLower())
                                        {
                                            EmployeeLeaveDetails empDetail = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(item.EmployeeID, EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);

                                            if (EmployeeList.LeaveTypeDetails.LeaveTypeId > 0 && leaveTypeDetails.EffectiveFromDate?.Date == EmployeeList.LeaveTypeDetails.EffectiveFromDate?.Date &&
                                                leaveTypeDetails.LeaveAccruedNoOfDays == EmployeeList.LeaveTypeDetails.LeaveAccruedNoOfDays && empDetail != null)
                                            {
                                                afterroundoff = 0;
                                                isBalanceChange = false;
                                            }
                                            else
                                            {
                                                // total Months calculation                            
                                                DateTime From = (item.DateOfJoining != null && leaveTypeDetails?.EffectiveFromDate <= item.DateOfJoining) ? (DateTime)item.DateOfJoining : (DateTime)leaveTypeDetails.EffectiveFromDate; //Convert.ToDateTime("10/25/2021");
                                                DateTime To = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails?.EffectiveToDate.Value.Date <= DateTime.Now.Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : DateTime.Now.Date;

                                                // Get Total month count
                                                //TotalMonths = NoofMonths + 1;
                                                decimal? noofleavecredit = 0;
                                                int totalMonths = 0;
                                                proRateMonthDetails = _proRateMonthDetailsRepository.GetProRateMonthDetailsByID(EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);

                                                if (appConstants.DisplayName.ToLower() == "Monthly".ToLower())
                                                {
                                                    if (!string.IsNullOrEmpty(leaveTypeDetails.LeaveAccruedDay))
                                                    {
                                                        string leaveDay = "";
                                                        if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays")
                                                        {
                                                            leaveDay = "1";
                                                        }
                                                        else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays")
                                                        {

                                                            leaveDay = Convert.ToString(DateTime.DaysInMonth(To.Year, To.Month));
                                                        }
                                                        else
                                                        {
                                                            leaveDay = leaveTypeDetails?.LeaveAccruedDay;
                                                        }
                                                        if (leaveDay != "" && Convert.ToInt32(leaveDay) <= DateTime.Now.Day)
                                                        {
                                                            totalMonths = ((To.Year * 12) + To.Month) - ((From.Year * 12) + From.Month) + 1;
                                                        }
                                                        else
                                                        {
                                                            totalMonths = ((To.Year * 12) + To.Month) - ((From.Year * 12) + From.Month);
                                                            DateTime previousMonth = new DateTime(To.Year, To.Month, 1);
                                                            To = previousMonth.AddDays(-1);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        totalMonths = ((To.Year * 12) + To.Month) - ((From.Year * 12) + From.Month) + 1;
                                                    }

                                                    if (leaveTypeDetails.ProRate == true)
                                                    {

                                                        decimal? prorateleave = 0;
                                                        for (DateTime dt = From; dt <= To; dt = dt.AddMonths(1))
                                                        {
                                                            var MonthStart = new DateTime(dt.Year, dt.Month, 1);
                                                            var MonthLast = MonthStart.AddMonths(1).AddDays(-1);
                                                            string ProrateFromday = null;
                                                            string ProrateToday = null;
                                                            //decimal proratevalue = default;
                                                            prorateleave = 0;
                                                            if (MonthStart <= From.Date && From.Date <= MonthLast)
                                                            {
                                                                foreach (ProRateMonthDetailsView ProRateitem in proRateMonthDetails)
                                                                {
                                                                    if (ProRateitem?.Fromday?.ToLower() == "firstdays".ToLower())
                                                                    {
                                                                        ProrateFromday = "1";
                                                                    }
                                                                    else if (ProRateitem?.Fromday?.ToLower() == "lastdays".ToLower())
                                                                    {
                                                                        ProrateFromday = Convert.ToString(MonthLast.Day);
                                                                    }
                                                                    else
                                                                    {
                                                                        ProrateFromday = ProRateitem?.Fromday;
                                                                    }
                                                                    if (ProRateitem?.Today?.ToLower() == "firstdays".ToLower())
                                                                    {
                                                                        ProrateToday = "1";
                                                                    }
                                                                    else if (ProRateitem?.Today?.ToLower() == "lastdays".ToLower())
                                                                    {
                                                                        ProrateToday = Convert.ToString(MonthLast.Day);
                                                                    }
                                                                    else
                                                                    {
                                                                        ProrateToday = ProRateitem?.Today;
                                                                    }
                                                                    //fromdate                                                
                                                                    var fromDate = new DateTime(dt.Year, dt.Month, Convert.ToInt32(ProrateFromday));
                                                                    //todate
                                                                    var toDate = new DateTime(dt.Year, dt.Month, Convert.ToInt32(ProrateToday));
                                                                    if ((fromDate <= From.Date && From.Date <= toDate))
                                                                    {
                                                                        prorateleave = (decimal)ProRateitem.Count;
                                                                        break;
                                                                    }
                                                                    //proratevalue = (decimal)ProRateitem.Count;
                                                                }

                                                            }
                                                            else if (From.Date < MonthStart)
                                                            {

                                                                prorateleave = leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays;
                                                            }
                                                            noofleavecredit += prorateleave;

                                                        }
                                                    }
                                                    else
                                                    {
                                                        noofleavecredit = leaveTypeDetails.LeaveAccruedNoOfDays * totalMonths;
                                                    }


                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "Quarterly".ToLower())
                                                {
                                                    FinancialYearDateView startqtr = GetFinancialYearQuarter(From);
                                                    FinancialYearDateView endqtr = GetFinancialYearQuarter(To);
                                                    totalMonths = 0;
                                                    bool isCurrentPeriod = false;
                                                    if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                                    {
                                                        isCurrentPeriod = true;
                                                    }
                                                    else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                                                    {
                                                        if (DateTime.Now.Date == endqtr.ToDate.Date)
                                                        {
                                                            isCurrentPeriod = true;
                                                        }
                                                    }
                                                    DateTime toDate = DateTime.Now;
                                                    if (isCurrentPeriod)
                                                    {
                                                        toDate = (leaveTypeDetails?.EffectiveToDate != null && leaveTypeDetails?.EffectiveToDate.Value.Date <= endqtr.ToDate.Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endqtr.ToDate.Date;

                                                    }
                                                    else
                                                    {
                                                        toDate = (leaveTypeDetails?.EffectiveToDate != null && leaveTypeDetails?.EffectiveToDate.Value.Date <= endqtr.FromDate.Date.AddDays(-1).Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endqtr.FromDate.Date.AddDays(-1).Date;
                                                        //totalMonths = ((noOfMonth.Year * 12) + noOfMonth.Month) - ((From.Year * 12) + From.Month) + 1;
                                                    }
                                                    if (leaveTypeDetails.ProRate == true)
                                                    {
                                                        totalMonths = ((toDate.Year * 12) + toDate.Month) - ((From.Year * 12) + From.Month) + 1;
                                                        decimal leaveAccrued = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays / 3;

                                                        noofleavecredit = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;

                                                    }
                                                    else
                                                    {
                                                        int duration = 0;
                                                        for (DateTime date = startqtr.FromDate.Date; date <= toDate; date = date.AddMonths(3))
                                                        {
                                                            duration = duration + 1;
                                                        }
                                                        noofleavecredit = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : ((decimal)leaveTypeDetails.LeaveAccruedNoOfDays * duration);
                                                    }


                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "HalfYearly".ToLower())
                                                {
                                                    var starthalf = GetFinancialYearHalfYearly(From);
                                                    var endhalf = GetFinancialYearHalfYearly(To);
                                                    totalMonths = 0;
                                                    bool isCurrentPeriod = false;
                                                    if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                                    {
                                                        isCurrentPeriod = true;
                                                    }
                                                    else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                                                    {
                                                        if (DateTime.Now.Date == endhalf.ToDate.Date)
                                                        {
                                                            isCurrentPeriod = true;
                                                        }
                                                    }
                                                    DateTime toDate = DateTime.Now;
                                                    if (isCurrentPeriod)
                                                    {
                                                        toDate = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails.EffectiveToDate.Value.Date <= endhalf.ToDate.Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endhalf.ToDate.Date;
                                                        //totalMonths = ((noOfMonth.Year * 12) + noOfMonth.Month) - ((From.Year * 12) + From.Month) + 1;
                                                    }
                                                    else
                                                    {
                                                        toDate = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails.EffectiveToDate.Value.Date <= endhalf.FromDate.Date.AddDays(-1).Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endhalf.FromDate.Date.AddDays(-1).Date;

                                                    }
                                                    if (leaveTypeDetails.ProRate == true)
                                                    {
                                                        totalMonths = ((toDate.Year * 12) + toDate.Month) - ((From.Year * 12) + From.Month) + 1;
                                                        decimal leaveAccrued = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays / 6;
                                                        noofleavecredit = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;
                                                    }
                                                    else
                                                    {
                                                        int duration = 0;
                                                        for (DateTime date = starthalf.FromDate.Date; date <= toDate; date = date.AddMonths(6))
                                                        {
                                                            duration = duration + 1;
                                                        }
                                                        noofleavecredit = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : ((decimal)leaveTypeDetails.LeaveAccruedNoOfDays * duration);
                                                    }


                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "Yearly".ToLower())
                                                {
                                                    var startyear = GetFinancialYearly(From);
                                                    var endyear = GetFinancialYearly(To);
                                                    bool isCurrentPeriod = false;
                                                    totalMonths = 0;
                                                    if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                                    {
                                                        isCurrentPeriod = true;
                                                    }
                                                    else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                                                    {
                                                        if (DateTime.Now.Date == endyear.ToDate.Date)
                                                        {
                                                            isCurrentPeriod = true;
                                                        }
                                                    }
                                                    DateTime toDate = DateTime.Now;
                                                    if (isCurrentPeriod)
                                                    {
                                                        toDate = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails.EffectiveToDate.Value.Date <= endyear.ToDate.Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endyear.ToDate.Date;
                                                        //totalMonths = ((noOfMonth.Year * 12) + noOfMonth.Month) - ((From.Year * 12) + From.Month) + 1;
                                                    }
                                                    else
                                                    {
                                                        toDate = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails.EffectiveToDate.Value.Date <= endyear.FromDate.Date.AddDays(-1).Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endyear.FromDate.Date.AddDays(-1).Date;

                                                    }
                                                    if (leaveTypeDetails.ProRate == true)
                                                    {
                                                        totalMonths = ((toDate.Year * 12) + toDate.Month) - ((From.Year * 12) + From.Month) + 1;
                                                        decimal leaveAccrued = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays / 12;
                                                        noofleavecredit = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;
                                                    }
                                                    else
                                                    {
                                                        int duration = 0;
                                                        for (DateTime date = startyear.FromDate.Date; date <= toDate; date = date.AddMonths(12))
                                                        {
                                                            duration = duration + 1;
                                                        }
                                                        noofleavecredit = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : ((decimal)leaveTypeDetails.LeaveAccruedNoOfDays * duration);
                                                    }
                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "One-time".ToLower())
                                                {
                                                    noofleavecredit = leaveTypeDetails?.LeaveAccruedNoOfDays;
                                                }
                                                afterroundoff = GetRoundOff((decimal)noofleavecredit);

                                            }

                                        }
                                        else
                                        {
                                            afterroundoff = 0;
                                        }

                                        employeeLeaveDetails = _employeeLeaveDetailsRepository?.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(item.EmployeeID, EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);
                                       
                                        if (employeeLeaveDetails == null)
                                        {
                                            employeeLeaveDetails = new()
                                            {
                                                EmployeeID = item.EmployeeID,
                                                LeaveTypeID = EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId,
                                                BalanceLeave = afterroundoff,
                                                CreatedOn = DateTime.UtcNow
                                            };
                                            await _employeeLeaveDetailsRepository.AddAsync(employeeLeaveDetails);
                                            await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                        }
                                        else if (appConstantBasedOn?.AppConstantValue?.ToLower() != "LeaveGrant".ToLower() && isBalanceChange == true)
                                        {
                                            decimal accruedDays = 0;
                                            //decimal adjustBalance = 0;
                                            List<AppliedLeaveTypeDetails> appliedLeaveDetails = new List<AppliedLeaveTypeDetails>();
                                            if (EmployeeList?.LeaveTypeDetails?.LeaveAccruedNoOfDays != null && EmployeeList?.LeaveTypeDetails?.LeaveAccruedNoOfDays > 0 && appConstants?.DisplayName?.ToLower() == "One-time".ToLower())
                                            {
                                                 appliedLeaveDetails = _leaveRepository.GetAppliedLeaveByLeaveTypeAndEmployee(item.EmployeeID, EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);                                    
                                            }
                                            else
                                            {
                                                DateTime CurrentFinanceFromDate = new DateTime(DateTime.Now.Month >= 4 ? DateTime.Now.Year : DateTime.Now.Year + 1, 4, 1);
                                                DateTime CurrentFinanceToDate = new DateTime(CurrentFinanceFromDate.Year + 1, 3, 31);
                                                appliedLeaveDetails = _leaveRepository.GetAppliedLeaveByLeaveTypeAndDate(item.EmployeeID, CurrentFinanceFromDate, CurrentFinanceToDate, EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);                                                
                                            }
                                            if (appliedLeaveDetails?.Count > 0)
                                            {
                                                decimal daysCount = 0;
                                                foreach (AppliedLeaveTypeDetails leaveDetail in appliedLeaveDetails)
                                                {
                                                    if (leaveDetail.IsFullDay)
                                                    {
                                                        daysCount += 1;
                                                    }
                                                    else if (leaveDetail.IsFirstHalf || leaveDetail.IsSecondHalf)
                                                    {
                                                        daysCount += (decimal)0.5;
                                                    }
                                                }
                                                accruedDays = afterroundoff - daysCount;

                                            }
                                            else
                                            {
                                                accruedDays = afterroundoff;
                                            }
                                            employeeLeaveDetails.BalanceLeave = accruedDays;
                                            employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                            employeeLeaveDetails.AdjustmentEffectiveFromDate = null;
                                            //if (employeeLeaveDetails.AdjustmentEffectiveFromDate != null)
                                            //{
                                            //    employeeLeaveDetails.AdjustmentBalanceLeave = adjustBalance > 0 ? adjustBalance : employeeLeaveDetails.AdjustmentBalanceLeave;
                                            //}
                                            _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                            await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                        }
                                    }
                                    else
                                    {
                                        deleteExisting = true;
                                    }
                                }
                                else
                                {
                                    deleteExisting = true;
                                }
                                if (deleteExisting == true)
                                {
                                    List<ApplyLeaves> leavelist = _LeaveApplyRepository.GetAppliedleaveByEmployeeId(item.EmployeeID, EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);
                                    if (leavelist?.Count > 0)
                                    {
                                        foreach (ApplyLeaves appliedLeaves in leavelist)
                                        {
                                            if (appliedLeaves.LeaveId > 0)
                                            {
                                                List<AppliedLeaveDetails> appliedLeaveDetails = _AppliedLeaveDetailsRepository.GetByID(appliedLeaves.LeaveId);
                                                foreach (AppliedLeaveDetails appliedLeave in appliedLeaveDetails)
                                                {
                                                    _AppliedLeaveDetailsRepository.Delete(appliedLeave);
                                                    await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                                }
                                                ApplyLeaves ApplyLeaves = _LeaveApplyRepository.GetAppliedleaveByIdToDelete(appliedLeaves.LeaveId);
                                                if (ApplyLeaves != null && ApplyLeaves.LeaveId > 0)
                                                {
                                                    _LeaveApplyRepository.Delete(ApplyLeaves);
                                                    await _LeaveApplyRepository.SaveChangesAsync();
                                                }

                                            }
                                        }
                                    }
                                    EmployeeLeaveDetails empLeaveDetail = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(item.EmployeeID, EmployeeList.LeaveTypeDetails == null ? 0 : EmployeeList.LeaveTypeDetails.LeaveTypeId);
                                    if (empLeaveDetail != null)
                                    {
                                        _employeeLeaveDetailsRepository.Delete(empLeaveDetail);
                                        await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                    }

                                }

                            }
                            catch (Exception ex)
                            {
                                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/InsertOneTimeLeave Employee Id - " + Convert.ToString(item.EmployeeID));
                            }

                        }
                    }

                }
                //}

                return "Success";
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Get Leave Balance
        public async Task<LeaveBalanceView> GetLeaveBalance(EmployeeListView employee)
        {
            //return _LeaveApplyRepository.GetLeaveBalanceByEmployeeId(employee);

            List<EmployeeAvailableLeaveDetails> employeeLeaveDetails = _LeaveApplyRepository.GetActiveLeaveBalanceDetails(employee);
            LeaveBalanceView leaveBalance = _LeaveApplyRepository.GetLeaveBalanceByEmployeeId(employee);
            int currrentFinanceYear = DateTime.Now.Year;
            if (DateTime.Now.Month > 3)
                currrentFinanceYear = DateTime.Now.Year;
            else
                currrentFinanceYear = DateTime.Now.Year - 1;

            if (employeeLeaveDetails.Count > 0)
            {
                foreach (EmployeeAvailableLeaveDetails item in employeeLeaveDetails)
                {

                    if (item.BalanceBasedOn.Select(x => x.BalanceBasedOnValue).FirstOrDefault() == "LeaveGrant")
                    {
                        item.ActualBalanceLeave = GetGrantLeaveCardBalance(item?.EmployeeID == null ? 0 : (int)item.EmployeeID, item.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, employee.FromDate, employee.ToDate, item.LeaveGrantRequestDetails, item.AppliedLeaveDates);

                    }
                    else
                    {

                        if (employee.FromDate.Year != currrentFinanceYear)
                        {

                            DateTime fromDate = DateTime.Now.Date;
                            DateTime toDate = employee.ToDate;
                            DateTime? dateOfJoining = employee.EmployeeDetails.Where(x => x.EmployeeID == item.EmployeeID).Select(x => x.DOJ).FirstOrDefault();
                            if (employee.FromDate.Year > currrentFinanceYear)
                            {
                                if (item.EffectiveFromDate != null && item.EffectiveFromDate > fromDate)
                                {
                                    fromDate = (DateTime)item.EffectiveFromDate;
                                }

                                if (dateOfJoining != null && dateOfJoining > fromDate.Date)
                                {
                                    fromDate = (DateTime)dateOfJoining;
                                }
                                if (item.EffectiveToDate != null && item.EffectiveToDate < toDate)
                                {
                                    toDate = (DateTime)item.EffectiveToDate;
                                }
                            }
                            else
                            {

                                // DateTime? dateOfJoining = employee.EmployeeDetails.Where(x => x.EmployeeID == item.EmployeeID).Select(x => x.DOJ).FirstOrDefault();
                                if (dateOfJoining != null && dateOfJoining > toDate.Date)
                                {
                                    toDate = (DateTime)dateOfJoining;
                                }
                                if (item.EffectiveFromDate != null && item.EffectiveFromDate > toDate.Date)
                                {
                                    toDate = (DateTime)item.EffectiveFromDate;
                                }
                            }

                            List<AppliedLeaveTypeDetails> appliedLeaveDetails = _leaveRepository.GetAppliedLeaveByLeaveTypeAndDate(item?.EmployeeID == null ? 0 : (int)item.EmployeeID, fromDate, toDate, item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID);
                            List<LeaveAdjustmentDetails> leaveAdjustmentDetails = _leaveAdjustmentDetailsRepository.GetLeaveAdjustmentDetailsByDate(item?.EmployeeID == null ? 0 : (int)item.EmployeeID, item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, fromDate, toDate);
                            LeaveCarryForwardListView leaveCarryForwardDetails = _leaveEntitlementRepository.GetLeaveTypeCarryForwardDetails(item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID);
                            //LeaveTypes leaveTypeDetails = _leaveTypeRepository?.GetByID(leaveTypeId);
                            AppConstants appConstants = _appConstantsRepository.GetAppconstantByID(item.LeaveAccruedType);
                            if (appliedLeaveDetails?.Count > 0 || leaveAdjustmentDetails?.Count > 0 || leaveCarryForwardDetails != null)
                            {
                                item.ActualBalanceLeave = await GetFinancialYearLeaveBalance(item?.ActualBalanceLeave == null ? 0 : (decimal)item.ActualBalanceLeave, fromDate, toDate, item?.EmployeeID == null ? 0 : (int)item.EmployeeID, item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, appliedLeaveDetails, leaveAdjustmentDetails, leaveCarryForwardDetails);
                            }
                            else if (appConstants?.DisplayName?.ToLower() != "onetime")
                            {
                                DateTime? leaveAccruedDate = fromDate;
                                int duration = 0;
                                decimal leaveAccrued = 0;
                                //string appConstants = dbContext.AppConstants.Where(x => x.AppConstantId == item.LeaveAccruedType).Select(x => x.AppConstantValue).FirstOrDefault();

                                if (appConstants?.DisplayName?.ToLower() == "monthly")
                                {
                                    leaveAccrued = item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays;
                                    if (item?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                    {
                                        fromDate = fromDate.AddMonths(1);
                                    }
                                    else if (item?.LeaveAccruedDay?.ToLower() != "lastdays".ToLower() && item?.LeaveAccruedDay?.ToLower() != "firstdays".ToLower() &&
                                        !string.IsNullOrEmpty(item?.LeaveAccruedDay) && DateTime.Now.Day > Convert.ToInt32(item?.LeaveAccruedDay))
                                    {
                                        fromDate = fromDate.AddMonths(1);
                                    }

                                }
                                else if (appConstants?.DisplayName?.ToLower() == "halfyearly")
                                {
                                    leaveAccrued = item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays / 6;
                                    FinancialYearDateView start = GetFinancialYearHalfYearly(fromDate);
                                    if (item?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                    {
                                        fromDate = start.ToDate.AddMonths(1);
                                    }
                                }
                                else if (appConstants?.DisplayName?.ToLower() == "quarterly")
                                {
                                    leaveAccrued = item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays / 3;
                                    FinancialYearDateView start = GetFinancialYearQuarter(fromDate);
                                    if (item?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                    {
                                        fromDate = start.ToDate.AddMonths(1);
                                    }
                                }
                                else if (appConstants?.DisplayName?.ToLower() == "yearly")
                                {
                                    leaveAccrued = item.LeaveAccruedNoOfDays == null ? 0 : (decimal)item.LeaveAccruedNoOfDays / 12;
                                    FinancialYearDateView start = GetFinancialYearly(fromDate);
                                    if (item?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                    {
                                        fromDate = start.ToDate.AddMonths(1);
                                    }
                                }

                                for (DateTime from = fromDate; from <= toDate; from = from.AddMonths(1))
                                {
                                    duration = duration + 1;
                                }
                                item.ActualBalanceLeave = item.ActualBalanceLeave + (leaveAccrued * duration);
                            }
                        }

                    }
                }
                leaveBalance.Leaves = (from leave in employeeLeaveDetails
                                       group leave by leave.EmployeeID into g
                                       select new LeaveBalanceList
                                       {
                                           EmployeeId = g.Key,
                                           BalanceLeaves = GetRoundOffValues(g.Sum(x => x.ActualBalanceLeave == null ? 0 : (decimal)x.ActualBalanceLeave))
                                       }).ToList();
            }

            return leaveBalance;
        }
        #endregion

        #region Get Finacial Year Date details
        public static int GetIso8601WeekOfYear(DateTime time)
        {
            return CultureInfo.InvariantCulture.Calendar.GetWeekOfYear(time, CalendarWeekRule.FirstFullWeek, DayOfWeek.Sunday);
        }
        public static DateTime FirstDateOfWeek(int year, int weekOfYear, System.Globalization.CultureInfo ci)
        {
            DateTime jan1 = new(year, 1, 1);
            int daysOffset = (int)DayOfWeek.Sunday - (int)jan1.DayOfWeek;
            DateTime firstWeekDay = jan1.AddDays(daysOffset);
            int firstWeek = ci.Calendar.GetWeekOfYear(jan1, ci.DateTimeFormat.CalendarWeekRule, DayOfWeek.Sunday);
            if ((firstWeek <= 1 || firstWeek >= 52) && daysOffset >= -3)
            {
                weekOfYear -= 1;
            }
            return firstWeekDay.AddDays(weekOfYear * 7);
        }
        public static FinancialYearDateView GetFinancialWeek(DateTime date)
        {
            FinancialYearDateView finacialyeardates = new();
            int thisWeekNumber = GetIso8601WeekOfYear(date);
            finacialyeardates.FromDate = FirstDateOfWeek(DateTime.Now.Year, thisWeekNumber, CultureInfo.CurrentCulture);
            finacialyeardates.ToDate = finacialyeardates.FromDate.AddDays(6);
            return finacialyeardates;
        }
        public static FinancialYearDateView GetFinancialYearMonthly(DateTime date)
        {
            FinancialYearDateView finacialyeardates = new();
            finacialyeardates.FromDate = new DateTime(date.Year, date.Month, 1); //firstdate 
            finacialyeardates.ToDate = new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month));//lastdate
            return finacialyeardates;
        }
        public static FinancialYearDateView GetFinancialYearQuarter(DateTime date)
        {
            FinancialYearDateView finacialyeardates = new();

            if (date.Month >= 4 && date.Month <= 6)
            {
                finacialyeardates.FromDate = new DateTime(date.Year, 4, 1);//firstdate 
                finacialyeardates.ToDate = new DateTime(date.Year, 6, 30);//lastdate
            }
            else if (date.Month >= 7 && date.Month <= 9)
            {
                finacialyeardates.FromDate = new DateTime(date.Year, 7, 1);//firstdate 
                finacialyeardates.ToDate = new DateTime(date.Year, 9, 30);//lastdate
            }
            else if (date.Month >= 10 && date.Month <= 12)
            {
                finacialyeardates.FromDate = new DateTime(date.Year, 10, 1);//firstdate 
                finacialyeardates.ToDate = new DateTime(date.Year, 12, 31);//lastdate
            }
            else
            {
                finacialyeardates.FromDate = new DateTime(date.Year, 1, 1);//firstdate 
                finacialyeardates.ToDate = new DateTime(date.Year, 3, 31);//lastdate
            }
            return finacialyeardates;
        }
        public static FinancialYearDateView GetFinancialYearHalfYearly(DateTime date)
        {
            FinancialYearDateView finacialyeardates = new();
            if (date.Month >= 4 && date.Month <= 9)
            {
                finacialyeardates.FromDate = new DateTime(date.Year, 4, 1);//firstdate 
                finacialyeardates.ToDate = new DateTime(date.Year, 9, 30);//lastdate
            }
            else
            {
                if (date.Month >= 10 && date.Month <= 12)
                {
                    finacialyeardates.FromDate = new DateTime(date.Year, 10, 1);//firstdate
                    finacialyeardates.ToDate = new DateTime(date.Year + 1, 3, 31);//lastdate
                }
                else
                {
                    finacialyeardates.FromDate = new DateTime(date.Year - 1, 10, 1);//firstdate
                    finacialyeardates.ToDate = new DateTime(date.Year, 3, 31);//lastdate
                }
            }
            return finacialyeardates;
        }
        public static FinancialYearDateView GetFinancialYearly(DateTime date)
        {
            FinancialYearDateView finacialyeardates = new();
            if (date.Month >= 1 && date.Month <= 3)
            {
                finacialyeardates.FromDate = new DateTime(date.Year - 1, 4, 1);//firstdate 
                finacialyeardates.ToDate = new DateTime(date.Year, 3, 31);//lastdate
            }
            else
            {
                finacialyeardates.FromDate = new DateTime(date.Year, 4, 1);//firstdate 
                finacialyeardates.ToDate = new DateTime(date.Year + 1, 3, 31);//lastdate
            }
            return finacialyeardates;
        }
        #endregion

        #region Get Leave Restriction Details by LeaveId
        public LeaveTypeRestrictionsView GetLeaveRestrictionsDetailsByLeaveTypeId(int leaveTypeId)
        {
            return _leaveRepository.GetLeaveRestrictionsDetailsById(leaveTypeId);
        }
        #endregion

        #region Get Employee Apply Leave Details
        public decimal GetLeaveByEmployeeId(int employeeId, int LeaveTypeId, DateTime fromDate, DateTime toDate, int leaveId, bool isEdit)
        {
            return _leaveRepository.GetLeaveByEmployeeId(employeeId, LeaveTypeId, fromDate, toDate, leaveId, isEdit);
        }
        #endregion

        #region Get Employee Apply Leave Details By EmployeeId and Date
        public List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndBackwardToDate(int employeeId, int LeaveTypeId, DateTime toDate)
        {
            return _leaveRepository.GetApplyLeaveByEmployeeIdAndBackwardToDate(employeeId, LeaveTypeId, toDate);
        }
        #endregion

        #region Get Active Leaves by Previous Date
        public ApplyLeavesView GetAppliedLeaveByLeaveIds(int employeeId, List<int?> activeLeaveTypeId, ApplyLeavesView leaveView, List<WeekendViewDefinition> WeekendList)
        {
            List<DateTime> leaveDate = leaveView?.AppliedLeaveDetails?.Select(x => x.Date).ToList();
            if (leaveDate?.Count > 0)
            {
                foreach (DateTime date in leaveDate)
                {
                    DateTime preDate = DateTime.Now;
                    bool isNotHoliday = true;
                    int i = -1;
                    while (isNotHoliday == true)
                    {
                        preDate = date.AddDays(i);
                        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, preDate, leaveView.LocationId, leaveView.ShiftId);
                        isNotHoliday = holidayDate == null ? false : true;
                        if (isNotHoliday == false)
                        {
                            WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(preDate.DayOfWeek.ToString())).FirstOrDefault();

                            isNotHoliday = isweekend == null ? false : true;
                        }
                        i = i - 1;
                    }
                    ApplyLeavesView result = _leaveRepository.GetAppliedLeaveByLeaveIds(employeeId, activeLeaveTypeId, preDate);
                    if (result != null)
                    {
                        return result;
                    }
                    if (result == null)
                    {
                        DateTime nxtDate = DateTime.Now;
                        bool isNextNotHoliday = true;
                        int j = 1;
                        while (isNextNotHoliday == true)
                        {
                            nxtDate = date.AddDays(j);
                            Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, nxtDate, leaveView.LocationId, leaveView.ShiftId);
                            isNextNotHoliday = holidayDate == null ? false : true;
                            if (isNextNotHoliday == false)
                            {
                                WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(nxtDate.DayOfWeek.ToString())).FirstOrDefault();

                                isNextNotHoliday = isweekend == null ? false : true;
                            }
                            j = j + 1;
                        }
                        result = _leaveRepository.GetAppliedLeaveByLeaveIds(employeeId, activeLeaveTypeId, nxtDate);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Get Current Financial year Holiday List By Holid Id
        public Holiday GetHolidayDetailByDate(List<int?> activeHolidayId, ApplyLeavesView leaveView, List<WeekendViewDefinition> WeekendList)
        {
            List<DateTime> leaveDate = leaveView?.AppliedLeaveDetails?.Select(x => x.Date).ToList();
            if (leaveDate?.Count > 0)
            {
                foreach (DateTime date in leaveDate)
                {
                    DateTime preDate = DateTime.Now;
                    bool isNotHoliday = true;
                    int i = -1;
                    while (isNotHoliday == true)
                    {
                        preDate = date.AddDays(i);
                        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, preDate, leaveView.LocationId, leaveView.ShiftId);
                        if (holidayDate == null || !activeHolidayId.Contains(holidayDate.HolidayID))
                        {
                            WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(preDate.DayOfWeek.ToString())).FirstOrDefault();

                            isNotHoliday = isweekend == null ? false : true;
                        }
                        else
                        {
                            isNotHoliday = false;
                        }

                        i = i - 1;
                    }
                    Holiday result = _holidayRepository.GetHolidayDetailByDate(activeHolidayId, leaveView.DepartmentId, preDate, leaveView.LocationId, leaveView.ShiftId);
                    if (result != null)
                    {
                        return result;
                    }
                    if (result == null)
                    {
                        DateTime nxtDate = DateTime.Now;
                        bool isNextNotHoliday = true;
                        int j = 1;
                        while (isNextNotHoliday == true)
                        {
                            nxtDate = date.AddDays(j);
                            Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, nxtDate, leaveView.LocationId, leaveView.ShiftId);
                            if (holidayDate == null || !activeHolidayId.Contains(holidayDate?.HolidayID))
                            {
                                WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(nxtDate.DayOfWeek.ToString())).FirstOrDefault();
                                //if (isweekend != null)
                                //{
                                //    isNextNotHoliday = true;
                                //}
                                isNextNotHoliday = isweekend == null ? false : true;
                            }
                            else
                            {
                                isNextNotHoliday = false;
                            }
                            j = j + 1;
                        }
                        result = _holidayRepository.GetHolidayDetailByDate(activeHolidayId, leaveView.DepartmentId, nxtDate, leaveView.LocationId, leaveView.ShiftId);
                        if (result != null)
                        {
                            return result;
                        }
                    }
                }
            }
            return null;
        }
        #endregion

        #region Get Employee Applied Leave by EmployeeId
        public List<ApplyLeavesView> GetAppliedLeaveByEmployeeId(int employeeId, DateTime fromDate, DateTime toDate)
        {
            return _leaveRepository.GetAppliedLeaveByEmployeeId(employeeId, fromDate, toDate);
        }
        #endregion

        #region Update Employee Leave Balance
        public async Task<bool> UpdateEmployeeLeaveBalance(EmployeeLeaveBalanceUpdateView leaveBalanceUpdateView)
        {
            try
            {
                bool isGrantLeave = _leaveTypeRepository.CheckIsGrantLeaveByLeaveTypeId(leaveBalanceUpdateView.LeaveTypeId);
                decimal? actualDays = (leaveBalanceUpdateView?.AdjustmentLeaveBalance - leaveBalanceUpdateView?.ActualLeaveBalance);
                if (isGrantLeave == true)
                {
                    if (actualDays != null)
                    {
                        decimal? effectiveToDate = _leaveRestrictionsRepository.GetGrantLeaveExpireDate(leaveBalanceUpdateView.LeaveTypeId);

                        LeaveGrantRequestDetails leaveGrantRequestDetails = new();
                        leaveGrantRequestDetails.LeaveTypeId = leaveBalanceUpdateView.LeaveTypeId;
                        leaveGrantRequestDetails.EmployeeID = leaveBalanceUpdateView.EmployeeId;
                        leaveGrantRequestDetails.NumberOfDay = actualDays;
                        leaveGrantRequestDetails.BalanceDay = actualDays;
                        leaveGrantRequestDetails.Status = "Approved";
                        leaveGrantRequestDetails.EffectiveFromDate = leaveBalanceUpdateView.AdjustmentEffectiveFromDate;
                        leaveGrantRequestDetails.EffectiveToDate = effectiveToDate == null ? null :
                                leaveBalanceUpdateView.AdjustmentEffectiveFromDate.Value.AddDays((double)effectiveToDate - 1);
                        leaveGrantRequestDetails.IsActive = true;
                        leaveGrantRequestDetails.IsLeaveAdjustment = true;
                        leaveGrantRequestDetails.CreatedOn = DateTime.UtcNow;
                        leaveGrantRequestDetails.CreatedBy = leaveBalanceUpdateView?.ModifiedBy;
                        await _leaveGrantRequestDetailsRepository.AddAsync(leaveGrantRequestDetails);
                        await _leaveGrantRequestDetailsRepository.SaveChangesAsync();
                    }
                    else
                    {
                        return false;
                    }

                }
                else
                {
                    EmployeeLeaveDetails employeeLeaveDetails = new();
                    int status = leaveBalanceUpdateView.LeaveTypeId;
                    employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(leaveBalanceUpdateView.EmployeeId, leaveBalanceUpdateView.LeaveTypeId);
                    //LeaveAdjustmentDetails adjustmentDetails = _leaveAdjustmentDetailsRepository.GetByDateAndEmployeeId(leaveBalanceUpdateView.EmployeeId,
                    //   leaveBalanceUpdateView?.AdjustmentEffectiveFromDate, leaveBalanceUpdateView.LeaveTypeId);
                    if (employeeLeaveDetails != null)
                    {
                        decimal? adjustmentLeaveBalance = employeeLeaveDetails?.AdjustmentBalanceLeave == null ? employeeLeaveDetails.BalanceLeave : employeeLeaveDetails.AdjustmentBalanceLeave;
                        AppConstants appConstants = new();
                        LeaveAdjustmentDetails leaveAdjustmentDetails = _leaveAdjustmentDetailsRepository.GetPreviousLeaveAdjustmentDetailsByEmployeeId(leaveBalanceUpdateView.EmployeeId, leaveBalanceUpdateView.LeaveTypeId, leaveBalanceUpdateView.AdjustmentEffectiveFromDate);
                        if (leaveBalanceUpdateView?.AdjustmentEffectiveFromDate.Value.Date < DateTime.Now.Date && leaveAdjustmentDetails == null)
                        {
                            LeaveTypes LeaveTypeDetails = _leaveTypeRepository.GetByID(leaveBalanceUpdateView.LeaveTypeId);
                            int LeaveAccruedType = LeaveTypeDetails?.LeaveAccruedType == null ? 0 : (int)LeaveTypeDetails?.LeaveAccruedType;
                            decimal leaveAccrued = 0;
                            appConstants = _appConstantsRepository.GetAppconstantByID(LeaveAccruedType);
                            //int NoofMonths = 0;
                            int totalMonths = ((DateTime.Now.Year * 12) + DateTime.Now.Month) - ((leaveBalanceUpdateView.AdjustmentEffectiveFromDate.Value.Year * 12) + leaveBalanceUpdateView.AdjustmentEffectiveFromDate.Value.Month) + 1;
                            if (appConstants != null)
                            {
                                // total Months calculation

                                DateTime From = (DateTime)leaveBalanceUpdateView.AdjustmentEffectiveFromDate;
                                DateTime To = DateTime.Now.Date;
                                if (appConstants.DisplayName.ToLower() == "Monthly".ToLower())
                                {
                                    leaveAccrued = LeaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)LeaveTypeDetails.LeaveAccruedNoOfDays;

                                    totalMonths = totalMonths - 2;

                                    try
                                    {
                                        if (LeaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                        {
                                            LeaveTypeDetails.LeaveAccruedDay = "1";
                                        }
                                        else if (LeaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                                        {
                                            LeaveTypeDetails.LeaveAccruedDay = Convert.ToString(DateTime.DaysInMonth(From.Year, From.Month));
                                        }
                                        if (!string.IsNullOrEmpty(LeaveTypeDetails?.LeaveAccruedDay) && DateTime.Now.Day >= Convert.ToInt32(LeaveTypeDetails?.LeaveAccruedDay))
                                        {
                                            totalMonths = totalMonths + 1;
                                        }
                                        if (!string.IsNullOrEmpty(LeaveTypeDetails?.LeaveAccruedDay) && From.Day < Convert.ToInt32(LeaveTypeDetails?.LeaveAccruedDay))
                                        {
                                            totalMonths = totalMonths + 1;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        LoggerManager.LoggingErrorTrack(ex, "UpdateEmployeeLeaveBalance", "Leaves Service", LeaveTypeDetails?.LeaveAccruedDay);
                                    }

                                }
                                else if (appConstants?.DisplayName?.ToLower() == "Quarterly".ToLower())
                                {
                                    FinancialYearDateView startqtr = GetFinancialYearQuarter(From);
                                    //FinancialYearDateView endqtr = GetFinancialYearQuarter(To);
                                    leaveAccrued = LeaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)LeaveTypeDetails.LeaveAccruedNoOfDays / 3;

                                    totalMonths = 0;
                                    DateTime fromDate = startqtr.FromDate.Date;
                                    if (LeaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                    {
                                        fromDate = startqtr.ToDate.AddDays(1);
                                    }
                                    else
                                    {
                                        if (startqtr.ToDate == leaveBalanceUpdateView?.AdjustmentEffectiveFromDate.Value.Date)
                                        {
                                            fromDate = startqtr.ToDate.AddDays(1);
                                        }
                                        else
                                        {
                                            if (DateTime.Now.Date > startqtr.ToDate && leaveBalanceUpdateView?.AdjustmentEffectiveFromDate.Value.Date < startqtr.ToDate)
                                            {
                                                fromDate = leaveBalanceUpdateView.AdjustmentEffectiveFromDate.Value.Date;
                                            }
                                            else
                                            {
                                                fromDate = startqtr.ToDate.AddDays(1);
                                            }
                                        }
                                    }
                                    for (DateTime date = fromDate.Date; date <= DateTime.Now.Date; date = date.AddMonths(3))
                                    {
                                        totalMonths = totalMonths + 3;
                                    }
                                }
                                else if (appConstants?.DisplayName?.ToLower() == "HalfYearly".ToLower())
                                {
                                    var starthalf = GetFinancialYearHalfYearly(From);
                                    var endhalf = GetFinancialYearHalfYearly(To);
                                    leaveAccrued = LeaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)LeaveTypeDetails.LeaveAccruedNoOfDays / 6;

                                    totalMonths = 0;
                                    DateTime fromDate = starthalf.FromDate.Date;
                                    if (LeaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                    {
                                        fromDate = starthalf.ToDate.AddDays(1);
                                    }
                                    else
                                    {
                                        if (starthalf.ToDate == leaveBalanceUpdateView?.AdjustmentEffectiveFromDate.Value.Date)
                                        {
                                            fromDate = starthalf.ToDate.AddDays(1);
                                        }
                                        else
                                        {
                                            if (DateTime.Now.Date > starthalf.ToDate && leaveBalanceUpdateView?.AdjustmentEffectiveFromDate.Value.Date < starthalf.ToDate)
                                            {
                                                fromDate = leaveBalanceUpdateView.AdjustmentEffectiveFromDate.Value.Date;
                                            }
                                            else
                                            {
                                                fromDate = starthalf.ToDate.AddDays(1);
                                            }
                                        }
                                    }

                                    for (DateTime date = fromDate.Date; date <= DateTime.Now.Date; date = date.AddMonths(6))
                                    {
                                        totalMonths = totalMonths + 6;
                                    }
                                }
                                else if (appConstants?.DisplayName?.ToLower() == "Yearly".ToLower())
                                {
                                    var startyear = GetFinancialYearly(From);
                                    //var endyear = GetFinancialYearly(To);
                                    leaveAccrued = LeaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)LeaveTypeDetails.LeaveAccruedNoOfDays / 12;

                                    totalMonths = 0;
                                    DateTime fromDate = startyear.FromDate.Date;
                                    if (LeaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                    {
                                        fromDate = startyear.ToDate.AddDays(1);
                                    }
                                    else
                                    {
                                        if (startyear.ToDate == leaveBalanceUpdateView?.AdjustmentEffectiveFromDate.Value.Date)
                                        {
                                            fromDate = startyear.ToDate.AddDays(1);
                                        }
                                        else
                                        {
                                            if (DateTime.Now.Date > startyear.ToDate && leaveBalanceUpdateView?.AdjustmentEffectiveFromDate.Value.Date < startyear.ToDate)
                                            {
                                                fromDate = leaveBalanceUpdateView.AdjustmentEffectiveFromDate.Value.Date;
                                            }
                                            else
                                            {
                                                fromDate = startyear.ToDate.AddDays(1);
                                            }
                                        }
                                    }

                                    for (DateTime date = fromDate.Date; date <= DateTime.Now.Date; date = date.AddMonths(12))
                                    {
                                        totalMonths = totalMonths + 12;
                                    }
                                }
                                adjustmentLeaveBalance = leaveBalanceUpdateView?.AdjustmentLeaveBalance + Math.Round((leaveAccrued * totalMonths), 2);

                            }

                        }
                        else if (leaveBalanceUpdateView?.AdjustmentEffectiveFromDate.Value.Date == DateTime.Now.Date)
                        {
                            adjustmentLeaveBalance = leaveBalanceUpdateView.AdjustmentLeaveBalance;
                        }
                        if (leaveBalanceUpdateView?.AdjustmentEffectiveFromDate.Value.Date <= DateTime.Now.Date)
                        {
                            decimal adjustmentBalance = leaveBalanceUpdateView.AdjustmentLeaveBalance == null ? 0 : (decimal)leaveBalanceUpdateView.AdjustmentLeaveBalance;
                            decimal actualLeave = leaveBalanceUpdateView.ActualLeaveBalance == null ? 0 : (decimal)leaveBalanceUpdateView.ActualLeaveBalance;
                            decimal daysDiff = adjustmentBalance - actualLeave;
                            decimal totalAdjustDays = employeeLeaveDetails.AdjustmentDays == null ? daysDiff : (decimal)employeeLeaveDetails.AdjustmentDays + daysDiff;
                            employeeLeaveDetails.AdjustmentBalanceLeave = adjustmentLeaveBalance;
                            employeeLeaveDetails.AdjustmentEffectiveFromDate = leaveBalanceUpdateView?.AdjustmentEffectiveFromDate;
                            employeeLeaveDetails.AdjustmentDays = totalAdjustDays;
                            employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                            employeeLeaveDetails.ModifiedBy = leaveBalanceUpdateView?.ModifiedBy;
                            _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                            await _employeeLeaveDetailsRepository.SaveChangesAsync();
                        }
                    }
                }
                LeaveAdjustmentDetails adjustmentDetails = new();
                adjustmentDetails.EmployeeId = leaveBalanceUpdateView?.EmployeeId;
                adjustmentDetails.LeavetypeId = leaveBalanceUpdateView?.LeaveTypeId;
                adjustmentDetails.EffectiveFromDate = leaveBalanceUpdateView?.AdjustmentEffectiveFromDate;
                adjustmentDetails.PreviousBalance = leaveBalanceUpdateView?.ActualLeaveBalance;
                adjustmentDetails.AdjustmentBalance = leaveBalanceUpdateView?.AdjustmentLeaveBalance;
                adjustmentDetails.NoOfDays = (leaveBalanceUpdateView?.AdjustmentLeaveBalance - leaveBalanceUpdateView?.ActualLeaveBalance);
                adjustmentDetails.CreatedBy = leaveBalanceUpdateView?.ModifiedBy;
                adjustmentDetails.CreatedOn = DateTime.UtcNow;
                await _leaveAdjustmentDetailsRepository.AddAsync(adjustmentDetails);
                await _leaveAdjustmentDetailsRepository.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateEmployeeLeaveBalance", JsonConvert.SerializeObject(leaveBalanceUpdateView));
                throw;
            }
        }
        #endregion

        #region Get RoundOff
        public static decimal GetRoundOff(decimal count)
        {
            decimal sd = default;

            if (count != null && count != 0)
            {
                //var decimalValues = (decimal)1.5112548;
                decimal decimalValue = decimal.Round(count, 2, MidpointRounding.AwayFromZero);
                var split = decimalValue.ToString(CultureInfo.InvariantCulture).Split('.');

                var de = Convert.ToDecimal("0." + (split.Length > 1 ? split[1] : 0));
                if (de is >= (decimal)0.0 and <= (decimal)0.24)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.0);

                }
                else if (de is >= (decimal)0.25 and <= (decimal)0.49)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.25);

                }
                else if (de is >= (decimal)0.50 and <= (decimal)0.74)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.50);

                }
                else if (de is >= (decimal)0.75 and <= (decimal)0.99)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.75);

                }
            }
            else
            {
                sd = 0;
            }

            return sd;
        }
        #endregion

        #region Get Applied Leave Details By EmployeeId And LeaveId
        public List<ApplyLeavesView> GetAppliedLeaveDetailsByEmployeeIdAndLeaveId(int employeeId, int leaveId)
        {
            return _leaveRepository.GetAppliedLeaveDetailsByEmployeeIdAndLeaveId(employeeId, leaveId);
        }
        #endregion

        #region Holiday Name Duplication
        public string HolidayNameDuplication(HolidayDetailView pholiday)
        {
            return _holidayRepository.HolidayNameDuplication(pholiday.HolidayName, pholiday.HolidayID, (DateTime)pholiday.HolidayDate, pholiday.IsRestrictHoliday == null ? false : (bool)pholiday.IsRestrictHoliday);
        }
        #endregion

        #region Get Holiday by Dept and Location and Shift and HolidayDate  
        public List<Holiday> GetHolidaybyDeptandLocandShifandDate(int DepartmentId, DateTime FromDate, DateTime ToDate, int LocationId, int ShiftDetailsId)
        {

            return _holidayRepository.GetHolidayByDate(DepartmentId, FromDate, ToDate, LocationId, ShiftDetailsId);

        }
        #endregion

        #region Get Employees Leave List for Timesheet   
        public List<EmployeeLeavesForTimeSheetView> GetEmployeeLeavesForTimesheet(int employeeID, DateTime fromDate, DateTime toDate)
        {

            return _AppliedLeaveDetailsRepository.GetEmployeeLeavesForTimesheet(employeeID, fromDate, toDate);

        }
        #endregion

        #region Get Exists Leave By EmployeeId
        public List<AppliedLeaveDetailsView> GetEmployeeExistsLeaves(int employeeId, DateTime fromDate)
        {
            return _leaveRepository.GetEmployeeExistsLeaves(employeeId, fromDate);
        }
        #endregion

        #region Credit Carry Forward Leaves
        //public async Task<bool> CreditCarryForwardLeaves(List<EmployeeDetailsForLeaveView> EmployeeList)
        public async Task<bool> CreditCarryForwardLeaves(tempparameter temp)
        {

            List<EmployeeDetailsForLeaveView> EmployeeList = temp.EmployeeDetail;
            DateTime todayDate = temp.executedate == null ? DateTime.Now.Date : (DateTime)temp.executedate;

            List<LeaveCarryForwardListView> leaveCarryForwardLists;
            LeaveCarryForwardListView carryforwarddetails = new();
            EmployeeLeaveDetails empleavebalancedetail;
            try
            {
                leaveCarryForwardLists = _leaveEntitlementRepository.GetCarryForwardList();
                DateTime executedate = default;
                var Quarter = GetFinancialYearQuarter(todayDate);
                var HalfYearly = GetFinancialYearHalfYearly(todayDate);

                foreach (LeaveCarryForwardListView Carryforwardlist in leaveCarryForwardLists)
                {
                    try
                    {
                        if (Carryforwardlist?.Period?.ToLower() == "monthly".ToLower())
                        {
                            if (Carryforwardlist?.ResetDay?.ToLower() == "firstdays".ToLower())
                            {
                                executedate = new DateTime(todayDate.Year, todayDate.Month, 1);
                            }
                            else if (Carryforwardlist?.ResetDay?.ToLower() == "lastdays".ToLower())
                            {
                                executedate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));
                            }
                            else
                            {
                                executedate = new DateTime(todayDate.Year, todayDate.Month, Convert.ToInt32(Carryforwardlist.ResetDay));
                            }
                        }
                        else if (Carryforwardlist?.Period?.ToLower() == "quarterly".ToLower())
                        {

                            if (Carryforwardlist?.ResetDay?.ToLower() == "FirstDays".ToLower())
                            {
                                executedate = Quarter.FromDate;
                            }
                            else if (Carryforwardlist?.ResetDay?.ToLower() == "LastDays".ToLower())
                            {
                                executedate = Quarter.ToDate;
                            }
                        }
                        else if (Carryforwardlist?.Period?.ToLower() == "halfYearly".ToLower())
                        {

                            if (Carryforwardlist?.ResetDay?.ToLower() == "FirstDays".ToLower())
                            {
                                executedate = HalfYearly.FromDate;
                            }
                            else if (Carryforwardlist?.ResetDay?.ToLower() == "LastDays".ToLower())
                            {
                                executedate = HalfYearly.ToDate;
                            }
                        }
                        else if (Carryforwardlist?.Period?.ToLower() == "yearly".ToLower())
                        {

                            int month = (int)Carryforwardlist?.ResetMonth;
                            var dat = Carryforwardlist?.ResetDay;
                            int year = todayDate.Year; //DateTime.UtcNow.Year;
                            if (Carryforwardlist?.ResetDay?.ToLower() == "firstdays".ToLower())
                            {
                                executedate = new DateTime(year, month, 1);
                            }
                            else if (Carryforwardlist?.ResetDay?.ToLower() == "lastdays".ToLower())
                            {
                                executedate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                            }
                            else
                            {
                                executedate = new DateTime(year, month, Convert.ToInt32(Carryforwardlist.ResetDay));
                            }
                        }

                        //if (DateTime.UtcNow.Date == executedate)
                        if (todayDate == executedate)
                        {
                            carryforwarddetails = Carryforwardlist;
                        }
                        else
                        {
                            carryforwarddetails = null;
                        }

                        if (carryforwarddetails != null)
                        {
                            foreach (EmployeeDetailsForLeaveView emplist in EmployeeList)
                            {
                                try
                                {
                                    empleavebalancedetail = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(emplist.EmployeeID, Carryforwardlist.LeaveTypeID);

                                    if (empleavebalancedetail != null)
                                    {
                                        decimal removenullcarry = (decimal)(Carryforwardlist?.MaximumCarryForwardDays == null ? 0 : Carryforwardlist.MaximumCarryForwardDays);
                                        decimal removenullreimburs = (decimal)(Carryforwardlist?.MaximumReimbursementDays == null ? 0 : Carryforwardlist.MaximumReimbursementDays);
                                        decimal CarryforwardleaveRoundOff = GetRoundOff(removenullcarry);
                                        decimal reimbursementRoundOff = GetRoundOff(removenullreimburs);
                                        decimal? carryleaves = default;
                                        decimal? Reimbursement = default;
                                        decimal? adjustmentLeave = default;
                                        DateTime? adjustmenteffectivedate = null;
                                        decimal? afterCarryForwardBalance = default;
                                        // CarryForward
                                        if (Carryforwardlist.CarryForwardName?.ToLower() == "NoOfDays".ToLower())
                                        {
                                            if (empleavebalancedetail.AdjustmentBalanceLeave != null && empleavebalancedetail.AdjustmentEffectiveFromDate <= DateTime.Now.Date) //empleavebalancedetail.AdjustmentEffectiveFromDate==DateTime.UtcNow.Date ||
                                            {
                                                adjustmentLeave = null;
                                                adjustmenteffectivedate = null;
                                                afterCarryForwardBalance = empleavebalancedetail?.AdjustmentBalanceLeave > CarryforwardleaveRoundOff ? empleavebalancedetail.AdjustmentBalanceLeave - CarryforwardleaveRoundOff : 0;
                                                carryleaves = empleavebalancedetail?.AdjustmentBalanceLeave >= CarryforwardleaveRoundOff ? CarryforwardleaveRoundOff : empleavebalancedetail.AdjustmentBalanceLeave;
                                            }
                                            else if (empleavebalancedetail.AdjustmentBalanceLeave != null && empleavebalancedetail.AdjustmentEffectiveFromDate > DateTime.Now.Date)
                                            {
                                                adjustmentLeave = empleavebalancedetail?.AdjustmentBalanceLeave;
                                                adjustmenteffectivedate = empleavebalancedetail?.AdjustmentEffectiveFromDate;

                                                afterCarryForwardBalance = empleavebalancedetail?.BalanceLeave > CarryforwardleaveRoundOff ? empleavebalancedetail.BalanceLeave - CarryforwardleaveRoundOff : 0;
                                                carryleaves = empleavebalancedetail?.BalanceLeave >= CarryforwardleaveRoundOff ? CarryforwardleaveRoundOff : empleavebalancedetail.BalanceLeave;


                                            }
                                            else
                                            {
                                                adjustmentLeave = null;
                                                adjustmenteffectivedate = null;
                                                afterCarryForwardBalance = empleavebalancedetail?.BalanceLeave > CarryforwardleaveRoundOff ? empleavebalancedetail.BalanceLeave - CarryforwardleaveRoundOff : 0;
                                                carryleaves = empleavebalancedetail?.BalanceLeave >= CarryforwardleaveRoundOff ? CarryforwardleaveRoundOff : empleavebalancedetail.BalanceLeave;

                                            }

                                        }
                                        else if (Carryforwardlist?.CarryForwardName?.ToLower() == "All".ToLower())
                                        {
                                            if (empleavebalancedetail.AdjustmentBalanceLeave != null && empleavebalancedetail.AdjustmentEffectiveFromDate <= DateTime.Now.Date) //empleavebalancedetail.AdjustmentEffectiveFromDate==DateTime.UtcNow.Date ||
                                            {
                                                adjustmentLeave = null;
                                                adjustmenteffectivedate = null;
                                                carryleaves = empleavebalancedetail?.AdjustmentBalanceLeave;
                                            }
                                            else if (empleavebalancedetail.AdjustmentBalanceLeave != null && empleavebalancedetail.AdjustmentEffectiveFromDate > DateTime.Now.Date)
                                            {
                                                adjustmentLeave = empleavebalancedetail?.AdjustmentBalanceLeave;
                                                adjustmenteffectivedate = empleavebalancedetail?.AdjustmentEffectiveFromDate;
                                                carryleaves = empleavebalancedetail?.BalanceLeave;


                                            }
                                            else
                                            {
                                                adjustmentLeave = null;
                                                adjustmenteffectivedate = null;
                                                carryleaves = empleavebalancedetail?.BalanceLeave;
                                            }


                                        }
                                        else if (Carryforwardlist?.CarryForwardName?.ToLower() == "None".ToLower() || Carryforwardlist?.CarryForwardName == null)
                                        {
                                            carryleaves = 0;
                                            adjustmentLeave = null;
                                            adjustmenteffectivedate = null;
                                            if (empleavebalancedetail.AdjustmentBalanceLeave != null && empleavebalancedetail?.AdjustmentEffectiveFromDate <= DateTime.Now.Date) //empleavebalancedetail.AdjustmentEffectiveFromDate==DateTime.UtcNow.Date ||
                                            {

                                                afterCarryForwardBalance = empleavebalancedetail.AdjustmentBalanceLeave;
                                            }
                                            else if (empleavebalancedetail.AdjustmentBalanceLeave != null && empleavebalancedetail?.AdjustmentEffectiveFromDate > DateTime.Now.Date)
                                            {

                                                afterCarryForwardBalance = empleavebalancedetail?.BalanceLeave;


                                            }
                                            else
                                            {
                                                afterCarryForwardBalance = empleavebalancedetail?.BalanceLeave;
                                            }

                                        }
                                        // Rembursement
                                        if (Carryforwardlist?.ReimbursementName?.ToLower() == "NoOfDays".ToLower())
                                        {
                                            Reimbursement = afterCarryForwardBalance > reimbursementRoundOff ? reimbursementRoundOff : afterCarryForwardBalance;

                                        }
                                        else if (Carryforwardlist?.ReimbursementName?.ToLower() == "All".ToLower())
                                        {

                                            Reimbursement = afterCarryForwardBalance;
                                        }
                                        else if (Carryforwardlist?.ReimbursementName?.ToLower() == "None".ToLower() || Carryforwardlist.ReimbursementName == null)
                                        {
                                            Reimbursement = 0;
                                        }

                                        LeaveCarryForward leavecarryforwarddata = new();
                                        leavecarryforwarddata.EmployeeID = (int)empleavebalancedetail?.EmployeeID;
                                        leavecarryforwarddata.LeaveTypeID = Carryforwardlist.LeaveTypeID;
                                        leavecarryforwarddata.BalanceLeave = (decimal)empleavebalancedetail?.BalanceLeave;
                                        leavecarryforwarddata.AdjustmentBalanceLeave = empleavebalancedetail?.AdjustmentBalanceLeave;
                                        leavecarryforwarddata.AdjustmentEffectiveFromDate = empleavebalancedetail?.AdjustmentEffectiveFromDate;
                                        leavecarryforwarddata.CarryForwardLeaves = carryleaves;
                                        leavecarryforwarddata.ReimbursementLeaves = Reimbursement;
                                        leavecarryforwarddata.AdjustmentDays = empleavebalancedetail?.AdjustmentDays;
                                        leavecarryforwarddata.ResetDate = executedate.Date;
                                        leavecarryforwarddata.CreatedOn = DateTime.UtcNow;
                                        leavecarryforwarddata.CreatedBy = 1;

                                        await _leaveCarryForwardRepository.AddAsync(leavecarryforwarddata);
                                        await _leaveCarryForwardRepository.SaveChangesAsync();
                                        if (carryleaves != null)
                                        {
                                            empleavebalancedetail.BalanceLeave = carryleaves;
                                            empleavebalancedetail.AdjustmentBalanceLeave = adjustmentLeave;
                                            empleavebalancedetail.AdjustmentEffectiveFromDate = adjustmenteffectivedate;
                                            empleavebalancedetail.AdjustmentDays = adjustmenteffectivedate == null ? null : empleavebalancedetail.AdjustmentDays;
                                            empleavebalancedetail.ModifiedOn = DateTime.UtcNow;
                                            empleavebalancedetail.ModifiedBy = 1;
                                            _employeeLeaveDetailsRepository.Update(empleavebalancedetail);
                                            await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                        }
                                    }

                                }
                                catch (Exception ex)
                                {
                                    LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/CreditCarryForwardLeaves EmployeeId - ", Convert.ToString(emplist.EmployeeID));
                                }

                            }

                        }
                    }
                    catch (Exception ex)
                    {
                        LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/CreditCarryForwardLeaves LeaveTypeId - ", Convert.ToString(Carryforwardlist.LeaveTypeID));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/CreditCarryForewardLeaves");
            }
            return true;
        }
        #endregion

        #region Get Employee Edit Apply Leave Details By EmployeeId and FromDate
        public List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndFromDate(int employeeId, int leaveTypeId, int LeaveId, DateTime fromDate)
        {
            return _leaveRepository.GetApplyLeaveByEmployeeIdAndFromDate(employeeId, leaveTypeId, LeaveId, fromDate);
        }
        #endregion

        #region Get Employee Edit Apply Leave Details By EmployeeId and ToDate
        public List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndToDate(int employeeId, int leaveTypeId, int LeaveId, DateTime toDate)
        {
            return _leaveRepository.GetApplyLeaveByEmployeeIdAndToDate(employeeId, leaveTypeId, LeaveId, toDate);
        }
        #endregion

        #region Update Leave Type Status 
        public async Task<bool> UpdateLeaveTypeStatus(int leaveTypeId, bool isEnabled, int updatedBy)
        {
            try
            {
                LeaveTypes leaveDetails = _leaveRepository.GetByleaveId(leaveTypeId);
                if (leaveDetails != null)
                {
                    leaveDetails.IsActive = isEnabled;
                    leaveDetails.ModifiedOn = DateTime.UtcNow;
                    leaveDetails.ModifiedBy = updatedBy;
                    _leaveRepository.Update(leaveDetails);
                    await _leaveRepository.SaveChangesAsync();
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

        #region Update Holiday Status 
        public async Task<bool> UpdateHolidayStatus(int holidayId, bool isEnabled, int updatedBy)
        {
            try
            {
                Holiday holidayDetails = _holidayRepository.GetByID(holidayId);
                if (holidayDetails != null)
                {
                    holidayDetails.IsActive = isEnabled;
                    holidayDetails.ModifiedOn = DateTime.UtcNow;
                    holidayDetails.ModifiedBy = updatedBy;
                    _holidayRepository.Update(holidayDetails);
                    await _holidayRepository.SaveChangesAsync();
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

        #region Add or Update Grant Leave
        public async Task<int> AddorUpdateLeaveGrant(LeaveGrantRequestAndDocumentView leaveGrantRequestView, int grantRequestFutureDay)
        {
            try
            {
                int LeaveGrantDetailId = 0;

                LeaveGrantRequestDetails leaveGrantRequestDetails = new();
                if (leaveGrantRequestView?.leaveGrantRequestDetail?.LeaveGrantDetailId != 0) leaveGrantRequestDetails = _leaveGrantRequestDetailsRepository.GetByID(leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId);
                leaveGrantRequestDetails.LeaveTypeId = leaveGrantRequestView.leaveGrantRequestDetail.LeaveTypeId;
                leaveGrantRequestDetails.EmployeeID = leaveGrantRequestView.leaveGrantRequestDetail.EmployeeID;
                leaveGrantRequestDetails.NumberOfDay = leaveGrantRequestView?.leaveGrantRequestDetail?.NumberOfDay;
                leaveGrantRequestDetails.BalanceDay = leaveGrantRequestView?.leaveGrantRequestDetail?.NumberOfDay;
                leaveGrantRequestDetails.Reason = leaveGrantRequestView?.leaveGrantRequestDetail?.Reason;
                leaveGrantRequestDetails.EffectiveFromDate = leaveGrantRequestView?.leaveGrantRequestDetail?.EffectiveFromDate;
                if (grantRequestFutureDay == 0)
                {
                    leaveGrantRequestDetails.EffectiveToDate = null;
                }
                else
                {
                    leaveGrantRequestDetails.EffectiveToDate = leaveGrantRequestView?.leaveGrantRequestDetail?.EffectiveFromDate.Value.AddDays(grantRequestFutureDay - 1);
                }

                leaveGrantRequestDetails.IsActive = leaveGrantRequestView?.leaveGrantRequestDetail?.IsActive;
                if (leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId == 0)
                {
                    leaveGrantRequestDetails.Status = "Pending";
                    leaveGrantRequestDetails.CreatedOn = DateTime.UtcNow;
                    leaveGrantRequestDetails.CreatedBy = leaveGrantRequestView.leaveGrantRequestDetail?.CreatedBy;
                    await _leaveGrantRequestDetailsRepository.AddAsync(leaveGrantRequestDetails);
                    await _leaveGrantRequestDetailsRepository.SaveChangesAsync();
                    LeaveGrantDetailId = leaveGrantRequestDetails.LeaveGrantDetailId;

                }
                else
                {
                    leaveGrantRequestDetails.ModifiedOn = DateTime.UtcNow;
                    leaveGrantRequestDetails.ModifiedBy = leaveGrantRequestView.leaveGrantRequestDetail?.CreatedBy;
                    _leaveGrantRequestDetailsRepository.Update(leaveGrantRequestDetails);
                    LeaveGrantDetailId = leaveGrantRequestDetails.LeaveGrantDetailId;
                    await _leaveGrantRequestDetailsRepository.SaveChangesAsync();
                }
                if (leaveGrantRequestView.leaveGrantDocument.Count > 0)
                {
                    foreach (LeaveGrantDocument leaveGrantDocument in leaveGrantRequestView?.leaveGrantDocument)
                    {
                        LeaveGrantDocumentDetails leaveGrantDocumentDetails = new()
                        {
                            LeaveGrantDetailId = LeaveGrantDetailId,
                            DocumentName = leaveGrantDocument?.DocumentName,
                            DocumentPath = leaveGrantDocument?.DocumentPath,
                            DocumentType = leaveGrantDocument?.DocumentType,
                            IsActive = leaveGrantDocument?.IsActive,
                            CreatedOn = DateTime.UtcNow,
                            CreatedBy = leaveGrantRequestView?.leaveGrantRequestDetail?.CreatedBy
                        };
                        await _leaveGrantDocumentDetailsRepository.AddAsync(leaveGrantDocumentDetails);
                        await _leaveGrantDocumentDetailsRepository.SaveChangesAsync();
                    }
                }
                if (leaveGrantRequestView?.leaveGrantRequestDetail?.LeaveGrantDetailId > 0)
                {
                    List<EmployeeGrantLeaveApproval> approvalEmployeeId = _employeeGrantLeaveApprovalRepository.GetEmployeeGrantLeaveApprovalByLeaveGrantDetailId(leaveGrantRequestView.leaveGrantRequestDetail.LeaveGrantDetailId);
                    LeaveGrantDetailId = approvalEmployeeId.Where(x => x.LevelId == 1).Select(x => x.ApproverEmployeeId == null ? 0 : (int)x.ApproverEmployeeId).FirstOrDefault();

                }
                if (leaveGrantRequestView?.leaveGrantRequestDetail?.LeaveTypeId > 0 && leaveGrantRequestView?.leaveGrantRequestDetail?.LeaveGrantDetailId == 0)
                {
                    List<GrantLeaveApproval> approvalList = _grantLeaveApprovalRepository.GetGrantLeaveApprovalByLeaveTypeId(leaveGrantRequestView.leaveGrantRequestDetail.LeaveTypeId);
                    if (approvalList?.Count > 0)
                    {
                        foreach (GrantLeaveApproval approval in approvalList)
                        {
                            int? approverId = 0;
                            if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("GrantLeaveApproval", "ReportingTo"))
                            {
                                approverId = leaveGrantRequestView?.GrantLeaveApprover?.ReportingManagerEmployeeId;
                            }
                            else if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("GrantLeaveApproval", "DepartmentBUHead"))
                            {
                                approverId = leaveGrantRequestView?.GrantLeaveApprover?.DepartmentHeadEmployeeId;
                            }
                            else if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("GrantLeaveApproval", "HRBUHead"))
                            {
                                approverId = leaveGrantRequestView?.GrantLeaveApprover?.HRHeadEmployeeId;
                            }
                            else if (approval.LevelApprovalId == _appConstantsRepository.GetAppconstantIdByValue("GrantLeaveApproval", "Others"))
                            {
                                approverId = approval.LevelApprovalEmployeeId;
                            }
                            else
                            {
                                approverId = 0;
                            }
                            if (approverId > 0)
                            {
                                EmployeeGrantLeaveApproval employeeAppraoval = new EmployeeGrantLeaveApproval();
                                employeeAppraoval.LeaveGrantDetailId = LeaveGrantDetailId;
                                employeeAppraoval.ApproverEmployeeId = approverId;
                                employeeAppraoval.LevelId = approval.LevelId;
                                employeeAppraoval.Comments = "";
                                employeeAppraoval.Status = approval.LevelId == 1 ? "Pending" : null;
                                employeeAppraoval.CreatedOn = DateTime.UtcNow;
                                employeeAppraoval.CreatedBy = leaveGrantRequestView?.leaveGrantRequestDetail?.CreatedBy;
                                await _employeeGrantLeaveApprovalRepository.AddAsync(employeeAppraoval);
                                await _employeeGrantLeaveApprovalRepository.SaveChangesAsync();
                            }

                        }
                    }
                    List<EmployeeGrantLeaveApproval> approvalEmployee = _employeeGrantLeaveApprovalRepository.GetEmployeeGrantLeaveApprovalByLeaveGrantDetailId(LeaveGrantDetailId);
                    LeaveGrantDetailId = approvalEmployee.Where(x => x.LevelId == 1).Select(x => x.ApproverEmployeeId == null ? 0 : (int)x.ApproverEmployeeId).FirstOrDefault();
                }

                return LeaveGrantDetailId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Grant Leave By Id
        public List<LeaveGrantRequestAndDocumentView> GetGrantLeaveByEmployeeId(int employeeId)
        {
            return _leaveGrantRequestDetailsRepository.GetGrantLeaveByEmployeeId(employeeId);
        }
        #endregion

        //#region Get Grant Leave request details by LeaveTypeId and EmployeeId
        //public List<LeaveGrantRequestDetails> GetByLeaveTypeAndEmployeeID(int leaveTypeId, int employeeId)
        //{
        //    return _leaveGrantRequestDetailsRepository.GetByLeaveTypeAndEmployeeID(leaveTypeId, employeeId);
        //}
        //#endregion
        #region Get Grant Leave request details List by LeaveTypeId and EmployeeId
        public List<LeaveGrantRequestDetails> GetGrantLeaveListByTypeAndEmployeeID(int leaveTypeId, int employeeId, DateTime fromDate, DateTime toDate, int leaveGrantDetailId, bool isEdit)
        {
            return _leaveGrantRequestDetailsRepository.GetGrantLeaveListByTypeAndEmployeeID(leaveTypeId, employeeId, fromDate, toDate, leaveGrantDetailId, isEdit);
        }
        #endregion
        #region Get Before Gap Grant Leave request details by LeaveTypeId and EmployeeId
        public List<LeaveGrantRequestDetails> GetBackwardLeaveGrantGapByTypeIdAndEmployeeID(int leaveTypeId, int employeeId, int leaveGrantDetailId, DateTime fromDate, bool isEdit)
        {
            return _leaveGrantRequestDetailsRepository.GetBackwardLeaveGrantGapByTypeIdAndEmployeeID(leaveTypeId, employeeId, leaveGrantDetailId, fromDate, isEdit);
        }
        #endregion
        #region Get after Gap Grant Leave request details by LeaveTypeId and EmployeeId
        public List<LeaveGrantRequestDetails> GetForwardLeaveGrantGapByTypeIdAndEmployeeID(int leaveTypeId, int employeeId, int leaveGrantDetailId, DateTime fromDate, bool isEdit)
        {
            return _leaveGrantRequestDetailsRepository.GetForwardLeaveGrantGapByTypeIdAndEmployeeID(leaveTypeId, employeeId, leaveGrantDetailId, fromDate, isEdit);
        }
        #endregion
        #region Get Employee Apply Leave Details By EmployeeId and From Date
        public List<ApplyLeaves> GetApplyLeaveByEmployeeIdAndForwardToDate(int employeeId, int LeaveTypeId, DateTime fromDate)
        {
            return _leaveRepository.GetApplyLeaveByEmployeeIdAndForwardToDate(employeeId, LeaveTypeId, fromDate);
        }
        #endregion
        #region Get Grant Leave Details by Employee and LeaveGrant Id
        public List<LeaveGrantRequestAndDocumentView> GetGrantLeaveByEmployeeIdAndLeaveGrantId(int employeeId, int leaveGrantDetailId)
        {
            return _leaveGrantRequestDetailsRepository.GetGrantLeaveByEmployeeIdAndLeaveGrantId(employeeId, leaveGrantDetailId);
        }
        #endregion
        #region Delete Applied Grant Leave By Leave Grant Id
        public async Task<bool> DeleteAppliedGrantLeaveByLeaveGrantId(int leaveGrantDetailId)
        {
            try
            {
                if (leaveGrantDetailId > 0)
                {
                    List<LeaveGrantDocumentDetails> leaveGrantDocuments = _leaveGrantDocumentDetailsRepository.GetByID(leaveGrantDetailId);
                    foreach (LeaveGrantDocumentDetails appliedLeaveGrantDocuments in leaveGrantDocuments)
                    {
                        _leaveGrantDocumentDetailsRepository.Delete(appliedLeaveGrantDocuments);
                        await _leaveGrantDocumentDetailsRepository.SaveChangesAsync();
                        //Delete file from physical directory
                        string filePath = appliedLeaveGrantDocuments.DocumentPath;
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                    }
                    LeaveGrantRequestDetails ApplyLeaveGrantRequest = _leaveGrantRequestDetailsRepository.GetByID(leaveGrantDetailId);
                    if (ApplyLeaveGrantRequest != null && ApplyLeaveGrantRequest?.LeaveGrantDetailId > 0)
                    {
                        _leaveGrantRequestDetailsRepository.Delete(ApplyLeaveGrantRequest);
                        await _leaveGrantRequestDetailsRepository.SaveChangesAsync();
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion
        #region Delete Grant Leave Document By Leave Grant Id
        public async Task<bool> DeleteGrantLeaveDocumentByLeaveGrantDocId(int leaveGrantDocumentDetailId)
        {
            try
            {
                if (leaveGrantDocumentDetailId > 0)
                {

                    LeaveGrantDocumentDetails ApplyLeaveGrantDocument = _leaveGrantDocumentDetailsRepository.GetLeaveGrantDocument(leaveGrantDocumentDetailId);
                    if (ApplyLeaveGrantDocument != null && ApplyLeaveGrantDocument?.LeaveGrantDocumentDetailId > 0)
                    {
                        _leaveGrantDocumentDetailsRepository.Delete(ApplyLeaveGrantDocument);
                        await _leaveGrantDocumentDetailsRepository.SaveChangesAsync();

                        //Delete file from physical directory
                        string filePath = ApplyLeaveGrantDocument.DocumentPath;
                        if (File.Exists(filePath))
                        {
                            File.Delete(filePath);
                        }
                        return true;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return false;
        }
        #endregion
        //#region Update Employee Leave Details
        //private async Task<int> UpdateEmployeeLeaveDetails(int employeeId, int leaveTypeId, decimal totalLeave)
        //{
        //    try
        //    {
        //        int employeeLeaveDetailsID = 0;
        //        EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(employeeId, leaveTypeId);
        //        if (employeeLeaveDetails != null)
        //        {
        //            decimal totalAvailableLeave = totalLeave;
        //            if (employeeLeaveDetails.AdjustmentEffectiveFromDate != null)
        //            {
        //                if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= DateTime.UtcNow.Date)
        //                {
        //                    totalAvailableLeave += (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave;
        //                    employeeLeaveDetails.AdjustmentBalanceLeave = totalAvailableLeave;
        //                }
        //                else
        //                {
        //                    totalAvailableLeave += (decimal)employeeLeaveDetails?.BalanceLeave;
        //                    employeeLeaveDetails.BalanceLeave = totalAvailableLeave;
        //                }
        //            }
        //            else
        //            {
        //                totalAvailableLeave += (decimal)employeeLeaveDetails?.BalanceLeave;
        //                employeeLeaveDetails.BalanceLeave = totalAvailableLeave;
        //            }
        //            employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
        //            employeeLeaveDetails.ModifiedBy = employeeId;
        //            _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
        //            employeeLeaveDetailsID = employeeLeaveDetails.EmployeeLeaveDetailsID;
        //            await _LeaveApplyRepository.SaveChangesAsync();
        //        }
        //        return employeeLeaveDetailsID;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        //#endregion
        #region Check Leave Grant Dublicate details
        public bool ApplyLeaveGrantDatesDupilication(LeaveGrantRequestAndDocumentView leaveGrantRequestView)
        {

            return _leaveGrantRequestDetailsRepository.ApplyLeaveGrantDatesDupilication(leaveGrantRequestView);
        }
        #endregion
        //#region Check Employee Applied Leave Consecutive days
        //public List<AppliedLeaveDetails> GetConsecutiveEmployeAppliedLeaveDetails(int employeeId, int leaveTypeId, DateTime fromDate, DateTime toDate, int leaveId, bool isEdit)
        //{
        //    return _AppliedLeaveDetailsRepository.GetConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveTypeId, fromDate, toDate, leaveId, isEdit);
        //}
        //#endregion
        #region Check weekend or holiday
        public bool CheckWeekendOrHoliday(int employeeId, ApplyLeavesView leaveView, List<WeekendViewDefinition> WeekendList, LeaveTypeRestrictionsView leaverestriction)
        {

            if (leaveView?.AppliedLeaveDetails?.Count > 0)
            {
                int totaldays = leaveView == null ? 0 : leaveView.AppliedLeaveDetails == null ? 0 : leaveView.AppliedLeaveDetails.Count;

                if (leaverestriction?.Weekendsbetweenleaveperiod == false && leaverestriction?.Holidaybetweenleaveperiod == false)
                {
                    foreach (AppliedLeaveDetailsView item in leaveView?.AppliedLeaveDetails)
                    {

                        WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(item.Date.DayOfWeek.ToString())).FirstOrDefault();
                        if (isweekend != null)
                        {
                            return false;
                        }
                        else
                        {
                            Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, item.Date.Date, leaveView.LocationId, leaveView.ShiftId);
                            if (holidayDate != null)
                            {
                                return false;
                            }
                        }
                    }
                }
                else if (leaverestriction?.Weekendsbetweenleaveperiod == false && leaverestriction?.Holidaybetweenleaveperiod == true)
                {
                    int holidaycount = 0;
                    foreach (AppliedLeaveDetailsView item in leaveView?.AppliedLeaveDetails)
                    {
                        WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(item.Date.DayOfWeek.ToString())).FirstOrDefault();
                        if (isweekend != null)
                        {
                            return false;
                        }

                        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, item.Date.Date, leaveView.LocationId, leaveView.ShiftId);
                        if (holidayDate != null)
                        {
                            holidaycount = holidaycount + 1;
                        }
                    }
                    if (holidaycount == totaldays)
                    {
                        return false;
                    }
                }
                else if (leaverestriction?.Weekendsbetweenleaveperiod == true && leaverestriction?.Holidaybetweenleaveperiod == false)
                {
                    int weekendcount = 0;
                    foreach (AppliedLeaveDetailsView item in leaveView?.AppliedLeaveDetails)
                    {
                        WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(item.Date.DayOfWeek.ToString())).FirstOrDefault();
                        if (isweekend != null)
                        {
                            weekendcount = weekendcount + 1;
                        }

                        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, item.Date.Date, leaveView.LocationId, leaveView.ShiftId);
                        if (holidayDate != null)
                        {
                            return false;
                        }
                    }
                    if (weekendcount == totaldays)
                    {
                        return false;
                    }
                }
                else if (leaverestriction?.Weekendsbetweenleaveperiod == true && leaverestriction?.Holidaybetweenleaveperiod == true)
                {
                    int weekendHolidaycount = 0;
                    foreach (AppliedLeaveDetailsView item in leaveView?.AppliedLeaveDetails)
                    {
                        WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(item.Date.DayOfWeek.ToString())).FirstOrDefault();
                        if (isweekend != null)
                        {
                            weekendHolidaycount = weekendHolidaycount + 1;
                        }

                        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, item.Date.Date, leaveView.LocationId, leaveView.ShiftId);
                        if (holidayDate != null)
                        {
                            weekendHolidaycount = weekendHolidaycount + 1;
                        }
                    }
                    if (weekendHolidaycount == totaldays)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        #endregion Check weekend or holiday
        #region Check employee skip weekend or holiday
        public bool CheckEmployeeSkipWeekendOrHoliday(int employeeId, ApplyLeavesView leaveView, List<WeekendViewDefinition> WeekendList, LeaveTypeRestrictionsView leaverestriction)
        {
            int totaldays = leaveView == null ? 0 : leaveView.AppliedLeaveDetails == null ? 0 : leaveView.AppliedLeaveDetails.Count;
            if (totaldays > 0)
            {
                if (leaverestriction?.Weekendsbetweenleaveperiod == true && leaverestriction?.Holidaybetweenleaveperiod == true)
                {
                    //Pre days
                    DateTime leaveFistDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).FirstOrDefault();
                    bool isWorkingDay = true;
                    bool isCrossedWeekend = false;
                    int i = -1;
                    while (isWorkingDay == true)
                    {
                        DateTime preDate = leaveFistDay.AddDays(i);
                        WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(preDate.DayOfWeek.ToString())).FirstOrDefault();
                        if (isweekend == null)
                        {
                            Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, preDate, leaveView.LocationId, leaveView.ShiftId);
                            if (holidayDate == null)
                            {
                                if (isCrossedWeekend)
                                {
                                    bool data = _AppliedLeaveDetailsRepository.GetPreviousConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, preDate, leaveView.LeaveId);
                                    if (data == true)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        isWorkingDay = false;
                                    }
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }
                            }
                            else
                            {
                                bool data = _AppliedLeaveDetailsRepository.CheckPreviousConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, preDate, leaveView.LeaveId);
                                if (data == false)
                                {
                                    isCrossedWeekend = true;
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }
                            }

                        }
                        else
                        {
                            bool data = _AppliedLeaveDetailsRepository.CheckPreviousConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, preDate, leaveView.LeaveId);
                            if (data == false)
                            {
                                isCrossedWeekend = true;
                            }
                            else
                            {
                                isWorkingDay = false;
                            }

                        }

                        i = i - 1;
                    }


                    //Next days
                    DateTime leavelastDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).LastOrDefault();
                    isWorkingDay = true; isCrossedWeekend = false;
                    int j = 1;
                    while (isWorkingDay == true)
                    {
                        DateTime lastDate = leavelastDay.AddDays(j);
                        WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(lastDate.DayOfWeek.ToString())).FirstOrDefault();
                        if (isweekend == null)
                        {
                            Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, lastDate, leaveView.LocationId, leaveView.ShiftId);
                            if (holidayDate == null)
                            {
                                if (isCrossedWeekend)
                                {
                                    bool data = _AppliedLeaveDetailsRepository.GetNextConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lastDate, leaveView.LeaveId);
                                    if (data == true)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        isWorkingDay = false;
                                    }
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }
                            }
                            else
                            {
                                bool data = _AppliedLeaveDetailsRepository.CheckNextConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lastDate, leaveView.LeaveId);
                                if (data == false)
                                {
                                    isCrossedWeekend = true;
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }
                            }

                        }
                        else
                        {
                            bool data = _AppliedLeaveDetailsRepository.CheckNextConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lastDate, leaveView.LeaveId);
                            if (data == false)
                            {
                                isCrossedWeekend = true;
                            }
                            else
                            {
                                isWorkingDay = false;
                            }

                        }

                        j = j + 1;
                    }
                }
                else
                {
                    if (leaverestriction?.Weekendsbetweenleaveperiod == true)
                    {
                        //Pre days
                        DateTime leaveFistDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).FirstOrDefault();
                        bool isWorkingDay = true;
                        bool isCrossedWeekend = false; isCrossedWeekend = false;
                        int i = -1;
                        while (isWorkingDay == true)
                        {
                            DateTime preDate = leaveFistDay.AddDays(i);
                            WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(preDate.DayOfWeek.ToString())).FirstOrDefault();
                            if (isweekend == null)
                            {
                                if (isCrossedWeekend)
                                {
                                    bool data = _AppliedLeaveDetailsRepository.GetPreviousConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, preDate, leaveView.LeaveId);
                                    if (data == true)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        isWorkingDay = false;
                                    }
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }
                            }
                            else
                            {
                                bool data = _AppliedLeaveDetailsRepository.CheckPreviousConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, preDate, leaveView.LeaveId);
                                if (data == false)
                                {
                                    isCrossedWeekend = true;
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }

                            }

                            i = i - 1;
                        }


                        //Next days
                        DateTime leavelastDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).LastOrDefault();
                        isWorkingDay = true; isCrossedWeekend = false;
                        int j = 1;
                        while (isWorkingDay == true)
                        {
                            DateTime lastDate = leavelastDay.AddDays(j);
                            WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(lastDate.DayOfWeek.ToString())).FirstOrDefault();
                            if (isweekend == null)
                            {
                                if (isCrossedWeekend)
                                {
                                    bool data = _AppliedLeaveDetailsRepository.GetNextConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lastDate, leaveView.LeaveId);
                                    if (data == true)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        isWorkingDay = false;
                                    }
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }
                            }
                            else
                            {
                                bool data = _AppliedLeaveDetailsRepository.CheckNextConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lastDate, leaveView.LeaveId);
                                if (data == false)
                                {
                                    isCrossedWeekend = true;
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }

                            }
                            j = j + 1;
                        }
                    }

                    if (leaverestriction?.Holidaybetweenleaveperiod == true)
                    {
                        //Pre days
                        DateTime leaveFistDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).FirstOrDefault();
                        bool isWorkingDay = true;
                        bool isCrossedHoliday = false;
                        int i = -1;
                        while (isWorkingDay == true)
                        {
                            DateTime preDate = leaveFistDay.AddDays(i);
                            Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, preDate, leaveView.LocationId, leaveView.ShiftId);
                            if (holidayDate == null)
                            {
                                if (isCrossedHoliday)
                                {
                                    bool data = _AppliedLeaveDetailsRepository.GetPreviousConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, preDate, leaveView.LeaveId);
                                    if (data == true)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        isWorkingDay = false;
                                    }
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }
                            }
                            else
                            {
                                bool data = _AppliedLeaveDetailsRepository.CheckPreviousConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, preDate, leaveView.LeaveId);
                                if (data == false)
                                {
                                    isCrossedHoliday = true;
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }

                            }

                            i = i - 1;
                        }


                        //Next days
                        DateTime leavelastDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).LastOrDefault();
                        int j = 1;
                        isWorkingDay = true; isCrossedHoliday = false;
                        while (isWorkingDay == true)
                        {
                            DateTime lastDate = leavelastDay.AddDays(j);
                            Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, lastDate, leaveView.LocationId, leaveView.ShiftId);
                            if (holidayDate == null)
                            {
                                if (isCrossedHoliday)
                                {
                                    bool data = _AppliedLeaveDetailsRepository.GetNextConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lastDate, leaveView.LeaveId);
                                    if (data == true)
                                    {
                                        return false;
                                    }
                                    else
                                    {
                                        isWorkingDay = false;
                                    }
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }
                            }
                            else
                            {
                                bool data = _AppliedLeaveDetailsRepository.CheckNextConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lastDate, leaveView.LeaveId);
                                if (data == false)
                                {
                                    isCrossedHoliday = true;
                                }
                                else
                                {
                                    isWorkingDay = false;
                                }

                            }
                            j = j + 1;
                        }
                    }
                }

            }

            return true;
        }
        #endregion Check weekend or holiday
        #region Get Consecutive Employe Applied LeaveDetails
        public bool GetConsecutiveEmployeAppliedLeaveDetails(int employeeId, ApplyLeavesView leaveView, List<WeekendViewDefinition> WeekendList, decimal maximumConsicutiveDays)
        {
            int totaldays = leaveView == null ? 0 : leaveView.AppliedLeaveDetails == null ? 0 : leaveView.AppliedLeaveDetails.Count;
            if (totaldays > 0)
            {
                List<AppliedLeaveDetailsView> checkConsecutiveDays = new();
                foreach (AppliedLeaveDetailsView item in leaveView.AppliedLeaveDetails)
                {
                    if (item.IsFullDay)
                    {
                        checkConsecutiveDays.Add(item);
                    }
                    else if (item.IsFirstHalf)
                    {
                        AppliedLeaveDetailsView appliedLeave = new AppliedLeaveDetailsView();
                        appliedLeave.Date = item.Date;
                        appliedLeave.IsFullDay = item.IsFullDay ? true : false;
                        appliedLeave.IsFirstHalf = item.IsFirstHalf ? true : false;
                        appliedLeave.IsSecondHalf = item.IsSecondHalf ? true : false;
                        bool isScondOffLeave = _AppliedLeaveDetailsRepository.checkAppliedLeaveByDate(employeeId, leaveView.LeaveTypeId, item.Date, false, false, true, leaveView.LeaveId);
                        if (isScondOffLeave)
                        {
                            appliedLeave.IsFullDay = true;
                            appliedLeave.IsFirstHalf = false;
                        }
                        checkConsecutiveDays.Add(appliedLeave);
                    }
                    else if (item.IsSecondHalf)
                    {
                        AppliedLeaveDetailsView appliedLeave = new AppliedLeaveDetailsView();
                        appliedLeave.Date = item.Date;
                        appliedLeave.IsFullDay = item.IsFullDay ? true : false;
                        appliedLeave.IsFirstHalf = item.IsFirstHalf ? true : false;
                        appliedLeave.IsSecondHalf = item.IsSecondHalf ? true : false;
                        bool isFirstOffLeave = _AppliedLeaveDetailsRepository.checkAppliedLeaveByDate(employeeId, leaveView.LeaveTypeId, item.Date, false, true, false, leaveView.LeaveId);
                        if (isFirstOffLeave)
                        {
                            appliedLeave.IsFullDay = true;
                            appliedLeave.IsFirstHalf = false;
                        }
                        checkConsecutiveDays.Add(appliedLeave);
                    }
                }
                //Pre days
                DateTime leaveFistDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).FirstOrDefault();
                int i = -1;
                bool isApplied = false;
                while (isApplied == false)
                {
                    DateTime preDate = DateTime.Now;
                    bool isNotHoliday = true;
                    while (isNotHoliday == true)
                    {
                        preDate = leaveFistDay.AddDays(i);
                        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, preDate, leaveView.LocationId, leaveView.ShiftId);
                        isNotHoliday = holidayDate == null ? false : true;
                        if (isNotHoliday == false)
                        {
                            WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(preDate.DayOfWeek.ToString())).FirstOrDefault();

                            isNotHoliday = isweekend == null ? false : true;
                        }
                        i = i - 1;
                    }
                    List<AppliedLeaveDetails> result = null;
                    result = _AppliedLeaveDetailsRepository.GetConsecutiveAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, preDate, leaveView.LeaveId);
                    if (result?.Count == 0)
                    {
                        isApplied = false;
                        break;
                    }
                    else if (result?.Count == 1)
                    {
                        AppliedLeaveDetailsView appliedLeave = new AppliedLeaveDetailsView();
                        appliedLeave.Date = preDate;
                        appliedLeave.IsFullDay = result[0].IsFullDay;
                        appliedLeave.IsFirstHalf = result[0].IsFirstHalf;
                        appliedLeave.IsSecondHalf = result[0].IsSecondHalf;
                        checkConsecutiveDays.Add(appliedLeave);
                        if (result[0].IsFirstHalf || result[0].IsSecondHalf)
                        {
                            isApplied = false;
                            break;
                        }
                    }
                    else if (result?.Count == 2)
                    {
                        AppliedLeaveDetailsView appliedLeave = new AppliedLeaveDetailsView();
                        appliedLeave.Date = preDate;
                        appliedLeave.IsFullDay = true;
                        appliedLeave.IsFirstHalf = false;
                        appliedLeave.IsSecondHalf = false;
                        checkConsecutiveDays.Add(appliedLeave);
                    }
                }

                //Next days
                DateTime leavelastDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).LastOrDefault();
                int j = 1;
                isApplied = false;
                while (isApplied == false)
                {
                    DateTime lstDate = DateTime.Now;
                    bool isNotHoliday = true;

                    while (isNotHoliday == true)
                    {
                        lstDate = leavelastDay.AddDays(j);
                        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, lstDate, leaveView.LocationId, leaveView.ShiftId);
                        isNotHoliday = holidayDate == null ? false : true;
                        if (isNotHoliday == false)
                        {
                            WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(lstDate.DayOfWeek.ToString())).FirstOrDefault();

                            isNotHoliday = isweekend == null ? false : true;
                        }
                        j = j + 1;
                    }
                    List<AppliedLeaveDetails> result = null;
                    result = _AppliedLeaveDetailsRepository.GetConsecutiveAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lstDate, leaveView.LeaveId);
                    if (result?.Count == 0)
                    {
                        isApplied = false;
                        break;
                    }
                    else if (result?.Count == 1)
                    {
                        AppliedLeaveDetailsView appliedLeave = new AppliedLeaveDetailsView();
                        appliedLeave.Date = lstDate;
                        appliedLeave.IsFullDay = result[0].IsFullDay;
                        appliedLeave.IsFirstHalf = result[0].IsFirstHalf;
                        appliedLeave.IsSecondHalf = result[0].IsSecondHalf;
                        checkConsecutiveDays.Add(appliedLeave);
                        if (result[0].IsFirstHalf || result[0].IsSecondHalf)
                        {
                            isApplied = false;
                            break;
                        }
                    }
                    else if (result?.Count == 2)
                    {
                        AppliedLeaveDetailsView appliedLeave = new AppliedLeaveDetailsView();
                        appliedLeave.Date = lstDate;
                        appliedLeave.IsFullDay = true;
                        appliedLeave.IsFirstHalf = false;
                        appliedLeave.IsSecondHalf = false;
                        checkConsecutiveDays.Add(appliedLeave);
                    }
                }
                decimal consecutiveDays = 0;
                checkConsecutiveDays = checkConsecutiveDays.OrderBy(x => x.Date).ToList();
                foreach (AppliedLeaveDetailsView appliedLeave in checkConsecutiveDays)
                {
                    List<AppliedLeaveDetails> result = null;
                    if (appliedLeave.IsFullDay)
                    {
                        consecutiveDays += 1;
                    }
                    else if (appliedLeave.IsFirstHalf)
                    {
                        consecutiveDays += 0.5M;
                        if (consecutiveDays > maximumConsicutiveDays)
                        {
                            return false;
                        }
                        consecutiveDays = 0;
                    }
                    else if ((appliedLeave.IsSecondHalf))
                    {
                        consecutiveDays = 0.5M;
                    }
                    if (consecutiveDays > maximumConsicutiveDays)
                    {
                        return false;
                    }
                }

                ////Pre days
                //DateTime leaveFistDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).FirstOrDefault();
                //int preDays = totaldays;
                //int i = -1;
                //for (int x = totaldays; x <= maximumConsicutiveDays + 1; x++)
                //{
                //    DateTime preDate = DateTime.Now;
                //    bool isNotHoliday = true;

                //    while (isNotHoliday == true)
                //    {
                //        preDate = leaveFistDay.AddDays(i);
                //        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, preDate, leaveView.LocationId, leaveView.ShiftId);
                //        isNotHoliday = holidayDate == null ? false : true;
                //        if (isNotHoliday == false)
                //        {
                //            WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(preDate.DayOfWeek.ToString())).FirstOrDefault();

                //            isNotHoliday = isweekend == null ? false : true;
                //        }
                //        i = i - 1;
                //    }
                //    AppliedLeaveDetails result = null;
                //    bool isAlreadyApplied = false;
                //    if (leaveView.LeaveId > 0)
                //    {
                //        List<AppliedLeaveDetails> leaveDetails = _AppliedLeaveDetailsRepository.GetByID(leaveView.LeaveId);
                //        if (leaveDetails?.Count > 0)
                //        {
                //            isAlreadyApplied = leaveDetails.Any(x => x.Date.Date == preDate.Date);
                //            if (isAlreadyApplied)
                //            {
                //                result = new AppliedLeaveDetails();
                //                preDays = preDays - 1;
                //            }
                //            else
                //            {
                //                result = null;
                //            }

                //        }
                //    }

                //    if (isAlreadyApplied == false)
                //    {
                //        result = _AppliedLeaveDetailsRepository.GetConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, preDate);
                //    }
                //    if (result != null)
                //    {
                //        preDays = preDays + 1;
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (preDays > maximumConsicutiveDays)
                //    {
                //        return false;
                //    }
                //}

                ////Next days
                //DateTime leavelastDay = leaveView.AppliedLeaveDetails.Select(x => x.Date).LastOrDefault();
                //int lastDays = totaldays;
                //int j = 1;
                //for (int x = totaldays; x <= maximumConsicutiveDays + 1; x++)
                //{
                //    DateTime lstDate = DateTime.Now;
                //    bool isNotHoliday = true;

                //    while (isNotHoliday == true)
                //    {
                //        lstDate = leavelastDay.AddDays(j);
                //        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(leaveView.DepartmentId, lstDate, leaveView.LocationId, leaveView.ShiftId);
                //        isNotHoliday = holidayDate == null ? false : true;
                //        if (isNotHoliday == false)
                //        {
                //            WeekendViewDefinition isweekend = WeekendList?.Where(x => x.WeekendDayName.Contains(lstDate.DayOfWeek.ToString())).FirstOrDefault();

                //            isNotHoliday = isweekend == null ? false : true;
                //        }
                //        j = j + 1;
                //    }
                //    AppliedLeaveDetails result = null;
                //    bool isAlreadyApplied = false;
                //    if (leaveView.LeaveId > 0)
                //    {
                //        List<AppliedLeaveDetails> leaveDetails = _AppliedLeaveDetailsRepository.GetByID(leaveView.LeaveId);
                //        if (leaveDetails?.Count > 0)
                //        {
                //            isAlreadyApplied = leaveDetails.Any(x => x.Date.Date == lstDate.Date);
                //            if (isAlreadyApplied)
                //            {
                //                result = new AppliedLeaveDetails();
                //                lastDays = lastDays - 1;
                //            }
                //            else
                //            {
                //                result = null;
                //            }
                //        }
                //    }

                //    if (isAlreadyApplied == false)
                //    {
                //        result = _AppliedLeaveDetailsRepository.GetConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lstDate);
                //    }
                //    //AppliedLeaveDetails result = _AppliedLeaveDetailsRepository.GetConsecutiveEmployeAppliedLeaveDetails(employeeId, leaveView.LeaveTypeId, lstDate);
                //    if (result != null)
                //    {
                //        lastDays = lastDays + 1;
                //    }
                //    else
                //    {
                //        break;
                //    }
                //    if (lastDays > maximumConsicutiveDays)
                //    {
                //        return false;
                //    }
                //}


            }

            return true;
        }
        #endregion

        #region Download document
        public SupportingDocuments DownloadLeaveGrantDocumentById(int documentId)
        {
            SupportingDocuments documentDetails = new SupportingDocuments();
            try
            {
                if (documentId > 0)
                {
                    documentDetails = _leaveGrantDocumentDetailsRepository.DownloadLeaveGrantDocumentById(documentId);
                }
            }
            catch (Exception)
            {
                throw;
                //logger.Error(ex.Message.ToString());
            }
            return documentDetails;
        }
        #endregion
        //#region Update Grant Leaves Request        
        //public async Task<bool> UpdateGrantLeavesRequest(DateTime todayDate)
        //{
        //    List<LeaveGrantRequestDetails> leaveGrantRequestlist = new();
        //    EmployeeLeaveDetails empleavebalancedetail = new();
        //    try
        //    {
        //        leaveGrantRequestlist = _leaveGrantRequestDetailsRepository.GetGrantLeaveRequestByIsActive();

        //        foreach (LeaveGrantRequestDetails leaveGrantRequestDetails in leaveGrantRequestlist)
        //        {
        //            try
        //            {
        //                if (leaveGrantRequestDetails?.BalanceDay > 0 && leaveGrantRequestDetails?.EffectiveToDate < todayDate.Date)
        //                {
        //                    leaveGrantRequestDetails.IsActive = false;
        //                    leaveGrantRequestDetails.ModifiedOn = DateTime.UtcNow;
        //                    _leaveGrantRequestDetailsRepository.Update(leaveGrantRequestDetails);
        //                    await _leaveGrantRequestDetailsRepository.SaveChangesAsync();

        //                    EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(leaveGrantRequestDetails.EmployeeID, leaveGrantRequestDetails.LeaveTypeId);
        //                    if (employeeLeaveDetails != null)
        //                    {
        //                        decimal totalAvailableLeave = leaveGrantRequestDetails?.BalanceDay == null ? 0 : (decimal)leaveGrantRequestDetails?.BalanceDay;


        //                        if (employeeLeaveDetails.AdjustmentEffectiveFromDate != null)
        //                        {
        //                            if (employeeLeaveDetails?.AdjustmentEffectiveFromDate <= leaveGrantRequestDetails.EffectiveFromDate)
        //                            {
        //                                employeeLeaveDetails.AdjustmentBalanceLeave = (employeeLeaveDetails?.AdjustmentBalanceLeave == null ? 0 : (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave) - totalAvailableLeave;
        //                            }
        //                            else
        //                            {
        //                                employeeLeaveDetails.BalanceLeave = (employeeLeaveDetails?.BalanceLeave == null ? 0 : (decimal)employeeLeaveDetails?.BalanceLeave) - totalAvailableLeave;
        //                            }
        //                        }
        //                        else
        //                        {
        //                            employeeLeaveDetails.BalanceLeave = (employeeLeaveDetails?.BalanceLeave == null ? 0 : (decimal)employeeLeaveDetails?.BalanceLeave) - totalAvailableLeave;
        //                        }
        //                        employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
        //                        _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
        //                        await _LeaveApplyRepository.SaveChangesAsync();
        //                    }
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateGrantLeavesRequest Leavetype Id", Convert.ToString(leaveGrantRequestDetails.LeaveTypeId));
        //            }

        //        }


        //    }
        //    catch (Exception ex)
        //    {
        //        LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateGrantLeavesRequest");
        //    }
        //    return true;
        //}
        //#endregion
        #region update applied leave balance     
        public async Task<bool> UpdateAppliedLeaveBalance()
        {

            try
            {
                List<AppliedLeaveDetails> appliedLeaveList = _AppliedLeaveDetailsRepository.GetAppliedLeaveByDate(DateTime.Now.Date);

                foreach (AppliedLeaveDetails appliedLeave in appliedLeaveList)
                {
                    try
                    {
                        bool isGrant = _leaveTypeRepository.CheckIsGrantLeaveByLeaveId(appliedLeave?.LeaveId == null ? 0 : (int)appliedLeave.LeaveId);
                        if (isGrant==false)
                        {
                            EmployeeLeaveDetails employeeLeaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByLeaveId(appliedLeave.LeaveId == null ? 0 : (int)appliedLeave.LeaveId);
                            if (employeeLeaveDetails != null)
                            {
                                decimal leaveTotal = 0;
                                if (appliedLeave.IsFullDay)
                                {
                                    leaveTotal += (decimal)1;
                                }
                                if (appliedLeave.IsFirstHalf || appliedLeave.IsSecondHalf)
                                {
                                    leaveTotal += (decimal)0.5;
                                }
                                employeeLeaveDetails.BalanceLeave = (decimal)employeeLeaveDetails?.BalanceLeave - leaveTotal;
                                if (employeeLeaveDetails.AdjustmentEffectiveFromDate != null && employeeLeaveDetails?.AdjustmentEffectiveFromDate <= DateTime.Now.Date)
                                {
                                    employeeLeaveDetails.AdjustmentBalanceLeave = (decimal)employeeLeaveDetails?.AdjustmentBalanceLeave - leaveTotal;
                                }
                                _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                await _LeaveApplyRepository.SaveChangesAsync();
                            }
                        }

                    }
                    catch (Exception ex)
                    {
                        LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateAppliedLeaveBalance,  Leavetype Id", Convert.ToString(appliedLeave.LeaveId));
                    }
                }
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/UpdateAppliedLeaveBalance");
            }
            return true;
        }
        #endregion
        #region Get Employee Apply Leave Request Details
        public List<ApplyLeaves> GetLeaveRequestByEmployeeId(int employeeId, int LeaveTypeId, DateTime fromDate, DateTime toDate, int leaveId, bool isEdit)
        {
            return _leaveRepository.GetLeaveRequestByEmployeeId(employeeId, LeaveTypeId, fromDate, toDate, leaveId, isEdit);
        }
        #endregion
        #region Get reset date
        public DateTime GetLeaveResetDate(string period, string resetDay, int resetMonth)
        {
            DateTime resetDate = default;
            if (period?.ToLower() == "monthly".ToLower())
            {
                if (resetDay?.ToLower() == "firstdays".ToLower())
                {
                    resetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                }
                else if (resetDay?.ToLower() == "lastdays".ToLower())
                {
                    resetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
                }
                else
                {
                    resetDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, Convert.ToInt32(resetDay));
                }
                if (resetDate <= DateTime.Now.Date)
                {
                    resetDate = resetDate.AddMonths(1);
                }
            }
            else if (period?.ToLower() == "quarterly".ToLower())
            {
                FinancialYearDateView Quarter = GetFinancialYearQuarter(DateTime.Now.Date);
                if (resetDay?.ToLower() == "FirstDays".ToLower())
                {
                    resetDate = Quarter.FromDate;
                }
                else if (resetDay?.ToLower() == "LastDays".ToLower())
                {
                    resetDate = Quarter.ToDate;
                }
                if (resetDate <= DateTime.Now)
                {
                    resetDate = resetDate.AddMonths(3);
                }
            }
            else if (period?.ToLower() == "halfYearly".ToLower())
            {
                FinancialYearDateView HalfYearly = GetFinancialYearHalfYearly(DateTime.Now.Date);
                if (resetDay?.ToLower() == "FirstDays".ToLower())
                {
                    resetDate = HalfYearly.FromDate;
                }
                else if (resetDay?.ToLower() == "LastDays".ToLower())
                {
                    resetDate = HalfYearly.ToDate;
                }
                if (resetDate <= DateTime.Now)
                {
                    resetDate = resetDate.AddMonths(6);
                }
            }
            else if (period?.ToLower() == "yearly".ToLower())
            {

                int month = (int)resetMonth;
                int year = DateTime.Now.Year;
                if (resetDay?.ToLower() == "firstdays".ToLower())
                {
                    resetDate = new DateTime(year, month, 1);
                }
                else if (resetDay?.ToLower() == "lastdays".ToLower())
                {
                    resetDate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                }
                else
                {
                    resetDate = new DateTime(year, month, Convert.ToInt32(resetDay));
                }
                if (resetDate <= DateTime.Now)
                {
                    resetDate = resetDate.AddMonths(12);
                }
            }

            return resetDate;
        }
        #endregion

        //#region LeaveAccruedOnCreate
        //public async Task<bool> LeaveAccruedOnCreate(OneTimeEmployeeLeaveView EmployeeList)
        //{
        //    try
        //    {


        //        return true;
        //    }
        //    catch (Exception ex)
        //    {

        //        throw ex;
        //    }
        //}

        //#endregion


        #region LeaveAccruedOnEffectiveFromDate
        public async Task<bool> LeaveAccruedOnEffectiveFromDate(tempparameter temp)
        {
            List<EmployeeDetailsForLeaveView> EmployeeList = temp?.EmployeeDetail;
            DateTime todayDate = temp.executedate == null ? DateTime.Now.Date : (DateTime)temp.executedate;

            //DateTime todayDate =  DateTime.UtcNow.Date;

            try
            {
                LeaveTypes leaveDetails = new();
                List<LeaveTypes> leavetypeslist = new();
                LeaveApplicable leaveApplicable = new();
                List<ProRateMonthDetailsView> proRateMonthDetails = new();
                AppConstants appConstants = new();

                if (EmployeeList.Count != 0)
                {
                    leavetypeslist = _leaveTypeRepository?.GetAllLeaveType(todayDate);
                    //leavetypeslist = leavetypeslist.Where(x => x.LeaveTypeId == 27).Select(x => x).ToList();
                    foreach (LeaveTypes leaveTypesList in leavetypeslist)
                    {
                        try
                        {
                            DateTime Executeddate = default;
                            var Quarter = GetFinancialYearQuarter(todayDate);
                            var HalfYearly = GetFinancialYearHalfYearly(todayDate);
                            var Yearly = GetFinancialYearly(todayDate);

                            appConstants = _appConstantsRepository.GetAppconstantByID(leaveTypesList.LeaveAccruedType);
                            if (appConstants != null)
                            {
                                if (appConstants?.DisplayName?.ToLower() == "Monthly".ToLower())
                                {
                                    if (leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower())
                                    {
                                        Executeddate = new DateTime(todayDate.Year, todayDate.Month, 1);
                                    }
                                    else if (leaveTypesList?.LeaveAccruedDay?.ToLower() == "LastDays".ToLower())
                                    {
                                        Executeddate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));
                                    }
                                    else
                                    {
                                        Executeddate = new DateTime(todayDate.Year, todayDate.Month, Convert.ToInt32(leaveTypesList.LeaveAccruedDay));
                                    }
                                }
                                else if (appConstants?.DisplayName?.ToLower() == "Quarterly".ToLower())
                                {
                                    Executeddate = leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower() ? Quarter.FromDate : leaveTypesList.LeaveAccruedDay.ToLower() == "LastDays".ToLower() ? Quarter.ToDate : default;

                                }
                                else if (appConstants?.DisplayName?.ToLower() == "HalfYearly".ToLower())
                                {
                                    Executeddate = leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower() ? HalfYearly.FromDate : leaveTypesList.LeaveAccruedDay.ToLower() == "LastDays".ToLower() ? HalfYearly.ToDate : default;
                                }
                                else if (appConstants?.DisplayName?.ToLower() == "Yearly".ToLower())
                                {
                                    Executeddate = leaveTypesList?.LeaveAccruedDay?.ToLower() == "FirstDays".ToLower() ? Yearly.FromDate : leaveTypesList.LeaveAccruedDay.ToLower() == "LastDays".ToLower() ? Yearly.ToDate : default;
                                }
                                if (leaveTypesList?.EffectiveFromDate == todayDate && leaveTypesList?.EffectiveFromDate > Executeddate)
                                {
                                    leaveDetails = leaveTypesList;
                                }
                                else
                                {
                                    leaveDetails = null;
                                }
                            }
                            var Basedon = _appConstantsRepository.GetAppconstantByID(leaveTypesList?.BalanceBasedOn);
                            if (leaveDetails != null && (Basedon == null || Basedon?.AppConstantValue?.ToLower() == "FixedEntitlement".ToLower()))
                            {
                                leaveApplicable = _leaveApplicableRepository.GetleaveApplicableByLeaveId(leaveDetails.LeaveTypeId);

                                //EmployeeList = EmployeeList.Where(x => x.EmployeeID == 126).ToList();
                                foreach (EmployeeDetailsForLeaveView item in EmployeeList)
                                {
                                    try
                                    {



                                        switch (item?.Gender?.ToLower())
                                        {
                                            case "male":
                                                item.GenderMale = true;
                                                item.GenderFemale = null;
                                                item.GenderOther = null;

                                                break;
                                            case "female":
                                                item.GenderMale = null;
                                                item.GenderFemale = true;
                                                item.GenderOther = null;
                                                break;
                                            case "other":
                                                item.GenderMale = null;
                                                item.GenderFemale = null;
                                                item.GenderOther = true;
                                                break;
                                            default:
                                                item.GenderMale = null;
                                                item.GenderFemale = null;
                                                item.GenderOther = null;
                                                break;
                                        }
                                        switch (item?.MaritalStatus?.ToLower())
                                        {
                                            case "single":
                                                item.MaritalStatusSingle = true;
                                                item.MaritalStatusMarried = null;
                                                break;
                                            case "married":
                                                item.MaritalStatusSingle = null;
                                                item.MaritalStatusMarried = true;
                                                break;
                                            default:
                                                item.MaritalStatusSingle = null;
                                                item.MaritalStatusMarried = null;
                                                break;
                                        }
                                        bool leaveapplicable = false;
                                        bool isEligible = true;
                                        bool isApplicable = false;
                                        var prob = item.ProbationStatusID == 0 ? null : item.ProbationStatusID == null ? null : item.ProbationStatusID;

                                        if ((leaveApplicable?.Gender_Male == true && item?.GenderMale == true) || (leaveApplicable?.Gender_Female == true && item?.GenderFemale == true) || (leaveApplicable?.Gender_Others == true && item?.GenderOther == true))
                                        {
                                            leaveapplicable = true;
                                            isApplicable = true;
                                        }
                                        else if (leaveApplicable?.Gender_Male != true && leaveApplicable?.Gender_Female != true && leaveApplicable?.Gender_Others != true)
                                        {
                                            leaveapplicable = true;
                                        }
                                        else
                                        {
                                            isEligible = false;
                                        }
                                        if ((leaveApplicable?.MaritalStatus_Single == true && item?.MaritalStatusSingle == true) || (leaveApplicable?.MaritalStatus_Married == true && item?.MaritalStatusMarried == true))
                                        {
                                            leaveapplicable = true;
                                            isApplicable = true;
                                        }
                                        else if (leaveApplicable?.MaritalStatus_Single != true && leaveApplicable?.MaritalStatus_Married != true)
                                        {
                                            leaveapplicable = true;
                                        }
                                        else
                                        {
                                            isEligible = false;
                                        }
                                        //if (leaveApplicable?.EmployeeTypeId != 0 && leaveApplicable?.EmployeeTypeId != item?.EmployeeTypeID)
                                        //{
                                        //    isEligible = false;
                                        //}
                                        //else if (leaveApplicable?.EmployeeTypeId != 0 && leaveApplicable?.EmployeeTypeId == item?.EmployeeTypeID)
                                        //{
                                        //    isApplicable = true;
                                        //}
                                        //else
                                        //{
                                        //    leaveapplicable = true;
                                        //}
                                        //if (leaveApplicable?.ProbationStatus != 0 && leaveApplicable?.ProbationStatus != prob)
                                        //{
                                        //    isEligible = false;
                                        //}
                                        //else if (leaveApplicable?.ProbationStatus != 0 && leaveApplicable?.ProbationStatus == prob)
                                        //{
                                        //    isApplicable = true;
                                        //}
                                        //else
                                        //{
                                        //    leaveapplicable = true;
                                        //}



                                        //if (leaveApplicable?.Gender_Male == item?.GenderMale ||
                                        //    leaveApplicable?.Gender_Female == item?.GenderFemale ||
                                        //    leaveApplicable?.Gender_Others == item.GenderOther ||
                                        //    leaveApplicable?.MaritalStatus_Single == item.MaritalStatusSingle ||
                                        //    leaveApplicable?.MaritalStatus_Married == item.MaritalStatusMarried ||
                                        //    leaveApplicable?.EmployeeTypeId == item.EmployeeTypeID ||
                                        //    leaveApplicable?.ProbationStatus == prob
                                        //    )
                                        //{
                                        //    leaveapplicable = true;
                                        //}

                                        EmployeeLeaveApplicableView applicable = GetLeaveApplicable(leaveDetails, item);
                                        var exceptions = GetLeaveExceptions(leaveDetails, item);

                                        bool employeeApplicable = false;
                                        List<EmployeeApplicableLeave> employeeApplicableLeaves = _employeeApplicableLeaveRepository.GetEmployeeApplicableLeaveDetailsByLeaveTypeId(leaveDetails.LeaveTypeId);
                                        if (employeeApplicableLeaves.Any(x => x.EmployeeId != 0) && employeeApplicableLeaves.Any(m => m.EmployeeId == item.EmployeeID))
                                        {
                                            employeeApplicable = true;
                                        }
                                        bool leaveexception = false;
                                        bool isEligibleException = false;
                                        bool isException = false;
                                        LeaveApplicable leaveException = new();
                                        leaveException = _leaveApplicableRepository.GetleaveExceptionByLeaveId(leaveDetails.LeaveTypeId);

                                        if ((leaveException?.Gender_Male_Exception == true && item?.GenderMale == true) || (leaveException?.Gender_Female_Exception == true && item?.GenderFemale == true) || (leaveException?.Gender_Others_Exception == true && item?.GenderOther == true))
                                        {
                                            leaveexception = true;
                                            isException = true;
                                        }
                                        else if (leaveException?.Gender_Male_Exception != true && leaveException?.Gender_Female_Exception != true && leaveException?.Gender_Others_Exception != true)
                                        {
                                            leaveexception = false;
                                        }
                                        else
                                        {
                                            isEligibleException = false;
                                        }
                                        if ((leaveException?.MaritalStatus_Single_Exception == true && item?.MaritalStatusSingle == true) || (leaveException?.MaritalStatus_Married_Exception == true && item?.MaritalStatusMarried == true))
                                        {
                                            leaveexception = true;
                                            isException = true;
                                        }
                                        else if (leaveException?.MaritalStatus_Single_Exception != true && leaveException?.MaritalStatus_Married_Exception != true)
                                        {
                                            leaveexception = false;
                                        }
                                        else
                                        {
                                            isEligibleException = false;
                                        }
                                        //employee exception
                                        bool employeeException = false;
                                        List<EmployeeApplicableLeave> employeeExceptionLeaves = _employeeApplicableLeaveRepository.GetEmployeeExceptionLeaveDetailsByLeaveTypeId(leaveDetails.LeaveTypeId);
                                        if (employeeExceptionLeaves.Any(x => x.LeaveExceptionEmployeeId != 0) && employeeExceptionLeaves.Any(m => m.LeaveExceptionEmployeeId == item.EmployeeID))
                                        {
                                            employeeException = true;
                                        }
                                        EmployeeLeaveDetails employeeLeaveDetails = new();

                                        //if (leaveapplicable && applicable && exceptions == false)
                                        if ((isEligibleException == false && leaveexception == false && exceptions == false && isException == false) && (employeeException == false))
                                        {
                                            if ((isEligible && leaveapplicable && applicable.IsApplicable && (isApplicable || applicable.IsCreteriaMached)) || (employeeApplicable))
                                            {
                                                DateTime From = (item.DateOfJoining != null && leaveDetails?.EffectiveFromDate <= item.DateOfJoining) ? (DateTime)item.DateOfJoining : (DateTime)leaveDetails.EffectiveFromDate;
                                                DateTime To = (leaveDetails.EffectiveToDate != null && leaveDetails?.EffectiveToDate.Value.Date <= DateTime.Now.Date) ? leaveDetails.EffectiveToDate.Value.Date : DateTime.Now.Date;

                                                decimal? creditdates = 0;
                                                int totalMonths = 0;
                                                proRateMonthDetails = _proRateMonthDetailsRepository.GetProRateMonthDetailsByID(leaveDetails.LeaveTypeId);

                                                if (appConstants?.DisplayName?.ToLower() == "Monthly".ToLower())
                                                {

                                                    if (leaveDetails.ProRate == true)
                                                    {
                                                        var MonthStart = new DateTime(From.Year, From.Month, 1);
                                                        var MonthLast = MonthStart.AddMonths(1).AddDays(-1);
                                                        string ProrateFromday = null;
                                                        string ProrateToday = null;
                                                        foreach (ProRateMonthDetailsView ProRateitem in proRateMonthDetails)
                                                        {
                                                            if (ProRateitem?.Fromday?.ToLower() == "firstdays".ToLower())
                                                            {
                                                                ProrateFromday = "1";
                                                            }
                                                            else if (ProRateitem?.Fromday?.ToLower() == "lastdays".ToLower())
                                                            {
                                                                ProrateFromday = Convert.ToString(MonthLast.Day);
                                                            }
                                                            else
                                                            {
                                                                ProrateFromday = ProRateitem?.Fromday;
                                                            }
                                                            if (ProRateitem?.Today?.ToLower() == "firstdays".ToLower())
                                                            {
                                                                ProrateToday = "1";
                                                            }
                                                            else if (ProRateitem?.Today?.ToLower() == "lastdays".ToLower())
                                                            {
                                                                ProrateToday = Convert.ToString(MonthLast.Day);
                                                            }
                                                            else
                                                            {
                                                                ProrateToday = ProRateitem?.Today;
                                                            }
                                                            //fromdate                                                
                                                            var fromDate = new DateTime(From.Year, From.Month, Convert.ToInt32(ProrateFromday));
                                                            //todate
                                                            var toDate = new DateTime(From.Year, From.Month, Convert.ToInt32(ProrateToday));
                                                            if ((fromDate <= From.Date && From.Date <= toDate))
                                                            {
                                                                creditdates = (decimal)ProRateitem?.Count;
                                                                break;
                                                            }
                                                        }
                                                    }
                                                    else
                                                    {
                                                        creditdates = leaveDetails.LeaveAccruedNoOfDays;
                                                    }
                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "Quarterly".ToLower())
                                                {
                                                    FinancialYearDateView startqtr = GetFinancialYearQuarter(From);
                                                    FinancialYearDateView endqtr = GetFinancialYearQuarter(To);
                                                    totalMonths = 0;
                                                    DateTime toDate = DateTime.Now;
                                                    //bool isCurrentPeriod = false;
                                                    if (leaveDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                                    {
                                                        toDate = (leaveDetails.EffectiveToDate != null && leaveDetails?.EffectiveToDate.Value.Date <= endqtr.ToDate.Date) ? leaveDetails.EffectiveToDate.Value.Date : endqtr.ToDate.Date;
                                                        if (leaveDetails.ProRate == true)
                                                        {
                                                            totalMonths = ((toDate.Year * 12) + toDate.Month) - ((From.Year * 12) + From.Month) + 1;
                                                            decimal leaveAccrued = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays / 3;
                                                            creditdates = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;

                                                        }
                                                        else
                                                        {

                                                            creditdates = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : ((decimal)leaveDetails.LeaveAccruedNoOfDays);
                                                        }
                                                    }

                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "HalfYearly".ToLower())
                                                {
                                                    var starthalf = GetFinancialYearHalfYearly(From);
                                                    //var endhalf = GetFinancialYearHalfYearly(To);
                                                    totalMonths = 0;
                                                    DateTime toDate = DateTime.Now;
                                                    //bool isCurrentPeriod = false;
                                                    if (leaveDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                                    {
                                                        toDate = (leaveDetails.EffectiveToDate != null && leaveDetails?.EffectiveToDate.Value.Date <= starthalf.ToDate.Date) ? leaveDetails.EffectiveToDate.Value.Date : starthalf.ToDate.Date;
                                                        if (leaveDetails.ProRate == true)
                                                        {
                                                            totalMonths = ((toDate.Year * 12) + toDate.Month) - ((From.Year * 12) + From.Month) + 1;
                                                            decimal leaveAccrued = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays / 6;
                                                            creditdates = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;

                                                        }
                                                        else
                                                        {

                                                            creditdates = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : ((decimal)leaveDetails.LeaveAccruedNoOfDays);
                                                        }
                                                    }


                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "Yearly".ToLower())
                                                {
                                                    var startyear = GetFinancialYearly(From);
                                                    //var endyear = GetFinancialYearly(To);
                                                    DateTime toDate = DateTime.Now;
                                                    //bool isCurrentPeriod = false;
                                                    if (leaveDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                                    {
                                                        toDate = (leaveDetails.EffectiveToDate != null && leaveDetails?.EffectiveToDate.Value.Date <= startyear.ToDate.Date) ? leaveDetails.EffectiveToDate.Value.Date : startyear.ToDate.Date;
                                                        if (leaveDetails.ProRate == true)
                                                        {
                                                            totalMonths = ((toDate.Year * 12) + toDate.Month) - ((From.Year * 12) + From.Month) + 1;
                                                            decimal leaveAccrued = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveDetails.LeaveAccruedNoOfDays / 12;
                                                            creditdates = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;

                                                        }
                                                        else
                                                        {

                                                            creditdates = leaveDetails.LeaveAccruedNoOfDays == null ? 0 : ((decimal)leaveDetails.LeaveAccruedNoOfDays);
                                                        }
                                                    }
                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "One-time".ToLower())
                                                {
                                                    creditdates = leaveDetails?.LeaveAccruedNoOfDays;
                                                }
                                                employeeLeaveDetails = _employeeLeaveDetailsRepository?.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(item.EmployeeID, leaveDetails.LeaveTypeId);

                                                if (employeeLeaveDetails == null)
                                                {
                                                    employeeLeaveDetails = new()
                                                    {
                                                        EmployeeID = item.EmployeeID,
                                                        LeaveTypeID = leaveDetails?.LeaveTypeId,
                                                        BalanceLeave = creditdates,
                                                        CreatedOn = DateTime.UtcNow
                                                    };
                                                    await _employeeLeaveDetailsRepository.AddAsync(employeeLeaveDetails);
                                                    await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                                }
                                                else
                                                {

                                                    employeeLeaveDetails.BalanceLeave += creditdates;
                                                    employeeLeaveDetails.ModifiedOn = DateTime.UtcNow;
                                                    if (employeeLeaveDetails.AdjustmentEffectiveFromDate != null)
                                                    {
                                                        employeeLeaveDetails.AdjustmentBalanceLeave = employeeLeaveDetails.AdjustmentBalanceLeave == null ? null : employeeLeaveDetails.AdjustmentBalanceLeave + creditdates;
                                                    }
                                                    _employeeLeaveDetailsRepository.Update(employeeLeaveDetails);
                                                    await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                                }
                                            }
                                        }
                                        //else
                                        //{
                                        //    List<ApplyLeaves> leavelist = _LeaveApplyRepository.GetAppliedleaveByEmployeeId(item.EmployeeID, leaveDetails.LeaveTypeId);
                                        //    if (leavelist?.Count > 0)
                                        //    {
                                        //        foreach (ApplyLeaves appliedLeaves in leavelist)
                                        //        {
                                        //            if (appliedLeaves.LeaveId > 0)
                                        //            {
                                        //                List<AppliedLeaveDetails> appliedLeaveDetails = _AppliedLeaveDetailsRepository.GetByID(appliedLeaves.LeaveId);
                                        //                foreach (AppliedLeaveDetails appliedLeave in appliedLeaveDetails)
                                        //                {
                                        //                    _AppliedLeaveDetailsRepository.Delete(appliedLeave);
                                        //                    await _AppliedLeaveDetailsRepository.SaveChangesAsync();
                                        //                }
                                        //                ApplyLeaves ApplyLeaves = _LeaveApplyRepository.GetAppliedleaveByIdToDelete(appliedLeaves.LeaveId);
                                        //                if (ApplyLeaves != null && ApplyLeaves.LeaveId > 0)
                                        //                {
                                        //                    _LeaveApplyRepository.Delete(ApplyLeaves);
                                        //                    await _LeaveApplyRepository.SaveChangesAsync();
                                        //                }

                                        //            }
                                        //        }
                                        //    }
                                        //    EmployeeLeaveDetails empLeaveDetail = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(item.EmployeeID, leaveDetails.LeaveTypeId);
                                        //    if (empLeaveDetail != null)
                                        //    {
                                        //        _employeeLeaveDetailsRepository.Delete(empLeaveDetail);
                                        //        await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                        //    }

                                        //}
                                    }
                                    catch (Exception ex)
                                    {
                                        LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/LeaveAccruedOnEffectiveFromDate Emp Id -", Convert.ToString(item.EmployeeID));
                                    }


                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/LeaveAccruedOnEffectiveFromDate, LeaveType Id - ", Convert.ToString(leaveTypesList.LeaveTypeId));
                        }


                    }
                }

            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/LeaveAccruedOnEffectiveFromDate");
            }
            return true;
        }
        #endregion

        #region New Employee Leave
        public async Task<bool> AddNewEmployeeLeave(EmployeeDetailsForLeaveView employeedetails)
        {
            EmployeeDetailsForLeaveView EmployeeDetails = new();
            DateTime TodayDate = DateTime.Now.Date.Date;
            if (employeedetails?.DateOfJoining != null)
            {
                TodayDate = (DateTime)employeedetails.DateOfJoining;
            }

            try
            {
                //LeaveTypes leaveDetails = new();
                List<LeaveTypes> leavetypeslist = new();
                LeaveApplicable leaveApplicable = new();
                List<ProRateMonthDetailsView> proRateMonthDetails = new();
                AppConstants appConstants = new();
                if (employeedetails != null)
                {
                    leavetypeslist = _leaveTypeRepository?.GetAllLeaveType(DateTime.Now.Date);

                    foreach (LeaveTypes leaveTypesList in leavetypeslist)
                    {
                        try
                        {
                            //decimal noofdaycredit = 0;
                            decimal afterroundoff = 0;
                            // Effective From Date. Past date only.
                            if (leaveTypesList.EffectiveFromDate <= DateTime.Now.Date)
                            {
                                appConstants = _appConstantsRepository.GetAppconstantByID(leaveTypesList.LeaveAccruedType);
                                if (leaveTypesList != null)
                                {

                                    switch (employeedetails?.Gender?.ToLower())
                                    {
                                        case "male":
                                            employeedetails.GenderMale = true;
                                            employeedetails.GenderFemale = null;
                                            employeedetails.GenderOther = null;

                                            break;
                                        case "female":
                                            employeedetails.GenderMale = null;
                                            employeedetails.GenderFemale = true;
                                            employeedetails.GenderOther = null;
                                            break;
                                        case "other":
                                            employeedetails.GenderMale = null;
                                            employeedetails.GenderFemale = null;
                                            employeedetails.GenderOther = true;
                                            break;
                                        default:
                                            employeedetails.GenderMale = null;
                                            employeedetails.GenderFemale = null;
                                            employeedetails.GenderOther = null;
                                            break;
                                    }
                                    switch (employeedetails?.MaritalStatus?.ToLower())
                                    {
                                        case "single":
                                            employeedetails.MaritalStatusSingle = true;
                                            employeedetails.MaritalStatusMarried = null;
                                            break;
                                        case "married":
                                            employeedetails.MaritalStatusSingle = null;
                                            employeedetails.MaritalStatusMarried = true;
                                            break;
                                        default:
                                            employeedetails.MaritalStatusSingle = null;
                                            employeedetails.MaritalStatusMarried = null;
                                            break;
                                    }
                                    bool leaveapplicable = false;
                                    bool isEligible = true;
                                    bool isApplicable = false;
                                    leaveApplicable = _leaveApplicableRepository.GetleaveApplicableByLeaveId(leaveTypesList.LeaveTypeId);
                                    var prob = employeedetails.ProbationStatusID == 0 ? null : employeedetails.ProbationStatusID == null ? null : employeedetails.ProbationStatusID;

                                    if ((leaveApplicable?.Gender_Male == true && employeedetails?.GenderMale == true) || (leaveApplicable?.Gender_Female == true && employeedetails?.GenderFemale == true) || (leaveApplicable?.Gender_Others == true && employeedetails?.GenderOther == true))
                                    {
                                        leaveapplicable = true;
                                        isApplicable = true;
                                    }
                                    else if (leaveApplicable?.Gender_Male != true && leaveApplicable?.Gender_Female != true && leaveApplicable?.Gender_Others != true)
                                    {
                                        leaveapplicable = true;
                                    }
                                    else
                                    {
                                        isEligible = false;
                                    }
                                    if ((leaveApplicable?.MaritalStatus_Single == true && employeedetails?.MaritalStatusSingle == true) || (leaveApplicable?.MaritalStatus_Married == true && employeedetails?.MaritalStatusMarried == true))
                                    {
                                        leaveapplicable = true;
                                        isApplicable = true;
                                    }
                                    else if (leaveApplicable?.MaritalStatus_Single != true && leaveApplicable?.MaritalStatus_Married != true)
                                    {
                                        leaveapplicable = true;
                                    }
                                    else
                                    {
                                        isEligible = false;
                                    }
                                    EmployeeLeaveApplicableView applicable = GetLeaveApplicable(leaveTypesList, employeedetails);
                                    var exceptions = GetLeaveExceptions(leaveTypesList, employeedetails);
                                    bool employeeApplicable = false;
                                    List<EmployeeApplicableLeave> employeeApplicableLeaves = _employeeApplicableLeaveRepository.GetEmployeeApplicableLeaveDetailsByLeaveTypeId(leaveTypesList.LeaveTypeId);
                                    if (employeeApplicableLeaves.Any(x => x.EmployeeId != 0) && employeeApplicableLeaves.Any(m => m.EmployeeId == employeedetails.EmployeeID))
                                    {
                                        employeeApplicable = true;
                                    }
                                    bool leaveexception = false;
                                    bool isEligibleException = false;
                                    bool isException = false;
                                    LeaveApplicable leaveException = new();
                                    leaveException = _leaveApplicableRepository.GetleaveExceptionByLeaveId(leaveTypesList.LeaveTypeId);

                                    if ((leaveException?.Gender_Male_Exception == true && employeedetails?.GenderMale == true) || (leaveException?.Gender_Female_Exception == true && employeedetails?.GenderFemale == true) || (leaveException?.Gender_Others_Exception == true && employeedetails?.GenderOther == true))
                                    {
                                        leaveexception = true;
                                        isException = true;
                                    }
                                    else if (leaveException?.Gender_Male_Exception != true && leaveException?.Gender_Female_Exception != true && leaveException?.Gender_Others_Exception != true)
                                    {
                                        leaveexception = false;
                                    }
                                    else
                                    {
                                        isEligibleException = false;
                                    }
                                    if ((leaveException?.MaritalStatus_Single_Exception == true && employeedetails?.MaritalStatusSingle == true) || (leaveException?.MaritalStatus_Married_Exception == true && employeedetails?.MaritalStatusMarried == true))
                                    {
                                        leaveexception = true;
                                        isException = true;
                                    }
                                    else if (leaveException?.MaritalStatus_Single_Exception != true && leaveException?.MaritalStatus_Married_Exception != true)
                                    {
                                        leaveexception = false;
                                    }
                                    else
                                    {
                                        isEligibleException = false;
                                    }
                                    //employee exception
                                    bool employeeException = false;
                                    List<EmployeeApplicableLeave> employeeExceptionLeaves = _employeeApplicableLeaveRepository.GetEmployeeExceptionLeaveDetailsByLeaveTypeId(leaveTypesList.LeaveTypeId);
                                    if (employeeExceptionLeaves.Any(x => x.LeaveExceptionEmployeeId != 0) && employeeExceptionLeaves.Any(m => m.LeaveExceptionEmployeeId == employeedetails.EmployeeID))
                                    {
                                        employeeException = true;
                                    }
                                    EmployeeLeaveDetails employeeLeaveDetails = new();

                                    //if (leaveapplicable && applicable && exceptions == false)
                                    if ((isEligibleException == false && leaveexception == false && exceptions == false && isException == false) && (employeeException == false))
                                    {
                                        if ((isEligible && leaveapplicable && applicable.IsApplicable && (isApplicable || applicable.IsCreteriaMached)) || (employeeApplicable))
                                        {
                                            // leave credit only for  Balance Based On 
                                            AppConstants appConstantBasedOn = new();
                                            LeaveTypes leaveTypeDetails = new();
                                            leaveTypeDetails = _leaveTypeRepository?.GetByID(leaveTypesList == null ? 0 : leaveTypesList.LeaveTypeId);
                                            appConstantBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);
                                            if (appConstantBasedOn?.AppConstantValue?.ToLower() != "LeaveGrant".ToLower())
                                            {
                                                //EmployeeLeaveDetails empDetail = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(employeedetails.EmployeeID, leaveTypesList == null ? 0 : leaveTypesList.LeaveTypeId);

                                                // total Months calculation                            
                                                DateTime From = (employeedetails.DateOfJoining != null && leaveTypeDetails?.EffectiveFromDate <= employeedetails.DateOfJoining) ? (DateTime)employeedetails.DateOfJoining : (DateTime)leaveTypeDetails.EffectiveFromDate; //Convert.ToDateTime("10/25/2021");
                                                DateTime To = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails?.EffectiveToDate.Value.Date <= DateTime.Now.Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : DateTime.Now.Date;

                                                // Get Total month count
                                                //TotalMonths = NoofMonths + 1;
                                                decimal? noofleavecredit = 0;
                                                int totalMonths = 0;

                                                if (appConstants.DisplayName.ToLower() == "Monthly".ToLower())
                                                {
                                                    proRateMonthDetails = _proRateMonthDetailsRepository.GetProRateMonthDetailsByID(leaveTypesList == null ? 0 : leaveTypesList.LeaveTypeId);
                                                    if (!string.IsNullOrEmpty(leaveTypeDetails.LeaveAccruedDay))
                                                    {
                                                        string leaveDay = "";
                                                        if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays")
                                                        {
                                                            leaveDay = "1";
                                                        }
                                                        else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays")
                                                        {

                                                            leaveDay = Convert.ToString(DateTime.DaysInMonth(To.Year, To.Month));
                                                        }
                                                        else
                                                        {
                                                            leaveDay = leaveTypeDetails?.LeaveAccruedDay;
                                                        }
                                                        if (leaveDay != "" && Convert.ToInt32(leaveDay) <= DateTime.Now.Day)
                                                        {
                                                            totalMonths = ((To.Year * 12) + To.Month) - ((From.Year * 12) + From.Month) + 1;
                                                        }
                                                        else
                                                        {
                                                            totalMonths = ((To.Year * 12) + To.Month) - ((From.Year * 12) + From.Month);
                                                            DateTime previousMonth = new DateTime(To.Year, To.Month, 1);
                                                            To = previousMonth.AddDays(-1);

                                                        }
                                                    }
                                                    else
                                                    {
                                                        totalMonths = ((To.Year * 12) + To.Month) - ((From.Year * 12) + From.Month) + 1;
                                                    }

                                                    if (leaveTypeDetails.ProRate == true)
                                                    {

                                                        decimal? prorateleave = 0;
                                                        for (DateTime dt = From; dt <= To; dt = dt.AddMonths(1))
                                                        {
                                                            var MonthStart = new DateTime(dt.Year, dt.Month, 1);
                                                            var MonthLast = MonthStart.AddMonths(1).AddDays(-1);
                                                            string ProrateFromday = null;
                                                            string ProrateToday = null;
                                                            //decimal proratevalue = default;
                                                            prorateleave = 0;
                                                            foreach (ProRateMonthDetailsView ProRateitem in proRateMonthDetails)
                                                            {
                                                                if (ProRateitem?.Fromday?.ToLower() == "firstdays".ToLower())
                                                                {
                                                                    ProrateFromday = "1";
                                                                }
                                                                else if (ProRateitem?.Fromday?.ToLower() == "lastdays".ToLower())
                                                                {
                                                                    ProrateFromday = Convert.ToString(MonthLast.Day);
                                                                }
                                                                else
                                                                {
                                                                    ProrateFromday = ProRateitem?.Fromday;
                                                                }
                                                                if (ProRateitem?.Today?.ToLower() == "firstdays".ToLower())
                                                                {
                                                                    ProrateToday = "1";
                                                                }
                                                                else if (ProRateitem?.Today?.ToLower() == "lastdays".ToLower())
                                                                {
                                                                    ProrateToday = Convert.ToString(MonthLast.Day);
                                                                }
                                                                else
                                                                {
                                                                    ProrateToday = ProRateitem?.Today;
                                                                }
                                                                //fromdate                                                
                                                                var fromDate = new DateTime(dt.Year, dt.Month, Convert.ToInt32(ProrateFromday));
                                                                //todate
                                                                var toDate = new DateTime(dt.Year, dt.Month, Convert.ToInt32(ProrateToday));
                                                                if ((fromDate <= From.Date && From.Date <= toDate))
                                                                {
                                                                    prorateleave = (decimal)ProRateitem.Count;
                                                                    break;
                                                                }
                                                                //proratevalue = (decimal)ProRateitem.Count;
                                                            }
                                                            if(totalMonths>1)
                                                            {
                                                                noofleavecredit = (leaveTypeDetails.LeaveAccruedNoOfDays * (totalMonths-1))+ prorateleave;

                                                            }
                                                            else
                                                            {
                                                                noofleavecredit = prorateleave;
                                                            }
                                                            

                                                        }
                                                    }
                                                    else
                                                    {
                                                        noofleavecredit = leaveTypeDetails.LeaveAccruedNoOfDays * totalMonths;
                                                    }


                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "Quarterly".ToLower())
                                                {
                                                    FinancialYearDateView startqtr = GetFinancialYearQuarter(From);
                                                    FinancialYearDateView endqtr = GetFinancialYearQuarter(To);
                                                    totalMonths = 0;
                                                    bool isCurrentPeriod = false;
                                                    if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                                    {
                                                        isCurrentPeriod = true;
                                                    }
                                                    else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                                                    {
                                                        if (DateTime.Now.Date == endqtr.ToDate.Date)
                                                        {
                                                            isCurrentPeriod = true;
                                                        }
                                                    }
                                                    DateTime toDate = DateTime.Now;
                                                    if (isCurrentPeriod)
                                                    {
                                                        toDate = (leaveTypeDetails?.EffectiveToDate != null && leaveTypeDetails?.EffectiveToDate.Value.Date <= endqtr.ToDate.Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endqtr.ToDate.Date;

                                                    }
                                                    else
                                                    {
                                                        toDate = (leaveTypeDetails?.EffectiveToDate != null && leaveTypeDetails?.EffectiveToDate.Value.Date <= endqtr.FromDate.Date.AddDays(-1).Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endqtr.FromDate.Date.AddDays(-1).Date;
                                                        //totalMonths = ((noOfMonth.Year * 12) + noOfMonth.Month) - ((From.Year * 12) + From.Month) + 1;
                                                    }
                                                    if (leaveTypeDetails.ProRate == true)
                                                    {
                                                        totalMonths = ((toDate.Year * 12) + toDate.Month) - ((From.Year * 12) + From.Month) + 1;
                                                        decimal leaveAccrued = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays / 3;

                                                        noofleavecredit = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;

                                                    }
                                                    else
                                                    {
                                                        int duration = 0;
                                                        for (DateTime date = startqtr.FromDate.Date; date <= toDate; date = date.AddMonths(3))
                                                        {
                                                            duration = duration + 1;
                                                        }
                                                        noofleavecredit = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : ((decimal)leaveTypeDetails.LeaveAccruedNoOfDays * duration);
                                                    }


                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "HalfYearly".ToLower())
                                                {
                                                    var starthalf = GetFinancialYearHalfYearly(From);
                                                    var endhalf = GetFinancialYearHalfYearly(To);
                                                    totalMonths = 0;
                                                    bool isCurrentPeriod = false;
                                                    if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                                    {
                                                        isCurrentPeriod = true;
                                                    }
                                                    else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                                                    {
                                                        if (DateTime.Now.Date == endhalf.ToDate.Date)
                                                        {
                                                            isCurrentPeriod = true;
                                                        }
                                                    }
                                                    DateTime toDate = DateTime.Now;
                                                    if (isCurrentPeriod)
                                                    {
                                                        toDate = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails.EffectiveToDate.Value.Date <= endhalf.ToDate.Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endhalf.ToDate.Date;
                                                        //totalMonths = ((noOfMonth.Year * 12) + noOfMonth.Month) - ((From.Year * 12) + From.Month) + 1;
                                                    }
                                                    else
                                                    {
                                                        toDate = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails.EffectiveToDate.Value.Date <= endhalf.FromDate.Date.AddDays(-1).Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endhalf.FromDate.Date.AddDays(-1).Date;

                                                    }
                                                    if (leaveTypeDetails.ProRate == true)
                                                    {
                                                        totalMonths = ((toDate.Year * 12) + toDate.Month) - ((From.Year * 12) + From.Month) + 1;
                                                        decimal leaveAccrued = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays / 6;
                                                        noofleavecredit = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;
                                                    }
                                                    else
                                                    {
                                                        int duration = 0;
                                                        for (DateTime date = starthalf.FromDate.Date; date <= toDate; date = date.AddMonths(6))
                                                        {
                                                            duration = duration + 1;
                                                        }
                                                        noofleavecredit = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : ((decimal)leaveTypeDetails.LeaveAccruedNoOfDays * duration);
                                                    }


                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "Yearly".ToLower())
                                                {
                                                    var startyear = GetFinancialYearly(From);
                                                    var endyear = GetFinancialYearly(To);
                                                    bool isCurrentPeriod = false;
                                                    totalMonths = 0;
                                                    if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                                                    {
                                                        isCurrentPeriod = true;
                                                    }
                                                    else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                                                    {
                                                        if (DateTime.Now.Date == endyear.ToDate.Date)
                                                        {
                                                            isCurrentPeriod = true;
                                                        }
                                                    }
                                                    DateTime toDate = DateTime.Now;
                                                    if (isCurrentPeriod)
                                                    {
                                                        toDate = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails.EffectiveToDate.Value.Date <= endyear.ToDate.Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endyear.ToDate.Date;
                                                        //totalMonths = ((noOfMonth.Year * 12) + noOfMonth.Month) - ((From.Year * 12) + From.Month) + 1;
                                                    }
                                                    else
                                                    {
                                                        toDate = (leaveTypeDetails.EffectiveToDate != null && leaveTypeDetails.EffectiveToDate.Value.Date <= endyear.FromDate.Date.AddDays(-1).Date) ? leaveTypeDetails.EffectiveToDate.Value.Date : endyear.FromDate.Date.AddDays(-1).Date;

                                                    }
                                                    if (leaveTypeDetails.ProRate == true)
                                                    {
                                                        totalMonths = ((toDate.Year * 12) + toDate.Month) - ((From.Year * 12) + From.Month) + 1;
                                                        decimal leaveAccrued = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays / 12;
                                                        noofleavecredit = totalMonths > 0 ? (totalMonths * leaveAccrued) : 0;
                                                    }
                                                    else
                                                    {
                                                        int duration = 0;
                                                        for (DateTime date = startyear.FromDate.Date; date <= toDate; date = date.AddMonths(12))
                                                        {
                                                            duration = duration + 1;
                                                        }
                                                        noofleavecredit = leaveTypeDetails.LeaveAccruedNoOfDays == null ? 0 : ((decimal)leaveTypeDetails.LeaveAccruedNoOfDays * duration);
                                                    }
                                                }
                                                else if (appConstants?.DisplayName?.ToLower() == "One-time".ToLower())
                                                {
                                                    noofleavecredit = leaveTypeDetails?.LeaveAccruedNoOfDays;
                                                }
                                                afterroundoff = GetRoundOff((decimal)noofleavecredit);



                                            }
                                            else
                                            {
                                                afterroundoff = 0;
                                            }

                                            employeeLeaveDetails = _employeeLeaveDetailsRepository?.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(employeedetails.EmployeeID, leaveTypesList.LeaveTypeId);

                                            if (employeeLeaveDetails == null)
                                            {
                                                employeeLeaveDetails = new()
                                                {
                                                    EmployeeID = employeedetails?.EmployeeID,
                                                    LeaveTypeID = leaveTypesList?.LeaveTypeId,
                                                    BalanceLeave = afterroundoff,
                                                    CreatedOn = DateTime.UtcNow
                                                };
                                                await _employeeLeaveDetailsRepository.AddAsync(employeeLeaveDetails);
                                                await _employeeLeaveDetailsRepository.SaveChangesAsync();
                                            }

                                        }
                                    }

                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            LoggerManager.LoggingErrorTrack(ex, "Leaves", "Leaves/AddNewEmployeeLeave", Convert.ToString(leaveTypesList.LeaveTypeId));
                        }

                    }
                }

            }

            catch (Exception ex)
            {

                throw;
            }
            return true;

        }
        #endregion
        #region Get GetRound Off Values
        public static decimal GetRoundOffValues(decimal count)
        {
            decimal sd = default;
            string s = default;
            if (count != null && count != 0)
            {
                //var decimalValues = (decimal)1.5112548;
                decimal decimalValue = decimal.Round(count, 2, MidpointRounding.AwayFromZero);
                var split = decimalValue.ToString(CultureInfo.InvariantCulture).Split('.');

                char x = split[0].First();
                bool neg = default;

                if (x == '-')
                {
                    var value = decimalValue.ToString(CultureInfo.InvariantCulture).Split('-');
                    neg = true;
                    split = value[1].Split('.');
                    var d = Convert.ToDecimal("0." + (split.Length > 1 ? split[1] : 0));

                    if (d is >= (decimal)0.0 and <= (decimal)0.24)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.0);

                    }
                    else if (d is >= (decimal)0.25 and <= (decimal)0.49)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.25);

                    }
                    else if (d is >= (decimal)0.50 and <= (decimal)0.74)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.50);

                    }
                    else if (d is >= (decimal)0.75 and <= (decimal)0.99)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.75);

                    }
                    if (neg == true)
                    {
                        s = sd.ToString();
                        s = s.Insert(0, "-");
                        sd = decimal.Parse(s);
                    }
                }
                else
                {
                    var de = Convert.ToDecimal("0." + (split.Length > 1 ? split[1] : 0));

                    if (de is >= (decimal)0.0 and <= (decimal)0.24)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.0);

                    }
                    else if (de is >= (decimal)0.25 and <= (decimal)0.49)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.25);

                    }
                    else if (de is >= (decimal)0.50 and <= (decimal)0.74)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.50);

                    }
                    else if (de is >= (decimal)0.75 and <= (decimal)0.99)
                    {
                        sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.75);

                    }
                }
            }
            else
            {
                sd = 0;
            }

            return sd;
        }
        #endregion
        #region Get Leave By Employee Id
        public List<AppliedLeaveTypeDetails> GetLeavesByEmployeeId(WeekMonthAttendanceView weekMonthAttendanceView)
        {
            return _leaveRepository.GetEmployeeLeaveDetails(weekMonthAttendanceView.EmployeeId, weekMonthAttendanceView.FromDate, weekMonthAttendanceView.ToDate);
        }
        #endregion
        #region Get Appconstant List
        public List<AppConstants> GetAppconstantsList()
        {
            return _appConstantsRepository.GetAppconstantsList();
        }
        #endregion 
        #region Check applied leave date
        public bool checkAppliedLeaveByDate(int employeeId, int leaveTypeId, DateTime leaveDate, bool isFullday, bool isFirstHalf, bool isSecondHalf)
        {
            return _leaveRepository.checkAppliedLeaveByDate(employeeId, leaveTypeId, leaveDate, isFullday, isFirstHalf, isSecondHalf);
        }
        #endregion
        public bool checkMinimumGap(ApplyLeavesView ApplyLeavesView, decimal minimumGap)
        {
            int days = (int)minimumGap;
            decimal isHalfDay = minimumGap % 1;
            decimal halfDay = isHalfDay >= (decimal)0.5 && isHalfDay <= (decimal)0.99 ? (decimal)0.5 : 0;
            int daysToCheck = days;

            //decimal MaximumGap = isHalfDay > 0 ? days + (decimal)0.5 : days;
            DateTime startDate = DateTime.Now;
            bool isFirstdayHalf = false;

            //Check foreward
            if (ApplyLeavesView.AppliedLeaveDetails.Count > 0)
            {
                isFirstdayHalf = ApplyLeavesView?.AppliedLeaveDetails[ApplyLeavesView.AppliedLeaveDetails.Count - 1].IsFirstHalf == null ? false :
                   (bool)ApplyLeavesView?.AppliedLeaveDetails[ApplyLeavesView.AppliedLeaveDetails.Count - 1].IsFirstHalf;

                startDate = (DateTime)ApplyLeavesView?.AppliedLeaveDetails[ApplyLeavesView.AppliedLeaveDetails.Count - 1].Date;
                if (isFirstdayHalf)
                {
                    startDate = (DateTime)ApplyLeavesView?.AppliedLeaveDetails[ApplyLeavesView.AppliedLeaveDetails.Count - 1].Date.AddDays(-1);
                }

            }
            if (halfDay > 0 || isFirstdayHalf == true)
            {
                daysToCheck = daysToCheck + 1;
            }
            for (int i = 0; i < daysToCheck; i++)
            {

                if (isFirstdayHalf && i == 0)
                {
                    startDate = startDate.AddDays(1);
                    if (_leaveRepository.checkAppliedLeaveByLeaveId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId,
                        startDate, false, false, true, ApplyLeavesView.LeaveId))
                    {
                        return false;
                    }
                }
                else
                {
                    bool isNotHoliday = true;

                    while (isNotHoliday == true)
                    {
                        startDate = startDate.AddDays(1);
                        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(ApplyLeavesView.DepartmentId, startDate, ApplyLeavesView.LocationId, ApplyLeavesView.ShiftId);
                        isNotHoliday = holidayDate == null ? false : true;
                        if (isNotHoliday == false)
                        {
                            WeekendViewDefinition isweekend = ApplyLeavesView?.WeekendList?.Where(x => x.WeekendDayName.Contains(startDate.DayOfWeek.ToString())).FirstOrDefault();
                            isNotHoliday = isweekend == null ? false : true;
                        }

                    }
                    if (i == daysToCheck - 1)
                    {
                        if ((halfDay > 0 && isFirstdayHalf == false) || (halfDay == 0 && isFirstdayHalf == true))
                        {

                            if (_leaveRepository.checkAppliedLeaveByLeaveId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId,
                            startDate, false, true, false, ApplyLeavesView.LeaveId))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (_leaveRepository.checkAppliedLeaveByLeaveId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId,
                            startDate, true, false, false, ApplyLeavesView.LeaveId))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (_leaveRepository.checkAppliedLeaveByLeaveId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId,
                        startDate, true, false, false, ApplyLeavesView.LeaveId))
                        {
                            return false;
                        }
                    }
                }
            }

            //Check backward
            bool isSeconddayHalf = false;
            if (ApplyLeavesView.AppliedLeaveDetails.Count > 0)
            {
                isSeconddayHalf = ApplyLeavesView?.AppliedLeaveDetails[0].IsSecondHalf == null ? false :
                   (bool)ApplyLeavesView?.AppliedLeaveDetails[0].IsSecondHalf;
                startDate = (DateTime)ApplyLeavesView?.AppliedLeaveDetails[0].Date;
                if (isSeconddayHalf)
                {
                    startDate = (DateTime)ApplyLeavesView?.AppliedLeaveDetails[0].Date.AddDays(1);
                }
            }
            daysToCheck = days;
            if (halfDay > 0 || isSeconddayHalf == true)
            {
                daysToCheck = daysToCheck + 1;
            }
            for (int i = 0; i < daysToCheck; i++)
            {
                if (isSeconddayHalf && i == 0)
                {
                    startDate = startDate.AddDays(-1);

                    if (_leaveRepository.checkAppliedLeaveByLeaveId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId,
                        startDate, false, true, false, ApplyLeavesView.LeaveId))
                    {
                        return false;
                    }
                }
                else
                {
                    bool isNotHoliday = true;
                    while (isNotHoliday == true)
                    {
                        startDate = startDate.AddDays(-1);
                        Holiday holidayDate = _holidayRepository.CheckHolidayByDate(ApplyLeavesView.DepartmentId, startDate, ApplyLeavesView.LocationId, ApplyLeavesView.ShiftId);
                        isNotHoliday = holidayDate == null ? false : true;
                        if (isNotHoliday == false)
                        {
                            WeekendViewDefinition isweekend = ApplyLeavesView?.WeekendList?.Where(x => x.WeekendDayName.Contains(startDate.DayOfWeek.ToString())).FirstOrDefault();
                            isNotHoliday = isweekend == null ? false : true;
                        }

                    }
                    if (i == daysToCheck - 1)
                    {
                        if ((halfDay > 0 && isSeconddayHalf == false) || (halfDay == 0 && isSeconddayHalf == true))
                        {
                            if (_leaveRepository.checkAppliedLeaveByLeaveId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId,
                        startDate, false, false, true, ApplyLeavesView.LeaveId))
                            {
                                return false;
                            }
                        }
                        else
                        {
                            if (_leaveRepository.checkAppliedLeaveByLeaveId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId,
                        startDate, true, false, false, ApplyLeavesView.LeaveId))
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        if (_leaveRepository.checkAppliedLeaveByLeaveId(ApplyLeavesView.EmployeeId, ApplyLeavesView.LeaveTypeId,
                        startDate, true, false, false, ApplyLeavesView.LeaveId))
                        {
                            return false;
                        }
                    }
                }
            }
            return true;
        }

        #region Get GetRound Off Values
        public static decimal GetHalfDayValues(decimal count)
        {
            decimal sd = default;
            if (count != null && count != 0)
            {
                //var decimalValues = (decimal)1.5112548;
                decimal decimalValue = decimal.Round(count, 2, MidpointRounding.AwayFromZero);
                var split = decimalValue.ToString(CultureInfo.InvariantCulture).Split('.');

                var de = Convert.ToDecimal("0." + (split.Length > 1 ? split[1] : 0));
                if (de is >= (decimal)0.5 and <= (decimal)0.99)
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.5);
                }
                else
                {
                    sd = Convert.ToDecimal(split.Length > 0 ? split[0] : 0) + Convert.ToDecimal(.0);
                }
            }
            else
            {
                sd = 0;
            }
            return sd;
        }
        #endregion
        #region Get reset date
        public decimal GetLeaveAdjustmentDays(string period, DateTime? resetToDate, List<LeaveAdjustmentDetails> leaveAdjustmentDetails)
        {
            DateTime resetFromDate = default;
            decimal adjustmentDays = 0;
            if (resetToDate != null)
            {
                if (period?.ToLower() == "monthly".ToLower())
                {
                    resetFromDate = resetToDate.Value.AddMonths(-1);
                }
                else if (period?.ToLower() == "quarterly".ToLower())
                {
                    resetFromDate = resetToDate.Value.AddMonths(-3);
                }
                else if (period?.ToLower() == "halfYearly".ToLower())
                {
                    resetFromDate = resetToDate.Value.AddMonths(-6);
                }
                else if (period?.ToLower() == "yearly".ToLower())
                {
                    resetFromDate = resetToDate.Value.AddMonths(-12);
                }
                if (leaveAdjustmentDetails?.Count > 0)
                {
                    adjustmentDays = leaveAdjustmentDetails.Where(x => x.EffectiveFromDate >= resetFromDate && x.EffectiveFromDate <= resetToDate).Sum(x => x.NoOfDays == null ? 0 : (decimal)x.NoOfDays);
                }
            }
            return adjustmentDays;
        }
        #endregion
        #region Get leave history by leave id
        public List<LeaveHistoryView> GetLeaveHistoryByLeaveType(LeaveHistoryModel model)
        {
            List<LeaveHistoryView> leaveHistory = new List<LeaveHistoryView>();
            List<AppliedLeaveTypeDetails> appliedLeaveDetails = _leaveRepository.GetAppliedLeaveByLeaveTypeAndDate(model.EmployeeId, model.FromDate, model.ToDate, model.LeaveTypeId);
            List<LeaveAdjustmentDetails> leaveAdjustmentDetails = _leaveAdjustmentDetailsRepository.GetLeaveAdjustmentDetailsByDate(model.EmployeeId, model.LeaveTypeId, model.FromDate, model.ToDate);
            LeaveCarryForwardListView leaveCarryForwardDetails = _leaveEntitlementRepository.GetLeaveTypeCarryForwardDetails(model.LeaveTypeId);
            EmployeeLeaveDetails leaveDetails = _employeeLeaveDetailsRepository.GetEmployeeLeaveDetailByEmployeeIDandLeaveID(model.EmployeeId, model.LeaveTypeId);
            List<AppliedLeaveTypeDetails> allAppliedLeaveDetails = _leaveRepository.GetAllAppliedLeaveByLeaveTypeAndDate(model.EmployeeId, model.FromDate, model.ToDate, model.LeaveTypeId);
            List<LeaveGrantRequestDetails> grantRequestDetails = new List<LeaveGrantRequestDetails>();
            List<LeaveGrantRequestDetails> allGrantRequestDetails = _leaveGrantRequestDetailsRepository.GetGrantRequestDetailsByLeaveTypeId(model.LeaveTypeId, model.EmployeeId, model.ToDate, 0);
            decimal fromDateBalance = 0;
            if (leaveDetails.AdjustmentEffectiveFromDate != null)
            {
                fromDateBalance = leaveDetails?.AdjustmentBalanceLeave == null ? 0 : (decimal)leaveDetails?.AdjustmentBalanceLeave;
            }
            else
            {
                fromDateBalance = leaveDetails?.BalanceLeave == null ? 0 : (decimal)leaveDetails.BalanceLeave;
            }
            DateTime resetDate = GetResetDate(model.FromDate, model.LeaveTypeId, leaveCarryForwardDetails);
            LeaveTypes leaveTypeDetails = _leaveTypeRepository?.GetByID(model.LeaveTypeId);
            AppConstants appConstantBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);
            if (appConstantBasedOn?.AppConstantValue?.ToLower() == "LeaveGrant".ToLower())
            {
                grantRequestDetails = _leaveGrantRequestDetailsRepository.GetGrantLeaveListByEmployeeID(model.LeaveTypeId, model.EmployeeId, model.FromDate, model.ToDate);
            }

            if (model.FromDate.Date <= DateTime.Now.Date)
            {
                for (DateTime day = DateTime.Now.Date; model.FromDate.Date.AddDays(-1) <= day; day = day.AddDays(-1))
                {
                    //Check leave accured

                    if (appConstantBasedOn?.AppConstantValue?.ToLower() != "LeaveGrant".ToLower())
                    {
                        FinancialYearDateView periodDate = new FinancialYearDateView();

                        //Prorate 
                        AppConstants leaveAccuredType = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.LeaveAccruedType);
                        if (leaveAccuredType?.AppConstantValue?.ToLower() == "Monthly".ToLower())
                        {
                            string leaveDay = "";
                            if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays")
                            {
                                leaveDay = "1";
                            }
                            else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays")
                            {

                                leaveDay = Convert.ToString(DateTime.DaysInMonth(day.Year, day.Month));
                            }
                            else
                            {
                                leaveDay = leaveTypeDetails?.LeaveAccruedDay;
                            }
                            if (Convert.ToInt32(leaveDay) == day.Date.Day)
                            {
                                fromDateBalance = fromDateBalance - (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                            }
                        }
                        else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower())
                        {
                            periodDate = GetFinancialYearQuarter(day);
                        }
                        else if (leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower())
                        {
                            periodDate = GetFinancialYearHalfYearly(day);
                        }
                        else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                        {
                            periodDate = GetFinancialYearly(day);
                        }
                        if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower() ||
                            leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower() ||
                            leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                        {
                            if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                            {
                                if (periodDate.FromDate.Date == day.Date)
                                {
                                    fromDateBalance = fromDateBalance - (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                                }
                            }
                            else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                            {
                                if (periodDate.ToDate.Date == day.Date)
                                {
                                    fromDateBalance = fromDateBalance - (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                                }
                            }
                        }
                    }

                    //Check Applied leave dates
                    List<AppliedLeaveTypeDetails> appliedLeave = appliedLeaveDetails.Where(x => x.Date == day.Date).Select(x => x).ToList();
                    if (appliedLeave?.Count > 0)
                    {
                        foreach (var item in appliedLeave)
                        {
                            if (item.IsFullDay)
                            {
                                fromDateBalance = fromDateBalance + 1;
                            }
                            else
                            {
                                fromDateBalance = fromDateBalance + (decimal)0.5;
                            }
                        }
                    }

                    //Check leave Adjustment
                    LeaveAdjustmentDetails adjustment = leaveAdjustmentDetails.Where(x => x.EffectiveFromDate.Value.AddDays(-1).Date == day.Date).OrderByDescending(x => x.CreatedOn).Select(x => x).FirstOrDefault();
                    if (adjustment != null)
                    {
                        decimal val = (adjustment?.NoOfDays == null ? 0 : (decimal)adjustment.NoOfDays);
                        fromDateBalance = fromDateBalance - val;

                    }
                    LeaveAdjustmentDetails adjustmentDetail = leaveAdjustmentDetails.Where(x => x.EffectiveFromDate.Value.Date == day.Date).OrderByDescending(x => x.CreatedOn).Select(x => x).FirstOrDefault();
                    if (adjustmentDetail != null)
                    {
                        fromDateBalance = adjustmentDetail?.AdjustmentBalance == null ? 0 : (decimal)adjustmentDetail.AdjustmentBalance;
                    }


                    //Check Carryforward details
                    if (day.Date == resetDate.Date)
                    {
                        LeaveCarryForward carryForward = _leaveCarryForwardRepository.GetLeaveCarryForwardByLeaveTypeId(model.EmployeeId, model.LeaveTypeId, resetDate.Date);
                        if (carryForward != null)
                        {
                            if (carryForward.AdjustmentEffectiveFromDate != null)
                            {
                                fromDateBalance = carryForward?.AdjustmentBalanceLeave == null ? 0 : (int)carryForward.AdjustmentBalanceLeave;
                            }
                            else
                            {
                                fromDateBalance = carryForward.BalanceLeave;
                            }

                        }
                        //if (leaveCarryForwardDetails?.CarryForwardName?.ToLower() == "NoOfDays".ToLower())
                        //{
                        //    fromDateBalance = fromDateBalance >= leaveCarryForwardDetails?.MaximumCarryForwardDays ? (leaveCarryForwardDetails?.MaximumCarryForwardDays==null?0 : (decimal)leaveCarryForwardDetails.MaximumCarryForwardDays) : fromDateBalance;
                        //}                        
                        //else if (leaveCarryForwardDetails?.CarryForwardName?.ToLower() == "None".ToLower())
                        //{
                        //    fromDateBalance = 0;
                        //}

                        //if (leaveCarryForwardDetails?.Period?.ToLower() == "monthly".ToLower())
                        //{
                        //    resetDate = resetDate.AddMonths(-1);
                        //}
                        //else if (leaveCarryForwardDetails?.Period?.ToLower() == "quarterly".ToLower())
                        //{
                        //    resetDate = resetDate.AddMonths(-3);
                        //}
                        //else if (leaveCarryForwardDetails?.Period?.ToLower() == "halfyearly".ToLower())
                        //{
                        //    resetDate = resetDate.AddMonths(-6);
                        //}
                        //else if (leaveCarryForwardDetails?.Period?.ToLower() == "yearly".ToLower())
                        //{
                        //    resetDate = resetDate.AddMonths(-12);
                        //}
                    }
                }
            }
            else
            {
                for (DateTime day = DateTime.Now.Date; model.FromDate.Date > day; day = day.AddDays(1))
                {
                    //Check leave accured
                    //AppConstants appConstantBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);
                    if (appConstantBasedOn?.AppConstantValue?.ToLower() != "LeaveGrant".ToLower())
                    {
                        FinancialYearDateView periodDate = new FinancialYearDateView();

                        //Prorate 
                        AppConstants leaveAccuredType = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.LeaveAccruedType);
                        if (leaveAccuredType?.AppConstantValue?.ToLower() == "Monthly".ToLower())
                        {
                            string leaveDay = "";
                            if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays")
                            {
                                leaveDay = "1";
                            }
                            else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays")
                            {

                                leaveDay = Convert.ToString(DateTime.DaysInMonth(day.Year, day.Month));
                            }
                            else
                            {
                                leaveDay = leaveTypeDetails?.LeaveAccruedDay;
                            }
                            if (Convert.ToInt32(leaveDay) == day.Date.Day)
                            {
                                fromDateBalance = fromDateBalance + (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                            }
                        }
                        else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower())
                        {
                            periodDate = GetFinancialYearQuarter(day);
                        }
                        else if (leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower())
                        {
                            periodDate = GetFinancialYearHalfYearly(day);
                        }
                        else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                        {
                            periodDate = GetFinancialYearly(day);
                        }
                        if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower() ||
                            leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower() ||
                            leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                        {
                            if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                            {
                                if (periodDate.FromDate.Date == day.Date)
                                {
                                    fromDateBalance = fromDateBalance + (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                                }
                            }
                            else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                            {
                                if (periodDate.ToDate.Date == day.Date)
                                {
                                    fromDateBalance = fromDateBalance + (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                                }
                            }
                        }
                    }



                    //Check Applied leave dates
                    List<AppliedLeaveTypeDetails> appliedLeave = appliedLeaveDetails.Where(x => x.Date == day.Date).Select(x => x).ToList();
                    if (appliedLeave?.Count > 0)
                    {
                        foreach (var item in appliedLeave)
                        {
                            if (item.IsFullDay)
                            {
                                fromDateBalance = fromDateBalance - 1;
                            }
                            else
                            {
                                fromDateBalance = fromDateBalance - (decimal)0.5;
                            }
                        }
                    }

                    //Check leave Adjustment
                    LeaveAdjustmentDetails adjustmentDetail = leaveAdjustmentDetails.Where(x => x.EffectiveFromDate.Value.Date == day.Date).OrderByDescending(x => x.CreatedOn).Select(x => x).FirstOrDefault();
                    if (adjustmentDetail != null)
                    {
                        fromDateBalance = adjustmentDetail?.AdjustmentBalance == null ? 0 : (decimal)adjustmentDetail.AdjustmentBalance;
                    }

                    //Check Carryforward details
                    if (day.Date == resetDate.Date)
                    {
                        if (leaveCarryForwardDetails?.CarryForwardName?.ToLower() == "NoOfDays".ToLower())
                        {
                            fromDateBalance = fromDateBalance >= leaveCarryForwardDetails?.MaximumCarryForwardDays ? (leaveCarryForwardDetails?.MaximumCarryForwardDays == null ? 0 : (decimal)leaveCarryForwardDetails.MaximumCarryForwardDays) : fromDateBalance;
                        }
                        else if (leaveCarryForwardDetails?.CarryForwardName?.ToLower() == "None".ToLower())
                        {
                            fromDateBalance = 0;
                        }

                        if (leaveCarryForwardDetails?.Period?.ToLower() == "monthly".ToLower())
                        {
                            resetDate = resetDate.AddMonths(1);
                        }
                        else if (leaveCarryForwardDetails?.Period?.ToLower() == "quarterly".ToLower())
                        {
                            resetDate = resetDate.AddMonths(3);
                        }
                        else if (leaveCarryForwardDetails?.Period?.ToLower() == "halfyearly".ToLower())
                        {
                            resetDate = resetDate.AddMonths(6);
                        }
                        else if (leaveCarryForwardDetails?.Period?.ToLower() == "yearly".ToLower())
                        {
                            resetDate = resetDate.AddMonths(12);
                        }
                    }
                }
            }

            //Get leave history details

            for (DateTime day = model.FromDate.Date; model.ToDate.Date >= day; day = day.AddDays(1))
            {
                //Check leave accured
                //AppConstants appConstantBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);
                if (appConstantBasedOn?.AppConstantValue?.ToLower() != "LeaveGrant".ToLower())
                {
                    FinancialYearDateView periodDate = new FinancialYearDateView();

                    //Prorate 
                    AppConstants leaveAccuredType = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.LeaveAccruedType);
                    if (leaveAccuredType?.AppConstantValue?.ToLower() == "Monthly".ToLower())
                    {
                        string leaveDay = "";
                        if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays")
                        {
                            leaveDay = "1";
                        }
                        else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays")
                        {

                            leaveDay = Convert.ToString(DateTime.DaysInMonth(day.Year, day.Month));
                        }
                        else
                        {
                            leaveDay = leaveTypeDetails?.LeaveAccruedDay;
                        }
                        if (Convert.ToInt32(leaveDay) == day.Date.Day)
                        {
                            fromDateBalance = fromDateBalance + (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                            LeaveHistoryView history = new LeaveHistoryView();
                            history.Date = day.Date;
                            history.Remark = "Accrual";
                            history.Added = (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                            history.Used = 0;
                            history.Balance = fromDateBalance;
                            history.CreatedOn = leaveTypeDetails.CreatedOn;
                            leaveHistory.Add(history);
                        }
                    }
                    else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower())
                    {
                        periodDate = GetFinancialYearQuarter(day);
                    }
                    else if (leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower())
                    {
                        periodDate = GetFinancialYearHalfYearly(day);
                    }
                    else if (leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                    {
                        periodDate = GetFinancialYearly(day);
                    }
                    if (leaveAccuredType?.AppConstantValue?.ToLower() == "Quarterly".ToLower() ||
                        leaveAccuredType?.AppConstantValue?.ToLower() == "HalfYearly".ToLower() ||
                        leaveAccuredType?.AppConstantValue?.ToLower() == "Yearly".ToLower())
                    {
                        if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "firstdays".ToLower())
                        {
                            if (periodDate.FromDate.Date == day.Date)
                            {
                                fromDateBalance = fromDateBalance + (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                                LeaveHistoryView history = new LeaveHistoryView();
                                history.Date = day.Date;
                                history.Remark = "Accrual";
                                history.Added = (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                                history.Used = 0;
                                history.Balance = fromDateBalance;
                                history.CreatedOn = leaveTypeDetails.CreatedOn;
                                leaveHistory.Add(history);
                            }
                        }
                        else if (leaveTypeDetails?.LeaveAccruedDay?.ToLower() == "lastdays".ToLower())
                        {
                            if (periodDate.ToDate.Date == day.Date)
                            {
                                fromDateBalance = fromDateBalance + (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                                LeaveHistoryView history = new LeaveHistoryView();
                                history.Date = day.Date;
                                history.Remark = "Accrual";
                                history.Added = (leaveTypeDetails?.LeaveAccruedNoOfDays == null ? 0 : (decimal)leaveTypeDetails.LeaveAccruedNoOfDays);
                                history.Used = 0;
                                history.Balance = fromDateBalance;
                                history.CreatedOn = leaveTypeDetails.CreatedOn;
                                leaveHistory.Add(history);
                            }
                        }
                    }
                }
                else
                {

                    //Add or Update leave balance
                    List<LeaveGrantRequestDetails> grantRequestList = grantRequestDetails.Where(x => x.EffectiveFromDate == day.Date).Select(x => x).ToList();
                    foreach (LeaveGrantRequestDetails grantRequest in grantRequestList)
                    {
                        //decimal balance = GetGrantLeaveBalance(model.EmployeeId, model.LeaveTypeId, day.Date, grantRequest.LeaveGrantDetailId);
                        decimal balance = GetGrantLeaveActualBalance(model.EmployeeId, model.LeaveTypeId, day.Date, grantRequestDetails, appliedLeaveDetails, true);
                        //decimal GetGrantLeaveActualBalance(int employeeId, int leaveTypeId, DateTime date, int grantRequestId, List<LeaveGrantRequestDetails> grantRequestDetails, List<AppliedLeaveTypeDetails> allAppliedLeaveDetails)
                        fromDateBalance = balance;
                        decimal balanceDay = grantRequest?.NumberOfDay == null ? 0 : (decimal)grantRequest.NumberOfDay;
                        LeaveHistoryView history = new LeaveHistoryView();
                        history.Date = day.Date;
                        history.Remark = grantRequest?.IsLeaveAdjustment == true ? "Manual Correction" : "Accrual";
                        history.Added = balanceDay > 0 ? balanceDay : 0;
                        history.Used = balanceDay <= 0 ? (-1 * balanceDay) : 0;
                        history.Balance = balance;
                        history.CreatedOn = grantRequest?.CreatedOn;
                        leaveHistory.Add(history);

                    }
                }

                //Check Applied leave dates
                List<AppliedLeaveTypeDetails> appliedLeave = allAppliedLeaveDetails.Where(x => x.Date == day.Date).Select(x => x).ToList();
                if (appliedLeave?.Count > 0)
                {

                    if (appConstantBasedOn?.AppConstantValue?.ToLower() != "LeaveGrant".ToLower())
                    {
                        decimal cancelLeave = fromDateBalance;

                        foreach (var item in appliedLeave)
                        {
                            if (item.Status == "Approved" || item.Status == "Pending")
                            {
                                fromDateBalance = fromDateBalance - (item.IsFullDay == true ? 1 : (decimal)0.5);
                                LeaveHistoryView history = new LeaveHistoryView();
                                history.Date = day.Date;
                                history.Remark = "Leave Taken";
                                history.Added = 0;
                                history.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                history.Balance = fromDateBalance;
                                history.CreatedOn = item?.CreatedOn;
                                leaveHistory.Add(history);
                            }
                            else if (item.Status == "Cancelled")
                            {
                                //fromDateBalance = fromDateBalance + (item.IsFullDay == true ? 1 : (decimal)0.5);
                                LeaveHistoryView cancelHistory = new LeaveHistoryView();
                                cancelHistory.Date = day.Date;
                                cancelHistory.Remark = "Leave Taken";
                                cancelHistory.Added = 0;
                                cancelHistory.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                cancelHistory.Balance = fromDateBalance - (item.IsFullDay == true ? 1 : (decimal)0.5);
                                cancelHistory.CreatedOn = item?.CreatedOn;
                                leaveHistory.Add(cancelHistory);

                                LeaveHistoryView history = new LeaveHistoryView();
                                history.Date = day.Date;
                                history.Remark = "Leave Cancelled";
                                history.Added = 0;
                                history.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                history.Balance = fromDateBalance;
                                history.CreatedOn = item?.CreatedOn;
                                leaveHistory.Add(history);
                            }
                            else if (item.Status == "Rejected")
                            {
                                if (item.AppliedLeaveStatus == true)
                                {
                                    fromDateBalance = fromDateBalance - (item.IsFullDay == true ? 1 : (decimal)0.5);
                                    LeaveHistoryView history = new LeaveHistoryView();
                                    history.Date = day.Date;
                                    history.Remark = "Leave Taken";
                                    history.Added = 0;
                                    history.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                    history.Balance = fromDateBalance;
                                    history.CreatedOn = item?.CreatedOn;
                                    leaveHistory.Add(history);
                                }
                                else
                                {
                                    //fromDateBalance = fromDateBalance + (item.IsFullDay == true ? 1 : (decimal)0.5);
                                    LeaveHistoryView cancelHistory = new LeaveHistoryView();
                                    cancelHistory.Date = day.Date;
                                    cancelHistory.Remark = "Leave Taken";
                                    cancelHistory.Added = 0;
                                    cancelHistory.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                    cancelHistory.Balance = fromDateBalance - (item.IsFullDay == true ? 1 : (decimal)0.5);
                                    cancelHistory.CreatedOn = item?.CreatedOn;
                                    leaveHistory.Add(cancelHistory);

                                    LeaveHistoryView history = new LeaveHistoryView();
                                    history.Date = day.Date;
                                    history.Remark = "Leave Rejected";
                                    history.Added = 0;
                                    history.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                    history.Balance = fromDateBalance;
                                    history.CreatedOn = item?.CreatedOn;
                                    leaveHistory.Add(history);
                                }
                            }

                        }
                    }
                    else
                    {


                        foreach (var item in appliedLeave)
                        {
                            if (item.Status == "Approved" || item.Status == "Pending")
                            {
                                decimal balance = GetGrantLeaveActualBalance(model.EmployeeId, model.LeaveTypeId, day.Date, grantRequestDetails, appliedLeaveDetails, false);
                                fromDateBalance = balance;
                                LeaveHistoryView history = new LeaveHistoryView();
                                history.Date = day.Date;
                                history.Remark = "Leave Taken";
                                history.Added = 0;
                                history.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                history.Balance = fromDateBalance;
                                history.CreatedOn = item?.CreatedOn;
                                leaveHistory.Add(history);
                            }
                            else if (item.Status == "Cancelled")
                            {
                                decimal balance = GetGrantLeaveActualBalance(model.EmployeeId, model.LeaveTypeId, day.Date, grantRequestDetails, appliedLeaveDetails, false);
                                //fromDateBalance = balance;

                                LeaveHistoryView cancelHistory = new LeaveHistoryView();
                                cancelHistory.Date = day.Date;
                                cancelHistory.Remark = "Leave Taken";
                                cancelHistory.Added = 0;
                                cancelHistory.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                cancelHistory.Balance = balance - (item.IsFullDay == true ? 1 : (decimal)0.5);
                                cancelHistory.CreatedOn = item?.CreatedOn;
                                leaveHistory.Add(cancelHistory);

                                LeaveHistoryView history = new LeaveHistoryView();
                                history.Date = day.Date;
                                history.Remark = "Leave Cancelled";
                                history.Added = 0;
                                history.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                history.Balance = balance;
                                history.CreatedOn = item?.CreatedOn;
                                leaveHistory.Add(history);
                            }
                            else if (item.Status == "Rejected")
                            {
                                if (item.AppliedLeaveStatus == true)
                                {
                                    decimal balance = GetGrantLeaveActualBalance(model.EmployeeId, model.LeaveTypeId, day.Date, grantRequestDetails, appliedLeaveDetails, false);
                                    fromDateBalance = balance;
                                    LeaveHistoryView history = new LeaveHistoryView();
                                    history.Date = day.Date;
                                    history.Remark = "Leave Taken";
                                    history.Added = 0;
                                    history.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                    history.Balance = fromDateBalance;
                                    history.CreatedOn = item?.CreatedOn;
                                    leaveHistory.Add(history);
                                }
                                else
                                {
                                    decimal balance = GetGrantLeaveActualBalance(model.EmployeeId, model.LeaveTypeId, day.Date, grantRequestDetails, appliedLeaveDetails, false);

                                    LeaveHistoryView cancelHistory = new LeaveHistoryView();
                                    cancelHistory.Date = day.Date;
                                    cancelHistory.Remark = "Leave Taken";
                                    cancelHistory.Added = 0;
                                    cancelHistory.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                    cancelHistory.Balance = balance - (item.IsFullDay == true ? 1 : (decimal)0.5);
                                    cancelHistory.CreatedOn = item?.CreatedOn;
                                    leaveHistory.Add(cancelHistory);

                                    LeaveHistoryView history = new LeaveHistoryView();
                                    history.Date = day.Date;
                                    history.Remark = "Leave Rejected";
                                    history.Added = 0;
                                    history.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
                                    history.Balance = balance;
                                    history.CreatedOn = item?.CreatedOn;
                                    leaveHistory.Add(history);
                                }
                            }

                        }
                    }

                }

                //Check leave Adjustment               

                if (appConstantBasedOn?.AppConstantValue?.ToLower() != "LeaveGrant".ToLower())
                {
                    List<LeaveAdjustmentDetails> adjustmentDetail = leaveAdjustmentDetails.Where(x => x.EffectiveFromDate.Value.Date == day.Date).OrderBy(x => x.CreatedOn).Select(x => x).ToList();
                    if (adjustmentDetail?.Count > 0)
                    {
                        foreach (LeaveAdjustmentDetails item in adjustmentDetail)
                        {

                            decimal added = 0;
                            decimal used = 0;
                            if (item?.AdjustmentBalance != null)
                            {
                                if (item.AdjustmentBalance >= fromDateBalance)
                                {
                                    added = (item.AdjustmentBalance == null ? 0 : (decimal)item.AdjustmentBalance) - fromDateBalance;
                                }
                                else
                                {
                                    used = fromDateBalance - (item.AdjustmentBalance == null ? 0 : (decimal)item.AdjustmentBalance);

                                }
                            }
                            fromDateBalance = item?.AdjustmentBalance == null ? 0 : (decimal)item.AdjustmentBalance;
                            LeaveHistoryView history = new LeaveHistoryView();
                            history.Date = day.Date;
                            history.Remark = "Manual Correction";
                            history.Added = added > 0 ? added : (-1 * added);
                            history.Used = used > 0 ? used : (-1 * used);
                            history.Balance = fromDateBalance;
                            history.CreatedOn = item?.CreatedOn;
                            leaveHistory.Add(history);
                        }
                    }
                }

                //Check Carryforward details
                if (day.Date == resetDate.Date)
                {
                    if (leaveCarryForwardDetails?.CarryForwardName?.ToLower() == "NoOfDays".ToLower())
                    {
                        decimal usedDays = 0;
                        if (fromDateBalance > leaveCarryForwardDetails?.MaximumCarryForwardDays)
                        {
                            usedDays = fromDateBalance - (leaveCarryForwardDetails?.MaximumCarryForwardDays == null ? 0 : (decimal)leaveCarryForwardDetails.MaximumCarryForwardDays);
                        }
                        fromDateBalance = fromDateBalance >= leaveCarryForwardDetails?.MaximumCarryForwardDays ? (leaveCarryForwardDetails?.MaximumCarryForwardDays == null ? 0 : (decimal)leaveCarryForwardDetails.MaximumCarryForwardDays) : fromDateBalance;
                        LeaveHistoryView history = new LeaveHistoryView();
                        history.Date = day.Date;
                        history.Remark = "Reset Leave(End Of Day)";
                        history.Added = 0;
                        history.Used = usedDays;
                        history.Balance = fromDateBalance;
                        history.CreatedOn = DateTime.Now;
                        leaveHistory.Add(history);
                    }
                    else if (leaveCarryForwardDetails?.CarryForwardName?.ToLower() == "None".ToLower())
                    {
                        LeaveHistoryView history = new LeaveHistoryView();
                        history.Date = day.Date;
                        history.Remark = "Reset Leave(End Of Day)";
                        history.Added = 0;
                        history.Used = fromDateBalance;
                        history.Balance = 0;
                        history.CreatedOn = DateTime.Now;
                        leaveHistory.Add(history);
                        fromDateBalance = 0;
                    }
                    else if (leaveCarryForwardDetails?.CarryForwardName?.ToLower() == "All".ToLower())
                    {
                        LeaveHistoryView history = new LeaveHistoryView();
                        history.Date = day.Date;
                        history.Remark = "Reset Leave(End Of Day)";
                        history.Added = 0;
                        history.Used = 0;
                        history.Balance = fromDateBalance;
                        history.CreatedOn = DateTime.Now;
                        leaveHistory.Add(history);
                    }

                    if (leaveCarryForwardDetails?.Period?.ToLower() == "monthly".ToLower())
                    {
                        resetDate = resetDate.AddMonths(1);
                    }
                    else if (leaveCarryForwardDetails?.Period?.ToLower() == "quarterly".ToLower())
                    {
                        resetDate = resetDate.AddMonths(3);
                    }
                    else if (leaveCarryForwardDetails?.Period?.ToLower() == "halfyearly".ToLower())
                    {
                        resetDate = resetDate.AddMonths(6);
                    }
                    else if (leaveCarryForwardDetails?.Period?.ToLower() == "yearly".ToLower())
                    {
                        resetDate = resetDate.AddMonths(12);
                    }
                }

                //Check geant leave expiry
                if (appConstantBasedOn?.AppConstantValue?.ToLower() == "LeaveGrant".ToLower())
                {
                    List<LeaveGrantRequestDetails> grantRequestList = allGrantRequestDetails.Where(x => x.EffectiveToDate == day.Date.AddDays(-1)).Select(x => x).ToList();
                    foreach (LeaveGrantRequestDetails grantRequest in grantRequestList)
                    {
                        decimal balance = GetGrantLeaveActualBalance(model.EmployeeId, model.LeaveTypeId, day.Date, grantRequestDetails, appliedLeaveDetails, true);
                        //fromDateBalance = balance;
                        //decimal balanceDay = grantRequest?.NumberOfDay == null ? 0 : (decimal)grantRequest.NumberOfDay;
                        if (grantRequest.BalanceDay > 0)
                        {
                            LeaveHistoryView history = new LeaveHistoryView();
                            history.Date = day.Date;
                            history.Remark = "Leave Expiry";
                            history.Added = 0;
                            history.Used = grantRequest?.BalanceDay == null ? 0 : (decimal)grantRequest.BalanceDay;
                            history.Balance = balance;
                            history.CreatedOn = grantRequest?.CreatedOn;
                            leaveHistory.Add(history);
                        }


                    }
                }
            }

            //foreach (AppliedLeaveTypeDetails item in appliedLeaveDetails)
            //{
            //    LeaveHistoryView history = new LeaveHistoryView();
            //    history.Date = item.Date;
            //    history.Remark = "Leave Taken";
            //    history.Used = item.IsFullDay == true ? 1 : (decimal)0.5;
            //    leaveHistory.Add(history);
            //}
            return leaveHistory.OrderBy(x => x.Date).ThenBy(x => x.CreatedOn).Select(x => x).ToList();
        }
        #endregion
        #region Get Grant leave actual balance by date
        public decimal GetGrantLeaveActualBalance(int employeeId, int leaveTypeId, DateTime date, List<LeaveGrantRequestDetails> grantRequestDetails, List<AppliedLeaveTypeDetails> allAppliedLeaveDetails, bool isAdd)
        {
            decimal balanceLeave = 0;
            List<LeaveGrantRequestDetails> requestDetails = _leaveGrantRequestDetailsRepository.GetGrantRequestDetailsByLeaveTypeId(leaveTypeId, employeeId, date, 0);
            if (requestDetails?.Count > 0)
            {
                DateTime startDate = requestDetails.Select(x => (DateTime)x.EffectiveFromDate).FirstOrDefault();
                for (DateTime day = startDate; day <= date; day = day.AddDays(1))
                {

                    //Check expited leave
                    List<LeaveGrantRequestDetails> expiryData = _leaveGrantRequestDetailsRepository.CheckGrantRequestExpiry(leaveTypeId, employeeId, day.AddDays(-1));
                    if (expiryData?.Count > 0)
                    {
                        foreach (LeaveGrantRequestDetails item in expiryData)
                        {
                            if (item?.BalanceDay != null && item?.BalanceDay > 0)
                            {
                                balanceLeave = balanceLeave - ((decimal)item.BalanceDay);
                            }
                        }
                    }
                    //Get Balance
                    List<LeaveGrantRequestDetails> grantRequestList = requestDetails.Where(x => x.EffectiveFromDate?.Date == day.Date).Select(x => x).ToList();
                    if (grantRequestList?.Count > 0)
                    {
                        foreach (LeaveGrantRequestDetails grantRequest in grantRequestList)
                        {
                            balanceLeave = balanceLeave + (grantRequest?.NumberOfDay == null ? 0 : (decimal)grantRequest.NumberOfDay);
                        }
                    }

                    //Check applied leave
                    if (isAdd == false || day.Date != date.Date)
                    {
                        List<AppliedLeaveTypeDetails> appliedLeave = allAppliedLeaveDetails.Where(x => x.Date == day.Date).Select(x => x).ToList();
                        if (appliedLeave?.Count > 0)
                        {
                            foreach (var item in appliedLeave)
                            {
                                balanceLeave = balanceLeave - (item.IsFullDay == true ? 1 : (decimal)0.5);
                            }
                        }
                    }


                }
            }
            return balanceLeave;
        }
        #endregion
        #region Get Grant leave card balance
        public decimal GetGrantLeaveCardBalance(int employeeId, int leaveTypeId, DateTime Fromate, DateTime toDate, List<LeaveGrantRequestDetails> grantRequestDetails, List<AppliedLeaveTypeDetails> allAppliedLeaveDetails)
        {
            decimal balanceLeave = 0;
            List<LeaveGrantRequestDetails> requestDetails = _leaveGrantRequestDetailsRepository.GetGrantRequestCardBalance(leaveTypeId, employeeId, Fromate, toDate);
            if (requestDetails?.Count > 0)
            {
                foreach (LeaveGrantRequestDetails data in requestDetails)
                {
                    balanceLeave = balanceLeave + (data?.BalanceDay == null ? 0 : (decimal)data.BalanceDay);
                }
            }
            return balanceLeave;
        }
        #endregion
        #region Get reset day
        public DateTime GetResetDate(DateTime fromDate, int leaveTypeId, LeaveCarryForwardListView leaveCarryForwardDetails)
        {
            DateTime todayDate = fromDate;
            DateTime executedate = DateTime.MinValue;
            var Quarter = GetFinancialYearQuarter(todayDate);
            var HalfYearly = GetFinancialYearHalfYearly(todayDate);

            if (leaveCarryForwardDetails?.Period?.ToLower() == "monthly".ToLower())
            {
                if (leaveCarryForwardDetails?.ResetDay?.ToLower() == "firstdays".ToLower())
                {
                    executedate = new DateTime(todayDate.Year, todayDate.Month, 1);
                }
                else if (leaveCarryForwardDetails?.ResetDay?.ToLower() == "lastdays".ToLower())
                {
                    executedate = new DateTime(todayDate.Year, todayDate.Month, DateTime.DaysInMonth(todayDate.Year, todayDate.Month));
                }
                else
                {
                    executedate = new DateTime(todayDate.Year, todayDate.Month, Convert.ToInt32(leaveCarryForwardDetails.ResetDay));
                }
                if (executedate < todayDate)
                {
                    executedate = executedate.AddMonths(1);
                }
            }
            else if (leaveCarryForwardDetails?.Period?.ToLower() == "quarterly".ToLower())
            {

                if (leaveCarryForwardDetails?.ResetDay?.ToLower() == "FirstDays".ToLower())
                {
                    executedate = Quarter.FromDate;
                }
                else if (leaveCarryForwardDetails?.ResetDay?.ToLower() == "LastDays".ToLower())
                {
                    executedate = Quarter.ToDate;
                }
                if (executedate < todayDate)
                {
                    executedate = executedate.AddMonths(3);
                }
            }
            else if (leaveCarryForwardDetails?.Period?.ToLower() == "halfYearly".ToLower())
            {

                if (leaveCarryForwardDetails?.ResetDay?.ToLower() == "FirstDays".ToLower())
                {
                    executedate = HalfYearly.FromDate;
                }
                else if (leaveCarryForwardDetails?.ResetDay?.ToLower() == "LastDays".ToLower())
                {
                    executedate = HalfYearly.ToDate;
                }
                if (executedate < todayDate)
                {
                    executedate = executedate.AddMonths(6);
                }
            }
            else if (leaveCarryForwardDetails?.Period?.ToLower() == "yearly".ToLower())
            {

                int month = (int)leaveCarryForwardDetails?.ResetMonth;
                var dat = leaveCarryForwardDetails?.ResetDay;
                int year = todayDate.Year; //DateTime.UtcNow.Year;
                if (leaveCarryForwardDetails?.ResetDay?.ToLower() == "firstdays".ToLower())
                {
                    executedate = new DateTime(year, month, 1);
                }
                else if (leaveCarryForwardDetails?.ResetDay?.ToLower() == "lastdays".ToLower())
                {
                    executedate = new DateTime(year, month, DateTime.DaysInMonth(year, month));
                }
                else
                {
                    executedate = new DateTime(year, month, Convert.ToInt32(leaveCarryForwardDetails.ResetDay));
                }
                if (executedate < todayDate)
                {
                    executedate = executedate.AddMonths(12);
                }
            }

            return executedate;
        }
        #endregion

        #region Get Employee Leave and Balance Details
        public async Task<IndividualLeaveList> GetEmployeeLeavesBalanceDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            individualLeaveList.EmployeeAvailableLeaveDetails = _leaveRestrictionsRepository.GetEmployeeLeavesBalanceDetails(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate, employeeLeaveandRestriction.DateOfJoining);
            //individualLeaveList.HolidayList = _holidayRepository.GetCurrentFinancialYearHolidayList(employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate, employeeLeaveandRestriction.LocationId, employeeLeaveandRestriction.ShiftId, employeeLeaveandRestriction.DateOfJoining);
            if (individualLeaveList?.EmployeeAvailableLeaveDetails?.Count > 0)
            {
                int currrentFinanceYear = DateTime.Now.Year;
                List<EmployeeAvailableLeaveDetails> EmployeeAvailableLeaveListDetails = new List<EmployeeAvailableLeaveDetails>();
                if (DateTime.Now.Month > 3)
                    currrentFinanceYear = DateTime.Now.Year;
                else
                    currrentFinanceYear = DateTime.Now.Year - 1;

                foreach (EmployeeAvailableLeaveDetails item in individualLeaveList?.EmployeeAvailableLeaveDetails)
                {
                 

                    //if (item?.LeaveAdjustmentDetails?.Count > 0)
                    //{
                    //    var leaveAdjustmentList = (from le in item.LeaveAdjustmentDetails
                    //                               group le by le.EffectiveFromDate into ad
                    //                               select ad).ToList();
                    //    List<LeaveAdjustmentDetails> adjustment = new List<LeaveAdjustmentDetails>();
                    //    foreach (var detail in leaveAdjustmentList)
                    //    {
                    //        LeaveAdjustmentDetails data = detail.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                    //        adjustment.Add(data);
                    //    }
                    //    item.LeaveAdjustmentDetails = adjustment;
                    //}
                    //Calculate grant leave balance
                    if (item.BalanceBasedOn.Select(x => x.BalanceBasedOnValue).FirstOrDefault() == "LeaveGrant")
                    {
                        decimal balance = 0;
                        balance = GetGrantLeaveCardBalance(employeeLeaveandRestriction.EmployeeId, item.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, employeeLeaveandRestriction.FromDate, DateTime.Now.Date, item.LeaveGrantRequestDetails, item.AppliedLeaveDates);
                        if (employeeLeaveandRestriction.FromDate.Year != currrentFinanceYear)
                        {
                            item.DisplayBalanceLeave = _leaveGrantRequestDetailsRepository.GetGrantRequestCardBalanceByDate(employeeLeaveandRestriction.EmployeeId, item.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, employeeLeaveandRestriction.ToDate);
                        }
                        else
                        {
                            item.DisplayBalanceLeave = balance;
                        }

                        //decimal balance = GetGrantLeaveBalance(employeeLeaveandRestriction.EmployeeId,item.LeaveTypeID==null?0:(int)item.LeaveTypeID, DateTime.Now.Date,0);
                        item.ActualBalanceLeave = balance;
                        item.BalanceLeave = balance;
                        item.AdjustmentBalanceLeave = 0;
                        item.AdjustmentEffectiveFromDate = null;
                    }
                    else
                    {

                        //AppConstants appConstantBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);
                        if (employeeLeaveandRestriction.FromDate.Year != currrentFinanceYear)
                        {

                            DateTime fromDate = DateTime.Now.Date;
                            DateTime toDate = employeeLeaveandRestriction.ToDate;
                            if (employeeLeaveandRestriction.DateOfJoining != null && employeeLeaveandRestriction.DateOfJoining > fromDate)
                            {
                                fromDate = (DateTime)employeeLeaveandRestriction.DateOfJoining;
                            }
                            if (item.EffectiveFromDate != null && item.EffectiveFromDate > fromDate)
                            {
                                fromDate = (DateTime)item.EffectiveFromDate;
                            }
                            if (item.EffectiveToDate != null && item.EffectiveToDate < toDate)
                            {
                                fromDate = (DateTime)item.EffectiveToDate;
                            }
                            if (employeeLeaveandRestriction.DateOfJoining != null && employeeLeaveandRestriction.DateOfJoining > toDate)
                            {
                                toDate = (DateTime)employeeLeaveandRestriction.DateOfJoining;
                            }
                            List<AppliedLeaveTypeDetails> appliedLeaveDetails = _leaveRepository.GetAppliedLeaveByLeaveTypeAndDate(item?.EmployeeID == null ? 0 : (int)item.EmployeeID, fromDate, toDate, item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID);
                            List<LeaveAdjustmentDetails> leaveAdjustmentDetails = _leaveAdjustmentDetailsRepository.GetLeaveAdjustmentDetailsByDate(item?.EmployeeID == null ? 0 : (int)item.EmployeeID, item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, fromDate, toDate);
                            LeaveCarryForwardListView leaveCarryForwardDetails = _leaveEntitlementRepository.GetLeaveTypeCarryForwardDetails(item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID);
                            item.DisplayBalanceLeave = await GetFinancialYearLeaveBalance(item?.ActualBalanceLeave == null ? 0 : (decimal)item.ActualBalanceLeave, fromDate, toDate, employeeLeaveandRestriction.EmployeeId, item?.LeaveTypeID == null ? 0 : (int)item.LeaveTypeID, appliedLeaveDetails, leaveAdjustmentDetails, leaveCarryForwardDetails);
                        }

                    }
               
                    if (item.BalanceLeave != null)
                    {
                        item.BalanceLeave = GetRoundOff((decimal)item.BalanceLeave);
                    }
                    if (item.DisplayBalanceLeave != null)
                    {
                        item.DisplayBalanceLeave = GetRoundOffValues((decimal)item.DisplayBalanceLeave);
                    }
                }
            }
              return individualLeaveList;
        }
        #endregion

        public async Task<EmployeeAvailableLeaveDetails> GetEmployeeLeaveDetailsByEmployeeIdAndLeaveId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            EmployeeAvailableLeaveDetails employeeLeaveDetails = new();
            employeeLeaveDetails = _leaveRestrictionsRepository.GetLeaveDetailsByEmployeeIdAndLeaveId(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate, employeeLeaveandRestriction.LeaveId);
            if (employeeLeaveDetails != null)
            {
                int currrentFinanceYear = DateTime.Now.Year;
                if (DateTime.Now.Month > 3)
                    currrentFinanceYear = DateTime.Now.Year;
                else
                    currrentFinanceYear = DateTime.Now.Year - 1;
                if(employeeLeaveDetails != null)
                {
                    if (!string.IsNullOrEmpty(employeeLeaveDetails.ResetPeriod) && !string.IsNullOrEmpty(employeeLeaveDetails.ResetDay))
                    {
                        employeeLeaveDetails.LeaveResetOn = GetLeaveResetDate(employeeLeaveDetails.ResetPeriod, employeeLeaveDetails.ResetDay, employeeLeaveDetails.ResetMonth == null ? 0 : (int)employeeLeaveDetails.ResetMonth);
                    }

                    if (employeeLeaveDetails?.LeaveAdjustmentDetails?.Count > 0)
                    {
                        var leaveAdjustmentList = (from le in employeeLeaveDetails.LeaveAdjustmentDetails
                                                   group le by le.EffectiveFromDate into ad
                                                   select ad).ToList();
                        List<LeaveAdjustmentDetails> adjustment = new List<LeaveAdjustmentDetails>();
                        foreach (var detail in leaveAdjustmentList)
                        {
                            LeaveAdjustmentDetails data = detail.OrderByDescending(x => x.CreatedOn).FirstOrDefault();
                            adjustment.Add(data);
                        }
                        employeeLeaveDetails.LeaveAdjustmentDetails = adjustment;
                    }
                    //Calculate grant leave balance
                    if (employeeLeaveDetails.BalanceBasedOn.Select(x => x.BalanceBasedOnValue).FirstOrDefault() == "LeaveGrant")
                    {
                        decimal balance = 0;
                        balance = GetGrantLeaveCardBalance(employeeLeaveandRestriction.EmployeeId, employeeLeaveDetails.LeaveTypeID == null ? 0 : (int)employeeLeaveDetails.LeaveTypeID, employeeLeaveandRestriction.FromDate, DateTime.Now.Date, employeeLeaveDetails.LeaveGrantRequestDetails, employeeLeaveDetails.AppliedLeaveDates);
                        if (employeeLeaveandRestriction.FromDate.Year != currrentFinanceYear)
                        {
                            employeeLeaveDetails.DisplayBalanceLeave = _leaveGrantRequestDetailsRepository.GetGrantRequestCardBalanceByDate(employeeLeaveandRestriction.EmployeeId, employeeLeaveDetails.LeaveTypeID == null ? 0 : (int)employeeLeaveDetails.LeaveTypeID, employeeLeaveandRestriction.ToDate);
                        }
                        else
                        {
                            employeeLeaveDetails.DisplayBalanceLeave = balance;
                        }

                        //decimal balance = GetGrantLeaveBalance(employeeLeaveandRestriction.EmployeeId,employeeLeaveDetails.LeaveTypeID==null?0:(int)employeeLeaveDetails.LeaveTypeID, DateTime.Now.Date,0);
                        employeeLeaveDetails.ActualBalanceLeave = balance;
                        employeeLeaveDetails.BalanceLeave = balance;
                        employeeLeaveDetails.AdjustmentBalanceLeave = 0;
                        employeeLeaveDetails.AdjustmentEffectiveFromDate = null;
                    }
                    else
                    {

                        //AppConstants appConstantBasedOn = _appConstantsRepository.GetAppconstantByID(leaveTypeDetails.BalanceBasedOn);
                        if (employeeLeaveandRestriction.FromDate.Year != currrentFinanceYear)
                        {

                            DateTime fromDate = DateTime.Now.Date;
                            DateTime toDate = employeeLeaveandRestriction.ToDate;
                            if (employeeLeaveandRestriction.DateOfJoining != null && employeeLeaveandRestriction.DateOfJoining > fromDate)
                            {
                                fromDate = (DateTime)employeeLeaveandRestriction.DateOfJoining;
                            }
                            if (employeeLeaveDetails.EffectiveFromDate != null && employeeLeaveDetails.EffectiveFromDate > fromDate)
                            {
                                fromDate = (DateTime)employeeLeaveDetails.EffectiveFromDate;
                            }
                            if (employeeLeaveDetails.EffectiveToDate != null && employeeLeaveDetails.EffectiveToDate < toDate)
                            {
                                fromDate = (DateTime)employeeLeaveDetails.EffectiveToDate;
                            }
                            if (employeeLeaveandRestriction.DateOfJoining != null && employeeLeaveandRestriction.DateOfJoining > toDate)
                            {
                                toDate = (DateTime)employeeLeaveandRestriction.DateOfJoining;
                            }
                            List<AppliedLeaveTypeDetails> appliedLeaveDetails = _leaveRepository.GetAppliedLeaveByLeaveTypeAndDate(employeeLeaveDetails?.EmployeeID == null ? 0 : (int)employeeLeaveDetails.EmployeeID, fromDate, toDate, employeeLeaveDetails?.LeaveTypeID == null ? 0 : (int)employeeLeaveDetails.LeaveTypeID);
                            List<LeaveAdjustmentDetails> leaveAdjustmentDetails = _leaveAdjustmentDetailsRepository.GetLeaveAdjustmentDetailsByDate(employeeLeaveDetails?.EmployeeID == null ? 0 : (int)employeeLeaveDetails.EmployeeID, employeeLeaveDetails?.LeaveTypeID == null ? 0 : (int)employeeLeaveDetails.LeaveTypeID, fromDate, toDate);
                            LeaveCarryForwardListView leaveCarryForwardDetails = _leaveEntitlementRepository.GetLeaveTypeCarryForwardDetails(employeeLeaveDetails?.LeaveTypeID == null ? 0 : (int)employeeLeaveDetails.LeaveTypeID);
                            employeeLeaveDetails.DisplayBalanceLeave = await GetFinancialYearLeaveBalance(employeeLeaveDetails?.ActualBalanceLeave == null ? 0 : (decimal)employeeLeaveDetails.ActualBalanceLeave, fromDate, toDate, employeeLeaveandRestriction.EmployeeId, employeeLeaveDetails?.LeaveTypeID == null ? 0 : (int)employeeLeaveDetails.LeaveTypeID, appliedLeaveDetails, leaveAdjustmentDetails, leaveCarryForwardDetails);
                        }

                    }
                    if (employeeLeaveDetails.MaximumCarryForwardDays != null)
                    {
                        employeeLeaveDetails.MaximumCarryForwardDays = GetRoundOff((decimal)employeeLeaveDetails.MaximumCarryForwardDays);
                    }
                    if (employeeLeaveDetails.ActualBalanceLeave != null)
                    {
                        employeeLeaveDetails.ActualBalanceLeave = GetRoundOffValues((decimal)employeeLeaveDetails.ActualBalanceLeave);
                    }
                    if (employeeLeaveDetails.AdjustmentBalanceLeave != null)
                    {
                        employeeLeaveDetails.AdjustmentBalanceLeave = GetRoundOffValues((decimal)employeeLeaveDetails.AdjustmentBalanceLeave);
                    }
                    if (employeeLeaveDetails.MaximumConsecutiveDays != null)
                    {
                        employeeLeaveDetails.MaximumConsecutiveDays = GetHalfDayValues((decimal)employeeLeaveDetails.MaximumConsecutiveDays);
                    }
                    if (employeeLeaveDetails.MaximumLeavePerApplication != null)
                    {
                        employeeLeaveDetails.MaximumLeavePerApplication = GetHalfDayValues((decimal)employeeLeaveDetails.MaximumLeavePerApplication);
                    }
                    if (employeeLeaveDetails.MinimumGapTwoApplication != null)
                    {
                        employeeLeaveDetails.MinimumGapTwoApplication = GetHalfDayValues((decimal)employeeLeaveDetails.MinimumGapTwoApplication);
                    }
                    if (employeeLeaveDetails.MinimumNoOfApplicationsPeriod != null)
                    {
                        employeeLeaveDetails.MinimumNoOfApplicationsPeriod = GetRoundOff((decimal)employeeLeaveDetails.MinimumNoOfApplicationsPeriod);
                    }
                    if (employeeLeaveDetails.EnableFileUpload != null)
                    {
                        employeeLeaveDetails.EnableFileUpload = GetHalfDayValues((decimal)employeeLeaveDetails.EnableFileUpload);
                    }
                    if (employeeLeaveDetails.BalanceLeave != null)
                    {
                        employeeLeaveDetails.BalanceLeave = GetRoundOff((decimal)employeeLeaveDetails.BalanceLeave);
                    }
                    if (employeeLeaveDetails.GrantMinimumNoOfRequestDay != null)
                    {
                        employeeLeaveDetails.GrantMinimumNoOfRequestDay = GetHalfDayValues((decimal)employeeLeaveDetails.GrantMinimumNoOfRequestDay);
                    }
                    if (employeeLeaveDetails.GrantMaximumNoOfRequestDay != null)
                    {
                        employeeLeaveDetails.GrantMaximumNoOfRequestDay = GetHalfDayValues((decimal)employeeLeaveDetails.GrantMaximumNoOfRequestDay);
                    }
                    if (employeeLeaveDetails.GrantUploadDocumentSpecificPeriodDay != null)
                    {
                        employeeLeaveDetails.GrantUploadDocumentSpecificPeriodDay = GetHalfDayValues((decimal)employeeLeaveDetails.GrantUploadDocumentSpecificPeriodDay);
                    }
                    if (employeeLeaveDetails.DisplayBalanceLeave != null)
                    {
                        employeeLeaveDetails.DisplayBalanceLeave = GetRoundOffValues((decimal)employeeLeaveDetails.DisplayBalanceLeave);
                    }
                }
              
                  
            }
            //individualLeaveList.AppliedLeaveList = _leaveRepository.GetAppliedLeaveByEmployeeId(employeeLeaveandRestriction.EmployeeId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate);
            //individualLeaveList.HolidayDetails = _holidayRepository.GetHolidayDetails(employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate, employeeLeaveandRestriction.LocationId, employeeLeaveandRestriction.DateOfJoining);
            return employeeLeaveDetails;
        }
        #region Get Applied Leave Details By EmployeeId And LeaveId grouping
        public async Task<List<ApplyLeavesView>> GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestrictiond)
        {
            var data = _leaveRepository.GetEmployeeAppliedLeaveDetailsByEmployeeIdAndLeaveId(employeeLeaveandRestrictiond);
            return data;
        }
        #endregion

        #region Get Employee Applied leave Details
        public IndividualLeaveList GetEmployeeHolidayDetails(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            individualLeaveList.HolidayDetails = _holidayRepository.GetHolidayDetails(employeeLeaveandRestriction.DepartmentId, employeeLeaveandRestriction.FromDate, employeeLeaveandRestriction.ToDate.AddYears(1), employeeLeaveandRestriction.LocationId, employeeLeaveandRestriction.DateOfJoining);
            return individualLeaveList;
        }
        #endregion

        #region Get Employee Applied leave Details By EmployeeId
        public IndividualLeaveList GetAppliedLeaveDetailsByEmployeeId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            IndividualLeaveList individualLeaveList = new();
            individualLeaveList = _leaveRepository.GetEmployeeLeavesByEmployeeId(employeeLeaveandRestriction);
            return individualLeaveList;
        }
        #endregion

        #region
        public List<EmployeeRequestCount> GetPendingLeaveCount(EmployeeListByDepartment employeeList)
        {
            return _leaveRepository.GetPendingLeaveCount( employeeList);
        }
        #endregion
        #region
        public List<int> GetGrantLeaveByManagerId(int managerId)
        {
            return _leaveGrantRequestDetailsRepository.GetGrantLeaveByManagerId(managerId);
        }
        #endregion
        
    }
}