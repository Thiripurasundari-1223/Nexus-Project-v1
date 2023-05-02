using Attendance.DAL.Repository;
using SharedLibraries.Common;
using SharedLibraries.Models.Attendance;
using SharedLibraries.ViewModels;
using SharedLibraries.ViewModels.Attendance;
using SharedLibraries.ViewModels.Employees;
using SharedLibraries.ViewModels.Home;
using SharedLibraries.ViewModels.Leaves;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Attendance.DAL.Services
{
    public class AttendanceServices
    {
        private readonly IShiftDetailsRepository _shiftDetailsRepository;
        private readonly ITimeDefinitionRepository _timeDefinitionRepository;
        private readonly IWeekendDefinitionRepository _weekendDefinitionRepository;
        private readonly IShiftWeekendDefinitionRepository _shiftWeekendDefinitionRepository;
        private readonly IAttendanceRepository _AttendanceRepository;
        private readonly IAttendanceDetailRepository _AttendanceDetailRepository;
        private readonly IAbsentSettingRepository _AbsentSettingRepository;
        private readonly IAbsentDepartmentRepository _AbsentDepartmentRepository;
        private readonly IAbsentDesignationRepository _AbsentDesignationRepository;
        private readonly IAbsentLocationRepository _AbsentLocationRepository;
        private readonly IAbsentRoleRepository _AbsentRoleRepository;
        private readonly IAbsentEmployeeTypeRepository _AbsentEmployeeTypeRepository;
        private readonly IAbsentProbationStatusRepository _AbsentProbationStatusRepository;
        private readonly IAbsentEmployeeRepository _AbsentEmployeeRepository;
        private readonly IAbsentRestrictionRepository _AbsentRestrictionRepository;

        #region Constructor
        public AttendanceServices(IShiftDetailsRepository shiftDetailsRepository,
                                  ITimeDefinitionRepository timeDefinitionRepository,
                                  IWeekendDefinitionRepository weekendDefinitionRepository,
                                  IShiftWeekendDefinitionRepository shiftWeekendDefinitionRepository,
                                  IAttendanceRepository AttendanceRepository,
                                  IAttendanceDetailRepository AttendanceDetailRepository,
                                  IAbsentSettingRepository AbsentSettingRepository,
                                  IAbsentDepartmentRepository AbsentDepartmentRepository,
                                  IAbsentDesignationRepository AbsentDesignationRepository,
                                  IAbsentLocationRepository AbsentLocationRepository,
                                  IAbsentRoleRepository AbsentRoleRepository,
                                  IAbsentEmployeeTypeRepository AbsentEmployeeTypeRepository,
                                  IAbsentProbationStatusRepository AbsentProbationStatusRepository,
                                  IAbsentEmployeeRepository AbsentEmployeeRepository,
                                  IAbsentRestrictionRepository AbsentRestrictionRepository)
        {
            _shiftDetailsRepository = shiftDetailsRepository;
            _timeDefinitionRepository = timeDefinitionRepository;
            _weekendDefinitionRepository = weekendDefinitionRepository;
            _shiftWeekendDefinitionRepository = shiftWeekendDefinitionRepository;
            _AttendanceRepository = AttendanceRepository;
            _AttendanceDetailRepository = AttendanceDetailRepository;
            _AbsentSettingRepository = AbsentSettingRepository;
            _AbsentDepartmentRepository = AbsentDepartmentRepository;
            _AbsentDesignationRepository = AbsentDesignationRepository;
            _AbsentLocationRepository = AbsentLocationRepository;
            _AbsentRoleRepository = AbsentRoleRepository;
            _AbsentEmployeeTypeRepository = AbsentEmployeeTypeRepository;
            _AbsentProbationStatusRepository = AbsentProbationStatusRepository;
            _AbsentEmployeeRepository = AbsentEmployeeRepository;
            _AbsentRestrictionRepository = AbsentRestrictionRepository;
        }
        #endregion

        #region Add or Update Shift
        public async Task<int> AddOrUpdateShift(ShiftDetailsView shiftDetailsView)
        {
            try
            {
                int shiftDetailsId = 0;
                ShiftDetails shiftDetails = new ShiftDetails();
                if (shiftDetailsView.ShiftDetailsId != 0) shiftDetails = _shiftDetailsRepository.GetByID(shiftDetailsView.ShiftDetailsId);
                if (shiftDetails != null)
                {
                    shiftDetails.ShiftName = shiftDetailsView.ShiftName;
                    shiftDetails.ShiftCode = shiftDetailsView.ShiftCode;
                    string[] shiftTimeFrom = shiftDetailsView?.TimeFrom.Split(":");
                    if (!string.IsNullOrEmpty(shiftTimeFrom[0]))
                    {
                        shiftDetails.TimeFrom = new TimeSpan(shiftTimeFrom?.Length > 0 ? Convert.ToInt32(shiftTimeFrom[0]) : 0, shiftTimeFrom?.Length > 1 ? Convert.ToInt32(shiftTimeFrom[1]) : 0, shiftTimeFrom?.Length > 2 ? Convert.ToInt32(shiftTimeFrom[2]) : 0);
                    }
                    else { shiftDetails.TimeFrom = null; }
                    string[] shiftTimeTo = shiftDetailsView?.TimeTo.Split(":");
                    if (!string.IsNullOrEmpty(shiftTimeTo[0]))
                    {
                        shiftDetails.TimeTo = new TimeSpan(shiftTimeTo?.Length > 0 ? Convert.ToInt32(shiftTimeTo[0]) : 0, shiftTimeTo?.Length > 1 ? Convert.ToInt32(shiftTimeTo[1]) : 0, shiftTimeTo?.Length > 2 ? Convert.ToInt32(shiftTimeTo[2]) : 0);
                    }
                    else { shiftDetails.TimeTo = null; }
                    shiftDetails.ShiftDescription = shiftDetailsView.ShiftDescription;
                    //shiftDetails.EmployeeGroupId = shiftDetailsView.EmployeeGroupId;
                    shiftDetails.OverTime = shiftDetailsView.OverTime;
                    //shiftDetails.IsActive = false;
                    shiftDetails.IsFlexyShift = shiftDetailsView.IsFlexyShift;

                    if (shiftDetailsView.ShiftDetailsId == 0)
                    {
                        shiftDetails.CreatedOn = DateTime.UtcNow;
                        shiftDetails.CreatedBy = shiftDetailsView.CreatedBy;
                        await _shiftDetailsRepository.AddAsync(shiftDetails);
                        await _shiftDetailsRepository.SaveChangesAsync();
                        shiftDetailsId = shiftDetails.ShiftDetailsId;
                    }
                    else
                    {
                        shiftDetails.ModifiedOn = DateTime.UtcNow;
                        shiftDetails.ModifiedBy = shiftDetailsView.ModifiedBy;
                        _shiftDetailsRepository.Update(shiftDetails);
                        shiftDetailsId = shiftDetailsView.ShiftDetailsId;
                        await _shiftDetailsRepository.SaveChangesAsync();
                    }

                    if (shiftDetailsView?.TimeDefinition != null)
                    {
                        TimeDefinition timeDefinition = new TimeDefinition();
                        if (shiftDetailsView?.ShiftDetailsId != 0) timeDefinition = _timeDefinitionRepository.GetTimeDefinitionByShiftID(shiftDetailsView.ShiftDetailsId);
                        if (timeDefinition != null)
                        {
                            timeDefinition.TimeFrom = null;
                            timeDefinition.TimeTo = null;
                            //string[] timeDefFrom = shiftDetailsView.TimeDefinition.TimeFrom.Split(":");
                            //if (!string.IsNullOrEmpty(timeDefFrom[0]))
                            //{
                            //    timeDefinition.TimeFrom = new TimeSpan(timeDefFrom?.Length > 0 ? Convert.ToInt32(timeDefFrom[0]) : 0, timeDefFrom.Length > 1 ? Convert.ToInt32(timeDefFrom[1]) : 0, timeDefFrom.Length > 2 ? Convert.ToInt32(timeDefFrom[2]) : 0);
                            //}
                            //else { timeDefinition.TimeFrom = null; }
                            //string[] timeDefTo = shiftDetailsView.TimeDefinition.TimeTo.Split(":");
                            //if (!string.IsNullOrEmpty(timeDefTo[0]))
                            //{
                            //    timeDefinition.TimeTo = new TimeSpan(timeDefTo?.Length > 0 ? Convert.ToInt32(timeDefTo[0]) : 0, timeDefTo.Length > 1 ? Convert.ToInt32(timeDefTo[1]) : 0, timeDefTo.Length > 2 ? Convert.ToInt32(timeDefTo[2]) : 0);
                            //}
                            //else { timeDefinition.TimeTo = null; }
                            string[] BreakTime = shiftDetailsView?.TimeDefinition?.BreakTime.Split(":");
                            if (!string.IsNullOrEmpty(BreakTime[0]))
                            {
                                timeDefinition.BreakTime = new TimeSpan(BreakTime?.Length > 0 ? Convert.ToInt32(BreakTime[0]) : 0, BreakTime?.Length > 1 ? Convert.ToInt32(BreakTime[1]) : 0, BreakTime?.Length > 2 ? Convert.ToInt32(BreakTime[2]) : 0);
                            }
                            else { timeDefinition.BreakTime = null; }
                            string[] TotalHours = shiftDetailsView?.TimeDefinition?.TotalHours.Split(":");
                            if (!string.IsNullOrEmpty(TotalHours[0]))
                            {
                                timeDefinition.TotalHours = new TimeSpan(TotalHours?.Length > 0 ? Convert.ToInt32(TotalHours[0]) : 0, TotalHours?.Length > 1 ? Convert.ToInt32(TotalHours[1]) : 0, TotalHours?.Length > 2 ? Convert.ToInt32(TotalHours[2]) : 0);
                            }
                            else { timeDefinition.TotalHours = null; }
                            string[] AbsentFromHour = shiftDetailsView?.TimeDefinition?.AbsentFromHour.Split(":");
                            if (!string.IsNullOrEmpty(AbsentFromHour[0]))
                            {
                                timeDefinition.AbsentFromHour = new TimeSpan(AbsentFromHour?.Length > 0 ? Convert.ToInt32(AbsentFromHour[0]) : 0, AbsentFromHour?.Length > 1 ? Convert.ToInt32(AbsentFromHour[1]) : 0, AbsentFromHour?.Length > 2 ? Convert.ToInt32(AbsentFromHour[2]) : 0);
                            }
                            else { timeDefinition.AbsentFromHour = null; }
                            timeDefinition.AbsentFromOperator = shiftDetailsView?.TimeDefinition?.AbsentFromOperator;
                            string[] AbsentToHour = shiftDetailsView?.TimeDefinition?.AbsentToHour.Split(":");
                            if (!string.IsNullOrEmpty(AbsentToHour[0]))
                            {
                                timeDefinition.AbsentToHour = new TimeSpan(AbsentToHour?.Length > 0 ? Convert.ToInt32(AbsentToHour[0]) : 0, AbsentToHour?.Length > 1 ? Convert.ToInt32(AbsentToHour[1]) : 0, AbsentToHour?.Length > 2 ? Convert.ToInt32(AbsentToHour[2]) : 0);
                            }
                            else { timeDefinition.AbsentToHour = null; }
                            timeDefinition.AbsentToOperator = shiftDetailsView?.TimeDefinition?.AbsentToOperator;
                            string[] HalfaDayFromHour = shiftDetailsView?.TimeDefinition?.HalfaDayFromHour.Split(":");
                            if (!string.IsNullOrEmpty(HalfaDayFromHour[0]))
                            {
                                timeDefinition.HalfaDayFromHour = new TimeSpan(HalfaDayFromHour?.Length > 0 ? Convert.ToInt32(HalfaDayFromHour[0]) : 0, HalfaDayFromHour?.Length > 1 ? Convert.ToInt32(HalfaDayFromHour[1]) : 0, HalfaDayFromHour?.Length > 2 ? Convert.ToInt32(HalfaDayFromHour[2]) : 0);
                            }
                            else { timeDefinition.HalfaDayFromHour = null; }
                            timeDefinition.HalfaDayFromOperator = shiftDetailsView?.TimeDefinition?.HalfaDayFromOperator;
                            string[] HalfaDayToHour = shiftDetailsView?.TimeDefinition?.HalfaDayToHour.Split(":");
                            if (!string.IsNullOrEmpty(HalfaDayToHour[0]))
                            {
                                timeDefinition.HalfaDayToHour = new TimeSpan(HalfaDayToHour?.Length > 0 ? Convert.ToInt32(HalfaDayToHour[0]) : 0, HalfaDayToHour?.Length > 1 ? Convert.ToInt32(HalfaDayToHour[1]) : 0, HalfaDayToHour?.Length > 2 ? Convert.ToInt32(HalfaDayToHour[2]) : 0);
                            }
                            else { timeDefinition.HalfaDayToHour = null; }
                            timeDefinition.HalfaDayToOperator = shiftDetailsView?.TimeDefinition?.HalfaDayToOperator;
                            string[] PresentHour = shiftDetailsView?.TimeDefinition?.PresentHour.Split(":");
                            if (!string.IsNullOrEmpty(HalfaDayToHour[0]))
                            {
                                timeDefinition.PresentHour = new TimeSpan(PresentHour?.Length > 0 ? Convert.ToInt32(PresentHour[0]) : 0, PresentHour?.Length > 1 ? Convert.ToInt32(PresentHour[1]) : 0, PresentHour?.Length > 2 ? Convert.ToInt32(PresentHour[2]) : 0);
                            }
                            else { timeDefinition.PresentHour = null; }
                            timeDefinition.PresentOperator = shiftDetailsView?.TimeDefinition?.PresentOperator;
                            timeDefinition.IsConsiderAbsent = shiftDetailsView?.TimeDefinition?.IsConsiderAbsent;
                            timeDefinition.IsConsiderPresent = shiftDetailsView?.TimeDefinition?.IsConsiderPresent;
                            timeDefinition.IsConsiderHalfaDay = shiftDetailsView?.TimeDefinition?.IsConsiderHalfaDay;

                            if (shiftDetailsView.ShiftDetailsId == 0)
                            {
                                timeDefinition.CreatedOn = DateTime.UtcNow;
                                timeDefinition.CreatedBy = shiftDetailsView.TimeDefinition.CreatedBy;
                                timeDefinition.ShiftDetailsId = shiftDetails.ShiftDetailsId;
                                await _timeDefinitionRepository.AddAsync(timeDefinition);
                            }
                            else
                            {
                                timeDefinition.ModifiedOn = DateTime.UtcNow;
                                timeDefinition.ModifiedBy = shiftDetailsView.TimeDefinition.ModifiedBy;
                                timeDefinition.ShiftDetailsId = shiftDetails.ShiftDetailsId;
                                _timeDefinitionRepository.Update(timeDefinition);
                            }
                            await _timeDefinitionRepository.SaveChangesAsync();
                        }
                    }

                    if (shiftDetailsView.WeekendDefinitionView != null)
                    {
                        List<ShiftWeekendDefinition> shiftWeekendDefinition = new List<ShiftWeekendDefinition>();
                        if (shiftDetailsView?.ShiftDetailsId != 0) shiftWeekendDefinition = _shiftWeekendDefinitionRepository.GetWeekendDefinitionByShiftID(shiftDetailsView.ShiftDetailsId);
                        if (shiftWeekendDefinition == null || shiftDetailsView?.ShiftDetailsId == 0)
                        {
                            if (shiftDetailsView?.WeekendDefinitionView?.Count > 0)
                            {
                                foreach (var item in shiftDetailsView?.WeekendDefinitionView)
                                {
                                    ShiftWeekendDefinition weekenddefinition = new ShiftWeekendDefinition
                                    {
                                        WeekendDayId = item.WeekendDayId,
                                        ShiftDetailsId = shiftDetailsId,
                                        CreatedBy = item.CreatedBy,
                                        CreatedOn = DateTime.UtcNow
                                    };
                                    await _shiftWeekendDefinitionRepository.AddAsync(weekenddefinition);
                                    await _shiftWeekendDefinitionRepository.SaveChangesAsync();
                                }
                            }
                        }
                        else
                        {
                            foreach (var item in shiftWeekendDefinition)
                            {
                                ShiftWeekendDefinition shiftWeekend = _shiftWeekendDefinitionRepository.GetByShiftWeekendDefinitonId(shiftDetailsId);
                                if (shiftWeekend != null && shiftWeekend?.ShiftDetailsId > 0)
                                {
                                    _shiftWeekendDefinitionRepository.Delete(shiftWeekend);
                                    await _shiftWeekendDefinitionRepository.SaveChangesAsync();
                                }
                            }

                            if (shiftDetailsView?.WeekendDefinitionView?.Count > 0)
                            {
                                foreach (var item in shiftDetailsView?.WeekendDefinitionView)
                                {
                                    ShiftWeekendDefinition weekenddefinitions = new ShiftWeekendDefinition
                                    {
                                        WeekendDayId = item.WeekendDayId,
                                        ShiftDetailsId = shiftDetailsId,
                                        ModifiedBy = item.ModifiedBy,
                                        ModifiedOn = DateTime.UtcNow
                                    };
                                    await _shiftWeekendDefinitionRepository.AddAsync(weekenddefinitions);
                                    await _shiftWeekendDefinitionRepository.SaveChangesAsync();
                                }
                            }
                        }
                    }
                }
                return shiftDetailsId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Delete Shift
        public async Task<bool> DeleteShift(int pShiftDetailsId)
        {
            try
            {
                if (pShiftDetailsId > 0)
                {
                    ShiftDetails shiftDetails = _shiftDetailsRepository.GetByID(pShiftDetailsId);
                    if (shiftDetails != null && shiftDetails?.ShiftDetailsId > 0)
                    {
                        _shiftDetailsRepository.Delete(shiftDetails);
                        await _shiftDetailsRepository.SaveChangesAsync();
                    }
                    TimeDefinition timeDefinition = _timeDefinitionRepository.GetByID(pShiftDetailsId);
                    if (timeDefinition != null && timeDefinition?.ShiftDetailsId > 0)
                    {
                        _timeDefinitionRepository.Delete(timeDefinition);
                        await _timeDefinitionRepository.SaveChangesAsync();
                    }
                    ShiftWeekendDefinition weekendDefinition = _weekendDefinitionRepository.GetByID(pShiftDetailsId);
                    if (weekendDefinition != null && weekendDefinition?.ShiftDetailsId > 0)
                    {
                        _weekendDefinitionRepository.Delete(weekendDefinition);
                        await _weekendDefinitionRepository.SaveChangesAsync();
                    }
                    return true;
                }
            }
            catch (Exception)
            {
                throw;
                //logger.Error(ex.Message.ToString());
            }
            return false;
        }
        #endregion

        #region Get All ShiftDetails
        public List<ShiftView> GetAllShiftDetails()
        {
            return _shiftDetailsRepository.GetAllShiftDetails();
        }
        #endregion

        #region Get ShiftDetails By Id
        public AttendanceShiftDetailsView GetByShiftDetailsId(int pShiftDetailsId)
        {
            try
            {
                AttendanceShiftDetailsView attendanceShiftDetails = _shiftDetailsRepository.GetByShiftDetailsId(pShiftDetailsId);
                attendanceShiftDetails.timeDefinition = _timeDefinitionRepository.GetByTimeDefinitionShiftDetailsId(pShiftDetailsId);
                attendanceShiftDetails.weekendDefinition = _weekendDefinitionRepository.GetByWeekendDefinitionShiftDetailsId(pShiftDetailsId);
                return attendanceShiftDetails;
            }
            catch (Exception)
            {
                throw;
                //logger.Error(ex.Message.ToString());
                //return new AttendanceShiftDetailsView();
            }
        }
        #endregion

        #region Insert or Update Attendance
        public async Task<int> AddorUpdateAttendance(AttendanceView attendance)
        {
            int attendanceId = 0;
            try
            {
                SharedLibraries.Models.Attendance.Attendance empAttendance = new SharedLibraries.Models.Attendance.Attendance();
                if (attendance?.EmployeeId != 0) empAttendance = _AttendanceRepository.GetAttendanceById(attendance.EmployeeId, attendance.Date);
                if (empAttendance == null)
                {
                    SharedLibraries.Models.Attendance.Attendance empAttendanceAdd = new SharedLibraries.Models.Attendance.Attendance()
                    {
                        EmployeeId = attendance.EmployeeId,
                        Date = attendance.Date.Date,
                        IsCheckin = attendance.IsCheckin,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = attendance.EmployeeId,
                    };
                    await _AttendanceRepository.AddAsync(empAttendanceAdd);
                    await _AttendanceRepository.SaveChangesAsync();
                    attendanceId = empAttendanceAdd.AttendanceId;
                    AttendanceDetail attendanceDetailAdd = new AttendanceDetail
                    {
                        AttendanceId = attendanceId,
                        CheckinTime = attendance.CheckinCheckoutTime,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = attendance.CreatedBy,
                    };
                    await _AttendanceDetailRepository.AddAsync(attendanceDetailAdd);
                    await _AttendanceDetailRepository.SaveChangesAsync();
                }
                else
                {
                    if(attendance?.IsCheckin ==true && _AttendanceDetailRepository.GetAttendanceDetailByAttendanceId(empAttendance.AttendanceId, (DateTime)attendance.CheckinCheckoutTime, (DateTime)attendance.CheckinCheckoutTime.Value.AddMinutes(1), 0, false))
                    {
                        return -1;
                    }
                    else
                    {
                        if (attendance?.IsCheckin == false)
                        {
                            AttendanceDetail detail = _AttendanceDetailRepository.GetAttendanceDetailById(empAttendance.AttendanceId);
                            TimeSpan difference = detail?.CheckinTime == null ? new TimeSpan(0, 0, 0) : (attendance?.CheckinCheckoutTime - detail?.CheckinTime).Value;

                            detail.CheckoutTime = attendance?.CheckinCheckoutTime;
                            detail.TotalHours = difference;

                            _AttendanceDetailRepository.Update(detail);
                            await _AttendanceDetailRepository.SaveChangesAsync();
                            empAttendance.TotalHours = _AttendanceDetailRepository.GetAttendanceHoursById(empAttendance.AttendanceId);
                        }
                        else
                        {
                            //AttendanceDetail detail = _AttendanceDetailRepository.GetAttendanceDetailById(empAttendance.AttendanceId);
                            //TimeSpan difference = detail?.CheckoutTime == null ? new TimeSpan(0, 0, 0) : (attendance.CheckinCheckoutTime - detail.CheckoutTime).Value;

                            AttendanceDetail attendanceDetailAdd = new AttendanceDetail
                            {
                                AttendanceId = empAttendance.AttendanceId,
                                CheckinTime = attendance.CheckinCheckoutTime,
                                CreatedOn = DateTime.UtcNow,
                                CreatedBy = attendance.EmployeeId,
                            };
                            //detail.BreakHours = difference;
                            //detail.BreakoutTime = attendance.CheckinCheckoutTime;
                            //_AttendanceDetailRepository.Update(detail);
                            await _AttendanceDetailRepository.AddAsync(attendanceDetailAdd);
                            await _AttendanceDetailRepository.SaveChangesAsync();
                            //empAttendance.BreakHours = _AttendanceDetailRepository.GetAttendanceBreakHoursById(empAttendance.AttendanceId);
                        }
                        empAttendance.IsCheckin = attendance.IsCheckin;
                        empAttendance.ModifiedOn = DateTime.UtcNow;
                        empAttendance.ModifiedBy = attendance.EmployeeId;
                        _AttendanceRepository.Update(empAttendance);
                        await _AttendanceRepository.SaveChangesAsync();
                        attendanceId = empAttendance.AttendanceId;
                    }
                    
                }
                return attendanceId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion

        #region Get Daily Attendance Detail
        public WeeklyMonthlyAttendance GetDailyAttendancedetail(WeekMonthAttendanceView attendanceDetail)
        {
            if (attendanceDetail?.EmployeeId != 0)
            {
                WeeklyMonthlyAttendance attendance = _AttendanceDetailRepository.GetDailyAttendanceDetail(attendanceDetail.EmployeeId, attendanceDetail.ShiftDetailsId == null ? 0 : (int)attendanceDetail.ShiftDetailsId, attendanceDetail.ShiftDate);
                if (attendance?.WeeklyMonthlyAttendanceDetail?.Count > 0)
                {
                    attendance.WeeklyMonthlyAttendanceDetail = attendance?.WeeklyMonthlyAttendanceDetail?.Select(x => x).OrderBy(x => x.CheckinTime).ToList();

                    List<TimeSpan?> totalBreakHour = new List<TimeSpan?>();
                    for (int i = 0; i < attendance?.WeeklyMonthlyAttendanceDetail?.Count; i++)
                    {
                        if (i < (attendance?.WeeklyMonthlyAttendanceDetail?.Count - 1))
                        {
                            attendance.WeeklyMonthlyAttendanceDetail[i].BreakinTime = attendance?.WeeklyMonthlyAttendanceDetail[i]?.CheckoutTime;
                            attendance.WeeklyMonthlyAttendanceDetail[i].BreakoutTime = attendance?.WeeklyMonthlyAttendanceDetail[i + 1]?.CheckinTime;
                            TimeSpan? breakHours = attendance?.WeeklyMonthlyAttendanceDetail[i]?.BreakoutTime - attendance?.WeeklyMonthlyAttendanceDetail[i]?.CheckoutTime;
                            attendance.WeeklyMonthlyAttendanceDetail[i].BreakHours = breakHours == null ? new TimeSpan(0).ToString() : breakHours.ToString();
                        }
                        if (!string.IsNullOrEmpty(attendance?.WeeklyMonthlyAttendanceDetail[i]?.BreakHours))
                        {
                            string[] breakHour = attendance?.WeeklyMonthlyAttendanceDetail[i]?.BreakHours.Split(":");
                            if (breakHour?.Length > 0 && !string.IsNullOrEmpty(breakHour[0]))
                            {
                                totalBreakHour.Add(new TimeSpan(breakHour?.Length > 0 ? Convert.ToInt32(breakHour[0]) : 0, breakHour?.Length > 1 ? Convert.ToInt32(breakHour[1].Substring(0, 2)) : 0, breakHour?.Length > 2 ? Convert.ToInt32(breakHour[2].Substring(0, 2)) : 0));
                            }
                        }
                    }
                    attendance.BreakHours = new TimeSpan(totalBreakHour.Sum(r => r == null ? 0 : r.Value.Ticks)).ToString();
                }
                return attendance;
            }
            else
            {
                return new WeeklyMonthlyAttendance();
            }
        }
        #endregion

        #region Get Weekly and Monthly Attendance
        public WeekMonthAttendanceDetailedView GetWeeklyMonthlyAttendanceDetail(WeekMonthAttendanceView weekMonthAttendance)
        {
            WeekMonthAttendanceDetailedView detailedView = new();
            AbsentSettingsView absentApplicable = new();
            absentApplicable = _AbsentSettingRepository.GetAbsentSetting();
            EmployeeDetailsForLeaveView empDetails = new();
            empDetails = weekMonthAttendance?.EmployeeDetails;
            switch (empDetails?.Gender?.ToLower())
            {
                case "male":
                    empDetails.GenderMale = true;
                    empDetails.GenderFemale = null;
                    empDetails.GenderOther = null;

                    break;
                case "female":
                    empDetails.GenderMale = null;
                    empDetails.GenderFemale = true;
                    empDetails.GenderOther = null;
                    break;
                case "other":
                    empDetails.GenderMale = null;
                    empDetails.GenderFemale = null;
                    empDetails.GenderOther = true;
                    break;
                default:
                    empDetails.GenderMale = null;
                    empDetails.GenderFemale = null;
                    empDetails.GenderOther = null;
                    break;
            }
            switch (empDetails?.MaritalStatus?.ToLower())
            {
                case "single":
                    empDetails.MaritalStatusSingle = true;
                    empDetails.MaritalStatusMarried = null;
                    break;
                case "married":
                    empDetails.MaritalStatusSingle = null;
                    empDetails.MaritalStatusMarried = true;
                    break;
                default:
                    empDetails.MaritalStatusSingle = null;
                    empDetails.MaritalStatusMarried = null;
                    break;
            }
            bool absentapplicable = false;
            bool isEligible = true;
            bool isApplicable = false;
            if ((absentApplicable?.Gender_Male_Applicable == true && empDetails?.GenderMale == true) || (absentApplicable?.Gender_Female_Applicable == true && empDetails?.GenderFemale == true) || (absentApplicable?.Gender_Others_Applicable == true && empDetails?.GenderOther == true))
            {
                absentapplicable = true;
                isApplicable = true;
            }
            else if (absentApplicable?.Gender_Male_Applicable != true && absentApplicable?.Gender_Female_Applicable != true && absentApplicable?.Gender_Others_Applicable != true)
            {
                absentapplicable = true;
            }
            else
            {
                isEligible = false;
            }
            if ((absentApplicable?.MaritalStatus_Single_Applicable == true && empDetails?.MaritalStatusSingle == true) || (absentApplicable?.MaritalStatus_Married_Applicable == true && empDetails?.MaritalStatusMarried == true))
            {
                absentapplicable = true;
                isApplicable = true;
            }
            else if (absentApplicable?.MaritalStatus_Single_Applicable != true && absentApplicable?.MaritalStatus_Married_Applicable != true)
            {
                absentapplicable = true;
            }
            else
            {
                isEligible = false;
            }

            EmployeeLeaveApplicableView applicable = GetAbsentApplicable(empDetails);
            var exceptions = GetAbsentExceptions(empDetails);
            bool AbsentApplicable = false;
            List<AbsentEmployee> employeeAbsentApplicable = _AbsentEmployeeRepository.GetApplicableAbsentEmployee(absentApplicable?.AbsentSettingId == null ? 0 : (int)absentApplicable?.AbsentSettingId);
            if (employeeAbsentApplicable.Any(x => x.AbsentApplicableEmployeeId != 0) && employeeAbsentApplicable.Any(m => m.AbsentApplicableEmployeeId == empDetails.EmployeeID))
            {
                AbsentApplicable = true;
            }
            // exception
            bool absentexception = false;
            bool isEligibleException = false;
            bool isException = false;

            if ((absentApplicable?.Gender_Male_Exception == true && empDetails?.GenderMale == true) || (absentApplicable?.Gender_Female_Exception == true && empDetails?.GenderFemale == true) || (absentApplicable?.Gender_Others_Exception == true && empDetails?.GenderOther == true))
            {
                absentexception = true;
                isException = true;
            }
            else if (absentApplicable?.Gender_Male_Exception != true && absentApplicable?.Gender_Female_Exception != true && absentApplicable?.Gender_Others_Exception != true)
            {
                absentexception = false;
            }
            else
            {
                isEligibleException = false;
            }
            if ((absentApplicable?.MaritalStatus_Single_Exception == true && empDetails?.MaritalStatusSingle == true) || (absentApplicable?.MaritalStatus_Married_Exception == true && empDetails?.MaritalStatusMarried == true))
            {
                absentexception = true;
                isException = true;
            }
            else if (absentApplicable?.MaritalStatus_Single_Exception != true && absentApplicable?.MaritalStatus_Married_Exception != true)
            {
                absentexception = false;
            }
            else
            {
                isEligibleException = false;
            }
            //employee exception
            bool employeeException = false;
            List<AbsentEmployee> absentEmployeeException = _AbsentEmployeeRepository.GetExceptionAbsentEmployee(absentApplicable?.AbsentSettingId == null ? 0 : (int)absentApplicable?.AbsentSettingId);
            if (absentEmployeeException.Any(x => x.AbsentExceptionEmployeeId != 0) && absentEmployeeException.Any(m => m.AbsentExceptionEmployeeId == empDetails.EmployeeID))
            {
                employeeException = true;
            }
            if ((isEligibleException == false && absentexception == false && exceptions == false && isException == false) && (employeeException == false) && weekMonthAttendance.IsMail == false)
            {
                if ((isEligible && absentapplicable && applicable.IsApplicable && (isApplicable || applicable.IsCreteriaMached)) || (AbsentApplicable))
                {
                    weekMonthAttendance.FromDate = weekMonthAttendance.FromDate.AddDays(-7);
                    weekMonthAttendance.ToDate = weekMonthAttendance.ToDate.AddDays(+7);
                    detailedView.Restriction = _AbsentRestrictionRepository.GetAbsentRestriction();
                    detailedView.IsApplicable = true;
                }
            }

            List<WeeklyMonthlyAttendance> attendanceDetail = _AttendanceDetailRepository.GetWeeklyMonthlyAttendanceDetail(weekMonthAttendance);
            // Break hours
            /*
            foreach (WeeklyMonthlyAttendance attendance in attendanceDetail)
            {
                if (attendance?.WeeklyMonthlyAttendanceDetail?.Count > 0)
                {
                    attendance.WeeklyMonthlyAttendanceDetail = attendance?.WeeklyMonthlyAttendanceDetail?.Select(x => x).OrderBy(x => x.CheckinTime).ToList();

                    List<TimeSpan?> totalBreakHour = new List<TimeSpan?>();
                    List<TimeSpan?> totalRegularizeHour = new List<TimeSpan?>();
                    for (int i = 0; i < attendance?.WeeklyMonthlyAttendanceDetail?.Count; i++)
                    {
                        if (i < (attendance?.WeeklyMonthlyAttendanceDetail?.Count - 1))
                        {
                            attendance.WeeklyMonthlyAttendanceDetail[i].BreakinTime = attendance?.WeeklyMonthlyAttendanceDetail[i]?.CheckoutTime;
                            attendance.WeeklyMonthlyAttendanceDetail[i].BreakoutTime = attendance?.WeeklyMonthlyAttendanceDetail[i + 1]?.CheckinTime;
                            TimeSpan? breakHours = attendance?.WeeklyMonthlyAttendanceDetail[i]?.BreakoutTime - attendance?.WeeklyMonthlyAttendanceDetail[i]?.CheckoutTime;
                            attendance.WeeklyMonthlyAttendanceDetail[i].BreakHours = breakHours == null ? new TimeSpan(0).ToString() : breakHours.ToString();
                        }
                        if (!string.IsNullOrEmpty(attendance?.WeeklyMonthlyAttendanceDetail[i]?.BreakHours))
                        {
                            string[] breakHour = attendance?.WeeklyMonthlyAttendanceDetail[i]?.BreakHours.Split(":");
                            if (breakHour?.Length > 0 && !string.IsNullOrEmpty(breakHour[0]))
                            {
                                totalBreakHour.Add(new TimeSpan(breakHour?.Length > 0 ? Convert.ToInt32(breakHour[0]) : 0, breakHour?.Length > 1 ? Convert.ToInt32(breakHour[1].Substring(0, 2)) : 0, breakHour?.Length > 2 ? Convert.ToInt32(breakHour[2].Substring(0, 2)) : 0));
                            }
                        }
                        if(attendance.WeeklyMonthlyAttendanceDetail[i].Status=="Pending" &&
                            !string.IsNullOrEmpty(attendance?.WeeklyMonthlyAttendanceDetail[i]?.TotalHours))
                        {
                            string[] totalHour = attendance?.WeeklyMonthlyAttendanceDetail[i]?.TotalHours.Split(":");
                            totalRegularizeHour.Add(new TimeSpan(totalHour?.Length > 0 ? Convert.ToInt32(totalHour[0]) : 0, totalHour?.Length > 1 ? Convert.ToInt32(totalHour[1].Substring(0, 2)) : 0, totalHour?.Length > 2 ? Convert.ToInt32(totalHour[2].Substring(0, 2)) : 0));
                        }
                    }
                    attendance.BreakHours = new TimeSpan(totalBreakHour.Sum(r => r == null ? 0 : r.Value.Ticks)).ToString();
                    attendance.TotalPendingRegularizedHours = "00:00:00";
                    if(totalRegularizeHour?.Count>0)
                    {
                        attendance.TotalPendingRegularizedHours = new TimeSpan(totalRegularizeHour.Sum(r => r == null ? 0 : r.Value.Ticks)).ToString();
                    }
                }
            }
            // Break hours
            */
            detailedView.Attendances = attendanceDetail == null ? new List<WeeklyMonthlyAttendance>() : attendanceDetail;
            detailedView.ShiftDetail = _AttendanceDetailRepository.GetShiftDetails((int)weekMonthAttendance.ShiftDetailsId); 
            detailedView.WeekendShiftDetailList = _shiftDetailsRepository.GetShiftWeekendDetailsList(); 
            return detailedView; 
        }
        #endregion

        #region Get Attendance Details By Date
        public List<WeeklyMonthlyAttendance> GetWeeklyAttendanceDetailByDate(WeekMonthAttendanceView weekMonthAttendance)
        {

            List<WeeklyMonthlyAttendance> attendanceDetail = _AttendanceDetailRepository.GetWeeklyMonthlyAttendanceDetail(weekMonthAttendance);
            foreach (WeeklyMonthlyAttendance attendance in attendanceDetail)
            {
                if (attendance?.WeeklyMonthlyAttendanceDetail?.Count > 0)
                {
                    attendance.WeeklyMonthlyAttendanceDetail = attendance?.WeeklyMonthlyAttendanceDetail?.Select(x => x).OrderBy(x => x.CheckinTime).ToList();

                    List<TimeSpan?> totalBreakHour = new List<TimeSpan?>();
                    for (int i = 0; i < attendance?.WeeklyMonthlyAttendanceDetail?.Count; i++)
                    {
                        if (i < (attendance?.WeeklyMonthlyAttendanceDetail?.Count - 1))
                        {
                            attendance.WeeklyMonthlyAttendanceDetail[i].BreakinTime = attendance?.WeeklyMonthlyAttendanceDetail[i]?.CheckoutTime;
                            attendance.WeeklyMonthlyAttendanceDetail[i].BreakoutTime = attendance?.WeeklyMonthlyAttendanceDetail[i + 1]?.CheckinTime;
                            TimeSpan? breakHours = attendance?.WeeklyMonthlyAttendanceDetail[i]?.BreakoutTime - attendance?.WeeklyMonthlyAttendanceDetail[i]?.CheckoutTime;
                            attendance.WeeklyMonthlyAttendanceDetail[i].BreakHours = breakHours == null ? new TimeSpan(0).ToString() : breakHours.ToString();
                        }
                        if (!string.IsNullOrEmpty(attendance?.WeeklyMonthlyAttendanceDetail[i]?.BreakHours))
                        {
                            string[] breakHour = attendance?.WeeklyMonthlyAttendanceDetail[i]?.BreakHours.Split(":");
                            if (breakHour?.Length > 0 && !string.IsNullOrEmpty(breakHour[0]))
                            {
                                totalBreakHour.Add(new TimeSpan(breakHour?.Length > 0 ? Convert.ToInt32(breakHour[0]) : 0, breakHour?.Length > 1 ? Convert.ToInt32(breakHour[1].Substring(0, 2)) : 0, breakHour?.Length > 2 ? Convert.ToInt32(breakHour[2].Substring(0, 2)) : 0));
                            }
                        }
                    }
                    attendance.BreakHours = new TimeSpan(totalBreakHour.Sum(r => r == null ? 0 : r.Value.Ticks)).ToString();
                }

            }
            return attendanceDetail == null ? new List<WeeklyMonthlyAttendance>() : attendanceDetail;

        }
        #endregion

        #region Get All Attendance Details
        public EmployeeAttendanceShiftDetailsView GetEmployeeAttendanceDetails(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            EmployeeAttendanceShiftDetailsView employeeAttendance = new();
            employeeAttendance.EmployeesAttendances = _AttendanceDetailRepository.GetEmployeeAttendanceDetails(managerTeamLeaveView);
            employeeAttendance.ShiftViewDetails = _shiftDetailsRepository.GetShiftWeekendDetailsList();
            return employeeAttendance;
        }
        #endregion

        #region Get Details
        public List<DetailsView> GetAttendanceDetailsById(AttendanceWeekView attendanceWeekView)
        {
            return _AttendanceDetailRepository.GetAttendanceDetailsById(attendanceWeekView);
        }
        #endregion

        #region Insert or Update Attendance
        public async Task<int> InsertorUpdateTimeLog(TimelogView timelogView)
        {
            int attendanceId = 0;
            try
            {
                SharedLibraries.Models.Attendance.Attendance empAttendance = new SharedLibraries.Models.Attendance.Attendance();
                if (timelogView?.EmployeeId != 0) empAttendance = _AttendanceRepository.GetAttendanceById((int)timelogView.EmployeeId, (DateTime)timelogView.Date);
                if (empAttendance == null)
                {
                    SharedLibraries.Models.Attendance.Attendance empAttendanceAdd = new SharedLibraries.Models.Attendance.Attendance()
                    {
                        EmployeeId = (int)timelogView.EmployeeId,
                        Date = DateTime.UtcNow,
                        IsCheckin = false,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = (int)timelogView.EmployeeId
                    };
                    await _AttendanceRepository.AddAsync(empAttendanceAdd);
                    await _AttendanceRepository.SaveChangesAsync();
                    attendanceId = empAttendanceAdd.AttendanceId;
                    AttendanceDetail attendanceDetailAdd = new AttendanceDetail
                    {
                        AttendanceId = attendanceId,
                        CheckinTime = timelogView.CheckinTime,
                        CheckoutTime = timelogView.CheckoutTime,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = timelogView.EmployeeId,
                        Reason = timelogView.Reason,
                    };
                    await _AttendanceDetailRepository.AddAsync(attendanceDetailAdd);
                    await _AttendanceDetailRepository.SaveChangesAsync();
                }
                else
                {
                    AttendanceDetail attendanceDetailAdd = new AttendanceDetail
                    {
                        AttendanceId = empAttendance.AttendanceId,
                        CheckinTime = timelogView.CheckinTime,
                        CheckoutTime = timelogView.CheckoutTime,
                        CreatedOn = DateTime.UtcNow,
                        CreatedBy = timelogView.EmployeeId,
                        Reason = timelogView.Reason,
                    };
                    await _AttendanceDetailRepository.AddAsync(attendanceDetailAdd);
                    await _AttendanceDetailRepository.SaveChangesAsync();
                }
                return attendanceId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion

        #region Get Shift Name By Id
        public List<KeyWithValue> GetShiftNameById(List<int> shiftId)
        {
            return _shiftDetailsRepository.GetShiftNameById(shiftId);
        }
        #endregion

        #region Get Shift Id By Name
        public int GetShiftIdByName(string pShiftName)
        {
            return _shiftDetailsRepository.GetShiftIdByName(pShiftName);
        }
        #endregion

        #region Get All Shift Name
        public List<KeyWithValue> GetAllShiftName()
        {
            return _shiftDetailsRepository.GetAllShiftName();
        }
        #endregion

        #region Get Employee Shift Master Data
        public List<EmployeeShiftMasterData> GetEmployeeShiftMasterData()
        {

            return _shiftDetailsRepository.GetEmployeeShiftMasterData();
        }
        #endregion

        #region Get Shift Weekend Detais
        public ShiftDetailedView GetShiftWeekendDetails(int ShiftDetailsId)
        {
            ShiftDetailedView shiftDetailedView = new();
            shiftDetailedView.shiftViewDetails = _AttendanceDetailRepository.GetShiftWeekendDetails(ShiftDetailsId);
            shiftDetailedView.DefaultShiftView = _shiftDetailsRepository.GetShiftWeekendDetailsList();
            return shiftDetailedView;
        }
        #endregion

        #region Get Employee Absent List
        public List<ApplyLeavesView> GetEmployeeAbsentList(EmployeeDepartmentAndLocationView employeeDepartments)
        {
            return _AttendanceDetailRepository.GetEmployeeAbsentList(employeeDepartments);
        }
        #endregion

        #region Get Attendance Home Report
        public HomeReportData GetAttendanceHomeReport()
        {
            try
            {
                return _AttendanceRepository.GetAttendanceHomeReport();
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region 
        public List<EmployeesAttendanceDetails> GetEmployeeAttendanceDetailsByEmployeeId(int employeeId, DateTime fromDate, DateTime toDate)
        {
            return _AttendanceDetailRepository.GetEmployeeAttendanceDetailsByEmployeeId(employeeId, fromDate, toDate);
        }
        #endregion
        #region Insert or Update Attendance Details Time Log
        public async Task<int> InsertorUpdateAttendanceDetailTimeLog(TimeLogDetailView timelogView)
        {
            int AttendanceDetailId = 0;
            int attendance_Id = 0;
            try
            {
                SharedLibraries.Models.Attendance.Attendance empAttendance = new SharedLibraries.Models.Attendance.Attendance();
                DateTime dateTime = DateTime.UtcNow;
                TimeSpan totalHours = timelogView.CheckoutTime - timelogView.CheckinTime;
                if (totalHours.Ticks < new TimeSpan(24, 0, 0).Ticks)
                {

                    if (dateTime > timelogView?.CheckinTime)
                    {
                        if (timelogView?.EmployeeId != 0) empAttendance = _AttendanceRepository.GetAttendanceDetailById(timelogView.Date, (int)timelogView.EmployeeId);
                        if (empAttendance != null)
                        {
                            //bool isEdit = false;
                            //if(timelogView.AttendanceDetailId>0)
                            //{
                            //    isEdit = true;
                            //}
                            TimeSpan? exsitingTotalHour = _AttendanceDetailRepository.GetAttendanceDetailByEmployeeId(timelogView.EmployeeId, timelogView.Date, timelogView.AttendanceDetailId);
                            TimeSpan? totalCheckedinHours = exsitingTotalHour + totalHours;
                            bool isattendanceDetailsExists = _AttendanceDetailRepository.GetAttendanceDetailByAttendanceId(empAttendance.AttendanceId, timelogView.CheckinTime, timelogView.CheckoutTime, timelogView.AttendanceDetailId, timelogView?.AttendanceDetailId > 0 ? true : false);
                            if (!isattendanceDetailsExists)
                            {
                                if (totalCheckedinHours < new TimeSpan(24, 0, 0))
                                {
                                    AttendanceDetail attendanceDetailAdd = new();
                                    if (timelogView.AttendanceDetailId != 0) { attendanceDetailAdd = _AttendanceDetailRepository.GetAttendanceDetailByAttendanceDetailId(timelogView.AttendanceDetailId); }
                                    if (attendanceDetailAdd != null)
                                    {
                                        attendanceDetailAdd.AttendanceId = empAttendance.AttendanceId;
                                        attendanceDetailAdd.CheckinTime = timelogView.CheckinTime;
                                        attendanceDetailAdd.CheckoutTime = timelogView.CheckoutTime;
                                        attendanceDetailAdd.Reason = timelogView.Reason;
                                        attendanceDetailAdd.Isregularize = timelogView.Isregularize;
                                        //attendanceDetailAdd.Status = "Pending";
                                        attendanceDetailAdd.TotalHours = totalHours;
                                        attendanceDetailAdd.ManagerId = timelogView?.ApproverManagerId;
                                        if (timelogView.AttendanceDetailId == 0)
                                        {
                                            attendanceDetailAdd.CreatedOn = DateTime.UtcNow;
                                            attendanceDetailAdd.CreatedBy = timelogView.CreatedBy;
                                            attendanceDetailAdd.Status = "Pending";
                                            await _AttendanceDetailRepository.AddAsync(attendanceDetailAdd);
                                            await _AttendanceDetailRepository.SaveChangesAsync();
                                            AttendanceDetailId = attendanceDetailAdd.AttendanceDetailId;
                                        }
                                        else
                                        {
                                            _AttendanceDetailRepository.Update(attendanceDetailAdd);
                                            AttendanceDetailId = timelogView.AttendanceDetailId;
                                            await _AttendanceDetailRepository.SaveChangesAsync();
                                        }
                                    }
                                }
                                else
                                {
                                    AttendanceDetailId = -3;
                                }
                            }
                            else
                            {
                                AttendanceDetailId = -1;
                            }
                        }
                        else
                        {
                            DateTime fromdate = new DateTime(timelogView.Date.Year, timelogView.Date.Month, timelogView.Date.Day);
                            SharedLibraries.Models.Attendance.Attendance empAttendanceAdd = new SharedLibraries.Models.Attendance.Attendance()
                            {
                                EmployeeId = timelogView.EmployeeId,
                                Date = fromdate,
                                IsCheckin = false,
                                CreatedOn = DateTime.UtcNow,
                                CreatedBy = timelogView.EmployeeId,
                            };
                            await _AttendanceRepository.AddAsync(empAttendanceAdd);
                            await _AttendanceRepository.SaveChangesAsync();
                            attendance_Id = empAttendanceAdd.AttendanceId;
                            if (attendance_Id > 0)
                            {
                                bool isattendanceDetailsExists = _AttendanceDetailRepository.GetAttendanceDetailByAttendanceId(attendance_Id, timelogView.CheckinTime, timelogView.CheckoutTime, timelogView.AttendanceDetailId, false);
                                if (!isattendanceDetailsExists)
                                {
                                    AttendanceDetail attendanceDetailAdd = new AttendanceDetail
                                    {
                                        AttendanceId = attendance_Id,
                                        CheckinTime = timelogView?.CheckinTime,
                                        CheckoutTime = timelogView?.CheckoutTime,
                                        CreatedOn = DateTime.UtcNow,
                                        CreatedBy = timelogView?.EmployeeId,
                                        Reason = timelogView?.Reason,
                                        Status = "Pending",
                                        Isregularize = timelogView?.Isregularize,
                                        TotalHours = totalHours,
                                        ManagerId = timelogView?.ApproverManagerId
                                    };
                                    await _AttendanceDetailRepository.AddAsync(attendanceDetailAdd);
                                    await _AttendanceDetailRepository.SaveChangesAsync();
                                    AttendanceDetailId = attendanceDetailAdd.AttendanceDetailId;
                                }
                                else
                                {
                                    AttendanceDetailId = -1;
                                }
                            }
                        }
                    }
                    else
                    {
                        AttendanceDetailId = -2;
                    }
                }
                else
                {
                    AttendanceDetailId = -3;
                }

                return AttendanceDetailId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        #endregion
        #region Get Attentance Details by EmployeeId and Date
        public List<SharedLibraries.Models.Attendance.Attendance> GetAttendanceDetailByIdAndDate(int employeeId, DateTime fromDate, DateTime toDate)
        {
            return _AttendanceRepository.GetAttendanceDetailByIdAndDate(employeeId, fromDate, toDate);
        }
        #endregion

        #region Get Employee Time and Weekend Definition by shiftID
        public TimeandWeekendDefinitionView GetEmployeeTimeandWeekendDefinitionbyShiftID(int shiftID)
        {
            TimeandWeekendDefinitionView timeandweek = new TimeandWeekendDefinitionView()
            {
                WeekEndNameList = _shiftWeekendDefinitionRepository.GetShiftWeekendNameByShiftID(shiftID),
                ShiftTime = _timeDefinitionRepository.GetShiftTimeDefinitionByShiftID(shiftID)

            };
            return timeandweek;
        }
        #endregion
        #region Employee Time Log Approve and Reject
        public async Task<string> TimeLogApproveOrReject(TimeLogApproveOrRejectView timeLogApproveOrRejectView)
        {
            //int attendance_Id = 0;
            //int AttendanceDetailId = 0;
            string previousStatus = null;
            AttendanceDetail attendanceDetail = new();
            if (timeLogApproveOrRejectView.EmployeeId > 0 && timeLogApproveOrRejectView.AttendanceDetailId > 0) attendanceDetail = _AttendanceDetailRepository.GetAttendanceDetailByAttendanceDetailId(timeLogApproveOrRejectView.AttendanceDetailId);
            if (attendanceDetail != null)
            {
                if (timeLogApproveOrRejectView.IsApproveOrCancel.ToLower() == "approved")
                {
                    attendanceDetail.Status = timeLogApproveOrRejectView?.IsApproveOrCancel;
                    attendanceDetail.RejectReason = timeLogApproveOrRejectView?.RejectReason;
                }
                else if (timeLogApproveOrRejectView.IsApproveOrCancel.ToLower() == "rejected")
                {
                    attendanceDetail.Status = timeLogApproveOrRejectView?.IsApproveOrCancel;
                    attendanceDetail.RejectReason = timeLogApproveOrRejectView?.RejectReason;
                }
                else
                {
                    previousStatus = attendanceDetail.Status;
                    attendanceDetail.Status = timeLogApproveOrRejectView?.IsApproveOrCancel;
                    attendanceDetail.RejectReason = timeLogApproveOrRejectView?.RejectReason;
                }
                //attendanceDetail.ManagerId = timeLogApproveOrRejectView?.ApproverManagerId;
                _AttendanceDetailRepository.Update(attendanceDetail);
                await _AttendanceDetailRepository.SaveChangesAsync();
                //AttendanceDetailId = attendanceDetail.AttendanceDetailId;
                //attendance_Id = attendanceDetail.AttendanceId;
                if (timeLogApproveOrRejectView?.IsApproveOrCancel?.ToLower() == "approved")
                {
                    SharedLibraries.Models.Attendance.Attendance empAttendance = new SharedLibraries.Models.Attendance.Attendance();
                    empAttendance = _AttendanceRepository.GetAttendanceDetailByAttendaceId(attendanceDetail.AttendanceId, timeLogApproveOrRejectView.EmployeeId);

                    if (empAttendance != null)
                    {
                        empAttendance.TotalHours = empAttendance.TotalHours == null ? attendanceDetail.TotalHours : (empAttendance.TotalHours + attendanceDetail.TotalHours);
                        //empAttendance.BreakHours = OverallBreakhourse;
                        empAttendance.ModifiedOn = DateTime.UtcNow;
                        empAttendance.ModifiedBy = timeLogApproveOrRejectView.ModifiedBy;
                        _AttendanceRepository.Update(empAttendance);
                        await _AttendanceRepository.SaveChangesAsync();
                    }

                    //if (attendance_Id > 0)
                    //{
                    //    List<AttendanceDetail> attendanceDetailList = new();
                    //    attendanceDetailList = _AttendanceDetailRepository.GetAttendanceDetailListByAttendanceId(attendance_Id);
                    //    if (attendanceDetailList != null && attendanceDetailList.Count > 0)
                    //    {
                    //        //int i = 0;

                    //        //TimeSpan OverallTotalhourse = new TimeSpan(0, 0, 0);
                    //        //TimeSpan OverallBreakhourse = new TimeSpan(0, 0, 0);
                    //        //foreach (AttendanceDetail attenDetail in attendanceDetailList)
                    //        //{
                    //        //    AttendanceDetail attendanceDetails = new();
                    //        //    if (attenDetail.CheckinTime != null && attenDetail.CheckoutTime != null)
                    //        //    {

                    //        //        TimeSpan TotalhourseDifference = (attenDetail.CheckoutTime - attenDetail.CheckinTime).Value;
                    //        //        attenDetail.TotalHours = TotalhourseDifference;
                    //        //        OverallTotalhourse += TotalhourseDifference;
                    //        //    }
                    //        //    if (i > 0)
                    //        //    {
                    //        //        attendanceDetails = attendanceDetailList[i - 1];
                    //        //        if (attendanceDetailList[i - 1].CheckoutTime != null && attendanceDetailList[i].CheckinTime != null)
                    //        //        {
                    //        //            TimeSpan breakTimeDifference = (attendanceDetailList[i].CheckinTime - attendanceDetailList[i - 1].CheckoutTime).Value;
                    //        //            attendanceDetails.BreakHours = breakTimeDifference;
                    //        //            OverallBreakhourse += breakTimeDifference;
                    //        //        }
                    //        //        if (i == (attendanceDetailList.Count - 1))
                    //        //        { attenDetail.BreakHours = null; }
                    //        //        _AttendanceDetailRepository.Update(attendanceDetails);
                    //        //        await _AttendanceDetailRepository.SaveChangesAsync();
                    //        //    }
                    //        //    i++;
                    //        //    _AttendanceDetailRepository.Update(attenDetail);
                    //        //    //_AttendanceDetailRepository.Update(attendanceDetails);
                    //        //    await _AttendanceDetailRepository.SaveChangesAsync();
                    //        //}

                    //        SharedLibraries.Models.Attendance.Attendance empAttendance = new SharedLibraries.Models.Attendance.Attendance();
                    //        empAttendance = _AttendanceRepository.GetAttendanceDetailByAttendaceId(attendance_Id, timeLogApproveOrRejectView.EmployeeId);

                    //        if (empAttendance != null)
                    //        {
                    //            empAttendance.TotalHours = OverallTotalhourse;
                    //            //empAttendance.BreakHours = OverallBreakhourse;
                    //            empAttendance.ModifiedOn = DateTime.UtcNow;
                    //            empAttendance.ModifiedBy = timeLogApproveOrRejectView.ModifiedBy;
                    //            _AttendanceRepository.Update(empAttendance);
                    //            await _AttendanceRepository.SaveChangesAsync();
                    //        }
                    //    }
                    //}
                }
                else if (timeLogApproveOrRejectView?.IsApproveOrCancel?.ToLower() == "cancelled" && previousStatus?.ToLower() == "approved")
                {
                    SharedLibraries.Models.Attendance.Attendance empAttendance = new SharedLibraries.Models.Attendance.Attendance();
                    empAttendance = _AttendanceRepository.GetAttendanceDetailByAttendaceId(attendanceDetail.AttendanceId, timeLogApproveOrRejectView.EmployeeId);

                    if (empAttendance != null)
                    {
                        empAttendance.TotalHours = empAttendance.TotalHours == null ? null : (empAttendance.TotalHours - attendanceDetail.TotalHours);
                        //empAttendance.BreakHours = OverallBreakhourse;
                        empAttendance.ModifiedOn = DateTime.UtcNow;
                        empAttendance.ModifiedBy = timeLogApproveOrRejectView.ModifiedBy;
                        _AttendanceRepository.Update(empAttendance);
                        await _AttendanceRepository.SaveChangesAsync();
                    }
                }
                //else
                //{
                //    AttendanceDetailId = -1;
                //}
            }
            return timeLogApproveOrRejectView.IsApproveOrCancel;

        }
        #endregion
        public List<TeamLeaveView> GetEmployeeRegularizationList(ReportingManagerTeamLeaveView managerTeamLeaveView)
        {
            return _AttendanceDetailRepository.GetEmployeeRegularizationList(managerTeamLeaveView);
        }
        public EmployeeShiftDetailsView GetDefaultShiftId()
        {
            return _shiftDetailsRepository.GetDefaultShiftId();
        }
        #region 
        public List<AttendaceDetails> GetAttendanceDetailByDate(int employeeId, DateTime fromDate, DateTime toDate)
        {
            return _AttendanceRepository.GetAttendanceDetailByDate(employeeId, fromDate, toDate);
        }
        #endregion
        #region 
        public List<ApplyLeavesView> GetEmployeeRegularizationById(int employeeId, DateTime FromDate, DateTime ToDate)
        {
            return _AttendanceDetailRepository.GetEmployeeRegularizationById(employeeId, FromDate, ToDate);
        }
        #endregion
        #region Delete Regularization
        public async Task<bool> DeleteRegularizationByAttendanceDetailId(int attendanceDetailId)
        {
            try
            {
                if (attendanceDetailId > 0)
                {
                    AttendanceDetail attendanceDetail = _AttendanceDetailRepository.GetAttendanceDetailByAttendanceDetailId(attendanceDetailId);
                    if (attendanceDetail != null && attendanceDetail?.AttendanceDetailId > 0)
                    {
                        _AttendanceDetailRepository.Delete(attendanceDetail);
                        await _AttendanceDetailRepository.SaveChangesAsync();
                    }
                    if (attendanceDetail?.Status?.ToLower() == "approved")
                    {
                        SharedLibraries.Models.Attendance.Attendance empAttendance = new SharedLibraries.Models.Attendance.Attendance();
                        empAttendance = _AttendanceRepository.GetAttendanceDetailById(attendanceDetail.AttendanceId);

                        if (empAttendance != null)
                        {
                            empAttendance.TotalHours = empAttendance.TotalHours == null ? null : (empAttendance.TotalHours - attendanceDetail.TotalHours);
                            empAttendance.ModifiedOn = DateTime.UtcNow;
                            empAttendance.ModifiedBy = attendanceDetail.CreatedBy;
                            _AttendanceRepository.Update(empAttendance);
                            await _AttendanceRepository.SaveChangesAsync();
                        }
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
        public AttendaceDetails GetAttendanceHoursByDate(int employeeId, DateTime fromDate)
        {
            return _AttendanceRepository.GetAttendanceHoursByDate(employeeId, fromDate);
        }
        #region Update Shift Status 
        public async Task<bool> UpdateShiftStatus(int shiftId, bool isEnabled, int updatedBy)
        {
            try
            {
                ShiftDetails shiftDetails = _shiftDetailsRepository.GetByID(shiftId);
                if (shiftDetails != null)
                {
                    shiftDetails.IsActive = isEnabled;
                    shiftDetails.ModifiedOn = DateTime.UtcNow;
                    shiftDetails.ModifiedBy = updatedBy;
                    _shiftDetailsRepository.Update(shiftDetails);
                    await _shiftDetailsRepository.SaveChangesAsync();
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
        #region Get shift details by id
        public EmployeeShiftDetailsView GetDefaultShiftDetailsById(int shiftId)
        {
            return _shiftDetailsRepository.GetDefaultShiftDetailsById(shiftId);
        }
        #endregion
        #region Get Shift Weekend Detais
        public EmployeeShiftDetailView GetShiftDetails(int ShiftId)
        {
            return _AttendanceDetailRepository.GetShiftDetails(ShiftId);
        }
        #endregion
        public EmployeeShiftDetailsListView GetShiftDetailsList()
        {
            EmployeeShiftDetailsListView shiftDetailsListView = new();
            shiftDetailsListView.employeeShifts = _shiftDetailsRepository.GetShiftDetails();
            shiftDetailsListView.DefaultShiftView = _shiftDetailsRepository.GetDefaultShiftDetails();
            return shiftDetailsListView;
        }
        public List<ShiftViewDetails> GetShiftWeekendDetailsList()
        {
            return _shiftDetailsRepository.GetShiftWeekendDetailsList();
        }
        #region 
        public List<ShiftTimeandWeekendView> GetEmployeeTimeandWeekendbyShiftID()
        {
            return _shiftWeekendDefinitionRepository.GetEmployeeTimeandWeekendbyShiftID();
        }
        #endregion

        #region Insert And Update Absent Setting
        public async Task<int> InsertAndUpdateAbsentSetting(AbsentSettingsView absentSettingsView)
        {
            try
            {
                int AbsentSettingId = 0;
                AbsentSetting absentSetting = new();
                if (absentSettingsView.AbsentSettingId != 0) absentSetting = _AbsentSettingRepository.GetAbsentSettingById(absentSettingsView.AbsentSettingId);
                if (absentSetting != null)
                {
                    absentSetting.Gender_Male_Applicable = absentSettingsView.Gender_Male_Applicable;
                    absentSetting.Gender_Female_Applicable = absentSettingsView.Gender_Female_Applicable;
                    absentSetting.Gender_Others_Applicable = absentSettingsView.Gender_Others_Applicable;
                    absentSetting.MaritalStatus_Single_Applicable = absentSettingsView.MaritalStatus_Single_Applicable;
                    absentSetting.MaritalStatus_Married_Applicable = absentSettingsView.MaritalStatus_Married_Applicable;
                    absentSetting.Gender_Male_Exception = absentSettingsView.Gender_Male_Exception;
                    absentSetting.Gender_Female_Exception = absentSettingsView.Gender_Female_Exception;
                    absentSetting.Gender_Others_Exception = absentSettingsView.Gender_Others_Exception;
                    absentSetting.MaritalStatus_Single_Exception = absentSettingsView.MaritalStatus_Single_Exception;
                    absentSetting.MaritalStatus_Married_Exception = absentSettingsView.MaritalStatus_Married_Exception;
                    //absentSetting.Type = "AbsentApplicable";
                    if (absentSettingsView.AbsentSettingId == 0)
                    {
                        absentSetting.CreatedOn = DateTime.UtcNow;
                        absentSetting.CreatedBy = absentSettingsView.CreatedBy;
                        await _AbsentSettingRepository.AddAsync(absentSetting);
                        await _AbsentSettingRepository.SaveChangesAsync();
                        AbsentSettingId = absentSetting.AbsentSettingId;
                    }
                    else
                    {
                        absentSetting.ModifiedOn = DateTime.UtcNow;
                        absentSetting.ModifiedBy = absentSettingsView.ModifiedBy;
                        _AbsentSettingRepository.Update(absentSetting);
                        AbsentSettingId = absentSetting.AbsentSettingId;
                        await _AbsentSettingRepository.SaveChangesAsync();
                        // Delete Employee
                        var employeeList = _AbsentEmployeeRepository.GetAbsentEmployeeByAbsentSettingId(absentSettingsView.AbsentSettingId);
                        if (employeeList?.Count > 0)
                        {
                            foreach (AbsentEmployee employee in employeeList)
                            {
                                _AbsentEmployeeRepository.Delete(employee);
                                await _AbsentEmployeeRepository.SaveChangesAsync();
                            }
                            
                        }
                        // delete Department
                        var departmentList = _AbsentDepartmentRepository.GetAbsentDepartmentByAbsentSettingId(absentSettingsView.AbsentSettingId);
                        if (departmentList?.Count > 0)
                        {
                            foreach (AbsentDepartment department in departmentList)
                            {
                                _AbsentDepartmentRepository.Delete(department);
                                await _AbsentDepartmentRepository.SaveChangesAsync();
                            }
                            
                        }
                        // delete Designation
                        var designationList = _AbsentDesignationRepository.GetAbsentDesignationByAbsentSettingId(absentSettingsView.AbsentSettingId);
                        if (designationList?.Count > 0)
                        {
                            foreach (AbsentDesignation designation in designationList)
                            {
                                _AbsentDesignationRepository.Delete(designation);
                                await _AbsentDesignationRepository.SaveChangesAsync();
                            }
                            
                        }
                        // delete Location
                        var locationList = _AbsentLocationRepository.GetAbsentLocationByAbsentSettingId(absentSettingsView.AbsentSettingId);
                        if (locationList?.Count > 0)
                        {
                            foreach (AbsentLocation location in locationList)
                            {
                                _AbsentLocationRepository.Delete(location);
                                await _AbsentLocationRepository.SaveChangesAsync();
                            }
                            
                        }
                        // delete Role
                        var roleList = _AbsentRoleRepository.GetAbsentRoleByAbsentSettingId(absentSettingsView.AbsentSettingId);
                        if (roleList?.Count > 0)
                        {
                            foreach (AbsentRole role in roleList)
                            {
                                _AbsentRoleRepository.Delete(role);
                                await _AbsentRoleRepository.SaveChangesAsync();
                            }
                            
                        }
                        // delete Employee Type
                        var employeeTypeList = _AbsentEmployeeTypeRepository.GetAbsentEmployeeTypeByAbsentSettingId(absentSettingsView.AbsentSettingId);
                        if (employeeTypeList?.Count > 0)
                        {
                            foreach (AbsentEmployeeType employeeType in employeeTypeList)
                            {
                                _AbsentEmployeeTypeRepository.Delete(employeeType);
                                await _AbsentEmployeeTypeRepository.SaveChangesAsync();
                            }
                            
                        }
                        // delete Probation Status
                        var probationStatusList = _AbsentProbationStatusRepository.GetAbsentProbationStatusByAbsentSettingId(absentSettingsView.AbsentSettingId);
                        if (probationStatusList?.Count > 0)
                        {
                            foreach (AbsentProbationStatus probationStatus in probationStatusList)
                            {
                                _AbsentProbationStatusRepository.Delete(probationStatus);
                                await _AbsentProbationStatusRepository.SaveChangesAsync();
                            }
                            
                        }
                    }

                    //Add Exception
                    //if (absentSettingsView?.AbsentExceptionView != null)
                    //{
                    //    AbsentSetting leaveException = new();
                    //    if (absentSettingsView?.AbsentSettingId != 0) leaveException = _AbsentSettingRepository.GetAbsentExceptionSettingById(absentSettingsView.AbsentSettingId);
                    //    if (leaveException != null)
                    //    {
                    //        //leaveException.AbsentSettingId = AbsentSettingId;
                    //        leaveException.Gender_Male_Exception = absentSettingsView.AbsentExceptionView.Gender_Male_Exception;
                    //        leaveException.Gender_Female_Exception = absentSettingsView.AbsentExceptionView.Gender_Female_Exception;
                    //        leaveException.Gender_Others_Exception = absentSettingsView.AbsentExceptionView.Gender_Others_Exception;
                    //        leaveException.MaritalStatus_Single_Exception = absentSettingsView.AbsentExceptionView.MaritalStatus_Single_Exception;
                    //        leaveException.MaritalStatus_Married_Exception = absentSettingsView.AbsentExceptionView.MaritalStatus_Married_Exception;
                    //        leaveException.Type = "AbsentException";

                    //        if (absentSettingsView.AbsentSettingId == 0)
                    //        {
                    //            //absentSetting.CreatedOn = DateTime.UtcNow;
                    //            //absentSetting.CreatedBy = absentSettingsView.CreatedBy;
                    //            await _AbsentSettingRepository.AddAsync(leaveException);
                    //            await _AbsentSettingRepository.SaveChangesAsync();
                    //            //AbsentSettingId = absentSetting.AbsentSettingId;
                    //        }
                    //        else
                    //        {
                    //            _AbsentSettingRepository.Update(leaveException);
                    //            //AbsentSettingId = absentSetting.AbsentSettingId;
                    //            await _AbsentSettingRepository.SaveChangesAsync();
                    //        }
                    //    }
                    //}
                    //Add Applicable Department
                    if (absentSettingsView?.AbsentApplicableDepartmentId?.Count > 0)
                    {
                        foreach (int applicableDepartmentId in absentSettingsView?.AbsentApplicableDepartmentId)
                        {
                            AbsentDepartment applicableDepartment = new();
                            applicableDepartment.AbsentSettingId = AbsentSettingId;
                            applicableDepartment.AbsentApplicableDepartmentId = applicableDepartmentId;
                            applicableDepartment.AbsentExceptionDepartmentId = 0;
                            applicableDepartment.Type = "AbsentApplicable";
                            applicableDepartment.CreatedOn = DateTime.UtcNow;
                            applicableDepartment.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentDepartmentRepository.AddAsync(applicableDepartment);
                            await _AbsentDepartmentRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Exception Department
                    if (absentSettingsView?.AbsentExceptionDepartmentId?.Count > 0)
                    {
                        foreach (int leaveExceptionDepartmentId in absentSettingsView?.AbsentExceptionDepartmentId)
                        {
                            AbsentDepartment exceptionDepartment = new();
                            exceptionDepartment.AbsentSettingId = AbsentSettingId;
                            exceptionDepartment.AbsentExceptionDepartmentId = leaveExceptionDepartmentId;
                            exceptionDepartment.AbsentApplicableDepartmentId = 0;
                            exceptionDepartment.Type = "AbsentException";
                            exceptionDepartment.CreatedOn = DateTime.UtcNow;
                            exceptionDepartment.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentDepartmentRepository.AddAsync(exceptionDepartment);
                            await _AbsentDepartmentRepository.SaveChangesAsync();
                        }
                        
                    }
                    //Add Applicable Designation
                    if (absentSettingsView?.AbsentApplicableDesignationId?.Count > 0)
                    {
                        foreach (int applicableDesignationId in absentSettingsView?.AbsentApplicableDesignationId)
                        {
                            AbsentDesignation applicableDesignation = new();
                            applicableDesignation.AbsentSettingId = AbsentSettingId;
                            applicableDesignation.AbsentApplicableDesignationId = applicableDesignationId;
                            applicableDesignation.AbsentExceptionDesignationId = 0;
                            applicableDesignation.Type = "AbsentApplicable";
                            applicableDesignation.CreatedOn = DateTime.UtcNow;
                            applicableDesignation.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentDesignationRepository.AddAsync(applicableDesignation);
                            await _AbsentDesignationRepository.SaveChangesAsync();
                        }
                        
                    }
                    //Add Exception Designation
                    if (absentSettingsView?.AbsentExceptionDesignationId?.Count > 0)
                    {
                        foreach (int execptionDesignationId in absentSettingsView?.AbsentExceptionDesignationId)
                        {
                            AbsentDesignation exceptionDesignation = new();
                            exceptionDesignation.AbsentSettingId = AbsentSettingId;
                            exceptionDesignation.AbsentExceptionDesignationId = execptionDesignationId;
                            exceptionDesignation.AbsentApplicableDesignationId = 0;
                            exceptionDesignation.Type = "AbsentException";
                            exceptionDesignation.CreatedOn = DateTime.UtcNow;
                            exceptionDesignation.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentDesignationRepository.AddAsync(exceptionDesignation);
                            await _AbsentDesignationRepository.SaveChangesAsync();
                        }
                        
                    }
                    //Add Applicable Location
                    if (absentSettingsView?.AbsentApplicableLocationId?.Count > 0)
                    {
                        foreach (int applicableLocationId in absentSettingsView?.AbsentApplicableLocationId)
                        {
                            AbsentLocation applicableLocation = new();
                            applicableLocation.AbsentSettingId = AbsentSettingId;
                            applicableLocation.AbsentApplicableLocationId = applicableLocationId;
                            applicableLocation.AbsentExceptionLocationId = 0;
                            applicableLocation.Type = "AbsentApplicable";
                            applicableLocation.CreatedOn = DateTime.UtcNow;
                            applicableLocation.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentLocationRepository.AddAsync(applicableLocation);
                            await _AbsentLocationRepository.SaveChangesAsync();
                        }
                        
                    }
                    //Add Exception Location
                    if (absentSettingsView?.AbsentExceptionLocationId?.Count > 0)
                    {
                        foreach (int exceptionLocationId in absentSettingsView?.AbsentExceptionLocationId)
                        {
                            AbsentLocation exceptionLocation = new();
                            exceptionLocation.AbsentSettingId = AbsentSettingId;
                            exceptionLocation.AbsentExceptionLocationId = exceptionLocationId;
                            exceptionLocation.AbsentApplicableLocationId = 0;
                            exceptionLocation.Type = "AbsentException";
                            exceptionLocation.CreatedOn = DateTime.UtcNow;
                            exceptionLocation.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentLocationRepository.AddAsync(exceptionLocation);
                            await _AbsentLocationRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Applicable Role
                    if (absentSettingsView?.AbsentApplicableRoleId?.Count > 0)
                    {
                        foreach (int applicableRoleId in absentSettingsView?.AbsentApplicableRoleId)
                        {
                            AbsentRole applicableRole = new();
                            applicableRole.AbsentSettingId = AbsentSettingId;
                            applicableRole.AbsentApplicableRoleId = applicableRoleId;
                            applicableRole.AbsentExceptionRoleId = 0;
                            applicableRole.Type = "AbsentApplicable";
                            applicableRole.CreatedOn = DateTime.UtcNow;
                            applicableRole.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentRoleRepository.AddAsync(applicableRole);
                            await _AbsentRoleRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Exception Role
                    if (absentSettingsView?.AbsentExceptionRoleId?.Count > 0)
                    {
                        foreach (int exceptionRoleId in absentSettingsView?.AbsentExceptionRoleId)
                        {
                            AbsentRole exceptionRole = new();
                            exceptionRole.AbsentSettingId = AbsentSettingId;
                            exceptionRole.AbsentExceptionRoleId = exceptionRoleId;
                            exceptionRole.AbsentApplicableRoleId = 0;
                            exceptionRole.Type = "AbsentException";
                            exceptionRole.CreatedOn = DateTime.UtcNow;
                            exceptionRole.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentRoleRepository.AddAsync(exceptionRole);
                            await _AbsentRoleRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Applicable Employee
                    if (absentSettingsView?.AbsentApplicableEmployeeId?.Count > 0)
                    {
                        foreach (int applicableEmployeeId in absentSettingsView?.AbsentApplicableEmployeeId)
                        {
                            AbsentEmployee employeeApplicable = new();
                            employeeApplicable.AbsentSettingId = AbsentSettingId;
                            employeeApplicable.AbsentApplicableEmployeeId = applicableEmployeeId;
                            employeeApplicable.AbsentExceptionEmployeeId = 0;
                            employeeApplicable.Type = "AbsentApplicable";
                            employeeApplicable.CreatedOn = DateTime.UtcNow;
                            employeeApplicable.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentEmployeeRepository.AddAsync(employeeApplicable);
                            await _AbsentEmployeeRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Exception Employee 
                    if (absentSettingsView?.AbsentExceptionEmployeeId?.Count > 0)
                    {
                        foreach (int exceptionEmployeeId in absentSettingsView?.AbsentExceptionEmployeeId)
                        {
                            AbsentEmployee exceptionEmployee = new();
                            exceptionEmployee.AbsentSettingId = AbsentSettingId;
                            exceptionEmployee.AbsentExceptionEmployeeId = exceptionEmployeeId;
                            exceptionEmployee.AbsentApplicableEmployeeId = 0;
                            exceptionEmployee.Type = "AbsentException";
                            exceptionEmployee.CreatedOn = DateTime.UtcNow;
                            exceptionEmployee.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentEmployeeRepository.AddAsync(exceptionEmployee);
                            await _AbsentEmployeeRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Applicable Employee Type
                    if (absentSettingsView?.AbsentApplicableEmployeeTypeId?.Count > 0)
                    {
                        foreach (int employeeTypeId in absentSettingsView?.AbsentApplicableEmployeeTypeId)
                        {
                            AbsentEmployeeType employeeType = new();
                            employeeType.AbsentSettingId = AbsentSettingId;
                            employeeType.AbsentApplicableEmployeeTypeId = employeeTypeId;
                            employeeType.AbsentExceptionEmployeeTypeId = 0;
                            employeeType.Type = "AbsentApplicable";
                            employeeType.CreatedOn = DateTime.UtcNow;
                            employeeType.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentEmployeeTypeRepository.AddAsync(employeeType);
                            await _AbsentEmployeeTypeRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Exception Employee Type
                    if (absentSettingsView?.AbsentExceptionEmployeeTypeId?.Count > 0)
                    {
                        foreach (int exceptionEmployeeTypeId in absentSettingsView?.AbsentExceptionEmployeeTypeId)
                        {
                            AbsentEmployeeType exceptionEmployeeType = new();
                            exceptionEmployeeType.AbsentSettingId = AbsentSettingId;
                            exceptionEmployeeType.AbsentExceptionEmployeeTypeId = exceptionEmployeeTypeId;
                            exceptionEmployeeType.AbsentApplicableEmployeeTypeId = 0;
                            exceptionEmployeeType.Type = "AbsentException";
                            exceptionEmployeeType.CreatedOn = DateTime.UtcNow;
                            exceptionEmployeeType.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentEmployeeTypeRepository.AddAsync(exceptionEmployeeType);
                            await _AbsentEmployeeTypeRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Probation Status
                    if (absentSettingsView?.AbsentApplicableProbationStatusId?.Count > 0)
                    {
                        foreach (int probationStatusId in absentSettingsView?.AbsentApplicableProbationStatusId)
                        {
                            AbsentProbationStatus probationStatus = new();
                            probationStatus.AbsentSettingId = AbsentSettingId;
                            probationStatus.AbsentApplicableProbationStatusId = probationStatusId;
                            probationStatus.AbsentExceptionProbationStatusId = 0;
                            probationStatus.Type = "AbsentApplicable";
                            probationStatus.CreatedOn = DateTime.UtcNow;
                            probationStatus.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentProbationStatusRepository.AddAsync(probationStatus);
                            await _AbsentProbationStatusRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Exception Probation Status
                    if (absentSettingsView?.AbsentExceptionProbationStatusId?.Count > 0)
                    {
                        foreach (int exceptionProbationStatus in absentSettingsView?.AbsentExceptionProbationStatusId)
                        {
                            AbsentProbationStatus excepProbationStatus = new();
                            excepProbationStatus.AbsentSettingId = AbsentSettingId;
                            excepProbationStatus.AbsentExceptionProbationStatusId = exceptionProbationStatus;
                            excepProbationStatus.AbsentApplicableProbationStatusId = 0;
                            excepProbationStatus.Type = "AbsentException";
                            excepProbationStatus.CreatedOn = DateTime.UtcNow;
                            excepProbationStatus.CreatedBy = absentSettingsView.CreatedBy;
                            await _AbsentProbationStatusRepository.AddAsync(excepProbationStatus);
                            await _AbsentProbationStatusRepository.SaveChangesAsync();
                        }
                        
                    }
                    // Add Restrictions
                    if (absentSettingsView?.AbsentRestriction != null)
                    {
                        AbsentRestrictions absentRestriction = new();
                        if (absentSettingsView.AbsentSettingId != 0) absentRestriction = _AbsentRestrictionRepository.GetAbsentRestrictionsById(absentSettingsView.AbsentSettingId);
                        if (absentRestriction != null)
                        {
                            absentRestriction.WeekendsBetweenAttendacePeriod = absentSettingsView?.AbsentRestriction.WeekendsBetweenAttendacePeriod;
                            absentRestriction.HolidaysBetweenAttendancePeriod = absentSettingsView?.AbsentRestriction.HolidaysBetweenAttendancePeriod;
                            if (absentSettingsView?.AbsentSettingId == 0)
                            {
                                absentRestriction.CreatedOn = DateTime.UtcNow;
                                absentRestriction.CreatedBy = absentSettingsView.CreatedBy;
                                absentRestriction.AbsentSettingId = AbsentSettingId;
                                await _AbsentRestrictionRepository.AddAsync(absentRestriction);
                            }
                            else
                            {
                                absentRestriction.ModifiedOn = DateTime.UtcNow;
                                absentRestriction.ModifiedBy = absentSettingsView.CreatedBy;
                                absentRestriction.AbsentSettingId = AbsentSettingId;
                                _AbsentRestrictionRepository.Update(absentRestriction);
                            }
                            await _AbsentRestrictionRepository.SaveChangesAsync();
                        }
                    }
                }
                return AbsentSettingId;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region
        public AbsentSettingsView GetAbsentSettingDetails()
        {
            try
            {
                AbsentSettingsView absentSettingsView = _AbsentSettingRepository.GetAbsentSetting();
                absentSettingsView.AbsentApplicableDepartmentId = _AbsentDepartmentRepository.GetAbsentApplicableDepartment(absentSettingsView?.AbsentSettingId==null?0:(int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentExceptionDepartmentId = _AbsentDepartmentRepository.GetAbsentExceptionDepartment(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentApplicableDesignationId = _AbsentDesignationRepository.GetAbsentApplicableDesignation(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentExceptionDesignationId = _AbsentDesignationRepository.GetAbsentExceptionDesignation(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentApplicableLocationId = _AbsentLocationRepository.GetAbsentApplicableLocation(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentExceptionLocationId = _AbsentLocationRepository.GetAbsentExceptionLocation(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentApplicableRoleId = _AbsentRoleRepository.GetAbsentApplicableRole(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentExceptionRoleId = _AbsentRoleRepository.GetAbsentExceptionRole(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentApplicableEmployeeId = _AbsentEmployeeRepository.GetAbsentApplicableEmployee(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentExceptionEmployeeId = _AbsentEmployeeRepository.GetAbsentExceptionEmployee(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentApplicableEmployeeTypeId = _AbsentEmployeeTypeRepository.GetAbsentApplicableEmployeeType(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentExceptionEmployeeTypeId = _AbsentEmployeeTypeRepository.GetAbsentExceptionEmployeeType(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentApplicableProbationStatusId = _AbsentProbationStatusRepository.GetAbsentApplicableProbationStatus(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentExceptionProbationStatusId = _AbsentProbationStatusRepository.GetAbsentExceptionProbationStatus(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentSettingsView.AbsentRestriction = _AbsentRestrictionRepository.GetAbsentRestriction();
                return absentSettingsView;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Get Absent Applicable
        public EmployeeLeaveApplicableView GetAbsentApplicable(EmployeeDetailsForLeaveView employeedetails)
        {
            EmployeeLeaveApplicableView applicable = new EmployeeLeaveApplicableView();
            applicable.IsCreteriaMached = true;
            try
            {
                List<AbsentDepartment> absentDepartments = new();
                List<AbsentDesignation> absentDesignations = new();
                List<AbsentLocation> absentLocation = new();
                List<AbsentRole> absentRole = new();
                List<AbsentEmployeeType> absentEmployeeType = new();
                List<AbsentProbationStatus> absentProbationStatuse = new();
                AbsentSettingsView absentSettingsView = _AbsentSettingRepository.GetAbsentSetting();
                absentDepartments = _AbsentDepartmentRepository.GetApplicableAbsentDepartment(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentDesignations = _AbsentDesignationRepository.GetApplicableAbsentDesignation(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentLocation = _AbsentLocationRepository.GetApplicableAbsentLocation(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentRole = _AbsentRoleRepository.GetApplicableAbsentRole(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentEmployeeType = _AbsentEmployeeTypeRepository.GetApplicableAbsentEmployeeType(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentProbationStatuse = _AbsentProbationStatusRepository.GetApplicableAbsentProbationStatus(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);

                if (absentDepartments.Any(x => x.AbsentApplicableDepartmentId != 0) && !absentDepartments.Any(m => employeedetails.DepartmentID == m.AbsentApplicableDepartmentId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (absentDesignations.Any(x => x.AbsentApplicableDesignationId != 0) && !absentDesignations.Any(m => employeedetails.DesignationID == m.AbsentApplicableDesignationId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (absentLocation.Any(x => x.AbsentApplicableLocationId != 0) && !absentLocation.Any(m => employeedetails.LocationID == m.AbsentApplicableLocationId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (absentRole.Any(x => x.AbsentApplicableRoleId != 0) && !absentRole.Any(m => employeedetails.SystemRoleID == m.AbsentApplicableRoleId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (absentEmployeeType.Any(x => x.AbsentApplicableEmployeeTypeId != 0) && !absentEmployeeType.Any(m => employeedetails.EmployeeTypeID == m.AbsentApplicableEmployeeTypeId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (absentProbationStatuse.Any(x => x.AbsentApplicableProbationStatusId != 0) && !absentProbationStatuse.Any(m => employeedetails.ProbationStatusID == m.AbsentApplicableProbationStatusId))
                {
                    applicable.IsApplicable = false;
                    return applicable;
                }
                if (absentRole?.Count == 0 && absentLocation?.Count == 0 && absentDesignations?.Count == 0 && absentDepartments?.Count == 0 && absentEmployeeType?.Count == 0 && absentProbationStatuse?.Count == 0)
                {
                    applicable.IsCreteriaMached = false;
                }
                applicable.IsApplicable = true;
                return applicable;
            }
            catch (Exception ex)
            {
                LoggerManager.LoggingErrorTrack(ex, "Attendance", "Attendance/GetAbsentApplicable");
            }
            applicable.IsApplicable = false;
            applicable.IsCreteriaMached = false;
            return applicable;
        }
        #endregion
        #region
        public bool GetAbsentExceptions(EmployeeDetailsForLeaveView employeedetails)
        {
            try
            {

                List<AbsentDepartment> absentDepartments = new();
                List<AbsentDesignation> absentDesignations = new();
                List<AbsentLocation> absentLocation = new();
                List<AbsentRole> absentRole = new();
                List<AbsentEmployeeType> absentEmployeeType = new();
                List<AbsentProbationStatus> absentProbationStatuse = new();
                AbsentSettingsView absentSettingsView = _AbsentSettingRepository.GetAbsentSetting();
                absentDepartments = _AbsentDepartmentRepository.GetExceptionAbsentDepartment(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentDesignations = _AbsentDesignationRepository.GetExceptionAbsentDesignation(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentLocation = _AbsentLocationRepository.GetExceptionAbsentLocation(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentRole = _AbsentRoleRepository.GetExceptionAbsentRole(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentEmployeeType = _AbsentEmployeeTypeRepository.GetExceptionAbsentEmployeeType(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);
                absentProbationStatuse = _AbsentProbationStatusRepository.GetExceptionAbsentProbationStatus(absentSettingsView?.AbsentSettingId == null ? 0 : (int)absentSettingsView?.AbsentSettingId);

                var Excedept = absentDepartments.Any(m => employeedetails?.DepartmentID == m.AbsentExceptionDepartmentId);
                var Excedesi = absentDesignations.Any(m => employeedetails?.DesignationID == m.AbsentExceptionDesignationId);
                var Exceloc = absentLocation.Any(m => employeedetails?.LocationID == m.AbsentExceptionLocationId);
                var Excerol = absentRole.Any(m => employeedetails?.SystemRoleID == m.AbsentExceptionRoleId);
                var Execemptype = absentEmployeeType.Any(m => employeedetails?.EmployeeTypeID == m.AbsentExceptionEmployeeTypeId);
                var Execprostatus = absentProbationStatuse.Any(m => employeedetails?.ProbationStatusID == m.AbsentExceptionProbationStatusId);
                bool exceptioncheck = false;

                if (absentDepartments.Any(x => x.AbsentExceptionDepartmentId != 0) && !absentDepartments.Any(m => employeedetails?.DepartmentID == m.AbsentExceptionDepartmentId))
                {
                    return false;
                }
                if (absentDesignations.Any(x => x.AbsentExceptionDesignationId != 0) && !absentDesignations.Any(m => employeedetails?.DesignationID == m.AbsentExceptionDesignationId))
                {
                    return false;
                }
                if (absentLocation.Any(x => x.AbsentExceptionLocationId != 0) && !absentLocation.Any(m => employeedetails?.LocationID == m.AbsentExceptionLocationId))
                {
                    return false;
                }
                if (absentRole.Any(x => x.AbsentExceptionRoleId != 0) && !absentRole.Any(m => employeedetails?.SystemRoleID == m.AbsentExceptionRoleId))
                {
                    return false;
                }
                if (absentEmployeeType.Any(x => x.AbsentExceptionEmployeeTypeId != 0) && !absentEmployeeType.Any(m => employeedetails?.EmployeeTypeID == m.AbsentExceptionEmployeeTypeId))
                {
                    return false;
                }
                if (absentProbationStatuse.Any(x => x.AbsentExceptionProbationStatusId != 0) && !absentProbationStatuse.Any(m => employeedetails?.ProbationStatusID == m.AbsentExceptionProbationStatusId))
                {
                    return false;
                }

                if (absentDepartments.Any(x => x.AbsentExceptionDepartmentId != 0) && absentDepartments.Any(m => employeedetails?.DepartmentID == m.AbsentExceptionDepartmentId))
                {
                    exceptioncheck = true;
                }
                if (absentDesignations.Any(x => x.AbsentExceptionDesignationId != 0) && absentDesignations.Any(m => employeedetails?.DesignationID == m.AbsentExceptionDesignationId))
                {
                    exceptioncheck = true;
                }
                else if (absentDesignations.Any(x => x.AbsentExceptionDesignationId != 0) && !absentDesignations.Any(m => employeedetails?.DesignationID == m.AbsentExceptionDesignationId))
                {
                    return false;
                }
                if (absentLocation.Any(x => x.AbsentExceptionLocationId != 0) && absentLocation.Any(m => employeedetails?.LocationID == m.AbsentExceptionLocationId))
                {
                    exceptioncheck = true;
                }
                else if (absentLocation.Any(x => x.AbsentExceptionLocationId != 0) && !absentLocation.Any(m => employeedetails?.LocationID == m.AbsentExceptionLocationId))
                {
                    return false;
                }
                if (absentRole.Any(x => x.AbsentExceptionRoleId != 0) && absentRole.Any(m => employeedetails?.SystemRoleID == m.AbsentExceptionRoleId))
                {
                    exceptioncheck = true;
                }
                else if (absentRole.Any(x => x.AbsentExceptionRoleId != 0) && !absentRole.Any(m => employeedetails?.SystemRoleID == m.AbsentExceptionRoleId))
                {
                    return false;
                }
                if (absentEmployeeType.Any(x => x.AbsentExceptionEmployeeTypeId != 0) && absentEmployeeType.Any(m => employeedetails?.EmployeeTypeID == m.AbsentExceptionEmployeeTypeId))
                {
                    exceptioncheck = true;
                }
                else if (absentEmployeeType.Any(x => x.AbsentExceptionEmployeeTypeId != 0) && !absentEmployeeType.Any(m => employeedetails?.EmployeeTypeID == m.AbsentExceptionEmployeeTypeId))
                {
                    return false;
                }
                if (absentProbationStatuse.Any(x => x.AbsentExceptionProbationStatusId != 0) && absentProbationStatuse.Any(m => employeedetails?.ProbationStatusID == m.AbsentExceptionProbationStatusId))
                {
                    exceptioncheck = true;
                }
                else if (absentProbationStatuse.Any(x => x.AbsentExceptionProbationStatusId != 0) && !absentProbationStatuse.Any(m => employeedetails?.ProbationStatusID == m.AbsentExceptionProbationStatusId))
                {
                    return false;
                }
                return exceptioncheck;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
        #region Get all active shift details
        public List<ShiftView> GetAllActiveShift()
        {
            return _shiftDetailsRepository.GetAllActiveShift();
        }
        #endregion

        public RegularizationDetailView GetEmployeeRegularizationByEmployeeId(EmployeeLeaveandRestrictionViewModel employeeLeaveandRestriction)
        {
            return _AttendanceDetailRepository.GetEmployeeRegularizationByEmployeeId(employeeLeaveandRestriction);
        }

        #region
        public List<EmployeeRequestCount> GetAttendanceRequestCount(List<int> employeeIdLists)
        {
            return _AttendanceDetailRepository.GetAttendanceRequestCount(employeeIdLists);
        }
        #endregion
    }
}