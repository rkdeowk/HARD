using InventoryManagement.ViewModel;
using System.Windows.Controls;

namespace InventoryManagement.View
{
    public partial class ManageProduct : UserControl
    {
        ManageProductViewModel vm = new ManageProductViewModel();

        public ManageProduct()
        {
            InitializeComponent();

            DataContext = vm;
        }
    }
}
