using System.Net;
using static Reference.Device.DeviceHandler;

namespace Reference.Device
{
    public class Generator
    {
        public class MKSAPI : IDeviceAPI
        {
            public void Connect()
            {
            }

            public void Disconnect()
            {
            }

            public bool IsConnected()
            {
                return true;
            }
        }

        public class MKS : IDevice, ITcpIP
        {
            public int ID { get; set; }
            public string Name { get; set; }
            public IPAddress IPAddress { get; set; }
            public int Port { get; set; }

            private readonly IDeviceAPI _deviceAPI;
            public MKS()
            {
                this._deviceAPI = new MKSAPI();
            }
            public MKS(IDeviceAPI deviceAPI)
            {
                this._deviceAPI = deviceAPI;
            }

            public void CheckIO()
            {
            }

            public void Connect()
            {
                this._deviceAPI.Connect();
            }

            public void Disconnect()
            {
                this._deviceAPI.Disconnect();
            }

            public bool isConnected()
            {
                return this._deviceAPI.IsConnected();
            }
        }
    }
}
