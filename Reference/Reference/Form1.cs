using Reference.Config;
using Reference.Device;
using System.Windows.Forms;

namespace Reference
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            var k = JsonHandler.Instance.GetConfig();

            var z = new DeviceHandler();
        }
    }
}
