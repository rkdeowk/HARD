using Bookmarker.ViewModel;
using System.Windows.Controls;

namespace Bookmarker.View
{
    public partial class MainBookmarker : UserControl
    {
        MainBookmarkerViewModel vm = new MainBookmarkerViewModel();

        public MainBookmarker()
        {
            InitializeComponent();

            DataContext = vm;
        }

        private void dataGrid_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var row = sender as DataGrid;

            if (row == null) return;

            vm.Operation(row.SelectedItem as ConfigStruct);

            dataGrid.Focus();

            dataGrid.UnselectAll();
        }

        private void dataGrid_BeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            e.Cancel = true;
        }
    }
}
