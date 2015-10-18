using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

using Microsoft.Azure.Devices.Client;
using System.Text;
using Windows.UI.Popups;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MC_SensorRbP2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        private DeviceClient _deviceClient;
        //"Host name" in Azure portal
        private string _hostName = "[Your IoT Hub Name].azure-devices.net";
        //Fixed Id given by your application
        private string _deviceId = "[Your Device Id / Device Name]";
        //device.Authentication.SymmetricKey.PrimaryKey - After Device creation -- See <MC_SensorIoTHub\MC_AdminBackend\ManageDevices.cs>
        private string _sharedAccessKey = "[Your generated Device Primary Key]";

        DispatcherTimer _sendTelemetryTimer = new DispatcherTimer();

        public MainPage()
        {
            this.InitializeComponent();
            _sendTelemetryTimer.Tick += sendTelemetryData;
            _sendTelemetryTimer.Interval = TimeSpan.FromSeconds(1);

            string deviceConnectionString = "HostName={0};DeviceId={1};SharedAccessKey={2}";
            deviceConnectionString = String.Format(deviceConnectionString, _hostName, _deviceId, _sharedAccessKey);

            _deviceClient = DeviceClient.CreateFromConnectionString(deviceConnectionString, TransportType.Http1);
        }

        private void StartStopTelemetrySend_Click(object sender, RoutedEventArgs e)
        {

            if (_sendTelemetryTimer.IsEnabled)
                _sendTelemetryTimer.Stop();
            else
                _sendTelemetryTimer.Start();
        }

        private async void sendTelemetryData(object sender, object e)
        {
            string telemetry = DateTime.Now.ToUniversalTime().ToString();

            Message message = new Message(Encoding.ASCII.GetBytes(telemetry));
            await _deviceClient.SendEventAsync(message);

        }

        private async void ReceiveCommand_Click(object sender, RoutedEventArgs e)
        {
            var receivedMessage = await _deviceClient.ReceiveAsync();

            if (receivedMessage != null)
            {
                string messageData = Encoding.ASCII.GetString(receivedMessage.GetBytes());
                await _deviceClient.CompleteAsync(receivedMessage);
                //await _deviceClient.AbandonAsync(receivedMessage);
                //await _deviceClient.RejectAsync(receivedMessage);

                MessageDialog messageDialog = new MessageDialog(messageData);
                await messageDialog.ShowAsync();
            }

        }

    }
}
