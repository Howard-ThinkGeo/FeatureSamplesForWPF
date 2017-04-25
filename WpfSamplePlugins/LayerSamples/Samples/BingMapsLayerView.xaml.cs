using SlimGis.MapKit.Controls;
using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class BingMapsLayerView : UserControl
    {
        public BingMapsLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;

            BingMapsLayer bingMapsLayer = new BingMapsLayer(BingMapsType.Road, "AhDBjJalvtRXvYe6BsKuj2DwHT9Atlkas7HNVU7rDvoUvIu0_L_GVtSc8y9gNP61");
            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(bingMapsLayer);
            Map1.Overlays.Add(layerOverlay);

            // We can also use the code below to add a google maps to our map.
            // Map1.UseBingMapsAsBaseMap(BingMapsType.Road, "Your BingMaps Key");

            Map1.ZoomToFullBound();
        }

        private void MapTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            BingMapsLayer bingMapsLayer = Map1.FindLayer<BingMapsLayer>("Bing Maps");
            if (bingMapsLayer != null)
            {
                bingMapsLayer.MapsType = (BingMapsType)MapTypeComboBox.SelectedItem;
                Map1.Refresh(RefreshType.Redraw);
            }
        }
    }
}
