using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for PopupPromptUserControl.xaml
    /// </summary>
    public partial class PopupPromptUserControl : UserControl
    {
        public PopupPromptUserControl()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            PopupPromptViewModel vm = DataContext as PopupPromptViewModel;
            vm?.Confirm?.Invoke();
        }
    }
}
