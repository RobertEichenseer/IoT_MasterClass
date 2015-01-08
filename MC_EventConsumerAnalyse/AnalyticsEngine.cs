using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace MC_EventConsumerAnalyse
{
    public class AnalyticsEngine
    {
        public static ConcurrentStack<double> _humidityTelemetryIngest = new ConcurrentStack<double>();
        public static ConcurrentStack<double> _pollutionTelemetryIngest = new ConcurrentStack<double>();
        public Timer _timer = new Timer();

        public AnalyticsEngine()
        {
            _timer.Interval = 5000;
            _timer.Elapsed += _timer_Elapsed;
        }

        internal void StartAnalytics()
        {
            _timer.Start();
        }

        void _timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            _timer.Stop();

            //Calculate Avg Humidity
            if (_humidityTelemetryIngest.Count > 0)
            {
                double avgHumidity = _humidityTelemetryIngest.Sum() / _humidityTelemetryIngest.Count;
                Debug.WriteLine("Avg Humidity: {0}", avgHumidity);
                Debug.WriteLine("No of ingests: {0}", _humidityTelemetryIngest.Count);
                _humidityTelemetryIngest.Clear();
            }

            //Calculate Pollution

            if (_pollutionTelemetryIngest.Count > 0)
            {
                int pollutionCounter = _pollutionTelemetryIngest.Where(pollution => pollution >= 80).Count();
                if (pollutionCounter > 2)
                {
                    Debug.WriteLine("Pollution {0} times over threshold", pollutionCounter);
                    _pollutionTelemetryIngest.Clear();
                }
            }
            _timer.Start();
        }
    }
}
