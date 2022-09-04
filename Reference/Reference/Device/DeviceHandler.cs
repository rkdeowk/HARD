using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;

namespace Reference.Device
{
    public class DeviceHandler
    {
        static DeviceController _deviceController;

        public DeviceHandler()
        {
            _deviceController = new DeviceController();

            var generator = new Generator.MKS();
            generator.ID = 100;
            generator.Name = "Generator";
            generator.IPAddress = null;
            generator.Port = 10;
            _deviceController.AddDevice(generator);

            var process = new ProcessA();
            _deviceController.Process(process);
        }

        public interface IDeviceAPI
        {
            bool IsConnected();
            void Connect();
            void Disconnect();
        }

        public interface IDevice
        {
            int ID { get; set; }
            string Name { get; set; }

            bool isConnected();
            void Connect();
            void Disconnect();
            void CheckIO();
        }

        public interface ITcpIP
        {
            IPAddress IPAddress { get; set; }
            int Port { get; set; }
        }

        public abstract class DeviceControllerBase
        {
            private Thread _monitorThread;
            private int _monitorDelay;
            private bool _canMonitor;
            public static List<IDevice> Devices { get; set; }

            public DeviceControllerBase()
            {
                this._monitorDelay = 1000;
                this._monitorThread = new Thread(Monitor);
                Devices = new List<IDevice>();
            }

            public void AddDevice(IDevice device)
            {
                Devices.Add(device);
            }

            public void RemoveDevice(IDevice device)
            {
                Devices.Remove(device);
            }

            public IDevice GetDeviceByID(int deviceID)
            {
                return Devices.FirstOrDefault(x => x.ID == deviceID);
            }

            public IDevice GetDeviceByName(string deviceName)
            {
                return Devices.FirstOrDefault(x => x.Name == deviceName);
            }

            protected void Monitor()
            {
                while (this._canMonitor)
                {
                    foreach (var device in Devices)
                    {
                        device.CheckIO();
                        Thread.Sleep(this._monitorDelay);
                    }
                }
            }

            public void Process(IStrategy strategy)
            {
                strategy.Process();
            }
        }

        public interface IStrategy
        {
            void Process();
        }

        sealed class ProcessA : IStrategy
        {
            public void Process()
            {
                var generator = _deviceController.GetDeviceByName("Generator");
            }
        }

        sealed class ProcessB : IStrategy
        {
            public void Process()
            {

            }
        }

        public class DeviceController : DeviceControllerBase
        {
            public DeviceController()
            {

            }
        }
    }
}
