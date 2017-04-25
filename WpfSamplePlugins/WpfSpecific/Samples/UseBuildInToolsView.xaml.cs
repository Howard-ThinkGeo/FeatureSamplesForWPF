using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System;
using System.Globalization;

namespace SlimGis.Samples
{
    public partial class UseBuildInToolsView : UserControl
    {
        public UseBuildInToolsView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;

            LayerOverlay staticOverlay = new LayerOverlay { Name = "Static Overlay" };
            staticOverlay.Layers.Add(new OpenStreetMapLayer());
            Map1.Overlays.Add(staticOverlay);

            Map1.ZoomToFullBound();
        }
    }

    public class VisibilityToBooleanConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility)
            {
                Visibility v = (Visibility)value;
                return v == Visibility.Visible;
            }
            else if (value is bool)
            {
                return (bool)value ? Visibility.Visible : Visibility.Collapsed;
            }
            else return Binding.DoNothing;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert(value, targetType, parameter, culture);
        }
    }
}
