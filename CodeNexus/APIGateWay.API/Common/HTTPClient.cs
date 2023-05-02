using APIGateWay.API.Model;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace APIGateWay.API.Common
{
    public class HTTPClient
    {
        //private readonly IConfiguration _configuration;
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        #region Constructor
        public HTTPClient()//IConfiguration configuration
        {
            //_configuration = configuration;
        }
        #endregion

        #region Get Async
        /// <summary>
        /// Get Async
        /// </summary>
        /// <param name="baseURL"></param>
        /// <param name="endPointWithParams"></param>
        /// <returns></returns>
        public async Task<SuccessData> GetAsync(string baseURL, string endPointWithParams)
        {
            try
            {
                using var Client = new HttpClient
                {
                    BaseAddress = new Uri(baseURL)
                };
                HttpResponseMessage response = await Client.GetAsync(endPointWithParams);
                if (response?.IsSuccessStatusCode == true)
                {
                    return response.Content.ReadAsAsync<SuccessData>().Result;
                }
            }
            catch (Exception ex) { logger.Error(ex.Message.ToString()); }
            return new SuccessData();
        }
        #endregion

        #region Post As Json Async
        /// <summary>
        /// Post As Json Async
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <param name="baseURL"></param>
        /// <param name="endPoint"></param>
        /// <returns></returns>
        public async Task<SuccessData> PostAsJsonAsync<TModel>(TModel model, string baseURL, string endPoint)
        {
            try
            {
                using var Client = new HttpClient
                {
                    BaseAddress = new Uri(baseURL)
                };
                HttpResponseMessage response = await Client.PostAsJsonAsync(endPoint, model);
                if (response?.IsSuccessStatusCode == true)
                {
                    return response.Content.ReadAsAsync<SuccessData>().Result;
                }
            }
            catch (Exception ex) { logger.Error(ex.Message.ToString()); }
            return new SuccessData();
        }
        #endregion

        #region Delete Async
        /// <summary>
        /// Delete Async
        /// </summary>
        /// <param name="baseURL"></param>
        /// <param name="endPointWithParams"></param>
        /// <returns></returns>
        public async Task<SuccessData> DeleteAsync(string baseURL, string endPointWithParams)
        {
            try
            {
                using var Client = new HttpClient
                {
                    BaseAddress = new Uri(baseURL)
                };
                HttpResponseMessage response = await Client.DeleteAsync(endPointWithParams);
                if (response?.IsSuccessStatusCode == true)
                {
                    return response.Content.ReadAsAsync<SuccessData>().Result;
                }
            }
            catch (Exception ex) { logger.Error(ex.Message.ToString()); }
            return new SuccessData();
        }
        #endregion
    }
}