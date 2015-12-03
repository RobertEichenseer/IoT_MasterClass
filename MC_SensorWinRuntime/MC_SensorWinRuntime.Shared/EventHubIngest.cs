using System;
using System.Collections.Generic;
using System.Text;

namespace MC_SensorWinRuntime
{
    public class EventHubIngest
    {
        public async Task<bool> TelemetryIngest(Telemetry telemetry)
        {

            string serviceBusNamespace = "iotmc-ns";
            string serviceBusUri = string.Format("{0}.servicebus.windows.net", serviceBusNamespace);
            string eventHubName = "IoTMC";
            string eventHubSASKeyName = "Device01";
            string eventHubSASKey = "<< Your SAS Key here >>";

            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri(String.Format("https://{0}", serviceBusUri));
                httpClient.DefaultRequestHeaders.Accept.Clear();

                string sBToken = CreateServiceBusSASToken(eventHubSASKeyName, eventHubSASKey, serviceBusUri);
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("SharedAccessSignature", sBToken);
                HttpContent httpContent = new StringContent(JsonConvert.SerializeObject(telemetry), Encoding.UTF8);
                httpContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
              
                string ingestPath = String.Format("/{0}/publishers/device01/messages", eventHubName);
                var response = await httpClient.PostAsync(ingestPath, httpContent);
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }

                return false;
            }
        }

        private string CreateServiceBusSASToken(string eventHubSASKeyName, string eventHubSASKey, string serviceBusUri)
        {
            int expirySeconds = (int)DateTime.UtcNow.AddMinutes(20).Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
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
