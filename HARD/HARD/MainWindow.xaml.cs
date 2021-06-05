using System.Windows;

namespace HARD
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            Config.AutoD_Config.Instance.Load();
            Config.Linker_Config.Instance.Load();

            InitializeComponent();

            Topmost = true;
        }
    }
}
