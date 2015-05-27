using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;

namespace MC_SensorRbP2
{
    public class EventHubIngest
    {
        public bool TelemetryIngest(Telemetry telemetry)
        {

            string serviceBusNamespace = "iotmc-ns";
            string serviceBusUri = string.Format("{0}.servicebus.windows.net", serviceBusNamespace);
            string eventHubName = "IoTMC";
            string eventHubSASKeyName = "Device01";
            string eventHubSASKey = "t0JK19v94H3R8yAZ1uVkGcIUFi8zmGmBts4N09aNI0s=";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(String.Format("https://{0}", serviceBusUri));
                httpClient.DefaultRequestHeaders.Accept.Clear();

                string sBToken = CreateServiceBusSASToken(eventHubSASKeyName, eventHubSASKey, serviceBusUri);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", sBToken);
                HttpContent httpContent = new StringContent(telemetry.asJson(), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                string ingestPath = String.Format("/{0}/publishers/device01/messages", eventHubName);
                Task<HttpResponseMessage> response = httpClient.PostAsync(ingestPath, httpContent);
                response.Wait();
                if (response.Result.IsSuccessStatusCode)
                {
                    return true;
                }
                return false;
            }
        }

        private string CreateServiceBusSASToken(string eventHubSASKeyName, string eventHubSASKey, string serviceBusUri)
        {
            int expirySeconds = (int)new DateTime(2015, 12, 31).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            string stringToSign = WebUtility.UrlEncode(serviceBusUri) + "\n" + expirySeconds.ToString();
            string signature = HmacSha256(eventHubSASKey, stringToSign);
            return String.Format("sr={0}&sig={1}&se={2}&skn={3}", WebUtility.UrlEncode(serviceBusUri), WebUtility.UrlEncode(signature), expirySeconds, eventHubSASKeyName);
        }

        public string HmacSha256(string eventHubSASKey, string serviceBusUriExpiry)
        {
            var keyStrm = CryptographicBuffer.ConvertStringToBinary(eventHubSASKey, BinaryStringEncoding.Utf8);
            var valueStrm = CryptographicBuffer.ConvertStringToBinary(serviceBusUriExpiry, BinaryStringEncoding.Utf8);

            MacAlgorithmProvider macAlgorithmProvider = MacAlgorithmProvider.OpenAlgorithm(MacAlgorithmNames.HmacSha256);
            CryptographicHash cryptographicHash = macAlgorithmProvider.CreateHash(keyStrm);
            cryptographicHash.Append(valueStrm);

            return CryptographicBuffer.EncodeToBase64String(cryptographicHash.GetValueAndReset());
        }
    }
}
