using SlimGis.MapKit.Controls;
using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for GoogleMapsLayerView.xaml
    /// </summary>
    public partial class GoogleMapsLayerView : UserControl
    {
        public GoogleMapsLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;

            GoogleMapsLayer googleMapsLayer = new GoogleMapsLayer(GoogleMapsType.RoadMap, "AIzaSyC2CYaFRRi-Caf24Lq_2xu5seA3EzLWKv0");

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(googleMapsLayer);
            Map1.Overlays.Add(layerOverlay);

            // We can also use the code below to add a google maps to our map.
            // Map1.UseGoogleMapsAsBaseMap(GoogleMapsType.RoadMap, "Your api key", "Your client id");

            Map1.ZoomToFullBound();
        }

        private void MapTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            GoogleMapsLayer gooleMapsLayer = Map1.FindLayer<GoogleMapsLayer>("Google Maps");
            if (gooleMapsLayer != null)
            {
                gooleMapsLayer.MapsType = (GoogleMapsType)MapTypeComboBox.SelectedItem;
                Map1.Refresh(RefreshType.Redraw);
            }
        }
    }
}
