using SlimGis.MapKit.Controls;
using SlimGis.MapKit.Geometries;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Globalization;
using SlimGis.MapKit.Wpf;
using System.Linq;
using SlimGis.MapKit.Utilities;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for UseOverlaysView.xaml
    /// </summary>
    public partial class DrawGeometryView : UserControl
    {
        public DrawGeometryView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();
            Map1.ZoomToFullBound();

            TrackModeOptions.ItemsSource = Enum.GetValues(typeof(TrackMode));
        }

        private void RadioButton_Click(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;
            Map1.Behaviors.TrackBehavior.TrackMode = (TrackMode)radioButton.DataContext;
            DescPanel.Children.OfType<StackPanel>().ForEach(panel =>
            {
                panel.Visibility = Map1.Behaviors.TrackBehavior.TrackMode.ToString() == panel.Tag.ToString() ? Visibility.Visible : Visibility.Collapsed;
            });
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            Map1.Behaviors.TrackBehavior.Clear();
            Map1.Behaviors.TrackBehavior.Refresh();
        }
    }

    public class TrackOptionCheckStatusConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Length == 2 && values[0] is TrackMode && values[1] is MapControl)
            {
                TrackMode trackMode = (TrackMode)values[0];
                MapControl mapControl = (MapControl)values[1];
                return mapControl.Behaviors.TrackBehavior.TrackMode == trackMode;
            }
            else return Binding.DoNothing;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
