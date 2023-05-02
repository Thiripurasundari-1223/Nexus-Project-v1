using PowerBiReportAPI.Constants;

namespace PowerBiReportAPI.Extensions
{
    public static class PowerBIExtensions
    {
        public static string GetReportId(this string str)
        {
            var reportId = (typeof(PowerBIConstants)).GetField(str);
            if (reportId == null)
                return string.Empty;
            return (string)reportId.GetValue(null);
        }
    }
}
