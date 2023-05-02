using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace HeartBeat
{
    public class ApiConfigurations
    {
        public static string url = "https://development-api.cloudswyft.com/labs/virtualmachine/";
        public static string tenantId = "a7db876451fc4e2387bebda3856bcf7f";
        public static string subscriptionId = "44e7f1ad-c881-4c6c-9af1-86d478cb5f74";
        public static string clmpURL = "https://labs-staging.cloudswyft.com/scl/api/";

        public async Task<string> ShutdownVM(string resourceId)
        {
            string response = null;

            try
            {
                var jsonVMData = new { resourceId = resourceId, operation = "Stop" };

                var data = JsonConvert.SerializeObject(jsonVMData);

                response = await ApiVMCall("PUT", tenantId, subscriptionId, url, data);

                return response;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error   --- " + e.InnerException.Message);
            }
            return response;

        }


        public async Task<string> UpdateConsumedHours(string url, string data)
        {
            string response = null;

            try
            {
                response = await ApiCall("PUT", url, data);
                
                return "OK";
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

     
        private async Task<string> ApiVMCall(string method, string tenantId, string subscriptionId, string url, string serializeData = null)
        {
            HttpResponseMessage response = null;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                client.DefaultRequestHeaders.Add("TenantId", tenantId);
                client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionId);

                //var jsonVMData = new { resourceId = resourceId, operation = "Stop" };

                //data = JsonConvert.SerializeObject(jsonVMData);
                
                if (method == "POST")
                {
                    response = await client.PostAsync(client.BaseAddress + url, new StringContent(serializeData));
                }
                else if (method == "GET")
                {
                    response = await client.GetAsync(client.BaseAddress + url).ConfigureAwait(false);
                }
                else if (method == "PUT")
                {
                    response = await client.PutAsync(client.BaseAddress, new StringContent(serializeData, Encoding.UTF8, "application/json"));
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                return e.InnerException.Message;
            }
        }

        private async Task<string> ApiCall(string method, string url, string serializeData = null)
        {
            HttpResponseMessage response = null;

            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                HttpClient client = new HttpClient();
                client.BaseAddress = new Uri(url);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
             

                //var jsonVMData = new { resourceId = resourceId, operation = "Stop" };

                //data = JsonConvert.SerializeObject(jsonVMData);

                if (method == "POST")
                {
                    response = await client.PostAsync(client.BaseAddress + url, new StringContent(serializeData));
                }
                else if (method == "GET")
                {
                    response = await client.GetAsync(client.BaseAddress + url).ConfigureAwait(false);
                }
                else if (method == "PUT")
                {
                    response = await client.PutAsync(client.BaseAddress, new StringContent(serializeData, Encoding.UTF8, "application/json"));
                }

                return await response.Content.ReadAsStringAsync();
            }
            catch (Exception e)
            {
                return e.InnerException.Message;
            }
        }
        //class JsonVM
        //{
        //    public static string ResourceId = resourceId;
        //    public static string Operation = "Stop";
        //}

    }
}
