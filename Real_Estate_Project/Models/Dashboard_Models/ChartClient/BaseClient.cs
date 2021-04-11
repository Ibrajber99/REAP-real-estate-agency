using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Real_Estate_Project.Models.Dashboard_Models.ChartClient
{
    public class BaseClient<T> : HttpClient, IBaseClient<T> where T : class
    {
        private string BasePath;
        private const string MEDIA_TYPE = "application/json";

        public BaseClient(string baseAddress, string basePath)
        {
            BaseAddress = new Uri(baseAddress);
            BasePath = basePath;
        }

        public async Task<ClientResponseModel> GetChartResult(T item)
        {
            try
            {
                SetupHeaders();

                var objectToSerialize = GetChartObject(item);


                var serializedJson = GetSerializedObject(objectToSerialize);
                var bodyContent = GetBodyContent(serializedJson);

                var response = await PostAsync(BasePath, bodyContent);

                if (response.IsSuccessStatusCode)
                {
                    var responseString = await response.Content.ReadAsStringAsync();

                    var responseModel = JsonConvert.DeserializeObject
                        <ClientResponseModel>(responseString);

                    return responseModel;
                }
                else
                {
                    throw new Exception
                        (response.StatusCode.ToString());
                }
            }
            catch (Exception ex)
            {
                throw new Exception
                    ($"Something went wrong {ex.Message}");
            }
        }


        #region Request Helpers
        protected virtual void SetupHeaders()
        {
            DefaultRequestHeaders.Clear();

            //Define request data format  
            DefaultRequestHeaders.Accept.Add
                (new MediaTypeWithQualityHeaderValue
                (MEDIA_TYPE));
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected virtual string GetSerializedObject(object obj)
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver =
                new CamelCasePropertyNamesContractResolver();

            var serializedJson = JsonConvert
                .SerializeObject(obj, serializerSettings);

            return serializedJson;
        }

        protected virtual StringContent GetBodyContent(string serializedJson)
        {
            var bodyContent = new StringContent
                (serializedJson, Encoding.UTF8, "application/json");

            return bodyContent;
        }

        private object GetChartObject(object item,int width=200,int height=150)
        {
            var objToSerialize = new
            {
                Width = width,
                Height = height,
                Chart = item,
            };

            return objToSerialize;
        }
        #endregion
    }
}