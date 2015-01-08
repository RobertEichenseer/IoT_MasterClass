using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MC_SensorDotNet
{
    public class Telemetry
    {
        public double Humidity { get; set; }
        public string DeviceId {get; set;}
        public double Temperature {get; set;}
        public double Pollution { get; set; }
        
    }
}
