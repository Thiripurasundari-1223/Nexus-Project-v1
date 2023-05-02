using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SharedLibraries
{
    public enum FormControlType
    {
        CV=1,
        EA=2,
        VA=3

    }

    public enum CRStatus
    {
        Active=1,
        Inactive=2,
        Cold=3
    }

    public enum ProjectTypes
    {
        Fixedbid=1,
        TimeandMaterial=2,
        Non_Billable=3
    }

    public enum ProbationStatus
    {
        Active = 1,
        Inactive = 2,
    }
    public enum AllowUsersToView
    {
        Other = 1
    }

    public enum BalanceToBeDisplayed
    {
        Other = 1
    }

    public enum Months
    {
        January = 1,
        February = 2,
        March = 3,
        April = 4,
        May = 5,
        June = 6,
        July = 7,
        August = 8,
        September = 9,
        October = 10,
        November = 11,
        December = 12
    }
    public enum Days
    {
        Monday = 1,
        Tuesday = 2,
        Wednesday = 3,
        Thursday = 4,
        Friday = 5,
        saturday = 6,
        Sunday = 7
    }
    public enum Weightage
    {
        Ten = 10,
        Twenty = 20,
        Thirty = 30,
        Fourty = 40,
        Fifty = 50,
        Sixty = 60,
        Seventy = 70,
        Eighty = 80,
        Ninty = 90,
        Hundred = 100
    }
    public enum Type
    {
        Percentage = 1,
        Range = 2,
        numberCount = 3
    }
    public enum UIType
    {
        Slider = 1,
        LOV = 2,
        Counter = 3
    }
    public enum Duration
    {
        Monthly = 1,
        Quarterly = 2,
        HalfYearly = 3,
        Yearly = 4
    }
    public class CommonLib
    {
        public static int GetFormControlType(FormControlType pFormControlType)
        {
            int intReturnValue = 0;

            switch (pFormControlType)
            {
                case FormControlType.CV:
                    intReturnValue = 1;
                    break;
                case FormControlType.EA:
                    intReturnValue = 2;
                    break;
                case FormControlType.VA:
                    intReturnValue = 3;
                    break;
            }
            return intReturnValue;
        }

        public static int GetCRStatus(CRStatus cRStatus)
        {

            int intReturnValue = 0;
            switch (cRStatus)
            {
                case CRStatus.Active:
                    intReturnValue = 1;
                    break;
                case CRStatus.Inactive:
                    intReturnValue = 2;
                    break;
                case CRStatus.Cold:
                    intReturnValue = 3;
                    break;
            }
            return intReturnValue;
        }

        public static DateTime GetTodayStartTime()
        {
            return DateTime.Today;
        }

        public static DateTime GetTodayEndTime()
        {
            return DateTime.Today.AddDays(1).AddTicks(-2);
        }

        public static string RemoveDuplicateFromJsonString(string jsonString)
        {
            try
            {
                return string.IsNullOrEmpty(jsonString) ? string.Empty : "[" + string.Join(",", Regex.Split(jsonString.Replace("[", "").Replace("]", ""), ",").ToList().Distinct().ToList()) + "]";
            }
            catch { 
            }
            return "";
        }
        public static int GetProbationStatus(ProbationStatus probationStatus)
        {
            int intReturnValue = 0;
            switch (probationStatus)
            {
                case ProbationStatus.Active:
                    intReturnValue = 1;
                    break;
                case ProbationStatus.Inactive:
                    intReturnValue = 2;
                    break;
            }
            return intReturnValue;
        }
        public static int GetAllowUsersToView(AllowUsersToView allowUsersToView)
        {
            int intReturnValue = 0;
            switch (allowUsersToView)
            {
                case AllowUsersToView.Other:
                    intReturnValue = 1;
                    break;
            }
            return intReturnValue;
        }
        public static int GetBalanceToBeDisplayed(BalanceToBeDisplayed balanceToBeDisplayed)
        {
            int intReturnValue = 0;
            switch (balanceToBeDisplayed)
            {
                case BalanceToBeDisplayed.Other:
                    intReturnValue = 1;
                    break;
            }
            return intReturnValue;
        }
    }
}
