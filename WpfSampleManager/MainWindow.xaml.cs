using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private async void Window_Loaded(object sender, RoutedEventArgs e)
        {
            await GlobalResources.Instance.LoadAsync();
        }

        private void TreeView_SelectedItemChanged(object sender, RoutedPropertyChangedEventArgs<object> e)
        {
            TreeView treeView = (TreeView)sender;
            SampleModel sampleModel = treeView.SelectedItem as SampleModel;
            if (sampleModel != null)
            {
                GlobalResources.Instance.CurrentSample = sampleModel;
            }
            else if (treeView.SelectedItem is SampleCategoryPlugin)
            {
                GlobalResources.Instance.CurrentSample = ((SampleCategoryPlugin)treeView.SelectedItem).Samples.FirstOrDefault();
            }
        }

        private void ToggleButton_Click(object sender, RoutedEventArgs e)
        {
            ToggleButton toggleButton = (ToggleButton)sender;
            GlobalResources.Instance.ListPanelVisibility = toggleButton.IsChecked.Value ? Visibility.Collapsed : Visibility.Visible;
            GlobalResources.Instance.CurrentSample.ControlPanelVisibility = GlobalResources.Instance.ListPanelVisibility;
        }
    }
}
