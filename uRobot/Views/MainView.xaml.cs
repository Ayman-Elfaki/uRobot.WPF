using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using uRobot.Services;

namespace uRobot.Views
{
    /// <summary>
    /// Interaction logic for MainView.xaml
    /// </summary>
    public partial class MainView : Window
    {

        private BluetoothService _bluetoothService;
        public MainView()
        {
            InitializeComponent();
            Init();
        }

        private void Init()
        {
            _bluetoothService = new BluetoothService();
            devicesComBox.ItemsSource = _bluetoothService.GetDevices();

            aBtn.Click += async (s, e) => await DoAsync(Actions.A);
            bBtn.Click += async (s, e) => await DoAsync(Actions.B);
            cBtn.Click += async (s, e) => await DoAsync(Actions.C);
            dBtn.Click += async (s, e) => await DoAsync(Actions.D);

            startBtn.Click += async (s, e) => await DoAsync(Actions.Start);
            selectBtn.Click += async (s, e) => await DoAsync(Actions.Select);

            upBtn.Click += async (s, e) => await MoveAsync(Directions.Up);
            downBtn.Click += async (s, e) => await MoveAsync(Directions.Down);
            leftBtn.Click += async (s, e) => await MoveAsync(Directions.Left);
            rightBtn.Click += async (s, e) => await MoveAsync(Directions.Right);

            connectBtn.Click += async (s, e) => 
            {
                var port = devicesComBox.SelectedValue as string;
                if (port != null)
                {
                    if (!_bluetoothService.IsConnected)
                    {
                        connectBtn.IsEnabled = false;
                        connectBtn.Content = "Disconnect";
                        await _bluetoothService.ConnectAsync(port);
                        connectBtn.IsEnabled = true;
                    }
                    else
                    {
                        connectBtn.IsEnabled = false;
                        connectBtn.Content = "Connect";
                         _bluetoothService.Disconnect();
                        connectBtn.IsEnabled = true;
                    }
                }
            };

            refreshBtn.Click += (s, e) => 
            {
                devicesComBox.ItemsSource =_bluetoothService.GetDevices();
            };
        }

        protected async override void OnKeyDown(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                    await MoveAsync(Directions.Left);
                    break;
                case Key.Up:
                    await MoveAsync(Directions.Up);
                    break;
                case Key.Right:
                    await MoveAsync(Directions.Right);
                    break;
                case Key.Down:
                    await MoveAsync(Directions.Down);
                    break;
                case Key.A:
                    await DoAsync(Actions.D);
                    break;
                case Key.D:
                    await DoAsync(Actions.B);
                    break;
                case Key.S:
                    await DoAsync(Actions.C);
                    break;
                case Key.W:
                    await DoAsync(Actions.A);
                    break;
                case Key.Q:
                    await DoAsync(Actions.Select);
                    break;
                case Key.Space:
                    await DoAsync(Actions.Start);
                    break;
            }
            base.OnKeyDown(e);
        }
        protected async override void OnKeyUp(KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Left:
                case Key.Up:
                case Key.Right:
                case Key.Down:
                    await MoveAsync(Directions.Stop);
                    break;
            }

            base.OnKeyUp(e);
        }

        private async Task MoveAsync(Directions directions)
        {
            switch (directions)
            {
                case Directions.Up:
                    await _bluetoothService.SendAsync("1");
                    break;
                case Directions.Down:
                    await _bluetoothService.SendAsync("2");
                    break;
                case Directions.Right:
                    await _bluetoothService.SendAsync("3");
                    break;
                case Directions.Left:
                    await _bluetoothService.SendAsync("4");
                    break;
                case Directions.Stop:
                    await _bluetoothService.SendAsync("5");
                    break;
            }
        }
        private async Task DoAsync(Actions actions)
        {
            switch (actions)
            {
                case Actions.A:
                    await _bluetoothService.SendAsync("A");
                    break;
                case Actions.B:
                    await _bluetoothService.SendAsync("B");
                    break;
                case Actions.C:
                    await _bluetoothService.SendAsync("C");
                    break;
                case Actions.D:
                    await _bluetoothService.SendAsync("D");
                    break;
                case Actions.Start:
                    await _bluetoothService.SendAsync("S1");
                    break;
                case Actions.Select:
                    await _bluetoothService.SendAsync("S2");
                    break;
            }
        }

    }


    public enum Directions
    {
        Up,
        Down,
        Right,
        Left,
        Stop
    }

    public enum Actions
    {
        A,
        B,
        C,
        D,
        Start,
        Select
    }

}
