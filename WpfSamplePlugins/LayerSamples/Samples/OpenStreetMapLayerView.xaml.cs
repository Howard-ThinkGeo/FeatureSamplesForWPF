using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class OpenStreetMapLayerView : UserControl
    {
        public OpenStreetMapLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;

            OpenStreetMapLayer openStreetMapLayer = new OpenStreetMapLayer();

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(openStreetMapLayer);
            Map1.Overlays.Add(layerOverlay);

            // We can also use the code below to add a google maps to our map.
            // Map1.UseOpenStreetMapAsBaseMap();

            Map1.ZoomToFullBound();
        }
    }
}
