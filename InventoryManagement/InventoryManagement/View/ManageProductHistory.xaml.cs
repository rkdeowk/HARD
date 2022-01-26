using InventoryManagement.ViewModel;
using System.Windows;

namespace InventoryManagement.View
{
    public partial class ManageProductHistory : Window
    {
        ManageProductHistoryViewModel vm = new ManageProductHistoryViewModel();

        public ManageProductHistory()
        {
            InitializeComponent();

            DataContext = vm;
        }

        public void GetHistory(string str)
        {
            vm.GetHistoryData(str);
        }
    }
}
