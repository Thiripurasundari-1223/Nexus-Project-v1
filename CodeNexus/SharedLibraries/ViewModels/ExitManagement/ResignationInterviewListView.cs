using SharedLibraries.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLibraries.ViewModels.ExitManagement
{
    public class ResignationInterviewListView
    {
        public List<ResignationInterviewDetailView> ResignationInterviewDetailList { get;set;}
        public List<KeyWithValue> ResignationInterview { get; set; }
        public List<KeyWithValue> ReasonRelievingPosition { get; set; }
        public bool isEnableExitInterview { get; set; }
    }
}
