using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Utilities;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class GeoImageLayerView : UserControl
    {
        public GeoImageLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;

            GeoImage image = new GeoImage(@"SampleData/bing-aerial-900913.png");
            GeoSize size = image.GetSize();
            GeoImageLayer geoImageLayer = new GeoImageLayer(image, new WorldFile(GeoCommonHelper.GetMaxBound(GeoUnit.Meter), (float)size.Width, (float)size.Height));

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(geoImageLayer);
            Map1.Overlays.Add(layerOverlay);
            Map1.ZoomToFullBound();
        }
    }
}
