using HARD.ViewModel.AutoD;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace HARD.View.AutoD
{
    public partial class AutoD_View : UserControl
    {
        AutoD_ViewModel vm = new AutoD_ViewModel();

        public AutoD_View()
        {
            InitializeComponent();

            DataContext = vm;
        }

        private void DataGridActionList_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.Header = $"Action {e.Row.GetIndex() + 1}";
        }

        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (vm.isSave) return;

            vm.RecoedSequence(e.Key);
        }

        private void TextBoxFileNameToSave_GotFocus(object sender, RoutedEventArgs e)
        {
            vm.isSave = true;
            Keyboard.ClearFocus();
        }

        private void DataGridActionList_GotFocus(object sender, RoutedEventArgs e)
        {
            vm.isSave = true;
        }

        private void TextBoxFileNameToSave_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Escape)
            {
                Keyboard.ClearFocus();
            }
        }

        private void DataGridActionList_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape)
            {
                Keyboard.ClearFocus();
            }
        }
    }
}
