using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.WebServiceClient;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Threading.Tasks;

namespace CrmConsoleApp
{
    static class Helper
    {
        public static IOrganizationService Connect(string URL, string ClientID, string ClientSecret, string TenantID)
        {
            IOrganizationService _orgService = null;
            string Token = GetAuthenticationHeader(URL, ClientID, ClientSecret, TenantID);
            Uri serviceUrl = new Uri(URL + @"/xrmservices/2011/organization.svc/web?SdkClientVersion=8.2");

            using (OrganizationWebProxyClient sdkService = new OrganizationWebProxyClient(serviceUrl, new TimeSpan(0, 100, 0), false))
            {
                sdkService.HeaderToken = Token;
                _orgService = sdkService != null ? (IOrganizationService)sdkService : null;
            }

            return _orgService;
        }
        /// <summary>
        /// Method to get authentication header for token
        /// </summary>
        private static string GetAuthenticationHeader(string URL, string clientId, string clientSecret, string TenantID)
        {
            string tenandId = TenantID;
            string clientApplicationId = clientId;
            string aadResource = URL;
            string clientClientSecret = clientSecret;

            string oauthUrl = string.Format("https://login.microsoftonline.com/{0}/oauth2/token", tenandId);
            string reqBody = string.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&resource={2}", Uri.EscapeDataString(clientApplicationId), Uri.EscapeDataString(clientClientSecret), Uri.EscapeDataString(aadResource));

            HttpClient client = new HttpClient();
            HttpContent content = new StringContent(reqBody);
            content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/x-www-form-urlencoded");
            AccessToken token = null;

            using (HttpResponseMessage response = client.PostAsync(oauthUrl, content).Result)
            {
                if (response.IsSuccessStatusCode)
                {
                    DataContractJsonSerializer serializer = new DataContractJsonSerializer(typeof(AccessToken));
                    Stream json = response.Content.ReadAsStreamAsync().Result;
                    token = (AccessToken)serializer.ReadObject(json);
                }
            }
            return token.access_token;
        }
    }
}
