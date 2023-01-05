using ConcatText.ViewModel;
using System.Windows;

namespace ConcatText
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            DataContext = new MainViewModel();
        }
    }
}
