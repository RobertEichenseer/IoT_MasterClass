using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Gpio;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace MC_SensorRbP2
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private DispatcherTimer _dispatcherTimer;
        private GpioController _gpioController;

        private GpioPin _gpioSuccessPin;
        private int _ledSuccessPin = 13;

        private GpioPin _gpioErrorPin;
        private int _ledErrorPin = 6;

        private EventHubIngest _eventHubIngest;

        public MainPage()
        {
            this.InitializeComponent();

            _eventHubIngest = new EventHubIngest();

            _gpioController = GpioController.GetDefault();
            if (_gpioController == null)
                return;

            _gpioSuccessPin = _gpioController.OpenPin(_ledSuccessPin);
            _gpioSuccessPin.SetDriveMode(GpioPinDriveMode.Output);
            _gpioSuccessPin.Write(GpioPinValue.Low);

            _gpioErrorPin = _gpioController.OpenPin(_ledErrorPin);
            _gpioErrorPin.SetDriveMode(GpioPinDriveMode.Output);
            _gpioErrorPin.Write(GpioPinValue.Low);

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(1000);
            _dispatcherTimer.Tick += (sender, e) => {

                _gpioSuccessPin.Write(GpioPinValue.High);
                _gpioErrorPin.Write(GpioPinValue.High);

                bool ingestTask = _eventHubIngest.TelemetryIngest(new Telemetry()
                {
                    DeviceId = "Device-78",
                    Humidity = 10,      //Read from Sensors
                    Pollution = 100,    //Read from Sensors
                    Temperature = 35,   //Read from Sensors
                });


                //Check if pins are available
                if (_gpioController == null || _gpioSuccessPin == null)
                    return;

                _gpioSuccessPin.Write(ingestTask ? GpioPinValue.Low : GpioPinValue.High);
                _gpioErrorPin.Write(ingestTask ? GpioPinValue.High : GpioPinValue.Low);

            };
            _dispatcherTimer.Start();
        }

    }
}
