using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_SensorRbP2
{
    public class Telemetry
    {
        public string asJson()
        {
            return String.Format("{{ Humidity: {0}, DeviceId: '{1}', Temperature: {2}; Pollution: {3} }}", Humidity, DeviceId, Temperature, Pollution);
        }

        public double Humidity { get; set; }
        public string DeviceId { get; set; }
        public double Temperature { get; set; }
        public double Pollution { get; set; }
    }
}
