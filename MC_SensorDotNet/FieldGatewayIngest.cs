using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace MC_SensorDotNet
{
    public class FieldGatewayIngest
    {
        public Task TelemetryIngest(Telemetry telemetry)
        {
            string fieldGatewayUrl = "http://<<add the ip of your field gateway here>>/sendtelemetry";

            StringContent stringContent = new StringContent(JsonConvert.SerializeObject(telemetry)); 
            HttpClient httpClient = new HttpClient();

            return httpClient.PostAsync(fieldGatewayUrl, stringContent);
        }
    }
}
