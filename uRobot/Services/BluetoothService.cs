using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace uRobot.Services
{
    public class BluetoothService
    {
        private SerialPort _serialPort;

        public bool IsConnected { get; private set; }

        public BluetoothService()
        {
            _serialPort = new SerialPort();
        }

        public async Task ConnectAsync(string portName, int baudRate = 9600)
        {
            await Task.Run(() =>
            {
                _serialPort.PortName = portName;
                _serialPort.BaudRate = baudRate;
                try
                {
                    if (!_serialPort.IsOpen)
                    {
                        _serialPort.Open();
                        IsConnected = _serialPort.IsOpen;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });
        }

        public void Disconnect()
        {
            try
            {
                if (_serialPort.IsOpen)
                {
                    _serialPort.Close();
                    _serialPort.Dispose();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public async Task SendAsync(string data)
        {
            await Task.Run(() =>
            {
                try
                {
                    if (_serialPort.IsOpen)
                    {
                        _serialPort.WriteLine(data);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }

            });
        }

        public IEnumerable<string> GetDevices()
        {
            return SerialPort.GetPortNames();
        }
    }

}
