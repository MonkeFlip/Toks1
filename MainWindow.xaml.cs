using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO.Ports;
using System.Threading;

namespace Toks1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        TextBox read_mem, write_mem;
        SerialPort serialPort;
        public MainWindow()
        {
            InitializeComponent();
            try
            {
                string[] ports = SerialPort.GetPortNames();
                if (ports.Length < 2)
                {
                    throw new InvalidOperationException("There is no paired serial ports.");
                }

                try
                {
                    serialPort = new SerialPort(ports[0], 9600);
                    serialPort.Open();
                    read_mem = serial1_field;
                    write_mem = serial2_field;
                    Serial2Enter.IsEnabled = false;
                    serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                    serial2_field.IsEnabled = false;
                }
                catch (Exception)
                {
                    try
                    {
                        serialPort = new SerialPort(ports[1], 9600);
                        serialPort.Open();
                        read_mem = serial2_field;
                        write_mem = serial1_field;
                        Serial1Enter.IsEnabled = false;
                        serialPort.DataReceived += new SerialDataReceivedEventHandler(DataReceivedHandler);
                        serial1_field.IsEnabled = false;
                    }
                    catch (Exception e) {
                        if ()
                        ControlAndDebugMessage(e.Message);
                    }
                }
            }
            catch (Exception e) {
                ControlAndDebugMessage(e.Message);
            }            
        }


        private void Clean_Click(object sender, RoutedEventArgs e)
        {
            serial1_field.Text = serial2_field.Text = ControlAndDebug.Text = "";
        }

        private void Serial1Enter_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Write(read_mem.Text);
        }

        private void Serial2Enter_Click(object sender, RoutedEventArgs e)
        {
            serialPort.Write(read_mem.Text);
        }

        private void baud9600_Click(object sender, RoutedEventArgs e)
        {
            serialPort.BaudRate = 9600;
        }

        private void baud19200_Click(object sender, RoutedEventArgs e)
        {
            serialPort.BaudRate = 19200;
        }

        private void baud38400_Click(object sender, RoutedEventArgs e)
        {
            serialPort.BaudRate = 38400;
        }

        private void baud57600_Click(object sender, RoutedEventArgs e)
        {
            serialPort.BaudRate = 57600;
        }

        private void baud115200_Click(object sender, RoutedEventArgs e)
        {
            serialPort.BaudRate = 115200;
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
            ControlAndDebug.Text = message;
        }
    }
}
