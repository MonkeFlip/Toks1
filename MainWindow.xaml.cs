using System;
using System.Windows;
using System.Windows.Controls;
using System.IO.Ports;
using System.Text;

namespace Toks1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
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
                }
                catch (Exception)
                {
                    try
                    {
                        serialPort.PortName = ports[1];
                        serialPort.Open();
                    }
                    catch (Exception e) {
                        ControlAndDebugMessage(e.Message);
                    }
                }
            }
            catch (Exception e) {
                serial1_field.IsReadOnly = serial2_field.IsReadOnly = true;
                SerialEnter.IsEnabled = false;
                baud115200.IsEnabled = baud19200.IsEnabled = baud57600.IsEnabled = baud38400.IsEnabled = baud9600.IsEnabled = false;
                ControlAndDebugMessage(e.Message);
                return;
            }

            serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
        }

        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            serial1_field.Text = serial2_field.Text = "";
        }

        private void SerialEnter_Click(object sender, RoutedEventArgs e)
        {
            byte[] mem = Encoding.UTF8.GetBytes(serial1_field.Text);
            serialPort.Write(mem, 0, mem.Length);
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
            Decoder mem = Encoding.UTF8.GetDecoder();
            byte[] arr = new byte[sp.BytesToRead];
            char[] a = new char[arr.Length];
            this.Dispatcher.Invoke(() =>
            {
                for (int i = 0; i < arr.Length; i++)
                {
                    arr[i] = (byte)sp.ReadByte();
                }

                mem.GetChars(arr, 0, arr.Length, a, 0);
                serial2_field.Text = new string(a);
            });
        }

        private void ControlAndDebugMessage(string message)
        {
            ControlAndDebug.Text += message + "\n";
            ControlAndDebug.ScrollToEnd();
        }
    }
}