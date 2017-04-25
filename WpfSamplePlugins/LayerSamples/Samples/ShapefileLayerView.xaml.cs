using SlimGis.MapKit.Geometries;
using SlimGis.MapKit.Layers;
using SlimGis.MapKit.Symbologies;
using SlimGis.MapKit.Wpf;
using System.Windows;
using System.Windows.Controls;

namespace SlimGis.Samples
{
    public partial class ShapefileLayerView : UserControl
    {
        public ShapefileLayerView()
        {
            InitializeComponent();
        }

        private void Map1_Loaded(object sender, RoutedEventArgs e)
        {
            Map1.MapUnit = GeoUnit.Meter;
            Map1.UseOpenStreetMapAsBaseMap();

            ShapefileLayer shapefileLayer = new ShapefileLayer("SampleData/countries-900913.shp");
            shapefileLayer.Styles.Add(new FillStyle(GeoColor.FromHtml("#55FAB04D"), GeoColors.White));

            LayerOverlay layerOverlay = new LayerOverlay();
            layerOverlay.Layers.Add(shapefileLayer);
            Map1.Overlays.Add(layerOverlay);

            Map1.ZoomToFullBound();
        }
    }
}
