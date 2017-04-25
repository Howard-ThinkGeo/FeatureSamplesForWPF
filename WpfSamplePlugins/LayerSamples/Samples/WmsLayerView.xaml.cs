using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    /// <summary>
    /// Interaction logic for WmsLayerView.xaml
    /// </summary>
    public partial class WmsLayerView : UserControl
    {
        public WmsLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            WmsLayer wmsLayer = new WmsLayer("https://ahocevar.com/geoserver/wms");
            wmsLayer.Name = "Wms";
            wmsLayer.Crs = "EPSG:900913";
            wmsLayer.Layers.Add("topp:states");
            wmsLayer.Parameters.Add("TILED", "TRUE");

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(wmsLayer);
            Map1.Overlays.Add(layerOverlay);

            Map1.ZoomToFullBound();
        }
    }
}
