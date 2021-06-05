using HARD.ViewModel.Linker;
using System.Windows.Controls;

namespace HARD.View.Linker
{
    public partial class Linker_View : UserControl
    {
        public Linker_View()
        {
            InitializeComponent();

            DataContext = new Linker_ViewModel();
        }
    }
}
