using InventoryManagement.ViewModel;
using System.Windows.Controls;

namespace InventoryManagement.View
{
    public partial class ManageFiber : UserControl
    {
        ManageFiberViewModel vm = new ManageFiberViewModel();

        public ManageFiber()
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
