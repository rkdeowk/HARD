using InventoryManagement.ViewModel;
using System.Windows.Controls;

namespace InventoryManagement.View
{
    public partial class ManageSWVersion : UserControl
    {
        ManageSWVersionViewModel vm = new ManageSWVersionViewModel();

        public ManageSWVersion()
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
