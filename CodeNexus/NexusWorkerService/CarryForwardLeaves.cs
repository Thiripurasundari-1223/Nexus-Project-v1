using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using NexusWorkerService.Model;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NexusWorkerService
{
    public class CarryForwardLeaves : BackgroundService
    {
        private IConfiguration _configuration;
        public CarryForwardLeaves(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            //AccruedLeaves();

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    SerilogLogging.LogSchedulerInfo("Start  carryforward leave execution");
                    //Time when method needs to be called
                    var DailyTime = "23:00:00";
                    if (!string.IsNullOrEmpty(_configuration.GetValue<string>("ApplicationSetting:CarryForwardLeavesTime")))
                    {
                        DailyTime = _configuration.GetValue<string>("ApplicationSetting:CarryForwardLeavesTime");
                    }
                    var timeParts = DailyTime.Split(new char[1] { ':' });
                    var dateNow = DateTime.Now;
                    var date = new DateTime(dateNow.Year, dateNow.Month, dateNow.Day,
                               int.Parse(timeParts[0]), int.Parse(timeParts[1]), int.Parse(timeParts[2]));
                    TimeSpan ts;
                    if (date > dateNow)
                        ts = date - dateNow;
                    else
                    {
                        date = date.AddDays(1);
                        ts = date - dateNow;
                    }
                    SerilogLogging.LogSchedulerInfo("End carryforward leave execution");
                    //waits certan time and run the code
                    SerilogLogging.LogSchedulerInfo("Next carryforward leave Scheduler start Time :" + date.ToString());
                    await Task.Delay(ts).ContinueWith((x) => CarryForward());
                }
                catch (Exception ex)
                {
                    SerilogLogging.LogSchedulerError(ex);
                }
            }
        }

        public async Task<bool> CarryForward()
        {
            HttpClientHandler clientHandler = new HttpClientHandler();
            clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
            using (var client = new HttpClient(clientHandler))
            {
                if (!string.IsNullOrEmpty(_configuration.GetValue<string>("ApplicationSetting:BaseAPI")))
                {
                    client.BaseAddress = new Uri(_configuration.GetValue<string>("ApplicationSetting:BaseAPI"));
                    SerilogLogging.LogSchedulerInfo("Base API URL :" + _configuration.GetValue<string>("ApplicationSetting:BaseAPI"));
                    HttpResponseMessage responseTask = await client.GetAsync("Leaves/CarryForwardLeaves");
                    SerilogLogging.LogSchedulerInfo(JsonConvert.SerializeObject(responseTask.Content.ReadAsAsync<SuccessData>().Result));
                    if (responseTask.IsSuccessStatusCode)
                    {
                        SerilogLogging.LogSchedulerInfo("CarryForward Leave  successfully");
                    }
                    else
                    {
                        SerilogLogging.LogSchedulerInfo("Failed to execute CarryForward Leave alert");
                    }
                }
                else
                {
                    SerilogLogging.LogSchedulerInfo("Base API URL value is Empty, Please check Application Setting");
                }
            }
            return true;
        }
    }        
}
