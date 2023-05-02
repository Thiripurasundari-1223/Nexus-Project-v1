using Microsoft.AspNetCore.Mvc.Filters;
using System.Reflection;
using LoggerManager = SharedLibraries.Common.LoggerManager;

namespace APIGateWay.API.Common
{
    public class ExecutedActions: IActionFilter
    {
        private readonly LoggerManager logger;
        public ExecutedActions(LoggerManager logger)
        {
            this.logger = logger;
        }
        public void OnActionExecuting(ActionExecutingContext context)
        {
            LoggerManager.LogInformation("Excuted Action -> " + context.ActionDescriptor.DisplayName.ToString(), MethodBase.GetCurrentMethod().Name, string.Empty);
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {
            // our code after action executes
        }
    }
}