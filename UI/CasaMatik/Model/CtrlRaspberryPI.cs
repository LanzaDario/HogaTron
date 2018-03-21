using System;
using Windows.Devices.Gpio;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HogaTron.Model;

namespace HogaTron.Model
{        
    class CtrlRaspberryPI    
    {
        var gpio = GpioController.GetDefault();
        private GpioPin IOpin;

        private void IOInicializar(int pinNum, string tipoIO)
        {

            //private GpioPinValue IOpinValue = GpioPinValue.High;

            // Show an error if there is no GPIO controller
            if (gpio == null)
            {
                //GpioStatus.Text = "There is no GPIO controller on this device.";
                return;
            }

            IOpin = gpio.OpenPin(pinNum);

            if (tipoIO == "Input")
            {
                // Check if input pull-up resistors are supported
                if (IOpin.IsDriveModeSupported(GpioPinDriveMode.InputPullUp))
                    IOpin.SetDriveMode(GpioPinDriveMode.InputPullUp);
                else
                    IOpin.SetDriveMode(GpioPinDriveMode.Input);

                // Set a debounce timeout to filter out switch bounce noise from a button press
                IOpin.DebounceTimeout = TimeSpan.FromMilliseconds(50);
                // Register for the ValueChanged event so our buttonPin_ValueChanged 
                // function is called when the button is pressed
                IOpin.ValueChanged += CtrlRaspberryPI.ValueChanged;
            }
            else
            {
                // Initialize LED to the OFF state by first writing a HIGH value
                // We write HIGH because the LED is wired in a active LOW configuration
                IOpin.Write(GpioPinValue.High);
                IOpin.SetDriveMode(GpioPinDriveMode.Output);
            }

            //GpioStatus.Text = "GPIO pins initialized correctly.";
        }

        private void IOValueChanged(GpioPin sender, GpioPinValueChangedEventArgs e)
        {
            // toggle the state of the LED every time the button is pressed
            if (e.Edge == GpioPinEdge.FallingEdge)
            {
                ledPinValue = (ledPinValue == GpioPinValue.Low) ?
                    GpioPinValue.High : GpioPinValue.Low;
                ledPin.Write(ledPinValue);
            }

            // need to invoke UI updates on the UI thread because this event
            // handler gets invoked on a separate thread.
            var task = Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => {
                if (e.Edge == GpioPinEdge.FallingEdge)
                {
                    ledEllipse.Fill = (ledPinValue == GpioPinValue.Low) ? 
                        redBrush : grayBrush;
                    GpioStatus.Text = "Button Pressed";
                }
                else
                {
                    GpioStatus.Text = "Button Released";
                }
            });
        }
    }
}