using InventoryManagement.Data;
using System.Windows;

namespace InventoryManagement
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            init();
        }

        private void init()
        {
            Log.MakeBackupLogDir();
        }
    }
}
