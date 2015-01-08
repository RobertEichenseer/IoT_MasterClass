using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using System.Text;

namespace MC_SensorNetduino
{
    public class Program
    {
        static Timer _ingestTimer;
        static bool _enableSendTelemetry = false;
        static Random _random;

        static string _requestUrl = "http://<< add the ip of your field gateway here>>/sendtelemetry";

        public static void Main()
        {
            _random = new Random(DateTime.Now.Millisecond);
            _ingestTimer = new Timer(new TimerCallback(SendTelemetry), null, 0, 1000);

            OutputPort led = new OutputPort(Pins.ONBOARD_LED, false);
            InputPort button = new InputPort(Pins.ONBOARD_SW1, false, Port.ResistorMode.Disabled);
            bool buttonState = false;

            while (true)
            {
                buttonState = button.Read();
                if (buttonState)
                    _enableSendTelemetry = !_enableSendTelemetry;

                led.Write(!buttonState);
            }
        }

        private static void SendTelemetry(object state)
        {
            if (!_enableSendTelemetry)
                return;

            //Emulate telemetry data 
            int pollution = (int)(_random.NextDouble() * 100);
            string postData = String.Concat("{'DeviceId':'Device-100','Temperature':88,'Humidity':68,'Pollution':", pollution.ToString(), "}");
            var postDataByte = Encoding.UTF8.GetBytes(postData);
            var postDataLength = postDataByte.Length;
            Debug.Print(String.Concat("Telemetry Post: ", postData));

            using (var myRequest = (HttpWebRequest)WebRequest.Create(_requestUrl))
            {
                myRequest.Method = "POST";
                myRequest.ContentType = "application/json";
                myRequest.ContentLength = postDataLength;
                using (var stream = myRequest.GetRequestStream())
                {
                    stream.Write(postDataByte, 0, postDataLength);
                    using (var response = (HttpWebResponse)myRequest.GetResponse())
                    {
                        Debug.Print("Response: " + response.StatusCode + " " + response.StatusDescription);
                        Debug.Print("");
                    }
                }
            }
        }
    }
}
