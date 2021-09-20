using System;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;

namespace Toks1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox read_mem;
        SerialPort serialPort;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                ControlAndDebug.IsReadOnly = true;
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length < 2)
                {
                    throw new InvalidOperationException("There are no paired serial ports.");
                }

                try
                {
                    serialPort = new SerialPort(ports[0], 115200, Parity.None, 8, StopBits.One);
                    serialPort.Open();
                    read_mem = serial1_field;
                    Serial2Enter.IsEnabled = false;
                    serial2_field.IsReadOnly = true;                
                }
                catch (Exception)
                {
                    try
                    {
                        serialPort.PortName = ports[1];
                        serialPort.Open();
                        read_mem = serial2_field;
                        Serial1Enter.IsEnabled = false;
                        serial1_field.IsReadOnly = true;
                    }
                    catch (Exception e) {
                        ControlAndDebugMessage(e.Message);
                    }
                }
            }
            catch (Exception e) {
                serial1_field.IsReadOnly = true;
                serial2_field.IsReadOnly = true;
                Serial1Enter.IsEnabled = false;
                Serial2Enter.IsEnabled = false;
                baud115200.IsEnabled = baud19200.IsEnabled = baud57600.IsEnabled = baud38400.IsEnabled = baud9600.IsEnabled = false;
                ControlAndDebugMessage(e.Message);
                return;
            }

            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            serial1_field.Text = serial2_field.Text = " ";
        }

        private void SerialEnter_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Write(read_mem.Text);
        }

        private void ChangeBaud(object sender, RoutedEventArgs e)
        {
            Button mem = (Button)sender;
            serialPort.BaudRate = int.Parse(mem.Content.ToString());
            ControlAndDebugMessage($"Baud rate set to {mem.Content.ToString()}");
        }

        private void DataReceivedHandler(object sender, SerialDataReceivedEventArgs e)
        {
            SerialPort sp = (SerialPort)sender;
            this.Dispatcher.Invoke(() =>
            {
                read_mem.Text = sp.ReadExisting();
            });
        }

        private void ControlAndDebugMessage(string message)
        {
            ControlAndDebug.Text += message + "\n";
            ControlAndDebug.ScrollToEnd();
        }
    }
}